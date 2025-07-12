#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.ExperimentationModels
{
    public enum AnalysisTaskState
    {
        Waiting,
        ReadyForSubmission,
        SubmittingToPipeline,
        Running,
        Completed,
        Failed,
        Canceled
    }
    [Serializable]
    public class CreateExclusionGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public string Name;
    }

    [Serializable]
    public class CreateExclusionGroupResult : PlayFabResultCommon
    {
        public string ExclusionGroupId;
    }
    [Serializable]
    public class CreateExperimentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public DateTime? EndDate;
        public string ExclusionGroupId;
        public uint? ExclusionGroupTrafficAllocation;
        public ExperimentType? ExperimentType;
        public string Name;
        public string SegmentId;
        public DateTime StartDate;
        public List<string> TitlePlayerAccountTestIds;
        public List<Variant> Variants;
    }

    [Serializable]
    public class CreateExperimentResult : PlayFabResultCommon
    {
        public string ExperimentId;
    }
    [Serializable]
    public class DeleteExclusionGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExclusionGroupId;
    }
    [Serializable]
    public class DeleteExperimentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExperimentId;
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
    public class ExclusionGroupTrafficAllocation : PlayFabBaseModel
    {
        public string ExperimentId;
        public uint TrafficAllocation;
    }

    [Serializable]
    public class Experiment : PlayFabBaseModel
    {
        public string Description;
        public DateTime? EndDate;
        public string ExclusionGroupId;
        public uint? ExclusionGroupTrafficAllocation;
        public ExperimentType? ExperimentType;
        public string Id;
        public string Name;
        public string SegmentId;
        public DateTime StartDate;
        public ExperimentState? State;
        public List<string> TitlePlayerAccountTestIds;
        public List<Variant> Variants;
    }

    [Serializable]
    public class ExperimentExclusionGroup : PlayFabBaseModel
    {
        public string Description;
        public string ExclusionGroupId;
        public string Name;
    }

    public enum ExperimentState
    {
        New,
        Started,
        Stopped,
        Deleted
    }

    public enum ExperimentType
    {
        Active,
        Snapshot
    }
    [Serializable]
    public class GetExclusionGroupsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetExclusionGroupsResult : PlayFabResultCommon
    {
        public List<ExperimentExclusionGroup> ExclusionGroups;
    }
    [Serializable]
    public class GetExclusionGroupTrafficRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExclusionGroupId;
    }

    [Serializable]
    public class GetExclusionGroupTrafficResult : PlayFabResultCommon
    {
        public List<ExclusionGroupTrafficAllocation> TrafficAllocations;
    }
    [Serializable]
    public class GetExperimentsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
    }

    [Serializable]
    public class GetExperimentsResult : PlayFabResultCommon
    {
        public List<Experiment> Experiments;
    }
    [Serializable]
    public class GetLatestScorecardRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExperimentId;
    }

    [Serializable]
    public class GetLatestScorecardResult : PlayFabResultCommon
    {
        public Scorecard Scorecard;
    }
    [Serializable]
    public class GetTreatmentAssignmentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetTreatmentAssignmentResult : PlayFabResultCommon
    {
        public TreatmentAssignment TreatmentAssignment;
    }

    [Serializable]
    public class MetricData : PlayFabBaseModel
    {
        public double ConfidenceIntervalEnd;
        public double ConfidenceIntervalStart;
        public float DeltaAbsoluteChange;
        public float DeltaRelativeChange;
        public string InternalName;
        public string Movement;
        public string Name;
        public float PMove;
        public float PValue;
        public float PValueThreshold;
        public string StatSigLevel;
        public float StdDev;
        public float Value;
    }

    [Serializable]
    public class Scorecard : PlayFabBaseModel
    {
        public string DateGenerated;
        public string Duration;
        public double EventsProcessed;
        public string ExperimentId;
        public string ExperimentName;
        public AnalysisTaskState? LatestJobStatus;
        public bool SampleRatioMismatch;
        public List<ScorecardDataRow> ScorecardDataRows;
    }

    [Serializable]
    public class ScorecardDataRow : PlayFabBaseModel
    {
        public bool IsControl;
        public Dictionary<string,MetricData> MetricDataRows;
        public uint PlayerCount;
        public string VariantName;
    }
    [Serializable]
    public class StartExperimentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExperimentId;
    }
    [Serializable]
    public class StopExperimentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string ExperimentId;
    }

    [Serializable]
    public class TreatmentAssignment : PlayFabBaseModel
    {
        public List<Variable> Variables;
        public List<string> Variants;
    }
    [Serializable]
    public class UpdateExclusionGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public string ExclusionGroupId;
        public string Name;
    }
    [Serializable]
    public class UpdateExperimentRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string Description;
        public DateTime? EndDate;
        public string ExclusionGroupId;
        public uint? ExclusionGroupTrafficAllocation;
        public ExperimentType? ExperimentType;
        public string Id;
        public string Name;
        public string SegmentId;
        public DateTime StartDate;
        public List<string> TitlePlayerAccountTestIds;
        public List<Variant> Variants;
    }

    [Serializable]
    public class Variable : PlayFabBaseModel
    {
        public string Name;
        public string Value;
    }

    [Serializable]
    public class Variant : PlayFabBaseModel
    {
        public string Description;
        public string Id;
        public bool IsControl;
        public string Name;
        public string TitleDataOverrideLabel;
        public uint TrafficPercentage;
        public List<Variable> Variables;
    }
}
#endif
