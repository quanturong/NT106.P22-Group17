

#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY
    using UnityEngine;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class Player
    {
        protected internal Room RoomReference { get; set; }
        private int actorNumber = -1;
        public int ActorNumber
        {
            get { return this.actorNumber; }
        }
        public readonly bool IsLocal;


        public bool HasRejoined
        {
            get; internal set;
        }
		private string nickName = string.Empty;
        public string NickName
        {
            get
            {
                return this.nickName;
            }
            set
            {
                if (!string.IsNullOrEmpty(this.nickName) && this.nickName.Equals(value))
                {
                    return;
                }

                this.nickName = value;
                if (this.IsLocal)
                {
                    this.SetPlayerNameProperty();
                }
            }
        }
        public string UserId { get; internal set; }
        public bool IsMasterClient
        {
            get
            {
                if (this.RoomReference == null)
                {
                    return false;
                }

                return this.ActorNumber == this.RoomReference.MasterClientId;
            }
        }
        public bool IsInactive { get; protected internal set; }
        public Hashtable CustomProperties { get; set; }
        public object TagObject;
        protected internal Player(string nickName, int actorNumber, bool isLocal) : this(nickName, actorNumber, isLocal, null)
        {
        }
        protected internal Player(string nickName, int actorNumber, bool isLocal, Hashtable playerProperties)
        {
            this.IsLocal = isLocal;
            this.actorNumber = actorNumber;
            this.NickName = nickName;

            this.CustomProperties = new Hashtable();
            this.InternalCacheProperties(playerProperties);
        }
        public Player Get(int id)
        {
            if (this.RoomReference == null)
            {
                return null;
            }

            return this.RoomReference.GetPlayer(id);
        }
        public Player GetNext()
        {
            return GetNextFor(this.ActorNumber);
        }
        public Player GetNextFor(Player currentPlayer)
        {
            if (currentPlayer == null)
            {
                return null;
            }
            return GetNextFor(currentPlayer.ActorNumber);
        }
        public Player GetNextFor(int currentPlayerId)
        {
            if (this.RoomReference == null || this.RoomReference.Players == null || this.RoomReference.Players.Count < 2)
            {
                return null;
            }

            Dictionary<int, Player> players = this.RoomReference.Players;
            int nextHigherId = int.MaxValue;    // we look for the next higher ID
            int lowestId = currentPlayerId;     // if we are the player with the highest ID, there is no higher and we return to the lowest player's id

            foreach (int playerid in players.Keys)
            {
                if (playerid < lowestId)
                {
                    lowestId = playerid;        // less than any other ID (which must be at least less than this player's id).
                }
                else if (playerid > currentPlayerId && playerid < nextHigherId)
                {
                    nextHigherId = playerid;    // more than our ID and less than those found so far.
                }
            }
            return (nextHigherId != int.MaxValue) ? players[nextHigherId] : players[lowestId];
        }
        protected internal virtual void InternalCacheProperties(Hashtable properties)
        {
            if (properties == null || properties.Count == 0 || this.CustomProperties.Equals(properties))
            {
                return;
            }
            if (!this.IsLocal && properties.ContainsKey(ActorProperties.PlayerName))
            {
                string nameInServersProperties = (string)properties[ActorProperties.PlayerName];
                this.NickName = nameInServersProperties;
            }

            if (properties.ContainsKey(ActorProperties.UserId))
            {
                this.UserId = (string)properties[ActorProperties.UserId];
            }
            if (properties.ContainsKey(ActorProperties.IsInactive))
            {
                this.IsInactive = (bool)properties[ActorProperties.IsInactive]; //TURNBASED new well-known property for players
            }

            this.CustomProperties.MergeStringKeys(properties);
            this.CustomProperties.StripKeysWithNullValues();
        }
        public override string ToString()
        {
            return string.Format("#{0:00} '{1}'",this.ActorNumber, this.NickName);
        }
        public string ToStringFull()
        {
            return string.Format("#{0:00} '{1}'{2} {3}", this.ActorNumber, this.NickName, this.IsInactive ? " (inactive)" : "", this.CustomProperties.ToStringFull());
        }
        public override bool Equals(object p)
        {
            Player pp = p as Player;
            return (pp != null && this.GetHashCode() == pp.GetHashCode());
        }
        public override int GetHashCode()
        {
            return this.ActorNumber;
        }
        protected internal void ChangeLocalID(int newID)
        {
            if (!this.IsLocal)
            {
                return;
            }

            this.actorNumber = newID;
        }
        public bool SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, WebFlags webFlags = null)
        {
            if (propertiesToSet == null || propertiesToSet.Count == 0)
            {
                return false;
            }

            Hashtable customProps = propertiesToSet.StripToStringKeys() as Hashtable;

            if (this.RoomReference != null)
            {
                if (this.RoomReference.IsOffline)
                {
                    if (customProps.Count == 0)
                    {
                        return false;
                    }
                    this.CustomProperties.Merge(customProps);
                    this.CustomProperties.StripKeysWithNullValues();
                    this.RoomReference.LoadBalancingClient.InRoomCallbackTargets.OnPlayerPropertiesUpdate(this, customProps);
                    return true;
                }
                else
                {
                    Hashtable customPropsToCheck = expectedValues.StripToStringKeys() as Hashtable;
                    return this.RoomReference.LoadBalancingClient.OpSetPropertiesOfActor(this.actorNumber, customProps, customPropsToCheck, webFlags);
                }
            }
            if (this.IsLocal)
            {
                if (customProps.Count == 0)
                {
                    return false;
                }
                if (expectedValues == null && webFlags == null)
                {
                    this.CustomProperties.Merge(customProps);
                    this.CustomProperties.StripKeysWithNullValues();
                    return true;
                }
            }

            return false;
        }
        internal bool UpdateNickNameOnJoined()
        {
            if (this.RoomReference == null || this.RoomReference.CustomProperties == null || !this.IsLocal)
            {
                return false;
            }

            bool found = this.RoomReference.CustomProperties.ContainsKey(ActorProperties.PlayerName);
            string nickFromProps = found ? this.RoomReference.CustomProperties[ActorProperties.PlayerName] as string : string.Empty;

            if (!string.Equals(this.NickName, nickFromProps))
            {
                return this.SetPlayerNameProperty();
            }

            return true;
        }
        private bool SetPlayerNameProperty()
        {
            if (this.RoomReference != null && !this.RoomReference.IsOffline)
            {
                Hashtable properties = new Hashtable();
                properties[ActorProperties.PlayerName] = this.nickName;
                return this.RoomReference.LoadBalancingClient.OpSetPropertiesOfActor(this.ActorNumber, properties);
            }

            return false;
        }
    }
}