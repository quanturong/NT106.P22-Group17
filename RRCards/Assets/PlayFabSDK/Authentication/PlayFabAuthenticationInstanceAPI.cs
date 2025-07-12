#if !DISABLE_PLAYFABENTITY_API

using System;
using System.Collections.Generic;
using PlayFab.AuthenticationModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabAuthenticationInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabAuthenticationInstanceAPI()
        {
            authenticationContext = new PlayFabAuthenticationContext();
        }

        public PlayFabAuthenticationInstanceAPI(PlayFabApiSettings settings)
        {
            apiSettings = settings;
            authenticationContext = new PlayFabAuthenticationContext();
        }

        public PlayFabAuthenticationInstanceAPI(PlayFabAuthenticationContext context)
        {
            authenticationContext = context ?? new PlayFabAuthenticationContext();
        }

        public PlayFabAuthenticationInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
        {
            apiSettings = settings;
            authenticationContext = context ?? new PlayFabAuthenticationContext();
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
        public void AuthenticateGameServerWithCustomId(AuthenticateCustomIdRequest request, Action<AuthenticateCustomIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/GameServerIdentity/AuthenticateGameServerWithCustomId", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void Delete(DeleteRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/GameServerIdentity/Delete", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetEntityToken(GetEntityTokenRequest request, Action<GetEntityTokenResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            AuthType authType = AuthType.None;
#if !DISABLE_PLAYFABCLIENT_API
            if (context.IsClientLoggedIn()) { authType = AuthType.LoginSession; }
#endif
#if ENABLE_PLAYFABSERVER_API || ENABLE_PLAYFABADMIN_API || ENABLE_PLAYFAB_SECRETKEY
            if (callSettings.DeveloperSecretKey != null) { authType = AuthType.DevSecretKey; }
#endif
#if !DISABLE_PLAYFABENTITY_API
            if (context.IsEntityLoggedIn()) { authType = AuthType.EntityToken; }
#endif
            PlayFabHttp.MakeApiCall("/Authentication/GetEntityToken", request, authType, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ValidateEntityToken(ValidateEntityTokenRequest request, Action<ValidateEntityTokenResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Authentication/ValidateEntityToken", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
