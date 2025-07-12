#if !DISABLE_PLAYFABENTITY_API

using System;
using System.Collections.Generic;
using PlayFab.ExperimentationModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabExperimentationInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabExperimentationInstanceAPI(PlayFabAuthenticationContext context)
        {
            if (context == null)
                throw new PlayFabException(PlayFabExceptionCode.AuthContextRequired, "Context cannot be null, create a PlayFabAuthenticationContext for each player in advance, or call <PlayFabClientInstanceAPI>.GetAuthenticationContext()");
            authenticationContext = context;
        }

        public PlayFabExperimentationInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
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
        public void CreateExclusionGroup(CreateExclusionGroupRequest request, Action<CreateExclusionGroupResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/CreateExclusionGroup", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void CreateExperiment(CreateExperimentRequest request, Action<CreateExperimentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/CreateExperiment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteExclusionGroup(DeleteExclusionGroupRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/DeleteExclusionGroup", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void DeleteExperiment(DeleteExperimentRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/DeleteExperiment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetExclusionGroups(GetExclusionGroupsRequest request, Action<GetExclusionGroupsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/GetExclusionGroups", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetExclusionGroupTraffic(GetExclusionGroupTrafficRequest request, Action<GetExclusionGroupTrafficResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/GetExclusionGroupTraffic", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetExperiments(GetExperimentsRequest request, Action<GetExperimentsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/GetExperiments", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetLatestScorecard(GetLatestScorecardRequest request, Action<GetLatestScorecardResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/GetLatestScorecard", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetTreatmentAssignment(GetTreatmentAssignmentRequest request, Action<GetTreatmentAssignmentResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/GetTreatmentAssignment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void StartExperiment(StartExperimentRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/StartExperiment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void StopExperiment(StopExperimentRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/StopExperiment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateExclusionGroup(UpdateExclusionGroupRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/UpdateExclusionGroup", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UpdateExperiment(UpdateExperimentRequest request, Action<EmptyResponse> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/Experimentation/UpdateExperiment", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
