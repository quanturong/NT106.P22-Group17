

namespace Photon.Pun
{
    using System.Diagnostics;
    using UnityEngine;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using UnityEngine.SceneManagement;

    using Photon.Realtime;
    using Debug = UnityEngine.Debug;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    #if UNITY_EDITOR
    using UnityEditor;
    using System.IO;
    #endif


    public struct InstantiateParameters
    {
        public int[] viewIDs;
        public byte objLevelPrefix;
        public object[] data;
        public byte @group;
        public Quaternion rotation;
        public Vector3 position;
        public string prefabName;
        public Player creator;
        public int timestamp;

        public InstantiateParameters(string prefabName, Vector3 position, Quaternion rotation, byte @group, object[] data, byte objLevelPrefix, int[] viewIDs, Player creator, int timestamp)
        {
            this.prefabName = prefabName;
            this.position = position;
            this.rotation = rotation;
            this.@group = @group;
            this.data = data;
            this.objLevelPrefix = objLevelPrefix;
            this.viewIDs = viewIDs;
            this.creator = creator;
            this.timestamp = timestamp;
        }
    }
    public static partial class PhotonNetwork
    {
        public const string PunVersion = "2.50";
        public static string GameVersion
        {
            get { return gameVersion; }
            set
            {
                gameVersion = value;
                NetworkingClient.AppVersion = string.Format("{0}_{1}", value, PhotonNetwork.PunVersion);
            }
        }

        private static string gameVersion;
        public static string AppVersion
        {
            get { return NetworkingClient.AppVersion; }
        }
        public static LoadBalancingClient NetworkingClient;
        public static readonly int MAX_VIEW_IDS = 1000; // VIEW & PLAYER LIMIT CAN BE EASILY CHANGED, SEE DOCS
        public const string ServerSettingsFileName = "PhotonServerSettings";

        private static ServerSettings photonServerSettings;
        public static ServerSettings PhotonServerSettings
        {
            get
            {
                if (photonServerSettings == null)
                {
                    LoadOrCreateSettings();
                }

                return photonServerSettings;
            }
            private set { photonServerSettings = value; }
        }
        public static string ServerAddress { get { return (NetworkingClient != null) ? NetworkingClient.CurrentServerAddress : "<not connected>"; } }
        public static string CloudRegion { get { return (NetworkingClient != null && IsConnected && Server!=ServerConnection.NameServer) ? NetworkingClient.CloudRegion : null; } }
        public static string CurrentCluster { get { return (NetworkingClient != null ) ? NetworkingClient.CurrentCluster : null; } }
        private const string PlayerPrefsKey = "PUNCloudBestRegion";
        public static string BestRegionSummaryInPreferences
        {
            get
            {
                return PlayerPrefs.GetString(PlayerPrefsKey, null);
            }
            internal set
            {
                if (String.IsNullOrEmpty(value))
                {
                    PlayerPrefs.DeleteKey(PlayerPrefsKey);
                }
                else
                {
                    PlayerPrefs.SetString(PlayerPrefsKey, value.ToString());
                }
            }
        }
        public static bool IsConnected
        {
            get
            {
                if (OfflineMode)
                {
                    return true;
                }

                if (NetworkingClient == null)
                {
                    return false;
                }

                return NetworkingClient.IsConnected;
            }
        }
        public static bool IsConnectedAndReady
        {
            get
            {
                if (OfflineMode)
                {
                    return true;
                }
                if (NetworkingClient == null)
                {
                    return false;
                }

                return NetworkingClient.IsConnectedAndReady;
            }
        }
        public static ClientState NetworkClientState
        {
            get
            {
                if (OfflineMode)
                {
                    return (offlineModeRoom != null) ? ClientState.Joined : ClientState.ConnectedToMasterServer;
                }

                if (NetworkingClient == null)
                {
                    return ClientState.Disconnected;
                }

                return NetworkingClient.State;
            }
        }
        public static ConnectMethod ConnectMethod = ConnectMethod.NotCalled;
        public static ServerConnection Server
        {
            get
            {
                if (OfflineMode)
                {
                    return CurrentRoom == null ? ServerConnection.MasterServer : ServerConnection.GameServer;
                }
                return (PhotonNetwork.NetworkingClient != null) ? PhotonNetwork.NetworkingClient.Server : ServerConnection.NameServer;
            }
        }
        public static AuthenticationValues AuthValues
        {
            get { return (NetworkingClient != null) ? NetworkingClient.AuthValues : null; }
            set { if (NetworkingClient != null) NetworkingClient.AuthValues = value; }
        }
        public static TypedLobby CurrentLobby
        {
            get { return NetworkingClient.CurrentLobby; }
        }
        public static Room CurrentRoom
        {
            get
            {
                if (offlineMode)
                {
                    return offlineModeRoom;
                }

                return NetworkingClient == null ? null : NetworkingClient.CurrentRoom;
            }
        }
        public static PunLogLevel LogLevel = PunLogLevel.ErrorsOnly;
        public static Player LocalPlayer
        {
            get
            {
                if (NetworkingClient == null)
                {
                    return null; // suppress ExitApplication errors
                }

                return NetworkingClient.LocalPlayer;
            }
        }
        public static string NickName
        {
            get
            {
                return NetworkingClient.NickName;
            }

            set
            {
                NetworkingClient.NickName = value;
            }
        }
        public static Player[] PlayerList
        {
            get
            {
                Room room = CurrentRoom;
                if (room != null)
                {
                    return room.Players.Values.OrderBy((x) => x.ActorNumber).ToArray();
                }
                return new Player[0];
            }
        }
        public static Player[] PlayerListOthers
        {
            get
            {
                Room room = CurrentRoom;
                if (room != null)
                {
                    return room.Players.Values.OrderBy((x) => x.ActorNumber).Where(x => !x.IsLocal).ToArray();
                }
                return new Player[0];
            }
        }
        public static bool EnableCloseConnection = false;
        public static float PrecisionForVectorSynchronization = 0.000099f;
        public static float PrecisionForQuaternionSynchronization = 1.0f;
        public static float PrecisionForFloatSynchronization = 0.01f;
        public static bool OfflineMode
        {
            get
            {
                return offlineMode;
            }

            set
            {
                if (value == offlineMode)
                {
                    return;
                }

                if (value && IsConnected)
                {
                    Debug.LogError("Can't start OFFLINE mode while connected!");
                    return;
                }

                if (NetworkingClient.IsConnected)
                {
                    NetworkingClient.Disconnect(); // Cleanup (also calls OnLeftRoom to reset stuff)
                }

                offlineMode = value;

                if (offlineMode)
                {
                    NetworkingClient.ChangeLocalID(-1, true);
                    NetworkingClient.ConnectionCallbackTargets.OnConnectedToMaster();
                }
                else
                {
                    bool wasInOfflineRoom = offlineModeRoom != null;

                    if (wasInOfflineRoom)
                    {
                        LeftRoomCleanup();
                    }
                    offlineModeRoom = null;
                    PhotonNetwork.NetworkingClient.CurrentRoom = null;
                    NetworkingClient.ChangeLocalID(-1);
                    if (wasInOfflineRoom)
                    {
                        NetworkingClient.MatchMakingCallbackTargets.OnLeftRoom();
                    }
                }
            }
        }

