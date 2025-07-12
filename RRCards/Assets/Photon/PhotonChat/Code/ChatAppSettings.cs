
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif


namespace Photon.Chat
{
    using System;
    using ExitGames.Client.Photon;
    #if SUPPORTED_UNITY
    using UnityEngine.Serialization;
    #endif
    #if !NETFX_CORE || SUPPORTED_UNITY
    [Serializable]
    #endif
    public class ChatAppSettings
    {
        public string AppIdChat;
        public string AppVersion;
        public string FixedRegion;
        public string Server;
        public ushort Port;
        public string ProxyServer;
        public ConnectionProtocol Protocol = ConnectionProtocol.Udp;
        public bool EnableProtocolFallback = true;
        public DebugLevel NetworkLogging = DebugLevel.ERROR;
        public bool IsDefaultNameServer { get { return string.IsNullOrEmpty(this.Server); } }
    }
}