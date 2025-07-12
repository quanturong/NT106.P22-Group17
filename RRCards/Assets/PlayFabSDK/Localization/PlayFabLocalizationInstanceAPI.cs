#if !DISABLE_PLAYFABENTITY_API

using System;
using System.Collections.Generic;
using PlayFab.LocalizationModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabLocalizationInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabLocalizationInstanceAPI(PlayFabAuthenticationContext context)
        {
            if (context == null)
                throw new PlayFabException(PlayFabExceptionCode.AuthContextRequired, "Context cannot be null, create a PlayFabAuthenticationContext for each player in advance, or call <PlayFabClientInstanceAPI>.GetAuthenticationContext()");
            authenticationContext = context;
        }

        public PlayFabLocalizationInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
        {
            if (context == null)
                throw new PlayFabException(PlayFabExceptionCode.AuthContextRequired, "Context cannot be null, create a PlayFabAuthenticationContext for each player in advance, or call <PlayFabClientInstanceAPI>.GetAuthenticationContext()");
            apiSettings = settings;
            authenticationContext = context;
        }
        public bool IsEntityLoggedIn()
        {
            return authenticationContext == null ? false : authenticationContext.IsEntityLoggedIn();
        }
        public void ForgetAllCredentials()
        {
            if (authenticationContext != null)
            {
                authenticationContext.ForgetAllCredentials();
            }
        }
        public void GetLanguageList(GetLanguageListRequest request, Action<GetLanguageListResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Locale/GetLanguageList", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
