using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using System;

namespace Smart.Platform.Social.Data.RelativeConnections
{
    /// <summary>
    /// Manages RelativeConnection data.
    /// </summary>
    public class RelativeConnectionDataAdministrator : DataAdministratorBase
    {
        #region Constructors

        private RelativeConnectionDataAdministrator() : base() { }

        public RelativeConnectionDataAdministrator( IDataManagementPolicy       dataManagementPolicy,
                                                IDataAccessStrategy         dataAccessStrategy,
                                                string                      defaultCultureInfoName,
                                                IDataAdministratorProvider  dataAdministratorProvider)
            : base(dataManagementPolicy, dataAccessStrategy, defaultCultureInfoName, dataAdministratorProvider)
        { }

        #endregion

        #region Protected Override Methods

        protected override IDataItemCollection NewCollection()
        {
            return new RelativeConnectionCollection(this, _defaultCultureInfoName);
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
        /// Loads the items by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        public void LoadItemsByFromRelativeMemberIDToRelativeMemberID(  string applicationID, 
                                                                        string fromRelativeMemberID,
                                                                        string toRelativeMemberID,
                                                                        RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionDataAccessStrategy)_dataAccessStrategy).SelectByFromRelativeMemberIDToRelativeMemberID(   _items,
                                                                                                                                    applicationID, 
                                                                                                                                    fromRelativeMemberID, 
                                                                                                                                    toRelativeMemberID, 
                                                                                                                                    connectionContractType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">For relative member identifier.</param>
        /// <param name="withRelativeMemberID">The with relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        public void LoadItemsByForRelativeMemberIDWithRelativeMemberID( string applicationID, 
                                                                        string forRelativeMemberID,
                                                                        string withRelativeMemberID,
                                                                        RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionDataAccessStrategy)_dataAccessStrategy).SelectByForRelativeMemberIDWithRelativeMemberID(  _items,
                                                                                                                                    applicationID, 
                                                                                                                                    forRelativeMemberID, 
                                                                                                                                    withRelativeMemberID, 
                                                                                                                                    connectionContractType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by fromRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        public void LoadItemsByFromRelativeMemberID(    string applicationID, 
                                                        string fromRelativeMemberID,
                                                        RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionDataAccessStrategy)_dataAccessStrategy).SelectByFromRelativeMemberID( _items,
                                                                                                                applicationID, 
                                                                                                                fromRelativeMemberID, 
                                                                                                                connectionContractType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by toRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        public void LoadItemsByToRelativeMemberID(  string applicationID, 
                                                    string toRelativeMemberID,
                                                    RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionDataAccessStrategy)_dataAccessStrategy).SelectByToRelativeMemberID(   _items,
                                                                                                                applicationID, 
                                                                                                                toRelativeMemberID, 
                                                                                                                connectionContractType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by withRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="withRelativeMemberID">The with relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        public void LoadItemsByWithRelativeMemberID(string applicationID, 
                                                    string withRelativeMemberID,
                                                    RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionDataAccessStrategy)_dataAccessStrategy).SelectByWithRelativeMemberID( _items,
                                                                                                                applicationID, 
                                                                                                                withRelativeMemberID, 
                                                                                                                connectionContractType);

            this.DoAfterLoad();
        }

        #endregion
    }
}
