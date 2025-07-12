

namespace Photon.Pun
{
    using System;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using Photon.Realtime;
    using UnityEngine;
    [Serializable]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/getting-started/initial-setup")]
    public class ServerSettings : ScriptableObject
    {
        [Tooltip("Core Photon Server/Cloud settings.")]
        public AppSettings AppSettings;
        [Tooltip("Developer build override for Best Region.")]
        public string DevRegion;

        [Tooltip("Log output by PUN.")]
        public PunLogLevel PunLogging = PunLogLevel.ErrorsOnly;

        [Tooltip("Logs additional info for debugging.")]
        public bool EnableSupportLogger;

        [Tooltip("Enables apps to keep the connection without focus.")]
        public bool RunInBackground = true;

        [Tooltip("Simulates an online connection.\nPUN can be used as usual.")]
        public bool StartInOfflineMode;

        [Tooltip("RPC name list.\nUsed as shortcut when sending calls.")]
        public List<string> RpcList = new List<string>();   // set by scripts and or via Inspector

        #if UNITY_EDITOR
        public bool DisableAutoOpenWizard;
        public bool ShowSettings;
        public bool DevRegionSetOnce;
        #endif
        public void UseCloud(string cloudAppid, string code = "")
        {
            this.AppSettings.AppIdRealtime = cloudAppid;
            this.AppSettings.Server = null;
            this.AppSettings.FixedRegion = string.IsNullOrEmpty(code) ? null : code;
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
        public static string BestRegionSummaryInPreferences
        {
            get { return PhotonNetwork.BestRegionSummaryInPreferences; }
        }
        public static void ResetBestRegionCodeInPreferences()
        {
            PhotonNetwork.BestRegionSummaryInPreferences = null;
        }
        public override string ToString()
        {
            return "ServerSettings: " + this.AppSettings.ToStringFull();
        }
    }
}