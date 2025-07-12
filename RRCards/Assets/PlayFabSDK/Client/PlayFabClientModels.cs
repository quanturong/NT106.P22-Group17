#if !DISABLE_PLAYFABCLIENT_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.ClientModels
{
    [Serializable]
    public class AcceptTradeRequest : PlayFabRequestCommon
    {
        public List<string> AcceptedInventoryInstanceIds;
        public string OfferingPlayerId;
        public string TradeId;
    }

    [Serializable]
    public class AcceptTradeResponse : PlayFabResultCommon
    {
        public TradeInfo Trade;
    }

    public enum AdActivity
    {
        Opened,
        Closed,
        Start,
        End
    }

    [Serializable]
    public class AdCampaignAttributionModel : PlayFabBaseModel
    {
        public DateTime AttributedAt;
        public string CampaignId;
        public string Platform;
    }

    [Serializable]
    public class AddFriendRequest : PlayFabRequestCommon
    {
        public string FriendEmail;
        public string FriendPlayFabId;
        public string FriendTitleDisplayName;
        public string FriendUsername;
    }

    [Serializable]
    public class AddFriendResult : PlayFabResultCommon
    {
        public bool Created;
    }

    [Serializable]
    public class AddGenericIDRequest : PlayFabRequestCommon
    {
        public GenericServiceId GenericId;
    }

    [Serializable]
    public class AddGenericIDResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class AddOrUpdateContactEmailRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string EmailAddress;
    }

    [Serializable]
    public class AddOrUpdateContactEmailResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class AddSharedGroupMembersRequest : PlayFabRequestCommon
    {
        public List<string> PlayFabIds;
        public string SharedGroupId;
    }

    [Serializable]
    public class AddSharedGroupMembersResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class AddUsernamePasswordRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Email;
        public string Password;
        public string Username;
    }
    [Serializable]
    public class AddUsernamePasswordResult : PlayFabResultCommon
    {
        public string Username;
    }
    [Serializable]
    public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public Dictionary<string,string> CustomTags;
        public string VirtualCurrency;
    }
    [Serializable]
    public class AdPlacementDetails : PlayFabBaseModel
    {
        public string PlacementId;
        public string PlacementName;
        public int? PlacementViewsRemaining;
        public double? PlacementViewsResetMinutes;
        public string RewardAssetUrl;
        public string RewardDescription;
        public string RewardId;
        public string RewardName;
    }
    [Serializable]
    public class AdRewardItemGranted : PlayFabBaseModel
    {
        public string CatalogId;
        public string DisplayName;
        public string InstanceId;
        public string ItemId;
    }
    [Serializable]
    public class AdRewardResults : PlayFabBaseModel
    {
        public List<AdRewardItemGranted> GrantedItems;
        public Dictionary<string,int> GrantedVirtualCurrencies;
        public Dictionary<string,int> IncrementedStatistics;
    }
    [Serializable]
    public class AndroidDevicePushNotificationRegistrationRequest : PlayFabRequestCommon
    {
        public string ConfirmationMessage;
        public string DeviceToken;
        public bool? SendPushNotificationConfirmation;
    }

    [Serializable]
    public class AndroidDevicePushNotificationRegistrationResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class AttributeInstallRequest : PlayFabRequestCommon
    {
        public string Adid;
        public string Idfa;
    }

    [Serializable]
    public class AttributeInstallResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class BattleNetAccountPlayFabIdPair : PlayFabBaseModel
    {
        public string BattleNetAccountId;
        public string PlayFabId;
    }

    [Serializable]
    public class CancelTradeRequest : PlayFabRequestCommon
    {
        public string TradeId;
    }

    [Serializable]
    public class CancelTradeResponse : PlayFabResultCommon
    {
        public TradeInfo Trade;
    }

    [Serializable]
    public class CartItem : PlayFabBaseModel
    {
        public string Description;
        public string DisplayName;
        public string ItemClass;
        public string ItemId;
        public string ItemInstanceId;
        public Dictionary<string,uint> RealCurrencyPrices;
        public Dictionary<string,uint> VCAmount;
        public Dictionary<string,uint> VirtualCurrencyPrices;
    }
    [Serializable]
    public class CatalogItem : PlayFabBaseModel
    {
        public CatalogItemBundleInfo Bundle;
        public bool CanBecomeCharacter;
        public string CatalogVersion;
        public CatalogItemConsumableInfo Consumable;
        public CatalogItemContainerInfo Container;
        public string CustomData;
        public string Description;
        public string DisplayName;
        public int InitialLimitedEditionCount;
        public bool IsLimitedEdition;
        public bool IsStackable;
        public bool IsTradable;
        public string ItemClass;
        public string ItemId;
        public string ItemImageUrl;
        public Dictionary<string,uint> RealCurrencyPrices;
        public List<string> Tags;
        public Dictionary<string,uint> VirtualCurrencyPrices;
    }

    [Serializable]
    public class CatalogItemBundleInfo : PlayFabBaseModel
    {
        public List<string> BundledItems;
        public List<string> BundledResultTables;
        public Dictionary<string,uint> BundledVirtualCurrencies;
    }

    [Serializable]
    public class CatalogItemConsumableInfo : PlayFabBaseModel
    {
        public uint? UsageCount;
        public uint? UsagePeriod;
        public string UsagePeriodGroup;
    }
    [Serializable]
    public class CatalogItemContainerInfo : PlayFabBaseModel
    {
        public List<string> ItemContents;
        public string KeyItemId;
        public List<string> ResultTableContents;
        public Dictionary<string,uint> VirtualCurrencyContents;
    }

    [Serializable]
    public class CharacterInventory : PlayFabBaseModel
    {
        public string CharacterId;
        public List<ItemInstance> Inventory;
    }

    [Serializable]
    public class CharacterLeaderboardEntry : PlayFabBaseModel
    {
        public string CharacterId;
        public string CharacterName;
        public string CharacterType;
        public string DisplayName;
        public string PlayFabId;
        public int Position;
        public int StatValue;
    }

    [Serializable]
    public class CharacterResult : PlayFabBaseModel
    {
        public string CharacterId;
        public string CharacterName;
        public string CharacterType;
    }

    public enum CloudScriptRevisionOption
    {
        Live,
        Latest,
        Specific
    }
    [Serializable]
    public class ConfirmPurchaseRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string OrderId;
    }
    [Serializable]
    public class ConfirmPurchaseResult : PlayFabResultCommon
    {
        public List<ItemInstance> Items;
        public string OrderId;
        public DateTime PurchaseDate;
    }

    [Serializable]
    public class ConsumeItemRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public int ConsumeCount;
        public Dictionary<string,string> CustomTags;
        public string ItemInstanceId;
    }

    [Serializable]
    public class ConsumeItemResult : PlayFabResultCommon
    {
        public string ItemInstanceId;
        public int RemainingUses;
    }

    [Serializable]
    public class ConsumeMicrosoftStoreEntitlementsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public MicrosoftStorePayload MarketplaceSpecificData;
    }

    [Serializable]
    public class ConsumeMicrosoftStoreEntitlementsResponse : PlayFabResultCommon
    {
        public List<ItemInstance> Items;
    }

    [Serializable]
    public class ConsumePS5EntitlementsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public PlayStation5Payload MarketplaceSpecificData;
    }

    [Serializable]
    public class ConsumePS5EntitlementsResult : PlayFabResultCommon
    {
        public List<ItemInstance> Items;
    }

    [Serializable]
    public class ConsumePSNEntitlementsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public int ServiceLabel;
    }

    [Serializable]
    public class ConsumePSNEntitlementsResult : PlayFabResultCommon
    {
        public List<ItemInstance> ItemsGranted;
    }

    [Serializable]
    public class ConsumeXboxEntitlementsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public string XboxToken;
    }

    [Serializable]
    public class ConsumeXboxEntitlementsResult : PlayFabResultCommon
    {
        public List<ItemInstance> Items;
    }

    [Serializable]
    public class ContactEmailInfoModel : PlayFabBaseModel
    {
        public string EmailAddress;
        public string Name;
        public EmailVerificationStatus? VerificationStatus;
    }

    public enum ContinentCode
    {
        AF,
        AN,
        AS,
        EU,
        NA,
        OC,
        SA,
        Unknown
    }

    public enum CountryCode
    {
        AF,
        AX,
        AL,
        DZ,
        AS,
        AD,
        AO,
        AI,
        AQ,
        AG,
        AR,
        AM,
        AW,
        AU,
        AT,
        AZ,
        BS,
        BH,
        BD,
        BB,
        BY,
        BE,
        BZ,
        BJ,
        BM,
        BT,
        BO,
        BQ,
        BA,
        BW,
        BV,
        BR,
        IO,
        BN,
        BG,
        BF,
        BI,
        KH,
        CM,
        CA,
        CV,
        KY,
        CF,
        TD,
        CL,
        CN,
        CX,
        CC,
        CO,
        KM,
        CG,
        CD,
        CK,
        CR,
        CI,
        HR,
        CU,
        CW,
        CY,
        CZ,
        DK,
        DJ,
        DM,
        DO,
        EC,
        EG,
        SV,
        GQ,
        ER,
        EE,
        ET,
        FK,
        FO,
        FJ,
        FI,
        FR,
        GF,
        PF,
        TF,
        GA,
        GM,
        GE,
        DE,
        GH,
        GI,
        GR,
        GL,
        GD,
        GP,
        GU,
        GT,
        GG,
        GN,
        GW,
        GY,
        HT,
        HM,
        VA,
        HN,
        HK,
        HU,
        IS,
        IN,
        ID,
        IR,
        IQ,
        IE,
        IM,
        IL,
        IT,
        JM,
        JP,
        JE,
        JO,
        KZ,
        KE,
        KI,
        KP,
        KR,
        KW,
        KG,
        LA,
        LV,
        LB,
        LS,
        LR,
        LY,
        LI,
        LT,
        LU,
        MO,
        MK,
        MG,
        MW,
        MY,
        MV,
        ML,
        MT,
        MH,
        MQ,
        MR,
        MU,
        YT,
        MX,
        FM,
        MD,
        MC,
        MN,
        ME,
        MS,
        MA,
        MZ,
        MM,
        NA,
        NR,
        NP,
        NL,
        NC,
        NZ,
        NI,
        NE,
        NG,
        NU,
        NF,
        MP,
        NO,
        OM,
        PK,
        PW,
        PS,
        PA,
        PG,
        PY,
        PE,
        PH,
        PN,
        PL,
        PT,
        PR,
        QA,
        RE,
        RO,
        RU,
        RW,
        BL,
        SH,
        KN,
        LC,
        MF,
        PM,
        VC,
        WS,
        SM,
        ST,
        SA,
        SN,
        RS,
        SC,
        SL,
        SG,
        SX,
        SK,
        SI,
        SB,
        SO,
        ZA,
        GS,
        SS,
        ES,
        LK,
        SD,
        SR,
        SJ,
        SZ,
        SE,
        CH,
        SY,
        TW,
        TJ,
        TZ,
        TH,
        TL,
        TG,
        TK,
        TO,
        TT,
        TN,
        TR,
        TM,
        TC,
        TV,
        UG,
        UA,
        AE,
        GB,
        US,
        UM,
        UY,
        UZ,
        VU,
        VE,
        VN,
        VG,
        VI,
        WF,
        EH,
        YE,
        ZM,
        ZW,
        Unknown
    }
    [Serializable]
    public class CreateSharedGroupRequest : PlayFabRequestCommon
    {
        public string SharedGroupId;
    }

    [Serializable]
    public class CreateSharedGroupResult : PlayFabResultCommon
    {
        public string SharedGroupId;
    }

    public enum Currency
    {
        AED,
        AFN,
        ALL,
        AMD,
        ANG,
        AOA,
        ARS,
        AUD,
        AWG,
        AZN,
        BAM,
        BBD,
        BDT,
        BGN,
        BHD,
        BIF,
        BMD,
        BND,
        BOB,
        BRL,
        BSD,
        BTN,
        BWP,
        BYR,
        BZD,
        CAD,
        CDF,
        CHF,
        CLP,
        CNY,
        COP,
        CRC,
        CUC,
        CUP,
        CVE,
        CZK,
        DJF,
        DKK,
        DOP,
        DZD,
        EGP,
        ERN,
        ETB,
        EUR,
        FJD,
        FKP,
        GBP,
        GEL,
        GGP,
        GHS,
        GIP,
        GMD,
        GNF,
        GTQ,
        GYD,
        HKD,
        HNL,
        HRK,
        HTG,
        HUF,
        IDR,
        ILS,
        IMP,
        INR,
        IQD,
        IRR,
        ISK,
        JEP,
        JMD,
        JOD,
        JPY,
        KES,
        KGS,
        KHR,
        KMF,
        KPW,
        KRW,
        KWD,
        KYD,
        KZT,
        LAK,
        LBP,
        LKR,
        LRD,
        LSL,
        LYD,
        MAD,
        MDL,
        MGA,
        MKD,
        MMK,
        MNT,
        MOP,
        MRO,
        MUR,
        MVR,
        MWK,
        MXN,
        MYR,
        MZN,
        NAD,
        NGN,
        NIO,
        NOK,
        NPR,
        NZD,
        OMR,
        PAB,
        PEN,
        PGK,
        PHP,
        PKR,
        PLN,
        PYG,
        QAR,
        RON,
        RSD,
        RUB,
        RWF,
        SAR,
        SBD,
        SCR,
        SDG,
        SEK,
        SGD,
        SHP,
        SLL,
        SOS,
        SPL,
        SRD,
        STD,
        SVC,
        SYP,
        SZL,
        THB,
        TJS,
        TMT,
        TND,
        TOP,
        TRY,
        TTD,
        TVD,
        TWD,
        TZS,
        UAH,
        UGX,
        USD,
        UYU,
        UZS,
        VEF,
        VND,
        VUV,
        WST,
        XAF,
        XCD,
        XDR,
        XOF,
        XPF,
        YER,
        ZAR,
        ZMW,
        ZWD
    }

    [Serializable]
    public class CustomPropertyDetails : PlayFabBaseModel
    {
        public string Name;
        public object Value;
    }

    [Serializable]
    public class DeletedPropertyDetails : PlayFabBaseModel
    {
        public string Name;
        public bool WasDeleted;
    }
    [Serializable]
    public class DeletePlayerCustomPropertiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? ExpectedPropertiesVersion;
        public List<string> PropertyNames;
    }

    [Serializable]
    public class DeletePlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public List<DeletedPropertyDetails> DeletedProperties;
        public int PropertiesVersion;
    }
    [Serializable]
    public class DeviceInfoRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Info;
    }

    public enum EmailVerificationStatus
    {
        Unverified,
        Pending,
        Confirmed
    }

    [Serializable]
    public class EmptyResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class EmptyResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }

    [Serializable]
    public class EntityTokenResponse : PlayFabBaseModel
    {
        public EntityKey Entity;
        public string EntityToken;
        public DateTime? TokenExpiration;
    }

    [Serializable]
    public class ExecuteCloudScriptRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
        public object FunctionParameter;
        public bool? GeneratePlayStreamEvent;
        public CloudScriptRevisionOption? RevisionSelection;
        public int? SpecificRevision;
    }

    [Serializable]
    public class ExecuteCloudScriptResult : PlayFabResultCommon
    {
        public int APIRequestsIssued;
        public ScriptExecutionError Error;
        public double ExecutionTimeSeconds;
        public string FunctionName;
        public object FunctionResult;
        public bool? FunctionResultTooLarge;
        public int HttpRequestsIssued;
        public List<LogStatement> Logs;
        public bool? LogsTooLarge;
        public uint MemoryConsumedBytes;
        public double ProcessorTimeSeconds;
        public int Revision;
    }

    public enum ExternalFriendSources
    {
        None,
        Steam,
        Facebook,
        Xbox,
        Psn,
        All
    }

    [Serializable]
    public class FacebookInstantGamesPlayFabIdPair : PlayFabBaseModel
    {
        public string FacebookInstantGamesId;
        public string PlayFabId;
    }

    [Serializable]
    public class FacebookPlayFabIdPair : PlayFabBaseModel
    {
        public string FacebookId;
        public string PlayFabId;
    }

    [Serializable]
    public class FriendInfo : PlayFabBaseModel
    {
        public UserFacebookInfo FacebookInfo;
        public string FriendPlayFabId;
        public UserGameCenterInfo GameCenterInfo;
        public PlayerProfileModel Profile;
        public UserPsnInfo PSNInfo;
        public UserSteamInfo SteamInfo;
        public List<string> Tags;
        public string TitleDisplayName;
        public string Username;
        public UserXboxInfo XboxInfo;
    }

    [Serializable]
    public class GameCenterPlayFabIdPair : PlayFabBaseModel
    {
        public string GameCenterId;
        public string PlayFabId;
    }

    [Serializable]
    public class GenericPlayFabIdPair : PlayFabBaseModel
    {
        public GenericServiceId GenericId;
        public string PlayFabId;
    }

    [Serializable]
    public class GenericServiceId : PlayFabBaseModel
    {
        public string ServiceName;
        public string UserId;
    }

    [Serializable]
    public class GetAccountInfoRequest : PlayFabRequestCommon
    {
        public string Email;
        public string PlayFabId;
        public string TitleDisplayName;
        public string Username;
    }
    [Serializable]
    public class GetAccountInfoResult : PlayFabResultCommon
    {
        public UserAccountInfo AccountInfo;
    }
    [Serializable]
    public class GetAdPlacementsRequest : PlayFabRequestCommon
    {
        public string AppId;
        public NameIdentifier Identifier;
    }
    [Serializable]
    public class GetAdPlacementsResult : PlayFabResultCommon
    {
        public List<AdPlacementDetails> AdPlacements;
    }

    [Serializable]
    public class GetCatalogItemsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
    }
    [Serializable]
    public class GetCatalogItemsResult : PlayFabResultCommon
    {
        public List<CatalogItem> Catalog;
    }
    [Serializable]
    public class GetCharacterDataRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public uint? IfChangedFromDataVersion;
        public List<string> Keys;
        public string PlayFabId;
    }

    [Serializable]
    public class GetCharacterDataResult : PlayFabResultCommon
    {
        public string CharacterId;
        public Dictionary<string,UserDataRecord> Data;
        public uint DataVersion;
    }
    [Serializable]
    public class GetCharacterInventoryRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetCharacterInventoryResult : PlayFabResultCommon
    {
        public string CharacterId;
        public List<ItemInstance> Inventory;
        public Dictionary<string,int> VirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
    }

    [Serializable]
    public class GetCharacterLeaderboardRequest : PlayFabRequestCommon
    {
        public int? MaxResultsCount;
        public int StartPosition;
        public string StatisticName;
    }
    [Serializable]
    public class GetCharacterLeaderboardResult : PlayFabResultCommon
    {
        public List<CharacterLeaderboardEntry> Leaderboard;
    }

    [Serializable]
    public class GetCharacterStatisticsRequest : PlayFabRequestCommon
    {
        public string CharacterId;
    }
    [Serializable]
    public class GetCharacterStatisticsResult : PlayFabResultCommon
    {
        public Dictionary<string,int> CharacterStatistics;
    }

    [Serializable]
    public class GetContentDownloadUrlRequest : PlayFabRequestCommon
    {
        public string HttpMethod;
        public string Key;
        public bool? ThruCDN;
    }

    [Serializable]
    public class GetContentDownloadUrlResult : PlayFabResultCommon
    {
        public string URL;
    }

    [Serializable]
    public class GetFriendLeaderboardAroundPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExternalFriendSources? ExternalPlatformFriends;
        public int? MaxResultsCount;
        public string PlayFabId;
        public PlayerProfileViewConstraints ProfileConstraints;
        public string StatisticName;
        public int? Version;
        public string XboxToken;
    }
    [Serializable]
    public class GetFriendLeaderboardAroundPlayerResult : PlayFabResultCommon
    {
        public List<PlayerLeaderboardEntry> Leaderboard;
        public DateTime? NextReset;
        public int Version;
    }

    [Serializable]
    public class GetFriendLeaderboardRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExternalFriendSources? ExternalPlatformFriends;
        public int? MaxResultsCount;
        public PlayerProfileViewConstraints ProfileConstraints;
        public int StartPosition;
        public string StatisticName;
        public int? Version;
        public string XboxToken;
    }

    [Serializable]
    public class GetFriendsListRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExternalFriendSources? ExternalPlatformFriends;
        public PlayerProfileViewConstraints ProfileConstraints;
        public string XboxToken;
    }
    [Serializable]
    public class GetFriendsListResult : PlayFabResultCommon
    {
        public List<FriendInfo> Friends;
    }

    [Serializable]
    public class GetLeaderboardAroundCharacterRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public int? MaxResultsCount;
        public string StatisticName;
    }
    [Serializable]
    public class GetLeaderboardAroundCharacterResult : PlayFabResultCommon
    {
        public List<CharacterLeaderboardEntry> Leaderboard;
    }

    [Serializable]
    public class GetLeaderboardAroundPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? MaxResultsCount;
        public string PlayFabId;
        public PlayerProfileViewConstraints ProfileConstraints;
        public string StatisticName;
        public int? Version;
    }
    [Serializable]
    public class GetLeaderboardAroundPlayerResult : PlayFabResultCommon
    {
        public List<PlayerLeaderboardEntry> Leaderboard;
        public DateTime? NextReset;
        public int Version;
    }

    [Serializable]
    public class GetLeaderboardForUsersCharactersRequest : PlayFabRequestCommon
    {
        public string StatisticName;
    }
    [Serializable]
    public class GetLeaderboardForUsersCharactersResult : PlayFabResultCommon
    {
        public List<CharacterLeaderboardEntry> Leaderboard;
    }

    [Serializable]
    public class GetLeaderboardRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? MaxResultsCount;
        public PlayerProfileViewConstraints ProfileConstraints;
        public int StartPosition;
        public string StatisticName;
        public int? Version;
    }
    [Serializable]
    public class GetLeaderboardResult : PlayFabResultCommon
    {
        public List<PlayerLeaderboardEntry> Leaderboard;
        public DateTime? NextReset;
        public int Version;
    }

    [Serializable]
    public class GetPaymentTokenRequest : PlayFabRequestCommon
    {
        public string TokenProvider;
    }

    [Serializable]
    public class GetPaymentTokenResult : PlayFabResultCommon
    {
        public string OrderId;
        public string ProviderToken;
    }

    [Serializable]
    public class GetPhotonAuthenticationTokenRequest : PlayFabRequestCommon
    {
        public string PhotonApplicationId;
    }

    [Serializable]
    public class GetPhotonAuthenticationTokenResult : PlayFabResultCommon
    {
        public string PhotonCustomAuthenticationToken;
    }

    [Serializable]
    public class GetPlayerCombinedInfoRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayFabId;
    }

    [Serializable]
    public class GetPlayerCombinedInfoRequestParams : PlayFabBaseModel
    {
        public bool GetCharacterInventories;
        public bool GetCharacterList;
        public bool GetPlayerProfile;
        public bool GetPlayerStatistics;
        public bool GetTitleData;
        public bool GetUserAccountInfo;
        public bool GetUserData;
        public bool GetUserInventory;
        public bool GetUserReadOnlyData;
        public bool GetUserVirtualCurrency;
        public List<string> PlayerStatisticNames;
        public PlayerProfileViewConstraints ProfileConstraints;
        public List<string> TitleDataKeys;
        public List<string> UserDataKeys;
        public List<string> UserReadOnlyDataKeys;
    }
    [Serializable]
    public class GetPlayerCombinedInfoResult : PlayFabResultCommon
    {
        public GetPlayerCombinedInfoResultPayload InfoResultPayload;
        public string PlayFabId;
    }

    [Serializable]
    public class GetPlayerCombinedInfoResultPayload : PlayFabBaseModel
    {
        public UserAccountInfo AccountInfo;
        public List<CharacterInventory> CharacterInventories;
        public List<CharacterResult> CharacterList;
        public PlayerProfileModel PlayerProfile;
        public List<StatisticValue> PlayerStatistics;
        public Dictionary<string,string> TitleData;
        public Dictionary<string,UserDataRecord> UserData;
        public uint UserDataVersion;
        public List<ItemInstance> UserInventory;
        public Dictionary<string,UserDataRecord> UserReadOnlyData;
        public uint UserReadOnlyDataVersion;
        public Dictionary<string,int> UserVirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> UserVirtualCurrencyRechargeTimes;
    }

    [Serializable]
    public class GetPlayerCustomPropertyRequest : PlayFabRequestCommon
    {
        public string PropertyName;
    }

    [Serializable]
    public class GetPlayerCustomPropertyResult : PlayFabResultCommon
    {
        public int PropertiesVersion;
        public CustomPropertyDetails Property;
    }
    [Serializable]
    public class GetPlayerProfileRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public PlayerProfileViewConstraints ProfileConstraints;
    }

    [Serializable]
    public class GetPlayerProfileResult : PlayFabResultCommon
    {
        public PlayerProfileModel PlayerProfile;
    }

    [Serializable]
    public class GetPlayerSegmentsRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class GetPlayerSegmentsResult : PlayFabResultCommon
    {
        public List<GetSegmentResult> Segments;
    }

    [Serializable]
    public class GetPlayerStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<string> StatisticNames;
        public List<StatisticNameVersion> StatisticNameVersions;
    }
    [Serializable]
    public class GetPlayerStatisticsResult : PlayFabResultCommon
    {
        public List<StatisticValue> Statistics;
    }

    [Serializable]
    public class GetPlayerStatisticVersionsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string StatisticName;
    }

    [Serializable]
    public class GetPlayerStatisticVersionsResult : PlayFabResultCommon
    {
        public List<PlayerStatisticVersion> StatisticVersions;
    }
    [Serializable]
    public class GetPlayerTagsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Namespace;
        public string PlayFabId;
    }

    [Serializable]
    public class GetPlayerTagsResult : PlayFabResultCommon
    {
        public string PlayFabId;
        public List<string> Tags;
    }

    [Serializable]
    public class GetPlayerTradesRequest : PlayFabRequestCommon
    {
        public TradeStatus? StatusFilter;
    }

    [Serializable]
    public class GetPlayerTradesResponse : PlayFabResultCommon
    {
        public List<TradeInfo> AcceptedTrades;
        public List<TradeInfo> OpenedTrades;
    }

    [Serializable]
    public class GetPlayFabIDsFromBattleNetAccountIdsRequest : PlayFabRequestCommon
    {
        public List<string> BattleNetAccountIds;
    }
    [Serializable]
    public class GetPlayFabIDsFromBattleNetAccountIdsResult : PlayFabResultCommon
    {
        public List<BattleNetAccountPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromFacebookIDsRequest : PlayFabRequestCommon
    {
        public List<string> FacebookIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromFacebookIDsResult : PlayFabResultCommon
    {
        public List<FacebookPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromFacebookInstantGamesIdsRequest : PlayFabRequestCommon
    {
        public List<string> FacebookInstantGamesIds;
    }
    [Serializable]
    public class GetPlayFabIDsFromFacebookInstantGamesIdsResult : PlayFabResultCommon
    {
        public List<FacebookInstantGamesPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromGameCenterIDsRequest : PlayFabRequestCommon
    {
        public List<string> GameCenterIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromGameCenterIDsResult : PlayFabResultCommon
    {
        public List<GameCenterPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromGenericIDsRequest : PlayFabRequestCommon
    {
        public List<GenericServiceId> GenericIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromGenericIDsResult : PlayFabResultCommon
    {
        public List<GenericPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromGoogleIDsRequest : PlayFabRequestCommon
    {
        public List<string> GoogleIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromGoogleIDsResult : PlayFabResultCommon
    {
        public List<GooglePlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromGooglePlayGamesPlayerIDsRequest : PlayFabRequestCommon
    {
        public List<string> GooglePlayGamesPlayerIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromGooglePlayGamesPlayerIDsResult : PlayFabResultCommon
    {
        public List<GooglePlayGamesPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromKongregateIDsRequest : PlayFabRequestCommon
    {
        public List<string> KongregateIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromKongregateIDsResult : PlayFabResultCommon
    {
        public List<KongregatePlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromNintendoServiceAccountIdsRequest : PlayFabRequestCommon
    {
        public List<string> NintendoAccountIds;
    }
    [Serializable]
    public class GetPlayFabIDsFromNintendoServiceAccountIdsResult : PlayFabResultCommon
    {
        public List<NintendoServiceAccountPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromNintendoSwitchDeviceIdsRequest : PlayFabRequestCommon
    {
        public List<string> NintendoSwitchDeviceIds;
    }
    [Serializable]
    public class GetPlayFabIDsFromNintendoSwitchDeviceIdsResult : PlayFabResultCommon
    {
        public List<NintendoSwitchPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromPSNAccountIDsRequest : PlayFabRequestCommon
    {
        public int? IssuerId;
        public List<string> PSNAccountIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromPSNAccountIDsResult : PlayFabResultCommon
    {
        public List<PSNAccountPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromPSNOnlineIDsRequest : PlayFabRequestCommon
    {
        public int? IssuerId;
        public List<string> PSNOnlineIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromPSNOnlineIDsResult : PlayFabResultCommon
    {
        public List<PSNOnlinePlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromSteamIDsRequest : PlayFabRequestCommon
    {
        public List<string> SteamStringIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromSteamIDsResult : PlayFabResultCommon
    {
        public List<SteamPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromSteamNamesRequest : PlayFabRequestCommon
    {
        public List<string> SteamNames;
    }
    [Serializable]
    public class GetPlayFabIDsFromSteamNamesResult : PlayFabResultCommon
    {
        public List<SteamNamePlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromTwitchIDsRequest : PlayFabRequestCommon
    {
        public List<string> TwitchIds;
    }
    [Serializable]
    public class GetPlayFabIDsFromTwitchIDsResult : PlayFabResultCommon
    {
        public List<TwitchPlayFabIdPair> Data;
    }

    [Serializable]
    public class GetPlayFabIDsFromXboxLiveIDsRequest : PlayFabRequestCommon
    {
        public string Sandbox;
        public List<string> XboxLiveAccountIDs;
    }
    [Serializable]
    public class GetPlayFabIDsFromXboxLiveIDsResult : PlayFabResultCommon
    {
        public List<XboxLiveAccountPlayFabIdPair> Data;
    }
    [Serializable]
    public class GetPublisherDataRequest : PlayFabRequestCommon
    {
        public List<string> Keys;
    }

    [Serializable]
    public class GetPublisherDataResult : PlayFabResultCommon
    {
        public Dictionary<string,string> Data;
    }

    [Serializable]
    public class GetPurchaseRequest : PlayFabRequestCommon
    {
        public string OrderId;
    }

    [Serializable]
    public class GetPurchaseResult : PlayFabResultCommon
    {
        public string OrderId;
        public string PaymentProvider;
        public DateTime PurchaseDate;
        public string TransactionId;
        public string TransactionStatus;
    }

    [Serializable]
    public class GetSegmentResult : PlayFabBaseModel
    {
        public string ABTestParent;
        public string Id;
        public string Name;
    }

    [Serializable]
    public class GetSharedGroupDataRequest : PlayFabRequestCommon
    {
        public bool? GetMembers;
        public List<string> Keys;
        public string SharedGroupId;
    }

    [Serializable]
    public class GetSharedGroupDataResult : PlayFabResultCommon
    {
        public Dictionary<string,SharedGroupDataRecord> Data;
        public List<string> Members;
    }
    [Serializable]
    public class GetStoreItemsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string StoreId;
    }

    [Serializable]
    public class GetStoreItemsResult : PlayFabResultCommon
    {
        public string CatalogVersion;
        public StoreMarketingModel MarketingData;
        public SourceType? Source;
        public List<StoreItem> Store;
        public string StoreId;
    }
    [Serializable]
    public class GetTimeRequest : PlayFabRequestCommon
    {
    }
    [Serializable]
    public class GetTimeResult : PlayFabResultCommon
    {
        public DateTime Time;
    }
    [Serializable]
    public class GetTitleDataRequest : PlayFabRequestCommon
    {
        public List<string> Keys;
        public string OverrideLabel;
    }

    [Serializable]
    public class GetTitleDataResult : PlayFabResultCommon
    {
        public Dictionary<string,string> Data;
    }

    [Serializable]
    public class GetTitleNewsRequest : PlayFabRequestCommon
    {
        public int? Count;
    }

    [Serializable]
    public class GetTitleNewsResult : PlayFabResultCommon
    {
        public List<TitleNewsItem> News;
    }
    [Serializable]
    public class GetTitlePublicKeyRequest : PlayFabRequestCommon
    {
        public string TitleId;
        public string TitleSharedSecret;
    }

    [Serializable]
    public class GetTitlePublicKeyResult : PlayFabResultCommon
    {
        public string RSAPublicKey;
    }

    [Serializable]
    public class GetTradeStatusRequest : PlayFabRequestCommon
    {
        public string OfferingPlayerId;
        public string TradeId;
    }

    [Serializable]
    public class GetTradeStatusResponse : PlayFabResultCommon
    {
        public TradeInfo Trade;
    }
    [Serializable]
    public class GetUserDataRequest : PlayFabRequestCommon
    {
        public uint? IfChangedFromDataVersion;
        public List<string> Keys;
        public string PlayFabId;
    }

    [Serializable]
    public class GetUserDataResult : PlayFabResultCommon
    {
        public Dictionary<string,UserDataRecord> Data;
        public uint DataVersion;
    }

    [Serializable]
    public class GetUserInventoryRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class GetUserInventoryResult : PlayFabResultCommon
    {
        public List<ItemInstance> Inventory;
        public Dictionary<string,int> VirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
    }

    [Serializable]
    public class GooglePlayFabIdPair : PlayFabBaseModel
    {
        public string GoogleId;
        public string PlayFabId;
    }

    [Serializable]
    public class GooglePlayGamesPlayFabIdPair : PlayFabBaseModel
    {
        public string GooglePlayGamesPlayerId;
        public string PlayFabId;
    }
    [Serializable]
    public class GrantCharacterToUserRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterName;
        public Dictionary<string,string> CustomTags;
        public string ItemId;
    }

    [Serializable]
    public class GrantCharacterToUserResult : PlayFabResultCommon
    {
        public string CharacterId;
        public string CharacterType;
        public bool Result;
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
    public class ItemPurchaseRequest : PlayFabBaseModel
    {
        public string Annotation;
        public string ItemId;
        public uint Quantity;
        public List<string> UpgradeFromItems;
    }

    [Serializable]
    public class KongregatePlayFabIdPair : PlayFabBaseModel
    {
        public string KongregateId;
        public string PlayFabId;
    }

    [Serializable]
    public class LinkAndroidDeviceIDRequest : PlayFabRequestCommon
    {
        public string AndroidDevice;
        public string AndroidDeviceId;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string OS;
    }

    [Serializable]
    public class LinkAndroidDeviceIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkAppleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string IdentityToken;
    }

    [Serializable]
    public class LinkBattleNetRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string IdentityToken;
    }

    [Serializable]
    public class LinkCustomIDRequest : PlayFabRequestCommon
    {
        public string CustomId;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
    }

    [Serializable]
    public class LinkCustomIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkedPlatformAccountModel : PlayFabBaseModel
    {
        public string Email;
        public LoginIdentityProvider? Platform;
        public string PlatformUserId;
        public string Username;
    }
    [Serializable]
    public class LinkFacebookAccountRequest : PlayFabRequestCommon
    {
        public string AccessToken;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
    }

    [Serializable]
    public class LinkFacebookAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkFacebookInstantGamesIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FacebookInstantGamesSignature;
        public bool? ForceLink;
    }

    [Serializable]
    public class LinkFacebookInstantGamesIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkGameCenterAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string GameCenterId;
        public string PublicKeyUrl;
        public string Salt;
        public string Signature;
        public string Timestamp;
    }

    [Serializable]
    public class LinkGameCenterAccountResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class LinkGoogleAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string ServerAuthCode;
    }

    [Serializable]
    public class LinkGoogleAccountResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class LinkGooglePlayGamesServicesAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string ServerAuthCode;
    }

    [Serializable]
    public class LinkGooglePlayGamesServicesAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkIOSDeviceIDRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DeviceId;
        public string DeviceModel;
        public bool? ForceLink;
        public string OS;
    }

    [Serializable]
    public class LinkIOSDeviceIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkKongregateAccountRequest : PlayFabRequestCommon
    {
        public string AuthTicket;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string KongregateId;
    }

    [Serializable]
    public class LinkKongregateAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkNintendoServiceAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string IdentityToken;
    }

    [Serializable]
    public class LinkNintendoSwitchDeviceIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string NintendoSwitchDeviceId;
    }

    [Serializable]
    public class LinkNintendoSwitchDeviceIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkOpenIdConnectRequest : PlayFabRequestCommon
    {
        public string ConnectionId;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string IdToken;
    }

    [Serializable]
    public class LinkPSNAccountRequest : PlayFabRequestCommon
    {
        public string AuthCode;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public int? IssuerId;
        public string RedirectUri;
    }

    [Serializable]
    public class LinkPSNAccountResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class LinkSteamAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string SteamTicket;
        public bool? TicketIsServiceSpecific;
    }

    [Serializable]
    public class LinkSteamAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkTwitchAccountRequest : PlayFabRequestCommon
    {
        public string AccessToken;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
    }

    [Serializable]
    public class LinkTwitchAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkXboxAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string XboxToken;
    }

    [Serializable]
    public class LinkXboxAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class ListPlayerCustomPropertiesRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class ListPlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public List<CustomPropertyDetails> Properties;
        public int PropertiesVersion;
    }
    [Serializable]
    public class ListUsersCharactersRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class ListUsersCharactersResult : PlayFabResultCommon
    {
        public List<CharacterResult> Characters;
    }

    [Serializable]
    public class LocationModel : PlayFabBaseModel
    {
        public string City;
        public ContinentCode? ContinentCode;
        public CountryCode? CountryCode;
        public double? Latitude;
        public double? Longitude;
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
    public class LoginResult : PlayFabLoginResultCommon
    {
        public EntityTokenResponse EntityToken;
        public GetPlayerCombinedInfoResultPayload InfoResultPayload;
        public DateTime? LastLoginTime;
        public bool NewlyCreated;
        public string PlayFabId;
        public string SessionTicket;
        public UserSettings SettingsForUser;
        public TreatmentAssignment TreatmentAssignment;
    }
    [Serializable]
    public class LoginWithAndroidDeviceIDRequest : PlayFabRequestCommon
    {
        public string AndroidDevice;
        public string AndroidDeviceId;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string OS;
        public string PlayerSecret;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithAppleRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public string IdentityToken;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithBattleNetRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public string IdentityToken;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithCustomIDRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public string CustomId;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithEmailAddressRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Email;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string Password;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithFacebookInstantGamesIdRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public string FacebookInstantGamesSignature;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithFacebookRequest : PlayFabRequestCommon
    {
        public string AccessToken;
        public string AuthenticationToken;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithGameCenterRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerId;
        public string PlayerSecret;
        public string PublicKeyUrl;
        public string Salt;
        public string Signature;
        public string Timestamp;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithGoogleAccountRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string ServerAuthCode;
        public bool? SetEmail;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithGooglePlayGamesServicesRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string ServerAuthCode;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithIOSDeviceIDRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string DeviceId;
        public string DeviceModel;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string OS;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithKongregateRequest : PlayFabRequestCommon
    {
        public string AuthTicket;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string KongregateId;
        public string PlayerSecret;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithNintendoServiceAccountRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public string IdentityToken;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithNintendoSwitchDeviceIdRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string NintendoSwitchDeviceId;
        public string PlayerSecret;
        public string TitleId;
    }

    [Serializable]
    public class LoginWithOpenIdConnectRequest : PlayFabRequestCommon
    {
        public string ConnectionId;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public string IdToken;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithPlayFabRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string Password;
        public string TitleId;
        public string Username;
    }
    [Serializable]
    public class LoginWithPSNRequest : PlayFabRequestCommon
    {
        public string AuthCode;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public int? IssuerId;
        public string PlayerSecret;
        public string RedirectUri;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithSteamRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string SteamTicket;
        public bool? TicketIsServiceSpecific;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithTwitchRequest : PlayFabRequestCommon
    {
        public string AccessToken;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
    }
    [Serializable]
    public class LoginWithXboxRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string TitleId;
        public string XboxToken;
    }

    [Serializable]
    public class LogStatement : PlayFabBaseModel
    {
        public object Data;
        public string Level;
        public string Message;
    }

    [Serializable]
    public class MembershipModel : PlayFabBaseModel
    {
        public bool IsActive;
        public DateTime MembershipExpiration;
        public string MembershipId;
        public DateTime? OverrideExpiration;
        public List<SubscriptionModel> Subscriptions;
    }

    [Serializable]
    public class MicrosoftStorePayload : PlayFabBaseModel
    {
        public string CollectionsMsIdKey;
        public string UserId;
        public string XboxToken;
    }

    [Serializable]
    public class ModifyUserVirtualCurrencyResult : PlayFabResultCommon
    {
        public int Balance;
        public int BalanceChange;
        public string PlayFabId;
        public string VirtualCurrency;
    }
    [Serializable]
    public class NameIdentifier : PlayFabBaseModel
    {
        public string Id;
        public string Name;
    }

    [Serializable]
    public class NintendoServiceAccountPlayFabIdPair : PlayFabBaseModel
    {
        public string NintendoServiceAccountId;
        public string PlayFabId;
    }

    [Serializable]
    public class NintendoSwitchPlayFabIdPair : PlayFabBaseModel
    {
        public string NintendoSwitchDeviceId;
        public string PlayFabId;
    }

    [Serializable]
    public class OpenTradeRequest : PlayFabRequestCommon
    {
        public List<string> AllowedPlayerIds;
        public List<string> OfferedInventoryInstanceIds;
        public List<string> RequestedCatalogItemIds;
    }

    [Serializable]
    public class OpenTradeResponse : PlayFabResultCommon
    {
        public TradeInfo Trade;
    }
    [Serializable]
    public class PayForPurchaseRequest : PlayFabRequestCommon
    {
        public string Currency;
        public Dictionary<string,string> CustomTags;
        public string OrderId;
        public string ProviderName;
        public string ProviderTransactionId;
    }
    [Serializable]
    public class PayForPurchaseResult : PlayFabResultCommon
    {
        public uint CreditApplied;
        public string OrderId;
        public string ProviderData;
        public string ProviderToken;
        public string PurchaseConfirmationPageURL;
        public string PurchaseCurrency;
        public uint PurchasePrice;
        public TransactionStatus? Status;
        public Dictionary<string,int> VCAmount;
        public Dictionary<string,int> VirtualCurrency;
    }

    [Serializable]
    public class PaymentOption : PlayFabBaseModel
    {
        public string Currency;
        public uint Price;
        public string ProviderName;
        public uint StoreCredit;
    }

    [Serializable]
    public class PlayerLeaderboardEntry : PlayFabBaseModel
    {
        public string DisplayName;
        public string PlayFabId;
        public int Position;
        public PlayerProfileModel Profile;
        public int StatValue;
    }

    [Serializable]
    public class PlayerProfileModel : PlayFabBaseModel
    {
        public List<AdCampaignAttributionModel> AdCampaignAttributions;
        public string AvatarUrl;
        public DateTime? BannedUntil;
        public List<ContactEmailInfoModel> ContactEmailAddresses;
        public DateTime? Created;
        public string DisplayName;
        public List<string> ExperimentVariants;
        public DateTime? LastLogin;
        public List<LinkedPlatformAccountModel> LinkedAccounts;
        public List<LocationModel> Locations;
        public List<MembershipModel> Memberships;
        public LoginIdentityProvider? Origination;
        public string PlayerId;
        public string PublisherId;
        public List<PushNotificationRegistrationModel> PushNotificationRegistrations;
        public List<StatisticModel> Statistics;
        public List<TagModel> Tags;
        public string TitleId;
        public uint? TotalValueToDateInUSD;
        public List<ValueToDateModel> ValuesToDate;
    }

    [Serializable]
    public class PlayerProfileViewConstraints : PlayFabBaseModel
    {
        public bool ShowAvatarUrl;
        public bool ShowBannedUntil;
        public bool ShowCampaignAttributions;
        public bool ShowContactEmailAddresses;
        public bool ShowCreated;
        public bool ShowDisplayName;
        public bool ShowExperimentVariants;
        public bool ShowLastLogin;
        public bool ShowLinkedAccounts;
        public bool ShowLocations;
        public bool ShowMemberships;
        public bool ShowOrigination;
        public bool ShowPushNotificationRegistrations;
        public bool ShowStatistics;
        public bool ShowTags;
        public bool ShowTotalValueToDateInUsd;
        public bool ShowValuesToDate;
    }

    [Serializable]
    public class PlayerStatisticVersion : PlayFabBaseModel
    {
        public DateTime ActivationTime;
        public DateTime? DeactivationTime;
        public DateTime? ScheduledActivationTime;
        public DateTime? ScheduledDeactivationTime;
        public string StatisticName;
        public uint Version;
    }

    [Serializable]
    public class PlayStation5Payload : PlayFabBaseModel
    {
        public List<string> Ids;
        public string ServiceLabel;
    }

    [Serializable]
    public class PSNAccountPlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string PSNAccountId;
    }

    [Serializable]
    public class PSNOnlinePlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string PSNOnlineId;
    }
    [Serializable]
    public class PurchaseItemRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string ItemId;
        public int Price;
        public string StoreId;
        public string VirtualCurrency;
    }

    [Serializable]
    public class PurchaseItemResult : PlayFabResultCommon
    {
        public List<ItemInstance> Items;
    }

    [Serializable]
    public class PurchaseReceiptFulfillment : PlayFabBaseModel
    {
        public List<ItemInstance> FulfilledItems;
        public string RecordedPriceSource;
        public string RecordedTransactionCurrency;
        public uint? RecordedTransactionTotal;
    }

    public enum PushNotificationPlatform
    {
        ApplePushNotificationService,
        GoogleCloudMessaging
    }

    [Serializable]
    public class PushNotificationRegistrationModel : PlayFabBaseModel
    {
        public string NotificationEndpointARN;
        public PushNotificationPlatform? Platform;
    }
    [Serializable]
    public class RedeemCouponRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public string CouponCode;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class RedeemCouponResult : PlayFabResultCommon
    {
        public List<ItemInstance> GrantedItems;
    }

    [Serializable]
    public class RefreshPSNAuthTokenRequest : PlayFabRequestCommon
    {
        public string AuthCode;
        public int? IssuerId;
        public string RedirectUri;
    }
    [Serializable]
    public class RegisterForIOSPushNotificationRequest : PlayFabRequestCommon
    {
        public string ConfirmationMessage;
        public string DeviceToken;
        public bool? SendPushNotificationConfirmation;
    }

    [Serializable]
    public class RegisterForIOSPushNotificationResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class RegisterPlayFabUserRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DisplayName;
        public string Email;
        public string EncryptedRequest;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string Password;
        public string PlayerSecret;
        public bool? RequireBothUsernameAndEmail;
        public string TitleId;
        public string Username;
    }
    [Serializable]
    public class RegisterPlayFabUserResult : PlayFabLoginResultCommon
    {
        public EntityTokenResponse EntityToken;
        public string PlayFabId;
        public string SessionTicket;
        public UserSettings SettingsForUser;
        public string Username;
    }
    [Serializable]
    public class RemoveContactEmailRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class RemoveContactEmailResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class RemoveFriendRequest : PlayFabRequestCommon
    {
        public string FriendPlayFabId;
    }

    [Serializable]
    public class RemoveFriendResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class RemoveGenericIDRequest : PlayFabRequestCommon
    {
        public GenericServiceId GenericId;
    }

    [Serializable]
    public class RemoveGenericIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class RemoveSharedGroupMembersRequest : PlayFabRequestCommon
    {
        public List<string> PlayFabIds;
        public string SharedGroupId;
    }

    [Serializable]
    public class RemoveSharedGroupMembersResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class ReportAdActivityRequest : PlayFabRequestCommon
    {
        public AdActivity Activity;
        public Dictionary<string,string> CustomTags;
        public string PlacementId;
        public string RewardId;
    }
    [Serializable]
    public class ReportAdActivityResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class ReportPlayerClientRequest : PlayFabRequestCommon
    {
        public string Comment;
        public Dictionary<string,string> CustomTags;
        public string ReporteeId;
    }
    [Serializable]
    public class ReportPlayerClientResult : PlayFabResultCommon
    {
        public int SubmissionsRemaining;
    }
    [Serializable]
    public class RestoreIOSPurchasesRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public string ReceiptData;
    }
    [Serializable]
    public class RestoreIOSPurchasesResult : PlayFabResultCommon
    {
        public List<PurchaseReceiptFulfillment> Fulfillments;
    }
    [Serializable]
    public class RewardAdActivityRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlacementId;
        public string RewardId;
    }
    [Serializable]
    public class RewardAdActivityResult : PlayFabResultCommon
    {
        public string AdActivityEventId;
        public List<string> DebugResults;
        public string PlacementId;
        public string PlacementName;
        public int? PlacementViewsRemaining;
        public double? PlacementViewsResetMinutes;
        public AdRewardResults RewardResults;
    }

    [Serializable]
    public class ScriptExecutionError : PlayFabBaseModel
    {
        public string Error;
        public string Message;
        public string StackTrace;
    }
    [Serializable]
    public class SendAccountRecoveryEmailRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Email;
        public string EmailTemplateId;
        public string TitleId;
    }

    [Serializable]
    public class SendAccountRecoveryEmailResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SetFriendTagsRequest : PlayFabRequestCommon
    {
        public string FriendPlayFabId;
        public List<string> Tags;
    }

    [Serializable]
    public class SetFriendTagsResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SetPlayerSecretRequest : PlayFabRequestCommon
    {
        public string EncryptedRequest;
        public string PlayerSecret;
    }

    [Serializable]
    public class SetPlayerSecretResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class SharedGroupDataRecord : PlayFabBaseModel
    {
        public DateTime LastUpdated;
        public string LastUpdatedBy;
        public UserDataPermission? Permission;
        public string Value;
    }

    public enum SourceType
    {
        Admin,
        BackEnd,
        GameClient,
        GameServer,
        Partner,
        Custom,
        API
    }
    [Serializable]
    public class StartPurchaseRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public List<ItemPurchaseRequest> Items;
        public string StoreId;
    }

    [Serializable]
    public class StartPurchaseResult : PlayFabResultCommon
    {
        public List<CartItem> Contents;
        public string OrderId;
        public List<PaymentOption> PaymentOptions;
        public Dictionary<string,int> VirtualCurrencyBalances;
    }

    [Serializable]
    public class StatisticModel : PlayFabBaseModel
    {
        public string Name;
        public int Value;
        public int Version;
    }

    [Serializable]
    public class StatisticNameVersion : PlayFabBaseModel
    {
        public string StatisticName;
        public uint Version;
    }

    [Serializable]
    public class StatisticUpdate : PlayFabBaseModel
    {
        public string StatisticName;
        public int Value;
        public uint? Version;
    }

    [Serializable]
    public class StatisticValue : PlayFabBaseModel
    {
        public string StatisticName;
        public int Value;
        public uint Version;
    }

    [Serializable]
    public class SteamNamePlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string SteamName;
    }

    [Serializable]
    public class SteamPlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string SteamStringId;
    }
    [Serializable]
    public class StoreItem : PlayFabBaseModel
    {
        public object CustomData;
        public uint? DisplayPosition;
        public string ItemId;
        public Dictionary<string,uint> RealCurrencyPrices;
        public Dictionary<string,uint> VirtualCurrencyPrices;
    }
    [Serializable]
    public class StoreMarketingModel : PlayFabBaseModel
    {
        public string Description;
        public string DisplayName;
        public object Metadata;
    }

    [Serializable]
    public class SubscriptionModel : PlayFabBaseModel
    {
        public DateTime Expiration;
        public DateTime InitialSubscriptionTime;
        public bool IsActive;
        public SubscriptionProviderStatus? Status;
        public string SubscriptionId;
        public string SubscriptionItemId;
        public string SubscriptionProvider;
    }

    public enum SubscriptionProviderStatus
    {
        NoError,
        Cancelled,
        UnknownError,
        BillingError,
        ProductUnavailable,
        CustomerDidNotAcceptPriceChange,
        FreeTrial,
        PaymentPending
    }
    [Serializable]
    public class SubtractUserVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public Dictionary<string,string> CustomTags;
        public string VirtualCurrency;
    }

    [Serializable]
    public class TagModel : PlayFabBaseModel
    {
        public string TagValue;
    }

    public enum TitleActivationStatus
    {
        None,
        ActivatedTitleKey,
        PendingSteam,
        ActivatedSteam,
        RevokedSteam
    }

    [Serializable]
    public class TitleNewsItem : PlayFabBaseModel
    {
        public string Body;
        public string NewsId;
        public DateTime Timestamp;
        public string Title;
    }

    [Serializable]
    public class TradeInfo : PlayFabBaseModel
    {
        public List<string> AcceptedInventoryInstanceIds;
        public string AcceptedPlayerId;
        public List<string> AllowedPlayerIds;
        public DateTime? CancelledAt;
        public DateTime? FilledAt;
        public DateTime? InvalidatedAt;
        public List<string> OfferedCatalogItemIds;
        public List<string> OfferedInventoryInstanceIds;
        public string OfferingPlayerId;
        public DateTime? OpenedAt;
        public List<string> RequestedCatalogItemIds;
        public TradeStatus? Status;
        public string TradeId;
    }

    public enum TradeStatus
    {
        Invalid,
        Opening,
        Open,
        Accepting,
        Accepted,
        Filled,
        Cancelled
    }

    public enum TransactionStatus
    {
        CreateCart,
        Init,
        Approved,
        Succeeded,
        FailedByProvider,
        DisputePending,
        RefundPending,
        Refunded,
        RefundFailed,
        ChargedBack,
        FailedByUber,
        FailedByPlayFab,
        Revoked,
        TradePending,
        Traded,
        Upgraded,
        StackPending,
        Stacked,
        Other,
        Failed
    }

    [Serializable]
    public class TreatmentAssignment : PlayFabBaseModel
    {
        public List<Variable> Variables;
        public List<string> Variants;
    }

    [Serializable]
    public class TwitchPlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string TwitchId;
    }

    [Serializable]
    public class UnlinkAndroidDeviceIDRequest : PlayFabRequestCommon
    {
        public string AndroidDeviceId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkAndroidDeviceIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkAppleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkBattleNetRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkCustomIDRequest : PlayFabRequestCommon
    {
        public string CustomId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkCustomIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkFacebookAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkFacebookAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkFacebookInstantGamesIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FacebookInstantGamesId;
    }

    [Serializable]
    public class UnlinkFacebookInstantGamesIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkGameCenterAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkGameCenterAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkGoogleAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkGoogleAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkGooglePlayGamesServicesAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkGooglePlayGamesServicesAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkIOSDeviceIDRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DeviceId;
    }

    [Serializable]
    public class UnlinkIOSDeviceIDResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkKongregateAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkKongregateAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkNintendoServiceAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkNintendoSwitchDeviceIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string NintendoSwitchDeviceId;
    }

    [Serializable]
    public class UnlinkNintendoSwitchDeviceIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkOpenIdConnectRequest : PlayFabRequestCommon
    {
        public string ConnectionId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkPSNAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkPSNAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkSteamAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkSteamAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkTwitchAccountRequest : PlayFabRequestCommon
    {
        public string AccessToken;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkTwitchAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkXboxAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UnlinkXboxAccountResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UnlockContainerInstanceRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public string ContainerItemInstanceId;
        public Dictionary<string,string> CustomTags;
        public string KeyItemInstanceId;
    }
    [Serializable]
    public class UnlockContainerItemRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public string ContainerItemId;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class UnlockContainerItemResult : PlayFabResultCommon
    {
        public List<ItemInstance> GrantedItems;
        public string UnlockedItemInstanceId;
        public string UnlockedWithItemInstanceId;
        public Dictionary<string,uint> VirtualCurrency;
    }

    [Serializable]
    public class UpdateAvatarUrlRequest : PlayFabRequestCommon
    {
        public string ImageUrl;
    }
    [Serializable]
    public class UpdateCharacterDataRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public List<string> KeysToRemove;
        public UserDataPermission? Permission;
    }

    [Serializable]
    public class UpdateCharacterDataResult : PlayFabResultCommon
    {
        public uint DataVersion;
    }
    [Serializable]
    public class UpdateCharacterStatisticsRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,int> CharacterStatistics;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UpdateCharacterStatisticsResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UpdatePlayerCustomPropertiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? ExpectedPropertiesVersion;
        public List<UpdateProperty> Properties;
    }

    [Serializable]
    public class UpdatePlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public int PropertiesVersion;
    }
    [Serializable]
    public class UpdatePlayerStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<StatisticUpdate> Statistics;
    }

    [Serializable]
    public class UpdatePlayerStatisticsResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UpdateProperty : PlayFabBaseModel
    {
        public string Name;
        public object Value;
    }
    [Serializable]
    public class UpdateSharedGroupDataRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public List<string> KeysToRemove;
        public UserDataPermission? Permission;
        public string SharedGroupId;
    }

    [Serializable]
    public class UpdateSharedGroupDataResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UpdateUserDataRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public List<string> KeysToRemove;
        public UserDataPermission? Permission;
    }

    [Serializable]
    public class UpdateUserDataResult : PlayFabResultCommon
    {
        public uint DataVersion;
    }
    [Serializable]
    public class UpdateUserTitleDisplayNameRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DisplayName;
    }

    [Serializable]
    public class UpdateUserTitleDisplayNameResult : PlayFabResultCommon
    {
        public string DisplayName;
    }

    [Serializable]
    public class UserAccountInfo : PlayFabBaseModel
    {
        public UserAndroidDeviceInfo AndroidDeviceInfo;
        public UserAppleIdInfo AppleAccountInfo;
        public DateTime Created;
        public UserCustomIdInfo CustomIdInfo;
        public UserFacebookInfo FacebookInfo;
        public UserFacebookInstantGamesIdInfo FacebookInstantGamesIdInfo;
        public UserGameCenterInfo GameCenterInfo;
        public UserGoogleInfo GoogleInfo;
        public UserGooglePlayGamesInfo GooglePlayGamesInfo;
        public UserIosDeviceInfo IosDeviceInfo;
        public UserKongregateInfo KongregateInfo;
        public UserNintendoSwitchAccountIdInfo NintendoSwitchAccountInfo;
        public UserNintendoSwitchDeviceIdInfo NintendoSwitchDeviceIdInfo;
        public List<UserOpenIdInfo> OpenIdInfo;
        public string PlayFabId;
        public UserPrivateAccountInfo PrivateInfo;
        public UserPsnInfo PsnInfo;
        public UserServerCustomIdInfo ServerCustomIdInfo;
        public UserSteamInfo SteamInfo;
        public UserTitleInfo TitleInfo;
        public UserTwitchInfo TwitchInfo;
        public string Username;
        public UserXboxInfo XboxInfo;
    }

    [Serializable]
    public class UserAndroidDeviceInfo : PlayFabBaseModel
    {
        public string AndroidDeviceId;
    }

    [Serializable]
    public class UserAppleIdInfo : PlayFabBaseModel
    {
        public string AppleSubjectId;
    }

    [Serializable]
    public class UserCustomIdInfo : PlayFabBaseModel
    {
        public string CustomId;
    }
    public enum UserDataPermission
    {
        Private,
        Public
    }

    [Serializable]
    public class UserDataRecord : PlayFabBaseModel
    {
        public DateTime LastUpdated;
        public UserDataPermission? Permission;
        public string Value;
    }

    [Serializable]
    public class UserFacebookInfo : PlayFabBaseModel
    {
        public string FacebookId;
        public string FullName;
    }

    [Serializable]
    public class UserFacebookInstantGamesIdInfo : PlayFabBaseModel
    {
        public string FacebookInstantGamesId;
    }

    [Serializable]
    public class UserGameCenterInfo : PlayFabBaseModel
    {
        public string GameCenterId;
    }

    [Serializable]
    public class UserGoogleInfo : PlayFabBaseModel
    {
        public string GoogleEmail;
        public string GoogleGender;
        public string GoogleId;
        public string GoogleLocale;
        public string GoogleName;
    }

    [Serializable]
    public class UserGooglePlayGamesInfo : PlayFabBaseModel
    {
        public string GooglePlayGamesPlayerAvatarImageUrl;
        public string GooglePlayGamesPlayerDisplayName;
        public string GooglePlayGamesPlayerId;
    }

    [Serializable]
    public class UserIosDeviceInfo : PlayFabBaseModel
    {
        public string IosDeviceId;
    }

    [Serializable]
    public class UserKongregateInfo : PlayFabBaseModel
    {
        public string KongregateId;
        public string KongregateName;
    }

    [Serializable]
    public class UserNintendoSwitchAccountIdInfo : PlayFabBaseModel
    {
        public string NintendoSwitchAccountSubjectId;
    }

    [Serializable]
    public class UserNintendoSwitchDeviceIdInfo : PlayFabBaseModel
    {
        public string NintendoSwitchDeviceId;
    }

    [Serializable]
    public class UserOpenIdInfo : PlayFabBaseModel
    {
        public string ConnectionId;
        public string Issuer;
        public string Subject;
    }

    public enum UserOrigination
    {
        Organic,
        Steam,
        Google,
        Amazon,
        Facebook,
        Kongregate,
        GamersFirst,
        Unknown,
        IOS,
        LoadTest,
        Android,
        PSN,
        GameCenter,
        CustomId,
        XboxLive,
        Parse,
        Twitch,
        ServerCustomId,
        NintendoSwitchDeviceId,
        FacebookInstantGamesId,
        OpenIdConnect,
        Apple,
        NintendoSwitchAccount,
        GooglePlayGames,
        XboxMobileStore,
        King,
        BattleNet
    }

    [Serializable]
    public class UserPrivateAccountInfo : PlayFabBaseModel
    {
        public string Email;
    }

    [Serializable]
    public class UserPsnInfo : PlayFabBaseModel
    {
        public string PsnAccountId;
        public string PsnOnlineId;
    }

    [Serializable]
    public class UserServerCustomIdInfo : PlayFabBaseModel
    {
        public string CustomId;
    }

    [Serializable]
    public class UserSettings : PlayFabBaseModel
    {
        public bool GatherDeviceInfo;
        public bool GatherFocusInfo;
        public bool NeedsAttribution;
    }

    [Serializable]
    public class UserSteamInfo : PlayFabBaseModel
    {
        public TitleActivationStatus? SteamActivationStatus;
        public string SteamCountry;
        public Currency? SteamCurrency;
        public string SteamId;
        public string SteamName;
    }

    [Serializable]
    public class UserTitleInfo : PlayFabBaseModel
    {
        public string AvatarUrl;
        public DateTime Created;
        public string DisplayName;
        public DateTime? FirstLogin;
        public bool? isBanned;
        public DateTime? LastLogin;
        public UserOrigination? Origination;
        public EntityKey TitlePlayerAccount;
    }

    [Serializable]
    public class UserTwitchInfo : PlayFabBaseModel
    {
        public string TwitchId;
        public string TwitchUserName;
    }

    [Serializable]
    public class UserXboxInfo : PlayFabBaseModel
    {
        public string XboxUserId;
        public string XboxUserSandbox;
    }

    [Serializable]
    public class ValidateAmazonReceiptRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CurrencyCode;
        public Dictionary<string,string> CustomTags;
        public int PurchasePrice;
        public string ReceiptId;
        public string UserId;
    }
    [Serializable]
    public class ValidateAmazonReceiptResult : PlayFabResultCommon
    {
        public List<PurchaseReceiptFulfillment> Fulfillments;
    }
    [Serializable]
    public class ValidateGooglePlayPurchaseRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CurrencyCode;
        public Dictionary<string,string> CustomTags;
        public uint? PurchasePrice;
        public string ReceiptJson;
        public string Signature;
    }
    [Serializable]
    public class ValidateGooglePlayPurchaseResult : PlayFabResultCommon
    {
        public List<PurchaseReceiptFulfillment> Fulfillments;
    }
    [Serializable]
    public class ValidateIOSReceiptRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CurrencyCode;
        public Dictionary<string,string> CustomTags;
        public int PurchasePrice;
        public string ReceiptData;
    }
    [Serializable]
    public class ValidateIOSReceiptResult : PlayFabResultCommon
    {
        public List<PurchaseReceiptFulfillment> Fulfillments;
    }

    [Serializable]
    public class ValidateWindowsReceiptRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CurrencyCode;
        public Dictionary<string,string> CustomTags;
        public uint PurchasePrice;
        public string Receipt;
    }
    [Serializable]
    public class ValidateWindowsReceiptResult : PlayFabResultCommon
    {
        public List<PurchaseReceiptFulfillment> Fulfillments;
    }

    [Serializable]
    public class ValueToDateModel : PlayFabBaseModel
    {
        public string Currency;
        public uint TotalValue;
        public string TotalValueAsDecimal;
    }

    [Serializable]
    public class Variable : PlayFabBaseModel
    {
        public string Name;
        public string Value;
    }

    [Serializable]
    public class VirtualCurrencyRechargeTime : PlayFabBaseModel
    {
        public int RechargeMax;
        public DateTime RechargeTime;
        public int SecondsToRecharge;
    }
    [Serializable]
    public class WriteClientCharacterEventRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Body;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string EventName;
        public DateTime? Timestamp;
    }
    [Serializable]
    public class WriteClientPlayerEventRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Body;
        public Dictionary<string,string> CustomTags;
        public string EventName;
        public DateTime? Timestamp;
    }

    [Serializable]
    public class WriteEventResponse : PlayFabResultCommon
    {
        public string EventId;
    }
    [Serializable]
    public class WriteTitleEventRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Body;
        public Dictionary<string,string> CustomTags;
        public string EventName;
        public DateTime? Timestamp;
    }

    [Serializable]
    public class XboxLiveAccountPlayFabIdPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string XboxLiveAccountId;
    }
}
#endif
