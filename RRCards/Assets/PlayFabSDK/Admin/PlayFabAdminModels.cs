#if ENABLE_PLAYFABADMIN_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.AdminModels
{
    [Serializable]
    public class AbortTaskInstanceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string TaskInstanceId;
    }
    [Serializable]
    public class Action : PlayFabBaseModel
    {
        public AddInventoryItemV2Content AddInventoryItemV2Content;
        public BanPlayerContent BanPlayerContent;
        public DeleteInventoryItemV2Content DeleteInventoryItemV2Content;
        public DeletePlayerContent DeletePlayerContent;
        public ExecuteCloudScriptContent ExecuteCloudScriptContent;
        public ExecuteFunctionContent ExecuteFunctionContent;
        public GrantItemContent GrantItemContent;
        public GrantVirtualCurrencyContent GrantVirtualCurrencyContent;
        public IncrementPlayerStatisticContent IncrementPlayerStatisticContent;
        public PushNotificationContent PushNotificationContent;
        public SendEmailContent SendEmailContent;
        public SubtractInventoryItemV2Content SubtractInventoryItemV2Content;
    }

    [Serializable]
    public class ActionsOnPlayersInSegmentTaskParameter : PlayFabBaseModel
    {
        public List<Action> Actions;
        public string SegmentId;
    }

    [Serializable]
    public class ActionsOnPlayersInSegmentTaskSummary : PlayFabBaseModel
    {
        public DateTime? CompletedAt;
        public string ErrorMessage;
        public bool? ErrorWasFatal;
        public double? EstimatedSecondsRemaining;
        public double? PercentComplete;
        public string ScheduledByUserId;
        public DateTime StartedAt;
        public TaskInstanceStatus? Status;
        public NameIdentifier TaskIdentifier;
        public string TaskInstanceId;
        public int? TotalPlayersInSegment;
        public int? TotalPlayersProcessed;
    }

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
    public class AdCampaignSegmentFilter : PlayFabBaseModel
    {
        public string CampaignId;
        public string CampaignSource;
        public SegmentFilterComparison? Comparison;
    }

    [Serializable]
    public class AddInventoryItemsV2SegmentAction : PlayFabBaseModel
    {
        public int? Amount;
        public string CollectionId;
        public int? DurationInSeconds;
        public string ItemId;
        public string StackId;
    }

    [Serializable]
    public class AddInventoryItemV2Content : PlayFabBaseModel
    {
        public int? Amount;
        public string CollectionId;
        public int? DurationInSeconds;
        public string ItemId;
        public string StackId;
    }

    [Serializable]
    public class AddLocalizedNewsRequest : PlayFabRequestCommon
    {
        public string Body;
        public Dictionary<string,string> CustomTags;
        public string Language;
        public string NewsId;
        public string Title;
    }

    [Serializable]
    public class AddLocalizedNewsResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class AddNewsRequest : PlayFabRequestCommon
    {
        public string Body;
        public Dictionary<string,string> CustomTags;
        public DateTime? Timestamp;
        public string Title;
    }

    [Serializable]
    public class AddNewsResult : PlayFabResultCommon
    {
        public string NewsId;
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
    public class AddUserVirtualCurrencyRequest : PlayFabRequestCommon
    {
        public int Amount;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
        public string VirtualCurrency;
    }
    [Serializable]
    public class AddVirtualCurrencyTypesRequest : PlayFabRequestCommon
    {
        public List<VirtualCurrencyData> VirtualCurrencies;
    }

    [Serializable]
    public class AllPlayersSegmentFilter : PlayFabBaseModel
    {
    }

    [Serializable]
    public class ApiCondition : PlayFabBaseModel
    {
        public Conditionals? HasSignatureOrEncryption;
    }

    public enum AuthTokenType
    {
        Email
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
    public class BanPlayerContent : PlayFabBaseModel
    {
        public int? BanDurationHours;
        public string BanReason;
    }

    [Serializable]
    public class BanPlayerSegmentAction : PlayFabBaseModel
    {
        public uint? BanHours;
        public string ReasonForBan;
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
    public class BlankResult : PlayFabResultCommon
    {
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
    public class CheckLimitedEditionItemAvailabilityRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public string ItemId;
    }

    [Serializable]
    public class CheckLimitedEditionItemAvailabilityResult : PlayFabResultCommon
    {
        public int Amount;
    }

    [Serializable]
    public class ChurnPredictionSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public ChurnRiskLevel? RiskLevel;
    }

    public enum ChurnRiskLevel
    {
        NoData,
        LowRisk,
        MediumRisk,
        HighRisk
    }

    [Serializable]
    public class CloudScriptFile : PlayFabBaseModel
    {
        public string FileContents;
        public string Filename;
    }

    [Serializable]
    public class CloudScriptTaskParameter : PlayFabBaseModel
    {
        public object Argument;
        public string FunctionName;
    }

    [Serializable]
    public class CloudScriptTaskSummary : PlayFabBaseModel
    {
        public DateTime? CompletedAt;
        public double? EstimatedSecondsRemaining;
        public double? PercentComplete;
        public ExecuteCloudScriptResult Result;
        public string ScheduledByUserId;
        public DateTime StartedAt;
        public TaskInstanceStatus? Status;
        public NameIdentifier TaskIdentifier;
        public string TaskInstanceId;
    }

    [Serializable]
    public class CloudScriptVersionStatus : PlayFabBaseModel
    {
        public int LatestRevision;
        public int PublishedRevision;
        public int Version;
    }

    public enum Conditionals
    {
        Any,
        True,
        False
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

    [Serializable]
    public class ContentInfo : PlayFabBaseModel
    {
        public string Key;
        public DateTime LastModified;
        public double Size;
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
    public class CreateActionsOnPlayerSegmentTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public bool IsActive;
        public string Name;
        public ActionsOnPlayersInSegmentTaskParameter Parameter;
        public string Schedule;
    }
    [Serializable]
    public class CreateCloudScriptTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public bool IsActive;
        public string Name;
        public CloudScriptTaskParameter Parameter;
        public string Schedule;
    }
    [Serializable]
    public class CreateInsightsScheduledScalingTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public bool IsActive;
        public string Name;
        public InsightsScalingTaskParameter Parameter;
        public string Schedule;
    }

    [Serializable]
    public class CreateOpenIdConnectionRequest : PlayFabRequestCommon
    {
        public string ClientId;
        public string ClientSecret;
        public string ConnectionId;
        public bool? IgnoreNonce;
        public string IssuerDiscoveryUrl;
        public OpenIdIssuerInformation IssuerInformation;
        public string IssuerOverride;
    }
    [Serializable]
    public class CreatePlayerSharedSecretRequest : PlayFabRequestCommon
    {
        public string FriendlyName;
    }

    [Serializable]
    public class CreatePlayerSharedSecretResult : PlayFabResultCommon
    {
        public string SecretKey;
    }
    [Serializable]
    public class CreatePlayerStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public StatisticAggregationMethod? AggregationMethod;
        public Dictionary<string,string> CustomTags;
        public string StatisticName;
        public StatisticResetIntervalOption? VersionChangeInterval;
    }

    [Serializable]
    public class CreatePlayerStatisticDefinitionResult : PlayFabResultCommon
    {
        public PlayerStatisticDefinition Statistic;
    }
    [Serializable]
    public class CreateSegmentRequest : PlayFabRequestCommon
    {
        public SegmentModel SegmentModel;
    }

    [Serializable]
    public class CreateSegmentResponse : PlayFabResultCommon
    {
        public string ErrorMessage;
        public string SegmentId;
    }

    [Serializable]
    public class CreateTaskResult : PlayFabResultCommon
    {
        public string TaskId;
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
    public class CustomPropertyBooleanSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string PropertyName;
        public bool PropertyValue;
    }

    [Serializable]
    public class CustomPropertyDateTimeSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string PropertyName;
        public DateTime PropertyValue;
    }

    [Serializable]
    public class CustomPropertyDetails : PlayFabBaseModel
    {
        public string Name;
        public object Value;
    }

    [Serializable]
    public class CustomPropertyNumericSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string PropertyName;
        public double PropertyValue;
    }

    [Serializable]
    public class CustomPropertyStringSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string PropertyName;
        public string PropertyValue;
    }

    [Serializable]
    public class DeleteContentRequest : PlayFabRequestCommon
    {
        public string Key;
    }

    [Serializable]
    public class DeletedPropertyDetails : PlayFabBaseModel
    {
        public string Name;
        public bool WasDeleted;
    }

    [Serializable]
    public class DeleteInventoryItemsV2SegmentAction : PlayFabBaseModel
    {
        public string CollectionId;
        public string ItemId;
        public string StackId;
    }

    [Serializable]
    public class DeleteInventoryItemV2Content : PlayFabBaseModel
    {
        public string CollectionId;
        public string ItemId;
        public string StackId;
    }
    [Serializable]
    public class DeleteMasterPlayerAccountRequest : PlayFabRequestCommon
    {
        public string MetaData;
        public string PlayFabId;
    }

    [Serializable]
    public class DeleteMasterPlayerAccountResult : PlayFabResultCommon
    {
        public string JobReceiptId;
        public List<string> TitleIds;
    }
    [Serializable]
    public class DeleteMasterPlayerEventDataRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class DeleteMasterPlayerEventDataResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class DeleteMembershipSubscriptionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string MembershipId;
        public string PlayFabId;
        public string SubscriptionId;
    }

    [Serializable]
    public class DeleteMembershipSubscriptionResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteOpenIdConnectionRequest : PlayFabRequestCommon
    {
        public string ConnectionId;
    }

    [Serializable]
    public class DeletePlayerContent : PlayFabBaseModel
    {
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
    public class DeletePlayerSegmentAction : PlayFabBaseModel
    {
    }
    [Serializable]
    public class DeletePlayerSharedSecretRequest : PlayFabRequestCommon
    {
        public string SecretKey;
    }

    [Serializable]
    public class DeletePlayerSharedSecretResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeletePlayerStatisticSegmentAction : PlayFabBaseModel
    {
        public string StatisticName;
    }
    [Serializable]
    public class DeleteSegmentRequest : PlayFabRequestCommon
    {
        public string SegmentId;
    }

    [Serializable]
    public class DeleteSegmentsResponse : PlayFabResultCommon
    {
        public string ErrorMessage;
    }
    [Serializable]
    public class DeleteStoreRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public string StoreId;
    }

    [Serializable]
    public class DeleteStoreResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class DeleteTaskRequest : PlayFabRequestCommon
    {
        public NameIdentifier Identifier;
    }
    [Serializable]
    public class DeleteTitleDataOverrideRequest : PlayFabRequestCommon
    {
        public string OverrideLabel;
    }

    [Serializable]
    public class DeleteTitleDataOverrideResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class DeleteTitleRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class DeleteTitleResult : PlayFabResultCommon
    {
    }

    public enum EffectType
    {
        Allow,
        Deny
    }

    [Serializable]
    public class EmailNotificationSegmentAction : PlayFabBaseModel
    {
        public string EmailTemplateId;
        public string EmailTemplateName;
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
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }

    [Serializable]
    public class ExecuteAzureFunctionSegmentAction : PlayFabBaseModel
    {
        public string AzureFunction;
        public object FunctionParameter;
        public bool GenerateFunctionExecutedEvents;
    }

    [Serializable]
    public class ExecuteCloudScriptContent : PlayFabBaseModel
    {
        public string CloudScriptMethodArguments;
        public string CloudScriptMethodName;
        public bool PublishResultsToPlayStream;
    }

    [Serializable]
    public class ExecuteCloudScriptResult : PlayFabBaseModel
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
    public class ExecuteCloudScriptSegmentAction : PlayFabBaseModel
    {
        public string CloudScriptFunction;
        public bool CloudScriptPublishResultsToPlayStream;
        public object FunctionParameter;
        public string FunctionParameterJson;
    }

    [Serializable]
    public class ExecuteFunctionContent : PlayFabBaseModel
    {
        public string CloudScriptFunctionArguments;
        public string CloudScriptFunctionName;
        public bool PublishResultsToPlayStream;
    }
    [Serializable]
    public class ExportMasterPlayerDataRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class ExportMasterPlayerDataResult : PlayFabResultCommon
    {
        public string JobReceiptId;
    }
    [Serializable]
    public class ExportPlayersInSegmentRequest : PlayFabRequestCommon
    {
        public string SegmentId;
    }

    [Serializable]
    public class ExportPlayersInSegmentResult : PlayFabResultCommon
    {
        public string ExportId;
        public string SegmentId;
    }

    [Serializable]
    public class FirstLoginDateSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public DateTime LogInDate;
    }

    [Serializable]
    public class FirstLoginTimespanSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public double DurationInMinutes;
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
    public class GetActionsOnPlayersInSegmentTaskInstanceResult : PlayFabResultCommon
    {
        public ActionsOnPlayersInSegmentTaskParameter Parameter;
        public ActionsOnPlayersInSegmentTaskSummary Summary;
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
    public class GetCloudScriptRevisionRequest : PlayFabRequestCommon
    {
        public int? Revision;
        public int? Version;
    }

    [Serializable]
    public class GetCloudScriptRevisionResult : PlayFabResultCommon
    {
        public DateTime CreatedAt;
        public List<CloudScriptFile> Files;
        public bool IsPublished;
        public int Revision;
        public int Version;
    }

    [Serializable]
    public class GetCloudScriptTaskInstanceResult : PlayFabResultCommon
    {
        public CloudScriptTaskParameter Parameter;
        public CloudScriptTaskSummary Summary;
    }

    [Serializable]
    public class GetCloudScriptVersionsRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class GetCloudScriptVersionsResult : PlayFabResultCommon
    {
        public List<CloudScriptVersionStatus> Versions;
    }

    [Serializable]
    public class GetContentListRequest : PlayFabRequestCommon
    {
        public string Prefix;
    }

    [Serializable]
    public class GetContentListResult : PlayFabResultCommon
    {
        public List<ContentInfo> Contents;
        public int ItemCount;
        public uint TotalSize;
    }

    [Serializable]
    public class GetContentUploadUrlRequest : PlayFabRequestCommon
    {
        public string ContentType;
        public string Key;
    }

    [Serializable]
    public class GetContentUploadUrlResult : PlayFabResultCommon
    {
        public string URL;
    }
    [Serializable]
    public class GetDataReportRequest : PlayFabRequestCommon
    {
        public int Day;
        public int Month;
        public string ReportName;
        public int Year;
    }

    [Serializable]
    public class GetDataReportResult : PlayFabResultCommon
    {
        public string DownloadUrl;
    }
    [Serializable]
    public class GetPlayedTitleListRequest : PlayFabRequestCommon
    {
        public string PlayFabId;
    }

    [Serializable]
    public class GetPlayedTitleListResult : PlayFabResultCommon
    {
        public List<string> TitleIds;
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
    public class GetPlayerIdFromAuthTokenRequest : PlayFabRequestCommon
    {
        public string Token;
        public AuthTokenType TokenType;
    }

    [Serializable]
    public class GetPlayerIdFromAuthTokenResult : PlayFabResultCommon
    {
        public string PlayFabId;
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
    public class GetPlayerSharedSecretsRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class GetPlayerSharedSecretsResult : PlayFabResultCommon
    {
        public List<SharedSecret> SharedSecrets;
    }
    [Serializable]
    public class GetPlayersInSegmentExportRequest : PlayFabRequestCommon
    {
        public string ExportId;
    }

    [Serializable]
    public class GetPlayersInSegmentExportResponse : PlayFabResultCommon
    {
        public string IndexUrl;
        public string State;
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
    public class GetPlayerStatisticDefinitionsRequest : PlayFabRequestCommon
    {
    }
    [Serializable]
    public class GetPlayerStatisticDefinitionsResult : PlayFabResultCommon
    {
        public List<PlayerStatisticDefinition> Statistics;
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
    public class GetPolicyRequest : PlayFabRequestCommon
    {
        public string PolicyName;
    }

    [Serializable]
    public class GetPolicyResponse : PlayFabResultCommon
    {
        public string PolicyName;
        public int PolicyVersion;
        public List<PermissionStatement> Statements;
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
    public class GetSegmentsRequest : PlayFabRequestCommon
    {
        public List<string> SegmentIds;
    }

    [Serializable]
    public class GetSegmentsResponse : PlayFabResultCommon
    {
        public string ErrorMessage;
        public List<SegmentModel> Segments;
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
    public class GetTaskInstanceRequest : PlayFabRequestCommon
    {
        public string TaskInstanceId;
    }
    [Serializable]
    public class GetTaskInstancesRequest : PlayFabRequestCommon
    {
        public DateTime? StartedAtRangeFrom;
        public DateTime? StartedAtRangeTo;
        public TaskInstanceStatus? StatusFilter;
        public NameIdentifier TaskIdentifier;
    }

    [Serializable]
    public class GetTaskInstancesResult : PlayFabResultCommon
    {
        public List<TaskInstanceBasicSummary> Summaries;
    }

    [Serializable]
    public class GetTasksRequest : PlayFabRequestCommon
    {
        public NameIdentifier Identifier;
    }

    [Serializable]
    public class GetTasksResult : PlayFabResultCommon
    {
        public List<ScheduledTask> Tasks;
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
    public class GrantItemContent : PlayFabBaseModel
    {
        public string CatalogVersion;
        public string ItemId;
        public int ItemQuantity;
    }

    [Serializable]
    public class GrantItemSegmentAction : PlayFabBaseModel
    {
        public string CatelogId;
        public string ItemId;
        public uint Quantity;
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
    public class GrantVirtualCurrencyContent : PlayFabBaseModel
    {
        public int CurrencyAmount;
        public string CurrencyCode;
    }

    [Serializable]
    public class GrantVirtualCurrencySegmentAction : PlayFabBaseModel
    {
        public int Amount;
        public string CurrencyCode;
    }
    [Serializable]
    public class IncrementLimitedEditionItemAvailabilityRequest : PlayFabRequestCommon
    {
        public int Amount;
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public string ItemId;
    }

    [Serializable]
    public class IncrementLimitedEditionItemAvailabilityResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class IncrementPlayerStatisticContent : PlayFabBaseModel
    {
        public int StatisticChangeBy;
        public string StatisticName;
    }

    [Serializable]
    public class IncrementPlayerStatisticSegmentAction : PlayFabBaseModel
    {
        public int IncrementValue;
        public string StatisticName;
    }
    [Serializable]
    public class IncrementPlayerStatisticVersionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string StatisticName;
    }

    [Serializable]
    public class IncrementPlayerStatisticVersionResult : PlayFabResultCommon
    {
        public PlayerStatisticVersion StatisticVersion;
    }

    [Serializable]
    public class InsightsScalingTaskParameter : PlayFabBaseModel
    {
        public int Level;
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
    public class LastLoginDateSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public DateTime LogInDate;
    }

    [Serializable]
    public class LastLoginTimespanSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public double DurationInMinutes;
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
    public class LinkedUserAccountHasEmailSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public SegmentLoginIdentityProvider? LoginProvider;
    }

    [Serializable]
    public class LinkedUserAccountSegmentFilter : PlayFabBaseModel
    {
        public SegmentLoginIdentityProvider? LoginProvider;
    }

    [Serializable]
    public class ListOpenIdConnectionRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class ListOpenIdConnectionResponse : PlayFabResultCommon
    {
        public List<OpenIdConnection> Connections;
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
    public class ListVirtualCurrencyTypesRequest : PlayFabRequestCommon
    {
    }

    [Serializable]
    public class ListVirtualCurrencyTypesResult : PlayFabResultCommon
    {
        public List<VirtualCurrencyData> VirtualCurrencies;
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

    [Serializable]
    public class LocationSegmentFilter : PlayFabBaseModel
    {
        public SegmentCountryCode? CountryCode;
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
    public class LogStatement : PlayFabBaseModel
    {
        public object Data;
        public string Level;
        public string Message;
    }
    [Serializable]
    public class LookupUserAccountInfoRequest : PlayFabRequestCommon
    {
        public string Email;
        public string PlayFabId;
        public string TitleDisplayName;
        public string Username;
    }

    [Serializable]
    public class LookupUserAccountInfoResult : PlayFabResultCommon
    {
        public UserAccountInfo UserInfo;
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
    public class OpenIdConnection : PlayFabBaseModel
    {
        public string ClientId;
        public string ClientSecret;
        public string ConnectionId;
        public bool DiscoverConfiguration;
        public bool? IgnoreNonce;
        public OpenIdIssuerInformation IssuerInformation;
        public string IssuerOverride;
    }

    [Serializable]
    public class OpenIdIssuerInformation : PlayFabBaseModel
    {
        public string AuthorizationUrl;
        public string Issuer;
        public object JsonWebKeySet;
        public string TokenUrl;
    }

    [Serializable]
    public class PermissionStatement : PlayFabBaseModel
    {
        public string Action;
        public ApiCondition ApiConditions;
        public string Comment;
        public EffectType Effect;
        public string Principal;
        public string Resource;
    }

    [Serializable]
    public class PlayerChurnPredictionSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public ChurnRiskLevel? RiskLevel;
    }

    [Serializable]
    public class PlayerChurnPredictionTimeSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public double DurationInDays;
    }

    [Serializable]
    public class PlayerChurnPreviousPredictionSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public ChurnRiskLevel? RiskLevel;
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
    public class PlayerStatisticDefinition : PlayFabBaseModel
    {
        public StatisticAggregationMethod? AggregationMethod;
        public uint CurrentVersion;
        public string StatisticName;
        public StatisticResetIntervalOption? VersionChangeInterval;
    }

    [Serializable]
    public class PlayerStatisticVersion : PlayFabBaseModel
    {
        public DateTime ActivationTime;
        public string ArchiveDownloadUrl;
        public DateTime? DeactivationTime;
        public DateTime? ScheduledActivationTime;
        public DateTime? ScheduledDeactivationTime;
        public string StatisticName;
        public StatisticVersionStatus? Status;
        public uint Version;
    }

    [Serializable]
    public class PushNotificationContent : PlayFabBaseModel
    {
        public string Message;
        public string PushNotificationTemplateId;
        public string Subject;
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
    public class PushNotificationSegmentAction : PlayFabBaseModel
    {
        public string PushNotificationTemplateId;
    }

    [Serializable]
    public class PushNotificationSegmentFilter : PlayFabBaseModel
    {
        public SegmentPushNotificationDevicePlatform? PushNotificationDevicePlatform;
    }

    public enum PushSetupPlatform
    {
        GCM,
        APNS,
        APNS_SANDBOX
    }

    [Serializable]
    public class RandomResultTable : PlayFabBaseModel
    {
        public List<ResultTableNode> Nodes;
        public string TableId;
    }

    [Serializable]
    public class RandomResultTableListing : PlayFabBaseModel
    {
        public string CatalogVersion;
        public List<ResultTableNode> Nodes;
        public string TableId;
    }

    [Serializable]
    public class RefundPurchaseRequest : PlayFabRequestCommon
    {
        public string OrderId;
        public string PlayFabId;
        public string Reason;
    }

    [Serializable]
    public class RefundPurchaseResponse : PlayFabResultCommon
    {
        public string PurchaseStatus;
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
    public class RemoveVirtualCurrencyTypesRequest : PlayFabRequestCommon
    {
        public List<VirtualCurrencyData> VirtualCurrencies;
    }
    [Serializable]
    public class ResetCharacterStatisticsRequest : PlayFabRequestCommon
    {
        public string CharacterId;
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class ResetCharacterStatisticsResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class ResetPasswordRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Password;
        public string Token;
    }

    [Serializable]
    public class ResetPasswordResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class ResetUserStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string PlayFabId;
    }

    [Serializable]
    public class ResetUserStatisticsResult : PlayFabResultCommon
    {
    }

    public enum ResolutionOutcome
    {
        Revoke,
        Reinstate,
        Manual
    }

    [Serializable]
    public class ResolvePurchaseDisputeRequest : PlayFabRequestCommon
    {
        public string OrderId;
        public ResolutionOutcome Outcome;
        public string PlayFabId;
        public string Reason;
    }

    [Serializable]
    public class ResolvePurchaseDisputeResponse : PlayFabResultCommon
    {
        public string PurchaseStatus;
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
    public class RunTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public NameIdentifier Identifier;
    }

    [Serializable]
    public class RunTaskResult : PlayFabResultCommon
    {
        public string TaskInstanceId;
    }

    [Serializable]
    public class ScheduledTask : PlayFabBaseModel
    {
        public string Description;
        public bool IsActive;
        public DateTime? LastRunTime;
        public string Name;
        public DateTime? NextRunTime;
        public object Parameter;
        public string Schedule;
        public string TaskId;
        public ScheduledTaskType? Type;
    }

    public enum ScheduledTaskType
    {
        CloudScript,
        ActionsOnPlayerSegment,
        CloudScriptAzureFunctions,
        InsightsScheduledScaling
    }

    [Serializable]
    public class ScriptExecutionError : PlayFabBaseModel
    {
        public string Error;
        public string Message;
        public string StackTrace;
    }

    [Serializable]
    public class SegmentAndDefinition : PlayFabBaseModel
    {
        public AdCampaignSegmentFilter AdCampaignFilter;
        public AllPlayersSegmentFilter AllPlayersFilter;
        public ChurnPredictionSegmentFilter ChurnPredictionFilter;
        public CustomPropertyBooleanSegmentFilter CustomPropertyBooleanFilter;
        public CustomPropertyDateTimeSegmentFilter CustomPropertyDateTimeFilter;
        public CustomPropertyNumericSegmentFilter CustomPropertyNumericFilter;
        public CustomPropertyStringSegmentFilter CustomPropertyStringFilter;
        public FirstLoginDateSegmentFilter FirstLoginDateFilter;
        public FirstLoginTimespanSegmentFilter FirstLoginFilter;
        public LastLoginDateSegmentFilter LastLoginDateFilter;
        public LastLoginTimespanSegmentFilter LastLoginFilter;
        public LinkedUserAccountSegmentFilter LinkedUserAccountFilter;
        public LinkedUserAccountHasEmailSegmentFilter LinkedUserAccountHasEmailFilter;
        public LocationSegmentFilter LocationFilter;
        public PlayerChurnPredictionSegmentFilter PlayerChurnPredictionFilter;
        public PlayerChurnPredictionTimeSegmentFilter PlayerChurnPredictionTimeFilter;
        public PlayerChurnPreviousPredictionSegmentFilter PlayerChurnPreviousPredictionFilter;
        public PushNotificationSegmentFilter PushNotificationFilter;
        public StatisticSegmentFilter StatisticFilter;
        public TagSegmentFilter TagFilter;
        public TotalValueToDateInUSDSegmentFilter TotalValueToDateInUSDFilter;
        public UserOriginationSegmentFilter UserOriginationFilter;
        public ValueToDateSegmentFilter ValueToDateFilter;
        public VirtualCurrencyBalanceSegmentFilter VirtualCurrencyBalanceFilter;
    }

    public enum SegmentCountryCode
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
        ZW
    }

    public enum SegmentCurrency
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

    public enum SegmentFilterComparison
    {
        GreaterThan,
        LessThan,
        EqualTo,
        NotEqualTo,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Exists,
        Contains,
        NotContains
    }

    public enum SegmentLoginIdentityProvider
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
        GooglePlayGames
    }

    [Serializable]
    public class SegmentModel : PlayFabBaseModel
    {
        public string Description;
        public List<SegmentTrigger> EnteredSegmentActions;
        public DateTime LastUpdateTime;
        public List<SegmentTrigger> LeftSegmentActions;
        public string Name;
        public string SegmentId;
        public List<SegmentOrDefinition> SegmentOrDefinitions;
    }

    [Serializable]
    public class SegmentOrDefinition : PlayFabBaseModel
    {
        public List<SegmentAndDefinition> SegmentAndDefinitions;
    }

    public enum SegmentPushNotificationDevicePlatform
    {
        ApplePushNotificationService,
        GoogleCloudMessaging
    }

    [Serializable]
    public class SegmentTrigger : PlayFabBaseModel
    {
        public AddInventoryItemsV2SegmentAction AddInventoryItemsV2Action;
        public BanPlayerSegmentAction BanPlayerAction;
        public DeleteInventoryItemsV2SegmentAction DeleteInventoryItemsV2Action;
        public DeletePlayerSegmentAction DeletePlayerAction;
        public DeletePlayerStatisticSegmentAction DeletePlayerStatisticAction;
        public EmailNotificationSegmentAction EmailNotificationAction;
        public ExecuteAzureFunctionSegmentAction ExecuteAzureFunctionAction;
        public ExecuteCloudScriptSegmentAction ExecuteCloudScriptAction;
        public GrantItemSegmentAction GrantItemAction;
        public GrantVirtualCurrencySegmentAction GrantVirtualCurrencyAction;
        public IncrementPlayerStatisticSegmentAction IncrementPlayerStatisticAction;
        public PushNotificationSegmentAction PushNotificationAction;
        public SubtractInventoryItemsV2SegmentAction SubtractInventoryItemsV2Action;
    }
    [Serializable]
    public class SendAccountRecoveryEmailRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Email;
        public string EmailTemplateId;
    }

    [Serializable]
    public class SendAccountRecoveryEmailResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class SendEmailContent : PlayFabBaseModel
    {
        public string EmailTemplateId;
    }
    [Serializable]
    public class SetMembershipOverrideRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public DateTime ExpirationTime;
        public string MembershipId;
        public string PlayFabId;
    }

    [Serializable]
    public class SetMembershipOverrideResult : PlayFabResultCommon
    {
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
    public class SetPublishedRevisionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int Revision;
        public int Version;
    }

    [Serializable]
    public class SetPublishedRevisionResult : PlayFabResultCommon
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
    public class SetTitleDataAndOverridesRequest : PlayFabRequestCommon
    {
        public List<TitleDataKeyValue> KeyValues;
        public string OverrideLabel;
    }

    [Serializable]
    public class SetTitleDataAndOverridesResult : PlayFabResultCommon
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
    public class SetupPushNotificationRequest : PlayFabRequestCommon
    {
        public string Credential;
        public string Key;
        public string Name;
        public bool OverwriteOldARN;
        public PushSetupPlatform Platform;
    }

    [Serializable]
    public class SetupPushNotificationResult : PlayFabResultCommon
    {
        public string ARN;
    }

    [Serializable]
    public class SharedSecret : PlayFabBaseModel
    {
        public bool Disabled;
        public string FriendlyName;
        public string SecretKey;
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

    public enum StatisticAggregationMethod
    {
        Last,
        Min,
        Max,
        Sum
    }

    [Serializable]
    public class StatisticModel : PlayFabBaseModel
    {
        public string Name;
        public int Value;
        public int Version;
    }

    public enum StatisticResetIntervalOption
    {
        Never,
        Hour,
        Day,
        Week,
        Month
    }

    [Serializable]
    public class StatisticSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string FilterValue;
        public string Name;
        public bool? UseCurrentVersion;
        public int? Version;
    }

    public enum StatisticVersionArchivalStatus
    {
        NotScheduled,
        Scheduled,
        Queued,
        InProgress,
        Complete
    }

    public enum StatisticVersionStatus
    {
        Active,
        SnapshotPending,
        Snapshot,
        ArchivalPending,
        Archived
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
    public class SubtractInventoryItemsV2SegmentAction : PlayFabBaseModel
    {
        public int? Amount;
        public string CollectionId;
        public int? DurationInSeconds;
        public string ItemId;
        public string StackId;
    }

    [Serializable]
    public class SubtractInventoryItemV2Content : PlayFabBaseModel
    {
        public int? Amount;
        public string CollectionId;
        public int? DurationInSeconds;
        public string ItemId;
        public string StackId;
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

    [Serializable]
    public class TagSegmentFilter : PlayFabBaseModel
    {
        public SegmentFilterComparison? Comparison;
        public string TagValue;
    }

    [Serializable]
    public class TaskInstanceBasicSummary : PlayFabBaseModel
    {
        public DateTime? CompletedAt;
        public string ErrorMessage;
        public double? EstimatedSecondsRemaining;
        public double? PercentComplete;
        public string ScheduledByUserId;
        public DateTime StartedAt;
        public TaskInstanceStatus? Status;
        public NameIdentifier TaskIdentifier;
        public string TaskInstanceId;
        public ScheduledTaskType? Type;
    }

    public enum TaskInstanceStatus
    {
        Succeeded,
        Starting,
        InProgress,
        Failed,
        Aborted,
        Stalled
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
    public class TitleDataKeyValue : PlayFabBaseModel
    {
        public string Key;
        public string Value;
    }

    [Serializable]
    public class TotalValueToDateInUSDSegmentFilter : PlayFabBaseModel
    {
        public string Amount;
        public SegmentFilterComparison? Comparison;
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
    public class UpdateCatalogItemsRequest : PlayFabRequestCommon
    {
        public List<CatalogItem> Catalog;
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public bool? SetAsDefaultCatalog;
    }

    [Serializable]
    public class UpdateCatalogItemsResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UpdateCloudScriptRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DeveloperPlayFabId;
        public List<CloudScriptFile> Files;
        public bool Publish;
    }

    [Serializable]
    public class UpdateCloudScriptResult : PlayFabResultCommon
    {
        public int Revision;
        public int Version;
    }

    [Serializable]
    public class UpdateOpenIdConnectionRequest : PlayFabRequestCommon
    {
        public string ClientId;
        public string ClientSecret;
        public string ConnectionId;
        public bool? IgnoreNonce;
        public string IssuerDiscoveryUrl;
        public OpenIdIssuerInformation IssuerInformation;
        public string IssuerOverride;
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
    public class UpdatePlayerSharedSecretRequest : PlayFabRequestCommon
    {
        public bool Disabled;
        public string FriendlyName;
        public string SecretKey;
    }

    [Serializable]
    public class UpdatePlayerSharedSecretResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UpdatePlayerStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public StatisticAggregationMethod? AggregationMethod;
        public string StatisticName;
        public StatisticResetIntervalOption? VersionChangeInterval;
    }

    [Serializable]
    public class UpdatePlayerStatisticDefinitionResult : PlayFabResultCommon
    {
        public PlayerStatisticDefinition Statistic;
    }
    [Serializable]
    public class UpdatePolicyRequest : PlayFabRequestCommon
    {
        public bool OverwritePolicy;
        public string PolicyName;
        public int PolicyVersion;
        public List<PermissionStatement> Statements;
    }

    [Serializable]
    public class UpdatePolicyResponse : PlayFabResultCommon
    {
        public string PolicyName;
        public List<PermissionStatement> Statements;
    }

    [Serializable]
    public class UpdateProperty : PlayFabBaseModel
    {
        public string Name;
        public object Value;
    }
    [Serializable]
    public class UpdateRandomResultTablesRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public List<RandomResultTable> Tables;
    }

    [Serializable]
    public class UpdateRandomResultTablesResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UpdateSegmentRequest : PlayFabRequestCommon
    {
        public SegmentModel SegmentModel;
    }

    [Serializable]
    public class UpdateSegmentResponse : PlayFabResultCommon
    {
        public string ErrorMessage;
        public string SegmentId;
    }
    [Serializable]
    public class UpdateStoreItemsRequest : PlayFabRequestCommon
    {
        public string CatalogVersion;
        public Dictionary<string,string> CustomTags;
        public StoreMarketingModel MarketingData;
        public List<StoreItem> Store;
        public string StoreId;
    }

    [Serializable]
    public class UpdateStoreItemsResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UpdateTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public NameIdentifier Identifier;
        public bool IsActive;
        public string Name;
        public object Parameter;
        public string Schedule;
        public ScheduledTaskType Type;
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
    public class UpdateUserTitleDisplayNameRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DisplayName;
        public string PlayFabId;
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
    public class UserOriginationSegmentFilter : PlayFabBaseModel
    {
        public SegmentLoginIdentityProvider? LoginProvider;
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
    public class ValueToDateSegmentFilter : PlayFabBaseModel
    {
        public string Amount;
        public SegmentFilterComparison? Comparison;
        public SegmentCurrency? Currency;
    }

    [Serializable]
    public class VirtualCurrencyBalanceSegmentFilter : PlayFabBaseModel
    {
        public int Amount;
        public SegmentFilterComparison? Comparison;
        public string CurrencyCode;
    }

    [Serializable]
    public class VirtualCurrencyData : PlayFabBaseModel
    {
        public string CurrencyCode;
        public string DisplayName;
        public int? InitialDeposit;
        public int? RechargeMax;
        public int? RechargeRate;
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
