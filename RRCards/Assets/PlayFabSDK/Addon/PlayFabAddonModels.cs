#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.AddonModels
{
    [Serializable]
    public class CreateOrUpdateAppleRequest : PlayFabRequestCommon
    {
        public string AppBundleId;
        public string AppSharedSecret;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public bool? IgnoreExpirationDate;
        public bool? RequireSecureAuthentication;
    }

    [Serializable]
    public class CreateOrUpdateAppleResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateFacebookInstantGamesRequest : PlayFabRequestCommon
    {
        public string AppID;
        public string AppSecret;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
    }

    [Serializable]
    public class CreateOrUpdateFacebookInstantGamesResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateFacebookRequest : PlayFabRequestCommon
    {
        public string AppID;
        public string AppSecret;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public string NotificationEmail;
    }

    [Serializable]
    public class CreateOrUpdateFacebookResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateGoogleRequest : PlayFabRequestCommon
    {
        public string AppLicenseKey;
        public string AppPackageID;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public string OAuthClientID;
        public string OAuthClientSecret;
        public string OAuthCustomRedirectUri;
        public string ServiceAccountKey;
    }

    [Serializable]
    public class CreateOrUpdateGoogleResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateKongregateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public string SecretAPIKey;
    }

    [Serializable]
    public class CreateOrUpdateKongregateResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateNintendoRequest : PlayFabRequestCommon
    {
        public string ApplicationID;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<NintendoEnvironment> Environments;
        public bool? ErrorIfExists;
    }

    [Serializable]
    public class CreateOrUpdateNintendoResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdatePSNRequest : PlayFabRequestCommon
    {
        public string ClientID;
        public string ClientSecret;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public string NextGenClientID;
        public string NextGenClientSecret;
    }

    [Serializable]
    public class CreateOrUpdatePSNResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateSteamRequest : PlayFabRequestCommon
    {
        public string ApplicationId;
        public Dictionary<string,string> CustomTags;
        public bool? EnforceServiceSpecificTickets;
        public EntityKey Entity;
        public bool? ErrorIfExists;
        public string SecretKey;
        public bool? UseSandbox;
    }

    [Serializable]
    public class CreateOrUpdateSteamResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class CreateOrUpdateTwitchRequest : PlayFabRequestCommon
    {
        public string ClientID;
        public string ClientSecret;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? ErrorIfExists;
    }

    [Serializable]
    public class CreateOrUpdateTwitchResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteAppleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteAppleResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteFacebookInstantGamesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteFacebookInstantGamesResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteFacebookRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteFacebookResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteGoogleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteGoogleResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteKongregateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteKongregateResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteNintendoRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteNintendoResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeletePSNRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeletePSNResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteSteamRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteSteamResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteTwitchRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteTwitchResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }

    [Serializable]
    public class GetAppleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetAppleResponse : PlayFabResultCommon
    {
        public string AppBundleId;
        public bool Created;
        public bool? IgnoreExpirationDate;
        public bool? RequireSecureAuthentication;
    }

    [Serializable]
    public class GetFacebookInstantGamesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetFacebookInstantGamesResponse : PlayFabResultCommon
    {
        public string AppID;
        public bool Created;
    }

    [Serializable]
    public class GetFacebookRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetFacebookResponse : PlayFabResultCommon
    {
        public string AppID;
        public bool Created;
        public string NotificationEmail;
    }

    [Serializable]
    public class GetGoogleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetGoogleResponse : PlayFabResultCommon
    {
        public string AppPackageID;
        public bool Created;
        public string OAuthClientID;
        public string OauthCustomRedirectUri;
    }

    [Serializable]
    public class GetKongregateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetKongregateResponse : PlayFabResultCommon
    {
        public bool Created;
    }

    [Serializable]
    public class GetNintendoRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetNintendoResponse : PlayFabResultCommon
    {
        public string ApplicationID;
        public bool Created;
        public List<NintendoEnvironment> Environments;
    }

    [Serializable]
    public class GetPSNRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetPSNResponse : PlayFabResultCommon
    {
        public string ClientID;
        public bool Created;
        public string NextGenClientID;
    }

    [Serializable]
    public class GetSteamRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetSteamResponse : PlayFabResultCommon
    {
        public string ApplicationId;
        public bool Created;
        public bool? EnforceServiceSpecificTickets;
        public bool? UseSandbox;
    }

    [Serializable]
    public class GetTwitchRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetTwitchResponse : PlayFabResultCommon
    {
        public string ClientID;
        public bool Created;
    }

    [Serializable]
    public class NintendoEnvironment : PlayFabBaseModel
    {
        public string ClientID;
        public string ClientSecret;
        public string ID;
    }
}
#endif
