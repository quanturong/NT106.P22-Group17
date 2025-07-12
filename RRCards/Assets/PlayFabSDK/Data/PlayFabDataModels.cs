#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.DataModels
{
    [Serializable]
    public class AbortFileUploadsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> FileNames;
        public int? ProfileVersion;
    }

    [Serializable]
    public class AbortFileUploadsResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public int ProfileVersion;
    }
    [Serializable]
    public class DeleteFilesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> FileNames;
        public int? ProfileVersion;
    }

    [Serializable]
    public class DeleteFilesResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public int ProfileVersion;
    }
    [Serializable]
    public class EntityKey : PlayFabBaseModel
    {
        public string Id;
        public string Type;
    }
    [Serializable]
    public class FinalizeFileUploadsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> FileNames;
        public int ProfileVersion;
    }

    [Serializable]
    public class FinalizeFileUploadsResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public Dictionary<string,GetFileMetadata> Metadata;
        public int ProfileVersion;
    }

    [Serializable]
    public class GetFileMetadata : PlayFabBaseModel
    {
        public string Checksum;
        public string DownloadUrl;
        public string FileName;
        public DateTime LastModified;
        public int Size;
    }
    [Serializable]
    public class GetFilesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class GetFilesResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public Dictionary<string,GetFileMetadata> Metadata;
        public int ProfileVersion;
    }
    [Serializable]
    public class GetObjectsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public bool? EscapeObject;
    }

    [Serializable]
    public class GetObjectsResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public Dictionary<string,ObjectResult> Objects;
        public int ProfileVersion;
    }

    [Serializable]
    public class InitiateFileUploadMetadata : PlayFabBaseModel
    {
        public string FileName;
        public string UploadUrl;
    }
    [Serializable]
    public class InitiateFileUploadsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public List<string> FileNames;
        public int? ProfileVersion;
    }

    [Serializable]
    public class InitiateFileUploadsResponse : PlayFabResultCommon
    {
        public EntityKey Entity;
        public int ProfileVersion;
        public List<InitiateFileUploadMetadata> UploadDetails;
    }

    [Serializable]
    public class ObjectResult : PlayFabBaseModel
    {
        public object DataObject;
        public string EscapedDataObject;
        public string ObjectName;
    }

    public enum OperationTypes
    {
        Created,
        Updated,
        Deleted,
        None
    }

    [Serializable]
    public class SetObject : PlayFabBaseModel
    {
        public object DataObject;
        public bool? DeleteObject;
        public string EscapedDataObject;
        public string ObjectName;
    }

    [Serializable]
    public class SetObjectInfo : PlayFabBaseModel
    {
        public string ObjectName;
        public string OperationReason;
        public OperationTypes? SetResult;
    }
    [Serializable]
    public class SetObjectsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public int? ExpectedProfileVersion;
        public List<SetObject> Objects;
    }

    [Serializable]
    public class SetObjectsResponse : PlayFabResultCommon
    {
        public int ProfileVersion;
        public List<SetObjectInfo> SetResults;
    }
}
#endif
