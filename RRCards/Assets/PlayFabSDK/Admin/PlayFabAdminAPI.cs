#if ENABLE_PLAYFABADMIN_API && !DISABLE_PLAYFAB_STATIC_API

using System;
using System.Collections.Generic;
using PlayFab.AdminModels;
using PlayFab.Internal;

namespace PlayFab
{
    public static class PlayFabAdminAPI
    {
        static PlayFabAdminAPI() {}
        public static void ForgetAllCredentials()
        {
            PlayFabSettings.staticPlayer.ForgetAllCredentials();
        }
        public static void AbortTaskInstance(AbortTaskInstanceRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AbortTaskInstance", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void AddLocalizedNews(AddLocalizedNewsRequest request, Action<AddLocalizedNewsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AddLocalizedNews", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void AddNews(AddNewsRequest request, Action<AddNewsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AddNews", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void AddPlayerTag(AddPlayerTagRequest request, Action<AddPlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AddPlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void AddUserVirtualCurrency(AddUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AddUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void AddVirtualCurrencyTypes(AddVirtualCurrencyTypesRequest request, Action<BlankResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/AddVirtualCurrencyTypes", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void BanUsers(BanUsersRequest request, Action<BanUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/BanUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CheckLimitedEditionItemAvailability(CheckLimitedEditionItemAvailabilityRequest request, Action<CheckLimitedEditionItemAvailabilityResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CheckLimitedEditionItemAvailability", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateActionsOnPlayersInSegmentTask(CreateActionsOnPlayerSegmentTaskRequest request, Action<CreateTaskResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreateActionsOnPlayersInSegmentTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateCloudScriptTask(CreateCloudScriptTaskRequest request, Action<CreateTaskResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreateCloudScriptTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateInsightsScheduledScalingTask(CreateInsightsScheduledScalingTaskRequest request, Action<CreateTaskResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreateInsightsScheduledScalingTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateOpenIdConnection(CreateOpenIdConnectionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreateOpenIdConnection", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreatePlayerSharedSecret(CreatePlayerSharedSecretRequest request, Action<CreatePlayerSharedSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreatePlayerSharedSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreatePlayerStatisticDefinition(CreatePlayerStatisticDefinitionRequest request, Action<CreatePlayerStatisticDefinitionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreatePlayerStatisticDefinition", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void CreateSegment(CreateSegmentRequest request, Action<CreateSegmentResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/CreateSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteContent(DeleteContentRequest request, Action<BlankResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteContent", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteMasterPlayerAccount(DeleteMasterPlayerAccountRequest request, Action<DeleteMasterPlayerAccountResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteMasterPlayerAccount", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteMasterPlayerEventData(DeleteMasterPlayerEventDataRequest request, Action<DeleteMasterPlayerEventDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteMasterPlayerEventData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteMembershipSubscription(DeleteMembershipSubscriptionRequest request, Action<DeleteMembershipSubscriptionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteMembershipSubscription", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteOpenIdConnection(DeleteOpenIdConnectionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteOpenIdConnection", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeletePlayer(DeletePlayerRequest request, Action<DeletePlayerResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeletePlayer", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeletePlayerCustomProperties(DeletePlayerCustomPropertiesRequest request, Action<DeletePlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeletePlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeletePlayerSharedSecret(DeletePlayerSharedSecretRequest request, Action<DeletePlayerSharedSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeletePlayerSharedSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteSegment(DeleteSegmentRequest request, Action<DeleteSegmentsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteStore(DeleteStoreRequest request, Action<DeleteStoreResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteStore", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteTask(DeleteTaskRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteTitle(DeleteTitleRequest request, Action<DeleteTitleResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteTitle", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void DeleteTitleDataOverride(DeleteTitleDataOverrideRequest request, Action<DeleteTitleDataOverrideResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/DeleteTitleDataOverride", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ExportMasterPlayerData(ExportMasterPlayerDataRequest request, Action<ExportMasterPlayerDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ExportMasterPlayerData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ExportPlayersInSegment(ExportPlayersInSegmentRequest request, Action<ExportPlayersInSegmentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ExportPlayersInSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetActionsOnPlayersInSegmentTaskInstance(GetTaskInstanceRequest request, Action<GetActionsOnPlayersInSegmentTaskInstanceResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetActionsOnPlayersInSegmentTaskInstance", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetAllSegments(GetAllSegmentsRequest request, Action<GetAllSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetAllSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetCatalogItems(GetCatalogItemsRequest request, Action<GetCatalogItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetCatalogItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetCloudScriptRevision(GetCloudScriptRevisionRequest request, Action<GetCloudScriptRevisionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetCloudScriptRevision", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetCloudScriptTaskInstance(GetTaskInstanceRequest request, Action<GetCloudScriptTaskInstanceResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetCloudScriptTaskInstance", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetCloudScriptVersions(GetCloudScriptVersionsRequest request, Action<GetCloudScriptVersionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetCloudScriptVersions", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetContentList(GetContentListRequest request, Action<GetContentListResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetContentList", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetContentUploadUrl(GetContentUploadUrlRequest request, Action<GetContentUploadUrlResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetContentUploadUrl", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetDataReport(GetDataReportRequest request, Action<GetDataReportResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetDataReport", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayedTitleList(GetPlayedTitleListRequest request, Action<GetPlayedTitleListResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayedTitleList", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerCustomProperty(GetPlayerCustomPropertyRequest request, Action<GetPlayerCustomPropertyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerCustomProperty", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerIdFromAuthToken(GetPlayerIdFromAuthTokenRequest request, Action<GetPlayerIdFromAuthTokenResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerIdFromAuthToken", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerProfile(GetPlayerProfileRequest request, Action<GetPlayerProfileResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerProfile", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerSegments(GetPlayersSegmentsRequest request, Action<GetPlayerSegmentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerSharedSecrets(GetPlayerSharedSecretsRequest request, Action<GetPlayerSharedSecretsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerSharedSecrets", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayersInSegment(GetPlayersInSegmentRequest request, Action<GetPlayersInSegmentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayersInSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerStatisticDefinitions(GetPlayerStatisticDefinitionsRequest request, Action<GetPlayerStatisticDefinitionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerStatisticDefinitions", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerStatisticVersions(GetPlayerStatisticVersionsRequest request, Action<GetPlayerStatisticVersionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerStatisticVersions", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPlayerTags(GetPlayerTagsRequest request, Action<GetPlayerTagsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPlayerTags", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPolicy(GetPolicyRequest request, Action<GetPolicyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPolicy", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetPublisherData(GetPublisherDataRequest request, Action<GetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetRandomResultTables(GetRandomResultTablesRequest request, Action<GetRandomResultTablesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetRandomResultTables", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetSegmentExport(GetPlayersInSegmentExportRequest request, Action<GetPlayersInSegmentExportResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetSegmentExport", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetSegments(GetSegmentsRequest request, Action<GetSegmentsResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetSegments", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetStoreItems(GetStoreItemsRequest request, Action<GetStoreItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetStoreItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTaskInstances(GetTaskInstancesRequest request, Action<GetTaskInstancesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetTaskInstances", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTasks(GetTasksRequest request, Action<GetTasksResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetTasks", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTitleData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetTitleInternalData(GetTitleDataRequest request, Action<GetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserAccountInfo(LookupUserAccountInfoRequest request, Action<LookupUserAccountInfoResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserAccountInfo", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserBans(GetUserBansRequest request, Action<GetUserBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserInventory(GetUserInventoryRequest request, Action<GetUserInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserInventory", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserPublisherData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserPublisherInternalData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserPublisherReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GetUserReadOnlyData(GetUserDataRequest request, Action<GetUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GetUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void GrantItemsToUsers(GrantItemsToUsersRequest request, Action<GrantItemsToUsersResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/GrantItemsToUsers", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void IncrementLimitedEditionItemAvailability(IncrementLimitedEditionItemAvailabilityRequest request, Action<IncrementLimitedEditionItemAvailabilityResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/IncrementLimitedEditionItemAvailability", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void IncrementPlayerStatisticVersion(IncrementPlayerStatisticVersionRequest request, Action<IncrementPlayerStatisticVersionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/IncrementPlayerStatisticVersion", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ListOpenIdConnection(ListOpenIdConnectionRequest request, Action<ListOpenIdConnectionResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ListOpenIdConnection", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ListPlayerCustomProperties(ListPlayerCustomPropertiesRequest request, Action<ListPlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ListPlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ListVirtualCurrencyTypes(ListVirtualCurrencyTypesRequest request, Action<ListVirtualCurrencyTypesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ListVirtualCurrencyTypes", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RefundPurchase(RefundPurchaseRequest request, Action<RefundPurchaseResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RefundPurchase", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RemovePlayerTag(RemovePlayerTagRequest request, Action<RemovePlayerTagResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RemovePlayerTag", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RemoveVirtualCurrencyTypes(RemoveVirtualCurrencyTypesRequest request, Action<BlankResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RemoveVirtualCurrencyTypes", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ResetCharacterStatistics(ResetCharacterStatisticsRequest request, Action<ResetCharacterStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ResetCharacterStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ResetPassword(ResetPasswordRequest request, Action<ResetPasswordResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ResetPassword", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ResetUserStatistics(ResetUserStatisticsRequest request, Action<ResetUserStatisticsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ResetUserStatistics", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void ResolvePurchaseDispute(ResolvePurchaseDisputeRequest request, Action<ResolvePurchaseDisputeResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/ResolvePurchaseDispute", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RevokeAllBansForUser(RevokeAllBansForUserRequest request, Action<RevokeAllBansForUserResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RevokeAllBansForUser", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RevokeBans(RevokeBansRequest request, Action<RevokeBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RevokeBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RevokeInventoryItem(RevokeInventoryItemRequest request, Action<RevokeInventoryResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RevokeInventoryItem", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RevokeInventoryItems(RevokeInventoryItemsRequest request, Action<RevokeInventoryItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RevokeInventoryItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void RunTask(RunTaskRequest request, Action<RunTaskResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/RunTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SendAccountRecoveryEmail(SendAccountRecoveryEmailRequest request, Action<SendAccountRecoveryEmailResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SendAccountRecoveryEmail", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetCatalogItems(UpdateCatalogItemsRequest request, Action<UpdateCatalogItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetCatalogItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetMembershipOverride(SetMembershipOverrideRequest request, Action<SetMembershipOverrideResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetMembershipOverride", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetPlayerSecret(SetPlayerSecretRequest request, Action<SetPlayerSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetPlayerSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetPublishedRevision(SetPublishedRevisionRequest request, Action<SetPublishedRevisionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetPublishedRevision", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetPublisherData(SetPublisherDataRequest request, Action<SetPublisherDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetStoreItems(UpdateStoreItemsRequest request, Action<UpdateStoreItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetStoreItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetTitleData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetTitleData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetTitleDataAndOverrides(SetTitleDataAndOverridesRequest request, Action<SetTitleDataAndOverridesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetTitleDataAndOverrides", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetTitleInternalData(SetTitleDataRequest request, Action<SetTitleDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetTitleInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SetupPushNotification(SetupPushNotificationRequest request, Action<SetupPushNotificationResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SetupPushNotification", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void SubtractUserVirtualCurrency(SubtractUserVirtualCurrencyRequest request, Action<ModifyUserVirtualCurrencyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/SubtractUserVirtualCurrency", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateBans(UpdateBansRequest request, Action<UpdateBansResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateBans", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateCatalogItems(UpdateCatalogItemsRequest request, Action<UpdateCatalogItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateCatalogItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateCloudScript(UpdateCloudScriptRequest request, Action<UpdateCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateCloudScript", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateOpenIdConnection(UpdateOpenIdConnectionRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateOpenIdConnection", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdatePlayerCustomProperties(UpdatePlayerCustomPropertiesRequest request, Action<UpdatePlayerCustomPropertiesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdatePlayerCustomProperties", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdatePlayerSharedSecret(UpdatePlayerSharedSecretRequest request, Action<UpdatePlayerSharedSecretResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdatePlayerSharedSecret", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdatePlayerStatisticDefinition(UpdatePlayerStatisticDefinitionRequest request, Action<UpdatePlayerStatisticDefinitionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdatePlayerStatisticDefinition", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdatePolicy(UpdatePolicyRequest request, Action<UpdatePolicyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdatePolicy", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateRandomResultTables(UpdateRandomResultTablesRequest request, Action<UpdateRandomResultTablesResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateRandomResultTables", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateSegment(UpdateSegmentRequest request, Action<UpdateSegmentResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateSegment", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateStoreItems(UpdateStoreItemsRequest request, Action<UpdateStoreItemsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateStoreItems", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateTask(UpdateTaskRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateTask", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserPublisherData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserPublisherData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserPublisherInternalData(UpdateUserInternalDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserPublisherInternalData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserPublisherReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserPublisherReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserReadOnlyData(UpdateUserDataRequest request, Action<UpdateUserDataResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserReadOnlyData", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }
        public static void UpdateUserTitleDisplayName(UpdateUserTitleDisplayNameRequest request, Action<UpdateUserTitleDisplayNameResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? PlayFabSettings.staticPlayer;
            var callSettings = PlayFabSettings.staticSettings;
            if (string.IsNullOrEmpty(callSettings.DeveloperSecretKey)) { throw new PlayFabException(PlayFabExceptionCode.DeveloperKeyNotSet, "Must set DeveloperSecretKey in settings to call this method"); }


            PlayFabHttp.MakeApiCall("/Admin/UpdateUserTitleDisplayName", request, AuthType.DevSecretKey, resultCallback, errorCallback, customData, extraHeaders, context, callSettings);
        }


    }
}

#endif
