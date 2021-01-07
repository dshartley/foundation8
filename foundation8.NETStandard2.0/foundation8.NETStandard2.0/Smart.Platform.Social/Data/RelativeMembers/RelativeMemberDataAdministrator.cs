using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using System.Collections.Generic;
using System;

namespace Smart.Platform.Social.Data.RelativeMembers
{
    /// <summary>
    /// Manages RelativeMember data.
    /// </summary>
    public class RelativeMemberDataAdministrator : DataAdministratorBase
    {
        #region Constructors

        private RelativeMemberDataAdministrator() : base() { }

        public RelativeMemberDataAdministrator( IDataManagementPolicy       dataManagementPolicy,
                                                IDataAccessStrategy         dataAccessStrategy,
                                                string                      defaultCultureInfoName,
                                                IDataAdministratorProvider  dataAdministratorProvider)
            : base(dataManagementPolicy, dataAccessStrategy, defaultCultureInfoName, dataAdministratorProvider)
        { }

        #endregion

        #region Protected Override Methods

        protected override IDataItemCollection NewCollection()
        {
            return new RelativeMemberCollection(this, _defaultCultureInfoName);
        }

        /// <summary>
        /// Sets up the foreign keys. To set up a foreign key get the data administrator for the relevant foreign key
        /// from the data administrator provider and handle the DataItemPrimaryKeyModified event. In handling this event
        /// update the foreign key of items in the collection accordingly.
        /// </summary>
        protected override void SetupForeignKeys()
        {
            // No foreign keys
        }

        protected override void SetupOmittedKeys()
        {
            // ConnectionContractType
            this._dataAccessStrategy.SetOmittedParameter(RelativeMemberDataParameterKeys.ConnectionContractTypes.ToString());

        }

        #endregion

        #region Public Override Methods

        public override void HandleDataItemModified(IDataItem item, int propertyEnum, string message)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            this.OnDataItemModified(item, propertyEnum, message);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the items by ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        public void LoadItemsByID(  int id, 
                                    RelativeConnectionContractTypes connectionContractType,
                                    string currentRelativeMemberID)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByID(_items, id, connectionContractType, currentRelativeMemberID);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by userProfileID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="userProfileID">The userProfileID.</param>
        public void LoadItemsByUserProfileID(   string applicationID, 
                                                string userProfileID)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByUserProfileID(_items, 
                                                                                                    applicationID, 
                                                                                                    userProfileID);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by email.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        public void LoadItemsByEmail(   string applicationID, 
                                        string email)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByEmail(_items,
                                                                                            applicationID, 
                                                                                            email);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by email.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="email">The email.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        public void LoadItemsByEmail(   string applicationID, 
                                        string email, 
                                        RelativeConnectionContractTypes connectionContractType,
                                        string currentRelativeMemberID)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByEmail(_items,
                                                                                            applicationID, 
                                                                                            email, 
                                                                                            connectionContractType, 
                                                                                            currentRelativeMemberID);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by findtext.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="findText">The findtext.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        public void LoadItemsByFindText(string applicationID, 
                                        string findText, 
                                        string currentRelativeMemberID, 
                                        RelativeMemberScopeTypes scopeType, 
                                        string previousRelativeMemberID,
                                        int numberOfItemsToLoad, 
                                        bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByFindText( _items, 
                                                                                                applicationID, 
                                                                                                findText, 
                                                                                                currentRelativeMemberID, 
                                                                                                scopeType, 
                                                                                                previousRelativeMemberID, 
                                                                                                numberOfItemsToLoad, 
                                                                                                selectItemsAfterPreviousYN);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by aspecttypes.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="aspectTypes">The aspectTypes.</param>
        /// <param name="maxResults">The maxResults.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="previousRelativeMemberID">The previousRelativeMemberID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        public void LoadItemsByAspects( string applicationID, 
                                        List<RelativeMemberQueryAspectTypes> aspectTypes,
                                        int maxResults,
                                        string currentRelativeMemberID,
                                        RelativeMemberScopeTypes scopeType,
                                        string previousRelativeMemberID,
                                        int numberOfItemsToLoad,
                                        bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeMemberDataAccessStrategy)_dataAccessStrategy).SelectByAspects(  _items,
                                                                                                applicationID,
                                                                                                aspectTypes, 
                                                                                                maxResults,
                                                                                                currentRelativeMemberID, 
                                                                                                scopeType, 
                                                                                                previousRelativeMemberID, 
                                                                                                numberOfItemsToLoad, 
                                                                                                selectItemsAfterPreviousYN);

            this.DoAfterLoad();
        }

        #endregion
    }
}
