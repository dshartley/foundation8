using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using System;

namespace Smart.Platform.Social.Data.RelativeConnectionRequests
{
    /// <summary>
    /// Manages RelativeConnectionRequest data.
    /// </summary>
    public class RelativeConnectionRequestDataAdministrator : DataAdministratorBase
    {
        #region Constructors

        private RelativeConnectionRequestDataAdministrator() : base() { }

        public RelativeConnectionRequestDataAdministrator( IDataManagementPolicy       dataManagementPolicy,
                                                IDataAccessStrategy         dataAccessStrategy,
                                                string                      defaultCultureInfoName,
                                                IDataAdministratorProvider  dataAdministratorProvider)
            : base(dataManagementPolicy, dataAccessStrategy, defaultCultureInfoName, dataAdministratorProvider)
        { }

        #endregion

        #region Protected Override Methods

        protected override IDataItemCollection NewCollection()
        {
            return new RelativeConnectionRequestCollection(this, _defaultCultureInfoName);
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
        /// <param name="requestType">Type of the request.</param>
        public void LoadItemsByFromRelativeMemberIDToRelativeMemberID(  string applicationID,
                                                                        string fromRelativeMemberID,
                                                                        string toRelativeMemberID,
                                                                        RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionRequestDataAccessStrategy)_dataAccessStrategy).SelectByFromRelativeMemberIDToRelativeMemberID(_items,
                                                                                                                                        applicationID, 
                                                                                                                                        fromRelativeMemberID, 
                                                                                                                                        toRelativeMemberID, 
                                                                                                                                        requestType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by fromRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        public void LoadItemsByFromRelativeMemberID(string applicationID, 
                                                    string fromRelativeMemberID,
                                                    RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionRequestDataAccessStrategy)_dataAccessStrategy).SelectByFromRelativeMemberID(  _items,
                                                                                                                        applicationID, 
                                                                                                                        fromRelativeMemberID, 
                                                                                                                        requestType);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by toRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        public void LoadItemsByToRelativeMemberID(  string applicationID, 
                                                    string toRelativeMemberID,
                                                    RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeConnectionRequestDataAccessStrategy)_dataAccessStrategy).SelectByToRelativeMemberID(_items,
                                                                                                                    applicationID, 
                                                                                                                    toRelativeMemberID, 
                                                                                                                    requestType);

            this.DoAfterLoad();
        }

        #endregion
    }
}
