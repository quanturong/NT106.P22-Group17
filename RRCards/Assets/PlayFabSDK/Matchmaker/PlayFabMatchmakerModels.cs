#if ENABLE_PLAYFABSERVER_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.MatchmakerModels
{
    [Serializable]
    public class AuthUserRequest : PlayFabRequestCommon
    {
        public string AuthorizationTicket;
    }

    [Serializable]
    public class AuthUserResponse : PlayFabResultCommon
    {
        public bool Authorized;
        public string PlayFabId;
    }
    [Serializable]
    public class ItemInstance : PlayFabBaseModel
    {
        public string Annotation;
        public List<string> BundleContents;
        public string BundleParent;
        public string CatalogVersion;
        public Dictionary<string,string> CustomData;
        public string DisplayName;
        public DateTime? Expiration;
        public string ItemClass;
        public string ItemId;
        public string ItemInstanceId;
        public DateTime? PurchaseDate;
        public int? RemainingUses;
        public string UnitCurrency;
        public uint UnitPrice;
        public int? UsesIncrementedBy;
    }

    [Serializable]
    public class PlayerJoinedRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public string PlayFabId;
    }

    [Serializable]
    public class PlayerJoinedResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class PlayerLeftRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public string PlayFabId;
    }

    [Serializable]
    public class PlayerLeftResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UserInfoRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int MinCatalogVersion;
        public string PlayFabId;
    }

    [Serializable]
    public class UserInfoResponse : PlayFabResultCommon
    {
        public List<ItemInstance> Inventory;
        public bool IsDeveloper;
        public string PlayFabId;
        public string SteamId;
        public string TitleDisplayName;
        public string Username;
        public Dictionary<string,int> VirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
    }

    [Serializable]
    public class VirtualCurrencyRechargeTime : PlayFabBaseModel
    {
        public int RechargeMax;
        public DateTime RechargeTime;
        public int SecondsToRecharge;
    }
}
#endif
