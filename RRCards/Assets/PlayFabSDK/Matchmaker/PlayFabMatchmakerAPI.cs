#if ENABLE_PLAYFABSERVER_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.MatchmakerModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabMatchmakerAPI
    {
        static PlayFabMatchmakerAPI() {}
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        [Obsolete("No longer available", true)]
        public static void AuthUser(AuthUserRequest request, Action<AuthUserResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Matchmaker/AuthUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        [Obsolete("No longer available", true)]
        public static void PlayerJoined(PlayerJoinedRequest request, Action<PlayerJoinedResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Matchmaker/PlayerJoined", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        [Obsolete("No longer available", true)]
        public static void PlayerLeft(PlayerLeftRequest request, Action<PlayerLeftResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Matchmaker/PlayerLeft", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        [Obsolete("No longer available", true)]
        public static void UserInfo(UserInfoRequest request, Action<UserInfoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Matchmaker/UserInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
