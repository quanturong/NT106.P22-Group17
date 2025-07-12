#if !DISABLE_PLAYFABENTITY_API

using System;
using System.Collections.Generic;
using PlayFab.AddonModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabAddonInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabAddonInstanceAPI(PlayFabAuthenticationContext context)
        {
            if (context == null)
                throw new PlayFabException(PlayFabExceptionCode.AuthContextRequired, "Context cannot be null, create a PlayFabAuthenticationContext for each player in advance, or call <PlayFabClientInstanceAPI>.GetAuthenticationContext()");
            authenticationContext = context;
        }

        public PlayFabAddonInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
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
        public void CreateOrUpdateApple(CreateOrUpdateAppleRequest request, Action<CreateOrUpdateAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateFacebook(CreateOrUpdateFacebookRequest request, Action<CreateOrUpdateFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateFacebookInstantGames(CreateOrUpdateFacebookInstantGamesRequest request, Action<CreateOrUpdateFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateGoogle(CreateOrUpdateGoogleRequest request, Action<CreateOrUpdateGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateKongregate(CreateOrUpdateKongregateRequest request, Action<CreateOrUpdateKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateNintendo(CreateOrUpdateNintendoRequest request, Action<CreateOrUpdateNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdatePSN(CreateOrUpdatePSNRequest request, Action<CreateOrUpdatePSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdatePSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateSteam(CreateOrUpdateSteamRequest request, Action<CreateOrUpdateSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateOrUpdateTwitch(CreateOrUpdateTwitchRequest request, Action<CreateOrUpdateTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteApple(DeleteAppleRequest request, Action<DeleteAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteFacebook(DeleteFacebookRequest request, Action<DeleteFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteFacebookInstantGames(DeleteFacebookInstantGamesRequest request, Action<DeleteFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteGoogle(DeleteGoogleRequest request, Action<DeleteGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteKongregate(DeleteKongregateRequest request, Action<DeleteKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteNintendo(DeleteNintendoRequest request, Action<DeleteNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeletePSN(DeletePSNRequest request, Action<DeletePSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeletePSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteSteam(DeleteSteamRequest request, Action<DeleteSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteTwitch(DeleteTwitchRequest request, Action<DeleteTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/DeleteTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetApple(GetAppleRequest request, Action<GetAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetFacebook(GetFacebookRequest request, Action<GetFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetFacebookInstantGames(GetFacebookInstantGamesRequest request, Action<GetFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetGoogle(GetGoogleRequest request, Action<GetGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetKongregate(GetKongregateRequest request, Action<GetKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetNintendo(GetNintendoRequest request, Action<GetNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPSN(GetPSNRequest request, Action<GetPSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetPSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetSteam(GetSteamRequest request, Action<GetSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTwitch(GetTwitchRequest request, Action<GetTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Addon/GetTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
