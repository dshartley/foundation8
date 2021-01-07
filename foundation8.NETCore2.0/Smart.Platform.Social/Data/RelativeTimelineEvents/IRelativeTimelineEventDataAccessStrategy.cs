using Smart.Platform.Data;
using System.Collections.Generic;

namespace Smart.Platform.Social.Data.RelativeTimelineEvents
{
    /// <summary>
    /// Defines a class which provides data access for RelativeTimelineEvent data.
    /// </summary>
    public interface IRelativeTimelineEventDataAccessStrategy
    {

        /// <summary>
        /// Selects the by forRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">The forRelativeMemberID.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="relativeTimelineEventTypes">The relativeTimelineEventTypes.</param>
        /// <param name="previousRelativeTimelineEventID">The previousRelativeTimelineEventID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        IDataItemCollection SelectByForRelativeMemberID(    IDataItemCollection collection,
                                                            string applicationID,
                                                            string forRelativeMemberID,
                                                            string currentRelativeMemberID,
                                                            RelativeTimelineEventScopeTypes scopeType,
                                                            List<RelativeTimelineEventTypes> relativeTimelineEventTypes,
                                                            string previousRelativeTimelineEventID, 
                                                            int numberOfItemsToLoad, 
                                                            bool selectItemsAfterPreviousYN);

        /// <summary>
        /// Selects the by relativeInteractionID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="relativeInteractionID">The relativeInteractionID.</param>
        /// <returns></returns>
        IDataItemCollection SelectByRelativeInteractionID(  IDataItemCollection collection,
                                                            string applicationID,
                                                            string relativeInteractionID);

    }
}
