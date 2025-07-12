#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.EventsModels
{
    [Serializable]
    public class CreateTelemetryKeyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string KeyName;
    }

    [Serializable]
    public class CreateTelemetryKeyResponse : PlayFabResultCommon
    {
        public TelemetryKeyDetails NewKeyDetails;
    }

    [Serializable]
    public class DeleteTelemetryKeyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string KeyName;
    }

    [Serializable]
    public class DeleteTelemetryKeyResponse : PlayFabResultCommon
    {
        public bool WasKeyDeleted;
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }

    [Serializable]
    public class EventContents : PlayFabBaseModel
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string EventNamespace;
        public string Name;
        public string OriginalId;
        public DateTime? OriginalTimestamp;
        public object Payload;
        public string PayloadJSON;
    }

    [Serializable]
    public class GetTelemetryKeyRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string KeyName;
    }

    [Serializable]
    public class GetTelemetryKeyResponse : PlayFabResultCommon
    {
        public TelemetryKeyDetails KeyDetails;
    }

    [Serializable]
    public class ListTelemetryKeysRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class ListTelemetryKeysResponse : PlayFabResultCommon
    {
        public List<TelemetryKeyDetails> KeyDetails;
    }

    [Serializable]
    public class SetTelemetryKeyActiveRequest : PlayFabRequestCommon
    {
        public bool Active;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string KeyName;
    }

    [Serializable]
    public class SetTelemetryKeyActiveResponse : PlayFabResultCommon
    {
        public TelemetryKeyDetails KeyDetails;
        public bool WasKeyUpdated;
    }

    [Serializable]
    public class TelemetryKeyDetails : PlayFabBaseModel
    {
        public DateTime CreateTime;
        public bool IsActive;
        public string KeyValue;
        public DateTime LastUpdateTime;
        public string Name;
    }

    [Serializable]
    public class WriteEventsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public List<EventContents> Events;
    }

    [Serializable]
    public class WriteEventsResponse : PlayFabResultCommon
    {
        public List<string> AssignedEventIds;
    }
}
#endif
