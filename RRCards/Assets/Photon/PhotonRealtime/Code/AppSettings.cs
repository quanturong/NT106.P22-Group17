
#if UNITY_2017_4_OR_NEWER
#define SUPPORTED_UNITY
#endif

namespace Photon.Realtime
{
    using System;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    #if !NETFX_CORE || SUPPORTED_UNITY
    [Serializable]
    #endif
    public class AppSettings
    {
        public string AppIdRealtime;
        public string AppIdFusion;
        public string AppIdChat;
        public string AppIdVoice;
        public string AppVersion;
        public bool UseNameServer = true;
        public string FixedRegion;
        #if SUPPORTED_UNITY
        [NonSerialized]
        #endif
        public string BestRegionSummaryFromStorage;
        public string Server;
        public int Port;
        public string ProxyServer;
        public ConnectionProtocol Protocol = ConnectionProtocol.Udp;
        public bool EnableProtocolFallback = true;
        public AuthModeOption AuthMode = AuthModeOption.Auth;
        public bool EnableLobbyStatistics;
        public DebugLevel NetworkLogging = DebugLevel.ERROR;
        public bool IsMasterServerAddress
        {
            get { return !this.UseNameServer; }
        }
        public bool IsBestRegion
        {
            get { return this.UseNameServer && string.IsNullOrEmpty(this.FixedRegion); }
        }
        public bool IsDefaultNameServer
        {
            get { return this.UseNameServer && string.IsNullOrEmpty(this.Server); }
        }
        public bool IsDefaultPort
        {
            get { return this.Port <= 0; }
        }
        public string ToStringFull()
        {
            return string.Format(
                                 "appId {0}{1}{2}{3}" +
                                 "use ns: {4}, reg: {5}, {9}, " +
                                 "{6}{7}{8}" +
                                 "auth: {10}",
                                 String.IsNullOrEmpty(this.AppIdRealtime) ? string.Empty : "Realtime/PUN: " + this.HideAppId(this.AppIdRealtime) + ", ",
                                 String.IsNullOrEmpty(this.AppIdFusion) ? string.Empty : "Fusion: " + this.HideAppId(this.AppIdFusion) + ", ",
                                 String.IsNullOrEmpty(this.AppIdChat) ? string.Empty : "Chat: " + this.HideAppId(this.AppIdChat) + ", ",
                                 String.IsNullOrEmpty(this.AppIdVoice) ? string.Empty : "Voice: " + this.HideAppId(this.AppIdVoice) + ", ",
                                 String.IsNullOrEmpty(this.AppVersion) ? string.Empty : "AppVersion: " + this.AppVersion + ", ",
                                 "UseNameServer: " + this.UseNameServer + ", ",
                                 "Fixed Region: " + this.FixedRegion + ", ",
                                 String.IsNullOrEmpty(this.Server) ? string.Empty : "Server: " + this.Server + ", ",
                                 this.IsDefaultPort ? string.Empty : "Port: " + this.Port + ", ",
                                 String.IsNullOrEmpty(ProxyServer) ? string.Empty : "Proxy: " + this.ProxyServer + ", ",
                                 this.Protocol,
                                 this.AuthMode
                                );
        }
        public static bool IsAppId(string val)
        {
            try
            {
                new Guid(val);
            }
            catch
            {
                return false;
            }

            return true;
        }


        private string HideAppId(string appId)
        {
            return string.IsNullOrEmpty(appId) || appId.Length < 8
                       ? appId
                       : string.Concat(appId.Substring(0, 8), "***");
        }

        public AppSettings CopyTo(AppSettings d)
        {
            d.AppIdRealtime = this.AppIdRealtime;
            d.AppIdFusion = this.AppIdFusion;
            d.AppIdChat = this.AppIdChat;
            d.AppIdVoice = this.AppIdVoice;
            d.AppVersion = this.AppVersion;
            d.UseNameServer = this.UseNameServer;
            d.FixedRegion = this.FixedRegion;
            d.BestRegionSummaryFromStorage = this.BestRegionSummaryFromStorage;
            d.Server = this.Server;
            d.Port = this.Port;
            d.ProxyServer = this.ProxyServer;
            d.Protocol = this.Protocol;
            d.AuthMode = this.AuthMode;
            d.EnableLobbyStatistics = this.EnableLobbyStatistics;
            d.NetworkLogging = this.NetworkLogging;
            d.EnableProtocolFallback = this.EnableProtocolFallback;
            return d;
        }
    }
}
