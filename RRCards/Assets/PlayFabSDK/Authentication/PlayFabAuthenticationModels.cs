#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.AuthenticationModels
{
    [Serializable]
    public class AuthenticateCustomIdRequest : PlayFabRequestCommon
    {
        public string CustomId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class AuthenticateCustomIdResult : PlayFabResultCommon
    {
        public EntityTokenResponse EntityToken;
        public bool NewlyCreated;
    }
    [Serializable]
    public class DeleteRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class EmptyResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }

    [Serializable]
    public class EntityLineage : PlayFabBaseModel
    {
        public string CharacterId;
        public string GroupId;
        public string MasterPlayerAccountId;
        public string NamespaceId;
        public string TitleId;
        public string TitlePlayerAccountId;
    }

    [Serializable]
    public class EntityTokenResponse : PlayFabBaseModel
    {
        public EntityKey Entity;
        public string EntityToken;
        public DateTime? TokenExpiration;
    }
    [Serializable]
    public class GetEntityTokenRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetEntityTokenResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public string EntityToken;
        public DateTime? TokenExpiration;
    }

    public enum IdentifiedDeviceType
    {
        Unknown,
        XboxOne,
        Scarlett,
        WindowsOneCore,
        WindowsOneCoreMobile,
        Win32,
        android,
        iOS,
        PlayStation,
        Nintendo
    }

    public enum LoginIdentityProvider
    {
        Unknown,
        PlayFab,
        Custom,
        GameCenter,
        GooglePlay,
        Steam,
        XBoxLive,
        PSN,
        Kongregate,
        Facebook,
        IOSDevice,
        AndroidDevice,
        Twitch,
        WindowsHello,
        GameServer,
        CustomServer,
        NintendoSwitch,
        FacebookInstantGames,
        OpenIdConnect,
        Apple,
        NintendoSwitchAccount,
        GooglePlayGames,
        XboxMobileStore,
        King,
        BattleNet
    }
    [Serializable]
    public class ValidateEntityTokenRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string EntityToken;
    }

    [Serializable]
    public class ValidateEntityTokenResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public IdentifiedDeviceType? IdentifiedDeviceType;
        public LoginIdentityProvider? IdentityProvider;
        public string IdentityProviderIssuedId;
        public EntityLineage Lineage;
    }
}
#endif
