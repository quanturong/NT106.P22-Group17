using System.Collections.Generic;
using System;

namespace PlayFab
{
    public class PlayFabApiSettings
    {
        private string _ProductionEnvironmentUrl = PlayFabSettings.DefaultPlayFabApiUrl;
        public readonly Dictionary<string, string> _requestGetParams = new Dictionary<string, string> {
            { "sdk", PlayFabSettings.VersionString }
        };

        public virtual Dictionary<string, string> RequestGetParams { get { return _requestGetParams; } }
        public virtual string ProductionEnvironmentUrl { get { return _ProductionEnvironmentUrl; } set { _ProductionEnvironmentUrl = value; } }
        public virtual string TitleId { get; set; }
        internal virtual string VerticalName { get; set; }
#if ENABLE_PLAYFABSERVER_API || ENABLE_PLAYFABADMIN_API || UNITY_EDITOR || ENABLE_PLAYFAB_SECRETKEY
        public virtual string DeveloperSecretKey { get; set; }
#endif
        public virtual bool DisableDeviceInfo { get; set; }
        public virtual bool DisableFocusTimeCollection { get; set; }
        public virtual bool CompressResponses { get; set; }
        internal virtual bool DecompressWithDownloadHandler { get; set; } = true;


        public virtual string GetFullUrl(string apiCall, Dictionary<string, string> getParams)
        {
            return PlayFabSettings.GetFullUrl(apiCall, getParams, this);
        }
    }
    internal class PlayFabSettingsRedirect : PlayFabApiSettings
    {
        private readonly Func<PlayFabSharedSettings> GetSO;
        public PlayFabSettingsRedirect(Func<PlayFabSharedSettings> getSO) { GetSO = getSO; }

        public override string ProductionEnvironmentUrl
        {
            get { var so = GetSO(); return so == null ? base.ProductionEnvironmentUrl : so.ProductionEnvironmentUrl; }
            set { var so = GetSO(); if (so != null) so.ProductionEnvironmentUrl = value; base.ProductionEnvironmentUrl = value; }
        }

        internal override string VerticalName
        {
            get { var so = GetSO(); return so == null ? base.VerticalName : so.VerticalName; }
            set { var so = GetSO(); if (so != null) so.VerticalName = value; base.VerticalName = value; }
        }

#if ENABLE_PLAYFABSERVER_API || ENABLE_PLAYFABADMIN_API || UNITY_EDITOR || ENABLE_PLAYFAB_SECRETKEY
        public override string DeveloperSecretKey
        {
            get { var so = GetSO(); return so == null ? base.DeveloperSecretKey : so.DeveloperSecretKey; }
            set { var so = GetSO(); if (so != null) so.DeveloperSecretKey = value; base.DeveloperSecretKey = value; }
        }
#endif

        public override string TitleId
        {
            get { var so = GetSO(); return so == null ? base.TitleId : so.TitleId; }
            set { var so = GetSO(); if (so != null) so.TitleId = value; base.TitleId = value; }
        }

        public override bool DisableDeviceInfo
        {
            get { var so = GetSO(); return so == null ? base.DisableDeviceInfo : so.DisableDeviceInfo; }
            set { var so = GetSO(); if (so != null) so.DisableDeviceInfo = value; base.DisableDeviceInfo = value; }
        }

        public override bool DisableFocusTimeCollection
        {
            get { var so = GetSO(); return so == null ? base.DisableFocusTimeCollection : so.DisableFocusTimeCollection; }
            set { var so = GetSO(); if (so != null) so.DisableFocusTimeCollection = value; base.DisableFocusTimeCollection = value; }
        }

        public override bool CompressResponses
        {
            get { var so = GetSO(); return so == null ? base.CompressResponses : so.CompressResponses; }
            set { var so = GetSO(); if (so != null) so.CompressResponses = value; base.CompressResponses = value; }
        }

        internal override bool DecompressWithDownloadHandler
        {
            get { var so = GetSO(); return so == null ? base.DecompressWithDownloadHandler : so.DecompressWithDownloadHandler; }
            set { var so = GetSO(); if (so != null) so.DecompressWithDownloadHandler = value; base.DecompressWithDownloadHandler = value; }
        }
    }
}