        private static bool offlineMode = false;
        private static Room offlineModeRoom = null;
        public static bool AutomaticallySyncScene
        {
            get
            {
                return automaticallySyncScene;
            }
            set
            {
                automaticallySyncScene = value;
                if (automaticallySyncScene && CurrentRoom != null)
                {
                    LoadLevelIfSynced();
                }
            }
        }

        private static bool automaticallySyncScene = false;
        public static bool EnableLobbyStatistics
        {
            get
            {
                return NetworkingClient.EnableLobbyStatistics;
            }
        }
        public static bool InLobby
        {
            get
            {
                return NetworkingClient.InLobby;
            }
        }
        public static int SendRate
        {
            get
            {
                return 1000 / sendFrequency;
            }

            set
            {
                sendFrequency = 1000 / value;
                if (PhotonHandler.Instance != null)
                {
                    PhotonHandler.Instance.UpdateInterval = sendFrequency;
                }
            }
        }

        private static int sendFrequency = 33; // in milliseconds.
        public static int SerializationRate
        {
            get
            {
                return 1000 / serializationFrequency;
            }

            set
            {
                serializationFrequency = 1000 / value;
                if (PhotonHandler.Instance != null)
                {
                    PhotonHandler.Instance.UpdateIntervalOnSerialize = serializationFrequency;
                }
            }
        }

        private static int serializationFrequency = 100; // in milliseconds. I.e. 100 = 100ms which makes 10 times/second
        public static bool IsMessageQueueRunning
        {
            get
            {
                return isMessageQueueRunning;
            }

            set
            {
                isMessageQueueRunning = value;
            }
        }
        private static bool isMessageQueueRunning = true;
        public static double Time
        {
            get
            {
                if (UnityEngine.Time.frameCount == frame)
                {
                    return frametime;
                }

                uint u = (uint)ServerTimestamp;
                double t = u;
                frametime =  t / 1000.0d;
                frame = UnityEngine.Time.frameCount;
                return frametime;
            }
        }

        private static double frametime;
        private static int frame;
        public static int ServerTimestamp
        {
            get
            {
                if (OfflineMode)
                {
                    if (StartupStopwatch != null && StartupStopwatch.IsRunning)
                    {
                        return (int)StartupStopwatch.ElapsedMilliseconds;
                    }
                    return Environment.TickCount;
                }

                return NetworkingClient.LoadBalancingPeer.ServerTimeInMilliSeconds;   // TODO: implement ServerTimeInMilliSeconds in LBC
            }
        }
        private static Stopwatch StartupStopwatch;
        public static float KeepAliveInBackground
        {
            set
            {
                if (PhotonHandler.Instance != null)
                {
                    PhotonHandler.Instance.KeepAliveInBackground = (int)Mathf.Round(value * 1000.0f);
                }
            }

            get { return PhotonHandler.Instance != null ? Mathf.Round(PhotonHandler.Instance.KeepAliveInBackground / 1000.0f) : 60.0f; }
        }
        public static float MinimalTimeScaleToDispatchInFixedUpdate = -1f;
        public static bool IsMasterClient
        {
            get
            {
                if (OfflineMode)
                {
                    return true;
                }

                return NetworkingClient.CurrentRoom != null && NetworkingClient.CurrentRoom.MasterClientId == LocalPlayer.ActorNumber;  // TODO: implement MasterClient shortcut in LBC?
            }
        }
        public static Player MasterClient
        {
            get
            {
                if (OfflineMode)
                {
                    return PhotonNetwork.LocalPlayer;
                }

                if (NetworkingClient == null || NetworkingClient.CurrentRoom == null)
                {
                    return null;
                }

                return NetworkingClient.CurrentRoom.GetPlayer(NetworkingClient.CurrentRoom.MasterClientId);
            }
        }
        public static bool InRoom
        {
            get
            {
                return NetworkClientState == ClientState.Joined;
            }
        }
        public static int CountOfPlayersOnMaster
        {
            get
            {
                return NetworkingClient.PlayersOnMasterCount;
            }
        }
        public static int CountOfPlayersInRooms
        {
            get
            {
                return NetworkingClient.PlayersInRoomsCount;
            }
        }
        public static int CountOfPlayers
        {
            get
            {
                return NetworkingClient.PlayersInRoomsCount + NetworkingClient.PlayersOnMasterCount;
            }
        }
        public static int CountOfRooms
        {
            get
            {
                return NetworkingClient.RoomsCount;
            }
        }
        public static bool NetworkStatisticsEnabled
        {
            get
            {
                return NetworkingClient.LoadBalancingPeer.TrafficStatsEnabled;
            }

            set
            {
                NetworkingClient.LoadBalancingPeer.TrafficStatsEnabled = value;
            }
        }
        public static int ResentReliableCommands
        {
            get { return NetworkingClient.LoadBalancingPeer.ResentReliableCommands; }
        }
        public static bool CrcCheckEnabled
        {
            get { return NetworkingClient.LoadBalancingPeer.CrcEnabled; }
            set
            {
                if (!IsConnected)
                {
                    NetworkingClient.LoadBalancingPeer.CrcEnabled = value;
                }
                else
                {
                    Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + NetworkingClient.LoadBalancingPeer.CrcEnabled);
                }
            }
        }
        public static int PacketLossByCrcCheck
        {
            get { return NetworkingClient.LoadBalancingPeer.PacketLossByCrc; }
        }
        public static int MaxResendsBeforeDisconnect
        {
            get { return NetworkingClient.LoadBalancingPeer.SentCountAllowance; }
            set
            {
                if (value < 3) value = 3;
                if (value > 10) value = 10;
                NetworkingClient.LoadBalancingPeer.SentCountAllowance = value;
            }
        }
        public static int QuickResends
        {
            get { return NetworkingClient.LoadBalancingPeer.QuickResendAttempts; }
            set
            {
                if (value < 0) value = 0;
                if (value > 3) value = 3;
                NetworkingClient.LoadBalancingPeer.QuickResendAttempts = (byte)value;
            }
        }
        [Obsolete("Set port overrides in ServerPortOverrides. Not used anymore!")]
        public static bool UseAlternativeUdpPorts { get; set; }
        public static PhotonPortDefinition ServerPortOverrides
        {
            get { return (NetworkingClient == null) ? new PhotonPortDefinition() :  NetworkingClient.ServerPortOverrides; }
            set { if (NetworkingClient != null) NetworkingClient.ServerPortOverrides = value; }
        }


