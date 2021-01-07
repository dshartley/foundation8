using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using System;
using System.Collections.Generic;

namespace Smart.Platform.Social.Data.RelativeTimelineEvents
{
    /// <summary>
    /// Manages RelativeTimelineEvent data.
    /// </summary>
    public class RelativeTimelineEventDataAdministrator : DataAdministratorBase
    {
        #region Constructors

        private RelativeTimelineEventDataAdministrator() : base() { }

        public RelativeTimelineEventDataAdministrator( IDataManagementPolicy       dataManagementPolicy,
                                                IDataAccessStrategy         dataAccessStrategy,
                                                string                      defaultCultureInfoName,
                                                IDataAdministratorProvider  dataAdministratorProvider)
            : base(dataManagementPolicy, dataAccessStrategy, defaultCultureInfoName, dataAdministratorProvider)
        { }

        #endregion

        #region Protected Override Methods

        protected override IDataItemCollection NewCollection()
        {
            return new RelativeTimelineEventCollection(this, _defaultCultureInfoName);
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
        /// Loads the items by forRelativeMemberID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">The forRelativeMemberID.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="relativeTimelineEventTypes">The relativeTimelineEventTypes.</param>
        /// <param name="previousRelativeTimelineEventID">The previousRelativeTimelineEventID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public void LoadItemsByForRelativeMemberID( string applicationID, 
                                                    string forRelativeMemberID,
                                                    string currentRelativeMemberID,
                                                    RelativeTimelineEventScopeTypes scopeType,
                                                    List<RelativeTimelineEventTypes> relativeTimelineEventTypes,
                                                    string previousRelativeTimelineEventID, 
                                                    int numberOfItemsToLoad, 
                                                    bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            if (String.IsNullOrEmpty(forRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "forRelativeMemberID is nothing"));
            if (String.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (relativeTimelineEventTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeTimelineEventTypes is nothing"));
            if (String.IsNullOrEmpty(previousRelativeTimelineEventID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeTimelineEventID is nothing"));

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeTimelineEventDataAccessStrategy)_dataAccessStrategy).SelectByForRelativeMemberID(   _items,
                                                                                                                    applicationID,
                                                                                                                    forRelativeMemberID,
                                                                                                                    currentRelativeMemberID,
                                                                                                                    scopeType,
                                                                                                                    relativeTimelineEventTypes,
                                                                                                                    previousRelativeTimelineEventID,
                                                                                                                    numberOfItemsToLoad,
                                                                                                                    selectItemsAfterPreviousYN);

            this.DoAfterLoad();
        }

        /// <summary>
        /// Loads the items by relativeInteractionID.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="relativeInteractionID">The relativeInteractionID.</param>
        public void LoadItemsByRelativeInteractionID(   string applicationID, 
                                                        string relativeInteractionID)
        {
            #region Check Parameters

            #endregion

            this.SetupCollection();

            // Load the data
            _items = ((IRelativeTimelineEventDataAccessStrategy)_dataAccessStrategy).SelectByRelativeInteractionID( _items,
                                                                                                                    applicationID,
                                                                                                                    relativeInteractionID);

            this.DoAfterLoad();
        }

        #endregion
    }
}
