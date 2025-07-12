
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif

namespace Photon.Chat
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class ChatPeer : PhotonPeer
    {
        public string NameServerHost = "ns.photonengine.io";
        private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>() { { ConnectionProtocol.Udp, 5058 }, { ConnectionProtocol.Tcp, 4533 }, { ConnectionProtocol.WebSocket, 80 }, { ConnectionProtocol.WebSocketSecure, 443 } };
        public string NameServerAddress { get { return this.GetNameServerAddress(); } }

        virtual internal bool IsProtocolSecure { get { return this.UsedProtocol == ConnectionProtocol.WebSocketSecure; } }
        public ChatPeer(IPhotonPeerListener listener, ConnectionProtocol protocol) : base(listener, protocol)
        {
            this.ConfigUnitySockets();
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
            #endif

            if (websocketType != null)
            {
                this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = websocketType;
                this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = websocketType;
            }

            #if NET_4_6 && (UNITY_EDITOR || !ENABLE_IL2CPP) && !NETFX_CORE
            this.SocketImplementationConfig[ConnectionProtocol.Udp] = typeof(SocketUdpAsync);
            this.SocketImplementationConfig[ConnectionProtocol.Tcp] = typeof(SocketTcpAsync);
            #endif
        }
        public ushort NameServerPortOverride;
        private string GetNameServerAddress()
        {
            var protocolPort = 0;
            ProtocolToNameServerPort.TryGetValue(this.TransportProtocol, out protocolPort);

            if (this.NameServerPortOverride != 0)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, string.Format("Using NameServerPortInAppSettings as port for Name Server: {0}", this.NameServerPortOverride));
                protocolPort = this.NameServerPortOverride;
            }

            switch (this.TransportProtocol)
            {
                case ConnectionProtocol.Udp:
                case ConnectionProtocol.Tcp:
                    return string.Format("{0}:{1}", NameServerHost, protocolPort);
                case ConnectionProtocol.WebSocket:
                    return string.Format("ws://{0}:{1}", NameServerHost, protocolPort);
                case ConnectionProtocol.WebSocketSecure:
                    return string.Format("wss://{0}:{1}", NameServerHost, protocolPort);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public bool AuthenticateOnNameServer(string appId, string appVersion, string region, AuthenticationValues authValues)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
            }

            var opParameters = new Dictionary<byte, object>();

            opParameters[ParameterCode.AppVersion] = appVersion;
            opParameters[ParameterCode.ApplicationId] = appId;
            opParameters[ParameterCode.Region] = region;

            if (authValues != null)
            {
                if (!string.IsNullOrEmpty(authValues.UserId))
                {
                    opParameters[ParameterCode.UserId] = authValues.UserId;
                }

                if (authValues.AuthType != CustomAuthenticationType.None)
                {
                    opParameters[ParameterCode.ClientAuthenticationType] = (byte) authValues.AuthType;
                    if (authValues.Token != null)
                    {
                        opParameters[ParameterCode.Secret] = authValues.Token;
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

            return this.SendOperation(ChatOperationCode.Authenticate, opParameters, new SendOptions() { Reliability = true, Encrypt = this.IsEncryptionAvailable });
        }
    }
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
            return string.Format("AuthenticationValues Type: {3} UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null, this.AuthType);
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
    public class ParameterCode
    {
        public const byte ApplicationId = 224;
        public const byte Secret = 221;
        public const byte AppVersion = 220;
        public const byte ClientAuthenticationType = 217;
        public const byte ClientAuthenticationParams = 216;
        public const byte ClientAuthenticationData = 214;
        public const byte Region = 210;
        public const byte Address = 230;
        public const byte UserId = 225;
    }
    public class ErrorCode
    {
        public const int Ok = 0;
        public const int OperationNotAllowedInCurrentState = -3;
        public const int InvalidOperationCode = -2;
        public const int InternalServerError = -1;
        public const int InvalidAuthentication = 0x7FFF;
        public const int GameIdAlreadyExists = 0x7FFF - 1;
        public const int GameFull = 0x7FFF - 2;
        public const int GameClosed = 0x7FFF - 3;
        public const int ServerFull = 0x7FFF - 5;
        public const int UserBlocked = 0x7FFF - 6;
        public const int NoRandomMatchFound = 0x7FFF - 7;
        public const int GameDoesNotExist = 0x7FFF - 9;
        public const int MaxCcuReached = 0x7FFF - 10;
        public const int InvalidRegion = 0x7FFF - 11;
        public const int CustomAuthenticationFailed = 0x7FFF - 12;
        public const int AuthenticationTicketExpired = 0x7FF1;
    }

}
