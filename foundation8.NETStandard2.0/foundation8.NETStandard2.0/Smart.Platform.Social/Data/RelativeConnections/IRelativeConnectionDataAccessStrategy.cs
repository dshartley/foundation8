using Smart.Platform.Data;

namespace Smart.Platform.Social.Data.RelativeConnections
{
    /// <summary>
    /// Defines a class which provides data access for RelativeConnection data.
    /// </summary>
    public interface IRelativeConnectionDataAccessStrategy
    {

        /// <summary>
        /// Selects the by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByFromRelativeMemberIDToRelativeMemberID( IDataItemCollection collection,
                                                                            string applicationID,
                                                                            string fromRelativeMemberID,
                                                                            string toRelativeMemberID,
                                                                            RelativeConnectionContractTypes connectionContractType);

        /// <summary>
        /// Selects the by forRelativeMemberID and withRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">The forRelativeMemberID.</param>
        /// <param name="withRelativeMemberID">The withRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByForRelativeMemberIDWithRelativeMemberID(IDataItemCollection collection,
                                                                            string applicationID,
                                                                            string forRelativeMemberID,
                                                                            string withRelativeMemberID,
                                                                            RelativeConnectionContractTypes connectionContractType);

        /// <summary>
        /// Selects the by fromRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByFromRelativeMemberID(   IDataItemCollection collection,
                                                            string applicationID,
                                                            string fromRelativeMemberID,
                                                            RelativeConnectionContractTypes connectionContractType);

        /// <summary>
        /// Selects the by toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByToRelativeMemberID( IDataItemCollection collection,
                                                        string applicationID,
                                                        string toRelativeMemberID,
                                                        RelativeConnectionContractTypes connectionContractType);

        /// <summary>
        /// Selects the by withRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="withRelativeMemberID">The withRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        IDataItemCollection SelectByWithRelativeMemberID(   IDataItemCollection collection,
                                                            string applicationID,
                                                            string withRelativeMemberID,
                                                            RelativeConnectionContractTypes connectionContractType);

    }
}
