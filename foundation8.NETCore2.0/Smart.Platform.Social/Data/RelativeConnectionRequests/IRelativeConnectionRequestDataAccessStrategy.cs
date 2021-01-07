using Smart.Platform.Data;

namespace Smart.Platform.Social.Data.RelativeConnectionRequests
{
    /// <summary>
    /// Defines a class which provides data access for RelativeConnectionRequest data.
    /// </summary>
    public interface IRelativeConnectionRequestDataAccessStrategy
    {

        /// <summary>
        /// Selects the by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByFromRelativeMemberIDToRelativeMemberID( IDataItemCollection collection,
                                                                            string applicationID,
                                                                            string fromRelativeMemberID,
                                                                            string toRelativeMemberID,
                                                                            RelativeConnectionRequestTypes requestType);

        /// <summary>
        /// Selects the by fromRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByFromRelativeMemberID(   IDataItemCollection collection,
                                                            string applicationID,
                                                            string fromRelativeMemberID,
                                                            RelativeConnectionRequestTypes requestType);

        /// <summary>
        /// Selects the by toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByToRelativeMemberID( IDataItemCollection collection,
                                                        string applicationID,
                                                        string toRelativeMemberID,
                                                        RelativeConnectionRequestTypes requestType);


    }
}
