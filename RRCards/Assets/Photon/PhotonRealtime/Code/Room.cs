
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class Room : RoomInfo
    {
        public LoadBalancingClient LoadBalancingClient { get; set; }
        public new string Name
        {
            get
            {
                return this.name;
            }

            internal set
            {
                this.name = value;
            }
        }

        private bool isOffline;

        public bool IsOffline
        {
            get
            {
                return isOffline;
            }

            private set
            {
                isOffline = value;
            }
        }
        public new bool IsOpen
        {
            get
            {
                return this.isOpen;
            }

            set
            {
                if (value != this.isOpen)
                {
                    if (!this.isOffline)
                    {
                        this.LoadBalancingClient.OpSetPropertiesOfRoom(new Hashtable() { { GamePropertyKey.IsOpen, value } });
                    }
                }

                this.isOpen = value;
            }
        }
        public new bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                if (value != this.isVisible)
                {
                    if (!this.isOffline)
                    {
                        this.LoadBalancingClient.OpSetPropertiesOfRoom(new Hashtable() { { GamePropertyKey.IsVisible, value } });
                    }
                }

                this.isVisible = value;
            }
        }
        public new int MaxPlayers
        {
            get
            {
                return this.maxPlayers;
            }

            set
            {
                if (value >= 0 && value != this.maxPlayers)
                {
                    this.maxPlayers = value;
                    byte maxPlayersAsByte = value <= byte.MaxValue ? (byte)value : (byte)0;
                    if (!this.isOffline)
                    {
                        this.LoadBalancingClient.OpSetPropertiesOfRoom(new Hashtable() { { GamePropertyKey.MaxPlayers, maxPlayersAsByte }, { GamePropertyKey.MaxPlayersInt, this.maxPlayers } });
                    }
                }
            }
        }
        public new int PlayerCount
        {
            get
            {
                if (this.Players == null)
                {
                    return 0;
                }

                return (byte)this.Players.Count;
            }
        }
        private Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Dictionary<int, Player> Players
        {
            get
            {
                return this.players;
            }

            private set
            {
                this.players = value;
            }
        }
        public string[] ExpectedUsers
        {
            get { return this.expectedUsers; }
        }
        public int PlayerTtl
        {
            get { return this.playerTtl; }

            set
            {
                if (value != this.playerTtl)
                {
                    if (!this.isOffline)
                    {
                        this.LoadBalancingClient.OpSetPropertyOfRoom(GamePropertyKey.PlayerTtl, value);  // TODO: implement Offline Mode
                    }
                }

                this.playerTtl = value;
            }
        }
        public int EmptyRoomTtl
        {
            get { return this.emptyRoomTtl; }

            set
            {
                if (value != this.emptyRoomTtl)
                {
                    if (!this.isOffline)
                    {
                        this.LoadBalancingClient.OpSetPropertyOfRoom(GamePropertyKey.EmptyRoomTtl, value);  // TODO: implement Offline Mode
                    }
                }

                this.emptyRoomTtl = value;
            }
        }
        public int MasterClientId { get { return this.masterClientId; } }
        public string[] PropertiesListedInLobby
        {
            get
            {
                return this.propertiesListedInLobby;
            }

            private set
            {
                this.propertiesListedInLobby = value;
            }
        }
        public bool AutoCleanUp
        {
            get
            {
                return this.autoCleanUp;
            }
        }
        public bool BroadcastPropertiesChangeToAll { get; private set; }
        public bool SuppressRoomEvents { get; private set; }
        public bool SuppressPlayerInfo { get; private set; }
        public bool PublishUserId { get; private set; }
        public bool DeleteNullProperties { get; private set; }

        #if SERVERSDK
        public bool CheckUserOnJoin { get; private set; }
        #endif
        public Room(string roomName, RoomOptions options, bool isOffline = false) : base(roomName, options != null ? options.CustomRoomProperties : null)
        {
            if (options != null)
            {
                this.isVisible = options.IsVisible;
                this.isOpen = options.IsOpen;
                this.maxPlayers = options.MaxPlayers;
                this.propertiesListedInLobby = options.CustomRoomPropertiesForLobby;
            }

            this.isOffline = isOffline;
        }
        internal void InternalCacheRoomFlags(int roomFlags)
        {
            this.BroadcastPropertiesChangeToAll = (roomFlags & (int)RoomOptionBit.BroadcastPropsChangeToAll) != 0;
            this.SuppressRoomEvents = (roomFlags & (int)RoomOptionBit.SuppressRoomEvents) != 0;
            this.SuppressPlayerInfo = (roomFlags & (int)RoomOptionBit.SuppressPlayerInfo) != 0;
            this.PublishUserId = (roomFlags & (int)RoomOptionBit.PublishUserId) != 0;
            this.DeleteNullProperties = (roomFlags & (int)RoomOptionBit.DeleteNullProps) != 0;
            #if SERVERSDK
            this.CheckUserOnJoin = (roomFlags & (int)RoomOptionBit.CheckUserOnJoin) != 0;
            #endif
            this.autoCleanUp = (roomFlags & (int)RoomOptionBit.DeleteCacheOnLeave) != 0;
        }

        protected internal override void InternalCacheProperties(Hashtable propertiesToCache)
        {
            int oldMasterId = this.masterClientId;

            base.InternalCacheProperties(propertiesToCache);    // important: updating the properties fields has no way to do callbacks on change

            if (oldMasterId != 0 && this.masterClientId != oldMasterId)
            {
                this.LoadBalancingClient.InRoomCallbackTargets.OnMasterClientSwitched(this.GetPlayer(this.masterClientId));
            }
        }
        public virtual bool SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedProperties = null, WebFlags webFlags = null)
        {
            if (propertiesToSet == null || propertiesToSet.Count == 0)
            {
                return false;
            }
            Hashtable customProps = propertiesToSet.StripToStringKeys() as Hashtable;

            if (this.isOffline)
            {
                if (customProps.Count == 0)
                {
                    return false;
                }
                this.CustomProperties.Merge(customProps);
                this.CustomProperties.StripKeysWithNullValues();
                this.LoadBalancingClient.InRoomCallbackTargets.OnRoomPropertiesUpdate(propertiesToSet);

            }
            else
            {
                return this.LoadBalancingClient.OpSetPropertiesOfRoom(customProps, expectedProperties, webFlags);
            }

            return true;
        }
        public bool SetPropertiesListedInLobby(string[] lobbyProps)
        {
            if (this.isOffline)
            {
                return false;
            }
            Hashtable customProps = new Hashtable();
            customProps[GamePropertyKey.PropsListedInLobby] = lobbyProps;
            return this.LoadBalancingClient.OpSetPropertiesOfRoom(customProps);
        }
        protected internal virtual void RemovePlayer(Player player)
        {
            this.Players.Remove(player.ActorNumber);
            player.RoomReference = null;
        }
        protected internal virtual void RemovePlayer(int id)
        {
            this.RemovePlayer(this.GetPlayer(id));
        }
        public bool SetMasterClient(Player masterClientPlayer)
        {
            if (this.isOffline)
            {
                return false;
            }
            Hashtable newProps = new Hashtable() { { GamePropertyKey.MasterClientId, masterClientPlayer.ActorNumber } };
            Hashtable prevProps = new Hashtable() { { GamePropertyKey.MasterClientId, this.MasterClientId } };
            return this.LoadBalancingClient.OpSetPropertiesOfRoom(newProps, prevProps);
        }
        public virtual bool AddPlayer(Player player)
        {
            if (!this.Players.ContainsKey(player.ActorNumber))
            {
                this.StorePlayer(player);
                return true;
            }

            return false;
        }
        public virtual Player StorePlayer(Player player)
        {
            this.Players[player.ActorNumber] = player;
            player.RoomReference = this;

            return player;
        }
        public virtual Player GetPlayer(int id, bool findMaster = false)
        {
            int idToFind = (findMaster && id == 0) ? this.MasterClientId : id;

            Player result = null;
            this.Players.TryGetValue(idToFind, out result);

            return result;
        }
        public bool ClearExpectedUsers()
        {
            if (this.ExpectedUsers == null || this.ExpectedUsers.Length == 0)
            {
                return false;
            }
            return this.SetExpectedUsers(new string[0], this.ExpectedUsers);
        }
        public bool SetExpectedUsers(string[] newExpectedUsers)
        {
            if (newExpectedUsers == null || newExpectedUsers.Length == 0)
            {
                this.LoadBalancingClient.DebugReturn(DebugLevel.ERROR, "newExpectedUsers array is null or empty, call Room.ClearExpectedUsers() instead if this is what you want.");
                return false;
            }
            return this.SetExpectedUsers(newExpectedUsers, this.ExpectedUsers);
        }

        private bool SetExpectedUsers(string[] newExpectedUsers, string[] oldExpectedUsers)
        {
            if (this.isOffline)
            {
                return false;
            }
            Hashtable gameProperties = new Hashtable(1);
            gameProperties.Add(GamePropertyKey.ExpectedUsers, newExpectedUsers);
            Hashtable expectedProperties = null;
            if (oldExpectedUsers != null)
            {
                expectedProperties = new Hashtable(1);
                expectedProperties.Add(GamePropertyKey.ExpectedUsers, oldExpectedUsers);
            }
            return this.LoadBalancingClient.OpSetPropertiesOfRoom(gameProperties, expectedProperties);
        }
        public override string ToString()
        {
            return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", this.name, this.isVisible ? "visible" : "hidden", this.isOpen ? "open" : "closed", this.maxPlayers, this.PlayerCount);
        }
        public new string ToStringFull()
        {
            return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", this.name, this.isVisible ? "visible" : "hidden", this.isOpen ? "open" : "closed", this.maxPlayers, this.PlayerCount, this.CustomProperties.ToStringFull());
        }
    }
}