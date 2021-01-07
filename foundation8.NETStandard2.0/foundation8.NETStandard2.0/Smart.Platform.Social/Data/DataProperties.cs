using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Platform.Social.Data
{
    public enum DataProperties
    {

        // RelativeConnectionRequests
        RelativeConnectionRequests_ID,
        RelativeConnectionRequests_ApplicationID,
        RelativeConnectionRequests_FromRelativeMemberID,
        RelativeConnectionRequests_ToRelativeMemberID,
        RelativeConnectionRequests_RequestType,
        RelativeConnectionRequests_DateActioned,
        RelativeConnectionRequests_RequestStatus,

        // RelativeConnections
        RelativeConnections_ID,
        RelativeConnections_ApplicationID,
        RelativeConnections_FromRelativeMemberID,
        RelativeConnections_ToRelativeMemberID,
        RelativeConnections_ConnectionContractType,
        RelativeConnections_DateActioned,
        RelativeConnections_DateLastActive,
        RelativeConnections_ConnectionStatus,

        // RelativeInteractions
        RelativeInteractions_ID,
        RelativeInteractions_ApplicationID,
        RelativeInteractions_FromRelativeMemberID,
        RelativeInteractions_ToRelativeMemberID,
        RelativeInteractions_InteractionType,
        RelativeInteractions_DateActioned,
        RelativeInteractions_InteractionStatus,
        RelativeInteractions_Text,

        // RelativeMembers
        RelativeMembers_ID,
        RelativeMembers_ApplicationID,
        RelativeMembers_UserProfileID,
        RelativeMembers_Email,
        RelativeMembers_FullName,
        RelativeMembers_AvatarImageFileName,
        RelativeMembers_ConnectionContractTypes,

        // RelativeTimelineEvents
        RelativeTimelineEvents_ID,
        RelativeTimelineEvents_ApplicationID,
        RelativeTimelineEvents_ForRelativeMemberID,
        RelativeTimelineEvents_RelativeInteractionID,
        RelativeTimelineEvents_EventType,
        RelativeTimelineEvents_DateActioned,
        RelativeTimelineEvents_EventStatus

    }

}
