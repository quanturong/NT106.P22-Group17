

#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class RoomInfo
    {
        public bool RemovedFromList;
        private Hashtable customProperties = new Hashtable();
        protected int maxPlayers = 0;
        protected int emptyRoomTtl = 0;
        protected int playerTtl = 0;
        protected string[] expectedUsers;
        protected bool isOpen = true;
        protected bool isVisible = true;
        protected bool autoCleanUp = true;
        protected string name;
        public int masterClientId;
        protected string[] propertiesListedInLobby;
        public Hashtable CustomProperties
        {
            get
            {
                return this.customProperties;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public int PlayerCount { get; private set; }
        public int MaxPlayers
        {
            get
            {
                return this.maxPlayers;
            }
        }
        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }
        }
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
        }
        protected internal RoomInfo(string roomName, Hashtable roomProperties)
        {
            this.InternalCacheProperties(roomProperties);

            this.name = roomName;
        }
        public override bool Equals(object other)
        {
            RoomInfo otherRoomInfo = other as RoomInfo;
            return (otherRoomInfo != null && this.Name.Equals(otherRoomInfo.name));
        }
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", this.name, this.isVisible ? "visible" : "hidden", this.isOpen ? "open" : "closed", this.maxPlayers, this.PlayerCount);
        }
        public string ToStringFull()
        {
            return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", this.name, this.isVisible ? "visible" : "hidden", this.isOpen ? "open" : "closed", this.maxPlayers, this.PlayerCount, this.customProperties.ToStringFull());
        }
        protected internal virtual void InternalCacheProperties(Hashtable propertiesToCache)
        {
            if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customProperties.Equals(propertiesToCache))
            {
                return;
            }
            if (propertiesToCache.ContainsKey(GamePropertyKey.Removed))
            {
                this.RemovedFromList = (bool)propertiesToCache[GamePropertyKey.Removed];
                if (this.RemovedFromList)
                {
                    return;
                }
            }
            if (propertiesToCache.ContainsKey(GamePropertyKey.MaxPlayersInt))
            {
                this.maxPlayers = Convert.ToInt32(propertiesToCache[GamePropertyKey.MaxPlayersInt]);
            }
            else if (propertiesToCache.ContainsKey(GamePropertyKey.MaxPlayers))
            {
                this.maxPlayers = Convert.ToInt32(propertiesToCache[GamePropertyKey.MaxPlayers]);
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.IsOpen))
            {
                this.isOpen = (bool)propertiesToCache[GamePropertyKey.IsOpen];
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.IsVisible))
            {
                this.isVisible = (bool)propertiesToCache[GamePropertyKey.IsVisible];
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.PlayerCount))
            {
                this.PlayerCount = Convert.ToInt32(propertiesToCache[GamePropertyKey.PlayerCount]);
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.CleanupCacheOnLeave))
            {
                this.autoCleanUp = (bool)propertiesToCache[GamePropertyKey.CleanupCacheOnLeave];
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.MasterClientId))
            {
                this.masterClientId = (int)propertiesToCache[GamePropertyKey.MasterClientId];
            }

            if (propertiesToCache.ContainsKey(GamePropertyKey.PropsListedInLobby))
            {
                this.propertiesListedInLobby = propertiesToCache[GamePropertyKey.PropsListedInLobby] as string[];
            }

            if (propertiesToCache.ContainsKey((byte)GamePropertyKey.ExpectedUsers))
            {
                this.expectedUsers = (string[])propertiesToCache[GamePropertyKey.ExpectedUsers];
            }

            if (propertiesToCache.ContainsKey((byte)GamePropertyKey.EmptyRoomTtl))
            {
                this.emptyRoomTtl = (int)propertiesToCache[GamePropertyKey.EmptyRoomTtl];
            }

            if (propertiesToCache.ContainsKey((byte)GamePropertyKey.PlayerTtl))
            {
                this.playerTtl = (int)propertiesToCache[GamePropertyKey.PlayerTtl];
            }
            this.customProperties.MergeStringKeys(propertiesToCache);
            this.customProperties.StripKeysWithNullValues();
        }
    }
}
