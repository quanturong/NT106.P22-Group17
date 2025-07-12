#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.LocalizationModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabLocalizationAPI
    {
        static PlayFabLocalizationAPI() {}
        public static bool IsEntityLoggedIn()
        {
            return PlayFabSettings.staticPlayer.IsEntityLoggedIn();
        }
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        public static void GetLanguageList(GetLanguageListRequest request, Action<GetLanguageListResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Locale/GetLanguageList", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
