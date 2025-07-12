#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.AddonModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabAddonAPI
    {
        static PlayFabAddonAPI() {}
        public static bool IsEntityLoggedIn()
        {
            return PlayFabSettings.staticPlayer.IsEntityLoggedIn();
        }
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        public static void CreateOrUpdateApple(CreateOrUpdateAppleRequest request, Action<CreateOrUpdateAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateFacebook(CreateOrUpdateFacebookRequest request, Action<CreateOrUpdateFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateFacebookInstantGames(CreateOrUpdateFacebookInstantGamesRequest request, Action<CreateOrUpdateFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateGoogle(CreateOrUpdateGoogleRequest request, Action<CreateOrUpdateGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateKongregate(CreateOrUpdateKongregateRequest request, Action<CreateOrUpdateKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateNintendo(CreateOrUpdateNintendoRequest request, Action<CreateOrUpdateNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdatePSN(CreateOrUpdatePSNRequest request, Action<CreateOrUpdatePSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdatePSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateSteam(CreateOrUpdateSteamRequest request, Action<CreateOrUpdateSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOrUpdateTwitch(CreateOrUpdateTwitchRequest request, Action<CreateOrUpdateTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/CreateOrUpdateTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteApple(DeleteAppleRequest request, Action<DeleteAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteFacebook(DeleteFacebookRequest request, Action<DeleteFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteFacebookInstantGames(DeleteFacebookInstantGamesRequest request, Action<DeleteFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteGoogle(DeleteGoogleRequest request, Action<DeleteGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteKongregate(DeleteKongregateRequest request, Action<DeleteKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteNintendo(DeleteNintendoRequest request, Action<DeleteNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeletePSN(DeletePSNRequest request, Action<DeletePSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeletePSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteSteam(DeleteSteamRequest request, Action<DeleteSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteTwitch(DeleteTwitchRequest request, Action<DeleteTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/DeleteTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetApple(GetAppleRequest request, Action<GetAppleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetApple", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetFacebook(GetFacebookRequest request, Action<GetFacebookResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetFacebook", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetFacebookInstantGames(GetFacebookInstantGamesRequest request, Action<GetFacebookInstantGamesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetFacebookInstantGames", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetGoogle(GetGoogleRequest request, Action<GetGoogleResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetGoogle", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetKongregate(GetKongregateRequest request, Action<GetKongregateResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetKongregate", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetNintendo(GetNintendoRequest request, Action<GetNintendoResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetNintendo", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPSN(GetPSNRequest request, Action<GetPSNResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetPSN", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetSteam(GetSteamRequest request, Action<GetSteamResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetSteam", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTwitch(GetTwitchRequest request, Action<GetTwitchResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Addon/GetTwitch", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