        private static int lastUsedViewSubId = 0;  // each player only needs to remember it's own (!) last used subId to speed up assignment
        private static int lastUsedViewSubIdStatic = 0;  // per room, the master is able to instantiate GOs. the subId for this must be unique too
        static PhotonNetwork()
        {
            #if !UNITY_EDITOR
            StaticReset();  // in builds, we just reset/init the client once
            #else

                #if UNITY_2019_4_OR_NEWER
                if (NetworkingClient == null)
                {
                    NetworkingClient = new LoadBalancingClient();
                }
                #else
                StaticReset();  // in OLDER unity editor versions there is no RuntimeInitializeOnLoadMethod, so call reset
                #endif

            #endif
        }

        #if UNITY_EDITOR && UNITY_2019_4_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        #endif
        private static void StaticReset()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            #endif
            monoRPCMethodsCache.Clear();
            OfflineMode = false;
            ConnectionProtocol protocol = PhotonNetwork.PhotonServerSettings.AppSettings.Protocol;
            NetworkingClient = new LoadBalancingClient(protocol);
            NetworkingClient.LoadBalancingPeer.QuickResendAttempts = 2;
            NetworkingClient.LoadBalancingPeer.SentCountAllowance = 9;

            NetworkingClient.EventReceived -= OnEvent;
            NetworkingClient.EventReceived += OnEvent;
            NetworkingClient.OpResponseReceived -= OnOperation;
            NetworkingClient.OpResponseReceived += OnOperation;
            NetworkingClient.StateChanged -= OnClientStateChanged;
            NetworkingClient.StateChanged += OnClientStateChanged;

            StartupStopwatch = new Stopwatch();
            StartupStopwatch.Start();
            PhotonHandler.Instance.Client = NetworkingClient;


