#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.LocalizationModels
{
    [Serializable]
    public class GetLanguageListRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetLanguageListResponse : PlayFabResultCommon
    {
        public List<string> LanguageList;
    }
}
#endif
