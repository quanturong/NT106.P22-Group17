#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.ProfilesModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabProfilesAPI
    {
        static PlayFabProfilesAPI() {}
        public static bool IsEntityLoggedIn()
        {
            return PlayFabSettings.staticPlayer.IsEntityLoggedIn();
        }
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        public static void GetGlobalPolicy(GetGlobalPolicyRequest request, Action<GetGlobalPolicyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/GetGlobalPolicy", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetProfile(GetEntityProfileRequest request, Action<GetEntityProfileResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/GetProfile", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetProfiles(GetEntityProfilesRequest request, Action<GetEntityProfilesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/GetProfiles", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTitlePlayersFromMasterPlayerAccountIds(GetTitlePlayersFromMasterPlayerAccountIdsRequest request, Action<GetTitlePlayersFromMasterPlayerAccountIdsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/GetTitlePlayersFromMasterPlayerAccountIds", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTitlePlayersFromXboxLiveIDs(GetTitlePlayersFromXboxLiveIDsRequest request, Action<GetTitlePlayersFromProviderIDsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/GetTitlePlayersFromXboxLiveIDs", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetDisplayName(SetDisplayNameRequest request, Action<SetDisplayNameResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/SetDisplayName", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetGlobalPolicy(SetGlobalPolicyRequest request, Action<SetGlobalPolicyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/SetGlobalPolicy", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetProfileLanguage(SetProfileLanguageRequest request, Action<SetProfileLanguageResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/SetProfileLanguage", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetProfilePolicy(SetEntityProfilePolicyRequest request, Action<SetEntityProfilePolicyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Profile/SetProfilePolicy", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
