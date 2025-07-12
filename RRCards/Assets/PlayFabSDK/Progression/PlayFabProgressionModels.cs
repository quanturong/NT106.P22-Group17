#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.ProgressionModels
{
    [Serializable]
    public class CreateLeaderboardDefinitionRequest : PlayFabRequestCommon
    {
        public List<LeaderboardColumn> Columns;
        public Dictionary<string,string> CustomTags;
        public string EntityType;
        public string Name;
        public int SizeLimit;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class CreateStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public List<StatisticColumn> Columns;
        public Dictionary<string,string> CustomTags;
        public string EntityType;
        public string Name;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class DeleteLeaderboardDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class DeleteLeaderboardEntriesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<string> EntityIds;
        public string Name;
    }

    [Serializable]
    public class DeleteStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class DeleteStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<StatisticDelete> Statistics;
    }

    [Serializable]
    public class DeleteStatisticsResponse : PlayFabResultCommon
    {
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
    public class EntityLeaderboardEntry : PlayFabBaseModel
    {
        public string DisplayName;
        public EntityKey Entity;
        public DateTime LastUpdated;
        public string Metadata;
        public int Rank;
        public List<string> Scores;
    }

    [Serializable]
    public class EntityStatistics : PlayFabBaseModel
    {
        public EntityKey EntityKey;
        public List<EntityStatisticValue> Statistics;
    }

    [Serializable]
    public class EntityStatisticValue : PlayFabBaseModel
    {
        public string Metadata;
        public string Name;
        public List<string> Scores;
        public int Version;
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
    public class GetEntityLeaderboardRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string LeaderboardName;
        public uint PageSize;
        public uint? StartingPosition;
        public uint? Version;
    }
    [Serializable]
    public class GetEntityLeaderboardResponse : PlayFabResultCommon
    {
        public List<LeaderboardColumn> Columns;
        public uint EntryCount;
        public DateTime? NextReset;
        public List<EntityLeaderboardEntry> Rankings;
        public uint Version;
    }

    [Serializable]
    public class GetFriendLeaderboardForEntityRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public ExternalFriendSources? ExternalFriendSources;
        public string LeaderboardName;
        public uint? Version;
        public string XboxToken;
    }
    [Serializable]
    public class GetLeaderboardAroundEntityRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string LeaderboardName;
        public uint MaxSurroundingEntries;
        public uint? Version;
    }

    [Serializable]
    public class GetLeaderboardDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class GetLeaderboardDefinitionResponse : PlayFabResultCommon
    {
        public List<LeaderboardColumn> Columns;
        public DateTime Created;
        public string EntityType;
        public DateTime? LastResetTime;
        public string Name;
        public int SizeLimit;
        public uint Version;
        public VersionConfiguration VersionConfiguration;
    }
    [Serializable]
    public class GetLeaderboardForEntitiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<string> EntityIds;
        public string LeaderboardName;
        public uint? Version;
    }

    [Serializable]
    public class GetStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class GetStatisticDefinitionResponse : PlayFabResultCommon
    {
        public List<StatisticColumn> Columns;
        public DateTime Created;
        public string EntityType;
        public DateTime? LastResetTime;
        public List<string> LinkedLeaderboardNames;
        public string Name;
        public uint Version;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class GetStatisticsForEntitiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<EntityKey> Entities;
    }

    [Serializable]
    public class GetStatisticsForEntitiesResponse : PlayFabResultCommon
    {
        public Dictionary<string,StatisticColumnCollection> ColumnDetails;
        public List<EntityStatistics> EntitiesStatistics;
    }

    [Serializable]
    public class GetStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetStatisticsResponse : PlayFabResultCommon
    {
        public Dictionary<string,StatisticColumnCollection> ColumnDetails;
        public EntityKey Entity;
        public Dictionary<string,EntityStatisticValue> Statistics;
    }

    [Serializable]
    public class IncrementLeaderboardVersionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class IncrementLeaderboardVersionResponse : PlayFabResultCommon
    {
        public uint Version;
    }

    [Serializable]
    public class IncrementStatisticVersionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
    }

    [Serializable]
    public class IncrementStatisticVersionResponse : PlayFabResultCommon
    {
        public uint Version;
    }

    [Serializable]
    public class LeaderboardColumn : PlayFabBaseModel
    {
        public LinkedStatisticColumn LinkedStatisticColumn;
        public string Name;
        public LeaderboardSortDirection SortDirection;
    }

    [Serializable]
    public class LeaderboardDefinition : PlayFabBaseModel
    {
        public List<LeaderboardColumn> Columns;
        public DateTime Created;
        public string EntityType;
        public DateTime? LastResetTime;
        public string Name;
        public int SizeLimit;
        public uint Version;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class LeaderboardEntryUpdate : PlayFabBaseModel
    {
        public string EntityId;
        public string Metadata;
        public List<string> Scores;
    }

    public enum LeaderboardSortDirection
    {
        Descending,
        Ascending
    }

    [Serializable]
    public class LinkedStatisticColumn : PlayFabBaseModel
    {
        public string LinkedStatisticColumnName;
        public string LinkedStatisticName;
    }

    [Serializable]
    public class ListLeaderboardDefinitionsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListLeaderboardDefinitionsResponse : PlayFabResultCommon
    {
        public List<LeaderboardDefinition> LeaderboardDefinitions;
    }

    [Serializable]
    public class ListStatisticDefinitionsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class ListStatisticDefinitionsResponse : PlayFabResultCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<StatisticDefinition> StatisticDefinitions;
    }

    public enum ResetInterval
    {
        Manual,
        Hour,
        Day,
        Week,
        Month
    }

    public enum StatisticAggregationMethod
    {
        Last,
        Min,
        Max,
        Sum
    }

    [Serializable]
    public class StatisticColumn : PlayFabBaseModel
    {
        public StatisticAggregationMethod AggregationMethod;
        public string Name;
    }

    [Serializable]
    public class StatisticColumnCollection : PlayFabBaseModel
    {
        public List<StatisticColumn> Columns;
    }

    [Serializable]
    public class StatisticDefinition : PlayFabBaseModel
    {
        public List<StatisticColumn> Columns;
        public DateTime Created;
        public string EntityType;
        public DateTime? LastResetTime;
        public List<string> LinkedLeaderboardNames;
        public string Name;
        public uint Version;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class StatisticDelete : PlayFabBaseModel
    {
        public string Name;
    }

    [Serializable]
    public class StatisticUpdate : PlayFabBaseModel
    {
        public string Metadata;
        public string Name;
        public List<string> Scores;
        public uint? Version;
    }

    [Serializable]
    public class UnlinkLeaderboardFromStatisticRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
        public string StatisticName;
    }

    [Serializable]
    public class UpdateLeaderboardDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
        public int? SizeLimit;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class UpdateLeaderboardEntriesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<LeaderboardEntryUpdate> Entries;
        public string LeaderboardName;
    }

    [Serializable]
    public class UpdateStatisticDefinitionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Name;
        public VersionConfiguration VersionConfiguration;
    }

    [Serializable]
    public class UpdateStatisticsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<StatisticUpdate> Statistics;
        public string TransactionId;
    }

    [Serializable]
    public class UpdateStatisticsResponse : PlayFabResultCommon
    {
        public Dictionary<string,StatisticColumnCollection> ColumnDetails;
        public EntityKey Entity;
        public Dictionary<string,EntityStatisticValue> Statistics;
    }

    [Serializable]
    public class VersionConfiguration : PlayFabBaseModel
    {
        public int MaxQueryableVersions;
        public ResetInterval ResetInterval;
    }
}
#endif
