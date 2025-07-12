#if ENABLE_PLAYFABSERVER_API

using System;
using System.Collections.Generic;
using PlayFab.ServerModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabServerInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabServerInstanceAPI() { }

        public PlayFabServerInstanceAPI(PlayFabApiSettings settings)
        {
            apiSettings = settings;
        }

        public PlayFabServerInstanceAPI(PlayFabAuthenticationContext context)
        {
            authenticationContext = context;
        }

        public PlayFabServerInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
        {
            apiSettings = settings;
            authenticationContext = context;
        }
        public void ForgetAllCredentials()
        {
            if (authenticationContext != null)
            {
                authenticationContext.ForgetAllCredentials();
            }
        }
        public void AddCharacterVirtualCurrency(AddCharacterVirtualCurrencyRequest request, Action<ModifyCharacterVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddCharacterVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AddFriend(AddFriendRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddFriend", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AddGenericID(AddGenericIDRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddGenericID", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AddPlayerTag(AddPlayerTagRequest request, Action<AddPlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddPlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AddSharedGroupMembers(AddSharedGroupMembersRequest request, Action<AddSharedGroupMembersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddSharedGroupMembers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AddUserVirtualCurrency(AddUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AddUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AuthenticateSessionTicket(AuthenticateSessionTicketRequest request, Action<AuthenticateSessionTicketResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AuthenticateSessionTicket", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void AwardSteamAchievement(AwardSteamAchievementRequest request, Action<AwardSteamAchievementResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/AwardSteamAchievement", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void BanUsers(BanUsersRequest request, Action<BanUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/BanUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ConsumeItem(ConsumeItemRequest request, Action<ConsumeItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/ConsumeItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateSharedGroup(CreateSharedGroupRequest request, Action<CreateSharedGroupResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/CreateSharedGroup", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteCharacterFromUser(DeleteCharacterFromUserRequest request, Action<DeleteCharacterFromUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/DeleteCharacterFromUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeletePlayer(DeletePlayerRequest request, Action<DeletePlayerResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/DeletePlayer", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeletePlayerCustomProperties(DeletePlayerCustomPropertiesRequest request, Action<DeletePlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/DeletePlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeletePushNotificationTemplate(DeletePushNotificationTemplateRequest request, Action<DeletePushNotificationTemplateResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/DeletePushNotificationTemplate", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteSharedGroup(DeleteSharedGroupRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/DeleteSharedGroup", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void EvaluateRandomResultTable(EvaluateRandomResultTableRequest request, Action<EvaluateRandomResultTableResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/EvaluateRandomResultTable", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ExecuteCloudScript(ExecuteCloudScriptServerRequest request, Action<ExecuteCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/ExecuteCloudScript", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

        public void ExecuteCloudScript<TOut>(ExecuteCloudScriptServerRequest request, Action<ExecuteCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method");
            Action<ExecuteCloudScriptResult> wrappedResultCallback = (wrappedResult) =>
            {
                var serializer = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
                var wrappedJson = serializer.SerializeObject(wrappedResult.FunctionResult);
                try {
                    wrappedResult.FunctionResult = serializer.DeserializeObject<TOut>(wrappedJson);
                } catch (Exception) {
                    wrappedResult.FunctionResult = wrappedJson;
                    wrappedResult.Logs.Add(new LogStatement { Level = "Warning", Data = wrappedJson, Message = "Sdk Message: Could not deserialize result as: " + typeof(TOut).Name });
                }
                resultCallback(wrappedResult);
            };
            PlayFabHttp.MakeApiCall("/Server/ExecuteCloudScript", request, AuthType.DevSecretKey, wrappedResultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetAllSegments(GetAllSegmentsRequest request, Action<GetAllSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetAllSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetAllUsersCharacters(ListUsersCharactersRequest request, Action<ListUsersCharactersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetAllUsersCharacters", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCatalogItems(GetCatalogItemsRequest request, Action<GetCatalogItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCatalogItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterInternalData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterInventory(GetCharacterInventoryRequest request, Action<GetCharacterInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterInventory", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterLeaderboard(GetCharacterLeaderboardRequest request, Action<GetCharacterLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterReadOnlyData(GetCharacterDataRequest request, Action<GetCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetCharacterStatistics(GetCharacterStatisticsRequest request, Action<GetCharacterStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetCharacterStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetContentDownloadUrl(GetContentDownloadUrlRequest request, Action<GetContentDownloadUrlResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetContentDownloadUrl", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetFriendLeaderboard(GetFriendLeaderboardRequest request, Action<GetLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetFriendLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetFriendsList(GetFriendsListRequest request, Action<GetFriendsListResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetFriendsList", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetLeaderboard(GetLeaderboardRequest request, Action<GetLeaderboardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetLeaderboard", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetLeaderboardAroundCharacter(GetLeaderboardAroundCharacterRequest request, Action<GetLeaderboardAroundCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetLeaderboardAroundCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetLeaderboardAroundUser(GetLeaderboardAroundUserRequest request, Action<GetLeaderboardAroundUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetLeaderboardAroundUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetLeaderboardForUserCharacters(GetLeaderboardForUsersCharactersRequest request, Action<GetLeaderboardForUsersCharactersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetLeaderboardForUserCharacters", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerCombinedInfo(GetPlayerCombinedInfoRequest request, Action<GetPlayerCombinedInfoResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerCombinedInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerCustomProperty(GetPlayerCustomPropertyRequest request, Action<GetPlayerCustomPropertyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerCustomProperty", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerProfile(GetPlayerProfileRequest request, Action<GetPlayerProfileResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerProfile", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerSegments(GetPlayersSegmentsRequest request, Action<GetPlayerSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayersInSegment(GetPlayersInSegmentRequest request, Action<GetPlayersInSegmentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayersInSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerStatistics(GetPlayerStatisticsRequest request, Action<GetPlayerStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerStatisticVersions(GetPlayerStatisticVersionsRequest request, Action<GetPlayerStatisticVersionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerStatisticVersions", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayerTags(GetPlayerTagsRequest request, Action<GetPlayerTagsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayerTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromBattleNetAccountIds(GetPlayFabIDsFromBattleNetAccountIdsRequest request, Action<GetPlayFabIDsFromBattleNetAccountIdsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromBattleNetAccountIds", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromFacebookIDs(GetPlayFabIDsFromFacebookIDsRequest request, Action<GetPlayFabIDsFromFacebookIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromFacebookIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromFacebookInstantGamesIds(GetPlayFabIDsFromFacebookInstantGamesIdsRequest request, Action<GetPlayFabIDsFromFacebookInstantGamesIdsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromFacebookInstantGamesIds", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromGenericIDs(GetPlayFabIDsFromGenericIDsRequest request, Action<GetPlayFabIDsFromGenericIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromGenericIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromNintendoServiceAccountIds(GetPlayFabIDsFromNintendoServiceAccountIdsRequest request, Action<GetPlayFabIDsFromNintendoServiceAccountIdsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromNintendoServiceAccountIds", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromNintendoSwitchDeviceIds(GetPlayFabIDsFromNintendoSwitchDeviceIdsRequest request, Action<GetPlayFabIDsFromNintendoSwitchDeviceIdsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromNintendoSwitchDeviceIds", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromPSNAccountIDs(GetPlayFabIDsFromPSNAccountIDsRequest request, Action<GetPlayFabIDsFromPSNAccountIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromPSNAccountIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromPSNOnlineIDs(GetPlayFabIDsFromPSNOnlineIDsRequest request, Action<GetPlayFabIDsFromPSNOnlineIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromPSNOnlineIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromSteamIDs(GetPlayFabIDsFromSteamIDsRequest request, Action<GetPlayFabIDsFromSteamIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromSteamIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromSteamNames(GetPlayFabIDsFromSteamNamesRequest request, Action<GetPlayFabIDsFromSteamNamesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromSteamNames", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromTwitchIDs(GetPlayFabIDsFromTwitchIDsRequest request, Action<GetPlayFabIDsFromTwitchIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromTwitchIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPlayFabIDsFromXboxLiveIDs(GetPlayFabIDsFromXboxLiveIDsRequest request, Action<GetPlayFabIDsFromXboxLiveIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPlayFabIDsFromXboxLiveIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetPublisherData(GetPublisherDataRequest request, Action<GetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetRandomResultTables(GetRandomResultTablesRequest request, Action<GetRandomResultTablesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetRandomResultTables", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetServerCustomIDsFromPlayFabIDs(GetServerCustomIDsFromPlayFabIDsRequest request, Action<GetServerCustomIDsFromPlayFabIDsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetServerCustomIDsFromPlayFabIDs", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetSharedGroupData(GetSharedGroupDataRequest request, Action<GetSharedGroupDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetSharedGroupData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetStoreItems(GetStoreItemsServerRequest request, Action<GetStoreItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetStoreItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTime(GetTimeRequest request, Action<GetTimeResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetTime", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTitleData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTitleInternalData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTitleNews(GetTitleNewsRequest request, Action<GetTitleNewsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetTitleNews", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserAccountInfo(GetUserAccountInfoRequest request, Action<GetUserAccountInfoResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserAccountInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserBans(GetUserBansRequest request, Action<GetUserBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserInventory(GetUserInventoryRequest request, Action<GetUserInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserInventory", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserPublisherData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserPublisherInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserPublisherReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetUserReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GetUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GrantCharacterToUser(GrantCharacterToUserRequest request, Action<GrantCharacterToUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GrantCharacterToUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GrantItemsToCharacter(GrantItemsToCharacterRequest request, Action<GrantItemsToCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GrantItemsToCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GrantItemsToUser(GrantItemsToUserRequest request, Action<GrantItemsToUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GrantItemsToUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GrantItemsToUsers(GrantItemsToUsersRequest request, Action<GrantItemsToUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/GrantItemsToUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkNintendoServiceAccount(LinkNintendoServiceAccountRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkNintendoServiceAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkNintendoServiceAccountSubject(LinkNintendoServiceAccountSubjectRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkNintendoServiceAccountSubject", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkNintendoSwitchDeviceId(LinkNintendoSwitchDeviceIdRequest request, Action<LinkNintendoSwitchDeviceIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkNintendoSwitchDeviceId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkPSNAccount(LinkPSNAccountRequest request, Action<LinkPSNAccountResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkPSNAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkPSNId(LinkPSNIdRequest request, Action<LinkPSNIdResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkPSNId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkServerCustomId(LinkServerCustomIdRequest request, Action<LinkServerCustomIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkServerCustomId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkSteamId(LinkSteamIdRequest request, Action<LinkSteamIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkSteamId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LinkXboxAccount(LinkXboxAccountRequest request, Action<LinkXboxAccountResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LinkXboxAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ListPlayerCustomProperties(ListPlayerCustomPropertiesRequest request, Action<ListPlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/ListPlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithAndroidDeviceID(LoginWithAndroidDeviceIDRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithAndroidDeviceID", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithCustomID(LoginWithCustomIDRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithCustomID", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithIOSDeviceID(LoginWithIOSDeviceIDRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithIOSDeviceID", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithPSN(LoginWithPSNRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithPSN", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithServerCustomId(LoginWithServerCustomIdRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithServerCustomId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithSteamId(LoginWithSteamIdRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithSteamId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithXbox(LoginWithXboxRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithXbox", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void LoginWithXboxId(LoginWithXboxIdRequest request, Action<ServerLoginResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/LoginWithXboxId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ModifyItemUses(ModifyItemUsesRequest request, Action<ModifyItemUsesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/ModifyItemUses", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void MoveItemToCharacterFromCharacter(MoveItemToCharacterFromCharacterRequest request, Action<MoveItemToCharacterFromCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/MoveItemToCharacterFromCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void MoveItemToCharacterFromUser(MoveItemToCharacterFromUserRequest request, Action<MoveItemToCharacterFromUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/MoveItemToCharacterFromUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void MoveItemToUserFromCharacter(MoveItemToUserFromCharacterRequest request, Action<MoveItemToUserFromCharacterResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/MoveItemToUserFromCharacter", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RedeemCoupon(RedeemCouponRequest request, Action<RedeemCouponResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RedeemCoupon", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RemoveFriend(RemoveFriendRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RemoveFriend", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RemoveGenericID(RemoveGenericIDRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RemoveGenericID", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RemovePlayerTag(RemovePlayerTagRequest request, Action<RemovePlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RemovePlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RemoveSharedGroupMembers(RemoveSharedGroupMembersRequest request, Action<RemoveSharedGroupMembersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RemoveSharedGroupMembers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ReportPlayer(ReportPlayerServerRequest request, Action<ReportPlayerServerResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/ReportPlayer", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RevokeAllBansForUser(RevokeAllBansForUserRequest request, Action<RevokeAllBansForUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RevokeAllBansForUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RevokeBans(RevokeBansRequest request, Action<RevokeBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RevokeBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RevokeInventoryItem(RevokeInventoryItemRequest request, Action<RevokeInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RevokeInventoryItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RevokeInventoryItems(RevokeInventoryItemsRequest request, Action<RevokeInventoryItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/RevokeInventoryItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SavePushNotificationTemplate(SavePushNotificationTemplateRequest request, Action<SavePushNotificationTemplateResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SavePushNotificationTemplate", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SendCustomAccountRecoveryEmail(SendCustomAccountRecoveryEmailRequest request, Action<SendCustomAccountRecoveryEmailResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SendCustomAccountRecoveryEmail", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SendEmailFromTemplate(SendEmailFromTemplateRequest request, Action<SendEmailFromTemplateResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SendEmailFromTemplate", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SendPushNotification(SendPushNotificationRequest request, Action<SendPushNotificationResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SendPushNotification", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SendPushNotificationFromTemplate(SendPushNotificationFromTemplateRequest request, Action<SendPushNotificationResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SendPushNotificationFromTemplate", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SetFriendTags(SetFriendTagsRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SetFriendTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SetPlayerSecret(SetPlayerSecretRequest request, Action<SetPlayerSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SetPlayerSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SetPublisherData(SetPublisherDataRequest request, Action<SetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SetTitleData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SetTitleInternalData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SubtractCharacterVirtualCurrency(SubtractCharacterVirtualCurrencyRequest request, Action<ModifyCharacterVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SubtractCharacterVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void SubtractUserVirtualCurrency(SubtractUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/SubtractUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkNintendoServiceAccount(UnlinkNintendoServiceAccountRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkNintendoServiceAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkNintendoSwitchDeviceId(UnlinkNintendoSwitchDeviceIdRequest request, Action<UnlinkNintendoSwitchDeviceIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkNintendoSwitchDeviceId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkPSNAccount(UnlinkPSNAccountRequest request, Action<UnlinkPSNAccountResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkPSNAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkServerCustomId(UnlinkServerCustomIdRequest request, Action<UnlinkServerCustomIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkServerCustomId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkSteamId(UnlinkSteamIdRequest request, Action<UnlinkSteamIdResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkSteamId", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlinkXboxAccount(UnlinkXboxAccountRequest request, Action<UnlinkXboxAccountResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlinkXboxAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlockContainerInstance(UnlockContainerInstanceRequest request, Action<UnlockContainerItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlockContainerInstance", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnlockContainerItem(UnlockContainerItemRequest request, Action<UnlockContainerItemResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UnlockContainerItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateAvatarUrl(UpdateAvatarUrlRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateAvatarUrl", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateBans(UpdateBansRequest request, Action<UpdateBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateCharacterData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateCharacterData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateCharacterInternalData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateCharacterInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateCharacterReadOnlyData(UpdateCharacterDataRequest request, Action<UpdateCharacterDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateCharacterReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateCharacterStatistics(UpdateCharacterStatisticsRequest request, Action<UpdateCharacterStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateCharacterStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdatePlayerCustomProperties(UpdatePlayerCustomPropertiesRequest request, Action<UpdatePlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdatePlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdatePlayerStatistics(UpdatePlayerStatisticsRequest request, Action<UpdatePlayerStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdatePlayerStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateSharedGroupData(UpdateSharedGroupDataRequest request, Action<UpdateSharedGroupDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateSharedGroupData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserInventoryItemCustomData(UpdateUserInventoryItemDataRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserInventoryItemCustomData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserPublisherData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserPublisherInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserPublisherReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateUserReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/UpdateUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void WriteCharacterEvent(WriteServerCharacterEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/WriteCharacterEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void WritePlayerEvent(WriteServerPlayerEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/WritePlayerEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void WriteTitleEvent(WriteTitleEventRequest request, Action<WriteEventResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }
            PlayFabHttp.MakeApiCall("/Server/WriteTitleEvent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
