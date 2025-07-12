#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.InsightsModels
{
    [Serializable]
    public class InsightsEmptyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class InsightsGetDetailsResponse : PlayFabResultCommon
    {
        public uint DataUsageMb;
        public string ErrorMessage;
        public InsightsGetLimitsResponse Limits;
        public List<InsightsGetOperationStatusResponse> PendingOperations;
        public int PerformanceLevel;
        public int RetentionDays;
    }

    [Serializable]
    public class InsightsGetLimitsResponse : PlayFabResultCommon
    {
        public int DefaultPerformanceLevel;
        public int DefaultStorageRetentionDays;
        public int StorageMaxRetentionDays;
        public int StorageMinRetentionDays;
        public List<InsightsPerformanceLevel> SubMeters;
    }
    [Serializable]
    public class InsightsGetOperationStatusRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string OperationId;
    }

    [Serializable]
    public class InsightsGetOperationStatusResponse : PlayFabResultCommon
    {
        public string Message;
        public DateTime OperationCompletedTime;
        public string OperationId;
        public DateTime OperationLastUpdated;
        public DateTime OperationStartedTime;
        public string OperationType;
        public int OperationValue;
        public string Status;
    }
    [Serializable]
    public class InsightsGetPendingOperationsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string OperationType;
    }

    [Serializable]
    public class InsightsGetPendingOperationsResponse : PlayFabResultCommon
    {
        public List<InsightsGetOperationStatusResponse> PendingOperations;
    }

    [Serializable]
    public class InsightsOperationResponse : PlayFabResultCommon
    {
        public string Message;
        public string OperationId;
        public string OperationType;
    }

    [Serializable]
    public class InsightsPerformanceLevel : PlayFabBaseModel
    {
        public int ActiveEventExports;
        public int CacheSizeMB;
        public int Concurrency;
        public double CreditsPerMinute;
        public int EventsPerSecond;
        public int Level;
        public int MaxMemoryPerQueryMB;
        public int VirtualCpuCores;
    }
    [Serializable]
    public class InsightsSetPerformanceRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int PerformanceLevel;
    }
    [Serializable]
    public class InsightsSetStorageRetentionRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int RetentionDays;
    }
}
#endif
