#if !DISABLE_PLAYFABENTITY_API

using System;
using System.Collections.Generic;
using PlayFab.CloudScriptModels;
using PlayFab.Internal;
using PlayFab.SharedModels;

namespace PlayFab
{
    public class PlayFabCloudScriptInstanceAPI : IPlayFabInstanceApi
    {
        public readonly PlayFabApiSettings apiSettings = null;
        public readonly PlayFabAuthenticationContext authenticationContext = null;

        public PlayFabCloudScriptInstanceAPI(PlayFabAuthenticationContext context)
        {
            if (context == null)
                throw new PlayFabException(PlayFabExceptionCode.AuthContextRequired, "Context cannot be null, create a PlayFabAuthenticationContext for each player in advance, or call <PlayFabClientInstanceAPI>.GetAuthenticationContext()");
            authenticationContext = context;
        }

        public PlayFabCloudScriptInstanceAPI(PlayFabApiSettings settings, PlayFabAuthenticationContext context)
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
        public void ExecuteEntityCloudScript(ExecuteEntityCloudScriptRequest request, Action<ExecuteCloudScriptResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ExecuteEntityCloudScript", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ExecuteFunction(ExecuteFunctionRequest request, Action<ExecuteFunctionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ExecuteFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void GetFunction(GetFunctionRequest request, Action<GetFunctionResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/GetFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ListEventHubFunctions(ListFunctionsRequest request, Action<ListEventHubFunctionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ListEventHubFunctions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ListFunctions(ListFunctionsRequest request, Action<ListFunctionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ListFunctions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ListHttpFunctions(ListFunctionsRequest request, Action<ListHttpFunctionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ListHttpFunctions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void ListQueuedFunctions(ListFunctionsRequest request, Action<ListQueuedFunctionsResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/ListQueuedFunctions", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void PostFunctionResultForEntityTriggeredAction(PostFunctionResultForEntityTriggeredActionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/PostFunctionResultForEntityTriggeredAction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void PostFunctionResultForFunctionExecution(PostFunctionResultForFunctionExecutionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/PostFunctionResultForFunctionExecution", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void PostFunctionResultForPlayerTriggeredAction(PostFunctionResultForPlayerTriggeredActionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/PostFunctionResultForPlayerTriggeredAction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void PostFunctionResultForScheduledTask(PostFunctionResultForScheduledTaskRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/PostFunctionResultForScheduledTask", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RegisterEventHubFunction(RegisterEventHubFunctionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/RegisterEventHubFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RegisterHttpFunction(RegisterHttpFunctionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/RegisterHttpFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void RegisterQueuedFunction(RegisterQueuedFunctionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/RegisterQueuedFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }
        public void UnregisterFunction(UnregisterFunctionRequest request, Action<EmptyResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
        {
            var context = (request == null ? null : request.AuthenticationContext) ?? authenticationContext;
            var callSettings = apiSettings ?? PlayFabSettings.staticSettings;
            if (!context.IsEntityLoggedIn()) throw new PlayFabException(PlayFabExceptionCode.NotLoggedIn,"Must be logged in to call this method");
            PlayFabHttp.MakeApiCall("/CloudScript/UnregisterFunction", request, AuthType.EntityToken, resultCallback, errorCallback, customData, extraHeaders, context, callSettings, this);
        }

    }
}

#endif
