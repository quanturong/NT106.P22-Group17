#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.ProgressionModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabProgressionAPI
    {
        static PlayFabProgressionAPI() {}
        public static bool IsEntityLoggedIn()
        {
            return PlayFabSettings.staticPlayer.IsEntityLoggedIn();
        }
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        public static void CreateLeaderboardDefinition(CreateLeaderboardDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/CreateLeaderboardDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateStatisticDefinition(CreateStatisticDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/CreateStatisticDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteLeaderboardDefinition(DeleteLeaderboardDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/DeleteLeaderboardDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteLeaderboardEntries(DeleteLeaderboardEntriesRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/DeleteLeaderboardEntries", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteStatisticDefinition(DeleteStatisticDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/DeleteStatisticDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteStatistics(DeleteStatisticsRequest request, Action<DeleteStatisticsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/DeleteStatistics", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetFriendLeaderboardForEntity(GetFriendLeaderboardForEntityRequest request, Action<GetEntityLeaderboardResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/GetFriendLeaderboardForEntity", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetLeaderboard(GetEntityLeaderboardRequest request, Action<GetEntityLeaderboardResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/GetLeaderboard", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetLeaderboardAroundEntity(GetLeaderboardAroundEntityRequest request, Action<GetEntityLeaderboardResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/GetLeaderboardAroundEntity", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetLeaderboardDefinition(GetLeaderboardDefinitionRequest request, Action<GetLeaderboardDefinitionResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/GetLeaderboardDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetLeaderboardForEntities(GetLeaderboardForEntitiesRequest request, Action<GetEntityLeaderboardResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/GetLeaderboardForEntities", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetStatisticDefinition(GetStatisticDefinitionRequest request, Action<GetStatisticDefinitionResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/GetStatisticDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetStatistics(GetStatisticsRequest request, Action<GetStatisticsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/GetStatistics", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetStatisticsForEntities(GetStatisticsForEntitiesRequest request, Action<GetStatisticsForEntitiesResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/GetStatisticsForEntities", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void IncrementLeaderboardVersion(IncrementLeaderboardVersionRequest request, Action<IncrementLeaderboardVersionResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/IncrementLeaderboardVersion", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void IncrementStatisticVersion(IncrementStatisticVersionRequest request, Action<IncrementStatisticVersionResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/IncrementStatisticVersion", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ListLeaderboardDefinitions(ListLeaderboardDefinitionsRequest request, Action<ListLeaderboardDefinitionsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/ListLeaderboardDefinitions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ListStatisticDefinitions(ListStatisticDefinitionsRequest request, Action<ListStatisticDefinitionsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/ListStatisticDefinitions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UnlinkLeaderboardFromStatistic(UnlinkLeaderboardFromStatisticRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/UnlinkLeaderboardFromStatistic", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateLeaderboardDefinition(UpdateLeaderboardDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/UpdateLeaderboardDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateLeaderboardEntries(UpdateLeaderboardEntriesRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Leaderboard/UpdateLeaderboardEntries", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateStatisticDefinition(UpdateStatisticDefinitionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/UpdateStatisticDefinition", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateStatistics(UpdateStatisticsRequest request, Action<UpdateStatisticsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");


            PlayFabHttp.MakeApiCall("/Statistic/UpdateStatistics", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
