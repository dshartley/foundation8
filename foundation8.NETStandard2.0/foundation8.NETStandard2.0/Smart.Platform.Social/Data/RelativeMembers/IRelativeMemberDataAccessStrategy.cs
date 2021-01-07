using Smart.Platform.Data;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Smart.Platform.Social.Data.RelativeMembers
{
    /// <summary>
    /// Defines a class which provides data access for RelativeMember data.
    /// </summary>
    public interface IRelativeMemberDataAccessStrategy
    {

        /// <summary>
        /// Selects the by ID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="id">The ID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <returns></returns>
        IDataItemCollection SelectByID( IDataItemCollection collection,
                                        int id, 
                                        RelativeConnectionContractTypes connectionContractType,
                                        string currentRelativeMemberID);

        /// <summary>
        /// Selects the by email.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="userProfileID">The userProfileID.</param>
        /// <returns></returns>
        IDataItemCollection SelectByUserProfileID(  IDataItemCollection collection,
                                                    string applicationID,
                                                    string userProfileID);

        /// <summary>
        /// Selects the by email.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        IDataItemCollection SelectByEmail(  IDataItemCollection collection,
                                            string applicationID,
                                            string email);

        /// <summary>
        /// Selects the by email.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <returns></returns>
        IDataItemCollection SelectByEmail(  IDataItemCollection collection,
                                            string applicationID,
                                            string email, 
                                            RelativeConnectionContractTypes connectionContractType,
                                            string currentRelativeMemberID);

        /// <summary>
        /// Selects the by findtext.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="findText">The findtext.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        IDataItemCollection SelectByFindText(   IDataItemCollection collection,
                                                string applicationID,
                                                string findText, 
                                                string currentRelativeMemberID,
                                                RelativeMemberScopeTypes scopeType, 
                                                string previousRelativeMemberID, 
                                                int numberOfItemsToLoad, 
                                                bool selectItemsAfterPreviousYN);

        /// <summary>
        /// Selects the by aspecttypes.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="aspectTypes">The aspectTypes.</param>
        /// <param name="maxResults">The maxResults.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        IDataItemCollection SelectByAspects(    IDataItemCollection collection,
                                                string applicationID,
                                                List<RelativeMemberQueryAspectTypes> aspectTypes,
                                                int maxResults,
                                                string currentRelativeMemberID,
                                                RelativeMemberScopeTypes scopeType,
                                                string previousRelativeMemberID,
                                                int numberOfItemsToLoad,
                                                bool selectItemsAfterPreviousYN);

    }
}
