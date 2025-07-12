
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
    using Debug = UnityEngine.Debug;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class LoadBalancingPeer : PhotonPeer
    {
        [Obsolete("Use RegionHandler.PingImplementation directly.")]
        protected internal static Type PingImplementation
        {
            get { return RegionHandler.PingImplementation; }
            set { RegionHandler.PingImplementation = value; }
        }


        private readonly Pool<ParameterDictionary> paramDictionaryPool = new Pool<ParameterDictionary>(
            () => new ParameterDictionary(),
            x => x.Clear(),
            1); // used in OpRaiseEvent() (avoids lots of new Dictionary() calls)
        public LoadBalancingPeer(ConnectionProtocol protocolType) : base(protocolType)
        {

            this.ConfigUnitySockets();
        }
        public LoadBalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType) : this(protocolType)
        {
            this.Listener = listener;
        }
        [System.Diagnostics.Conditional("SUPPORTED_UNITY")]
        private void ConfigUnitySockets()
        {
            Type websocketType = null;
            #if (UNITY_XBOXONE || UNITY_GAMECORE) && !UNITY_EDITOR
            websocketType = Type.GetType("ExitGames.Client.Photon.SocketNativeSource, Assembly-CSharp", false);
            if (websocketType == null)
            {
                websocketType = Type.GetType("ExitGames.Client.Photon.SocketNativeSource, Assembly-CSharp-firstpass", false);
            }
            if (websocketType == null)
            {
                websocketType = Type.GetType("ExitGames.Client.Photon.SocketNativeSource, PhotonRealtime", false);
            }
            if (websocketType != null)
            {
                this.SocketImplementationConfig[ConnectionProtocol.Udp] = websocketType;    // on Xbox, the native socket plugin supports UDP as well
            }
            #else
            websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, PhotonWebSocket", false);
            if (websocketType == null)
            {
                websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp-firstpass", false);
            }
            if (websocketType == null)
            {
                websocketType = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp", false);
            }
            #if UNITY_WEBGL
            if (websocketType == null && this.DebugOut >= DebugLevel.WARNING)
            {
                this.Listener.DebugReturn(DebugLevel.WARNING, "SocketWebTcp type not found in the usual Assemblies. This is required as wrapper for the browser WebSocket API. Make sure to make the PhotonLibs\\WebSocket code available.");
            }
            #endif
            #endif

            if (websocketType != null)
            {
                this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = websocketType;
                this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = websocketType;
            }
        }


        public virtual bool OpGetRegions(string appId)
        {
            Dictionary<byte, object> parameters = new Dictionary<byte, object>(1);
            parameters[(byte)ParameterCode.ApplicationId] = appId;

            return this.SendOperation(OperationCode.GetRegions, parameters, new SendOptions() { Reliability = true, Encrypt = true });
        }
        public virtual bool OpJoinLobby(TypedLobby lobby = null)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpJoinLobby()");
            }

            Dictionary<byte, object> parameters = null;
            if (lobby != null && !lobby.IsDefault)
            {
                parameters = new Dictionary<byte, object>();
                parameters[(byte)ParameterCode.LobbyName] = lobby.Name;
                parameters[(byte)ParameterCode.LobbyType] = (byte)lobby.Type;
            }

            return this.SendOperation(OperationCode.JoinLobby, parameters, SendOptions.SendReliable);
        }
        public virtual bool OpLeaveLobby()
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpLeaveLobby()");
            }

            return this.SendOperation(OperationCode.LeaveLobby, (Dictionary<byte, object>)null, SendOptions.SendReliable);
        }
        private void RoomOptionsToOpParameters(Dictionary<byte, object> op, RoomOptions roomOptions, bool usePropertiesKey = false)
        {
            if (roomOptions == null)
            {
                roomOptions = new RoomOptions();
            }

            Hashtable gameProperties = new Hashtable();
            gameProperties[GamePropertyKey.IsOpen] = roomOptions.IsOpen;
            gameProperties[GamePropertyKey.IsVisible] = roomOptions.IsVisible;
            gameProperties[GamePropertyKey.PropsListedInLobby] = (roomOptions.CustomRoomPropertiesForLobby == null) ? new string[0] : roomOptions.CustomRoomPropertiesForLobby;
            gameProperties.MergeStringKeys(roomOptions.CustomRoomProperties);


            if (roomOptions.MaxPlayers > 0)
            {
                byte maxPlayersAsByte = roomOptions.MaxPlayers <= byte.MaxValue ? (byte)roomOptions.MaxPlayers : (byte)0;

                gameProperties[GamePropertyKey.MaxPlayers] = maxPlayersAsByte;
                gameProperties[GamePropertyKey.MaxPlayersInt] = roomOptions.MaxPlayers;
            }

            if (!usePropertiesKey)
            {
                op[ParameterCode.GameProperties] = gameProperties;  // typically, the key for game props is 248
            }
            else
            {
                op[ParameterCode.Properties] = gameProperties;      // when an op uses 248 as filter, the "create room" props can be set as 251
            }


            int flags = 0;  // a new way to send the room options as bitwise-flags

            if (roomOptions.CleanupCacheOnLeave)
            {
                op[ParameterCode.CleanupCacheOnLeave] = true;	                // this defines the server's room settings and logic
                flags = flags | (int)RoomOptionBit.DeleteCacheOnLeave;          // this defines the server's room settings and logic (for servers that support flags)
            }
            else
            {
                op[ParameterCode.CleanupCacheOnLeave] = false;	                // this defines the server's room settings and logic
                gameProperties[GamePropertyKey.CleanupCacheOnLeave] = false;    // this is only informational for the clients which join
            }

            #if SERVERSDK
            op[ParameterCode.CheckUserOnJoin] = roomOptions.CheckUserOnJoin;
            if (roomOptions.CheckUserOnJoin)
            {
                flags = flags | (int) RoomOptionBit.CheckUserOnJoin;
            }
            #else
            flags = flags | (int) RoomOptionBit.CheckUserOnJoin;
            op[ParameterCode.CheckUserOnJoin] = true;
            #endif

            if (roomOptions.PlayerTtl > 0 || roomOptions.PlayerTtl == -1)
            {
                op[ParameterCode.PlayerTTL] = roomOptions.PlayerTtl;    // TURNBASED
            }

            if (roomOptions.EmptyRoomTtl > 0)
            {
                op[ParameterCode.EmptyRoomTTL] = roomOptions.EmptyRoomTtl;   //TURNBASED
            }

            if (roomOptions.SuppressRoomEvents)
            {
                flags = flags | (int)RoomOptionBit.SuppressRoomEvents;
                op[ParameterCode.SuppressRoomEvents] = true;
            }
            if (roomOptions.SuppressPlayerInfo)
            {
                flags = flags | (int)RoomOptionBit.SuppressPlayerInfo;
            }

            if (roomOptions.Plugins != null)
            {
                op[ParameterCode.Plugins] = roomOptions.Plugins;
            }
            if (roomOptions.PublishUserId)
            {
                flags = flags | (int)RoomOptionBit.PublishUserId;
                op[ParameterCode.PublishUserId] = true;
            }
            if (roomOptions.DeleteNullProperties)
            {
                flags = flags | (int)RoomOptionBit.DeleteNullProps; // this is only settable as flag
            }
            if (roomOptions.BroadcastPropsChangeToAll)
            {
                flags = flags | (int)RoomOptionBit.BroadcastPropsChangeToAll; // this is only settable as flag
            }

            op[ParameterCode.RoomOptionFlags] = flags;
        }
        public virtual bool OpCreateRoom(EnterRoomParams opParams)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpCreateRoom()");
            }

            Dictionary<byte, object> op = new Dictionary<byte, object>();
            SendOptions sendOptions = new SendOptions() { Reliability = true };

            if (!string.IsNullOrEmpty(opParams.RoomName))
            {
                op[ParameterCode.RoomName] = opParams.RoomName;
            }
            if (opParams.Lobby != null && !opParams.Lobby.IsDefault)
            {
                op[ParameterCode.LobbyName] = opParams.Lobby.Name;
                op[ParameterCode.LobbyType] = (byte)opParams.Lobby.Type;
            }

            if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
            {
                op[ParameterCode.Add] = opParams.ExpectedUsers;
                sendOptions.Encrypt = true;
            }
            if (opParams.Ticket != null)
            {
                op[ParameterCode.Ticket] = opParams.Ticket;
            }

            if (opParams.OnGameServer)
            {
                if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
                {
                    op[ParameterCode.PlayerProperties] = opParams.PlayerProperties;
                }
                op[ParameterCode.Broadcast] = true; // broadcast actor properties

                this.RoomOptionsToOpParameters(op, opParams.RoomOptions);
            }
            return this.SendOperation(OperationCode.CreateGame, op, sendOptions);
        }
        public virtual bool OpJoinRoom(EnterRoomParams opParams)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRoom()");
            }
            Dictionary<byte, object> op = new Dictionary<byte, object>();
            SendOptions sendOptions = new SendOptions() { Reliability = true };

            if (!string.IsNullOrEmpty(opParams.RoomName))
            {
                op[ParameterCode.RoomName] = opParams.RoomName;
            }

            if (opParams.JoinMode == JoinMode.CreateIfNotExists)
            {
                op[ParameterCode.JoinMode] = (byte)JoinMode.CreateIfNotExists;
                if (opParams.Lobby != null && !opParams.Lobby.IsDefault)
                {
                    op[ParameterCode.LobbyName] = opParams.Lobby.Name;
                    op[ParameterCode.LobbyType] = (byte)opParams.Lobby.Type;
                }
            }
            else if (opParams.JoinMode == JoinMode.RejoinOnly)
            {
                op[ParameterCode.JoinMode] = (byte)JoinMode.RejoinOnly; // changed from JoinMode.JoinOrRejoin
            }

            if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
            {
                op[ParameterCode.Add] = opParams.ExpectedUsers;
                sendOptions.Encrypt = true;
            }
            if (opParams.Ticket != null)
            {
                op[ParameterCode.Ticket] = opParams.Ticket;
            }

            if (opParams.OnGameServer)
            {
                if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
                {
                    op[ParameterCode.PlayerProperties] = opParams.PlayerProperties;
                }
                op[ParameterCode.Broadcast] = true; // broadcast actor properties

                this.RoomOptionsToOpParameters(op, opParams.RoomOptions);
            }
            return this.SendOperation(OperationCode.JoinGame, op, sendOptions);
        }
        public virtual bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomRoom()");
            }

            Hashtable expectedRoomProperties = new Hashtable();
            expectedRoomProperties.MergeStringKeys(opJoinRandomRoomParams.ExpectedCustomRoomProperties);

            if (opJoinRandomRoomParams.ExpectedMaxPlayers > 0)
            {
                byte maxPlayersAsByte = opJoinRandomRoomParams.ExpectedMaxPlayers <= byte.MaxValue ? (byte)opJoinRandomRoomParams.ExpectedMaxPlayers : (byte)0;

                expectedRoomProperties[GamePropertyKey.MaxPlayers] = maxPlayersAsByte;
                if (opJoinRandomRoomParams.ExpectedMaxPlayers > byte.MaxValue)
                {
                    expectedRoomProperties[GamePropertyKey.MaxPlayersInt] = opJoinRandomRoomParams.ExpectedMaxPlayers;
                }
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            SendOptions sendOptions = new SendOptions() { Reliability = true };
            if (expectedRoomProperties.Count > 0)
            {
                opParameters[ParameterCode.GameProperties] = expectedRoomProperties;
            }

            if (opJoinRandomRoomParams.MatchingType != MatchmakingMode.FillRoom)
            {
                opParameters[ParameterCode.MatchMakingType] = (byte)opJoinRandomRoomParams.MatchingType;
            }

            if (opJoinRandomRoomParams.TypedLobby != null && !opJoinRandomRoomParams.TypedLobby.IsDefault)
            {
                opParameters[ParameterCode.LobbyName] = opJoinRandomRoomParams.TypedLobby.Name;
                opParameters[ParameterCode.LobbyType] = (byte)opJoinRandomRoomParams.TypedLobby.Type;
            }

            if (!string.IsNullOrEmpty(opJoinRandomRoomParams.SqlLobbyFilter))
            {
                opParameters[ParameterCode.Data] = opJoinRandomRoomParams.SqlLobbyFilter;
            }

            if (opJoinRandomRoomParams.ExpectedUsers != null && opJoinRandomRoomParams.ExpectedUsers.Length > 0)
            {
                opParameters[ParameterCode.Add] = opJoinRandomRoomParams.ExpectedUsers;
                sendOptions.Encrypt = true;
            }
            if (opJoinRandomRoomParams.Ticket != null)
            {
                opParameters[ParameterCode.Ticket] = opJoinRandomRoomParams.Ticket;
            }

            opParameters[ParameterCode.AllowRepeats] = true; // enables temporary queueing for low ccu matchmaking situations
            return this.SendOperation(OperationCode.JoinRandomGame, opParameters, sendOptions);
        }
        public virtual bool OpJoinRandomOrCreateRoom(OpJoinRandomRoomParams opJoinRandomRoomParams, EnterRoomParams createRoomParams)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomOrCreateRoom()");
            }

            Hashtable expectedRoomProperties = new Hashtable();
            expectedRoomProperties.MergeStringKeys(opJoinRandomRoomParams.ExpectedCustomRoomProperties);
            if (opJoinRandomRoomParams.ExpectedMaxPlayers > 0)
            {
                byte maxPlayersAsByte = opJoinRandomRoomParams.ExpectedMaxPlayers <= byte.MaxValue ? (byte)opJoinRandomRoomParams.ExpectedMaxPlayers : (byte)0;

                expectedRoomProperties[GamePropertyKey.MaxPlayers] = maxPlayersAsByte;
                if (opJoinRandomRoomParams.ExpectedMaxPlayers > byte.MaxValue)
                {
                    expectedRoomProperties[GamePropertyKey.MaxPlayersInt] = opJoinRandomRoomParams.ExpectedMaxPlayers;
                }
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            SendOptions sendOptions = new SendOptions() { Reliability = true };
            if (expectedRoomProperties.Count > 0)
            {
                opParameters[ParameterCode.GameProperties] = expectedRoomProperties;    // used as filter. below, RoomOptionsToOpParameters has usePropertiesKey = true
            }

            if (opJoinRandomRoomParams.MatchingType != MatchmakingMode.FillRoom)
            {
                opParameters[ParameterCode.MatchMakingType] = (byte)opJoinRandomRoomParams.MatchingType;
            }

            if (opJoinRandomRoomParams.TypedLobby != null && !opJoinRandomRoomParams.TypedLobby.IsDefault)
            {
                opParameters[ParameterCode.LobbyName] = opJoinRandomRoomParams.TypedLobby.Name;
                opParameters[ParameterCode.LobbyType] = (byte)opJoinRandomRoomParams.TypedLobby.Type;
            }

            if (!string.IsNullOrEmpty(opJoinRandomRoomParams.SqlLobbyFilter))
            {
                opParameters[ParameterCode.Data] = opJoinRandomRoomParams.SqlLobbyFilter;
            }

            if (opJoinRandomRoomParams.ExpectedUsers != null && opJoinRandomRoomParams.ExpectedUsers.Length > 0)
            {
                opParameters[ParameterCode.Add] = opJoinRandomRoomParams.ExpectedUsers;
                sendOptions.Encrypt = true;
            }
            if (opJoinRandomRoomParams.Ticket != null)
            {
                opParameters[ParameterCode.Ticket] = opJoinRandomRoomParams.Ticket;
            }

            opParameters[ParameterCode.JoinMode] = (byte)JoinMode.CreateIfNotExists;
            opParameters[ParameterCode.AllowRepeats] = true; // enables temporary queueing for low ccu matchmaking situations

            if (createRoomParams != null)
            {
                if (!string.IsNullOrEmpty(createRoomParams.RoomName))
                {
                    opParameters[ParameterCode.RoomName] = createRoomParams.RoomName;
                }
            }
            return this.SendOperation(OperationCode.JoinRandomGame, opParameters, sendOptions);
        }
        public virtual bool OpLeaveRoom(bool becomeInactive, bool sendAuthCookie = false)
        {
            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            if (becomeInactive)
            {
                opParameters[ParameterCode.IsInactive] = true;
            }
            if (sendAuthCookie)
            {
                opParameters[ParameterCode.EventForward] = WebFlags.SendAuthCookieConst;
            }
            return this.SendOperation(OperationCode.Leave, opParameters, SendOptions.SendReliable);
        }
        public virtual bool OpGetGameList(TypedLobby lobby, string queryData)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList()");
            }

            if (lobby == null)
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. Lobby cannot be null.");
                }
                return false;
            }

            if (lobby.Type != LobbyType.SqlLobby)
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. LobbyType must be SqlLobby.");
                }
                return false;
            }

            if (lobby.IsDefault)
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. LobbyName must be not null and not empty.");
                }
                return false;
            }

            if (string.IsNullOrEmpty(queryData))
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. queryData must be not null and not empty.");
                }
                return false;
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            opParameters[(byte)ParameterCode.LobbyName] = lobby.Name;
            opParameters[(byte)ParameterCode.LobbyType] = (byte)lobby.Type;
            opParameters[(byte)ParameterCode.Data] = queryData;

            return this.SendOperation(OperationCode.GetGameList, opParameters, SendOptions.SendReliable);
        }
        public virtual bool OpFindFriends(string[] friendsToFind, FindFriendsOptions options = null)
        {
            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            if (friendsToFind != null && friendsToFind.Length > 0)
            {
                opParameters[ParameterCode.FindFriendsRequestList] = friendsToFind;
            }

            if (options != null)
            {
                opParameters[ParameterCode.FindFriendsOptions] = options.ToIntFlags();
            }

            SendOptions sendOptions = new SendOptions() { Reliability = true, Encrypt = true };
            return this.SendOperation(OperationCode.FindFriends, opParameters, sendOptions);
        }

        public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable actorProperties)
        {
            return this.OpSetPropertiesOfActor(actorNr, actorProperties.StripToStringKeys(), null);
        }
        protected internal bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, Hashtable expectedProperties = null, WebFlags webflags = null)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor()");
            }

            if (actorNr <= 0 || actorProperties == null || actorProperties.Count == 0)
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties must be not null nor empty.");
                }
                return false;
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            opParameters.Add(ParameterCode.Properties, actorProperties);
            opParameters.Add(ParameterCode.ActorNr, actorNr);
            opParameters.Add(ParameterCode.Broadcast, true);
            if (expectedProperties != null && expectedProperties.Count != 0)
            {
                opParameters.Add(ParameterCode.ExpectedValues, expectedProperties);
            }

            if (webflags != null && webflags.HttpForward)
            {
                opParameters[ParameterCode.EventForward] = webflags.WebhookFlags;
            }

            return this.SendOperation(OperationCode.SetProperties, opParameters, SendOptions.SendReliable);
        }


        protected bool OpSetPropertyOfRoom(byte propCode, object value)
        {
            Hashtable properties = new Hashtable();
            properties[propCode] = value;
            return this.OpSetPropertiesOfRoom(properties);
        }

        public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties)
        {
            return this.OpSetPropertiesOfRoom(gameProperties.StripToStringKeys());
        }
        protected internal bool OpSetPropertiesOfRoom(Hashtable gameProperties, Hashtable expectedProperties = null, WebFlags webflags = null)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom()");
            }
            if (gameProperties == null || gameProperties.Count == 0)
            {
                if (this.DebugOut >= DebugLevel.INFO)
                {
                    this.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom not sent. gameProperties must be not null nor empty.");
                }
                return false;
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            opParameters.Add(ParameterCode.Properties, gameProperties);
            opParameters.Add(ParameterCode.Broadcast, true);
            if (expectedProperties != null && expectedProperties.Count != 0)
            {
                opParameters.Add(ParameterCode.ExpectedValues, expectedProperties);
            }

            if (webflags!=null && webflags.HttpForward)
            {
                opParameters[ParameterCode.EventForward] = webflags.WebhookFlags;
            }

            return this.SendOperation(OperationCode.SetProperties, opParameters, SendOptions.SendReliable);
        }
        public virtual bool OpAuthenticate(string appId, string appVersion, AuthenticationValues authValues, string regionCode, bool getLobbyStatistics)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            if (getLobbyStatistics)
            {
                opParameters[ParameterCode.LobbyStats] = true;
            }
            if (authValues != null && authValues.Token != null)
            {
                opParameters[ParameterCode.Token] = authValues.Token;
                return this.SendOperation(OperationCode.Authenticate, opParameters, SendOptions.SendReliable); // we don't have to encrypt, when we have a token (which is encrypted)
            }

            opParameters[ParameterCode.AppVersion] = appVersion;
            opParameters[ParameterCode.ApplicationId] = appId;

            if (!string.IsNullOrEmpty(regionCode))
            {
                opParameters[ParameterCode.Region] = regionCode;
            }

            if (authValues != null)
            {

                if (!string.IsNullOrEmpty(authValues.UserId))
                {
                    opParameters[ParameterCode.UserId] = authValues.UserId;
                }

                if (authValues.AuthType != CustomAuthenticationType.None)
                {
                    opParameters[ParameterCode.ClientAuthenticationType] = (byte)authValues.AuthType;
                    if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
                    {
                        opParameters[ParameterCode.ClientAuthenticationParams] = authValues.AuthGetParameters;
                    }
                    if (authValues.AuthPostData != null)
                    {
                        opParameters[ParameterCode.ClientAuthenticationData] = authValues.AuthPostData;
                    }
                }
            }

            return this.SendOperation(OperationCode.Authenticate, opParameters, new SendOptions() { Reliability = true, Encrypt = true });
        }
        public virtual bool OpAuthenticateOnce(string appId, string appVersion, AuthenticationValues authValues, string regionCode, EncryptionMode encryptionMode, ConnectionProtocol expectedProtocol)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticateOnce(): authValues = "  + authValues + ", region = " + regionCode + ", encryption = " + encryptionMode);
            }

            var opParameters = new Dictionary<byte, object>();
            if (authValues != null && authValues.Token != null)
            {
                opParameters[ParameterCode.Token] = authValues.Token;
                return this.SendOperation(OperationCode.AuthenticateOnce, opParameters, SendOptions.SendReliable); // we don't have to encrypt, when we have a token (which is encrypted)
            }

            if (encryptionMode == EncryptionMode.DatagramEncryptionGCM && expectedProtocol != ConnectionProtocol.Udp)
            {
                throw new NotSupportedException("Expected protocol set to UDP, due to encryption mode DatagramEncryptionGCM.");
            }

            opParameters[ParameterCode.ExpectedProtocol] = (byte)expectedProtocol;
            opParameters[ParameterCode.EncryptionMode] = (byte)encryptionMode;

            opParameters[ParameterCode.AppVersion] = appVersion;
            opParameters[ParameterCode.ApplicationId] = appId;

            if (!string.IsNullOrEmpty(regionCode))
            {
                opParameters[ParameterCode.Region] = regionCode;
            }

            if (authValues != null)
            {
                if (!string.IsNullOrEmpty(authValues.UserId))
                {
                    opParameters[ParameterCode.UserId] = authValues.UserId;
                }

                if (authValues.AuthType != CustomAuthenticationType.None)
                {
                    opParameters[ParameterCode.ClientAuthenticationType] = (byte)authValues.AuthType;
                    if (authValues.Token != null)
                    {
                        opParameters[ParameterCode.Token] = authValues.Token;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
                        {
                            opParameters[ParameterCode.ClientAuthenticationParams] = authValues.AuthGetParameters;
                        }
                        if (authValues.AuthPostData != null)
                        {
                            opParameters[ParameterCode.ClientAuthenticationData] = authValues.AuthPostData;
                        }
                    }
                }
            }

            return this.SendOperation(OperationCode.AuthenticateOnce, opParameters, new SendOptions() { Reliability = true, Encrypt = true });
        }
        public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
        {
            if (this.DebugOut >= DebugLevel.ALL)
            {
                this.Listener.DebugReturn(DebugLevel.ALL, "OpChangeGroups()");
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            if (groupsToRemove != null)
            {
                opParameters[(byte)ParameterCode.Remove] = groupsToRemove;
            }
            if (groupsToAdd != null)
            {
                opParameters[(byte)ParameterCode.Add] = groupsToAdd;
            }

            return this.SendOperation(OperationCode.ChangeGroups, opParameters, SendOptions.SendReliable);
        }
        public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
        {
            var paramDict = this.paramDictionaryPool.Acquire();
            try
            {
                if (raiseEventOptions != null)
                {
                    if (raiseEventOptions.CachingOption != EventCaching.DoNotCache)
                    {
                        paramDict.Add(ParameterCode.Cache, (byte)raiseEventOptions.CachingOption);
                    }
                    switch (raiseEventOptions.CachingOption)
                    {
                        case EventCaching.SliceSetIndex:
                        case EventCaching.SlicePurgeIndex:
                        case EventCaching.SlicePurgeUpToIndex:
                            return this.SendOperation(OperationCode.RaiseEvent, paramDict, sendOptions);
                        case EventCaching.SliceIncreaseIndex:
                        case EventCaching.RemoveFromRoomCacheForActorsLeft:
                            return this.SendOperation(OperationCode.RaiseEvent, paramDict, sendOptions);
                        case EventCaching.RemoveFromRoomCache:
                            if (raiseEventOptions.TargetActors != null)
                            {
                                paramDict.Add(ParameterCode.ActorList, raiseEventOptions.TargetActors);
                            }
                            break;
                        default:
                            if (raiseEventOptions.TargetActors != null)
                            {
                                paramDict.Add(ParameterCode.ActorList, raiseEventOptions.TargetActors);
                            }
                            else if (raiseEventOptions.InterestGroup != 0)
                            {
                                paramDict.Add(ParameterCode.Group, (byte)raiseEventOptions.InterestGroup);
                            }
                            else if (raiseEventOptions.Receivers != ReceiverGroup.Others)
                            {
                                paramDict.Add(ParameterCode.ReceiverGroup, (byte)raiseEventOptions.Receivers);
                            }
                            if (raiseEventOptions.Flags.HttpForward)
                            {
                                paramDict.Add(ParameterCode.EventForward, (byte)raiseEventOptions.Flags.WebhookFlags);
                            }
                            break;
                    }
                }
                paramDict.Add(ParameterCode.Code, (byte)eventCode);
                if (customEventContent != null)
                {
                    paramDict.Add(ParameterCode.Data, (object)customEventContent);
                }
                return this.SendOperation(OperationCode.RaiseEvent, paramDict, sendOptions);
            }
            finally
            {
                this.paramDictionaryPool.Release(paramDict);
            }
        }
        public virtual bool OpSettings(bool receiveLobbyStats)
        {
            if (this.DebugOut >= DebugLevel.ALL)
            {
                this.Listener.DebugReturn(DebugLevel.ALL, "OpSettings()");
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            if (receiveLobbyStats)
            {
                opParameters[(byte)0] = receiveLobbyStats;
            }

            if (opParameters.Count == 0)
            {
                return true;
            }

            return this.SendOperation(OperationCode.ServerSettings, opParameters, SendOptions.SendReliable);
        }
    }
    internal enum RoomOptionBit : int
    {
        CheckUserOnJoin = 0x01,  // toggles a check of the UserId when joining (enabling returning to a game)
        DeleteCacheOnLeave = 0x02,  // deletes cache on leave
        SuppressRoomEvents = 0x04,  // suppresses all room events
        PublishUserId = 0x08,  // signals that we should publish userId
        DeleteNullProps = 0x10,  // signals that we should remove property if its value was set to null. see RoomOption to Delete Null Properties
        BroadcastPropsChangeToAll = 0x20,  // signals that we should send PropertyChanged event to all room players including initiator
        SuppressPlayerInfo = 0x40,  // disables events join and leave from the server as well as property broadcasts in a room (to minimize traffic)
    }
    public class FindFriendsOptions
    {
        public bool CreatedOnGs = false;    //flag: 0x01
        public bool Visible = false;        //flag: 0x02
        public bool Open = false;           //flag: 0x04
        internal int ToIntFlags()
        {
            int optionFlags = 0;
            if (this.CreatedOnGs)
            {
                optionFlags = optionFlags | 0x1;
            }
            if (this.Visible)
            {
                optionFlags = optionFlags | 0x2;
            }
            if (this.Open)
            {
                optionFlags = optionFlags | 0x4;
            }
            return optionFlags;
        }
    }
    public class OpJoinRandomRoomParams
    {
        public Hashtable ExpectedCustomRoomProperties;
        public int ExpectedMaxPlayers;
        public MatchmakingMode MatchingType;
        public TypedLobby TypedLobby;
        public string SqlLobbyFilter;
        public string[] ExpectedUsers;
        public object Ticket;
    }
    public class EnterRoomParams
    {
        public string RoomName;
        public RoomOptions RoomOptions;
        public TypedLobby Lobby;
        public Hashtable PlayerProperties;
        protected internal bool OnGameServer = true; // defaults to true! better send more parameter than too few (GS needs all)
        protected internal JoinMode JoinMode;
        public string[] ExpectedUsers;
        public object Ticket;
    }
    public class ErrorCode
    {
        public const int Ok = 0;
        public const int OperationNotAllowedInCurrentState = -3;
        [Obsolete("Use InvalidOperation.")]
        public const int InvalidOperationCode = -2;
        public const int InvalidOperation = -2;
        public const int InternalServerError = -1;
        public const int InvalidAuthentication = 0x7FFF;
        public const int GameIdAlreadyExists = 0x7FFF - 1;
        public const int GameFull = 0x7FFF - 2;
        public const int GameClosed = 0x7FFF - 3;

        [Obsolete("No longer used, cause random matchmaking is no longer a process.")]
        public const int AlreadyMatched = 0x7FFF - 4;
        public const int ServerFull = 0x7FFF - 5;
        public const int UserBlocked = 0x7FFF - 6;
        public const int NoRandomMatchFound = 0x7FFF - 7;
        public const int GameDoesNotExist = 0x7FFF - 9;
        public const int MaxCcuReached = 0x7FFF - 10;
        public const int InvalidRegion = 0x7FFF - 11;
        public const int CustomAuthenticationFailed = 0x7FFF - 12;
        public const int AuthenticationTicketExpired = 0x7FF1;
        public const int PluginReportedError = 0x7FFF - 15;
        public const int PluginMismatch = 0x7FFF - 16;
        public const int JoinFailedPeerAlreadyJoined = 32750; // 0x7FFF - 17,
        public const int JoinFailedFoundInactiveJoiner = 32749; // 0x7FFF - 18,
        public const int JoinFailedWithRejoinerNotFound = 32748; // 0x7FFF - 19,
        public const int JoinFailedFoundExcludedUserId = 32747; // 0x7FFF - 20,
        public const int JoinFailedFoundActiveJoiner = 32746; // 0x7FFF - 21,
        public const int HttpLimitReached = 32745; // 0x7FFF - 22,
        public const int ExternalHttpCallFailed = 32744; // 0x7FFF - 23,
        public const int OperationLimitReached = 32743; // 0x7FFF - 24,
        public const int SlotError = 32742; // 0x7FFF - 25,
        public const int InvalidEncryptionParameters = 32741; // 0x7FFF - 24,

}
    public class ActorProperties
    {
        public const byte PlayerName = 255; // was: 1
        public const byte IsInactive = 254;
        public const byte UserId = 253;
    }
    public class GamePropertyKey
    {
        public const byte MaxPlayers = 255;
        public const byte MaxPlayersInt = 243;
        public const byte IsVisible = 254;
        public const byte IsOpen = 253;
        public const byte PlayerCount = 252;
        public const byte Removed = 251;
        public const byte PropsListedInLobby = 250;
        public const byte CleanupCacheOnLeave = 249;
        public const byte MasterClientId = (byte)248;
        public const byte ExpectedUsers = (byte)247;
        public const byte PlayerTtl = (byte)246;
        public const byte EmptyRoomTtl = (byte)245;
    }
    public class EventCode
    {
        public const byte GameList = 230;
        public const byte GameListUpdate = 229;
        public const byte QueueState = 228;
        public const byte Match = 227;
        public const byte AppStats = 226;
        public const byte LobbyStats = 224;
        [Obsolete("TCP routing was removed after becoming obsolete.")]
        public const byte AzureNodeInfo = 210;
        public const byte Join = (byte)255;
        public const byte Leave = (byte)254;
        public const byte PropertiesChanged = (byte)253;
        [Obsolete("Use PropertiesChanged now.")]
        public const byte SetProperties = (byte)253;
        public const byte ErrorInfo = 251;
        public const byte CacheSliceChanged = 250;
        public const byte AuthEvent = 223;
    }
    public class ParameterCode
    {
        public const byte SuppressRoomEvents = 237;
        public const byte EmptyRoomTTL = 236;
        public const byte PlayerTTL = 235;
        public const byte EventForward = 234;
        [Obsolete("Use: IsInactive")]
        public const byte IsComingBack = (byte)233;
        public const byte IsInactive = (byte)233;
        public const byte CheckUserOnJoin = (byte)232;
        public const byte ExpectedValues = (byte)231;
        public const byte Address = 230;
        public const byte PeerCount = 229;
        public const byte GameCount = 228;
        public const byte MasterPeerCount = 227;
        public const byte UserId = 225;
        public const byte ApplicationId = 224;
        public const byte Position = 223;
        public const byte MatchMakingType = 223;
        public const byte GameList = 222;
        public const byte Token = 221;
        public const byte AppVersion = 220;
        [Obsolete("TCP routing was removed after becoming obsolete.")]
        public const byte AzureNodeInfo = 210;	// only used within events, so use: EventCode.AzureNodeInfo
        [Obsolete("TCP routing was removed after becoming obsolete.")]
        public const byte AzureLocalNodeId = 209;
        [Obsolete("TCP routing was removed after becoming obsolete.")]
        public const byte AzureMasterNodeId = 208;
        public const byte RoomName = (byte)255;
        public const byte Broadcast = (byte)250;
        public const byte ActorList = (byte)252;
        public const byte ActorNr = (byte)254;
        public const byte PlayerProperties = (byte)249;
        public const byte CustomEventContent = (byte)245;
        public const byte Data = (byte)245;
        public const byte Code = (byte)244;
        public const byte GameProperties = (byte)248;
        public const byte Properties = (byte)251;
        public const byte TargetActorNr = (byte)253;
        public const byte ReceiverGroup = (byte)246;
        public const byte Cache = (byte)247;
        public const byte CleanupCacheOnLeave = (byte)241;
        public const byte Group = 240;
        public const byte Remove = 239;
        public const byte PublishUserId = 239;
        public const byte Add = 238;
        public const byte Info = 218;
        public const byte ClientAuthenticationType = 217;
        public const byte ClientAuthenticationParams = 216;
        public const byte JoinMode = 215;
        public const byte ClientAuthenticationData = 214;
        public const byte MasterClientId = (byte)203;
        public const byte FindFriendsRequestList = (byte)1;
        public const byte FindFriendsOptions = (byte)2;
        public const byte FindFriendsResponseOnlineList = (byte)1;
        public const byte FindFriendsResponseRoomIdList = (byte)2;
        public const byte LobbyName = (byte)213;
        public const byte LobbyType = (byte)212;
        public const byte LobbyStats = (byte)211;
        public const byte Region = (byte)210;
        public const byte UriPath = 209;
        public const byte WebRpcParameters = 208;
        public const byte WebRpcReturnCode = 207;
        public const byte WebRpcReturnMessage = 206;
        public const byte CacheSliceIndex = 205;
        public const byte Plugins = 204;
        public const byte NickName = 202;
        public const byte PluginName = 201;
        public const byte PluginVersion = 200;
        public const byte Cluster = 196;
        public const byte ExpectedProtocol = 195;
        public const byte CustomInitData = 194;
        public const byte EncryptionMode = 193;
        public const byte EncryptionData = 192;
        public const byte RoomOptionFlags = 191;
        public const byte Ticket = 190;
        public const byte MatchMakingGroupId = 189;
        public const byte AllowRepeats = 188;
        public const byte ReportQos = 187;
    }
    public class OperationCode
    {
        [Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
        public const byte ExchangeKeysForEncryption = 250;
        [Obsolete]
        public const byte Join = 255;
        public const byte AuthenticateOnce = 231;
        public const byte Authenticate = 230;
        public const byte JoinLobby = 229;
        public const byte LeaveLobby = 228;
        public const byte CreateGame = 227;
        public const byte JoinGame = 226;
        public const byte JoinRandomGame = 225;
        public const byte Leave = (byte)254;
        public const byte RaiseEvent = (byte)253;
        public const byte SetProperties = (byte)252;
        public const byte GetProperties = (byte)251;
        public const byte ChangeGroups = (byte)248;
        public const byte FindFriends = 222;
        public const byte GetLobbyStats = 221;
        public const byte GetRegions = 220;
        public const byte WebRpc = 219;
        public const byte ServerSettings = 218;
        public const byte GetGameList = 217;
    }
    public enum JoinMode : byte
    {
        Default = 0,
        CreateIfNotExists = 1,
        JoinOrRejoin = 2,
        RejoinOnly = 3,
    }
    public enum MatchmakingMode : byte
    {
        FillRoom = 0,
        SerialMatching = 1,
        RandomMatching = 2
    }
    public enum ReceiverGroup : byte
    {
        Others = 0,
        All = 1,
        MasterClient = 2,
    }
    public enum EventCaching : byte
    {
        DoNotCache = 0,
        [Obsolete]
        MergeCache = 1,
        [Obsolete]
        ReplaceCache = 2,
        [Obsolete]
        RemoveCache = 3,
        AddToRoomCache = 4,
        AddToRoomCacheGlobal = 5,
        RemoveFromRoomCache = 6,
        RemoveFromRoomCacheForActorsLeft = 7,
        SliceIncreaseIndex = 10,
        SliceSetIndex = 11,
        SlicePurgeIndex = 12,
        SlicePurgeUpToIndex = 13,
    }
    [Flags]
    public enum PropertyTypeFlag : byte
    {
        None = 0x00,
        Game = 0x01,
        Actor = 0x02,
        GameAndActor = Game | Actor
    }
    public class RoomOptions
    {
        public bool IsVisible { get { return this.isVisible; } set { this.isVisible = value; } }
        private bool isVisible = true;
        public bool IsOpen { get { return this.isOpen; } set { this.isOpen = value; } }
        private bool isOpen = true;
        public int MaxPlayers;
        public int PlayerTtl;
        public int EmptyRoomTtl;
        public bool CleanupCacheOnLeave { get { return this.cleanupCacheOnLeave; } set { this.cleanupCacheOnLeave = value; } }
        private bool cleanupCacheOnLeave = true;
        public Hashtable CustomRoomProperties;
        public string[] CustomRoomPropertiesForLobby = new string[0];
        public string[] Plugins;
        public bool SuppressRoomEvents { get; set; }
        public bool SuppressPlayerInfo { get; set; }
        public bool PublishUserId { get; set; }
        public bool DeleteNullProperties { get; set; }
        public bool BroadcastPropsChangeToAll { get { return this.broadcastPropsChangeToAll; } set { this.broadcastPropsChangeToAll = value; } }
        private bool broadcastPropsChangeToAll = true;

        #if SERVERSDK
        public bool CheckUserOnJoin { get; set; }
        #endif
    }
    public class RaiseEventOptions
    {
        public readonly static RaiseEventOptions Default = new RaiseEventOptions();
        public EventCaching CachingOption;
        public byte InterestGroup;
        public int[] TargetActors;
        public ReceiverGroup Receivers;
        [Obsolete("Not used where SendOptions are a parameter too. Use SendOptions.Channel instead.")]
        public byte SequenceChannel;
        public WebFlags Flags = WebFlags.Default;
    }
    public enum LobbyType :byte
    {
        Default = 0,
        SqlLobby = 2,
        AsyncRandomLobby = 3
    }
    public class TypedLobby
    {
        public string Name;
        public LobbyType Type;
        public static readonly TypedLobby Default = new TypedLobby();
        public bool IsDefault { get { return string.IsNullOrEmpty(this.Name); } }
        internal TypedLobby()
        {
        }
        public TypedLobby(string name, LobbyType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public override string ToString()
        {
            return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
        }
    }
    public class TypedLobbyInfo : TypedLobby
    {
        public int PlayerCount;
        public int RoomCount;

        public override string ToString()
        {
            return string.Format("TypedLobbyInfo '{0}'[{1}] rooms: {2} players: {3}", this.Name, this.Type, this.RoomCount, this.PlayerCount);
        }
    }
    public enum AuthModeOption { Auth, AuthOnce, AuthOnceWss }
    public enum CustomAuthenticationType : byte
    {
        Custom = 0,
        Steam = 1,
        Facebook = 2,
        Oculus = 3,
        PlayStation4 = 4,
        [Obsolete("Use PlayStation4 or PlayStation5 as needed")]
        PlayStation = 4,
        Xbox = 5,
        Viveport = 10,
        NintendoSwitch = 11,
        PlayStation5 = 12,
        [Obsolete("Use PlayStation4 or PlayStation5 as needed")]
        Playstation5 = 12,
        Epic = 13,
        FacebookGaming = 15,
        None = byte.MaxValue
    }
    public class AuthenticationValues
    {
        private CustomAuthenticationType authType = CustomAuthenticationType.None;
        public CustomAuthenticationType AuthType
        {
            get { return authType; }
            set { authType = value; }
        }
        public string AuthGetParameters { get; set; }
        public object AuthPostData { get; private set; }
        public object Token { get; protected internal set; }
        public string UserId { get; set; }
        public AuthenticationValues()
        {
        }
        public AuthenticationValues(string userId)
        {
            this.UserId = userId;
        }
        public virtual void SetAuthPostData(string stringData)
        {
            this.AuthPostData = (string.IsNullOrEmpty(stringData)) ? null : stringData;
        }
        public virtual void SetAuthPostData(byte[] byteData)
        {
            this.AuthPostData = byteData;
        }
        public virtual void SetAuthPostData(Dictionary<string, object> dictData)
        {
            this.AuthPostData = dictData;
        }
        public virtual void AddAuthParameter(string key, string value)
        {
            string ampersand = string.IsNullOrEmpty(this.AuthGetParameters) ? "" : "&";
            this.AuthGetParameters = string.Format("{0}{1}{2}={3}", this.AuthGetParameters, ampersand, System.Uri.EscapeDataString(key), System.Uri.EscapeDataString(value));
        }
        public override string ToString()
        {
            return string.Format("AuthenticationValues = AuthType: {0} UserId: {1}{2}{3}{4}",
                                 this.AuthType,
                                 this.UserId,
                                 string.IsNullOrEmpty(this.AuthGetParameters) ? " GetParameters: yes" : "",
                                 this.AuthPostData == null ? "" : " PostData: yes",
                                 this.Token == null ? "" : " Token: yes");
        }
        public AuthenticationValues CopyTo(AuthenticationValues copy)
        {
            copy.AuthType = this.AuthType;
            copy.AuthGetParameters = this.AuthGetParameters;
            copy.AuthPostData = this.AuthPostData;
            copy.UserId = this.UserId;
            return copy;
        }
    }
}
