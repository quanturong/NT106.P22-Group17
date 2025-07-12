#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.CloudScriptModels
{
    [Serializable]
    public class AdCampaignAttributionModel : PlayFabBaseModel
    {
        public DateTime AttributedAt;
        public string CampaignId;
        public string Platform;
    }

    public enum CloudScriptRevisionOption
    {
        Live,
        Latest,
        Specific
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

    public enum EmailVerificationStatus
    {
        Unverified,
        Pending,
        Confirmed
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
    public class EventHubFunctionModel : PlayFabBaseModel
    {
        public string ConnectionString;
        public string EventHubName;
        public string FunctionName;
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
    public class ExecuteEntityCloudScriptRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string FunctionName;
        public object FunctionParameter;
        public bool? GeneratePlayStreamEvent;
        public CloudScriptRevisionOption? RevisionSelection;
        public int? SpecificRevision;
    }
    [Serializable]
    public class ExecuteFunctionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string FunctionName;
        public object FunctionParameter;
        public bool? GeneratePlayStreamEvent;
    }

    [Serializable]
    public class ExecuteFunctionResult : PlayFabResultCommon
    {
        public FunctionExecutionError Error;
        public int ExecutionTimeMilliseconds;
        public string FunctionName;
        public object FunctionResult;
        public bool? FunctionResultTooLarge;
    }

    [Serializable]
    public class FunctionExecutionError : PlayFabBaseModel
    {
        public string Error;
        public string Message;
        public string StackTrace;
    }

    [Serializable]
    public class FunctionModel : PlayFabBaseModel
    {
        public string FunctionAddress;
        public string FunctionName;
        public string TriggerType;
    }

    [Serializable]
    public class GetFunctionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
    }

    [Serializable]
    public class GetFunctionResult : PlayFabResultCommon
    {
        public string ConnectionString;
        public string FunctionUrl;
        public string QueueName;
        public string TriggerType;
    }

    [Serializable]
    public class HttpFunctionModel : PlayFabBaseModel
    {
        public string FunctionName;
        public string FunctionUrl;
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
    public class ListEventHubFunctionsResult : PlayFabResultCommon
    {
        public List<EventHubFunctionModel> Functions;
    }
    [Serializable]
    public class ListFunctionsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListFunctionsResult : PlayFabResultCommon
    {
        public List<FunctionModel> Functions;
    }

    [Serializable]
    public class ListHttpFunctionsResult : PlayFabResultCommon
    {
        public List<HttpFunctionModel> Functions;
    }

    [Serializable]
    public class ListQueuedFunctionsResult : PlayFabResultCommon
    {
        public List<QueuedFunctionModel> Functions;
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
    public class NameIdentifier : PlayFabBaseModel
    {
        public string Id;
        public string Name;
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
    public class PlayStreamEventEnvelopeModel : PlayFabBaseModel
    {
        public string EntityId;
        public string EntityType;
        public string EventData;
        public string EventName;
        public string EventNamespace;
        public string EventSettings;
    }

    [Serializable]
    public class PostFunctionResultForEntityTriggeredActionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public ExecuteFunctionResult FunctionResult;
    }

    [Serializable]
    public class PostFunctionResultForFunctionExecutionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public ExecuteFunctionResult FunctionResult;
    }

    [Serializable]
    public class PostFunctionResultForPlayerTriggeredActionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExecuteFunctionResult FunctionResult;
        public PlayerProfileModel PlayerProfile;
        public PlayStreamEventEnvelopeModel PlayStreamEventEnvelope;
    }

    [Serializable]
    public class PostFunctionResultForScheduledTaskRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExecuteFunctionResult FunctionResult;
        public NameIdentifier ScheduledTaskId;
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
    public class QueuedFunctionModel : PlayFabBaseModel
    {
        public string ConnectionString;
        public string FunctionName;
        public string QueueName;
    }
    [Serializable]
    public class RegisterEventHubFunctionRequest : PlayFabRequestCommon
    {
        public string ConnectionString;
        public Dictionary<string,string> CustomTags;
        public string EventHubName;
        public string FunctionName;
    }

    [Serializable]
    public class RegisterHttpFunctionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
        public string FunctionUrl;
    }
    [Serializable]
    public class RegisterQueuedFunctionRequest : PlayFabRequestCommon
    {
        public string ConnectionString;
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
        public string QueueName;
    }

    [Serializable]
    public class ScriptExecutionError : PlayFabBaseModel
    {
        public string Error;
        public string Message;
        public string StackTrace;
    }

    [Serializable]
    public class StatisticModel : PlayFabBaseModel
    {
        public string Name;
        public int Value;
        public int Version;
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
    public class TagModel : PlayFabBaseModel
    {
        public string TagValue;
    }

    public enum TriggerType
    {
        HTTP,
        Queue,
        EventHub
    }

    [Serializable]
    public class UnregisterFunctionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FunctionName;
    }

    [Serializable]
    public class ValueToDateModel : PlayFabBaseModel
    {
        public string Currency;
        public uint TotalValue;
        public string TotalValueAsDecimal;
    }
}
#endif
