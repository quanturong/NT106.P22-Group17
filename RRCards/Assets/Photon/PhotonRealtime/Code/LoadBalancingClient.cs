
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Realtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY
    using UnityEngine;
    using Debug = UnityEngine.Debug;
    #endif
    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif


    #region Enums
    public enum ClientState
    {
        PeerCreated,
        Authenticating,
        Authenticated,
        JoiningLobby,
        JoinedLobby,
        DisconnectingFromMasterServer,
        [Obsolete("Renamed to DisconnectingFromMasterServer")]
        DisconnectingFromMasterserver = DisconnectingFromMasterServer,
        ConnectingToGameServer,
        [Obsolete("Renamed to ConnectingToGameServer")]
        ConnectingToGameserver = ConnectingToGameServer,
        ConnectedToGameServer,
        [Obsolete("Renamed to ConnectedToGameServer")]
        ConnectedToGameserver = ConnectedToGameServer,
        Joining,
        Joined,
        Leaving,
        DisconnectingFromGameServer,
        [Obsolete("Renamed to DisconnectingFromGameServer")]
        DisconnectingFromGameserver = DisconnectingFromGameServer,
        ConnectingToMasterServer,
        [Obsolete("Renamed to ConnectingToMasterServer.")]
        ConnectingToMasterserver = ConnectingToMasterServer,
        Disconnecting,
        Disconnected,
        ConnectedToMasterServer,
        [Obsolete("Renamed to ConnectedToMasterServer.")]
        ConnectedToMasterserver = ConnectedToMasterServer,
        [Obsolete("Renamed to ConnectedToMasterServer.")]
        ConnectedToMaster = ConnectedToMasterServer,
        ConnectingToNameServer,
        ConnectedToNameServer,
        DisconnectingFromNameServer,
        ConnectWithFallbackProtocol
    }
    internal enum JoinType
    {
        CreateRoom,
        JoinRoom,
        JoinRandomRoom,
        JoinRandomOrCreateRoom,
        JoinOrCreateRoom
    }
    public enum DisconnectCause
    {
        None,
        ExceptionOnConnect,
        DnsExceptionOnConnect,
        ServerAddressInvalid,
        Exception,
        SendException,
        ReceiveException,
        ServerTimeout,
        ClientTimeout,
        DisconnectByServerLogic,
        DisconnectByServerReasonUnknown,
        InvalidAuthentication,
        CustomAuthenticationFailed,
        AuthenticationTicketExpired,
        MaxCcuReached,
        InvalidRegion,
        OperationNotAllowedInCurrentState,
        DisconnectByClientLogic,
        DisconnectByOperationLimit,
        DisconnectByDisconnectMessage,
        ApplicationQuit
    }
    public enum ServerConnection
    {
        MasterServer,
        GameServer,
        NameServer
    }
    public enum ClientAppType
    {
        Realtime,
        Voice,
        Fusion
    }
    public enum EncryptionMode
    {
        PayloadEncryption,
        DatagramEncryptionGCM = 13,
    }
    public struct PhotonPortDefinition
    {
        public static readonly PhotonPortDefinition AlternativeUdpPorts = new PhotonPortDefinition() { NameServerPort = 27000, MasterServerPort = 27001, GameServerPort = 27002};
        public ushort NameServerPort;
        public ushort MasterServerPort;
        public ushort GameServerPort;
    }


    #endregion
    public class LoadBalancingClient : IPhotonPeerListener
    {
        public LoadBalancingPeer LoadBalancingPeer { get; private set; }

        #if PHOTON_LOCATION
        public LocationInfo LocationInfo
        {
            get { return this.RegionHandler?.Location?.LocationInfo; }
        }
        #endif
        public SerializationProtocol SerializationProtocol
        {
            get
            {
                return this.LoadBalancingPeer.SerializationProtocolType;
            }
            set
            {
                this.LoadBalancingPeer.SerializationProtocolType = value;
            }
        }
        public string AppVersion { get; set; }
        public string AppId { get; set; }
        public ClientAppType ClientType { get; set; }
        public AuthenticationValues AuthValues { get; set; }
        public AuthModeOption AuthMode = AuthModeOption.Auth;
        public EncryptionMode EncryptionMode = EncryptionMode.PayloadEncryption;
        public ConnectionProtocol? ExpectedProtocol { get; set; }
        private object TokenForInit
        {
            get
            {
                if (this.AuthMode == AuthModeOption.Auth)
                {
                    return null;
                }
                return (this.AuthValues != null) ? this.AuthValues.Token : null;
            }
        }
        private object tokenCache;
        public bool IsUsingNameServer { get; set; }
        public string NameServerHost = "ns.photonengine.io";
        public string NameServerAddress { get { return this.GetNameServerAddress(); } }
        private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>() { { ConnectionProtocol.Udp, 5058 }, { ConnectionProtocol.Tcp, 4533 }, { ConnectionProtocol.WebSocket, 80 }, { ConnectionProtocol.WebSocketSecure, 443 } };
        [Obsolete("Set port overrides in ServerPortOverrides. Not used anymore!")]
        public bool UseAlternativeUdpPorts { get; set; }
        public PhotonPortDefinition ServerPortOverrides;
        public bool EnableProtocolFallback { get; set; }
        public string CurrentServerAddress { get { return this.LoadBalancingPeer.ServerAddress; } }
        public string MasterServerAddress { get; set; }
        public string GameServerAddress { get; protected internal set; }
        public Func<string, ServerConnection, string> AddressRewriter;
        public ServerConnection Server { get; private set; }
        public string ProxyServerAddress;
        public int ConnectCount { get; private set; }
        private ClientState state = ClientState.PeerCreated;
        public ClientState State
        {
            get
            {
                return this.state;
            }

            set
            {
                if (this.state == value)
                {
                    return;
                }

                ClientState previousState = this.state;
                this.state = value;

                if (this.StateChanged != null)
                {
                    this.StateChanged(previousState, this.state);
                }
            }
        }
        public bool IsConnected { get { return this.LoadBalancingPeer != null && this.State != ClientState.PeerCreated && this.State != ClientState.Disconnected; } }
        public bool IsConnectedAndReady
        {
            get
            {
                if (this.LoadBalancingPeer == null)
                {
                    return false;
                }

                switch (this.State)
                {
                    case ClientState.PeerCreated:
                    case ClientState.Disconnected:
                    case ClientState.Disconnecting:
                    case ClientState.DisconnectingFromGameServer:
                    case ClientState.DisconnectingFromMasterServer:
                    case ClientState.DisconnectingFromNameServer:
                    case ClientState.Authenticating:
                    case ClientState.ConnectingToGameServer:
                    case ClientState.ConnectingToMasterServer:
                    case ClientState.ConnectingToNameServer:
                    case ClientState.Joining:
                    case ClientState.Leaving:
                        return false;   // we are not ready to execute any operations
                }

                return true;
            }
        }
        public event Action<ClientState, ClientState> StateChanged;
        public event Action<EventData> EventReceived;
        public event Action<OperationResponse> OpResponseReceived;
        public ConnectionCallbacksContainer ConnectionCallbackTargets;
        public MatchMakingCallbacksContainer MatchMakingCallbackTargets;
        internal InRoomCallbacksContainer InRoomCallbackTargets;
        internal LobbyCallbacksContainer LobbyCallbackTargets;
        internal WebRpcCallbacksContainer WebRpcCallbackTargets;
        internal ErrorInfoCallbacksContainer ErrorInfoCallbackTargets;
        public DisconnectCause DisconnectedCause { get; protected set; }
        public string DisconnectMessage;
        public bool TelemetryEnabled = false;
        #pragma warning disable CS0414
        private bool telemetrySent = false;
        #pragma warning restore CS0414
        public SystemConnectionSummary SystemConnectionSummary;
        public bool InLobby
        {
            get { return this.State == ClientState.JoinedLobby; }
        }
        public TypedLobby CurrentLobby { get; internal set; }
        public bool EnableLobbyStatistics;
        private readonly List<TypedLobbyInfo> lobbyStatistics = new List<TypedLobbyInfo>();
        public Player LocalPlayer { get; internal set; }
        public string NickName
        {
            get
            {
                return this.LocalPlayer.NickName;
            }

            set
            {
                if (this.LocalPlayer == null)
                {
                    return;
                }

                this.LocalPlayer.NickName = value;
            }
        }
        public string UserId
        {
            get
            {
                if (this.AuthValues != null)
                {
                    return this.AuthValues.UserId;
                }
                return null;
            }
            set
            {
                if (this.AuthValues == null)
                {
                    this.AuthValues = new AuthenticationValues();
                }
                this.AuthValues.UserId = value;
            }
        }
        public Room CurrentRoom { get; set; }
        public bool InRoom
        {
            get
            {
                return this.state == ClientState.Joined && this.CurrentRoom != null;
            }
        }
        public int PlayersOnMasterCount { get; internal set; }
        public int PlayersInRoomsCount { get; internal set; }
        public int RoomsCount { get; internal set; }
        private JoinType lastJoinType;
        private EnterRoomParams enterRoomParamsCache;
        private OperationResponse failedRoomEntryOperation;
        private const int FriendRequestListMax = 512;
        private string[] friendListRequested;
        public bool IsFetchingFriendList { get { return this.friendListRequested != null; } }
        public string CloudRegion { get; private set; }
        public string CurrentCluster { get; private set; }
        public RegionHandler RegionHandler;
        private string bestRegionSummaryFromStorage;
        public string SummaryToCache;
        private bool connectToBestRegion = true;
        private class EncryptionDataParameters
        {
            public const byte Mode = 0;
            public const byte Secret1 = 1;
            public const byte Secret2 = 2;
        }


        private class CallbackTargetChange
        {
            public readonly object Target;
            public readonly bool AddTarget;

            public CallbackTargetChange(object target, bool addTarget)
            {
                this.Target = target;
                this.AddTarget = addTarget;
            }
        }

        private readonly Queue<CallbackTargetChange> callbackTargetChanges = new Queue<CallbackTargetChange>();
        private readonly HashSet<object> callbackTargets = new HashSet<object>();
        public LoadBalancingClient(ConnectionProtocol protocol = ConnectionProtocol.Udp)
        {
            this.ConnectionCallbackTargets = new ConnectionCallbacksContainer(this);
            this.MatchMakingCallbackTargets = new MatchMakingCallbacksContainer(this);
            this.InRoomCallbackTargets = new InRoomCallbacksContainer(this);
            this.LobbyCallbackTargets = new LobbyCallbacksContainer(this);
            this.WebRpcCallbackTargets = new WebRpcCallbacksContainer(this);
            this.ErrorInfoCallbackTargets = new ErrorInfoCallbacksContainer(this);

            this.LoadBalancingPeer = new LoadBalancingPeer(this, protocol);
            this.LoadBalancingPeer.OnDisconnectMessage += this.OnDisconnectMessageReceived;
            this.SerializationProtocol = SerializationProtocol.GpBinaryV18;
            this.LocalPlayer = this.CreatePlayer(string.Empty, -1, true, null); //TODO: Check if we can do this later


            #if SUPPORTED_UNITY
            CustomTypesUnity.Register();
            #endif

            #if UNITY_WEBGL
            if (this.LoadBalancingPeer.TransportProtocol == ConnectionProtocol.Tcp || this.LoadBalancingPeer.TransportProtocol == ConnectionProtocol.Udp)
            {
                this.LoadBalancingPeer.Listener.DebugReturn(DebugLevel.WARNING, "WebGL requires WebSockets. Switching TransportProtocol to WebSocketSecure.");
                this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }
            #endif

            this.State = ClientState.PeerCreated;
        }
        public LoadBalancingClient(string masterAddress, string appId, string gameVersion, ConnectionProtocol protocol = ConnectionProtocol.Udp) : this(protocol)
        {
            this.MasterServerAddress = masterAddress;
            this.AppId = appId;
            this.AppVersion = gameVersion;
        }

        public int NameServerPortInAppSettings;
        private string GetNameServerAddress()
        {
            var protocolPort = 0;
            ProtocolToNameServerPort.TryGetValue(this.LoadBalancingPeer.TransportProtocol, out protocolPort);

            if (this.NameServerPortInAppSettings != 0)
            {
                this.DebugReturn(DebugLevel.INFO, string.Format("Using NameServerPortInAppSettings: {0}", this.NameServerPortInAppSettings));
                protocolPort = this.NameServerPortInAppSettings;
            }

            if (this.ServerPortOverrides.NameServerPort > 0)
            {
                protocolPort = this.ServerPortOverrides.NameServerPort;
            }

            return this.ToProtocolAddress(this.NameServerHost, protocolPort, this.LoadBalancingPeer.TransportProtocol);
        }
        private string ToProtocolAddress(string address, int port, ConnectionProtocol protocol)
        {
            string protocolScheme = String.Empty;

            switch (protocol)
            {
                case ConnectionProtocol.Udp:
                case ConnectionProtocol.Tcp:
                    return string.Format("{0}:{1}", address, port);

                case ConnectionProtocol.WebSocket:
                    protocolScheme = "ws://";
                    break;
                case ConnectionProtocol.WebSocketSecure:
                    protocolScheme = "wss://";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Can not handle protocol: {protocol}.");
            }

            Uri uri = new Uri(protocolScheme + address);
            string result = $"{uri.Scheme}://{uri.Host}:{port}{uri.AbsolutePath}";

            if (this.AddressRewriter != null)
            {
                result = this.AddressRewriter(result, ServerConnection.NameServer);
            }

            return result;
        }

        #region Operations and Commands
        public virtual bool ConnectUsingSettings(AppSettings appSettings)
        {
            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn(DebugLevel.WARNING, "ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: " + this.LoadBalancingPeer.PeerState);
                return false;
            }

            if (appSettings == null)
            {
                this.DebugReturn(DebugLevel.ERROR, "ConnectUsingSettings failed. The appSettings can't be null.'");
                return false;
            }

            switch (this.ClientType)
            {
                case ClientAppType.Realtime:
                    this.AppId = appSettings.AppIdRealtime;
                    break;
                case ClientAppType.Voice:
                    this.AppId = appSettings.AppIdVoice;
                    break;
                case ClientAppType.Fusion:
                    this.AppId = appSettings.AppIdFusion;
                    break;
            }

            this.AppVersion = appSettings.AppVersion;

            this.IsUsingNameServer = appSettings.UseNameServer;
            this.CloudRegion = appSettings.FixedRegion;
            this.connectToBestRegion = string.IsNullOrEmpty(this.CloudRegion);

            this.EnableLobbyStatistics = appSettings.EnableLobbyStatistics;
            this.LoadBalancingPeer.DebugOut = appSettings.NetworkLogging;

            this.AuthMode = appSettings.AuthMode;
            if (appSettings.AuthMode == AuthModeOption.AuthOnceWss)
            {
                this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
                this.ExpectedProtocol = appSettings.Protocol;
            }
            else
            {
                this.LoadBalancingPeer.TransportProtocol = appSettings.Protocol;
                this.ExpectedProtocol = null;
            }

            this.EnableProtocolFallback = appSettings.EnableProtocolFallback;

            this.bestRegionSummaryFromStorage = appSettings.BestRegionSummaryFromStorage;
            this.DisconnectedCause = DisconnectCause.None;
            this.DisconnectMessage = null;
            this.SystemConnectionSummary = null;


            this.CheckConnectSetupWebGl();


            if (this.IsUsingNameServer)
            {
                this.Server = ServerConnection.NameServer;
                if (!appSettings.IsDefaultNameServer)
                {
                    this.NameServerHost = appSettings.Server;
                }

                this.ProxyServerAddress = appSettings.ProxyServer;
                this.NameServerPortInAppSettings = appSettings.Port;

                if (!this.LoadBalancingPeer.Connect(this.NameServerAddress, this.ProxyServerAddress, this.AppId, this.TokenForInit))
                {
                    return false;
                }

                this.State = ClientState.ConnectingToNameServer;
            }
            else
            {
                this.Server = ServerConnection.MasterServer;
                int portToUse = appSettings.IsDefaultPort ? 5055 : appSettings.Port;    // TODO: setup new (default) port config

                this.MasterServerAddress = this.ToProtocolAddress(appSettings.Server, portToUse, this.LoadBalancingPeer.TransportProtocol);

                if (!this.LoadBalancingPeer.Connect(this.MasterServerAddress, this.ProxyServerAddress, this.AppId, this.TokenForInit))
                {
                    return false;
                }

                this.State = ClientState.ConnectingToMasterServer;
            }

            return true;
        }


        [Obsolete("Use ConnectToMasterServer() instead.")]
        public bool Connect()
        {
            return this.ConnectToMasterServer();
        }
        public virtual bool ConnectToMasterServer()
        {
            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn(DebugLevel.WARNING, "ConnectToMasterServer() failed. Can only connect while in state 'Disconnected'. Current state: " + this.LoadBalancingPeer.PeerState);
                return false;
            }
            if (this.AuthMode != AuthModeOption.Auth && this.TokenForInit == null)
            {
                this.DebugReturn(DebugLevel.ERROR, "Connect() failed. Can't connect to MasterServer with Token == null in AuthMode: " + this.AuthMode);
                return false;
            }

            this.CheckConnectSetupWebGl();

            this.DisconnectedCause = DisconnectCause.None;
            this.DisconnectMessage = null;
            this.SystemConnectionSummary = null;
            if (this.LoadBalancingPeer.Connect(this.MasterServerAddress, this.ProxyServerAddress, this.AppId, this.TokenForInit))
            {
                this.connectToBestRegion = false;
                this.State = ClientState.ConnectingToMasterServer;
                this.Server = ServerConnection.MasterServer;
                return true;
            }

            return false;
        }
        public bool ConnectToNameServer()
        {
            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn(DebugLevel.WARNING, "ConnectToNameServer() failed. Can only connect while in state 'Disconnected'. Current state: " + this.LoadBalancingPeer.PeerState);
                return false;
            }

            this.IsUsingNameServer = true;
            this.CloudRegion = null;


            this.CheckConnectSetupWebGl();


            if (this.AuthMode == AuthModeOption.AuthOnceWss)
            {
                if (this.ExpectedProtocol == null)
                {
                    this.ExpectedProtocol = this.LoadBalancingPeer.TransportProtocol;
                }
                this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }

            this.DisconnectedCause = DisconnectCause.None;
            this.DisconnectMessage = null;
            this.SystemConnectionSummary = null;
            if (this.LoadBalancingPeer.Connect(this.NameServerAddress, this.ProxyServerAddress, "NameServer", this.TokenForInit))
            {
                this.connectToBestRegion = false;
                this.State = ClientState.ConnectingToNameServer;
                this.Server = ServerConnection.NameServer;
                return true;
            }

            return false;
        }
        public bool ConnectToRegionMaster(string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                this.DebugReturn(DebugLevel.ERROR, "ConnectToRegionMaster() failed. The region can not be null or empty.");
                return false;
            }

            this.IsUsingNameServer = true;

            if (this.State == ClientState.Authenticating)
            {
                if (this.LoadBalancingPeer.DebugOut >= DebugLevel.INFO)
                {
                    this.DebugReturn(DebugLevel.INFO, "ConnectToRegionMaster() will skip calling authenticate, as the current state is 'Authenticating'. Just wait for the result.");
                }
                return true;
            }

            if (this.State == ClientState.ConnectedToNameServer)
            {
                this.CloudRegion = region;

                bool authenticating = this.CallAuthenticate();
                if (authenticating)
                {
                    this.State = ClientState.Authenticating;
                }

                return authenticating;
            }


            this.LoadBalancingPeer.Disconnect();
            this.CloudRegion = region;


            this.CheckConnectSetupWebGl();


            if (this.AuthMode == AuthModeOption.AuthOnceWss)
            {
                if (this.ExpectedProtocol == null)
                {
                    this.ExpectedProtocol = this.LoadBalancingPeer.TransportProtocol;
                }
                this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }

            this.connectToBestRegion = false;
            this.DisconnectedCause = DisconnectCause.None;
            this.DisconnectMessage = null;
            this.SystemConnectionSummary = null;
            if (!this.LoadBalancingPeer.Connect(this.NameServerAddress, this.ProxyServerAddress, "NameServer", null))
            {
                return false;
            }

            this.State = ClientState.ConnectingToNameServer;
            this.Server = ServerConnection.NameServer;
            return true;
        }

        [Conditional("UNITY_WEBGL")]
        private void CheckConnectSetupWebGl()
        {
            #if UNITY_WEBGL
            if (this.LoadBalancingPeer.TransportProtocol != ConnectionProtocol.WebSocket && this.LoadBalancingPeer.TransportProtocol != ConnectionProtocol.WebSocketSecure)
            {
                this.DebugReturn(DebugLevel.WARNING, "WebGL requires WebSockets. Switching TransportProtocol to WebSocketSecure.");
                this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }

            this.EnableProtocolFallback = false; // no fallback on WebGL
            #endif
        }
        private bool Connect(string serverAddress, string proxyServerAddress, ServerConnection serverType)
        {

            if (this.State == ClientState.Disconnecting)
            {
                this.DebugReturn(DebugLevel.ERROR, "Connect() failed. Can't connect while disconnecting (still). Current state: " + this.State);
                return false;
            }
            if (this.AuthMode != AuthModeOption.Auth && serverType != ServerConnection.NameServer && this.TokenForInit == null)
            {
                this.DebugReturn(DebugLevel.ERROR, "Connect() failed. Can't connect to " + serverType + " with Token == null in AuthMode: " + this.AuthMode);
                return false;
            }


            this.DisconnectedCause = DisconnectCause.None;
            this.SystemConnectionSummary = null;
            bool connecting = this.LoadBalancingPeer.Connect(serverAddress, proxyServerAddress, this.AppId, this.TokenForInit);
            if (connecting)
            {
                this.Server = serverType;

                switch (serverType)
                {
                    case ServerConnection.NameServer:
                        State = ClientState.ConnectingToNameServer;
                        break;
                    case ServerConnection.MasterServer:
                        State = ClientState.ConnectingToMasterServer;
                        break;
                    case ServerConnection.GameServer:
                        State = ClientState.ConnectingToGameServer;
                        break;
                }
            }

            return connecting;
        }
        public bool ReconnectToMaster()
        {
            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + this.LoadBalancingPeer.PeerState);
                return false;
            }

            if (string.IsNullOrEmpty(this.MasterServerAddress))
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectToMaster() failed. MasterServerAddress is null or empty.");
                return false;
            }
            if (this.tokenCache == null)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectToMaster() failed. It seems the client doesn't have any previous authentication token to re-connect.");
                return false;
            }

            if (this.AuthValues == null)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectToMaster() with AuthValues == null is not correct!");
                this.AuthValues = new AuthenticationValues();
            }
            this.AuthValues.Token = this.tokenCache;

            return this.Connect(this.MasterServerAddress, this.ProxyServerAddress, ServerConnection.MasterServer);
        }
        public bool ReconnectAndRejoin()
        {
            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: " + this.LoadBalancingPeer.PeerState);
                return false;
            }

            if (string.IsNullOrEmpty(this.GameServerAddress))
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectAndRejoin() failed. It seems the client wasn't connected to a game server before (no address).");
                return false;
            }
            if (this.enterRoomParamsCache == null)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectAndRejoin() failed. It seems the client doesn't have any previous room to re-join.");
                return false;
            }
            if (this.tokenCache == null)
            {
                this.DebugReturn(DebugLevel.WARNING, "ReconnectAndRejoin() failed. It seems the client doesn't have any previous authentication token to re-connect.");
                return false;
            }

            if (this.AuthValues == null)
            {
                this.AuthValues = new AuthenticationValues();
            }
            this.AuthValues.Token = this.tokenCache;


            if (!string.IsNullOrEmpty(this.GameServerAddress) && this.enterRoomParamsCache != null)
            {
                this.lastJoinType = JoinType.JoinRoom;
                this.enterRoomParamsCache.JoinMode = JoinMode.RejoinOnly;
                this.enterRoomParamsCache.Ticket = null;
                return this.Connect(this.GameServerAddress, this.ProxyServerAddress, ServerConnection.GameServer);
            }

            return false;
        }
        public void Disconnect()
        {
            this.Disconnect(DisconnectCause.DisconnectByClientLogic);
        }
        internal void Disconnect(DisconnectCause cause)
        {
            if (this.State == ClientState.Disconnecting || this.State == ClientState.PeerCreated)
            {
                this.DebugReturn(DebugLevel.INFO, "Disconnect() call gets skipped due to State " + this.State + ". DisconnectedCause: " + this.DisconnectedCause + " Parameter cause: " + cause);
                return;
            }

            if (this.State != ClientState.Disconnected)
            {
                this.State = ClientState.Disconnecting;
                this.DisconnectedCause = cause;
                this.LoadBalancingPeer.Disconnect();
            }
        }
        private void DisconnectToReconnect()
        {
            switch (this.Server)
            {
                case ServerConnection.NameServer:
                    this.State = ClientState.DisconnectingFromNameServer;
                    break;
                case ServerConnection.MasterServer:
                    this.State = ClientState.DisconnectingFromMasterServer;
                    break;
                case ServerConnection.GameServer:
                    this.State = ClientState.DisconnectingFromGameServer;
                    break;
            }

            this.LoadBalancingPeer.Disconnect();
        }
        public void SimulateConnectionLoss(bool simulateTimeout)
        {
            this.DebugReturn(DebugLevel.WARNING, "SimulateConnectionLoss() set to: "+simulateTimeout);

            if (simulateTimeout)
            {
                this.LoadBalancingPeer.NetworkSimulationSettings.IncomingLossPercentage = 100;
                this.LoadBalancingPeer.NetworkSimulationSettings.OutgoingLossPercentage = 100;
            }

            this.LoadBalancingPeer.IsSimulationEnabled = simulateTimeout;
        }

        private bool CallAuthenticate()
        {
            if (this.IsUsingNameServer && this.Server != ServerConnection.NameServer && (this.AuthValues == null || this.AuthValues.Token == null))
            {
                this.DebugReturn(DebugLevel.ERROR, "Authenticate without Token is only allowed on Name Server. Connecting to: " + this.Server + " on: " + this.CurrentServerAddress + ". State: " + this.State);
                return false;
            }

            if (this.AuthMode == AuthModeOption.Auth)
            {
                if (!this.CheckIfOpCanBeSent(OperationCode.Authenticate, this.Server, "Authenticate"))
                {
                    return false;
                }
                return this.LoadBalancingPeer.OpAuthenticate(this.AppId, this.AppVersion, this.AuthValues, this.CloudRegion, (this.EnableLobbyStatistics && this.Server == ServerConnection.MasterServer));
            }
            else
            {
                if (!this.CheckIfOpCanBeSent(OperationCode.AuthenticateOnce, this.Server, "AuthenticateOnce"))
                {
                    return false;
                }

                ConnectionProtocol targetProtocolPastNameServer = this.ExpectedProtocol != null ? (ConnectionProtocol) this.ExpectedProtocol : this.LoadBalancingPeer.TransportProtocol;
                return this.LoadBalancingPeer.OpAuthenticateOnce(this.AppId, this.AppVersion, this.AuthValues, this.CloudRegion, this.EncryptionMode, targetProtocolPastNameServer);
            }
        }
        public void Service()
        {
            if (this.LoadBalancingPeer != null)
            {
                this.LoadBalancingPeer.Service();
            }
        }
        private bool OpGetRegions()
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.GetRegions, this.Server, "GetRegions"))
            {
                return false;
            }

            bool sent = this.LoadBalancingPeer.OpGetRegions(this.AppId);
            return sent;
        }
        public bool OpFindFriends(string[] friendsToFind, FindFriendsOptions options = null)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.FindFriends, this.Server, "FindFriends"))
            {
                return false;
            }

            if (this.IsFetchingFriendList)
            {
                this.DebugReturn(DebugLevel.WARNING, "OpFindFriends skipped: already fetching friends list.");
                return false;   // fetching friends currently, so don't do it again (avoid changing the list while fetching friends)
            }

            if (friendsToFind == null || friendsToFind.Length == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpFindFriends skipped: friendsToFind array is null or empty.");
                return false;
            }

            if (friendsToFind.Length > FriendRequestListMax)
            {
                this.DebugReturn(DebugLevel.ERROR, string.Format("OpFindFriends skipped: friendsToFind array exceeds allowed length of {0}.", FriendRequestListMax));
                return false;
            }

            List<string> friendsList = new List<string>(friendsToFind.Length);
            for (int i = 0; i < friendsToFind.Length; i++)
            {
                string friendUserId = friendsToFind[i];
                if (string.IsNullOrEmpty(friendUserId))
                {
                    this.DebugReturn(DebugLevel.WARNING,
                        string.Format(
                            "friendsToFind array contains a null or empty UserId, element at position {0} skipped.",
                            i));
                }
                else if (friendUserId.Equals(UserId))
                {
                    this.DebugReturn(DebugLevel.WARNING,
                        string.Format(
                            "friendsToFind array contains local player's UserId \"{0}\", element at position {1} skipped.",
                            friendUserId,
                            i));
                }
                else if (friendsList.Contains(friendUserId))
                {
                    this.DebugReturn(DebugLevel.WARNING,
                        string.Format(
                            "friendsToFind array contains duplicate UserId \"{0}\", element at position {1} skipped.",
                            friendUserId,
                            i));
                }
                else
                {
                    friendsList.Add(friendUserId);
                }
            }

            if (friendsList.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpFindFriends skipped: friends list to find is empty.");
                return false;
            }

            string[] filteredArray = friendsList.ToArray();
            bool sent = this.LoadBalancingPeer.OpFindFriends(filteredArray, options);
            this.friendListRequested = sent ? filteredArray : null;

            return sent;
        }
        public bool OpJoinLobby(TypedLobby lobby)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinLobby, this.Server, "JoinLobby"))
            {
                return false;
            }

            if (lobby == null)
            {
                lobby = TypedLobby.Default;
            }
            bool sent = this.LoadBalancingPeer.OpJoinLobby(lobby);
            if (sent)
            {
                this.CurrentLobby = lobby;
                this.State = ClientState.JoiningLobby;
            }

            return sent;
        }
        public bool OpLeaveLobby()
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.LeaveLobby, this.Server, "LeaveLobby"))
            {
                return false;
            }
            return this.LoadBalancingPeer.OpLeaveLobby();
        }
        public bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams = null)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinRandomGame, this.Server, "JoinRandomGame"))
            {
                return false;
            }

            if (opJoinRandomRoomParams == null)
            {
                opJoinRandomRoomParams = new OpJoinRandomRoomParams();
            }

            this.enterRoomParamsCache = new EnterRoomParams();
            this.enterRoomParamsCache.Lobby = opJoinRandomRoomParams.TypedLobby;
            this.enterRoomParamsCache.ExpectedUsers = opJoinRandomRoomParams.ExpectedUsers;
            this.enterRoomParamsCache.Ticket = opJoinRandomRoomParams.Ticket;


            bool sending = this.LoadBalancingPeer.OpJoinRandomRoom(opJoinRandomRoomParams);
            if (sending)
            {
                this.lastJoinType = JoinType.JoinRandomRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpJoinRandomOrCreateRoom(OpJoinRandomRoomParams opJoinRandomRoomParams, EnterRoomParams createRoomParams)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinRandomGame, this.Server, "OpJoinRandomOrCreateRoom"))
            {
                return false;
            }

            if (opJoinRandomRoomParams == null)
            {
                opJoinRandomRoomParams = new OpJoinRandomRoomParams();
            }
            if (createRoomParams == null)
            {
                createRoomParams = new EnterRoomParams();
            }

            createRoomParams.JoinMode = JoinMode.CreateIfNotExists;
            this.enterRoomParamsCache = createRoomParams;
            this.enterRoomParamsCache.Lobby = opJoinRandomRoomParams.TypedLobby;
            this.enterRoomParamsCache.ExpectedUsers = opJoinRandomRoomParams.ExpectedUsers;
            if (opJoinRandomRoomParams.Ticket != null)
            {
                this.enterRoomParamsCache.Ticket = opJoinRandomRoomParams.Ticket;
            }


            bool sending = this.LoadBalancingPeer.OpJoinRandomOrCreateRoom(opJoinRandomRoomParams, createRoomParams);
            if (sending)
            {
                this.lastJoinType = JoinType.JoinRandomOrCreateRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpCreateRoom(EnterRoomParams enterRoomParams)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.CreateGame, this.Server, "CreateGame"))
            {
                return false;
            }
            bool onGameServer = this.Server == ServerConnection.GameServer;
            enterRoomParams.OnGameServer = onGameServer;
            if (!onGameServer)
            {
                this.enterRoomParamsCache = enterRoomParams;
            }

            bool sending = this.LoadBalancingPeer.OpCreateRoom(enterRoomParams);
            if (sending)
            {
                this.lastJoinType = JoinType.CreateRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpJoinOrCreateRoom(EnterRoomParams enterRoomParams)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinGame, this.Server, "JoinOrCreateRoom"))
            {
                return false;
            }

            bool onGameServer = this.Server == ServerConnection.GameServer;
            enterRoomParams.JoinMode = JoinMode.CreateIfNotExists;
            enterRoomParams.OnGameServer = onGameServer;
            if (!onGameServer)
            {
                this.enterRoomParamsCache = enterRoomParams;
            }

            bool sending = this.LoadBalancingPeer.OpJoinRoom(enterRoomParams);
            if (sending)
            {
                this.lastJoinType = JoinType.JoinOrCreateRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpJoinRoom(EnterRoomParams enterRoomParams)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinGame, this.Server, "JoinRoom"))
            {
                return false;
            }

            bool onGameServer = this.Server == ServerConnection.GameServer;
            enterRoomParams.OnGameServer = onGameServer;
            if (!onGameServer)
            {
                this.enterRoomParamsCache = enterRoomParams;
            }

            bool sending = this.LoadBalancingPeer.OpJoinRoom(enterRoomParams);
            if (sending)
            {
                this.lastJoinType = enterRoomParams.JoinMode == JoinMode.CreateIfNotExists ? JoinType.JoinOrCreateRoom : JoinType.JoinRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpRejoinRoom(string roomName, object ticket = null)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.JoinGame, this.Server, "RejoinRoom"))
            {
                return false;
            }

            bool onGameServer = this.Server == ServerConnection.GameServer;

            EnterRoomParams opParams = new EnterRoomParams();
            opParams.RoomName = roomName;
            opParams.OnGameServer = onGameServer;
            opParams.JoinMode = JoinMode.RejoinOnly;
            opParams.Ticket = ticket;
            this.enterRoomParamsCache = opParams;

            bool sending = this.LoadBalancingPeer.OpJoinRoom(opParams);
            if (sending)
            {
                this.lastJoinType = JoinType.JoinRoom;
                this.State = ClientState.Joining;
            }
            return sending;
        }
        public bool OpLeaveRoom(bool becomeInactive, bool sendAuthCookie = false)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.Leave, this.Server, "LeaveRoom"))
            {
                return false;
            }

            if (this.LoadBalancingPeer.OpLeaveRoom(becomeInactive, sendAuthCookie))
            {
                this.State = ClientState.Leaving;
                this.GameServerAddress = String.Empty;
                this.enterRoomParamsCache = null;
                return true;
            }

            return false;
        }
        public bool OpGetGameList(TypedLobby typedLobby, string sqlLobbyFilter)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.GetGameList, this.Server, "GetGameList"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(sqlLobbyFilter))
            {
                this.DebugReturn(DebugLevel.ERROR, "Operation GetGameList requires a filter.");
                return false;
            }
            if (typedLobby.Type != LobbyType.SqlLobby)
            {
                this.DebugReturn(DebugLevel.ERROR, "Operation GetGameList can only be used for lobbies of type SqlLobby.");
                return false;
            }

            return this.LoadBalancingPeer.OpGetGameList(typedLobby, sqlLobbyFilter);
        }
        public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable propertiesToSet, Hashtable expectedProperties = null, WebFlags webFlags = null)
        {
            if (propertiesToSet == null || propertiesToSet.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetCustomPropertiesOfActor() failed. propertiesToSet must not be null nor empty.");
                return false;
            }

            if (this.CurrentRoom == null)
            {
                if (expectedProperties == null && webFlags == null && this.LocalPlayer != null && this.LocalPlayer.ActorNumber == actorNr)
                {
                    return this.LocalPlayer.SetCustomProperties(propertiesToSet);
                }

                if (this.LoadBalancingPeer.DebugOut >= DebugLevel.ERROR)
                {
                    this.DebugReturn(DebugLevel.ERROR, "OpSetCustomPropertiesOfActor() failed. To use expectedProperties or webForward, you have to be in a room. State: " + this.State);
                }
                return false;
            }

            Hashtable customActorProperties = new Hashtable();
            customActorProperties.MergeStringKeys(propertiesToSet);
            if (customActorProperties.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetCustomPropertiesOfActor() failed. Only string keys allowed for custom properties.");
                return false;
            }
            return this.OpSetPropertiesOfActor(actorNr, customActorProperties, expectedProperties, webFlags);
        }
        protected internal bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, Hashtable expectedProperties = null, WebFlags webFlags = null)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.SetProperties, this.Server, "SetProperties"))
            {
                return false;
            }

            if (actorProperties == null || actorProperties.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetPropertiesOfActor() failed. actorProperties must not be null nor empty.");
                return false;
            }
            bool res = this.LoadBalancingPeer.OpSetPropertiesOfActor(actorNr, actorProperties, expectedProperties, webFlags);
            if (res && !this.CurrentRoom.BroadcastPropertiesChangeToAll && (expectedProperties == null || expectedProperties.Count == 0))
            {
                Player target = this.CurrentRoom.GetPlayer(actorNr);
                if (target != null)
                {
                    target.InternalCacheProperties(actorProperties);
                    this.InRoomCallbackTargets.OnPlayerPropertiesUpdate(target, actorProperties);
                }
            }
            return res;
        }
        public bool OpSetCustomPropertiesOfRoom(Hashtable propertiesToSet, Hashtable expectedProperties = null, WebFlags webFlags = null)
        {
            if (propertiesToSet == null || propertiesToSet.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetCustomPropertiesOfRoom() failed. propertiesToSet must not be null nor empty.");
                return false;
            }
            Hashtable customGameProps = new Hashtable();
            customGameProps.MergeStringKeys(propertiesToSet);
            if (customGameProps.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetCustomPropertiesOfRoom() failed. Only string keys are allowed for custom properties.");
                return false;
            }
            return this.OpSetPropertiesOfRoom(customGameProps, expectedProperties, webFlags);
        }


        protected internal bool OpSetPropertyOfRoom(byte propCode, object value)
        {
            Hashtable properties = new Hashtable();
            properties[propCode] = value;
            return this.OpSetPropertiesOfRoom(properties);
        }
        protected internal bool OpSetPropertiesOfRoom(Hashtable gameProperties, Hashtable expectedProperties = null, WebFlags webFlags = null)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.SetProperties, this.Server, "SetProperties"))
            {
                return false;
            }

            if (gameProperties == null || gameProperties.Count == 0)
            {
                this.DebugReturn(DebugLevel.ERROR, "OpSetPropertiesOfRoom() failed. gameProperties must not be null nor empty.");
                return false;
            }
            bool res = this.LoadBalancingPeer.OpSetPropertiesOfRoom(gameProperties, expectedProperties, webFlags);
            if (res && !this.CurrentRoom.BroadcastPropertiesChangeToAll && (expectedProperties == null || expectedProperties.Count == 0))
            {
                this.CurrentRoom.InternalCacheProperties(gameProperties);
                this.InRoomCallbackTargets.OnRoomPropertiesUpdate(gameProperties);
            }
            return res;
        }
        public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.RaiseEvent, this.Server, "RaiseEvent"))
            {
                return false;
            }

            return this.LoadBalancingPeer.OpRaiseEvent(eventCode, customEventContent, raiseEventOptions, sendOptions);
        }
        public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
        {
            if (!this.CheckIfOpCanBeSent(OperationCode.ChangeGroups, this.Server, "ChangeGroups"))
            {
                return false;
            }

            return this.LoadBalancingPeer.OpChangeGroups(groupsToRemove, groupsToAdd);
        }


        #endregion

        #region Helpers
        private void ReadoutProperties(Hashtable gameProperties, Hashtable actorProperties, int targetActorNr)
        {
            if (this.CurrentRoom != null && gameProperties != null)
            {
                this.CurrentRoom.InternalCacheProperties(gameProperties);
                if (this.InRoom)
                {
                    this.InRoomCallbackTargets.OnRoomPropertiesUpdate(gameProperties);
                }
            }

            if (actorProperties != null && actorProperties.Count > 0)
            {
                if (targetActorNr > 0)
                {
                    Player target = this.CurrentRoom.GetPlayer(targetActorNr);
                    if (target != null)
                    {
                        Hashtable props = this.ReadoutPropertiesForActorNr(actorProperties, targetActorNr);
                        target.InternalCacheProperties(props);
                        this.InRoomCallbackTargets.OnPlayerPropertiesUpdate(target, props);
                    }
                }
                else
                {
                    int actorNr;
                    Hashtable props;
                    string newName;
                    Player target;

                    foreach (object key in actorProperties.Keys)
                    {
                        actorNr = (int)key;
                        if (actorNr == 0)
                        {
                            continue;
                        }

                        props = (Hashtable)actorProperties[key];
                        newName = (string)props[ActorProperties.PlayerName];

                        target = this.CurrentRoom.GetPlayer(actorNr);
                        if (target == null)
                        {
                            target = this.CreatePlayer(newName, actorNr, false, props);
                            this.CurrentRoom.StorePlayer(target);
                        }
                        target.InternalCacheProperties(props);
                    }
                }
            }
        }
        private Hashtable ReadoutPropertiesForActorNr(Hashtable actorProperties, int actorNr)
        {
            if (actorProperties.ContainsKey(actorNr))
            {
                return (Hashtable)actorProperties[actorNr];
            }

            return actorProperties;
        }
        public void ChangeLocalID(int newId, bool applyUserId = false)
        {
            if (this.LocalPlayer == null)
            {
                this.DebugReturn(DebugLevel.ERROR, "loadBalancingClient.LocalPlayer is null. It should be set in constructor and not changed. Failed to ChangeLocalID.");
                return;
            }


            if (applyUserId && string.IsNullOrEmpty(this.LocalPlayer.UserId))
            {
                this.LocalPlayer.UserId = this.AuthValues == null || string.IsNullOrEmpty(this.AuthValues.UserId) ? new System.Guid().ToString() : this.AuthValues.UserId;
            }


            if (this.CurrentRoom == null)
            {
                this.LocalPlayer.ChangeLocalID(newId);
                this.LocalPlayer.RoomReference = null;
            }
            else
            {
                this.CurrentRoom.RemovePlayer(this.LocalPlayer);
                this.LocalPlayer.ChangeLocalID(newId);
                this.CurrentRoom.StorePlayer(this.LocalPlayer);
            }
        }
        private void GameEnteredOnGameServer(OperationResponse operationResponse)
        {
            this.CurrentRoom = this.CreateRoom(this.enterRoomParamsCache.RoomName, this.enterRoomParamsCache.RoomOptions);
            this.CurrentRoom.LoadBalancingClient = this;
            int localActorNr = (int)operationResponse[ParameterCode.ActorNr];
            this.ChangeLocalID(localActorNr);

            if (operationResponse.Parameters.ContainsKey(ParameterCode.ActorList))
            {
                int[] actorsInRoom = (int[])operationResponse.Parameters[ParameterCode.ActorList];
                this.UpdatedActorList(actorsInRoom);
            }


            Hashtable actorProperties = (Hashtable)operationResponse[ParameterCode.PlayerProperties];
            Hashtable gameProperties = (Hashtable)operationResponse[ParameterCode.GameProperties];
            this.ReadoutProperties(gameProperties, actorProperties, 0);

            object temp;
            if (operationResponse.Parameters.TryGetValue(ParameterCode.RoomOptionFlags, out temp))
            {
                this.CurrentRoom.InternalCacheRoomFlags((int)temp);
            }
            if (this.CurrentRoom.SuppressRoomEvents)
            {
                this.State = ClientState.Joined;
                this.LocalPlayer.UpdateNickNameOnJoined();


                if (this.lastJoinType == JoinType.CreateRoom || (this.lastJoinType == JoinType.JoinOrCreateRoom && this.LocalPlayer.ActorNumber == 1))
                {
                    this.MatchMakingCallbackTargets.OnCreatedRoom();
                }

                this.MatchMakingCallbackTargets.OnJoinedRoom();
            }
        }


        private void UpdatedActorList(int[] actorsInGame)
        {
            if (actorsInGame != null)
            {
                foreach (int actorNumber in actorsInGame)
                {
                    if (actorNumber == 0)
                    {
                        continue;
                    }

                    Player target = this.CurrentRoom.GetPlayer(actorNumber);
                    if (target == null)
                    {
                        this.CurrentRoom.StorePlayer(this.CreatePlayer(string.Empty, actorNumber, false, null));
                    }
                }
            }
        }
        protected internal virtual Player CreatePlayer(string actorName, int actorNumber, bool isLocal, Hashtable actorProperties)
        {
            Player newPlayer = new Player(actorName, actorNumber, isLocal, actorProperties);
            return newPlayer;
        }
        protected internal virtual Room CreateRoom(string roomName, RoomOptions opt)
        {
            Room r = new Room(roomName, opt);
            return r;
        }

        private bool CheckIfOpAllowedOnServer(byte opCode, ServerConnection serverConnection)
        {
            switch (serverConnection)
            {
                case ServerConnection.MasterServer:
                    switch (opCode)
                    {
                        case OperationCode.CreateGame:
                        case OperationCode.Authenticate:
                        case OperationCode.AuthenticateOnce:
                        case OperationCode.FindFriends:
                        case OperationCode.GetGameList:
                        case OperationCode.GetLobbyStats:
                        case OperationCode.JoinGame:
                        case OperationCode.JoinLobby:
                        case OperationCode.LeaveLobby:
                        case OperationCode.WebRpc:
                        case OperationCode.ServerSettings:
                        case OperationCode.JoinRandomGame:
                            return true;
                    }
                    break;
                case ServerConnection.GameServer:
                    switch (opCode)
                    {
                        case OperationCode.CreateGame:
                        case OperationCode.Authenticate:
                        case OperationCode.AuthenticateOnce:
                        case OperationCode.ChangeGroups:
                        case OperationCode.GetProperties:
                        case OperationCode.JoinGame:
                        case OperationCode.Leave:
                        case OperationCode.WebRpc:
                        case OperationCode.ServerSettings:
                        case OperationCode.SetProperties:
                        case OperationCode.RaiseEvent:
                            return true;
                    }
                    break;
                case ServerConnection.NameServer:
                    switch (opCode)
                    {
                        case OperationCode.Authenticate:
                        case OperationCode.AuthenticateOnce:
                        case OperationCode.GetRegions:
                        case OperationCode.ServerSettings:
                            return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("serverConnection", serverConnection, null);
            }
            return false;
        }

        private bool CheckIfOpCanBeSent(byte opCode, ServerConnection serverConnection, string opName)
        {
            if (this.LoadBalancingPeer == null)
            {
                this.DebugReturn(DebugLevel.ERROR, string.Format("Operation {0} ({1}) can't be sent because peer is null", opName, opCode));
                return false;
            }

            if (!this.CheckIfOpAllowedOnServer(opCode, serverConnection))
            {
                if (this.LoadBalancingPeer.DebugOut >= DebugLevel.ERROR)
                {
                    this.DebugReturn(DebugLevel.ERROR, string.Format("Operation {0} ({1}) not allowed on current server ({2})", opName, opCode, serverConnection));
                }
                return false;
            }

            if (!this.CheckIfClientIsReadyToCallOperation(opCode))
            {
                DebugLevel levelToReport = DebugLevel.ERROR;
                if (opCode == OperationCode.RaiseEvent && (this.State == ClientState.Leaving || this.State == ClientState.Disconnecting || this.State == ClientState.DisconnectingFromGameServer))
                {
                    levelToReport = DebugLevel.INFO;
                }

                if (this.LoadBalancingPeer.DebugOut >= levelToReport)
                {
                    this.DebugReturn(levelToReport, string.Format("Operation {0} ({1}) not called because client is not connected or not ready yet, client state: {2}", opName, opCode, Enum.GetName(typeof(ClientState), this.State)));
                }

                return false;
            }

            if (this.LoadBalancingPeer.PeerState != PeerStateValue.Connected)
            {
                this.DebugReturn(DebugLevel.ERROR, string.Format("Operation {0} ({1}) can't be sent because peer is not connected, peer state: {2}", opName, opCode, this.LoadBalancingPeer.PeerState));
                return false;
            }
            return true;
        }

        private bool CheckIfClientIsReadyToCallOperation(byte opCode)
        {
            switch (opCode)
            {

                case OperationCode.Authenticate:
                case OperationCode.AuthenticateOnce:
                    return this.IsConnectedAndReady ||
                         this.State == ClientState.ConnectingToNameServer || // this is required since we do not set state to ConnectedToNameServer before authentication
                        this.State == ClientState.ConnectingToMasterServer || // this is required since we do not set state to ConnectedToMasterServer before authentication
                        this.State == ClientState.ConnectingToGameServer; // this is required since we do not set state to ConnectedToGameServer before authentication

                case OperationCode.ChangeGroups:
                case OperationCode.GetProperties:
                case OperationCode.SetProperties:
                case OperationCode.RaiseEvent:
                case OperationCode.Leave:
                    return this.InRoom;

                case OperationCode.JoinGame:
                case OperationCode.CreateGame:
                    return this.State == ClientState.ConnectedToMasterServer || this.InLobby || this.State == ClientState.ConnectedToGameServer; // CurrentRoom can be not null in case of quick rejoin

                case OperationCode.LeaveLobby:
                    return this.InLobby;

                case OperationCode.JoinRandomGame:
                case OperationCode.FindFriends:
                case OperationCode.GetGameList:
                case OperationCode.GetLobbyStats: // do we need to be inside lobby to call this?
                case OperationCode.JoinLobby: // You don't have to explicitly leave a lobby to join another (client can be in one max, at any time)
                    return this.State == ClientState.ConnectedToMasterServer || this.InLobby;
                case OperationCode.GetRegions:
                    return this.State == ClientState.ConnectedToNameServer;
            }
            return this.IsConnected;
        }

        #if PHOTON_TELEMETRY
        public bool SendTelemetry()
        {
            if (!this.TelemetryEnabled || this.telemetrySent)
            {
                return false;
            }

            this.telemetrySent = true;
            TelemetryReport report = new TelemetryReport(this);
            report.Send();

            return true;
        }
        #endif

        #endregion

        #region Implementation of IPhotonPeerListener
        public virtual void DebugReturn(DebugLevel level, string message)
        {
            if (this.LoadBalancingPeer.DebugOut != DebugLevel.ALL && level > this.LoadBalancingPeer.DebugOut)
            {
                return;
            }
            #if !SUPPORTED_UNITY
            Debug.WriteLine(message);
            #else
            if (level == DebugLevel.ERROR)
            {
                Debug.LogError(message);
            }
            else if (level == DebugLevel.WARNING)
            {
                Debug.LogWarning(message);
            }
            else if (level == DebugLevel.INFO)
            {
                Debug.Log(message);
            }
            else if (level == DebugLevel.ALL)
            {
                Debug.Log(message);
            }
            #endif
        }

        private void CallbackRoomEnterFailed(OperationResponse operationResponse)
        {
            if (operationResponse.ReturnCode != 0)
            {
                if (operationResponse.OperationCode == OperationCode.JoinGame)
                {
                    this.MatchMakingCallbackTargets.OnJoinRoomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
                }
                else if (operationResponse.OperationCode == OperationCode.CreateGame)
                {
                    this.MatchMakingCallbackTargets.OnCreateRoomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
                }
                else if (operationResponse.OperationCode == OperationCode.JoinRandomGame)
                {
                    this.MatchMakingCallbackTargets.OnJoinRandomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
                }
            }
        }
        public virtual void OnOperationResponse(OperationResponse operationResponse)
        {
            if (operationResponse.Parameters.ContainsKey(ParameterCode.Token))
            {
                if (this.AuthValues == null)
                {
                    this.AuthValues = new AuthenticationValues();
                }

                this.AuthValues.Token = operationResponse.Parameters[ParameterCode.Token];
                this.tokenCache = this.AuthValues.Token;
            }
            if (operationResponse.ReturnCode == ErrorCode.OperationLimitReached)
            {
                this.Disconnect(DisconnectCause.DisconnectByOperationLimit);
            }

            switch (operationResponse.OperationCode)
            {
                case OperationCode.Authenticate:
                case OperationCode.AuthenticateOnce:
                    {
                        if (operationResponse.Parameters.ContainsKey(ParameterCode.ReportQos))
                        {
                            this.TelemetryEnabled = (bool)operationResponse[ParameterCode.ReportQos];
                        }

                        if (operationResponse.ReturnCode != 0)
                        {
                            this.DebugReturn(DebugLevel.ERROR, operationResponse.ToStringFull() + " Server: " + this.Server + " Address: " + this.LoadBalancingPeer.ServerAddress);

                            switch (operationResponse.ReturnCode)
                            {
                                case ErrorCode.InvalidAuthentication:
                                    this.DisconnectedCause = DisconnectCause.InvalidAuthentication;
                                    break;
                                case ErrorCode.CustomAuthenticationFailed:
                                    this.DisconnectedCause = DisconnectCause.CustomAuthenticationFailed;
                                    this.ConnectionCallbackTargets.OnCustomAuthenticationFailed(operationResponse.DebugMessage);
                                    break;
                                case ErrorCode.InvalidRegion:
                                    this.DisconnectedCause = DisconnectCause.InvalidRegion;
                                    break;
                                case ErrorCode.MaxCcuReached:
                                    this.DisconnectedCause = DisconnectCause.MaxCcuReached;
                                    break;
                                case ErrorCode.InvalidOperation:
                                case ErrorCode.OperationNotAllowedInCurrentState:
                                    this.DisconnectedCause = DisconnectCause.OperationNotAllowedInCurrentState;
                                    break;
                                case ErrorCode.AuthenticationTicketExpired:
                                    this.DisconnectedCause = DisconnectCause.AuthenticationTicketExpired;
                                    break;
                            }

                            this.DisconnectMessage = $"Op: {operationResponse.OperationCode} ReturnCode: {operationResponse.ReturnCode} '{operationResponse.DebugMessage}'";
                            this.Disconnect(this.DisconnectedCause);
                            break;  // if auth didn't succeed, we disconnect (above) and exit this operation's handling
                        }

                        if (this.Server == ServerConnection.NameServer || this.Server == ServerConnection.MasterServer)
                        {
                            if (operationResponse.Parameters.ContainsKey(ParameterCode.UserId))
                            {
                                string incomingId = (string)operationResponse.Parameters[ParameterCode.UserId];
                                if (!string.IsNullOrEmpty(incomingId))
                                {
                                    this.UserId = incomingId;
                                    this.LocalPlayer.UserId = incomingId;
                                    this.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", this.UserId));
                                }
                            }
                            if (operationResponse.Parameters.ContainsKey(ParameterCode.NickName))
                            {
                                this.NickName = (string)operationResponse.Parameters[ParameterCode.NickName];
                                this.DebugReturn(DebugLevel.INFO, string.Format("Received your NickName from server. Updating local value to: {0}", this.NickName));
                            }

                            if (operationResponse.Parameters.ContainsKey(ParameterCode.EncryptionData))
                            {
                                this.SetupEncryption((Dictionary<byte, object>)operationResponse.Parameters[ParameterCode.EncryptionData]);
                            }
                        }

                        if (this.Server == ServerConnection.NameServer)
                        {
                            if (this.AuthMode == AuthModeOption.AuthOnceWss && this.ExpectedProtocol != null)
                            {
                                this.DebugReturn(DebugLevel.INFO, string.Format("AuthOnceWss mode. Auth response switches TransportProtocol to ExpectedProtocol: {0}.", this.ExpectedProtocol));
                                this.LoadBalancingPeer.TransportProtocol = (ConnectionProtocol)this.ExpectedProtocol;
                                this.ExpectedProtocol = null;
                            }

                            string receivedCluster = operationResponse[ParameterCode.Cluster] as string;
                            if (!string.IsNullOrEmpty(receivedCluster))
                            {
                                this.CurrentCluster = receivedCluster;
                            }
                            this.MasterServerAddress = operationResponse[ParameterCode.Address] as string;
                            if (this.ServerPortOverrides.MasterServerPort != 0)
                            {
                                this.MasterServerAddress = ReplacePortWithAlternative(this.MasterServerAddress, this.ServerPortOverrides.MasterServerPort);
                            }
                            if (this.AddressRewriter != null)
                            {
                                this.MasterServerAddress = this.AddressRewriter(this.MasterServerAddress, ServerConnection.MasterServer);
                            }

                            this.DisconnectToReconnect();
                        }
                        else if (this.Server == ServerConnection.MasterServer)
                        {
                            this.State = ClientState.ConnectedToMasterServer;
                            if (this.failedRoomEntryOperation == null)
                            {
                                this.ConnectionCallbackTargets.OnConnectedToMaster();
                            }
                            else
                            {
                                this.CallbackRoomEnterFailed(this.failedRoomEntryOperation);
                                this.failedRoomEntryOperation = null;
                            }

                            if (this.AuthMode != AuthModeOption.Auth)
                            {
                                this.LoadBalancingPeer.OpSettings(this.EnableLobbyStatistics);
                            }
                        }
                        else if (this.Server == ServerConnection.GameServer)
                        {
                            this.State = ClientState.Joining;

                            if (this.enterRoomParamsCache.JoinMode == JoinMode.RejoinOnly)
                            {
                                this.enterRoomParamsCache.PlayerProperties = null;
                            }
                            else
                            {
                                Hashtable allProps = new Hashtable();
                                allProps.Merge(this.LocalPlayer.CustomProperties);

                                if (!string.IsNullOrEmpty(this.LocalPlayer.NickName))
                                {
                                    allProps[ActorProperties.PlayerName] = this.LocalPlayer.NickName;
                                }

                                this.enterRoomParamsCache.PlayerProperties = allProps;
                            }

                            this.enterRoomParamsCache.OnGameServer = true;

                            if (this.lastJoinType == JoinType.JoinRoom || this.lastJoinType == JoinType.JoinRandomRoom  || this.lastJoinType == JoinType.JoinRandomOrCreateRoom || this.lastJoinType == JoinType.JoinOrCreateRoom)
                            {
                                this.LoadBalancingPeer.OpJoinRoom(this.enterRoomParamsCache);
                            }
                            else if (this.lastJoinType == JoinType.CreateRoom)
                            {
                                this.LoadBalancingPeer.OpCreateRoom(this.enterRoomParamsCache);
                            }
                            break;
                        }
                        Dictionary<string, object> data = (Dictionary<string, object>)operationResponse[ParameterCode.Data];
                        if (data != null)
                        {
                            this.ConnectionCallbackTargets.OnCustomAuthenticationResponse(data);
                        }
                        break;
                    }

                case OperationCode.GetRegions:

                    if (operationResponse.ReturnCode == ErrorCode.InvalidAuthentication)
                    {
                        this.DebugReturn(DebugLevel.ERROR, string.Format("GetRegions failed. AppId is unknown on the (cloud) server. "+operationResponse.DebugMessage));
                        this.Disconnect(DisconnectCause.InvalidAuthentication);
                        break;
                    }
                    if (operationResponse.ReturnCode != ErrorCode.Ok)
                    {
                        this.DebugReturn(DebugLevel.ERROR, "GetRegions failed. Can't provide regions list. ReturnCode: " + operationResponse.ReturnCode + ": " + operationResponse.DebugMessage);
                        this.Disconnect(DisconnectCause.InvalidAuthentication);
                        break;
                    }
                    if (this.RegionHandler == null)
                    {
                        this.RegionHandler = new RegionHandler(this.ServerPortOverrides.MasterServerPort);
                    }

                    if (this.RegionHandler.IsPinging)
                    {
                        this.DebugReturn(DebugLevel.WARNING, "Received an response for OpGetRegions while the RegionHandler is pinging regions already. Skipping this response in favor of completing the current region-pinging.");
                        return; // in this particular case, we suppress the duplicate GetRegion response. we don't want a callback for this, cause there is a warning already.
                    }

                    this.RegionHandler.SetRegions(operationResponse, this);
                    this.ConnectionCallbackTargets.OnRegionListReceived(this.RegionHandler);

                    if (this.connectToBestRegion)
                    {
                        this.RegionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, this.bestRegionSummaryFromStorage);
                    }
                    break;

                case OperationCode.JoinRandomGame:  // this happens only on the master server. on gameserver this is a "regular" join
                case OperationCode.CreateGame:
                case OperationCode.JoinGame:

                    if (operationResponse.ReturnCode != 0)
                    {
                        if (this.Server == ServerConnection.GameServer)
                        {
                            this.failedRoomEntryOperation = operationResponse;
                            this.DisconnectToReconnect();
                        }
                        else
                        {
                            this.State = (this.InLobby) ? ClientState.JoinedLobby : ClientState.ConnectedToMasterServer;
                            this.CallbackRoomEnterFailed(operationResponse);
                        }
                    }
                    else
                    {
                        if (this.Server == ServerConnection.GameServer)
                        {
                            this.GameEnteredOnGameServer(operationResponse);
                        }
                        else
                        {
                            this.GameServerAddress = (string)operationResponse[ParameterCode.Address];
                            if (this.ServerPortOverrides.GameServerPort != 0)
                            {
                                this.GameServerAddress = ReplacePortWithAlternative(this.GameServerAddress, this.ServerPortOverrides.GameServerPort);
                            }
                            if (this.AddressRewriter != null)
                            {
                                this.GameServerAddress = this.AddressRewriter(this.GameServerAddress, ServerConnection.GameServer);
                            }

                            string roomName = operationResponse[ParameterCode.RoomName] as string;
                            if (!string.IsNullOrEmpty(roomName))
                            {
                                this.enterRoomParamsCache.RoomName = roomName;
                            }

                            this.DisconnectToReconnect();
                        }
                    }
                    break;

                case OperationCode.GetGameList:
                    if (operationResponse.ReturnCode != 0)
                    {
                        this.DebugReturn(DebugLevel.ERROR, "GetGameList failed: " + operationResponse.ToStringFull());
                        break;
                    }

                    List<RoomInfo> _RoomInfoList = new List<RoomInfo>();

                    Hashtable games = (Hashtable)operationResponse[ParameterCode.GameList];
                    foreach (string gameName in games.Keys)
                    {
                        _RoomInfoList.Add(new RoomInfo(gameName, (Hashtable)games[gameName]));
                    }

                    this.LobbyCallbackTargets.OnRoomListUpdate(_RoomInfoList);
                    break;

                case OperationCode.JoinLobby:
                    this.State = ClientState.JoinedLobby;
                    this.LobbyCallbackTargets.OnJoinedLobby();
                    break;

                case OperationCode.LeaveLobby:
                    this.State = ClientState.ConnectedToMasterServer;
                    this.LobbyCallbackTargets.OnLeftLobby();
                    break;

                case OperationCode.Leave:
                    this.DisconnectToReconnect();
                    break;

                case OperationCode.FindFriends:
                    if (operationResponse.ReturnCode != 0)
                    {
                        this.DebugReturn(DebugLevel.ERROR, "OpFindFriends failed: " + operationResponse.ToStringFull());
                        this.friendListRequested = null;
                        break;
                    }

                    bool[] onlineList = operationResponse[ParameterCode.FindFriendsResponseOnlineList] as bool[];
                    string[] roomList = operationResponse[ParameterCode.FindFriendsResponseRoomIdList] as string[];

                    List<FriendInfo> friendList = new List<FriendInfo>(this.friendListRequested.Length);
                    for (int index = 0; index < this.friendListRequested.Length; index++)
                    {
                        FriendInfo friend = new FriendInfo();
                        friend.UserId = this.friendListRequested[index];
                        friend.Room = roomList[index];
                        friend.IsOnline = onlineList[index];
                        friendList.Insert(index, friend);
                    }

                    this.friendListRequested = null;

                    this.MatchMakingCallbackTargets.OnFriendListUpdate(friendList);
                    break;

                case OperationCode.WebRpc:
                    this.WebRpcCallbackTargets.OnWebRpcResponse(operationResponse);
                    break;
            }

            if (this.OpResponseReceived != null) this.OpResponseReceived(operationResponse);
        }
        public virtual void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    this.ConnectCount++;
                    this.telemetrySent = false;

                    if (this.State == ClientState.ConnectingToNameServer)
                    {
                        if (this.LoadBalancingPeer.DebugOut >= DebugLevel.ALL)
                        {
                            this.DebugReturn(DebugLevel.ALL, "Connected to nameserver.");
                        }

                        this.Server = ServerConnection.NameServer;
                        if (this.AuthValues != null)
                        {
                            this.AuthValues.Token = null; // when connecting to NameServer, invalidate the secret (only)
                        }
                    }

                    if (this.State == ClientState.ConnectingToGameServer)
                    {
                        if (this.LoadBalancingPeer.DebugOut >= DebugLevel.ALL)
                        {
                            this.DebugReturn(DebugLevel.ALL, "Connected to gameserver.");
                        }

                        this.Server = ServerConnection.GameServer;
                    }

                    if (this.State == ClientState.ConnectingToMasterServer)
                    {
                        if (this.LoadBalancingPeer.DebugOut >= DebugLevel.ALL)
                        {
                            this.DebugReturn(DebugLevel.ALL, "Connected to masterserver.");
                        }

                        this.Server = ServerConnection.MasterServer;
                        this.ConnectionCallbackTargets.OnConnected(); // if initial connect
                    }


                    if (this.LoadBalancingPeer.TransportProtocol != ConnectionProtocol.WebSocketSecure)
                    {
                        if (this.Server == ServerConnection.NameServer || this.AuthMode == AuthModeOption.Auth)
                        {
                            this.LoadBalancingPeer.EstablishEncryption();
                        }
                    }
                    else
                    {
                        goto case StatusCode.EncryptionEstablished;
                    }

                    break;

                case StatusCode.EncryptionEstablished:
                    if (this.Server == ServerConnection.NameServer)
                    {
                        this.State = ClientState.ConnectedToNameServer;
                        if (string.IsNullOrEmpty(this.CloudRegion))
                        {
                            this.OpGetRegions();
                            break;
                        }
                    }
                    else
                    {
                        if (this.AuthMode == AuthModeOption.AuthOnce || this.AuthMode == AuthModeOption.AuthOnceWss)
                        {
                            break;
                        }
                    }
                    bool authenticating = this.CallAuthenticate();
                    if (authenticating)
                    {
                        this.State = ClientState.Authenticating;
                    }
                    else
                    {
                        this.DebugReturn(DebugLevel.ERROR, "OpAuthenticate failed. Check log output and AuthValues. State: " + this.State);
                    }

                    break;

                case StatusCode.Disconnect:
                    this.friendListRequested = null;

                    bool wasInRoom = this.CurrentRoom != null;
                    this.CurrentRoom = null; // players get cleaned up inside this, too, except LocalPlayer (which we keep)
                    this.ChangeLocalID(-1);  // depends on this.CurrentRoom, so it must be called after updating that

                    if (this.Server == ServerConnection.GameServer && wasInRoom)
                    {
                        this.MatchMakingCallbackTargets.OnLeftRoom();
                    }

                    if (this.ExpectedProtocol != null && this.LoadBalancingPeer.TransportProtocol != this.ExpectedProtocol)
                    {
                        this.DebugReturn(DebugLevel.INFO, string.Format("On disconnect switches TransportProtocol to ExpectedProtocol: {0}.", this.ExpectedProtocol));
                        this.LoadBalancingPeer.TransportProtocol = (ConnectionProtocol)this.ExpectedProtocol;
                        this.ExpectedProtocol = null;
                    }

                    switch (this.State)
                    {
                        case ClientState.ConnectWithFallbackProtocol:
                            this.EnableProtocolFallback = false;                    // the client does a fallback only one time
                            this.LoadBalancingPeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
                            this.NameServerPortInAppSettings = 0;                  // this does not affect the ServerSettings file, just a variable at runtime
                            this.ServerPortOverrides = new PhotonPortDefinition(); // use default ports for the fallback

                            if (!this.LoadBalancingPeer.Connect(this.NameServerAddress, this.ProxyServerAddress, this.AppId, this.TokenForInit))
                            {
                                return;
                            }

                            this.State = ClientState.ConnectingToNameServer;
                            break;
                        case ClientState.PeerCreated:
                        case ClientState.Disconnecting:
                            if (this.AuthValues != null)
                            {
                                this.AuthValues.Token = null; // when leaving the server, invalidate the secret (but not the auth values)
                            }

                            #if PHOTON_TELEMETRY
                            this.SendTelemetry();
                            #endif

                            this.State = ClientState.Disconnected;
                            this.ConnectionCallbackTargets.OnDisconnected(this.DisconnectedCause);
                            break;

                        case ClientState.DisconnectingFromGameServer:
                        case ClientState.DisconnectingFromNameServer:
                            this.ConnectToMasterServer();                 // this gets the client back to the Master Server
                            break;

                        case ClientState.DisconnectingFromMasterServer:
                            this.Connect(this.GameServerAddress, this.ProxyServerAddress, ServerConnection.GameServer);     // this connects the client with the Game Server (when joining/creating a room)
                            break;

                        case ClientState.Disconnected:
                            break;

                        default:
                            string stacktrace = "";
                            #if DEBUG && !NETFX_CORE
                            stacktrace = new System.Diagnostics.StackTrace(true).ToString();
                            #endif
                            this.DebugReturn(DebugLevel.WARNING, "Got a unexpected Disconnect in LoadBalancingClient State: " + this.State + ". Server: " + this.Server + " Trace: " + stacktrace);

                            if (this.AuthValues != null)
                            {
                                this.AuthValues.Token = null; // when leaving the server, invalidate the secret (but not the auth values)
                            }

                            this.State = ClientState.Disconnected;
                            this.ConnectionCallbackTargets.OnDisconnected(this.DisconnectedCause);
                            break;
                    }
                    break;

                case StatusCode.DisconnectByServerUserLimit:
                    this.DebugReturn(DebugLevel.ERROR, "This connection was rejected due to the apps CCU limit.");
                    this.DisconnectedCause = DisconnectCause.MaxCcuReached;
                    this.State = ClientState.Disconnecting;
                    break;
                case StatusCode.DnsExceptionOnConnect:
                    this.DisconnectedCause = DisconnectCause.DnsExceptionOnConnect;
                    this.State = ClientState.Disconnecting;
                    break;
                case StatusCode.ServerAddressInvalid:
                    this.DisconnectedCause = DisconnectCause.ServerAddressInvalid;
                    this.State = ClientState.Disconnecting;
                    break;


                case StatusCode.ExceptionOnConnect:
                case StatusCode.SecurityExceptionOnConnect:
                case StatusCode.EncryptionFailedToEstablish:
                    this.SystemConnectionSummary = new SystemConnectionSummary(this);
                    this.DebugReturn(DebugLevel.ERROR, $"Connection lost. OnStatusChanged to {statusCode}. Client state was: {this.State}. {this.SystemConnectionSummary.ToString()}");

                    this.DisconnectedCause = DisconnectCause.ExceptionOnConnect;
                    ClientState nextState = ClientState.Disconnecting;
                    if (this.State == ClientState.ConnectingToNameServer)
                    {
                        if (this.EnableProtocolFallback && this.LoadBalancingPeer.UsedProtocol != ConnectionProtocol.WebSocketSecure)
                        {
                            nextState = ClientState.ConnectWithFallbackProtocol;
                        }
                    }

                    this.State = nextState;
                    break;

                case StatusCode.Exception:
                case StatusCode.ExceptionOnReceive:
                case StatusCode.SendError:
                    this.SystemConnectionSummary = new SystemConnectionSummary(this);
                    this.DebugReturn(DebugLevel.ERROR, $"Connection lost. OnStatusChanged to {statusCode}. Client state was: {this.State}. {this.SystemConnectionSummary.ToString()}");

                    this.DisconnectedCause = DisconnectCause.Exception;
                    this.State = ClientState.Disconnecting;
                    break;


                case StatusCode.DisconnectByServerTimeout:
                    this.SystemConnectionSummary = new SystemConnectionSummary(this);
                    this.DebugReturn(DebugLevel.ERROR, $"Connection lost. OnStatusChanged to {statusCode}. Client state was: {this.State}. {this.SystemConnectionSummary.ToString()}");

                    this.DisconnectedCause = DisconnectCause.ServerTimeout; // could check if app was in background (and is now back)
                    this.State = ClientState.Disconnecting;
                    break;

                case StatusCode.TimeoutDisconnect:
                    this.SystemConnectionSummary = new SystemConnectionSummary(this);
                    this.DebugReturn(DebugLevel.ERROR, $"Connection lost. OnStatusChanged to {statusCode}. Client state was: {this.State}. {this.SystemConnectionSummary.ToString()}");

                    this.DisconnectedCause = DisconnectCause.ClientTimeout;
                    nextState = ClientState.Disconnecting;
                    if (this.State == ClientState.ConnectingToNameServer)
                    {
                        if (this.EnableProtocolFallback && this.LoadBalancingPeer.UsedProtocol != ConnectionProtocol.WebSocketSecure)
                        {
                            nextState = ClientState.ConnectWithFallbackProtocol;
                        }
                    }

                    this.State = nextState;
                    break;


                case StatusCode.DisconnectByServerLogic:
                    this.DisconnectedCause = DisconnectCause.DisconnectByServerLogic;
                    this.State = ClientState.Disconnecting;
                    break;
                case StatusCode.DisconnectByServerReasonUnknown:
                    this.DisconnectedCause = DisconnectCause.DisconnectByServerReasonUnknown;
                    this.State = ClientState.Disconnecting;
                    break;
            }
        }
        public virtual void OnEvent(EventData photonEvent)
        {
            int actorNr = photonEvent.Sender;
            Player originatingPlayer = (this.CurrentRoom != null) ? this.CurrentRoom.GetPlayer(actorNr) : null;

            switch (photonEvent.Code)
            {
                case EventCode.GameList:
                case EventCode.GameListUpdate:
                    List<RoomInfo> _RoomInfoList = new List<RoomInfo>();

                    Hashtable games = (Hashtable)photonEvent[ParameterCode.GameList];
                    foreach (string gameName in games.Keys)
                    {
                        _RoomInfoList.Add(new RoomInfo(gameName, (Hashtable)games[gameName]));
                    }

                    this.LobbyCallbackTargets.OnRoomListUpdate(_RoomInfoList);

                    break;

                case EventCode.Join:
                    Hashtable actorProperties = (Hashtable)photonEvent[ParameterCode.PlayerProperties];

                    if (originatingPlayer == null)
                    {
                        if (actorNr > 0)
                        {
                            originatingPlayer = this.CreatePlayer(string.Empty, actorNr, false, actorProperties);
                            this.CurrentRoom.StorePlayer(originatingPlayer);
                        }
                    }
                    else
                    {
                        originatingPlayer.InternalCacheProperties(actorProperties);
                        originatingPlayer.IsInactive = false;
                        originatingPlayer.HasRejoined = actorNr != this.LocalPlayer.ActorNumber;    // event is for non-local player, who is known (by ActorNumber), so it's a returning player
                    }

                    if (actorNr == this.LocalPlayer.ActorNumber)
                    {
                        int[] actorsInRoom = (int[])photonEvent[ParameterCode.ActorList];
                        this.UpdatedActorList(actorsInRoom);
                        originatingPlayer.HasRejoined = this.enterRoomParamsCache != null && this.enterRoomParamsCache.JoinMode == JoinMode.RejoinOnly;

                        this.State = ClientState.Joined;
                        this.LocalPlayer.UpdateNickNameOnJoined();
                        if (this.lastJoinType == JoinType.CreateRoom || (this.lastJoinType == JoinType.JoinOrCreateRoom && this.LocalPlayer.ActorNumber == 1))
                        {
                            this.MatchMakingCallbackTargets.OnCreatedRoom();
                        }

                        this.MatchMakingCallbackTargets.OnJoinedRoom();
                    }
                    else
                    {
                        this.InRoomCallbackTargets.OnPlayerEnteredRoom(originatingPlayer);
                    }
                    break;

                case EventCode.Leave:
                    if (originatingPlayer != null)
                    {
                        bool isInactive = false;
                        if (photonEvent.Parameters.ContainsKey(ParameterCode.IsInactive))
                        {
                            isInactive = (bool)photonEvent.Parameters[ParameterCode.IsInactive];
                        }

                        originatingPlayer.IsInactive = isInactive;
                        originatingPlayer.HasRejoined = false;

                        if (!isInactive)
                        {
                            this.CurrentRoom.RemovePlayer(actorNr);
                        }
                    }

                    if (photonEvent.Parameters.ContainsKey(ParameterCode.MasterClientId))
                    {
                        int newMaster = (int)photonEvent[ParameterCode.MasterClientId];
                        if (newMaster != 0)
                        {
                            this.CurrentRoom.masterClientId = newMaster;
                            this.InRoomCallbackTargets.OnMasterClientSwitched(this.CurrentRoom.GetPlayer(newMaster));
                        }
                    }
                    this.InRoomCallbackTargets.OnPlayerLeftRoom(originatingPlayer);
                    break;

                case EventCode.PropertiesChanged:
                    int targetActorNr = 0;
                    if (photonEvent.Parameters.ContainsKey(ParameterCode.TargetActorNr))
                    {
                        targetActorNr = (int)photonEvent[ParameterCode.TargetActorNr];
                    }

                    Hashtable gameProperties = null;
                    Hashtable actorProps = null;
                    if (targetActorNr == 0)
                    {
                        gameProperties = (Hashtable)photonEvent[ParameterCode.Properties];
                    }
                    else
                    {
                        actorProps = (Hashtable)photonEvent[ParameterCode.Properties];
                    }

                    this.ReadoutProperties(gameProperties, actorProps, targetActorNr);
                    break;

                case EventCode.AppStats:
                    this.PlayersInRoomsCount = (int)photonEvent[ParameterCode.PeerCount];
                    this.RoomsCount = (int)photonEvent[ParameterCode.GameCount];
                    this.PlayersOnMasterCount = (int)photonEvent[ParameterCode.MasterPeerCount];
                    break;

                case EventCode.LobbyStats:
                    string[] names = photonEvent[ParameterCode.LobbyName] as string[];
                    int[] peers = photonEvent[ParameterCode.PeerCount] as int[];
                    int[] rooms = photonEvent[ParameterCode.GameCount] as int[];

                    byte[] types;
                    ByteArraySlice slice = photonEvent[ParameterCode.LobbyType] as ByteArraySlice;
                    bool useByteArraySlice = slice != null;

                    if (useByteArraySlice)
                    {
                        types = slice.Buffer;
                    }
                    else
                    {
                        types = photonEvent[ParameterCode.LobbyType] as byte[];
                    }

                    this.lobbyStatistics.Clear();
                    for (int i = 0; i < names.Length; i++)
                    {
                        TypedLobbyInfo info = new TypedLobbyInfo();
                        info.Name = names[i];
                        info.Type = (LobbyType)types[i];
                        info.PlayerCount = peers[i];
                        info.RoomCount = rooms[i];

                        this.lobbyStatistics.Add(info);
                    }

                    if (useByteArraySlice)
                    {
                        slice.Release();
                    }

                    this.LobbyCallbackTargets.OnLobbyStatisticsUpdate(this.lobbyStatistics);
                    break;

                case EventCode.ErrorInfo:
                    this.ErrorInfoCallbackTargets.OnErrorInfo(new ErrorInfo(photonEvent));
                    break;

                case EventCode.AuthEvent:
                    if (this.AuthValues == null)
                    {
                        this.AuthValues = new AuthenticationValues();
                    }

                    this.AuthValues.Token = photonEvent[ParameterCode.Token];
                    this.tokenCache = this.AuthValues.Token;
                    break;

            }

            this.UpdateCallbackTargets();
            if (this.EventReceived != null)
            {
                this.EventReceived(photonEvent);
            }
        }
        public virtual void OnMessage(object message)
        {
            this.DebugReturn(DebugLevel.ALL, string.Format("got OnMessage {0}", message));
        }

        #endregion


        private void OnDisconnectMessageReceived(DisconnectMessage obj)
        {
            this.DebugReturn(DebugLevel.ERROR, string.Format("Got DisconnectMessage. Code: {0} Msg: \"{1}\". Debug Info: {2}", obj.Code, obj.DebugMessage, obj.Parameters.ToStringFull()));
            this.DisconnectMessage = $"DisconnectMessage {obj.Code}: {obj.DebugMessage}";
            this.Disconnect(DisconnectCause.DisconnectByDisconnectMessage);
        }
        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            this.SummaryToCache = regionHandler.SummaryToCache;
            this.ConnectToRegionMaster(regionHandler.BestRegion.Code);
        }


        protected internal static string ReplacePortWithAlternative(string address, ushort replacementPort)
        {
            if (string.IsNullOrEmpty(address) || replacementPort == 0)
            {
                return address;
            }

            bool webSocket = address.StartsWith("ws");
            if (webSocket)
            {
                UriBuilder urib = new UriBuilder(address);
                urib.Port = replacementPort;
                return urib.ToString();
            }
            else
            {
                UriBuilder urib = new UriBuilder($"scheme://{address}");
                return string.Format("{0}:{1}", urib.Host, replacementPort);
            }
        }

        private void SetupEncryption(Dictionary<byte, object> encryptionData)
        {
            var mode = (EncryptionMode)(byte)encryptionData[EncryptionDataParameters.Mode];
            switch (mode)
            {
                case EncryptionMode.PayloadEncryption:
                    byte[] encryptionSecret = (byte[])encryptionData[EncryptionDataParameters.Secret1];
                    this.LoadBalancingPeer.InitPayloadEncryption(encryptionSecret);
                    break;
                case EncryptionMode.DatagramEncryptionGCM:
                    {
                        byte[] secret1 = (byte[])encryptionData[EncryptionDataParameters.Secret1];
                        this.LoadBalancingPeer.InitDatagramEncryption(secret1, null, true, true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool OpWebRpc(string uriPath, object parameters, bool sendAuthCookie = false)
        {
            if (string.IsNullOrEmpty(uriPath))
            {
                this.DebugReturn(DebugLevel.ERROR, "WebRPC method name must not be null nor empty.");
                return false;
            }
            if (!this.CheckIfOpCanBeSent(OperationCode.WebRpc, this.Server, "WebRpc"))
            {
                return false;
            }
            Dictionary<byte, object> opParameters = new Dictionary<byte, object>();
            opParameters.Add(ParameterCode.UriPath, uriPath);
            if (parameters != null)
            {
                opParameters.Add(ParameterCode.WebRpcParameters, parameters);
            }
            if (sendAuthCookie)
            {
                opParameters.Add(ParameterCode.EventForward, WebFlags.SendAuthCookieConst);
            }
            return this.LoadBalancingPeer.SendOperation(OperationCode.WebRpc, opParameters, SendOptions.SendReliable);
        }
        public void AddCallbackTarget(object target)
        {
            this.callbackTargetChanges.Enqueue(new CallbackTargetChange(target, true));
        }
        public void RemoveCallbackTarget(object target)
        {
            this.callbackTargetChanges.Enqueue(new CallbackTargetChange(target, false));
        }
        protected internal void UpdateCallbackTargets()
        {
            while (this.callbackTargetChanges.Count > 0)
            {
                CallbackTargetChange change = this.callbackTargetChanges.Dequeue();

                if (change.AddTarget)
                {
                    if (this.callbackTargets.Contains(change.Target))
                    {
                        continue;
                    }

                    this.callbackTargets.Add(change.Target);
                }
                else
                {
                    if (!this.callbackTargets.Contains(change.Target))
                    {
                        continue;
                    }

                    this.callbackTargets.Remove(change.Target);
                }

                this.UpdateCallbackTarget<IInRoomCallbacks>(change, this.InRoomCallbackTargets);
                this.UpdateCallbackTarget<IConnectionCallbacks>(change, this.ConnectionCallbackTargets);
                this.UpdateCallbackTarget<IMatchmakingCallbacks>(change, this.MatchMakingCallbackTargets);
                this.UpdateCallbackTarget<ILobbyCallbacks>(change, this.LobbyCallbackTargets);
                this.UpdateCallbackTarget<IWebRpcCallback>(change, this.WebRpcCallbackTargets);
                this.UpdateCallbackTarget<IErrorInfoCallback>(change, this.ErrorInfoCallbackTargets);

                IOnEventCallback onEventCallback = change.Target as IOnEventCallback;
                if (onEventCallback != null)
                {
                    if (change.AddTarget)
                    {
                        EventReceived += onEventCallback.OnEvent;
                    }
                    else
                    {
                        EventReceived -= onEventCallback.OnEvent;
                    }
                }
            }
        }
        private void UpdateCallbackTarget<T>(CallbackTargetChange change, List<T> container) where T : class
        {
            T target = change.Target as T;
            if (target != null)
            {
                if (change.AddTarget)
                {
                    container.Add(target);
                }
                else
                {
                    container.Remove(target);
                }
            }
        }
    }
    public interface IConnectionCallbacks
    {
        void OnConnected();
        void OnConnectedToMaster();
        void OnDisconnected(DisconnectCause cause);
        void OnRegionListReceived(RegionHandler regionHandler);
        void OnCustomAuthenticationResponse(Dictionary<string, object> data);
        void OnCustomAuthenticationFailed(string debugMessage);

    }
    public interface ILobbyCallbacks
    {
        void OnJoinedLobby();
        void OnLeftLobby();
        void OnRoomListUpdate(List<RoomInfo> roomList);
        void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics);
    }
    public interface IMatchmakingCallbacks
    {
        void OnFriendListUpdate(List<FriendInfo> friendList);
        void OnCreatedRoom();
        void OnCreateRoomFailed(short returnCode, string message);
        void OnJoinedRoom();
        void OnJoinRoomFailed(short returnCode, string message);
        void OnJoinRandomFailed(short returnCode, string message);
        void OnLeftRoom();
    }
    public interface IInRoomCallbacks
    {
        void OnPlayerEnteredRoom(Player newPlayer);
        void OnPlayerLeftRoom(Player otherPlayer);
        void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged);
        void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps);
        void OnMasterClientSwitched(Player newMasterClient);
    }
    public interface IOnEventCallback
    {
        void OnEvent(EventData photonEvent);
    }
    public interface IWebRpcCallback
    {
        void OnWebRpcResponse(OperationResponse response);
    }
    public interface IErrorInfoCallback
    {
        void OnErrorInfo(ErrorInfo errorInfo);
    }
    public class ConnectionCallbacksContainer : List<IConnectionCallbacks>, IConnectionCallbacks
    {
        private readonly LoadBalancingClient client;

        public ConnectionCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnConnected()
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnConnected();
            }
        }

        public void OnConnectedToMaster()
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnConnectedToMaster();
            }
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnRegionListReceived(regionHandler);
            }
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnDisconnected(cause);
            }
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnCustomAuthenticationResponse(data);
            }
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            this.client.UpdateCallbackTargets();

            foreach (IConnectionCallbacks target in this)
            {
                target.OnCustomAuthenticationFailed(debugMessage);
            }
        }
    }
    public class MatchMakingCallbacksContainer : List<IMatchmakingCallbacks>, IMatchmakingCallbacks
    {
        private readonly LoadBalancingClient client;

        public MatchMakingCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnCreatedRoom()
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnCreatedRoom();
            }
        }

        public void OnJoinedRoom()
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnJoinedRoom();
            }
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnCreateRoomFailed(returnCode, message);
            }
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnJoinRandomFailed(returnCode, message);
            }
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnJoinRoomFailed(returnCode, message);
            }
        }

        public void OnLeftRoom()
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnLeftRoom();
            }
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            this.client.UpdateCallbackTargets();

            foreach (IMatchmakingCallbacks target in this)
            {
                target.OnFriendListUpdate(friendList);
            }
        }
    }
    internal class InRoomCallbacksContainer : List<IInRoomCallbacks>, IInRoomCallbacks
    {
        private readonly LoadBalancingClient client;

        public InRoomCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            this.client.UpdateCallbackTargets();

            foreach (IInRoomCallbacks target in this)
            {
                target.OnPlayerEnteredRoom(newPlayer);
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            this.client.UpdateCallbackTargets();

            foreach (IInRoomCallbacks target in this)
            {
                target.OnPlayerLeftRoom(otherPlayer);
            }
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            this.client.UpdateCallbackTargets();

            foreach (IInRoomCallbacks target in this)
            {
                target.OnRoomPropertiesUpdate(propertiesThatChanged);
            }
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProp)
        {
            this.client.UpdateCallbackTargets();

            foreach (IInRoomCallbacks target in this)
            {
                target.OnPlayerPropertiesUpdate(targetPlayer, changedProp);
            }
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            this.client.UpdateCallbackTargets();

            foreach (IInRoomCallbacks target in this)
            {
                target.OnMasterClientSwitched(newMasterClient);
            }
        }
    }
    internal class LobbyCallbacksContainer : List<ILobbyCallbacks>, ILobbyCallbacks
    {
        private readonly LoadBalancingClient client;

        public LobbyCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnJoinedLobby()
        {
            this.client.UpdateCallbackTargets();

            foreach (ILobbyCallbacks target in this)
            {
                target.OnJoinedLobby();
            }
        }

        public void OnLeftLobby()
        {
            this.client.UpdateCallbackTargets();

            foreach (ILobbyCallbacks target in this)
            {
                target.OnLeftLobby();
            }
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            this.client.UpdateCallbackTargets();

            foreach (ILobbyCallbacks target in this)
            {
                target.OnRoomListUpdate(roomList);
            }
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            this.client.UpdateCallbackTargets();

            foreach (ILobbyCallbacks target in this)
            {
                target.OnLobbyStatisticsUpdate(lobbyStatistics);
            }
        }
    }
    internal class WebRpcCallbacksContainer : List<IWebRpcCallback>, IWebRpcCallback
    {
        private LoadBalancingClient client;

        public WebRpcCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnWebRpcResponse(OperationResponse response)
        {
            this.client.UpdateCallbackTargets();

            foreach (IWebRpcCallback target in this)
            {
                target.OnWebRpcResponse(response);
            }
        }
    }
    internal class ErrorInfoCallbacksContainer : List<IErrorInfoCallback>, IErrorInfoCallback
    {
        private LoadBalancingClient client;

        public ErrorInfoCallbacksContainer(LoadBalancingClient client)
        {
            this.client = client;
        }

        public void OnErrorInfo(ErrorInfo errorInfo)
        {
            this.client.UpdateCallbackTargets();
            foreach (IErrorInfoCallback target in this)
            {
                target.OnErrorInfo(errorInfo);
            }
        }
    }
    public class ErrorInfo
    {
        public readonly string Info;

        public ErrorInfo(EventData eventData)
        {
            this.Info = eventData[ParameterCode.Info] as string;
        }

        public override string ToString()
        {
            return string.Format("ErrorInfo: {0}", this.Info);
        }
    }
}