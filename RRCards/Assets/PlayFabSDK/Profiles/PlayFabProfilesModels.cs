#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.ProfilesModels
{
    public enum EffectType
    {
        Allow,
        Deny
    }
    [Serializable]
    public class EntityDataObject : PlayFabBaseModel
    {
        public object DataObject;
        public string EscapedDataObject;
        public string ObjectName;
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
    public class EntityPermissionStatement : PlayFabBaseModel
    {
        public string Action;
        public string Comment;
        public object Condition;
        public EffectType Effect;
        public object Principal;
        public string Resource;
    }

    [Serializable]
    public class EntityProfileBody : PlayFabBaseModel
    {
        public string AvatarUrl;
        public DateTime Created;
        public string DisplayName;
        public EntityKey Entity;
        public string EntityChain;
        public List<string> ExperimentVariants;
        public Dictionary<string,EntityProfileFileMetadata> Files;
        public string Language;
        public EntityLineage Lineage;
        public Dictionary<string,EntityDataObject> Objects;
        public List<EntityPermissionStatement> Permissions;
        public Dictionary<string,EntityStatisticValue> Statistics;
        public int VersionNumber;
    }
    [Serializable]
    public class EntityProfileFileMetadata : PlayFabBaseModel
    {
        public string Checksum;
        public string FileName;
        public DateTime LastModified;
        public int Size;
    }

    [Serializable]
    public class EntityStatisticValue : PlayFabBaseModel
    {
        public string Metadata;
        public string Name;
        public List<string> Scores;
        public int Version;
    }
    [Serializable]
    public class GetEntityProfileRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? DataAsObject;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetEntityProfileResponse : PlayFabResultCommon
    {
        public EntityProfileBody Profile;
    }
    [Serializable]
    public class GetEntityProfilesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public bool? DataAsObject;
        public List<EntityKey> Entities;
    }

    [Serializable]
    public class GetEntityProfilesResponse : PlayFabResultCommon
    {
        public List<EntityProfileBody> Profiles;
    }
    [Serializable]
    public class GetGlobalPolicyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetGlobalPolicyResponse : PlayFabResultCommon
    {
        public List<EntityPermissionStatement> Permissions;
    }
    [Serializable]
    public class GetTitlePlayersFromMasterPlayerAccountIdsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<string> MasterPlayerAccountIds;
        public string TitleId;
    }

    [Serializable]
    public class GetTitlePlayersFromMasterPlayerAccountIdsResponse : PlayFabResultCommon
    {
        public string TitleId;
        public Dictionary<string,EntityKey> TitlePlayerAccounts;
    }

    [Serializable]
    public class GetTitlePlayersFromProviderIDsResponse : PlayFabResultCommon
    {
        public Dictionary<string,EntityLineage> TitlePlayerAccounts;
    }
    [Serializable]
    public class GetTitlePlayersFromXboxLiveIDsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Sandbox;
        public string TitleId;
        public List<string> XboxLiveIds;
    }

    public enum OperationTypes
    {
        Created,
        Updated,
        Deleted,
        None
    }
    [Serializable]
    public class SetDisplayNameRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DisplayName;
        public EntityKey Entity;
        public int? ExpectedVersion;
    }

    [Serializable]
    public class SetDisplayNameResponse : PlayFabResultCommon
    {
        public OperationTypes? OperationResult;
        public int? VersionNumber;
    }
    [Serializable]
    public class SetEntityProfilePolicyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<EntityPermissionStatement> Statements;
    }

    [Serializable]
    public class SetEntityProfilePolicyResponse : PlayFabResultCommon
    {
        public List<EntityPermissionStatement> Permissions;
    }
    [Serializable]
    public class SetGlobalPolicyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<EntityPermissionStatement> Permissions;
    }

    [Serializable]
    public class SetGlobalPolicyResponse : PlayFabResultCommon
    {
    }
    [Serializable]
    public class SetProfileLanguageRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public int? ExpectedVersion;
        public string Language;
    }

    [Serializable]
    public class SetProfileLanguageResponse : PlayFabResultCommon
    {
        public OperationTypes? OperationResult;
        public int? VersionNumber;
    }
}
#endif
