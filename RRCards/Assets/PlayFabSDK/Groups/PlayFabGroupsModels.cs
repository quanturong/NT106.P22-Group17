#if !DISABLE_PLAYFABENTITY_API
using System;
using System.Collections.Generic;
using PlayFab.SharedModels;

namespace PlayFab.GroupsModels
{
    [Serializable]
    public class AcceptGroupApplicationRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class AcceptGroupInvitationRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class AddMembersRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
        public List<EntityKey> Members;
        public string RoleId;
    }
    [Serializable]
    public class ApplyToGroupRequest : PlayFabRequestCommon
    {
        public bool? AutoAcceptOutstandingInvite;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class ApplyToGroupResponse : PlayFabResultCommon
    {
        public EntityWithLineage Entity;
        public DateTime Expires;
        public EntityKey Group;
    }
    [Serializable]
    public class BlockEntityRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class ChangeMemberRoleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public string DestinationRoleId;
        public EntityKey Group;
        public List<EntityKey> Members;
        public string OriginRoleId;
    }
    [Serializable]
    public class CreateGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public string GroupName;
    }

    [Serializable]
    public class CreateGroupResponse : PlayFabResultCommon
    {
        public string AdminRoleId;
        public DateTime Created;
        public EntityKey Group;
        public string GroupName;
        public string MemberRoleId;
        public int ProfileVersion;
        public Dictionary<string,string> Roles;
    }
    [Serializable]
    public class CreateGroupRoleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
        public string RoleId;
        public string RoleName;
    }

    [Serializable]
    public class CreateGroupRoleResponse : PlayFabResultCommon
    {
        public int ProfileVersion;
        public string RoleId;
        public string RoleName;
    }
    [Serializable]
    public class DeleteGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
    }
    [Serializable]
    public class DeleteRoleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
        public string RoleId;
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
    public class EntityMemberRole : PlayFabBaseModel
    {
        public List<EntityWithLineage> Members;
        public string RoleId;
        public string RoleName;
    }
    [Serializable]
    public class EntityWithLineage : PlayFabBaseModel
    {
        public EntityKey Key;
        public Dictionary<string,EntityKey> Lineage;
    }
    [Serializable]
    public class GetGroupRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
        public string GroupName;
    }

    [Serializable]
    public class GetGroupResponse : PlayFabResultCommon
    {
        public string AdminRoleId;
        public DateTime Created;
        public EntityKey Group;
        public string GroupName;
        public string MemberRoleId;
        public int ProfileVersion;
        public Dictionary<string,string> Roles;
    }
    [Serializable]
    public class GroupApplication : PlayFabBaseModel
    {
        public EntityWithLineage Entity;
        public DateTime Expires;
        public EntityKey Group;
    }
    [Serializable]
    public class GroupBlock : PlayFabBaseModel
    {
        public EntityWithLineage Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class GroupInvitation : PlayFabBaseModel
    {
        public DateTime Expires;
        public EntityKey Group;
        public EntityWithLineage InvitedByEntity;
        public EntityWithLineage InvitedEntity;
        public string RoleId;
    }
    [Serializable]
    public class GroupRole : PlayFabBaseModel
    {
        public string RoleId;
        public string RoleName;
    }
    [Serializable]
    public class GroupWithRoles : PlayFabBaseModel
    {
        public EntityKey Group;
        public string GroupName;
        public int ProfileVersion;
        public List<GroupRole> Roles;
    }
    [Serializable]
    public class InviteToGroupRequest : PlayFabRequestCommon
    {
        public bool? AutoAcceptOutstandingApplication;
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
        public string RoleId;
    }
    [Serializable]
    public class InviteToGroupResponse : PlayFabResultCommon
    {
        public DateTime Expires;
        public EntityKey Group;
        public EntityWithLineage InvitedByEntity;
        public EntityWithLineage InvitedEntity;
        public string RoleId;
    }
    [Serializable]
    public class IsMemberRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
        public string RoleId;
    }

    [Serializable]
    public class IsMemberResponse : PlayFabResultCommon
    {
        public bool IsMember;
    }
    [Serializable]
    public class ListGroupApplicationsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
    }

    [Serializable]
    public class ListGroupApplicationsResponse : PlayFabResultCommon
    {
        public List<GroupApplication> Applications;
    }
    [Serializable]
    public class ListGroupBlocksRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
    }

    [Serializable]
    public class ListGroupBlocksResponse : PlayFabResultCommon
    {
        public List<GroupBlock> BlockedEntities;
    }
    [Serializable]
    public class ListGroupInvitationsRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
    }

    [Serializable]
    public class ListGroupInvitationsResponse : PlayFabResultCommon
    {
        public List<GroupInvitation> Invitations;
    }
    [Serializable]
    public class ListGroupMembersRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
    }

    [Serializable]
    public class ListGroupMembersResponse : PlayFabResultCommon
    {
        public List<EntityMemberRole> Members;
    }
    [Serializable]
    public class ListMembershipOpportunitiesRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class ListMembershipOpportunitiesResponse : PlayFabResultCommon
    {
        public List<GroupApplication> Applications;
        public List<GroupInvitation> Invitations;
    }
    [Serializable]
    public class ListMembershipRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
    }

    [Serializable]
    public class ListMembershipResponse : PlayFabResultCommon
    {
        public List<GroupWithRoles> Groups;
    }

    public enum OperationTypes
    {
        Created,
        Updated,
        Deleted,
        None
    }
    [Serializable]
    public class RemoveGroupApplicationRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class RemoveGroupInvitationRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class RemoveMembersRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Group;
        public List<EntityKey> Members;
        public string RoleId;
    }
    [Serializable]
    public class UnblockEntityRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public EntityKey Entity;
        public EntityKey Group;
    }
    [Serializable]
    public class UpdateGroupRequest : PlayFabRequestCommon
    {
        public string AdminRoleId;
        public Dictionary<string,string> CustomTags;
        public int? ExpectedProfileVersion;
        public EntityKey Group;
        public string GroupName;
        public string MemberRoleId;
    }

    [Serializable]
    public class UpdateGroupResponse : PlayFabResultCommon
    {
        public string OperationReason;
        public int ProfileVersion;
        public OperationTypes? SetResult;
    }
    [Serializable]
    public class UpdateGroupRoleRequest : PlayFabRequestCommon
    {
        public Dictionary<string,string> CustomTags;
        public int? ExpectedProfileVersion;
        public EntityKey Group;
        public string RoleId;
        public string RoleName;
    }

    [Serializable]
    public class UpdateGroupRoleResponse : PlayFabResultCommon
    {
        public string OperationReason;
        public int ProfileVersion;
        public OperationTypes? SetResult;
    }
}
#endif
