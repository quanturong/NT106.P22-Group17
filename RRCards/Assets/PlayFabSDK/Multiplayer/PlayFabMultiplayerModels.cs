#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.MultiplayerModels
{
    public enum AccessPolicy
    {
        Public,
        Friends,
        Private
    }

    [Serializable]
    public class AssetReference : PlayFabBaseModel
    {
        public string FileName;
        public string MountPath;
    }

    [Serializable]
    public class AssetReferenceParams : PlayFabBaseModel
    {
        public string FileName;
        public string MountPath;
    }

    [Serializable]
    public class AssetSummary : PlayFabBaseModel
    {
        public string FileName;
        public Dictionary<string,string> Metadata;
    }

    public enum AttributeMergeFunction
    {
        Min,
        Max,
        Average
    }

    public enum AttributeNotSpecifiedBehavior
    {
        UseDefault,
        MatchAny
    }

    public enum AttributeSource
    {
        User,
        PlayerEntity
    }

    public enum AzureRegion
    {
        AustraliaEast,
        AustraliaSoutheast,
        BrazilSouth,
        CentralUs,
        EastAsia,
        EastUs,
        EastUs2,
        JapanEast,
        JapanWest,
        NorthCentralUs,
        NorthEurope,
        SouthCentralUs,
        SoutheastAsia,
        WestEurope,
        WestUs,
        SouthAfricaNorth,
        WestCentralUs,
        KoreaCentral,
        FranceCentral,
        WestUs2,
        CentralIndia,
        UaeNorth,
        UkSouth,
        SwedenCentral,
        CanadaCentral,
        MexicoCentral
    }

    public enum AzureVmFamily
    {
        A,
        Av2,
        Dv2,
        Dv3,
        F,
        Fsv2,
        Dasv4,
        Dav4,
        Dadsv5,
        Eav4,
        Easv4,
        Ev4,
        Esv4,
        Dsv3,
        Dsv2,
        NCasT4_v3,
        Ddv4,
        Ddsv4,
        HBv3,
        Ddv5,
        Ddsv5
    }

    public enum AzureVmSize
    {
        Standard_A1,
        Standard_A2,
        Standard_A3,
        Standard_A4,
        Standard_A1_v2,
        Standard_A2_v2,
        Standard_A4_v2,
        Standard_A8_v2,
        Standard_D1_v2,
        Standard_D2_v2,
        Standard_D3_v2,
        Standard_D4_v2,
        Standard_D5_v2,
        Standard_D2_v3,
        Standard_D4_v3,
        Standard_D8_v3,
        Standard_D16_v3,
        Standard_F1,
        Standard_F2,
        Standard_F4,
        Standard_F8,
        Standard_F16,
        Standard_F2s_v2,
        Standard_F4s_v2,
        Standard_F8s_v2,
        Standard_F16s_v2,
        Standard_D2as_v4,
        Standard_D4as_v4,
        Standard_D8as_v4,
        Standard_D16as_v4,
        Standard_D2a_v4,
        Standard_D4a_v4,
        Standard_D8a_v4,
        Standard_D16a_v4,
        Standard_D2ads_v5,
        Standard_D4ads_v5,
        Standard_D8ads_v5,
        Standard_D16ads_v5,
        Standard_E2a_v4,
        Standard_E4a_v4,
        Standard_E8a_v4,
        Standard_E16a_v4,
        Standard_E2as_v4,
        Standard_E4as_v4,
        Standard_E8as_v4,
        Standard_E16as_v4,
        Standard_D2s_v3,
        Standard_D4s_v3,
        Standard_D8s_v3,
        Standard_D16s_v3,
        Standard_DS1_v2,
        Standard_DS2_v2,
        Standard_DS3_v2,
        Standard_DS4_v2,
        Standard_DS5_v2,
        Standard_NC4as_T4_v3,
        Standard_D2d_v4,
        Standard_D4d_v4,
        Standard_D8d_v4,
        Standard_D16d_v4,
        Standard_D2ds_v4,
        Standard_D4ds_v4,
        Standard_D8ds_v4,
        Standard_D16ds_v4,
        Standard_HB120_16rs_v3,
        Standard_HB120_32rs_v3,
        Standard_HB120_64rs_v3,
        Standard_HB120_96rs_v3,
        Standard_HB120rs_v3,
        Standard_D2d_v5,
        Standard_D4d_v5,
        Standard_D8d_v5,
        Standard_D16d_v5,
        Standard_D32d_v5,
        Standard_D2ds_v5,
        Standard_D4ds_v5,
        Standard_D8ds_v5,
        Standard_D16ds_v5,
        Standard_D32ds_v5
    }

    [Serializable]
    public class BuildAliasDetailsResponse : PlayFabResultCommon
    {
        public string AliasId;
        public string AliasName;
        public List<BuildSelectionCriterion> BuildSelectionCriteria;
    }

    [Serializable]
    public class BuildAliasParams : PlayFabBaseModel
    {
        public string AliasId;
    }

    [Serializable]
    public class BuildRegion : PlayFabBaseModel
    {
        public CurrentServerStats CurrentServerStats;
        public DynamicStandbySettings DynamicStandbySettings;
        public bool IsAssetReplicationComplete;
        public int MaxServers;
        public int? MultiplayerServerCountPerVm;
        public string Region;
        public ScheduledStandbySettings ScheduledStandbySettings;
        public int StandbyServers;
        public string Status;
        public AzureVmSize? VmSize;
    }

    [Serializable]
    public class BuildRegionParams : PlayFabBaseModel
    {
        public DynamicStandbySettings DynamicStandbySettings;
        public int MaxServers;
        public int? MultiplayerServerCountPerVm;
        public string Region;
        public ScheduledStandbySettings ScheduledStandbySettings;
        public int StandbyServers;
        public AzureVmSize? VmSize;
    }

    [Serializable]
    public class BuildSelectionCriterion : PlayFabBaseModel
    {
        public Dictionary<string,uint> BuildWeightDistribution;
    }

    [Serializable]
    public class BuildSummary : PlayFabBaseModel
    {
        public string BuildId;
        public string BuildName;
        public DateTime? CreationTime;
        public Dictionary<string,string> Metadata;
        public List<BuildRegion> RegionConfigurations;
    }
    [Serializable]
    public class CancelAllMatchmakingTicketsForPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string QueueName;
    }

    [Serializable]
    public class CancelAllMatchmakingTicketsForPlayerResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class CancelAllServerBackfillTicketsForPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string QueueName;
    }

    [Serializable]
    public class CancelAllServerBackfillTicketsForPlayerResult : PlayFabResultCommon
    {
    }

    public enum CancellationReason
    {
        Requested,
        Internal,
        Timeout
    }
    [Serializable]
    public class CancelMatchmakingTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string QueueName;
        public string TicketId;
    }

    [Serializable]
    public class CancelMatchmakingTicketResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class CancelServerBackfillTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string QueueName;
        public string TicketId;
    }

    [Serializable]
    public class CancelServerBackfillTicketResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class Certificate : PlayFabBaseModel
    {
        public string Base64EncodedValue;
        public string Name;
        public string Password;
    }

    [Serializable]
    public class CertificateSummary : PlayFabBaseModel
    {
        public string Name;
        public string Thumbprint;
    }

    [Serializable]
    public class ConnectedPlayer : PlayFabBaseModel
    {
        public string PlayerId;
    }

    public enum ContainerFlavor
    {
        ManagedWindowsServerCore,
        CustomLinux,
        ManagedWindowsServerCorePreview,
        Invalid
    }

    [Serializable]
    public class ContainerImageReference : PlayFabBaseModel
    {
        public string ImageName;
        public string Tag;
    }

    [Serializable]
    public class CoreCapacity : PlayFabBaseModel
    {
        public int Available;
        public string Region;
        public int Total;
        public AzureVmFamily? VmFamily;
    }

    [Serializable]
    public class CoreCapacityChange : PlayFabBaseModel
    {
        public int NewCoreLimit;
        public string Region;
        public AzureVmFamily VmFamily;
    }
    [Serializable]
    public class CreateBuildAliasRequest : PlayFabRequestCommon
    {
        public string AliasName;
        public List<BuildSelectionCriterion> BuildSelectionCriteria;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class CreateBuildWithCustomContainerRequest : PlayFabRequestCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildName;
        public ContainerFlavor? ContainerFlavor;
        public ContainerImageReference ContainerImageReference;
        public string ContainerRunCommand;
        public Dictionary<string,string> CustomTags;
        public List<AssetReferenceParams> GameAssetReferences;
        public List<GameCertificateReferenceParams> GameCertificateReferences;
        public List<GameSecretReferenceParams> GameSecretReferences;
        public LinuxInstrumentationConfiguration LinuxInstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfigurationParams MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public List<Port> Ports;
        public List<BuildRegionParams> RegionConfigurations;
        public ServerResourceConstraintParams ServerResourceConstraints;
        public AzureVmSize? VmSize;
        public VmStartupScriptParams VmStartupScriptConfiguration;
    }

    [Serializable]
    public class CreateBuildWithCustomContainerResponse : PlayFabResultCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildId;
        public string BuildName;
        public ContainerFlavor? ContainerFlavor;
        public string ContainerRunCommand;
        public DateTime? CreationTime;
        public ContainerImageReference CustomGameContainerImage;
        public List<AssetReference> GameAssetReferences;
        public List<GameCertificateReference> GameCertificateReferences;
        public List<GameSecretReference> GameSecretReferences;
        public LinuxInstrumentationConfiguration LinuxInstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfiguration MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public string OsPlatform;
        public List<Port> Ports;
        public List<BuildRegion> RegionConfigurations;
        public ServerResourceConstraintParams ServerResourceConstraints;
        public string ServerType;
        public bool? UseStreamingForAssetDownloads;
        public AzureVmSize? VmSize;
        public VmStartupScriptConfiguration VmStartupScriptConfiguration;
    }
    [Serializable]
    public class CreateBuildWithManagedContainerRequest : PlayFabRequestCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildName;
        public ContainerFlavor? ContainerFlavor;
        public Dictionary<string,string> CustomTags;
        public List<AssetReferenceParams> GameAssetReferences;
        public List<GameCertificateReferenceParams> GameCertificateReferences;
        public List<GameSecretReferenceParams> GameSecretReferences;
        public string GameWorkingDirectory;
        public InstrumentationConfiguration InstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfigurationParams MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public List<Port> Ports;
        public List<BuildRegionParams> RegionConfigurations;
        public ServerResourceConstraintParams ServerResourceConstraints;
        public string StartMultiplayerServerCommand;
        public AzureVmSize? VmSize;
        public VmStartupScriptParams VmStartupScriptConfiguration;
        public WindowsCrashDumpConfiguration WindowsCrashDumpConfiguration;
    }

    [Serializable]
    public class CreateBuildWithManagedContainerResponse : PlayFabResultCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildId;
        public string BuildName;
        public ContainerFlavor? ContainerFlavor;
        public DateTime? CreationTime;
        public List<AssetReference> GameAssetReferences;
        public List<GameCertificateReference> GameCertificateReferences;
        public List<GameSecretReference> GameSecretReferences;
        public string GameWorkingDirectory;
        public InstrumentationConfiguration InstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfiguration MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public string OsPlatform;
        public List<Port> Ports;
        public List<BuildRegion> RegionConfigurations;
        public ServerResourceConstraintParams ServerResourceConstraints;
        public string ServerType;
        public string StartMultiplayerServerCommand;
        public bool? UseStreamingForAssetDownloads;
        public AzureVmSize? VmSize;
        public VmStartupScriptConfiguration VmStartupScriptConfiguration;
    }
    [Serializable]
    public class CreateBuildWithProcessBasedServerRequest : PlayFabRequestCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildName;
        public Dictionary<string,string> CustomTags;
        public List<AssetReferenceParams> GameAssetReferences;
        public List<GameCertificateReferenceParams> GameCertificateReferences;
        public List<GameSecretReferenceParams> GameSecretReferences;
        public string GameWorkingDirectory;
        public InstrumentationConfiguration InstrumentationConfiguration;
        public bool? IsOSPreview;
        public LinuxInstrumentationConfiguration LinuxInstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfigurationParams MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public string OsPlatform;
        public List<Port> Ports;
        public List<BuildRegionParams> RegionConfigurations;
        public string StartMultiplayerServerCommand;
        public AzureVmSize? VmSize;
        public VmStartupScriptParams VmStartupScriptConfiguration;
    }

    [Serializable]
    public class CreateBuildWithProcessBasedServerResponse : PlayFabResultCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildId;
        public string BuildName;
        public ContainerFlavor? ContainerFlavor;
        public DateTime? CreationTime;
        public List<AssetReference> GameAssetReferences;
        public List<GameCertificateReference> GameCertificateReferences;
        public List<GameSecretReference> GameSecretReferences;
        public string GameWorkingDirectory;
        public InstrumentationConfiguration InstrumentationConfiguration;
        public bool? IsOSPreview;
        public LinuxInstrumentationConfiguration LinuxInstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public MonitoringApplicationConfiguration MonitoringApplicationConfiguration;
        public int MultiplayerServerCountPerVm;
        public string OsPlatform;
        public List<Port> Ports;
        public List<BuildRegion> RegionConfigurations;
        public string ServerType;
        public string StartMultiplayerServerCommand;
        public bool? UseStreamingForAssetDownloads;
        public AzureVmSize? VmSize;
        public VmStartupScriptConfiguration VmStartupScriptConfiguration;
    }
    [Serializable]
    public class CreateLobbyRequest : PlayFabRequestCommon
    {
        public AccessPolicy? AccessPolicy;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> LobbyData;
        public uint MaxPlayers;
        public List<Member> Members;
        public EntityKey Owner;
        public OwnerMigrationPolicy? OwnerMigrationPolicy;
        public bool RestrictInvitesToLobbyOwner;
        public Dictionary<string,string> SearchData;
        public bool UseConnections;
    }

    [Serializable]
    public class CreateLobbyResult : PlayFabResultCommon
    {
        public string ConnectionString;
        public string LobbyId;
    }
    [Serializable]
    public class CreateMatchmakingTicketRequest : PlayFabRequestCommon
    {
        public MatchmakingPlayer Creator;
        public Dictionary<string,string> CustomTags;
        public int GiveUpAfterSeconds;
        public List<EntityKey> MembersToMatchWith;
        public string QueueName;
    }

    [Serializable]
    public class CreateMatchmakingTicketResult : PlayFabResultCommon
    {
        public string TicketId;
    }
    [Serializable]
    public class CreateRemoteUserRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public DateTime? ExpirationTime;
        public string Region;
        public string Username;
        public string VmId;
    }

    [Serializable]
    public class CreateRemoteUserResponse : PlayFabResultCommon
    {
        public DateTime? ExpirationTime;
        public string Password;
        public string Username;
    }
    [Serializable]
    public class CreateServerBackfillTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int GiveUpAfterSeconds;
        public List<MatchmakingPlayerWithTeamAssignment> Members;
        public string QueueName;
        public ServerDetails ServerDetails;
    }

    [Serializable]
    public class CreateServerBackfillTicketResult : PlayFabResultCommon
    {
        public string TicketId;
    }
    [Serializable]
    public class CreateServerMatchmakingTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int GiveUpAfterSeconds;
        public List<MatchmakingPlayer> Members;
        public string QueueName;
    }
    [Serializable]
    public class CreateTitleMultiplayerServersQuotaChangeRequest : PlayFabRequestCommon
    {
        public string ChangeDescription;
        public List<CoreCapacityChange> Changes;
        public string ContactEmail;
        public Dictionary<string,string> CustomTags;
        public string Notes;
        public DateTime? StartDate;
    }

    [Serializable]
    public class CreateTitleMultiplayerServersQuotaChangeResponse : PlayFabResultCommon
    {
        public string RequestId;
        public bool WasApproved;
    }

    [Serializable]
    public class CurrentServerStats : PlayFabBaseModel
    {
        public int Active;
        public int Propping;
        public int StandingBy;
        public int Total;
    }

    [Serializable]
    public class CustomDifferenceRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideDouble> DifferenceOverrides;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class CustomRegionSelectionRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideUnsignedInt> MaxLatencyOverrides;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class CustomSetIntersectionRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideUnsignedInt> MinIntersectionSizeOverrides;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class CustomTeamDifferenceRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideDouble> DifferenceOverrides;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class CustomTeamSizeBalanceRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideUnsignedInt> DifferenceOverrides;
        public uint SecondsBetweenExpansions;
    }
    [Serializable]
    public class DeleteAssetRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FileName;
    }
    [Serializable]
    public class DeleteBuildAliasRequest : PlayFabRequestCommon
    {
        public string AliasId;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class DeleteBuildRegionRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public string Region;
    }
    [Serializable]
    public class DeleteBuildRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class DeleteCertificateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }
    [Serializable]
    public class DeleteContainerImageRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ImageName;
    }
    [Serializable]
    public class DeleteLobbyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
    }
    [Serializable]
    public class DeleteRemoteUserRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public string Region;
        public string Username;
        public string VmId;
    }
    [Serializable]
    public class DeleteSecretRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class DifferenceRule : PlayFabBaseModel
    {
        public QueueRuleAttribute Attribute;
        public AttributeNotSpecifiedBehavior AttributeNotSpecifiedBehavior;
        public CustomDifferenceRuleExpansion CustomExpansion;
        public double? DefaultAttributeValue;
        public double Difference;
        public LinearDifferenceRuleExpansion LinearExpansion;
        public AttributeMergeFunction MergeFunction;
        public string Name;
        public uint? SecondsUntilOptional;
        public double Weight;
    }

    public enum DirectPeerConnectivityOptions
    {
        None,
        SamePlatformType,
        DifferentPlatformType,
        AnyPlatformType,
        SameEntityLoginProvider,
        DifferentEntityLoginProvider,
        AnyEntityLoginProvider,
        AnyPlatformTypeAndEntityLoginProvider,
        OnlyServers
    }

    [Serializable]
    public class DynamicStandbySettings : PlayFabBaseModel
    {
        public List<DynamicStandbyThreshold> DynamicFloorMultiplierThresholds;
        public bool IsEnabled;
        public int? RampDownSeconds;
    }

    [Serializable]
    public class DynamicStandbyThreshold : PlayFabBaseModel
    {
        public double Multiplier;
        public double TriggerThresholdPercentage;
    }

    [Serializable]
    public class EmptyResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class EnableMultiplayerServersForTitleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class EnableMultiplayerServersForTitleResponse : PlayFabResultCommon
    {
        public TitleMultiplayerServerEnabledStatus? Status;
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
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
    public class FindFriendLobbiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public ExternalFriendSources? ExternalPlatformFriends;
        public string Filter;
        public string OrderBy;
        public PaginationRequest Pagination;
        public string XboxToken;
    }

    [Serializable]
    public class FindFriendLobbiesResult : PlayFabResultCommon
    {
        public List<FriendLobbySummary> Lobbies;
        public PaginationResponse Pagination;
    }
    [Serializable]
    public class FindLobbiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Filter;
        public string OrderBy;
        public PaginationRequest Pagination;
    }

    [Serializable]
    public class FindLobbiesResult : PlayFabResultCommon
    {
        public List<LobbySummary> Lobbies;
        public PaginationResponse Pagination;
    }

    [Serializable]
    public class FriendLobbySummary : PlayFabBaseModel
    {
        public string ConnectionString;
        public uint CurrentPlayers;
        public List<EntityKey> Friends;
        public string LobbyId;
        public uint MaxPlayers;
        public MembershipLock? MembershipLock;
        public EntityKey Owner;
        public Dictionary<string,string> SearchData;
    }

    [Serializable]
    public class GameCertificateReference : PlayFabBaseModel
    {
        public string GsdkAlias;
        public string Name;
    }

    [Serializable]
    public class GameCertificateReferenceParams : PlayFabBaseModel
    {
        public string GsdkAlias;
        public string Name;
    }

    [Serializable]
    public class GameSecretReference : PlayFabBaseModel
    {
        public string Name;
    }

    [Serializable]
    public class GameSecretReferenceParams : PlayFabBaseModel
    {
        public string Name;
    }
    [Serializable]
    public class GetAssetDownloadUrlRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FileName;
    }

    [Serializable]
    public class GetAssetDownloadUrlResponse : PlayFabResultCommon
    {
        public string AssetDownloadUrl;
        public string FileName;
    }
    [Serializable]
    public class GetAssetUploadUrlRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string FileName;
    }

    [Serializable]
    public class GetAssetUploadUrlResponse : PlayFabResultCommon
    {
        public string AssetUploadUrl;
        public string FileName;
    }
    [Serializable]
    public class GetBuildAliasRequest : PlayFabRequestCommon
    {
        public string AliasId;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class GetBuildRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetBuildResponse : PlayFabResultCommon
    {
        public bool? AreAssetsReadonly;
        public string BuildId;
        public string BuildName;
        public string BuildStatus;
        public ContainerFlavor? ContainerFlavor;
        public string ContainerRunCommand;
        public DateTime? CreationTime;
        public ContainerImageReference CustomGameContainerImage;
        public List<AssetReference> GameAssetReferences;
        public List<GameCertificateReference> GameCertificateReferences;
        public InstrumentationConfiguration InstrumentationConfiguration;
        public Dictionary<string,string> Metadata;
        public int MultiplayerServerCountPerVm;
        public string OsPlatform;
        public List<Port> Ports;
        public List<BuildRegion> RegionConfigurations;
        public ServerResourceConstraintParams ServerResourceConstraints;
        public string ServerType;
        public string StartMultiplayerServerCommand;
        public AzureVmSize? VmSize;
        public VmStartupScriptConfiguration VmStartupScriptConfiguration;
    }
    [Serializable]
    public class GetContainerRegistryCredentialsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetContainerRegistryCredentialsResponse : PlayFabResultCommon
    {
        public string DnsName;
        public string Password;
        public string Username;
    }
    [Serializable]
    public class GetLobbyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
    }

    [Serializable]
    public class GetLobbyResult : PlayFabResultCommon
    {
        public Lobby Lobby;
    }
    [Serializable]
    public class GetMatchmakingQueueRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string QueueName;
    }

    [Serializable]
    public class GetMatchmakingQueueResult : PlayFabResultCommon
    {
        public MatchmakingQueueConfig MatchmakingQueue;
    }
    [Serializable]
    public class GetMatchmakingTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool EscapeObject;
        public string QueueName;
        public string TicketId;
    }

    [Serializable]
    public class GetMatchmakingTicketResult : PlayFabResultCommon
    {
        public string CancellationReasonString;
        public uint? ChangeNumber;
        public DateTime Created;
        public EntityKey Creator;
        public int GiveUpAfterSeconds;
        public string MatchId;
        public List<MatchmakingPlayer> Members;
        public List<EntityKey> MembersToMatchWith;
        public string QueueName;
        public string Status;
        public string TicketId;
    }
    [Serializable]
    public class GetMatchRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool EscapeObject;
        public string MatchId;
        public string QueueName;
        public bool ReturnMemberAttributes;
    }

    [Serializable]
    public class GetMatchResult : PlayFabResultCommon
    {
        public string ArrangementString;
        public string MatchId;
        public List<MatchmakingPlayerWithTeamAssignment> Members;
        public List<string> RegionPreferences;
        public ServerDetails ServerDetails;
    }
    [Serializable]
    public class GetMultiplayerServerDetailsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string SessionId;
    }

    [Serializable]
    public class GetMultiplayerServerDetailsResponse : PlayFabResultCommon
    {
        public string BuildId;
        public List<ConnectedPlayer> ConnectedPlayers;
        public string FQDN;
        public string IPV4Address;
        public DateTime? LastStateTransitionTime;
        public List<Port> Ports;
        public List<PublicIpAddress> PublicIPV4Addresses;
        public string Region;
        public string ServerId;
        public string SessionId;
        public string State;
        public string VmId;
    }
    [Serializable]
    public class GetMultiplayerServerLogsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ServerId;
    }

    [Serializable]
    public class GetMultiplayerServerLogsResponse : PlayFabResultCommon
    {
        public string LogDownloadUrl;
    }
    [Serializable]
    public class GetMultiplayerSessionLogsBySessionIdRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string SessionId;
    }
    [Serializable]
    public class GetQueueStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string QueueName;
    }

    [Serializable]
    public class GetQueueStatisticsResult : PlayFabResultCommon
    {
        public uint? NumberOfPlayersMatching;
        public Statistics TimeToMatchStatisticsInSeconds;
    }
    [Serializable]
    public class GetRemoteLoginEndpointRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public string Region;
        public string VmId;
    }

    [Serializable]
    public class GetRemoteLoginEndpointResponse : PlayFabResultCommon
    {
        public string IPV4Address;
        public int Port;
    }
    [Serializable]
    public class GetServerBackfillTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool EscapeObject;
        public string QueueName;
        public string TicketId;
    }

    [Serializable]
    public class GetServerBackfillTicketResult : PlayFabResultCommon
    {
        public string CancellationReasonString;
        public DateTime Created;
        public int GiveUpAfterSeconds;
        public string MatchId;
        public List<MatchmakingPlayerWithTeamAssignment> Members;
        public string QueueName;
        public ServerDetails ServerDetails;
        public string Status;
        public string TicketId;
    }
    [Serializable]
    public class GetTitleEnabledForMultiplayerServersStatusRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetTitleEnabledForMultiplayerServersStatusResponse : PlayFabResultCommon
    {
        public TitleMultiplayerServerEnabledStatus? Status;
    }
    [Serializable]
    public class GetTitleMultiplayerServersQuotaChangeRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string RequestId;
    }

    [Serializable]
    public class GetTitleMultiplayerServersQuotaChangeResponse : PlayFabResultCommon
    {
        public QuotaChange Change;
    }
    [Serializable]
    public class GetTitleMultiplayerServersQuotasRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetTitleMultiplayerServersQuotasResponse : PlayFabResultCommon
    {
        public TitleMultiplayerServersQuotas Quotas;
    }

    [Serializable]
    public class InstrumentationConfiguration : PlayFabBaseModel
    {
        public bool? IsEnabled;
        public List<string> ProcessesToMonitor;
    }
    [Serializable]
    public class InviteToLobbyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey InviteeEntity;
        public string LobbyId;
        public EntityKey MemberEntity;
    }
    [Serializable]
    public class JoinArrangedLobbyRequest : PlayFabRequestCommon
    {
        public AccessPolicy? AccessPolicy;
        public string ArrangementString;
        public Dictionary<string,string> CustomTags;
        public uint MaxPlayers;
        public Dictionary<string,string> MemberData;
        public EntityKey MemberEntity;
        public OwnerMigrationPolicy? OwnerMigrationPolicy;
        public bool RestrictInvitesToLobbyOwner;
        public bool UseConnections;
    }
    [Serializable]
    public class JoinLobbyAsServerRequest : PlayFabRequestCommon
    {
        public string ConnectionString;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> ServerData;
        public EntityKey ServerEntity;
    }

    [Serializable]
    public class JoinLobbyAsServerResult : PlayFabResultCommon
    {
        public string LobbyId;
    }
    [Serializable]
    public class JoinLobbyRequest : PlayFabRequestCommon
    {
        public string ConnectionString;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> MemberData;
        public EntityKey MemberEntity;
    }

    [Serializable]
    public class JoinLobbyResult : PlayFabResultCommon
    {
        public string LobbyId;
    }
    [Serializable]
    public class JoinMatchmakingTicketRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public MatchmakingPlayer Member;
        public string QueueName;
        public string TicketId;
    }

    [Serializable]
    public class JoinMatchmakingTicketResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class LeaveLobbyAsServerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public EntityKey ServerEntity;
    }
    [Serializable]
    public class LeaveLobbyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public EntityKey MemberEntity;
    }

    [Serializable]
    public class LinearDifferenceRuleExpansion : PlayFabBaseModel
    {
        public double Delta;
        public double? Limit;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class LinearRegionSelectionRuleExpansion : PlayFabBaseModel
    {
        public uint Delta;
        public uint Limit;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class LinearSetIntersectionRuleExpansion : PlayFabBaseModel
    {
        public uint Delta;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class LinearTeamDifferenceRuleExpansion : PlayFabBaseModel
    {
        public double Delta;
        public double? Limit;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class LinearTeamSizeBalanceRuleExpansion : PlayFabBaseModel
    {
        public uint Delta;
        public uint? Limit;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class LinuxInstrumentationConfiguration : PlayFabBaseModel
    {
        public bool IsEnabled;
    }
    [Serializable]
    public class ListAssetSummariesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListAssetSummariesResponse : PlayFabResultCommon
    {
        public List<AssetSummary> AssetSummaries;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListBuildAliasesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListBuildAliasesResponse : PlayFabResultCommon
    {
        public List<BuildAliasDetailsResponse> BuildAliases;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListBuildSummariesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListBuildSummariesResponse : PlayFabResultCommon
    {
        public List<BuildSummary> BuildSummaries;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListCertificateSummariesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListCertificateSummariesResponse : PlayFabResultCommon
    {
        public List<CertificateSummary> CertificateSummaries;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListContainerImagesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListContainerImagesResponse : PlayFabResultCommon
    {
        public List<string> Images;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListContainerImageTagsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ImageName;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListContainerImageTagsResponse : PlayFabResultCommon
    {
        public int PageSize;
        public string SkipToken;
        public List<string> Tags;
    }
    [Serializable]
    public class ListMatchmakingQueuesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListMatchmakingQueuesResult : PlayFabResultCommon
    {
        public List<MatchmakingQueueConfig> MatchMakingQueues;
    }
    [Serializable]
    public class ListMatchmakingTicketsForPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string QueueName;
    }

    [Serializable]
    public class ListMatchmakingTicketsForPlayerResult : PlayFabResultCommon
    {
        public List<string> TicketIds;
    }
    [Serializable]
    public class ListMultiplayerServersRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string Region;
        public string SkipToken;
    }

    [Serializable]
    public class ListMultiplayerServersResponse : PlayFabResultCommon
    {
        public List<MultiplayerServerSummary> MultiplayerServerSummaries;
        public int PageSize;
        public string SkipToken;
    }
    [Serializable]
    public class ListPartyQosServersRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListPartyQosServersResponse : PlayFabResultCommon
    {
        public int PageSize;
        public List<QosServer> QosServers;
        public string SkipToken;
    }
    [Serializable]
    public class ListQosServersForTitleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? IncludeAllRegions;
        public string RoutingPreference;
    }

    [Serializable]
    public class ListQosServersForTitleResponse : PlayFabResultCommon
    {
        public int PageSize;
        public List<QosServer> QosServers;
        public string SkipToken;
    }
    [Serializable]
    public class ListSecretSummariesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string SkipToken;
    }

    [Serializable]
    public class ListSecretSummariesResponse : PlayFabResultCommon
    {
        public int PageSize;
        public List<SecretSummary> SecretSummaries;
        public string SkipToken;
    }
    [Serializable]
    public class ListServerBackfillTicketsForPlayerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string QueueName;
    }

    [Serializable]
    public class ListServerBackfillTicketsForPlayerResult : PlayFabResultCommon
    {
        public List<string> TicketIds;
    }
    [Serializable]
    public class ListTitleMultiplayerServersQuotaChangesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListTitleMultiplayerServersQuotaChangesResponse : PlayFabResultCommon
    {
        public List<QuotaChange> Changes;
    }
    [Serializable]
    public class ListVirtualMachineSummariesRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public int? PageSize;
        public string Region;
        public string SkipToken;
    }

    [Serializable]
    public class ListVirtualMachineSummariesResponse : PlayFabResultCommon
    {
        public int PageSize;
        public string SkipToken;
        public List<VirtualMachineSummary> VirtualMachines;
    }

    [Serializable]
    public class Lobby : PlayFabBaseModel
    {
        public AccessPolicy AccessPolicy;
        public uint ChangeNumber;
        public string ConnectionString;
        public Dictionary<string,string> LobbyData;
        public string LobbyId;
        public uint MaxPlayers;
        public List<Member> Members;
        public MembershipLock MembershipLock;
        public EntityKey Owner;
        public OwnerMigrationPolicy? OwnerMigrationPolicy;
        public string PubSubConnectionHandle;
        public bool RestrictInvitesToLobbyOwner;
        public Dictionary<string,string> SearchData;
        public LobbyServer Server;
        public bool UseConnections;
    }

    [Serializable]
    public class LobbyEmptyResult : PlayFabResultCommon
    {
    }

    [Serializable]
    public class LobbyServer : PlayFabBaseModel
    {
        public string PubSubConnectionHandle;
        public Dictionary<string,string> ServerData;
        public EntityKey ServerEntity;
    }

    [Serializable]
    public class LobbySummary : PlayFabBaseModel
    {
        public string ConnectionString;
        public uint CurrentPlayers;
        public string LobbyId;
        public uint MaxPlayers;
        public MembershipLock? MembershipLock;
        public EntityKey Owner;
        public Dictionary<string,string> SearchData;
    }
    [Serializable]
    public class MatchmakingPlayer : PlayFabBaseModel
    {
        public MatchmakingPlayerAttributes Attributes;
        public EntityKey Entity;
    }
    [Serializable]
    public class MatchmakingPlayerAttributes : PlayFabBaseModel
    {
        public object DataObject;
        public string EscapedDataObject;
    }
    [Serializable]
    public class MatchmakingPlayerWithTeamAssignment : PlayFabBaseModel
    {
        public MatchmakingPlayerAttributes Attributes;
        public EntityKey Entity;
        public string TeamId;
    }

    [Serializable]
    public class MatchmakingQueueConfig : PlayFabBaseModel
    {
        public BuildAliasParams BuildAliasParams;
        public string BuildId;
        public List<DifferenceRule> DifferenceRules;
        public List<MatchTotalRule> MatchTotalRules;
        public uint MaxMatchSize;
        public uint? MaxTicketSize;
        public uint MinMatchSize;
        public string Name;
        public RegionSelectionRule RegionSelectionRule;
        public bool ServerAllocationEnabled;
        public List<SetIntersectionRule> SetIntersectionRules;
        public StatisticsVisibilityToPlayers StatisticsVisibilityToPlayers;
        public List<StringEqualityRule> StringEqualityRules;
        public List<TeamDifferenceRule> TeamDifferenceRules;
        public List<MatchmakingQueueTeam> Teams;
        public TeamSizeBalanceRule TeamSizeBalanceRule;
        public TeamTicketSizeSimilarityRule TeamTicketSizeSimilarityRule;
    }

    [Serializable]
    public class MatchmakingQueueTeam : PlayFabBaseModel
    {
        public uint MaxTeamSize;
        public uint MinTeamSize;
        public string Name;
    }

    [Serializable]
    public class MatchTotalRule : PlayFabBaseModel
    {
        public QueueRuleAttribute Attribute;
        public MatchTotalRuleExpansion Expansion;
        public double Max;
        public double Min;
        public string Name;
        public uint? SecondsUntilOptional;
        public double Weight;
    }

    [Serializable]
    public class MatchTotalRuleExpansion : PlayFabBaseModel
    {
        public List<OverrideDouble> MaxOverrides;
        public List<OverrideDouble> MinOverrides;
        public uint SecondsBetweenExpansions;
    }

    [Serializable]
    public class Member : PlayFabBaseModel
    {
        public Dictionary<string,string> MemberData;
        public EntityKey MemberEntity;
        public string PubSubConnectionHandle;
    }

    public enum MembershipLock
    {
        Unlocked,
        Locked
    }

    [Serializable]
    public class MonitoringApplicationConfiguration : PlayFabBaseModel
    {
        public AssetReference AssetReference;
        public string ExecutionScriptName;
        public string InstallationScriptName;
        public double? OnStartRuntimeInMinutes;
    }

    [Serializable]
    public class MonitoringApplicationConfigurationParams : PlayFabBaseModel
    {
        public AssetReferenceParams AssetReference;
        public string ExecutionScriptName;
        public string InstallationScriptName;
        public double? OnStartRuntimeInMinutes;
    }

    [Serializable]
    public class MultiplayerServerSummary : PlayFabBaseModel
    {
        public List<ConnectedPlayer> ConnectedPlayers;
        public DateTime? LastStateTransitionTime;
        public string Region;
        public string ServerId;
        public string SessionId;
        public string State;
        public string VmId;
    }

    public enum OsPlatform
    {
        Windows,
        Linux
    }

    [Serializable]
    public class OverrideDouble : PlayFabBaseModel
    {
        public double Value;
    }

    [Serializable]
    public class OverrideUnsignedInt : PlayFabBaseModel
    {
        public uint Value;
    }

    public enum OwnerMigrationPolicy
    {
        None,
        Automatic,
        Manual,
        Server
    }

    [Serializable]
    public class PaginationRequest : PlayFabBaseModel
    {
        public string ContinuationToken;
        public uint? PageSizeRequested;
    }

    [Serializable]
    public class PaginationResponse : PlayFabBaseModel
    {
        public string ContinuationToken;
        public uint? TotalMatchedLobbyCount;
    }

    [Serializable]
    public class PartyInvitationConfiguration : PlayFabBaseModel
    {
        public List<EntityKey> EntityKeys;
        public string Identifier;
        public string Revocability;
    }

    public enum PartyInvitationRevocability
    {
        Creator,
        Anyone
    }

    [Serializable]
    public class PartyNetworkConfiguration : PlayFabBaseModel
    {
        public string DirectPeerConnectivityOptions;
        public uint MaxDevices;
        public uint MaxDevicesPerUser;
        public uint MaxEndpointsPerDevice;
        public uint MaxUsers;
        public uint MaxUsersPerDevice;
        public PartyInvitationConfiguration PartyInvitationConfiguration;
    }

    [Serializable]
    public class Port : PlayFabBaseModel
    {
        public string Name;
        public int Num;
        public ProtocolType Protocol;
    }

    public enum ProtocolType
    {
        TCP,
        UDP
    }

    [Serializable]
    public class PublicIpAddress : PlayFabBaseModel
    {
        public string FQDN;
        public string IpAddress;
        public string RoutingType;
    }

    [Serializable]
    public class QosServer : PlayFabBaseModel
    {
        public string Region;
        public string ServerUrl;
    }

    [Serializable]
    public class QueueRuleAttribute : PlayFabBaseModel
    {
        public string Path;
        public AttributeSource Source;
    }

    [Serializable]
    public class QuotaChange : PlayFabBaseModel
    {
        public string ChangeDescription;
        public List<CoreCapacityChange> Changes;
        public bool IsPendingReview;
        public string Notes;
        public string RequestId;
        public string ReviewComments;
        public bool WasApproved;
    }

    [Serializable]
    public class RegionSelectionRule : PlayFabBaseModel
    {
        public CustomRegionSelectionRuleExpansion CustomExpansion;
        public LinearRegionSelectionRuleExpansion LinearExpansion;
        public uint MaxLatency;
        public string Name;
        public string Path;
        public uint? SecondsUntilOptional;
        public double Weight;
    }
    [Serializable]
    public class RemoveMatchmakingQueueRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string QueueName;
    }

    [Serializable]
    public class RemoveMatchmakingQueueResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class RemoveMemberFromLobbyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public EntityKey MemberEntity;
        public bool PreventRejoin;
    }
    [Serializable]
    public class RequestMultiplayerServerRequest : PlayFabRequestCommon
    {
        public BuildAliasParams BuildAliasParams;
        public string BuildId;
        public Dictionary<string,string> CustomTags;
        public List<string> InitialPlayers;
        public List<string> PreferredRegions;
        public string SessionCookie;
        public string SessionId;
    }

    [Serializable]
    public class RequestMultiplayerServerResponse : PlayFabResultCommon
    {
        public string BuildId;
        public List<ConnectedPlayer> ConnectedPlayers;
        public string FQDN;
        public string IPV4Address;
        public DateTime? LastStateTransitionTime;
        public List<Port> Ports;
        public List<PublicIpAddress> PublicIPV4Addresses;
        public string Region;
        public string ServerId;
        public string SessionId;
        public string State;
        public string VmId;
    }
    [Serializable]
    public class RequestPartyServiceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public PartyNetworkConfiguration NetworkConfiguration;
        public string PartyId;
        public List<string> PreferredRegions;
    }

    [Serializable]
    public class RequestPartyServiceResponse : PlayFabResultCommon
    {
        public string InvitationId;
        public string PartyId;
        public string SerializedNetworkDescriptor;
    }
    [Serializable]
    public class RolloverContainerRegistryCredentialsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class RolloverContainerRegistryCredentialsResponse : PlayFabResultCommon
    {
        public string DnsName;
        public string Password;
        public string Username;
    }

    public enum RoutingType
    {
        Microsoft,
        Internet
    }

    [Serializable]
    public class Schedule : PlayFabBaseModel
    {
        public string Description;
        public DateTime EndTime;
        public bool IsDisabled;
        public bool IsRecurringWeekly;
        public DateTime StartTime;
        public int TargetStandby;
    }

    [Serializable]
    public class ScheduledStandbySettings : PlayFabBaseModel
    {
        public bool IsEnabled;
        public List<Schedule> ScheduleList;
    }

    [Serializable]
    public class Secret : PlayFabBaseModel
    {
        public DateTime? ExpirationDate;
        public string Name;
        public string Value;
    }

    [Serializable]
    public class SecretSummary : PlayFabBaseModel
    {
        public DateTime? ExpirationDate;
        public string Name;
        public string Version;
    }

    [Serializable]
    public class ServerDetails : PlayFabBaseModel
    {
        public string Fqdn;
        public string IPV4Address;
        public List<Port> Ports;
        public string Region;
        public string ServerId;
    }

    [Serializable]
    public class ServerResourceConstraintParams : PlayFabBaseModel
    {
        public double CpuLimit;
        public double MemoryLimitGB;
    }

    public enum ServerType
    {
        Container,
        Process
    }

    [Serializable]
    public class SetIntersectionRule : PlayFabBaseModel
    {
        public QueueRuleAttribute Attribute;
        public AttributeNotSpecifiedBehavior AttributeNotSpecifiedBehavior;
        public CustomSetIntersectionRuleExpansion CustomExpansion;
        public List<string> DefaultAttributeValue;
        public LinearSetIntersectionRuleExpansion LinearExpansion;
        public uint MinIntersectionSize;
        public string Name;
        public uint? SecondsUntilOptional;
        public double Weight;
    }
    [Serializable]
    public class SetMatchmakingQueueRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public MatchmakingQueueConfig MatchmakingQueue;
    }

    [Serializable]
    public class SetMatchmakingQueueResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class ShutdownMultiplayerServerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string SessionId;
    }

    [Serializable]
    public class Statistics : PlayFabBaseModel
    {
        public double Average;
        public double Percentile50;
        public double Percentile90;
        public double Percentile99;
    }

    [Serializable]
    public class StatisticsVisibilityToPlayers : PlayFabBaseModel
    {
        public bool ShowNumberOfPlayersMatching;
        public bool ShowTimeToMatch;
    }

    [Serializable]
    public class StringEqualityRule : PlayFabBaseModel
    {
        public QueueRuleAttribute Attribute;
        public AttributeNotSpecifiedBehavior AttributeNotSpecifiedBehavior;
        public string DefaultAttributeValue;
        public StringEqualityRuleExpansion Expansion;
        public string Name;
        public uint? SecondsUntilOptional;
        public double Weight;
    }

    [Serializable]
    public class StringEqualityRuleExpansion : PlayFabBaseModel
    {
        public List<bool> EnabledOverrides;
        public uint SecondsBetweenExpansions;
    }
    [Serializable]
    public class SubscribeToLobbyResourceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey EntityKey;
        public string PubSubConnectionHandle;
        public string ResourceId;
        public uint SubscriptionVersion;
        public SubscriptionType Type;
    }

    [Serializable]
    public class SubscribeToLobbyResourceResult : PlayFabResultCommon
    {
        public string Topic;
    }
    [Serializable]
    public class SubscribeToMatchResourceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey EntityKey;
        public string PubSubConnectionHandle;
        public string ResourceId;
        public uint SubscriptionVersion;
        public string Type;
    }

    [Serializable]
    public class SubscribeToMatchResourceResult : PlayFabResultCommon
    {
        public string Topic;
    }

    public enum SubscriptionType
    {
        LobbyChange,
        LobbyInvite
    }

    [Serializable]
    public class TeamDifferenceRule : PlayFabBaseModel
    {
        public QueueRuleAttribute Attribute;
        public CustomTeamDifferenceRuleExpansion CustomExpansion;
        public double DefaultAttributeValue;
        public double Difference;
        public LinearTeamDifferenceRuleExpansion LinearExpansion;
        public string Name;
        public uint? SecondsUntilOptional;
    }

    [Serializable]
    public class TeamSizeBalanceRule : PlayFabBaseModel
    {
        public CustomTeamSizeBalanceRuleExpansion CustomExpansion;
        public uint Difference;
        public LinearTeamSizeBalanceRuleExpansion LinearExpansion;
        public string Name;
        public uint? SecondsUntilOptional;
    }

    [Serializable]
    public class TeamTicketSizeSimilarityRule : PlayFabBaseModel
    {
        public string Name;
        public uint? SecondsUntilOptional;
    }

    public enum TitleMultiplayerServerEnabledStatus
    {
        Initializing,
        Enabled,
        Disabled
    }

    [Serializable]
    public class TitleMultiplayerServersQuotas : PlayFabBaseModel
    {
        public List<CoreCapacity> CoreCapacities;
    }
    [Serializable]
    public class UnsubscribeFromLobbyResourceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey EntityKey;
        public string PubSubConnectionHandle;
        public string ResourceId;
        public uint SubscriptionVersion;
        public SubscriptionType Type;
    }
    [Serializable]
    public class UnsubscribeFromMatchResourceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey EntityKey;
        public string PubSubConnectionHandle;
        public string ResourceId;
        public uint SubscriptionVersion;
        public string Type;
    }

    [Serializable]
    public class UnsubscribeFromMatchResourceResult : PlayFabResultCommon
    {
    }
    [Serializable]
    public class UntagContainerImageRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ImageName;
        public string Tag;
    }
    [Serializable]
    public class UpdateBuildAliasRequest : PlayFabRequestCommon
    {
        public string AliasId;
        public string AliasName;
        public List<BuildSelectionCriterion> BuildSelectionCriteria;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class UpdateBuildNameRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public string BuildName;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class UpdateBuildRegionRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public BuildRegionParams BuildRegion;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class UpdateBuildRegionsRequest : PlayFabRequestCommon
    {
        public string BuildId;
        public List<BuildRegionParams> BuildRegions;
        public Dictionary<string,string> CustomTags;
    }
    [Serializable]
    public class UpdateLobbyAsServerRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LobbyId;
        public Dictionary<string,string> ServerData;
        public List<string> ServerDataToDelete;
        public EntityKey ServerEntity;
    }
    [Serializable]
    public class UpdateLobbyRequest : PlayFabRequestCommon
    {
        public AccessPolicy? AccessPolicy;
        public Dictionary<string,string> CustomTags;
        public Dictionary<string,string> LobbyData;
        public List<string> LobbyDataToDelete;
        public string LobbyId;
        public uint? MaxPlayers;
        public Dictionary<string,string> MemberData;
        public List<string> MemberDataToDelete;
        public EntityKey MemberEntity;
        public MembershipLock? MembershipLock;
        public EntityKey Owner;
        public Dictionary<string,string> SearchData;
        public List<string> SearchDataToDelete;
    }
    [Serializable]
    public class UploadCertificateRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceUpdate;
        public Certificate GameCertificate;
    }
    [Serializable]
    public class UploadSecretRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? ForceUpdate;
        public Secret GameSecret;
    }

    [Serializable]
    public class VirtualMachineSummary : PlayFabBaseModel
    {
        public string HealthStatus;
        public string State;
        public string VmId;
    }

    [Serializable]
    public class VmStartupScriptConfiguration : PlayFabBaseModel
    {
        public List<VmStartupScriptPortRequest> PortRequests;
        public AssetReference VmStartupScriptAssetReference;
    }

    [Serializable]
    public class VmStartupScriptParams : PlayFabBaseModel
    {
        public List<VmStartupScriptPortRequestParams> PortRequests;
        public AssetReferenceParams VmStartupScriptAssetReference;
    }

    [Serializable]
    public class VmStartupScriptPortRequest : PlayFabBaseModel
    {
        public string Name;
        public ProtocolType Protocol;
    }

    [Serializable]
    public class VmStartupScriptPortRequestParams : PlayFabBaseModel
    {
        public string Name;
        public ProtocolType Protocol;
    }

    [Serializable]
    public class WindowsCrashDumpConfiguration : PlayFabBaseModel
    {
        public int? CustomDumpFlags;
        public int? DumpType;
        public bool IsEnabled;
    }
}
#endif