            Application.runInBackground = PhotonServerSettings.RunInBackground;
            PrefabPool = new DefaultPool();
            rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
            for (int index = 0; index < PhotonNetwork.PhotonServerSettings.RpcList.Count; index++)
            {
                var name = PhotonNetwork.PhotonServerSettings.RpcList[index];
                rpcShortcuts[name] = index;
            }
            CustomTypes.Register();
        }
        public static bool ConnectUsingSettings()
        {
            if (PhotonServerSettings == null)
            {
                Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: " + ServerSettingsFileName);
                return false;
            }

            return ConnectUsingSettings(PhotonServerSettings.AppSettings, PhotonServerSettings.StartInOfflineMode);
        }

        public static bool ConnectUsingSettings(AppSettings appSettings, bool startInOfflineMode = false) // parameter name hides static class member
        {
            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                Debug.LogWarning("ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }
            if (PhotonHandler.AppQuits)
            {
                Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
                return false;
            }
            if (PhotonServerSettings == null)
            {
                Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: " + ServerSettingsFileName);
                return false;
            }

            SetupLogging();


            NetworkingClient.LoadBalancingPeer.TransportProtocol = appSettings.Protocol;
            NetworkingClient.ExpectedProtocol = null;
            NetworkingClient.EnableProtocolFallback = appSettings.EnableProtocolFallback;
            NetworkingClient.AuthMode = appSettings.AuthMode;


            IsMessageQueueRunning = true;
            NetworkingClient.AppId = appSettings.AppIdRealtime;
            GameVersion = appSettings.AppVersion;



            if (startInOfflineMode)
            {
                OfflineMode = true;
                return true;
            }

            if (OfflineMode)
            {
                OfflineMode = false; // Cleanup offline mode
                Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
            }


            NetworkingClient.EnableLobbyStatistics = appSettings.EnableLobbyStatistics;
            NetworkingClient.ProxyServerAddress = appSettings.ProxyServer;


            if (appSettings.IsMasterServerAddress)
            {
                if (AuthValues == null)
                {
                    AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
                }
                else if (string.IsNullOrEmpty(AuthValues.UserId))
                {
                    AuthValues.UserId = Guid.NewGuid().ToString();
                }
                return ConnectToMaster(appSettings.Server, appSettings.Port, appSettings.AppIdRealtime);
            }


            NetworkingClient.NameServerPortInAppSettings = appSettings.Port;
            if (!appSettings.IsDefaultNameServer)
            {
                NetworkingClient.NameServerHost = appSettings.Server;
            }


            if (appSettings.IsBestRegion)
            {
                return ConnectToBestCloudServer();
            }

            return ConnectToRegion(appSettings.FixedRegion);
        }
        public static bool ConnectToMaster(string masterServerAddress, int port, string appID)
        {
            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }
            if (PhotonHandler.AppQuits)
            {
                Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
                return false;
            }

            if (OfflineMode)
            {
                OfflineMode = false; // Cleanup offline mode
                Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
            }

            if (!IsMessageQueueRunning)
            {
                IsMessageQueueRunning = true;
                Debug.LogWarning("ConnectToMaster() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
            }

            SetupLogging();
            ConnectMethod = ConnectMethod.ConnectToMaster;

            NetworkingClient.IsUsingNameServer = false;
            NetworkingClient.MasterServerAddress = (port == 0) ? masterServerAddress : masterServerAddress + ":" + port;
            NetworkingClient.AppId = appID;

            return NetworkingClient.ConnectToMasterServer();
        }
        public static bool ConnectToBestCloudServer()
        {
            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                Debug.LogWarning("ConnectToBestCloudServer() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }
            if (PhotonHandler.AppQuits)
            {
                Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
                return false;
            }

            SetupLogging();
            ConnectMethod = ConnectMethod.ConnectToBest;
            bool couldConnect = PhotonNetwork.NetworkingClient.ConnectToNameServer();
            return couldConnect;
        }
        public static bool ConnectToRegion(string region)
        {
            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected && NetworkingClient.Server != ServerConnection.NameServer)
            {
                Debug.LogWarning("ConnectToRegion() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }
            if (PhotonHandler.AppQuits)
            {
                Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
                return false;
            }

            SetupLogging();
            ConnectMethod = ConnectMethod.ConnectToRegion;

            if (!string.IsNullOrEmpty(region))
            {
                return NetworkingClient.ConnectToRegionMaster(region);
            }

            return false;
        }
        public static void Disconnect()
        {
            if (OfflineMode)
            {
                OfflineMode = false;
                offlineModeRoom = null;
                NetworkingClient.State = ClientState.Disconnecting;
                NetworkingClient.OnStatusChanged(StatusCode.Disconnect);
                return;
            }

            if (NetworkingClient == null)
            {
                return; // Suppress error when quitting playmode in the editor
            }

            NetworkingClient.Disconnect();
        }
        public static bool Reconnect()
        {
            if (string.IsNullOrEmpty(NetworkingClient.MasterServerAddress))
            {
                Debug.LogWarning("Reconnect() failed. It seems the client wasn't connected before?! Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }

            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                Debug.LogWarning("Reconnect() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }

            if (OfflineMode)
            {
                OfflineMode = false; // Cleanup offline mode
                Debug.LogWarning("Reconnect() disabled the offline mode. No longer offline.");
            }

            if (!IsMessageQueueRunning)
            {
                IsMessageQueueRunning = true;
                Debug.LogWarning("Reconnect() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
            }

            NetworkingClient.IsUsingNameServer = false;
            return NetworkingClient.ReconnectToMaster();
        }
        public static void NetworkStatisticsReset()
        {
            NetworkingClient.LoadBalancingPeer.TrafficStatsReset();
        }
        public static string NetworkStatisticsToString()
        {
            if (NetworkingClient == null || OfflineMode)
            {
                return "Offline or in OfflineMode. No VitalStats available.";
            }

            return NetworkingClient.LoadBalancingPeer.VitalStatsToString(false);
        }
        private static bool VerifyCanUseNetwork()
        {
            if (IsConnected)
            {
                return true;
            }

            Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
            return false;
        }
        public static int GetPing()
        {
            return NetworkingClient.LoadBalancingPeer.RoundTripTime;
        }
        public static void FetchServerTimestamp()
        {
            if (NetworkingClient != null)
            {
                NetworkingClient.LoadBalancingPeer.FetchServerTimestamp();
            }
        }
        public static void SendAllOutgoingCommands()
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }

            while (NetworkingClient.LoadBalancingPeer.SendOutgoingCommands())
            {
            }
        }
        public static bool CloseConnection(Player kickPlayer)
        {
            if (!VerifyCanUseNetwork())
            {
                return false;
            }

            if (!PhotonNetwork.EnableCloseConnection)
            {
                Debug.LogError("CloseConnection is disabled. No need to call it.");
                return false;
            }

            if (!LocalPlayer.IsMasterClient)
            {
                Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
                return false;
            }

            if (kickPlayer == null)
            {
                Debug.LogError("CloseConnection: No such player connected!");
                return false;
            }

            RaiseEventOptions options = new RaiseEventOptions() { TargetActors = new int[] { kickPlayer.ActorNumber } };
            return NetworkingClient.OpRaiseEvent(PunEvent.CloseConnection, null, options, SendOptions.SendReliable);
        }
        public static bool SetMasterClient(Player masterClientPlayer)
        {
            if (!InRoom || !VerifyCanUseNetwork() || OfflineMode)
            {
                if (LogLevel == PunLogLevel.Informational) Debug.Log("Can not SetMasterClient(). Not in room or in OfflineMode.");
                return false;
            }

            return CurrentRoom.SetMasterClient(masterClientPlayer);
        }
        public static bool JoinRandomRoom()
        {
            return JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null);
        }
        public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, int expectedMaxPlayers)
        {
            return JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null);
        }
        public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, int expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers = null)
        {
            if (OfflineMode)
            {
                if (offlineModeRoom != null)
                {
                    Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
                    return false;
                }
                EnterOfflineRoom("offline room", null, true);
                return true;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("JoinRandomRoom failed. Client is on "+ NetworkingClient.Server+ " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : " but not ready for operations (State: "+ NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }

            typedLobby = typedLobby ?? ((NetworkingClient.InLobby) ? NetworkingClient.CurrentLobby : null);  // use given lobby, or active lobby (if any active) or none

            OpJoinRandomRoomParams opParams = new OpJoinRandomRoomParams();
            opParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
            opParams.ExpectedMaxPlayers = expectedMaxPlayers;
            opParams.MatchingType = matchingType;
            opParams.TypedLobby = typedLobby;
            opParams.SqlLobbyFilter = sqlLobbyFilter;
            opParams.ExpectedUsers = expectedUsers;

            return NetworkingClient.OpJoinRandomRoom(opParams);
        }
        public static bool JoinRandomOrCreateRoom(Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = 0, MatchmakingMode matchingType = MatchmakingMode.FillRoom, TypedLobby typedLobby = null, string sqlLobbyFilter = null, string roomName = null, RoomOptions roomOptions = null, string[] expectedUsers = null)
        {
            if (OfflineMode)
            {
                if (offlineModeRoom != null)
                {
                    Debug.LogError("JoinRandomOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
                    return false;
                }
                EnterOfflineRoom("offline room", null, true);
                return true;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("JoinRandomOrCreateRoom failed. Client is on "+ NetworkingClient.Server+ " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : " but not ready for operations (State: "+ NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }

            typedLobby = typedLobby ?? ((NetworkingClient.InLobby) ? NetworkingClient.CurrentLobby : null); // use given lobby, or active lobby (if any active) or none

            OpJoinRandomRoomParams opParams = new OpJoinRandomRoomParams();
            opParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
            opParams.ExpectedMaxPlayers = expectedMaxPlayers;
            opParams.MatchingType = matchingType;
            opParams.TypedLobby = typedLobby;
            opParams.SqlLobbyFilter = sqlLobbyFilter;
            opParams.ExpectedUsers = expectedUsers;

            EnterRoomParams enterRoomParams = new EnterRoomParams();
            enterRoomParams.RoomName = roomName;
            enterRoomParams.RoomOptions = roomOptions;
            enterRoomParams.Lobby = typedLobby;
            enterRoomParams.ExpectedUsers = expectedUsers;

            return NetworkingClient.OpJoinRandomOrCreateRoom(opParams, enterRoomParams);
        }
        public static bool CreateRoom(string roomName, RoomOptions roomOptions = null, TypedLobby typedLobby = null, string[] expectedUsers = null)
        {
            if (OfflineMode)
            {
                if (offlineModeRoom != null)
                {
                    Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
                    return false;
                }
                EnterOfflineRoom(roomName, roomOptions, true);
                return true;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("CreateRoom failed. Client is on " + NetworkingClient.Server + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : "but not ready for operations (State: " + NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }

            typedLobby = typedLobby ?? ((NetworkingClient.InLobby) ? NetworkingClient.CurrentLobby : null);  // use given lobby, or active lobby (if any active) or none

            EnterRoomParams opParams = new EnterRoomParams();
            opParams.RoomName = roomName;
            opParams.RoomOptions = roomOptions;
            opParams.Lobby = typedLobby;
            opParams.ExpectedUsers = expectedUsers;

            return NetworkingClient.OpCreateRoom(opParams);
        }
        public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers = null)
        {
            if (OfflineMode)
            {
                if (offlineModeRoom != null)
                {
                    Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
                    return false;
                }
                EnterOfflineRoom(roomName, roomOptions, true);  // in offline mode, JoinOrCreateRoom assumes you create the room
                return true;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("JoinOrCreateRoom failed. Client is on " + NetworkingClient.Server + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : "but not ready for operations (State: " + NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
                return false;
            }

            typedLobby = typedLobby ?? ((NetworkingClient.InLobby) ? NetworkingClient.CurrentLobby : null);  // use given lobby, or active lobby (if any active) or none

            EnterRoomParams opParams = new EnterRoomParams();
            opParams.RoomName = roomName;
            opParams.RoomOptions = roomOptions;
            opParams.Lobby = typedLobby;
            opParams.PlayerProperties = LocalPlayer.CustomProperties;
            opParams.ExpectedUsers = expectedUsers;

            return NetworkingClient.OpJoinOrCreateRoom(opParams);
        }
        public static bool JoinRoom(string roomName, string[] expectedUsers = null)
        {
            if (OfflineMode)
            {
                if (offlineModeRoom != null)
                {
                    Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
                    return false;
                }
                EnterOfflineRoom(roomName, null, true);
                return true;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("JoinRoom failed. Client is on " + NetworkingClient.Server + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : "but not ready for operations (State: " + NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
                return false;
            }


            EnterRoomParams opParams = new EnterRoomParams();
            opParams.RoomName = roomName;
            opParams.ExpectedUsers = expectedUsers;

            return NetworkingClient.OpJoinRoom(opParams);
        }
        public static bool RejoinRoom(string roomName)
        {
            if (OfflineMode)
            {
                Debug.LogError("RejoinRoom failed due to offline mode.");
                return false;
            }
            if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
            {
                Debug.LogError("RejoinRoom failed. Client is on " + NetworkingClient.Server + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : "but not ready for operations (State: " + NetworkingClient.State + ")") + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
                return false;
            }
            if (string.IsNullOrEmpty(roomName))
            {
                Debug.LogError("RejoinRoom failed. A roomname is required. If you don't know one, how will you join?");
                return false;
            }

            return NetworkingClient.OpRejoinRoom(roomName);
        }
        public static bool ReconnectAndRejoin()
        {
            if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                Debug.LogWarning("ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
                return false;
            }
            if (OfflineMode)
            {
                OfflineMode = false; // Cleanup offline mode
                Debug.LogWarning("ReconnectAndRejoin() disabled the offline mode. No longer offline.");
            }

            if (!IsMessageQueueRunning)
            {
                IsMessageQueueRunning = true;
                Debug.LogWarning("ReconnectAndRejoin() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
            }

            return NetworkingClient.ReconnectAndRejoin();
        }
        public static bool LeaveRoom(bool becomeInactive = true)
        {
            if (OfflineMode)
            {
                offlineModeRoom = null;
                NetworkingClient.MatchMakingCallbackTargets.OnLeftRoom();
                NetworkingClient.ConnectionCallbackTargets.OnConnectedToMaster();
            }
            else
            {
                if (CurrentRoom == null)
                {
                    Debug.LogWarning("PhotonNetwork.CurrentRoom is null. You don't have to call LeaveRoom() when you're not in one. State: " + PhotonNetwork.NetworkClientState);
                }
                else
                {
                    becomeInactive = becomeInactive && CurrentRoom.PlayerTtl != 0; // in a room with playerTTL == 0, the operation "leave" will never turn a client inactive
                }
                return NetworkingClient.OpLeaveRoom(becomeInactive);
            }

            return true;
        }
        private static void EnterOfflineRoom(string roomName, RoomOptions roomOptions, bool createdRoom)
        {
            offlineModeRoom = new Room(roomName, roomOptions, true);
            NetworkingClient.ChangeLocalID(1, true);
            offlineModeRoom.masterClientId = 1;
            offlineModeRoom.AddPlayer(PhotonNetwork.LocalPlayer);
            offlineModeRoom.LoadBalancingClient = PhotonNetwork.NetworkingClient;
            PhotonNetwork.NetworkingClient.CurrentRoom = offlineModeRoom;

            if (createdRoom)
            {
                NetworkingClient.MatchMakingCallbackTargets.OnCreatedRoom();
            }

            NetworkingClient.MatchMakingCallbackTargets.OnJoinedRoom();
        }
        public static bool JoinLobby()
        {
            return JoinLobby(null);
        }
        public static bool JoinLobby(TypedLobby typedLobby)
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.Server == ServerConnection.MasterServer)
            {
                return NetworkingClient.OpJoinLobby(typedLobby);
            }

            return false;
        }
        public static bool LeaveLobby()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.Server == ServerConnection.MasterServer)
            {
                return NetworkingClient.OpLeaveLobby();
            }

            return false;
        }
        public static bool FindFriends(string[] friendsToFind)
        {
            if (NetworkingClient == null || offlineMode)
            {
                return false;
            }

            return NetworkingClient.OpFindFriends(friendsToFind);
        }
        public static bool GetCustomRoomList(TypedLobby typedLobby, string sqlLobbyFilter)
        {
            return NetworkingClient.OpGetGameList(typedLobby, sqlLobbyFilter);
        }
        public static bool SetPlayerCustomProperties(Hashtable customProperties)
        {
            if (customProperties == null)
            {
                customProperties = new Hashtable();
                foreach (object k in LocalPlayer.CustomProperties.Keys)
                {
                    customProperties[(string)k] = null;
                }
            }

            return LocalPlayer.SetCustomProperties(customProperties);
        }
        public static void RemovePlayerCustomProperties(string[] customPropertiesToDelete)
        {

            if (customPropertiesToDelete == null || customPropertiesToDelete.Length == 0 || LocalPlayer.CustomProperties == null)
            {
                LocalPlayer.CustomProperties = new Hashtable();
                return;
            }
            for (int i = 0; i < customPropertiesToDelete.Length; i++)
            {
                string key = customPropertiesToDelete[i];
                if (LocalPlayer.CustomProperties.ContainsKey(key))
                {
                    LocalPlayer.CustomProperties.Remove(key);
                }
            }
        }
        public static bool RaiseEvent(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
        {
            if (offlineMode)
            {
                if (raiseEventOptions.Receivers == ReceiverGroup.Others)
                {
                    return true;
                }

                EventData evData = new EventData { Code = eventCode };  // creates the equivalent of a received event
                evData.Parameters[ParameterCode.Data] = eventContent;
                evData.Parameters[ParameterCode.ActorNr] = 1;

                NetworkingClient.OnEvent(evData);
                return true;
            }

            if (!InRoom || eventCode >= 200)
            {
                Debug.LogWarning("RaiseEvent(" + eventCode + ") failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
                return false;
            }

            return NetworkingClient.OpRaiseEvent(eventCode, eventContent, raiseEventOptions, sendOptions);
        }
        private static bool RaiseEventInternal(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
        {
            if (offlineMode)
            {
                return false;
            }

            if (!InRoom)
            {
                Debug.LogWarning("RaiseEvent(" + eventCode + ") failed. Your event is not being sent! Check if your are in a Room");
                return false;
            }

            return NetworkingClient.OpRaiseEvent(eventCode, eventContent, raiseEventOptions, sendOptions);
        }
        public static bool AllocateViewID(PhotonView view)
        {
            if (view.ViewID != 0)
            {
                Debug.LogError("AllocateViewID() can't be used for PhotonViews that already have a viewID. This view is: " + view.ToString());
                return false;
            }

            int manualId = AllocateViewID(LocalPlayer.ActorNumber);
            view.ViewID = manualId;
            return true;
        }

        [Obsolete("Renamed. Use AllocateRoomViewID instead")]
        public static bool AllocateSceneViewID(PhotonView view)
        {
            return AllocateRoomViewID(view);
        }
        public static bool AllocateRoomViewID(PhotonView view)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Only the Master Client can AllocateRoomViewID(). Check PhotonNetwork.IsMasterClient!");
                return false;
            }

            if (view.ViewID != 0)
            {
                Debug.LogError("AllocateRoomViewID() can't be used for PhotonViews that already have a viewID. This view is: " + view.ToString());
                return false;
            }

            int manualId = AllocateViewID(0);
            view.ViewID = manualId;
            return true;
        }
        public static int AllocateViewID(bool roomObject)
        {
            if (roomObject && !LocalPlayer.IsMasterClient)
            {
                Debug.LogError("Only a Master Client can AllocateViewID() for room objects. This client/player is not a Master Client. Returning an invalid viewID: -1.");
                return 0;
            }

            int ownerActorNumber = roomObject ? 0 : LocalPlayer.ActorNumber;
            return AllocateViewID(ownerActorNumber);
        }
        public static int AllocateViewID(int ownerId)
        {
            if (ownerId == 0)
            {
                int newSubId = lastUsedViewSubIdStatic;
                int newViewId;
                int ownerIdOffset = ownerId * MAX_VIEW_IDS;
                for (int i = 1; i < MAX_VIEW_IDS; i++)
                {
                    newSubId = (newSubId + 1) % MAX_VIEW_IDS;
                    if (newSubId == 0)
                    {
                        continue;   // avoid using subID 0
                    }

                    newViewId = newSubId + ownerIdOffset;
                    if (!photonViewList.ContainsKey(newViewId))
                    {
                        lastUsedViewSubIdStatic = newSubId;
                        return newViewId;
                    }
                }
                throw new Exception(string.Format("AllocateViewID() failed. The room (user {0}) is out of 'room' viewIDs. It seems all available are in use.", ownerId));
            }
            else
            {
                int newSubId = lastUsedViewSubId;
                int newViewId;
                int ownerIdOffset = ownerId * MAX_VIEW_IDS;
                for (int i = 1; i <= MAX_VIEW_IDS; i++)
                {
                    newSubId = (newSubId + 1) % MAX_VIEW_IDS;
                    if (newSubId == 0)
                    {
                        continue;   // avoid using subID 0
                    }

                    newViewId = newSubId + ownerIdOffset;
                    if (!photonViewList.ContainsKey(newViewId))
                    {
                        lastUsedViewSubId = newSubId;
                        return newViewId;
                    }
                }

                throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of viewIDs. It seems all available are in use.", ownerId));
            }
        }


        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
        {
            if (CurrentRoom == null)
            {
                Debug.LogError("Can not Instantiate before the client joined/created a room. State: "+PhotonNetwork.NetworkClientState);
                return null;
            }

            Pun.InstantiateParameters netParams = new InstantiateParameters(prefabName, position, rotation, group, data, currentLevelPrefix, null, LocalPlayer, ServerTimestamp);
            return NetworkInstantiate(netParams, false);
        }

        [Obsolete("Renamed. Use InstantiateRoomObject instead")]
        public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
        {
            return InstantiateRoomObject(prefabName, position, rotation, group, data);
        }

        public static GameObject InstantiateRoomObject(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
        {
            if (CurrentRoom == null)
            {
                Debug.LogError("Can not Instantiate before the client joined/created a room.");
                return null;
            }

            if (LocalPlayer.IsMasterClient)
            {
                Pun.InstantiateParameters netParams = new InstantiateParameters(prefabName, position, rotation, group, data, currentLevelPrefix, null, LocalPlayer, ServerTimestamp);
                return NetworkInstantiate(netParams, true);
            }

            return null;
        }

        private static GameObject NetworkInstantiate(Hashtable networkEvent, Player creator)
        {
            string prefabName = (string)networkEvent[keyByteZero];
            int serverTime = (int)networkEvent[keyByteSix];
            int instantiationId = (int)networkEvent[keyByteSeven];

            Vector3 position;
            if (networkEvent.ContainsKey(keyByteOne))
            {
                position = (Vector3)networkEvent[keyByteOne];
            }
            else
            {
                position = Vector3.zero;
            }

            Quaternion rotation = Quaternion.identity;
            if (networkEvent.ContainsKey(keyByteTwo))
            {
                rotation = (Quaternion)networkEvent[keyByteTwo];
            }

            byte group = 0;
            if (networkEvent.ContainsKey(keyByteThree))
            {
                group = (byte)networkEvent[keyByteThree];
            }

            byte objLevelPrefix = 0;
            if (networkEvent.ContainsKey(keyByteEight))
            {
                objLevelPrefix = (byte)networkEvent[keyByteEight];
            }

            int[] viewsIDs;
            if (networkEvent.ContainsKey(keyByteFour))
            {
                viewsIDs = (int[])networkEvent[keyByteFour];
            }
            else
            {
                viewsIDs = new int[1] { instantiationId };
            }

            object[] incomingInstantiationData;
            if (networkEvent.ContainsKey(keyByteFive))
            {
                incomingInstantiationData = (object[])networkEvent[keyByteFive];
            }
            else
            {
                incomingInstantiationData = null;
            }
            if (group != 0 && !allowedReceivingGroups.Contains(group))
            {
                return null; // Ignore group
            }


            Pun.InstantiateParameters netParams = new InstantiateParameters(prefabName, position, rotation, group, incomingInstantiationData, objLevelPrefix, viewsIDs, creator, serverTime);
            return NetworkInstantiate(netParams, false, true);
        }


        private static readonly HashSet<string> PrefabsWithoutMagicCallback = new HashSet<string>();

        private static GameObject NetworkInstantiate(Pun.InstantiateParameters parameters, bool roomObject = false, bool instantiateEvent = false)
        {

            GameObject go = null;
            PhotonView[] photonViews;

            go = prefabPool.Instantiate(parameters.prefabName, parameters.position, parameters.rotation);


            if (go == null)
            {
                Debug.LogError("Failed to network-Instantiate: " + parameters.prefabName);
                return null;
            }

            if (go.activeSelf)
            {
                Debug.LogWarning("PrefabPool.Instantiate() should return an inactive GameObject. " + prefabPool.GetType().Name + " returned an active object. PrefabId: " + parameters.prefabName);
            }


            photonViews = go.GetPhotonViewsInChildren();


            if (photonViews.Length == 0)
            {
                Debug.LogError("PhotonNetwork.Instantiate() can only instantiate objects with a PhotonView component. This prefab does not have one: " + parameters.prefabName);
                return null;
            }

            bool localInstantiate = !instantiateEvent && LocalPlayer.Equals(parameters.creator);
            if (localInstantiate)
            {
                parameters.viewIDs = new int[photonViews.Length];
            }

            for (int i = 0; i < photonViews.Length; i++)
            {
                if (localInstantiate)
                {
                    parameters.viewIDs[i] = (roomObject) ? AllocateViewID(0) : AllocateViewID(parameters.creator.ActorNumber);
                }

                var view = photonViews[i];

                view.ViewID = 0;
                view.sceneViewId = 0;
                view.isRuntimeInstantiated = true;
                view.lastOnSerializeDataSent = null;
                view.lastOnSerializeDataReceived = null;
                view.Prefix = parameters.objLevelPrefix;
                view.InstantiationId = parameters.viewIDs[0];
                view.InstantiationData = parameters.data;
                view.ViewID = parameters.viewIDs[i];    // with didAwake true and viewID == 0, this will also register the view

                view.Group = parameters.group;
            }

            if (localInstantiate)
            {
                SendInstantiate(parameters, roomObject);
            }

            go.SetActive(true);
            if (!PrefabsWithoutMagicCallback.Contains(parameters.prefabName))
            {
                var list = go.GetComponents<IPunInstantiateMagicCallback>();
                if (list.Length > 0)
                {
                    PhotonMessageInfo pmi = new PhotonMessageInfo(parameters.creator, parameters.timestamp, photonViews[0]);
                    foreach (IPunInstantiateMagicCallback callbackComponent in list)
                    {
                        callbackComponent.OnPhotonInstantiate(pmi);
                    }
                }
                else
                {
                    PrefabsWithoutMagicCallback.Add(parameters.prefabName);
                }
            }

            return go;
        }


        private static readonly Hashtable SendInstantiateEvHashtable = new Hashtable();                             // SendInstantiate reuses this to reduce GC
        private static readonly RaiseEventOptions SendInstantiateRaiseEventOptions = new RaiseEventOptions();       // SendInstantiate reuses this to reduce GC

        internal static bool SendInstantiate(Pun.InstantiateParameters parameters, bool roomObject = false)
        {
            int instantiateId = parameters.viewIDs[0];   // LIMITS PHOTONVIEWS&PLAYERS

            SendInstantiateEvHashtable.Clear();     // SendInstantiate reuses this Hashtable to reduce GC

            SendInstantiateEvHashtable[keyByteZero] = parameters.prefabName;

            if (parameters.position != Vector3.zero)
            {
                SendInstantiateEvHashtable[keyByteOne] = parameters.position;
            }

            if (parameters.rotation != Quaternion.identity)
            {
                SendInstantiateEvHashtable[keyByteTwo] = parameters.rotation;
            }

            if (parameters.group != 0)
            {
                SendInstantiateEvHashtable[keyByteThree] = parameters.group;
            }
            if (parameters.viewIDs.Length > 1)
            {
                SendInstantiateEvHashtable[keyByteFour] = parameters.viewIDs; // LIMITS PHOTONVIEWS&PLAYERS
            }

            if (parameters.data != null)
            {
                SendInstantiateEvHashtable[keyByteFive] = parameters.data;
            }

            if (currentLevelPrefix > 0)
            {
                SendInstantiateEvHashtable[keyByteEight] = currentLevelPrefix;    // photonview's / object's level prefix
            }

            SendInstantiateEvHashtable[keyByteSix] = PhotonNetwork.ServerTimestamp;
            SendInstantiateEvHashtable[keyByteSeven] = instantiateId;


            SendInstantiateRaiseEventOptions.CachingOption = (roomObject) ? EventCaching.AddToRoomCacheGlobal : EventCaching.AddToRoomCache;

            return PhotonNetwork.RaiseEventInternal(PunEvent.Instantiation, SendInstantiateEvHashtable, SendInstantiateRaiseEventOptions, SendOptions.SendReliable);
        }
        public static void Destroy(PhotonView targetView)
        {
            if (targetView != null)
            {
                RemoveInstantiatedGO(targetView.gameObject, !InRoom);
            }
            else
            {
                Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
            }
        }
        public static void Destroy(GameObject targetGo)
        {
            RemoveInstantiatedGO(targetGo, !InRoom);
        }
        public static void DestroyPlayerObjects(Player targetPlayer)
        {
            if (targetPlayer == null)
            {
                Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
            }

            DestroyPlayerObjects(targetPlayer.ActorNumber);
        }
        public static void DestroyPlayerObjects(int targetPlayerId)
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }
            if (LocalPlayer.IsMasterClient || targetPlayerId == LocalPlayer.ActorNumber)
            {
                DestroyPlayerObjects(targetPlayerId, false);
            }
            else
            {
                Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + PhotonNetwork.IsMasterClient);
            }
        }
        public static void DestroyAll()
        {
            if (IsMasterClient)
            {
                DestroyAll(false);
            }
            else
            {
                Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
            }
        }
        public static void RemoveRPCs(Player targetPlayer)
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }

            if (!targetPlayer.IsLocal && !IsMasterClient)
            {
                Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
                return;
            }

            OpCleanActorRpcBuffer(targetPlayer.ActorNumber);
        }
        public static void RemoveRPCs(PhotonView targetPhotonView)
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }

            CleanRpcBufferIfMine(targetPhotonView);
        }
        internal static void RPC(PhotonView view, string methodName, RpcTarget target, bool encrypt, params object[] parameters)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                Debug.LogError("RPC method name cannot be null or empty.");
                return;
            }

            if (!VerifyCanUseNetwork())
            {
                return;
            }

            if (CurrentRoom == null)
            {
                Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
                return;
            }

            if (NetworkingClient != null)
            {
                RPC(view, methodName, target, null, encrypt, parameters);
            }
            else
            {
                Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
            }
        }
        internal static void RPC(PhotonView view, string methodName, Player targetPlayer, bool encrypt, params object[] parameters)
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }

            if (CurrentRoom == null)
            {
                Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
                return;
            }

            if (LocalPlayer == null)
            {
                Debug.LogError("RPC can't be sent to target Player being null! Did not send \"" + methodName + "\" call.");
            }

            if (NetworkingClient != null)
            {
                RPC(view, methodName, RpcTarget.Others, targetPlayer, encrypt, parameters);
            }
            else
            {
                Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
            }
        }
        public static void SetInterestGroups(byte group, bool enabled)
        {
            if (!VerifyCanUseNetwork())
            {
                return;
            }

            if (enabled)
            {
                byte[] groups = new byte[1] { (byte)group };
                SetInterestGroups(null, groups);
            }
            else
            {
                byte[] groups = new byte[1] { (byte)group };
                SetInterestGroups(groups, null);
            }
        }
        public static void LoadLevel(int levelNumber)
        {
            if (PhotonHandler.AppQuits)
            {
                return;
            }

            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelNumber);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelNumber,LoadSceneMode.Single);
        }
        public static void LoadLevel(string levelName)
        {
            if (PhotonHandler.AppQuits)
            {
                return;
            }

            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelName);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        }
        public static bool WebRpc(string name, object parameters, bool sendAuthCookie = false)
        {
            return NetworkingClient.OpWebRpc(name, parameters, sendAuthCookie);
        }
        private static void SetupLogging()
        {
            if (PhotonNetwork.LogLevel == PunLogLevel.ErrorsOnly)
            {
                PhotonNetwork.LogLevel = PhotonServerSettings.PunLogging;
            }
            if (PhotonNetwork.NetworkingClient.LoadBalancingPeer.DebugOut == DebugLevel.ERROR)
            {
                PhotonNetwork.NetworkingClient.LoadBalancingPeer.DebugOut = PhotonServerSettings.AppSettings.NetworkLogging;
            }
        }


        public static void LoadOrCreateSettings(bool reload = false)
        {
            if (reload)
            {
                photonServerSettings = null;    // PhotonEditor will use this to load and save the settings delayed
            }
            else if (photonServerSettings != null)
            {
                Debug.LogWarning("photonServerSettings is not null. Will not LoadOrCreateSettings().");
                return;
            }
            photonServerSettings = (ServerSettings)Resources.Load(PhotonNetwork.ServerSettingsFileName, typeof(ServerSettings));
            if (photonServerSettings != null)
            {
                return;
            }
            if (photonServerSettings == null)
            {
                photonServerSettings = (ServerSettings)ScriptableObject.CreateInstance("ServerSettings");
                if (photonServerSettings == null)
                {
                    Debug.LogError("Failed to create ServerSettings. PUN is unable to run this way. If you deleted it from the project, reload the Editor.");
                    return;
                }
            }


            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += delegate { LoadOrCreateSettings(true); };
                return;
            }

            string punResourcesDirectory = PhotonNetwork.FindPunAssetFolder() + "Resources/";
            string serverSettingsAssetPath = punResourcesDirectory + PhotonNetwork.ServerSettingsFileName + ".asset";
            string serverSettingsDirectory = Path.GetDirectoryName(serverSettingsAssetPath);

            if (!Directory.Exists(serverSettingsDirectory))
            {
                Directory.CreateDirectory(serverSettingsDirectory);
                AssetDatabase.ImportAsset(serverSettingsDirectory);
            }

            if (!File.Exists(serverSettingsAssetPath))
            {
                AssetDatabase.CreateAsset(photonServerSettings, serverSettingsAssetPath);
            }
            AssetDatabase.SaveAssets();
            EditorUserBuildSettings.development = true;
            #endif
        }


        #if UNITY_EDITOR
        public static string FindAssetPath(string asset)
        {
            string[] guids = AssetDatabase.FindAssets (asset, null);
            if (guids.Length != 1)
            {
                return string.Empty;
            } else
            {
                return AssetDatabase.GUIDToAssetPath (guids [0]);
            }
        }
        public static string FindPunAssetFolder()
        {
            string _thisPath =	FindAssetPath("PunClasses");
            string _PunFolderPath = string.Empty;
            string[] subdirectoryEntries = _thisPath.Split ('/');
            foreach (string dir in subdirectoryEntries)
            {
                if (!string.IsNullOrEmpty (dir))
                {
                    _PunFolderPath += dir +"/";

                    if (string.Equals (dir, "PhotonUnityNetworking"))
                    {
                        return _PunFolderPath;
                    }
                }
            }

            return "Assets/Photon/PhotonUnityNetworking/";
        }

        #endif

    }
}
