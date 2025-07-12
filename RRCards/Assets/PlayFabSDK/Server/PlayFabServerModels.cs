#if ENABLE_PLAYFABSERVER_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.ServerModels
{
    [Serializable]
    public class AdCampaignAttribution : PlayFabBaseModel
    {
        public DateTime AttributedAt;
        public string CampaignId;
        public string Platform;
    }

    [Serializable]
    public class AdCampaignAttributionModel : PlayFabBaseModel
    {
        public DateTime AttributedAt;
        public string CampaignId;
        public string Platform;
    }

    [Serializable]
    public class AddCharacterVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string VirtualCurrency;
    }

    [Serializable]
    public class AddFriendRequest : PlayFabRequestCommon
    {
        public string FriendEmail;
        public string FriendPlayFabId;
        public string FriendTitleDisplayName;
        public string FriendUsername;
        public string PlayFabId;
    }

    [Serializable]
    public class AddGenericIDRequest : PlayFabRequestCommon
    {
        public GenericServiceId GenericId;
        public string PlayFabId;
    }
    [Serializable]
    public class AddPlayerTagRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string TagName;
    }

    [Serializable]
    public class AddPlayerTagResult : PlayFabResultCommon
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
    public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string VirtualCurrency;
    }

    [Serializable]
    public class AdvancedPushPlatformMsg : PlayFabBaseModel
    {
        public bool? GCMDataOnly;
        public string Json;
        public PushNotificationPlatform Platform;
    }
    [Serializable]
    public class AuthenticateSessionTicketRequest : PlayFabRequestCommon
    {
        public string SessionTicket;
    }

    [Serializable]
    public class AuthenticateSessionTicketResult : PlayFabResultCommon
    {
        public bool? IsSessionTicketExpired;
        public UserAccountInfo UserInfo;
    }

    [Serializable]
    public class AwardSteamAchievementItem : PlayFabBaseModel
    {
        public string AchievementName;
        public string PlayFabId;
        public bool Result;
    }

    [Serializable]
    public class AwardSteamAchievementRequest : PlayFabRequestCommon
    {
        public List<AwardSteamAchievementItem> Achievements;
    }

    [Serializable]
    public class AwardSteamAchievementResult : PlayFabResultCommon
    {
        public List<AwardSteamAchievementItem> AchievementResults;
    }
    [Serializable]
    public class BanInfo : PlayFabBaseModel
    {
        public bool Active;
        public string BanId;
        public DateTime? Created;
        public DateTime? Expires;
        public string IPAddress;
        public string PlayFabId;
        public string Reason;
        public string UserFamilyType;
    }
    [Serializable]
    public class BanRequest : PlayFabBaseModel
    {
        public uint? DurationInHours;
        public string IPAddress;
        public string PlayFabId;
        public string Reason;
        public UserFamilyType? UserFamilyType;
    }
    [Serializable]
    public class BanUsersRequest : PlayFabRequestCommon
    {
        public List<BanRequest> Bans;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class BanUsersResult : PlayFabResultCommon
    {
        public List<BanInfo> BanData;
    }

    [Serializable]
    public class BattleNetAccountPlayFabIdPair : PlayFabBaseModel
    {
        public string BattleNetAccountId;
        public string PlayFabId;
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

    public enum ChurnRiskLevel
    {
        NoData,
        LowRisk,
        MediumRisk,
        HighRisk
    }

    public enum CloudScriptRevisionOption
    {
        Live,
        Latest,
        Specific
    }

    [Serializable]
    public class ConsumeItemRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public int ConsumeCount;
        public Dictionary<string,string> CustomTags;
        public string ItemInstanceId;
        public string PlayFabId;
    }

    [Serializable]
    public class ConsumeItemResult : PlayFabResultCommon
    {
        public string ItemInstanceId;
        public int RemainingUses;
    }

    [Serializable]
    public class ContactEmailInfo : PlayFabBaseModel
    {
        public string EmailAddress;
        public string Name;
        public EmailVerificationStatus? VerificationStatus;
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
    public class DeleteCharacterFromUserRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public bool SaveCharacterInventory;
    }

    [Serializable]
    public class DeleteCharacterFromUserResult : PlayFabResultCommon
    {
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
        public string PlayFabId;
        public List<string> PropertyNames;
    }

    [Serializable]
    public class DeletePlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public List<DeletedPropertyDetails> DeletedProperties;
        public string PlayFabId;
        public int PropertiesVersion;
    }
    [Serializable]
    public class DeletePlayerRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class DeletePlayerResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class DeletePushNotificationTemplateRequest : PlayFabRequestCommon
    {
        public string PushNotificationTemplateId;
    }

    [Serializable]
    public class DeletePushNotificationTemplateResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteSharedGroupRequest : PlayFabRequestCommon
    {
        public string SharedGroupId;
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
    public class EvaluateRandomResultTableRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string TableId;
    }
    [Serializable]
    public class EvaluateRandomResultTableResult : PlayFabResultCommon
    {
        public string ResultItemId;
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

    [Serializable]
    public class ExecuteCloudScriptServerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
        public object FunctionParameter;
        public bool? GeneratePlayStreamEvent;
        public string PlayFabId;
        public CloudScriptRevisionOption? RevisionSelection;
        public int? SpecificRevision;
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

    public enum GenericErrorCodes
    {
        Success,
        UnkownError,
        InvalidParams,
        AccountNotFound,
        AccountBanned,
        InvalidUsernameOrPassword,
        InvalidTitleId,
        InvalidEmailAddress,
        EmailAddressNotAvailable,
        InvalidUsername,
        InvalidPassword,
        UsernameNotAvailable,
        InvalidSteamTicket,
        AccountAlreadyLinked,
        LinkedAccountAlreadyClaimed,
        InvalidFacebookToken,
        AccountNotLinked,
        FailedByPaymentProvider,
        CouponCodeNotFound,
        InvalidContainerItem,
        ContainerNotOwned,
        KeyNotOwned,
        InvalidItemIdInTable,
        InvalidReceipt,
        ReceiptAlreadyUsed,
        ReceiptCancelled,
        GameNotFound,
        GameModeNotFound,
        InvalidGoogleToken,
        UserIsNotPartOfDeveloper,
        InvalidTitleForDeveloper,
        TitleNameConflicts,
        UserisNotValid,
        ValueAlreadyExists,
        BuildNotFound,
        PlayerNotInGame,
        InvalidTicket,
        InvalidDeveloper,
        InvalidOrderInfo,
        RegistrationIncomplete,
        InvalidPlatform,
        UnknownError,
        SteamApplicationNotOwned,
        WrongSteamAccount,
        TitleNotActivated,
        RegistrationSessionNotFound,
        NoSuchMod,
        FileNotFound,
        DuplicateEmail,
        ItemNotFound,
        ItemNotOwned,
        ItemNotRecycleable,
        ItemNotAffordable,
        InvalidVirtualCurrency,
        WrongVirtualCurrency,
        WrongPrice,
        NonPositiveValue,
        InvalidRegion,
        RegionAtCapacity,
        ServerFailedToStart,
        NameNotAvailable,
        InsufficientFunds,
        InvalidDeviceID,
        InvalidPushNotificationToken,
        NoRemainingUses,
        InvalidPaymentProvider,
        PurchaseInitializationFailure,
        DuplicateUsername,
        InvalidBuyerInfo,
        NoGameModeParamsSet,
        BodyTooLarge,
        ReservedWordInBody,
        InvalidTypeInBody,
        InvalidRequest,
        ReservedEventName,
        InvalidUserStatistics,
        NotAuthenticated,
        StreamAlreadyExists,
        ErrorCreatingStream,
        StreamNotFound,
        InvalidAccount,
        PurchaseDoesNotExist,
        InvalidPurchaseTransactionStatus,
        APINotEnabledForGameClientAccess,
        NoPushNotificationARNForTitle,
        BuildAlreadyExists,
        BuildPackageDoesNotExist,
        CustomAnalyticsEventsNotEnabledForTitle,
        InvalidSharedGroupId,
        NotAuthorized,
        MissingTitleGoogleProperties,
        InvalidItemProperties,
        InvalidPSNAuthCode,
        InvalidItemId,
        PushNotEnabledForAccount,
        PushServiceError,
        ReceiptDoesNotContainInAppItems,
        ReceiptContainsMultipleInAppItems,
        InvalidBundleID,
        JavascriptException,
        InvalidSessionTicket,
        UnableToConnectToDatabase,
        InternalServerError,
        InvalidReportDate,
        DatabaseThroughputExceeded,
        InvalidGameTicket,
        ExpiredGameTicket,
        GameTicketDoesNotMatchLobby,
        LinkedDeviceAlreadyClaimed,
        DeviceAlreadyLinked,
        DeviceNotLinked,
        PartialFailure,
        PublisherNotSet,
        ServiceUnavailable,
        VersionNotFound,
        RevisionNotFound,
        InvalidPublisherId,
        DownstreamServiceUnavailable,
        APINotIncludedInTitleUsageTier,
        DAULimitExceeded,
        APIRequestLimitExceeded,
        InvalidAPIEndpoint,
        BuildNotAvailable,
        ConcurrentEditError,
        ContentNotFound,
        CharacterNotFound,
        CloudScriptNotFound,
        ContentQuotaExceeded,
        InvalidCharacterStatistics,
        PhotonNotEnabledForTitle,
        PhotonApplicationNotFound,
        PhotonApplicationNotAssociatedWithTitle,
        InvalidEmailOrPassword,
        FacebookAPIError,
        InvalidContentType,
        KeyLengthExceeded,
        DataLengthExceeded,
        TooManyKeys,
        FreeTierCannotHaveVirtualCurrency,
        MissingAmazonSharedKey,
        AmazonValidationError,
        InvalidPSNIssuerId,
        PSNInaccessible,
        ExpiredAuthToken,
        FailedToGetEntitlements,
        FailedToConsumeEntitlement,
        TradeAcceptingUserNotAllowed,
        TradeInventoryItemIsAssignedToCharacter,
        TradeInventoryItemIsBundle,
        TradeStatusNotValidForCancelling,
        TradeStatusNotValidForAccepting,
        TradeDoesNotExist,
        TradeCancelled,
        TradeAlreadyFilled,
        TradeWaitForStatusTimeout,
        TradeInventoryItemExpired,
        TradeMissingOfferedAndAcceptedItems,
        TradeAcceptedItemIsBundle,
        TradeAcceptedItemIsStackable,
        TradeInventoryItemInvalidStatus,
        TradeAcceptedCatalogItemInvalid,
        TradeAllowedUsersInvalid,
        TradeInventoryItemDoesNotExist,
        TradeInventoryItemIsConsumed,
        TradeInventoryItemIsStackable,
        TradeAcceptedItemsMismatch,
        InvalidKongregateToken,
        FeatureNotConfiguredForTitle,
        NoMatchingCatalogItemForReceipt,
        InvalidCurrencyCode,
        NoRealMoneyPriceForCatalogItem,
        TradeInventoryItemIsNotTradable,
        TradeAcceptedCatalogItemIsNotTradable,
        UsersAlreadyFriends,
        LinkedIdentifierAlreadyClaimed,
        CustomIdNotLinked,
        TotalDataSizeExceeded,
        DeleteKeyConflict,
        InvalidXboxLiveToken,
        ExpiredXboxLiveToken,
        ResettableStatisticVersionRequired,
        NotAuthorizedByTitle,
        NoPartnerEnabled,
        InvalidPartnerResponse,
        APINotEnabledForGameServerAccess,
        StatisticNotFound,
        StatisticNameConflict,
        StatisticVersionClosedForWrites,
        StatisticVersionInvalid,
        APIClientRequestRateLimitExceeded,
        InvalidJSONContent,
        InvalidDropTable,
        StatisticVersionAlreadyIncrementedForScheduledInterval,
        StatisticCountLimitExceeded,
        StatisticVersionIncrementRateExceeded,
        ContainerKeyInvalid,
        CloudScriptExecutionTimeLimitExceeded,
        NoWritePermissionsForEvent,
        CloudScriptFunctionArgumentSizeExceeded,
        CloudScriptAPIRequestCountExceeded,
        CloudScriptAPIRequestError,
        CloudScriptHTTPRequestError,
        InsufficientGuildRole,
        GuildNotFound,
        OverLimit,
        EventNotFound,
        InvalidEventField,
        InvalidEventName,
        CatalogNotConfigured,
        OperationNotSupportedForPlatform,
        SegmentNotFound,
        StoreNotFound,
        InvalidStatisticName,
        TitleNotQualifiedForLimit,
        InvalidServiceLimitLevel,
        ServiceLimitLevelInTransition,
        CouponAlreadyRedeemed,
        GameServerBuildSizeLimitExceeded,
        GameServerBuildCountLimitExceeded,
        VirtualCurrencyCountLimitExceeded,
        VirtualCurrencyCodeExists,
        TitleNewsItemCountLimitExceeded,
        InvalidTwitchToken,
        TwitchResponseError,
        ProfaneDisplayName,
        UserAlreadyAdded,
        InvalidVirtualCurrencyCode,
        VirtualCurrencyCannotBeDeleted,
        IdentifierAlreadyClaimed,
        IdentifierNotLinked,
        InvalidContinuationToken,
        ExpiredContinuationToken,
        InvalidSegment,
        InvalidSessionId,
        SessionLogNotFound,
        InvalidSearchTerm,
        TwoFactorAuthenticationTokenRequired,
        GameServerHostCountLimitExceeded,
        PlayerTagCountLimitExceeded,
        RequestAlreadyRunning,
        ActionGroupNotFound,
        MaximumSegmentBulkActionJobsRunning,
        NoActionsOnPlayersInSegmentJob,
        DuplicateStatisticName,
        ScheduledTaskNameConflict,
        ScheduledTaskCreateConflict,
        InvalidScheduledTaskName,
        InvalidTaskSchedule,
        SteamNotEnabledForTitle,
        LimitNotAnUpgradeOption,
        NoSecretKeyEnabledForCloudScript,
        TaskNotFound,
        TaskInstanceNotFound,
        InvalidIdentityProviderId,
        MisconfiguredIdentityProvider,
        InvalidScheduledTaskType,
        BillingInformationRequired,
        LimitedEditionItemUnavailable,
        InvalidAdPlacementAndReward,
        AllAdPlacementViewsAlreadyConsumed,
        GoogleOAuthNotConfiguredForTitle,
        GoogleOAuthError,
        UserNotFriend,
        InvalidSignature,
        InvalidPublicKey,
        GoogleOAuthNoIdTokenIncludedInResponse,
        StatisticUpdateInProgress,
        LeaderboardVersionNotAvailable,
        StatisticAlreadyHasPrizeTable,
        PrizeTableHasOverlappingRanks,
        PrizeTableHasMissingRanks,
        PrizeTableRankStartsAtZero,
        InvalidStatistic,
        ExpressionParseFailure,
        ExpressionInvokeFailure,
        ExpressionTooLong,
        DataUpdateRateExceeded,
        RestrictedEmailDomain,
        EncryptionKeyDisabled,
        EncryptionKeyMissing,
        EncryptionKeyBroken,
        NoSharedSecretKeyConfigured,
        SecretKeyNotFound,
        PlayerSecretAlreadyConfigured,
        APIRequestsDisabledForTitle,
        InvalidSharedSecretKey,
        PrizeTableHasNoRanks,
        ProfileDoesNotExist,
        ContentS3OriginBucketNotConfigured,
        InvalidEnvironmentForReceipt,
        EncryptedRequestNotAllowed,
        SignedRequestNotAllowed,
        RequestViewConstraintParamsNotAllowed,
        BadPartnerConfiguration,
        XboxBPCertificateFailure,
        XboxXASSExchangeFailure,
        InvalidEntityId,
        StatisticValueAggregationOverflow,
        EmailMessageFromAddressIsMissing,
        EmailMessageToAddressIsMissing,
        SmtpServerAuthenticationError,
        SmtpServerLimitExceeded,
        SmtpServerInsufficientStorage,
        SmtpServerCommunicationError,
        SmtpServerGeneralFailure,
        EmailClientTimeout,
        EmailClientCanceledTask,
        EmailTemplateMissing,
        InvalidHostForTitleId,
        EmailConfirmationTokenDoesNotExist,
        EmailConfirmationTokenExpired,
        AccountDeleted,
        PlayerSecretNotConfigured,
        InvalidSignatureTime,
        NoContactEmailAddressFound,
        InvalidAuthToken,
        AuthTokenDoesNotExist,
        AuthTokenExpired,
        AuthTokenAlreadyUsedToResetPassword,
        MembershipNameTooLong,
        MembershipNotFound,
        GoogleServiceAccountInvalid,
        GoogleServiceAccountParseFailure,
        EntityTokenMissing,
        EntityTokenInvalid,
        EntityTokenExpired,
        EntityTokenRevoked,
        InvalidProductForSubscription,
        XboxInaccessible,
        SubscriptionAlreadyTaken,
        SmtpAddonNotEnabled,
        APIConcurrentRequestLimitExceeded,
        XboxRejectedXSTSExchangeRequest,
        VariableNotDefined,
        TemplateVersionNotDefined,
        FileTooLarge,
        TitleDeleted,
        TitleContainsUserAccounts,
        TitleDeletionPlayerCleanupFailure,
        EntityFileOperationPending,
        NoEntityFileOperationPending,
        EntityProfileVersionMismatch,
        TemplateVersionTooOld,
        MembershipDefinitionInUse,
        PaymentPageNotConfigured,
        FailedLoginAttemptRateLimitExceeded,
        EntityBlockedByGroup,
        RoleDoesNotExist,
        EntityIsAlreadyMember,
        DuplicateRoleId,
        GroupInvitationNotFound,
        GroupApplicationNotFound,
        OutstandingInvitationAcceptedInstead,
        OutstandingApplicationAcceptedInstead,
        RoleIsGroupDefaultMember,
        RoleIsGroupAdmin,
        RoleNameNotAvailable,
        GroupNameNotAvailable,
        EmailReportAlreadySent,
        EmailReportRecipientBlacklisted,
        EventNamespaceNotAllowed,
        EventEntityNotAllowed,
        InvalidEntityType,
        NullTokenResultFromAad,
        InvalidTokenResultFromAad,
        NoValidCertificateForAad,
        InvalidCertificateForAad,
        DuplicateDropTableId,
        MultiplayerServerError,
        MultiplayerServerTooManyRequests,
        MultiplayerServerNoContent,
        MultiplayerServerBadRequest,
        MultiplayerServerUnauthorized,
        MultiplayerServerForbidden,
        MultiplayerServerNotFound,
        MultiplayerServerConflict,
        MultiplayerServerInternalServerError,
        MultiplayerServerUnavailable,
        ExplicitContentDetected,
        PIIContentDetected,
        InvalidScheduledTaskParameter,
        PerEntityEventRateLimitExceeded,
        TitleDefaultLanguageNotSet,
        EmailTemplateMissingDefaultVersion,
        FacebookInstantGamesIdNotLinked,
        InvalidFacebookInstantGamesSignature,
        FacebookInstantGamesAuthNotConfiguredForTitle,
        EntityProfileConstraintValidationFailed,
        TelemetryIngestionKeyPending,
        TelemetryIngestionKeyNotFound,
        StatisticChildNameInvalid,
        DataIntegrityError,
        VirtualCurrencyCannotBeSetToOlderVersion,
        VirtualCurrencyMustBeWithinIntegerRange,
        EmailTemplateInvalidSyntax,
        EmailTemplateMissingCallback,
        PushNotificationTemplateInvalidPayload,
        InvalidLocalizedPushNotificationLanguage,
        MissingLocalizedPushNotificationMessage,
        PushNotificationTemplateMissingPlatformPayload,
        PushNotificationTemplatePayloadContainsInvalidJson,
        PushNotificationTemplateContainsInvalidIosPayload,
        PushNotificationTemplateContainsInvalidAndroidPayload,
        PushNotificationTemplateIosPayloadMissingNotificationBody,
        PushNotificationTemplateAndroidPayloadMissingNotificationBody,
        PushNotificationTemplateNotFound,
        PushNotificationTemplateMissingDefaultVersion,
        PushNotificationTemplateInvalidSyntax,
        PushNotificationTemplateNoCustomPayloadForV1,
        NoLeaderboardForStatistic,
        TitleNewsMissingDefaultLanguage,
        TitleNewsNotFound,
        TitleNewsDuplicateLanguage,
        TitleNewsMissingTitleOrBody,
        TitleNewsInvalidLanguage,
        EmailRecipientBlacklisted,
        InvalidGameCenterAuthRequest,
        GameCenterAuthenticationFailed,
        CannotEnablePartiesForTitle,
        PartyError,
        PartyRequests,
        PartyNoContent,
        PartyBadRequest,
        PartyUnauthorized,
        PartyForbidden,
        PartyNotFound,
        PartyConflict,
        PartyInternalServerError,
        PartyUnavailable,
        PartyTooManyRequests,
        PushNotificationTemplateMissingName,
        CannotEnableMultiplayerServersForTitle,
        WriteAttemptedDuringExport,
        MultiplayerServerTitleQuotaCoresExceeded,
        AutomationRuleNotFound,
        EntityAPIKeyLimitExceeded,
        EntityAPIKeyNotFound,
        EntityAPIKeyOrSecretInvalid,
        EconomyServiceUnavailable,
        EconomyServiceInternalError,
        QueryRateLimitExceeded,
        EntityAPIKeyCreationDisabledForEntity,
        ForbiddenByEntityPolicy,
        UpdateInventoryRateLimitExceeded,
        StudioCreationRateLimited,
        StudioCreationInProgress,
        DuplicateStudioName,
        StudioNotFound,
        StudioDeleted,
        StudioDeactivated,
        StudioActivated,
        TitleCreationRateLimited,
        TitleCreationInProgress,
        DuplicateTitleName,
        TitleActivationRateLimited,
        TitleActivationInProgress,
        TitleDeactivated,
        TitleActivated,
        CloudScriptAzureFunctionsExecutionTimeLimitExceeded,
        CloudScriptAzureFunctionsArgumentSizeExceeded,
        CloudScriptAzureFunctionsReturnSizeExceeded,
        CloudScriptAzureFunctionsHTTPRequestError,
        VirtualCurrencyBetaGetError,
        VirtualCurrencyBetaCreateError,
        VirtualCurrencyBetaInitialDepositSaveError,
        VirtualCurrencyBetaSaveError,
        VirtualCurrencyBetaDeleteError,
        VirtualCurrencyBetaRestoreError,
        VirtualCurrencyBetaSaveConflict,
        VirtualCurrencyBetaUpdateError,
        InsightsManagementDatabaseNotFound,
        InsightsManagementOperationNotFound,
        InsightsManagementErrorPendingOperationExists,
        InsightsManagementSetPerformanceLevelInvalidParameter,
        InsightsManagementSetStorageRetentionInvalidParameter,
        InsightsManagementGetStorageUsageInvalidParameter,
        InsightsManagementGetOperationStatusInvalidParameter,
        DuplicatePurchaseTransactionId,
        EvaluationModePlayerCountExceeded,
        GetPlayersInSegmentRateLimitExceeded,
        CloudScriptFunctionNameSizeExceeded,
        PaidInsightsFeaturesNotEnabled,
        CloudScriptAzureFunctionsQueueRequestError,
        EvaluationModeTitleCountExceeded,
        InsightsManagementTitleNotInFlight,
        LimitNotFound,
        LimitNotAvailableViaAPI,
        InsightsManagementSetStorageRetentionBelowMinimum,
        InsightsManagementSetStorageRetentionAboveMaximum,
        AppleNotEnabledForTitle,
        InsightsManagementNewActiveEventExportLimitInvalid,
        InsightsManagementSetPerformanceRateLimited,
        PartyRequestsThrottledFromRateLimiter,
        XboxServiceTooManyRequests,
        NintendoSwitchNotEnabledForTitle,
        RequestMultiplayerServersThrottledFromRateLimiter,
        TitleDataOverrideNotFound,
        DuplicateKeys,
        WasNotCreatedWithCloudRoot,
        LegacyMultiplayerServersDeprecated,
        VirtualCurrencyCurrentlyUnavailable,
        SteamUserNotFound,
        ElasticSearchOperationFailed,
        NotImplemented,
        PublisherNotFound,
        PublisherDeleted,
        ApiDisabledForMigration,
        ResourceNameUpdateNotAllowed,
        ApiNotEnabledForTitle,
        DuplicateTitleNameForPublisher,
        AzureTitleCreationInProgress,
        TitleConstraintsPublisherDeletion,
        InvalidPlayerAccountPoolId,
        PlayerAccountPoolNotFound,
        PlayerAccountPoolDeleted,
        TitleCleanupInProgress,
        AzureResourceConcurrentOperationInProgress,
        TitlePublisherUpdateNotAllowed,
        AzureResourceManagerNotSupportedInStamp,
        ApiNotIncludedInAzurePlayFabFeatureSet,
        GoogleServiceAccountFailedAuth,
        GoogleAPIServiceUnavailable,
        GoogleAPIServiceUnknownError,
        NoValidIdentityForAad,
        PlayerIdentityLinkNotFound,
        PhotonApplicationIdAlreadyInUse,
        CloudScriptUnableToDeleteProductionRevision,
        CustomIdNotFound,
        AutomationInvalidInput,
        AutomationInvalidRuleName,
        AutomationRuleAlreadyExists,
        AutomationRuleLimitExceeded,
        InvalidGooglePlayGamesServerAuthCode,
        PlayStreamConnectionFailed,
        InvalidEventContents,
        InsightsV1Deprecated,
        AnalysisSubscriptionNotFound,
        AnalysisSubscriptionFailed,
        AnalysisSubscriptionFoundAlready,
        AnalysisSubscriptionManagementInvalidInput,
        InvalidGameCenterId,
        InvalidNintendoSwitchAccountId,
        EntityAPIKeysNotSupported,
        IpAddressBanned,
        EntityLineageBanned,
        NamespaceMismatch,
        InvalidServiceConfiguration,
        InvalidNamespaceMismatch,
        LeaderboardColumnLengthMismatch,
        InvalidStatisticScore,
        LeaderboardColumnsNotSpecified,
        LeaderboardMaxSizeTooLarge,
        InvalidAttributeStatisticsSpecified,
        LeaderboardNotFound,
        TokenSigningKeyNotFound,
        LeaderboardNameConflict,
        LinkedStatisticColumnMismatch,
        NoLinkedStatisticToLeaderboard,
        StatDefinitionAlreadyLinkedToLeaderboard,
        LinkingStatsNotAllowedForEntityType,
        LeaderboardCountLimitExceeded,
        LeaderboardSizeLimitExceeded,
        LeaderboardDefinitionModificationNotAllowedWhileLinked,
        StatisticDefinitionModificationNotAllowedWhileLinked,
        LeaderboardUpdateNotAllowedWhileLinked,
        CloudScriptAzureFunctionsEventHubRequestError,
        ExternalEntityNotAllowedForTier,
        InvalidBaseTimeForInterval,
        EntityTypeMismatchWithStatDefinition,
        SpecifiedVersionLeaderboardNotFound,
        LeaderboardColumnLengthMismatchWithStatDefinition,
        DuplicateColumnNameFound,
        LinkedStatisticColumnNotFound,
        LinkedStatisticColumnRequired,
        MultipleLinkedStatisticsNotAllowed,
        DuplicateLinkedStatisticColumnNameFound,
        AggregationTypeNotAllowedForMultiColumnStatistic,
        MaxQueryableVersionsValueNotAllowedForTier,
        StatisticDefinitionHasNullOrEmptyVersionConfiguration,
        StatisticColumnLengthMismatch,
        InvalidExternalEntityId,
        UpdatingStatisticsUsingTransactionIdNotAvailableForFreeTier,
        TransactionAlreadyApplied,
        ReportDataNotRetrievedSuccessfully,
        ResetIntervalCannotBeModified,
        VersionIncrementRateExceeded,
        InvalidSteamUsername,
        InvalidVersionResetForLinkedLeaderboard,
        BattleNetNotEnabledForTitle,
        ReportNotProcessed,
        DataNotAvailable,
        InvalidReportName,
        ResourceNotModified,
        MatchmakingEntityInvalid,
        MatchmakingPlayerAttributesInvalid,
        MatchmakingQueueNotFound,
        MatchmakingMatchNotFound,
        MatchmakingTicketNotFound,
        MatchmakingAlreadyJoinedTicket,
        MatchmakingTicketAlreadyCompleted,
        MatchmakingQueueConfigInvalid,
        MatchmakingMemberProfileInvalid,
        NintendoSwitchDeviceIdNotLinked,
        MatchmakingNotEnabled,
        MatchmakingPlayerAttributesTooLarge,
        MatchmakingNumberOfPlayersInTicketTooLarge,
        MatchmakingAttributeInvalid,
        MatchmakingPlayerHasNotJoinedTicket,
        MatchmakingRateLimitExceeded,
        MatchmakingTicketMembershipLimitExceeded,
        MatchmakingUnauthorized,
        MatchmakingQueueLimitExceeded,
        MatchmakingRequestTypeMismatch,
        MatchmakingBadRequest,
        PubSubFeatureNotEnabledForTitle,
        PubSubTooManyRequests,
        PubSubConnectionNotFoundForEntity,
        PubSubConnectionHandleInvalid,
        PubSubSubscriptionLimitExceeded,
        TitleConfigNotFound,
        TitleConfigUpdateConflict,
        TitleConfigSerializationError,
        CatalogApiNotImplemented,
        CatalogEntityInvalid,
        CatalogTitleIdMissing,
        CatalogPlayerIdMissing,
        CatalogClientIdentityInvalid,
        CatalogOneOrMoreFilesInvalid,
        CatalogItemMetadataInvalid,
        CatalogItemIdInvalid,
        CatalogSearchParameterInvalid,
        CatalogFeatureDisabled,
        CatalogConfigInvalid,
        CatalogItemTypeInvalid,
        CatalogBadRequest,
        CatalogTooManyRequests,
        InvalidCatalogItemConfiguration,
        ExportInvalidStatusUpdate,
        ExportInvalidPrefix,
        ExportBlobContainerDoesNotExist,
        ExportNotFound,
        ExportCouldNotUpdate,
        ExportInvalidStorageType,
        ExportAmazonBucketDoesNotExist,
        ExportInvalidBlobStorage,
        ExportKustoException,
        ExportKustoConnectionFailed,
        ExportUnknownError,
        ExportCantEditPendingExport,
        ExportLimitExports,
        ExportLimitEvents,
        ExportInvalidPartitionStatusModification,
        ExportCouldNotCreate,
        ExportNoBackingDatabaseFound,
        ExportCouldNotDelete,
        ExportCannotDetermineEventQuery,
        ExportInvalidQuerySchemaModification,
        ExportQuerySchemaMissingRequiredColumns,
        ExportCannotParseQuery,
        ExportControlCommandsNotAllowed,
        ExportQueryMissingTableReference,
        ExportInsightsV1Deprecated,
        ExplorerBasicInvalidQueryName,
        ExplorerBasicInvalidQueryDescription,
        ExplorerBasicInvalidQueryConditions,
        ExplorerBasicInvalidQueryStartDate,
        ExplorerBasicInvalidQueryEndDate,
        ExplorerBasicInvalidQueryGroupBy,
        ExplorerBasicInvalidQueryAggregateType,
        ExplorerBasicInvalidQueryAggregateProperty,
        ExplorerBasicLoadQueriesError,
        ExplorerBasicLoadQueryError,
        ExplorerBasicCreateQueryError,
        ExplorerBasicDeleteQueryError,
        ExplorerBasicUpdateQueryError,
        ExplorerBasicSavedQueriesLimit,
        ExplorerBasicSavedQueryNotFound,
        TenantShardMapperShardNotFound,
        TitleNotEnabledForParty,
        PartyVersionNotFound,
        MultiplayerServerBuildReferencedByMatchmakingQueue,
        MultiplayerServerBuildReferencedByBuildAlias,
        MultiplayerServerBuildAliasReferencedByMatchmakingQueue,
        PartySerializationError,
        ExperimentationExperimentStopped,
        ExperimentationExperimentRunning,
        ExperimentationExperimentNotFound,
        ExperimentationExperimentNeverStarted,
        ExperimentationExperimentDeleted,
        ExperimentationClientTimeout,
        ExperimentationInvalidVariantConfiguration,
        ExperimentationInvalidVariableConfiguration,
        ExperimentInvalidId,
        ExperimentationNoScorecard,
        ExperimentationTreatmentAssignmentFailed,
        ExperimentationTreatmentAssignmentDisabled,
        ExperimentationInvalidDuration,
        ExperimentationMaxExperimentsReached,
        ExperimentationExperimentSchedulingInProgress,
        ExperimentationInvalidEndDate,
        ExperimentationInvalidStartDate,
        ExperimentationMaxDurationExceeded,
        ExperimentationExclusionGroupNotFound,
        ExperimentationExclusionGroupInsufficientCapacity,
        ExperimentationExclusionGroupCannotDelete,
        ExperimentationExclusionGroupInvalidTrafficAllocation,
        ExperimentationExclusionGroupInvalidName,
        ExperimentationLegacyExperimentInvalidOperation,
        MaxActionDepthExceeded,
        TitleNotOnUpdatedPricingPlan,
        SegmentManagementTitleNotInFlight,
        SegmentManagementNoExpressionTree,
        SegmentManagementTriggerActionCountOverLimit,
        SegmentManagementSegmentCountOverLimit,
        SegmentManagementInvalidSegmentId,
        SegmentManagementInvalidInput,
        SegmentManagementInvalidSegmentName,
        DeleteSegmentRateLimitExceeded,
        CreateSegmentRateLimitExceeded,
        UpdateSegmentRateLimitExceeded,
        GetSegmentsRateLimitExceeded,
        AsyncExportNotInFlight,
        AsyncExportNotFound,
        AsyncExportRateLimitExceeded,
        AnalyticsSegmentCountOverLimit,
        SnapshotNotFound,
        InventoryApiNotImplemented,
        InventoryCollectionDeletionDisallowed,
        LobbyDoesNotExist,
        LobbyRateLimitExceeded,
        LobbyPlayerAlreadyJoined,
        LobbyNotJoinable,
        LobbyMemberCannotRejoin,
        LobbyCurrentPlayersMoreThanMaxPlayers,
        LobbyPlayerNotPresent,
        LobbyBadRequest,
        LobbyPlayerMaxLobbyLimitExceeded,
        LobbyNewOwnerMustBeConnected,
        LobbyCurrentOwnerStillConnected,
        LobbyMemberIsNotOwner,
        LobbyServerMismatch,
        LobbyServerNotFound,
        LobbyDifferentServerAlreadyJoined,
        LobbyServerAlreadyJoined,
        LobbyIsNotClientOwned,
        LobbyDoesNotUseConnections,
        EventSamplingInvalidRatio,
        EventSamplingInvalidEventNamespace,
        EventSamplingInvalidEventName,
        EventSamplingRatioNotFound,
        TelemetryKeyNotFound,
        TelemetryKeyInvalidName,
        TelemetryKeyAlreadyExists,
        TelemetryKeyInvalid,
        TelemetryKeyCountOverLimit,
        TelemetryKeyDeactivated,
        TelemetryKeyLongInsightsRetentionNotAllowed,
        EventSinkConnectionInvalid,
        EventSinkConnectionUnauthorized,
        EventSinkRegionInvalid,
        EventSinkLimitExceeded,
        EventSinkSasTokenInvalid,
        EventSinkNotFound,
        EventSinkNameInvalid,
        EventSinkSasTokenPermissionInvalid,
        EventSinkSecretInvalid,
        EventSinkTenantNotFound,
        EventSinkAadNotFound,
        EventSinkDatabaseNotFound,
        EventSinkTitleUnauthorized,
        EventSinkInsufficientRoleAssignment,
        EventSinkContainerNotFound,
        EventSinkTenantIdInvalid,
        EventSinkResourceMisconfigured,
        EventSinkAccessDenied,
        EventSinkWriteConflict,
        EventSinkResourceNotFound,
        EventSinkResourceFeatureNotSupported,
        EventSinkBucketNameInvalid,
        EventSinkResourceUnavailable,
        OperationCanceled,
        InvalidDisplayNameRandomSuffixLength,
        AllowNonUniquePlayerDisplayNamesDisableNotAllowed,
        PartitionedEventInvalid,
        PartitionedEventCountOverLimit,
        ManageEventNamespaceInvalid,
        ManageEventNameInvalid,
        ManagedEventNotFound,
        ManageEventsInvalidRatio,
        ManagedEventInvalid,
        PlayerCustomPropertiesPropertyNameTooLong,
        PlayerCustomPropertiesPropertyNameIsInvalid,
        PlayerCustomPropertiesStringPropertyValueTooLong,
        PlayerCustomPropertiesValueIsInvalidType,
        PlayerCustomPropertiesVersionMismatch,
        PlayerCustomPropertiesPropertyCountTooHigh,
        PlayerCustomPropertiesDuplicatePropertyName,
        PlayerCustomPropertiesPropertyDoesNotExist,
        AddonAlreadyExists,
        AddonDoesntExist,
        CopilotDisabled,
        CopilotInvalidRequest,
        TrueSkillUnauthorized,
        TrueSkillInvalidTitleId,
        TrueSkillInvalidScenarioId,
        TrueSkillInvalidModelId,
        TrueSkillInvalidModelName,
        TrueSkillInvalidPlayerIds,
        TrueSkillInvalidEntityKey,
        TrueSkillInvalidConditionKey,
        TrueSkillInvalidConditionValue,
        TrueSkillInvalidConditionAffinityWeight,
        TrueSkillInvalidEventName,
        TrueSkillMatchResultCreated,
        TrueSkillMatchResultAlreadySubmitted,
        TrueSkillBadPlayerIdInMatchResult,
        TrueSkillInvalidBotIdInMatchResult,
        TrueSkillDuplicatePlayerInMatchResult,
        TrueSkillNoPlayerInMatchResultTeam,
        TrueSkillPlayersInMatchResultExceedingLimit,
        TrueSkillInvalidPreMatchPartyInMatchResult,
        TrueSkillInvalidTimestampInMatchResult,
        TrueSkillStartTimeMissingInMatchResult,
        TrueSkillEndTimeMissingInMatchResult,
        TrueSkillInvalidPlayerSecondsPlayedInMatchResult,
        TrueSkillNoTeamInMatchResult,
        TrueSkillNotEnoughTeamsInMatchResult,
        TrueSkillInvalidRanksInMatchResult,
        TrueSkillNoWinnerInMatchResult,
        TrueSkillMissingRequiredCondition,
        TrueSkillMissingRequiredEvent,
        TrueSkillUnknownEventName,
        TrueSkillInvalidEventCount,
        TrueSkillUnknownConditionKey,
        TrueSkillUnknownConditionValue,
        TrueSkillScenarioConfigDoesNotExist,
        TrueSkillUnknownModelId,
        TrueSkillNoModelInScenario,
        TrueSkillNotSupportedForTitle,
        TrueSkillModelIsNotActive,
        TrueSkillUnauthorizedToQueryOtherPlayerSkills,
        TrueSkillInvalidMaxIterations,
        TrueSkillEndTimeBeforeStartTime,
        TrueSkillInvalidJobId,
        TrueSkillInvalidMetadataId,
        TrueSkillMissingBuildVerison,
        TrueSkillJobAlreadyExists,
        TrueSkillJobNotFound,
        TrueSkillOperationCanceled,
        TrueSkillActiveModelLimitExceeded,
        TrueSkillTotalModelLimitExceeded,
        TrueSkillUnknownInitialModelId,
        TrueSkillUnauthorizedForJob,
        TrueSkillInvalidScenarioName,
        TrueSkillConditionStateIsRequired,
        TrueSkillEventStateIsRequired,
        TrueSkillDuplicateEvent,
        TrueSkillDuplicateCondition,
        TrueSkillInvalidAnomalyThreshold,
        TrueSkillConditionKeyLimitExceeded,
        TrueSkillConditionValuePerKeyLimitExceeded,
        TrueSkillInvalidTimestamp,
        TrueSkillEventLimitExceeded,
        TrueSkillInvalidPlayers,
        TrueSkillTrueSkillPlayerNull,
        TrueSkillInvalidPlayerId,
        TrueSkillInvalidSquadSize,
        TrueSkillConditionSetNotInModel,
        TrueSkillModelStateInvalidForOperation,
        TrueSkillScenarioContainsActiveModel,
        TrueSkillInvalidConditionRank,
        TrueSkillTotalScenarioLimitExceeded,
        TrueSkillInvalidConditionsList,
        GameSaveManifestNotFound,
        GameSaveManifestVersionAlreadyExists,
        GameSaveConflictUpdatingManifest,
        GameSaveManifestUpdatesNotAllowed,
        GameSaveFileAlreadyExists,
        GameSaveManifestVersionNotFinalized,
        GameSaveUnknownFileInManifest,
        GameSaveFileExceededReportedSize,
        GameSaveFileNotUploaded,
        GameSaveBadRequest,
        GameSaveOperationNotAllowed,
        GameSaveDataStorageQuotaExceeded,
        GameSaveNewerManifestExists,
        GameSaveBaseVersionNotAvailable,
        GameSaveManifestVersionQuarantined,
        GameSaveManifestUploadProgressUpdateNotAllowed,
        GameSaveNotFinalizedManifestNotEligibleAsKnownGood,
        GameSaveNoUpdatesRequested,
        GameSaveTitleDoesNotExist,
        GameSaveOperationNotAllowedForTitle,
        GameSaveManifestFilesLimitExceeded,
        GameSaveManifestDescriptionUpdateNotAllowed,
        GameSaveTitleConfigNotFound,
        GameSaveTitleAlreadyOnboarded,
        StateShareForbidden,
        StateShareTitleNotInFlight,
        StateShareStateNotFound,
        StateShareLinkNotFound,
        StateShareStateRedemptionLimitExceeded,
        StateShareStateRedemptionLimitNotUpdated,
        StateShareCreatedStatesLimitExceeded,
        StateShareIdMissingOrMalformed,
        PlayerCreationDisabled,
        AccountAlreadyExists
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
    public class GetAllSegmentsRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class GetAllSegmentsResult : PlayFabResultCommon
    {
        public List<GetSegmentResult> Segments;
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
        public string PlayFabId;
    }
    [Serializable]
    public class GetCharacterInventoryRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class GetCharacterInventoryResult : PlayFabResultCommon
    {
        public string CharacterId;
        public List<ItemInstance> Inventory;
        public string PlayFabId;
        public Dictionary<string,int> VirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
    }

    [Serializable]
    public class GetCharacterLeaderboardRequest : PlayFabRequestCommon
    {
        public int MaxResultsCount;
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
        public string PlayFabId;
    }

    [Serializable]
    public class GetCharacterStatisticsResult : PlayFabResultCommon
    {
        public string CharacterId;
        public Dictionary<string,int> CharacterStatistics;
        public string PlayFabId;
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
    public class GetFriendLeaderboardRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExternalFriendSources? ExternalPlatformFriends;
        public int MaxResultsCount;
        public string PlayFabId;
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
        public string PlayFabId;
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
        public int MaxResultsCount;
        public string PlayFabId;
        public string StatisticName;
    }
    [Serializable]
    public class GetLeaderboardAroundCharacterResult : PlayFabResultCommon
    {
        public List<CharacterLeaderboardEntry> Leaderboard;
    }

    [Serializable]
    public class GetLeaderboardAroundUserRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int MaxResultsCount;
        public string PlayFabId;
        public PlayerProfileViewConstraints ProfileConstraints;
        public string StatisticName;
        public int? Version;
    }
    [Serializable]
    public class GetLeaderboardAroundUserResult : PlayFabResultCommon
    {
        public List<PlayerLeaderboardEntry> Leaderboard;
        public DateTime? NextReset;
        public int Version;
    }

    [Serializable]
    public class GetLeaderboardForUsersCharactersRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
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
        public int MaxResultsCount;
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
        public string PlayFabId;
        public string PropertyName;
    }

    [Serializable]
    public class GetPlayerCustomPropertyResult : PlayFabResultCommon
    {
        public string PlayFabId;
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
    public class GetPlayerSegmentsResult : PlayFabResultCommon
    {
        public List<GetSegmentResult> Segments;
    }
    [Serializable]
    public class GetPlayersInSegmentRequest : PlayFabRequestCommon
    {
        public string ContinuationToken;
        public Dictionary<string,string> CustomTags;
        public bool? GetProfilesAsync;
        public uint? MaxBatchSize;
        public uint? SecondsToLive;
        public string SegmentId;
    }

    [Serializable]
    public class GetPlayersInSegmentResult : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<PlayerProfile> PlayerProfiles;
        public int ProfilesInSegment;
    }

    [Serializable]
    public class GetPlayersSegmentsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class GetPlayerStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public List<string> StatisticNames;
        public List<StatisticNameVersion> StatisticNameVersions;
    }
    [Serializable]
    public class GetPlayerStatisticsResult : PlayFabResultCommon
    {
        public string PlayFabId;
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
    public class GetRandomResultTablesRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public List<string> TableIDs;
    }
    [Serializable]
    public class GetRandomResultTablesResult : PlayFabResultCommon
    {
        public Dictionary<string,RandomResultTableListing> Tables;
    }

    [Serializable]
    public class GetSegmentResult : PlayFabBaseModel
    {
        public string ABTestParent;
        public string Id;
        public string Name;
    }

    [Serializable]
    public class GetServerCustomIDsFromPlayFabIDsRequest : PlayFabRequestCommon
    {
        public List<string> PlayFabIDs;
    }
    [Serializable]
    public class GetServerCustomIDsFromPlayFabIDsResult : PlayFabResultCommon
    {
        public List<ServerCustomIDPlayFabIDPair> Data;
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
    public class GetStoreItemsResult : PlayFabResultCommon
    {
        public string CatalogVersion;
        public StoreMarketingModel MarketingData;
        public SourceType? Source;
        public List<StoreItem> Store;
        public string StoreId;
    }
    [Serializable]
    public class GetStoreItemsServerRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
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
    public class GetUserAccountInfoRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class GetUserAccountInfoResult : PlayFabResultCommon
    {
        public UserAccountInfo UserInfo;
    }
    [Serializable]
    public class GetUserBansRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class GetUserBansResult : PlayFabResultCommon
    {
        public List<BanInfo> BanData;
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
        public string PlayFabId;
    }
    [Serializable]
    public class GetUserInventoryRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class GetUserInventoryResult : PlayFabResultCommon
    {
        public List<ItemInstance> Inventory;
        public string PlayFabId;
        public Dictionary<string,int> VirtualCurrency;
        public Dictionary<string,VirtualCurrencyRechargeTime> VirtualCurrencyRechargeTimes;
    }
    [Serializable]
    public class GrantCharacterToUserRequest : PlayFabRequestCommon
    {
        public string CharacterName;
        public string CharacterType;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class GrantCharacterToUserResult : PlayFabResultCommon
    {
        public string CharacterId;
    }
    [Serializable]
    public class GrantedItemInstance : PlayFabBaseModel
    {
        public string Annotation;
        public List<string> BundleContents;
        public string BundleParent;
        public string CatalogVersion;
        public string CharacterId;
        public Dictionary<string,string> CustomData;
        public string DisplayName;
        public DateTime? Expiration;
        public string ItemClass;
        public string ItemId;
        public string ItemInstanceId;
        public string PlayFabId;
        public DateTime? PurchaseDate;
        public int? RemainingUses;
        public bool Result;
        public string UnitCurrency;
        public uint UnitPrice;
        public int? UsesIncrementedBy;
    }
    [Serializable]
    public class GrantItemsToCharacterRequest : PlayFabRequestCommon
    {
        public string Annotation;
        public string CatalogVersion;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public List<string> ItemIds;
        public string PlayFabId;
    }

    [Serializable]
    public class GrantItemsToCharacterResult : PlayFabResultCommon
    {
        public List<GrantedItemInstance> ItemGrantResults;
    }
    [Serializable]
    public class GrantItemsToUserRequest : PlayFabRequestCommon
    {
        public string Annotation;
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public List<string> ItemIds;
        public string PlayFabId;
    }
    [Serializable]
    public class GrantItemsToUserResult : PlayFabResultCommon
    {
        public List<GrantedItemInstance> ItemGrantResults;
    }
    [Serializable]
    public class GrantItemsToUsersRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public List<ItemGrant> ItemGrants;
    }
    [Serializable]
    public class GrantItemsToUsersResult : PlayFabResultCommon
    {
        public List<GrantedItemInstance> ItemGrantResults;
    }

    [Serializable]
    public class ItemGrant : PlayFabBaseModel
    {
        public string Annotation;
        public string CharacterId;
        public Dictionary<string,string> Data;
        public string ItemId;
        public List<string> KeysToRemove;
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
    public class LinkedPlatformAccountModel : PlayFabBaseModel
    {
        public string Email;
        public LoginIdentityProvider? Platform;
        public string PlatformUserId;
        public string Username;
    }

    [Serializable]
    public class LinkNintendoServiceAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string IdentityToken;
        public string PlayFabId;
    }

    [Serializable]
    public class LinkNintendoServiceAccountSubjectRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string PlayFabId;
        public string Subject;
    }

    [Serializable]
    public class LinkNintendoSwitchDeviceIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string NintendoSwitchDeviceId;
        public string PlayFabId;
    }

    [Serializable]
    public class LinkNintendoSwitchDeviceIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkPSNAccountRequest : PlayFabRequestCommon
    {
        public string AuthCode;
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public int? IssuerId;
        public string PlayFabId;
        public string RedirectUri;
    }

    [Serializable]
    public class LinkPSNAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkPSNIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public int? IssuerId;
        public string PlayFabId;
        public string PSNUserId;
    }

    [Serializable]
    public class LinkPSNIdResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkServerCustomIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string PlayFabId;
        public string ServerCustomId;
    }

    [Serializable]
    public class LinkServerCustomIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkSteamIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string PlayFabId;
        public string SteamId;
    }

    [Serializable]
    public class LinkSteamIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LinkXboxAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceLink;
        public string PlayFabId;
        public string XboxToken;
    }

    [Serializable]
    public class LinkXboxAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class ListPlayerCustomPropertiesRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class ListPlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public string PlayFabId;
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
    public class LocalizedPushNotificationProperties : PlayFabBaseModel
    {
        public string Message;
        public string Subject;
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
    public class LoginWithAndroidDeviceIDRequest : PlayFabRequestCommon
    {
        public string AndroidDevice;
        public string AndroidDeviceId;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string OS;
    }
    [Serializable]
    public class LoginWithCustomIDRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public string CustomId;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
    }
    [Serializable]
    public class LoginWithIOSDeviceIDRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public string DeviceId;
        public string DeviceModel;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string OS;
    }
    [Serializable]
    public class LoginWithPSNRequest : PlayFabRequestCommon
    {
        public string AuthCode;
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public int? IssuerId;
        public string RedirectUri;
    }

    [Serializable]
    public class LoginWithServerCustomIdRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string PlayerSecret;
        public string ServerCustomId;
    }
    [Serializable]
    public class LoginWithSteamIdRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string SteamId;
    }
    [Serializable]
    public class LoginWithXboxIdRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
        public string Sandbox;
        public string XboxId;
    }
    [Serializable]
    public class LoginWithXboxRequest : PlayFabRequestCommon
    {
        public bool? CreateAccount;
        public Dictionary<string,string> CustomTags;
        public GetPlayerCombinedInfoRequestParams InfoRequestParameters;
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
    public class ModifyCharacterVirtualCurrencyResult : PlayFabResultCommon
    {
        public int Balance;
        public string VirtualCurrency;
    }
    [Serializable]
    public class ModifyItemUsesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ItemInstanceId;
        public string PlayFabId;
        public int UsesToAdd;
    }

    [Serializable]
    public class ModifyItemUsesResult : PlayFabResultCommon
    {
        public string ItemInstanceId;
        public int RemainingUses;
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
    public class MoveItemToCharacterFromCharacterRequest : PlayFabRequestCommon
    {
        public string GivingCharacterId;
        public string ItemInstanceId;
        public string PlayFabId;
        public string ReceivingCharacterId;
    }

    [Serializable]
    public class MoveItemToCharacterFromCharacterResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class MoveItemToCharacterFromUserRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public string ItemInstanceId;
        public string PlayFabId;
    }

    [Serializable]
    public class MoveItemToCharacterFromUserResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class MoveItemToUserFromCharacterRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public string ItemInstanceId;
        public string PlayFabId;
    }

    [Serializable]
    public class MoveItemToUserFromCharacterResult : PlayFabResultCommon
    {
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
    public class PlayerLeaderboardEntry : PlayFabBaseModel
    {
        public string DisplayName;
        public string PlayFabId;
        public int Position;
        public PlayerProfileModel Profile;
        public int StatValue;
    }

    [Serializable]
    public class PlayerLinkedAccount : PlayFabBaseModel
    {
        public string Email;
        public LoginIdentityProvider? Platform;
        public string PlatformUserId;
        public string Username;
    }

    [Serializable]
    public class PlayerLocation : PlayFabBaseModel
    {
        public string City;
        public ContinentCode ContinentCode;
        public CountryCode CountryCode;
        public double? Latitude;
        public double? Longitude;
    }

    [Serializable]
    public class PlayerProfile : PlayFabBaseModel
    {
        public List<AdCampaignAttribution> AdCampaignAttributions;
        public string AvatarUrl;
        public DateTime? BannedUntil;
        public ChurnRiskLevel? ChurnPrediction;
        public List<ContactEmailInfo> ContactEmailAddresses;
        public DateTime? Created;
        public Dictionary<string,object> CustomProperties;
        public string DisplayName;
        public DateTime? LastLogin;
        public List<PlayerLinkedAccount> LinkedAccounts;
        public Dictionary<string,PlayerLocation> Locations;
        public LoginIdentityProvider? Origination;
        public List<string> PlayerExperimentVariants;
        public string PlayerId;
        public List<PlayerStatistic> PlayerStatistics;
        public string PublisherId;
        public List<PushNotificationRegistration> PushNotificationRegistrations;
        public Dictionary<string,int> Statistics;
        public List<string> Tags;
        public string TitleId;
        public uint? TotalValueToDateInUSD;
        public Dictionary<string,uint> ValuesToDate;
        public Dictionary<string,int> VirtualCurrencyBalances;
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
    public class PlayerStatistic : PlayFabBaseModel
    {
        public string Id;
        public string Name;
        public int StatisticValue;
        public int StatisticVersion;
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
    public class PushNotificationPackage : PlayFabBaseModel
    {
        public int Badge;
        public string CustomData;
        public string Icon;
        public string Message;
        public string Sound;
        public string Title;
    }

    public enum PushNotificationPlatform
    {
        ApplePushNotificationService,
        GoogleCloudMessaging
    }

    [Serializable]
    public class PushNotificationRegistration : PlayFabBaseModel
    {
        public string NotificationEndpointARN;
        public PushNotificationPlatform? Platform;
    }

    [Serializable]
    public class PushNotificationRegistrationModel : PlayFabBaseModel
    {
        public string NotificationEndpointARN;
        public PushNotificationPlatform? Platform;
    }

    [Serializable]
    public class RandomResultTableListing : PlayFabBaseModel
    {
        public string CatalogVersion;
        public List<ResultTableNode> Nodes;
        public string TableId;
    }
    [Serializable]
    public class RedeemCouponRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public string CouponCode;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class RedeemCouponResult : PlayFabResultCommon
    {
        public List<ItemInstance> GrantedItems;
    }

    [Serializable]
    public class RemoveFriendRequest : PlayFabRequestCommon
    {
        public string FriendPlayFabId;
        public string PlayFabId;
    }

    [Serializable]
    public class RemoveGenericIDRequest : PlayFabRequestCommon
    {
        public GenericServiceId GenericId;
        public string PlayFabId;
    }
    [Serializable]
    public class RemovePlayerTagRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string TagName;
    }

    [Serializable]
    public class RemovePlayerTagResult : PlayFabResultCommon
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
    public class ReportPlayerServerRequest : PlayFabRequestCommon
    {
        public string Comment;
        public Dictionary<string,string> CustomTags;
        public string ReporteeId;
        public string ReporterId;
    }
    [Serializable]
    public class ReportPlayerServerResult : PlayFabResultCommon
    {
        public int SubmissionsRemaining;
    }

    [Serializable]
    public class ResultTableNode : PlayFabBaseModel
    {
        public string ResultItem;
        public ResultTableNodeType ResultItemType;
        public int Weight;
    }

    public enum ResultTableNodeType
    {
        ItemId,
        TableId
    }
    [Serializable]
    public class RevokeAllBansForUserRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class RevokeAllBansForUserResult : PlayFabResultCommon
    {
        public List<BanInfo> BanData;
    }
    [Serializable]
    public class RevokeBansRequest : PlayFabRequestCommon
    {
        public List<string> BanIds;
    }

    [Serializable]
    public class RevokeBansResult : PlayFabResultCommon
    {
        public List<BanInfo> BanData;
    }

    [Serializable]
    public class RevokeInventoryItem : PlayFabBaseModel
    {
        public string CharacterId;
        public string ItemInstanceId;
        public string PlayFabId;
    }
    [Serializable]
    public class RevokeInventoryItemRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public string ItemInstanceId;
        public string PlayFabId;
    }
    [Serializable]
    public class RevokeInventoryItemsRequest : PlayFabRequestCommon
    {
        public List<RevokeInventoryItem> Items;
    }

    [Serializable]
    public class RevokeInventoryItemsResult : PlayFabResultCommon
    {
        public List<RevokeItemError> Errors;
    }

    [Serializable]
    public class RevokeInventoryResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class RevokeItemError : PlayFabBaseModel
    {
        public GenericErrorCodes? Error;
        public RevokeInventoryItem Item;
    }
    [Serializable]
    public class SavePushNotificationTemplateRequest : PlayFabRequestCommon
    {
        public string AndroidPayload;
        public string Id;
        public string IOSPayload;
        public Dictionary<string,LocalizedPushNotificationProperties> LocalizedPushNotificationTemplates;
        public string Name;
    }
    [Serializable]
    public class SavePushNotificationTemplateResult : PlayFabResultCommon
    {
        public string PushNotificationTemplateId;
    }

    [Serializable]
    public class ScriptExecutionError : PlayFabBaseModel
    {
        public string Error;
        public string Message;
        public string StackTrace;
    }
    [Serializable]
    public class SendCustomAccountRecoveryEmailRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Email;
        public string EmailTemplateId;
        public string Username;
    }

    [Serializable]
    public class SendCustomAccountRecoveryEmailResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SendEmailFromTemplateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string EmailTemplateId;
        public string PlayFabId;
    }

    [Serializable]
    public class SendEmailFromTemplateResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SendPushNotificationFromTemplateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PushNotificationTemplateId;
        public string Recipient;
    }

    [Serializable]
    public class SendPushNotificationRequest : PlayFabRequestCommon
    {
        public List<AdvancedPushPlatformMsg> AdvancedPlatformDelivery;
        public Dictionary<string,string> CustomTags;
        public string Message;
        public PushNotificationPackage Package;
        public string Recipient;
        public string Subject;
        public List<PushNotificationPlatform> TargetPlatforms;
    }

    [Serializable]
    public class SendPushNotificationResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class ServerCustomIDPlayFabIDPair : PlayFabBaseModel
    {
        public string PlayFabId;
        public string ServerCustomId;
    }

    [Serializable]
    public class ServerLoginResult : PlayFabResultCommon
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
    public class SetFriendTagsRequest : PlayFabRequestCommon
    {
        public string FriendPlayFabId;
        public string PlayFabId;
        public List<string> Tags;
    }
    [Serializable]
    public class SetPlayerSecretRequest : PlayFabRequestCommon
    {
        public string PlayerSecret;
        public string PlayFabId;
    }

    [Serializable]
    public class SetPlayerSecretResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SetPublisherDataRequest : PlayFabRequestCommon
    {
        public string Key;
        public string Value;
    }

    [Serializable]
    public class SetPublisherDataResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SetTitleDataRequest : PlayFabRequestCommon
    {
        public string Key;
        public string Value;
    }

    [Serializable]
    public class SetTitleDataResult : PlayFabResultCommon
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
    public class SubtractCharacterVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string VirtualCurrency;
    }

    [Serializable]
    public class SubtractUserVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
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
    public class UnlinkNintendoServiceAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class UnlinkNintendoSwitchDeviceIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string NintendoSwitchDeviceId;
        public string PlayFabId;
    }

    [Serializable]
    public class UnlinkNintendoSwitchDeviceIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkPSNAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class UnlinkPSNAccountResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkServerCustomIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string ServerCustomId;
    }

    [Serializable]
    public class UnlinkServerCustomIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkSteamIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class UnlinkSteamIdResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UnlinkXboxAccountRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
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
        public string PlayFabId;
    }
    [Serializable]
    public class UnlockContainerItemRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string CharacterId;
        public string ContainerItemId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
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
        public string PlayFabId;
    }
    [Serializable]
    public class UpdateBanRequest : PlayFabBaseModel
    {
        public bool? Active;
        public string BanId;
        public DateTime? Expires;
        public string IPAddress;
        public bool? Permanent;
        public string Reason;
        public UserFamilyType? UserFamilyType;
    }
    [Serializable]
    public class UpdateBansRequest : PlayFabRequestCommon
    {
        public List<UpdateBanRequest> Bans;
    }

    [Serializable]
    public class UpdateBansResult : PlayFabResultCommon
    {
        public List<BanInfo> BanData;
    }
    [Serializable]
    public class UpdateCharacterDataRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public List<string> KeysToRemove;
        public UserDataPermission? Permission;
        public string PlayFabId;
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
        public string PlayFabId;
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
        public string PlayFabId;
        public List<UpdateProperty> Properties;
    }

    [Serializable]
    public class UpdatePlayerCustomPropertiesResult : PlayFabResultCommon
    {
        public string PlayFabId;
        public int PropertiesVersion;
    }
    [Serializable]
    public class UpdatePlayerStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceUpdate;
        public string PlayFabId;
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
        public string PlayFabId;
    }

    [Serializable]
    public class UpdateUserDataResult : PlayFabResultCommon
    {
        public uint DataVersion;
    }
    [Serializable]
    public class UpdateUserInternalDataRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public List<string> KeysToRemove;
        public string PlayFabId;
    }
    [Serializable]
    public class UpdateUserInventoryItemDataRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> Data;
        public string ItemInstanceId;
        public List<string> KeysToRemove;
        public string PlayFabId;
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

    public enum UserFamilyType
    {
        None,
        Xbox,
        Steam
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
    public class WriteEventResponse : PlayFabResultCommon
    {
        public string EventId;
    }
    [Serializable]
    public class WriteServerCharacterEventRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Body;
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string EventName;
        public string PlayFabId;
        public DateTime? Timestamp;
    }
    [Serializable]
    public class WriteServerPlayerEventRequest : PlayFabRequestCommon
    {
        public Dictionary<string,object> Body;
        public Dictionary<string,string> CustomTags;
        public string EventName;
        public string PlayFabId;
        public DateTime? Timestamp;
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
