using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Platform.Social
{

    public enum RelativeConnectionContractTypes
    {
        Friend = 1,
        Follow = 2,
        Handshake = 3,
        Contractless = 4,
        Transient = 5
    }

    public enum RelativeConnectionStatus
    {
        Active = 1
    }

    public enum RelativeConnectionRequestTypes
    {
        Friend = 1
    }

    public enum RelativeConnectionRequestStatus
    {
        Active = 1,
        Accepted = 2,
        Rejected = 3
    }

    public enum RelativeInteractionTypes
    {
        Standard = 1,
        PostFollowContractMyFeed = 2,
        PostFriendContractMyFeed = 3,
        PostFriendContractFriendFeed = 4,
        Handshake = 5,
        PostContractlessMyFeed = 6,
        PostContractlessMemberFeed = 7
    }

    public enum RelativeInteractionStatus
    {
        Active = 1
    }

    public enum RelativeInteractionBroadcastRecipientTypes
    {
        FromMember = 1,
        ToMember = 2,
        FriendContractMembers = 3,
        FollowContractMembers = 4,
        HandshakeContractMembers = 5,
        ContractlessContractMembers = 6
    }

    public enum RelativeTimelineEventTypes
    {
        Standard = 1,
        PostFollowContractMyFeed = 2,
        PostFriendContractMyFeed = 3,
        PostFriendContractFriendFeed = 4,
        Handshake = 5,
        PostContractlessMyFeed = 6,
        PostContractlessMemberFeed = 7
    }

    public enum RelativeTimelineEventStatus
    {
        Active = 1
    }

    public enum RelativeMemberScopeTypes
    {
        Unspecified = 0,
        All = 1,
        Friends = 2,
        Followers = 3,
        Followed = 4,
        Contractless = 5,
        HandshakeGiver = 6,
        HandshakeReceiver = 7,
        Transient = 8
    }

    public enum RelativeTimelineEventScopeTypes
    {
        Unspecified = 0,
        All = 1
    }

    public enum RelativeMemberQueryAspectTypes
    {
        RecentlyPostedTo = 1,
        RecentlyPostedFrom = 2
    }

}
