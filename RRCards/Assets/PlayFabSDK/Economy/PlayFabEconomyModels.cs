#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.EconomyModels
{
    [Serializable]
    public class AddInventoryItemsOperation : PlayFabBaseModel
    {
        public int? Amount;
        public double? DurationInSeconds;
        public InventoryItemReference Item;
        public InitialValues NewStackValues;
    }
    [Serializable]
    public class AddInventoryItemsRequest : PlayFabRequestCommon
    {
        public int? Amount;
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public double? DurationInSeconds;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public InventoryItemReference Item;
        public InitialValues NewStackValues;
    }

    [Serializable]
    public class AddInventoryItemsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }

    [Serializable]
    public class AlternateId : PlayFabBaseModel
    {
        public string Type;
        public string Value;
    }

    [Serializable]
    public class CatalogAlternateId : PlayFabBaseModel
    {
        public string Type;
        public string Value;
    }

    [Serializable]
    public class CatalogConfig : PlayFabBaseModel
    {
        public List<EntityKey> AdminEntities;
        public CatalogSpecificConfig Catalog;
        public List<DeepLinkFormat> DeepLinkFormats;
        public List<DisplayPropertyIndexInfo> DisplayPropertyIndexInfos;
        public FileConfig File;
        public ImageConfig Image;
        public bool IsCatalogEnabled;
        public List<string> Platforms;
        public ReviewConfig Review;
        public List<EntityKey> ReviewerEntities;
        public UserGeneratedContentSpecificConfig UserGeneratedContent;
    }

    [Serializable]
    public class CatalogItem : PlayFabBaseModel
    {
        public List<CatalogAlternateId> AlternateIds;
        public List<Content> Contents;
        public string ContentType;
        public DateTime? CreationDate;
        public EntityKey CreatorEntity;
        public List<DeepLink> DeepLinks;
        public string DefaultStackId;
        public Dictionary<string,string> Description;
        public object DisplayProperties;
        public string DisplayVersion;
        public DateTime? EndDate;
        public string ETag;
        public string Id;
        public List<Image> Images;
        public bool? IsHidden;
        public List<CatalogItemReference> ItemReferences;
        public Dictionary<string,KeywordSet> Keywords;
        public DateTime? LastModifiedDate;
        public ModerationState Moderation;
        public List<string> Platforms;
        public CatalogPriceOptions PriceOptions;
        public Rating Rating;
        public RealMoneyPriceDetails RealMoneyPriceDetails;
        public DateTime? StartDate;
        public StoreDetails StoreDetails;
        public List<string> Tags;
        public Dictionary<string,string> Title;
        public string Type;
    }

    [Serializable]
    public class CatalogItemReference : PlayFabBaseModel
    {
        public int? Amount;
        public string Id;
        public CatalogPriceOptions PriceOptions;
    }

    [Serializable]
    public class CatalogPrice : PlayFabBaseModel
    {
        public List<CatalogPriceAmount> Amounts;
        public int? UnitAmount;
        public double? UnitDurationInSeconds;
    }

    [Serializable]
    public class CatalogPriceAmount : PlayFabBaseModel
    {
        public int Amount;
        public string ItemId;
    }

    [Serializable]
    public class CatalogPriceAmountOverride : PlayFabBaseModel
    {
        public int? FixedValue;
        public string ItemId;
        public double? Multiplier;
    }

    [Serializable]
    public class CatalogPriceOptions : PlayFabBaseModel
    {
        public List<CatalogPrice> Prices;
    }

    [Serializable]
    public class CatalogPriceOptionsOverride : PlayFabBaseModel
    {
        public List<CatalogPriceOverride> Prices;
    }

    [Serializable]
    public class CatalogPriceOverride : PlayFabBaseModel
    {
        public List<CatalogPriceAmountOverride> Amounts;
    }

    [Serializable]
    public class CatalogSpecificConfig : PlayFabBaseModel
    {
        public List<string> ContentTypes;
        public List<string> Tags;
    }

    [Serializable]
    public class CategoryRatingConfig : PlayFabBaseModel
    {
        public string Name;
    }

    public enum ConcernCategory
    {
        None,
        OffensiveContent,
        ChildExploitation,
        MalwareOrVirus,
        PrivacyConcerns,
        MisleadingApp,
        PoorPerformance,
        ReviewResponse,
        SpamAdvertising,
        Profanity
    }

    [Serializable]
    public class Content : PlayFabBaseModel
    {
        public string Id;
        public string MaxClientVersion;
        public string MinClientVersion;
        public List<string> Tags;
        public string Type;
        public string Url;
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
    public class CreateDraftItemRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public CatalogItem Item;
        public bool Publish;
    }

    [Serializable]
    public class CreateDraftItemResponse : PlayFabResultCommon
    {
        public CatalogItem Item;
    }
    [Serializable]
    public class CreateUploadUrlsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<UploadInfo> Files;
    }

    [Serializable]
    public class CreateUploadUrlsResponse : PlayFabResultCommon
    {
        public List<UploadUrlMetadata> UploadUrls;
    }

    [Serializable]
    public class DeepLink : PlayFabBaseModel
    {
        public string Platform;
        public string Url;
    }

    [Serializable]
    public class DeepLinkFormat : PlayFabBaseModel
    {
        public string Format;
        public string Platform;
    }

    [Serializable]
    public class DeleteEntityItemReviewsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class DeleteEntityItemReviewsResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class DeleteInventoryCollectionRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ETag;
    }

    [Serializable]
    public class DeleteInventoryCollectionResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DeleteInventoryItemsOperation : PlayFabBaseModel
    {
        public InventoryItemReference Item;
    }
    [Serializable]
    public class DeleteInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public InventoryItemReference Item;
    }

    [Serializable]
    public class DeleteInventoryItemsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }

    [Serializable]
    public class DeleteItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }

    [Serializable]
    public class DeleteItemResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class DisplayPropertyIndexInfo : PlayFabBaseModel
    {
        public string Name;
        public DisplayPropertyType? Type;
    }

    public enum DisplayPropertyType
    {
        None,
        QueryDateTime,
        QueryDouble,
        QueryString,
        SearchString
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }
    [Serializable]
    public class ExecuteInventoryOperationsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public List<InventoryOperation> Operations;
    }

    [Serializable]
    public class ExecuteInventoryOperationsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class ExecuteTransferOperationsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string GivingCollectionId;
        public EntityKey GivingEntity;
        public string GivingETag;
        public string IdempotencyId;
        public List<TransferInventoryItemsOperation> Operations;
        public string ReceivingCollectionId;
        public EntityKey ReceivingEntity;
    }

    [Serializable]
    public class ExecuteTransferOperationsResponse : PlayFabResultCommon
    {
        public string GivingETag;
        public List<string> GivingTransactionIds;
        public string IdempotencyId;
        public string OperationStatus;
        public string OperationToken;
        public string ReceivingETag;
        public List<string> ReceivingTransactionIds;
    }

    [Serializable]
    public class FileConfig : PlayFabBaseModel
    {
        public List<string> ContentTypes;
        public List<string> Tags;
    }

    [Serializable]
    public class FilterOptions : PlayFabBaseModel
    {
        public string Filter;
        public bool? IncludeAllItems;
    }

    [Serializable]
    public class GetCatalogConfigRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetCatalogConfigResponse : PlayFabResultCommon
    {
        public CatalogConfig Config;
    }

    [Serializable]
    public class GetDraftItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }

    [Serializable]
    public class GetDraftItemResponse : PlayFabResultCommon
    {
        public CatalogItem Item;
    }

    [Serializable]
    public class GetDraftItemsRequest : PlayFabRequestCommon
    {
        public List<CatalogAlternateId> AlternateIds;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> Ids;
    }

    [Serializable]
    public class GetDraftItemsResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<CatalogItem> Items;
    }

    [Serializable]
    public class GetEntityDraftItemsRequest : PlayFabRequestCommon
    {
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Filter;
    }

    [Serializable]
    public class GetEntityDraftItemsResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<CatalogItem> Items;
    }

    [Serializable]
    public class GetEntityItemReviewRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }

    [Serializable]
    public class GetEntityItemReviewResponse : PlayFabResultCommon
    {
        public Review Review;
    }
    [Serializable]
    public class GetInventoryCollectionIdsRequest : PlayFabRequestCommon
    {
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetInventoryCollectionIdsResponse : PlayFabResultCommon
    {
        public List<string> CollectionIds;
        public string ContinuationToken;
    }
    [Serializable]
    public class GetInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Filter;
    }

    [Serializable]
    public class GetInventoryItemsResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public string ETag;
        public List<InventoryItem> Items;
    }
    [Serializable]
    public class GetInventoryOperationStatusRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetInventoryOperationStatusResponse : PlayFabResultCommon
    {
        public string OperationStatus;
    }
    [Serializable]
    public class GetItemContainersRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }

    [Serializable]
    public class GetItemContainersResponse : PlayFabResultCommon
    {
        public List<CatalogItem> Containers;
        public string ContinuationToken;
    }

    [Serializable]
    public class GetItemModerationStateRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public string Id;
    }

    [Serializable]
    public class GetItemModerationStateResponse : PlayFabResultCommon
    {
        public ModerationState State;
    }

    [Serializable]
    public class GetItemPublishStatusRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }

    [Serializable]
    public class GetItemPublishStatusResponse : PlayFabResultCommon
    {
        public PublishResult? Result;
        public string StatusMessage;
    }

    [Serializable]
    public class GetItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
    }
    [Serializable]
    public class GetItemResponse : PlayFabResultCommon
    {
        public CatalogItem Item;
    }

    [Serializable]
    public class GetItemReviewsRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public string Id;
        public string OrderBy;
    }

    [Serializable]
    public class GetItemReviewsResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<Review> Reviews;
    }

    [Serializable]
    public class GetItemReviewSummaryRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public string Id;
    }

    [Serializable]
    public class GetItemReviewSummaryResponse : PlayFabResultCommon
    {
        public Review LeastFavorableReview;
        public Review MostFavorableReview;
        public Rating Rating;
        public int ReviewsCount;
    }

    [Serializable]
    public class GetItemsRequest : PlayFabRequestCommon
    {
        public List<CatalogAlternateId> AlternateIds;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> Ids;
    }

    [Serializable]
    public class GetItemsResponse : PlayFabResultCommon
    {
        public List<CatalogItem> Items;
    }
    [Serializable]
    public class GetMicrosoftStoreAccessTokensRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetMicrosoftStoreAccessTokensResponse : PlayFabResultCommon
    {
        public string CollectionsAccessToken;
        public DateTime CollectionsAccessTokenExpirationDate;
    }
    [Serializable]
    public class GetTransactionHistoryRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Filter;
        public string OrderBy;
    }

    [Serializable]
    public class GetTransactionHistoryResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<Transaction> Transactions;
    }

    [Serializable]
    public class GooglePlayProductPurchase : PlayFabBaseModel
    {
        public string ProductId;
        public string Token;
    }

    public enum HelpfulnessVote
    {
        None,
        UnHelpful,
        Helpful
    }

    [Serializable]
    public class Image : PlayFabBaseModel
    {
        public string Id;
        public string Tag;
        public string Type;
        public string Url;
    }

    [Serializable]
    public class ImageConfig : PlayFabBaseModel
    {
        public List<string> Tags;
    }

    [Serializable]
    public class InitialValues : PlayFabBaseModel
    {
        public object DisplayProperties;
    }

    [Serializable]
    public class InventoryItem : PlayFabBaseModel
    {
        public int? Amount;
        public object DisplayProperties;
        public DateTime? ExpirationDate;
        public string Id;
        public string StackId;
        public string Type;
    }

    [Serializable]
    public class InventoryItemReference : PlayFabBaseModel
    {
        public AlternateId AlternateId;
        public string Id;
        public string StackId;
    }

    [Serializable]
    public class InventoryOperation : PlayFabBaseModel
    {
        public AddInventoryItemsOperation Add;
        public DeleteInventoryItemsOperation Delete;
        public PurchaseInventoryItemsOperation Purchase;
        public SubtractInventoryItemsOperation Subtract;
        public TransferInventoryItemsOperation Transfer;
        public UpdateInventoryItemsOperation Update;
    }

    [Serializable]
    public class KeywordSet : PlayFabBaseModel
    {
        public List<string> Values;
    }

    [Serializable]
    public class ModerationState : PlayFabBaseModel
    {
        public DateTime? LastModifiedDate;
        public string Reason;
        public ModerationStatus? Status;
    }

    public enum ModerationStatus
    {
        Unknown,
        AwaitingModeration,
        Approved,
        Rejected
    }

    [Serializable]
    public class Permissions : PlayFabBaseModel
    {
        public List<string> SegmentIds;
    }
    [Serializable]
    public class PublishDraftItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ETag;
        public string Id;
    }

    [Serializable]
    public class PublishDraftItemResponse : PlayFabResultCommon
    {
    }

    public enum PublishResult
    {
        Unknown,
        Pending,
        Succeeded,
        Failed,
        Canceled
    }

    [Serializable]
    public class PurchaseInventoryItemsOperation : PlayFabBaseModel
    {
        public int? Amount;
        public bool DeleteEmptyStacks;
        public double? DurationInSeconds;
        public InventoryItemReference Item;
        public InitialValues NewStackValues;
        public List<PurchasePriceAmount> PriceAmounts;
        public string StoreId;
    }
    [Serializable]
    public class PurchaseInventoryItemsRequest : PlayFabRequestCommon
    {
        public int? Amount;
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public bool DeleteEmptyStacks;
        public double? DurationInSeconds;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public InventoryItemReference Item;
        public InitialValues NewStackValues;
        public List<PurchasePriceAmount> PriceAmounts;
        public string StoreId;
    }

    [Serializable]
    public class PurchaseInventoryItemsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }

    [Serializable]
    public class PurchaseOverridesInfo : PlayFabBaseModel
    {
    }

    [Serializable]
    public class PurchasePriceAmount : PlayFabBaseModel
    {
        public int Amount;
        public string ItemId;
        public string StackId;
    }

    [Serializable]
    public class Rating : PlayFabBaseModel
    {
        public float? Average;
        public int? Count1Star;
        public int? Count2Star;
        public int? Count3Star;
        public int? Count4Star;
        public int? Count5Star;
        public int? TotalCount;
    }

    [Serializable]
    public class RealMoneyPriceDetails : PlayFabBaseModel
    {
        public Dictionary<string,int> AppleAppStorePrices;
        public Dictionary<string,int> GooglePlayPrices;
        public Dictionary<string,int> MicrosoftStorePrices;
        public Dictionary<string,int> NintendoEShopPrices;
        public Dictionary<string,int> PlayStationStorePrices;
        public Dictionary<string,int> SteamPrices;
    }
    [Serializable]
    public class RedeemAppleAppStoreInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Receipt;
    }

    [Serializable]
    public class RedeemAppleAppStoreInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class RedeemGooglePlayInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<GooglePlayProductPurchase> Purchases;
    }

    [Serializable]
    public class RedeemGooglePlayInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class RedeemMicrosoftStoreInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public string CollectionsIdKey;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string XboxToken;
    }

    [Serializable]
    public class RedeemMicrosoftStoreInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class RedeemNintendoEShopInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string NintendoServiceAccountIdToken;
    }

    [Serializable]
    public class RedeemNintendoEShopInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class RedeemPlayStationStoreInventoryItemsRequest : PlayFabRequestCommon
    {
        public string AuthorizationCode;
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string RedirectUri;
        public string ServiceLabel;
    }

    [Serializable]
    public class RedeemPlayStationStoreInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class RedeemSteamInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class RedeemSteamInventoryItemsResponse : PlayFabResultCommon
    {
        public List<RedemptionFailure> Failed;
        public List<RedemptionSuccess> Succeeded;
        public List<string> TransactionIds;
    }

    [Serializable]
    public class RedemptionFailure : PlayFabBaseModel
    {
        public string FailureCode;
        public string FailureDetails;
        public string MarketplaceAlternateId;
        public string MarketplaceTransactionId;
    }

    [Serializable]
    public class RedemptionSuccess : PlayFabBaseModel
    {
        public string MarketplaceAlternateId;
        public string MarketplaceTransactionId;
        public DateTime SuccessTimestamp;
    }

    [Serializable]
    public class ReportItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public ConcernCategory? ConcernCategory;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
        public string Reason;
    }

    [Serializable]
    public class ReportItemResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class ReportItemReviewRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public ConcernCategory? ConcernCategory;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ItemId;
        public string Reason;
        public string ReviewId;
    }

    [Serializable]
    public class ReportItemReviewResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class Review : PlayFabBaseModel
    {
        public Dictionary<string,int> CategoryRatings;
        public int HelpfulNegative;
        public int HelpfulPositive;
        public bool IsInstalled;
        public string ItemId;
        public string ItemVersion;
        public string Locale;
        public int Rating;
        public EntityKey ReviewerEntity;
        public string ReviewId;
        public string ReviewText;
        public DateTime Submitted;
        public string Title;
    }

    [Serializable]
    public class ReviewConfig : PlayFabBaseModel
    {
        public List<CategoryRatingConfig> CategoryRatings;
    }

    [Serializable]
    public class ReviewItemRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Id;
        public Review Review;
    }

    [Serializable]
    public class ReviewItemResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class ReviewTakedown : PlayFabBaseModel
    {
        public CatalogAlternateId AlternateId;
        public string ItemId;
        public string ReviewId;
    }

    [Serializable]
    public class SearchItemsRequest : PlayFabRequestCommon
    {
        public string ContinuationToken;
        public int Count;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string Filter;
        public string Language;
        public string OrderBy;
        public string Search;
        public string Select;
        public StoreReference Store;
    }

    [Serializable]
    public class SearchItemsResponse : PlayFabResultCommon
    {
        public string ContinuationToken;
        public List<CatalogItem> Items;
    }

    [Serializable]
    public class SetItemModerationStateRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public string Id;
        public string Reason;
        public ModerationStatus? Status;
    }

    [Serializable]
    public class SetItemModerationStateResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class StoreDetails : PlayFabBaseModel
    {
        public FilterOptions FilterOptions;
        public Permissions Permissions;
        public CatalogPriceOptionsOverride PriceOptionsOverride;
    }

    [Serializable]
    public class StoreReference : PlayFabBaseModel
    {
        public CatalogAlternateId AlternateId;
        public string Id;
    }

    [Serializable]
    public class SubmitItemReviewVoteRequest : PlayFabRequestCommon
    {
        public CatalogAlternateId AlternateId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ItemId;
        public string ReviewId;
        public HelpfulnessVote? Vote;
    }

    [Serializable]
    public class SubmitItemReviewVoteResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class SubtractInventoryItemsOperation : PlayFabBaseModel
    {
        public int? Amount;
        public bool DeleteEmptyStacks;
        public double? DurationInSeconds;
        public InventoryItemReference Item;
    }
    [Serializable]
    public class SubtractInventoryItemsRequest : PlayFabRequestCommon
    {
        public int? Amount;
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public bool DeleteEmptyStacks;
        public double? DurationInSeconds;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public InventoryItemReference Item;
    }

    [Serializable]
    public class SubtractInventoryItemsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }
    [Serializable]
    public class TakedownItemReviewsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<ReviewTakedown> Reviews;
    }

    [Serializable]
    public class TakedownItemReviewsResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class Transaction : PlayFabBaseModel
    {
        public string ApiName;
        public string ItemType;
        public List<TransactionOperation> Operations;
        public string OperationType;
        public TransactionPurchaseDetails PurchaseDetails;
        public TransactionRedeemDetails RedeemDetails;
        public DateTime Timestamp;
        public string TransactionId;
        public TransactionTransferDetails TransferDetails;
    }

    [Serializable]
    public class TransactionOperation : PlayFabBaseModel
    {
        public int? Amount;
        public double? DurationInSeconds;
        public string ItemFriendlyId;
        public string ItemId;
        public string ItemType;
        public string StackId;
        public string Type;
    }

    [Serializable]
    public class TransactionPurchaseDetails : PlayFabBaseModel
    {
        public string StoreFriendlyId;
        public string StoreId;
    }

    [Serializable]
    public class TransactionRedeemDetails : PlayFabBaseModel
    {
        public string Marketplace;
        public string MarketplaceTransactionId;
        public string OfferId;
    }

    [Serializable]
    public class TransactionTransferDetails : PlayFabBaseModel
    {
        public string GivingCollectionId;
        public EntityKey GivingEntity;
        public string ReceivingCollectionId;
        public EntityKey ReceivingEntity;
        public string TransferId;
    }

    [Serializable]
    public class TransferInventoryItemsOperation : PlayFabBaseModel
    {
        public int? Amount;
        public bool DeleteEmptyStacks;
        public InventoryItemReference GivingItem;
        public InitialValues NewStackValues;
        public InventoryItemReference ReceivingItem;
    }
    [Serializable]
    public class TransferInventoryItemsRequest : PlayFabRequestCommon
    {
        public int? Amount;
        public Dictionary<string,string> CustomTags;
        public bool DeleteEmptyStacks;
        public string GivingCollectionId;
        public EntityKey GivingEntity;
        public string GivingETag;
        public InventoryItemReference GivingItem;
        public string IdempotencyId;
        public InitialValues NewStackValues;
        public string ReceivingCollectionId;
        public EntityKey ReceivingEntity;
        public InventoryItemReference ReceivingItem;
    }

    [Serializable]
    public class TransferInventoryItemsResponse : PlayFabResultCommon
    {
        public string GivingETag;
        public List<string> GivingTransactionIds;
        public string IdempotencyId;
        public string OperationStatus;
        public string OperationToken;
        public List<string> ReceivingTransactionIds;
    }

    [Serializable]
    public class UpdateCatalogConfigRequest : PlayFabRequestCommon
    {
        public CatalogConfig Config;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class UpdateCatalogConfigResponse : PlayFabResultCommon
    {
    }

    [Serializable]
    public class UpdateDraftItemRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public CatalogItem Item;
        public bool Publish;
    }

    [Serializable]
    public class UpdateDraftItemResponse : PlayFabResultCommon
    {
        public CatalogItem Item;
    }

    [Serializable]
    public class UpdateInventoryItemsOperation : PlayFabBaseModel
    {
        public InventoryItem Item;
    }
    [Serializable]
    public class UpdateInventoryItemsRequest : PlayFabRequestCommon
    {
        public string CollectionId;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string ETag;
        public string IdempotencyId;
        public InventoryItem Item;
    }

    [Serializable]
    public class UpdateInventoryItemsResponse : PlayFabResultCommon
    {
        public string ETag;
        public string IdempotencyId;
        public List<string> TransactionIds;
    }

    [Serializable]
    public class UploadInfo : PlayFabBaseModel
    {
        public string FileName;
    }

    [Serializable]
    public class UploadUrlMetadata : PlayFabBaseModel
    {
        public string FileName;
        public string Id;
        public string Url;
    }

    [Serializable]
    public class UserGeneratedContentSpecificConfig : PlayFabBaseModel
    {
        public List<string> ContentTypes;
        public List<string> Tags;
    }
}
#endif
