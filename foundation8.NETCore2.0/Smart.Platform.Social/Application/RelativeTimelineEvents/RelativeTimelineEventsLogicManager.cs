using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Smart.Platform.Social;
using Smart.Platform.Social.Data;
using Smart.Platform.Social.Data.RelativeTimelineEvents;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Smart.Platform.Social.Application.RelativeTimelineEvents
{
    /// <summary>
    /// Manages the application layer for the RelativeTimelineEvents.
    /// </summary>
    public class RelativeTimelineEventsLogicManager
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        private CultureInfo _fromCultureInfo = new CultureInfo("en-GB");

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeTimelineEventsLogicManager"/> class.
        /// </summary>
        public RelativeTimelineEventsLogicManager() { }

        #endregion

        #region Public Methods

        private SocialDataManager _dataManager;

        /// <summary>
        /// Gets or sets the data manager.
        /// </summary>
        /// <value>The data manager.</value>
        public SocialDataManager DataManager
        {
            get { return _dataManager; }
            set { _dataManager = value; }
        }

        /// <summary>
        /// Insert the RelativeTimelineEvent.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Insert(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeTimelineEventDataAdministrator.Initialise();

            // Create a new data item
            RelativeTimelineEvent rtle = (RelativeTimelineEvent)_dataManager.RelativeTimelineEventDataAdministrator.Items.AddItem();

            rtle.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is inserted
            rtle.Status = DataItemStatusTypes.New;

            // Save to the database
            _dataManager.RelativeTimelineEventDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();
            dataWrapper.ID = rtle.ID.ToString();

        }

        /// <summary>
        /// Update the RelativeTimelineEvent.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Update(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeTimelineEventDataAdministrator.Initialise();

            // Create a new data item
            RelativeTimelineEvent rtle = (RelativeTimelineEvent)_dataManager.RelativeTimelineEventDataAdministrator.Items.AddItem();

            rtle.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is updated
            rtle.Status = DataItemStatusTypes.Modified;

            // Save to the database
            _dataManager.RelativeTimelineEventDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Delete the RelativeTimelineEvent.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Delete(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeTimelineEventDataAdministrator.Initialise();

            // Create a new data item
            RelativeTimelineEvent rtle = (RelativeTimelineEvent)_dataManager.RelativeTimelineEventDataAdministrator.Items.AddItem();

            rtle.ID = Int32.Parse(dataWrapper.ID);

            // Set the item status so that the item is deleted
            rtle.Status = DataItemStatusTypes.Deleted;

            // Save to the database
            _dataManager.RelativeTimelineEventDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Loads the RelativeTimelineEvents.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByForRelativeMemberID(  string applicationID, 
                                                string forRelativeMemberID,
                                                string currentRelativeMemberID,
                                                RelativeTimelineEventScopeTypes scopeType,
                                                List<RelativeTimelineEventTypes> relativeTimelineEventTypes,
                                                string previousRelativeTimelineEventID,
                                                int numberOfItemsToLoad,
                                                bool selectItemsAfterPreviousYN,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(forRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "forRelativeMemberID is nothing"));
            if (string.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (relativeTimelineEventTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeTimelineEventTypes is nothing"));
            if (string.IsNullOrEmpty(previousRelativeTimelineEventID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeTimelineEventID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeTimelineEventDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeTimelineEventDataAdministrator.LoadItemsByForRelativeMemberID( applicationID, 
                                                                                                forRelativeMemberID,
                                                                                                currentRelativeMemberID,
                                                                                                scopeType,
                                                                                                relativeTimelineEventTypes,
                                                                                                previousRelativeTimelineEventID,
                                                                                                numberOfItemsToLoad,
                                                                                                selectItemsAfterPreviousYN);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeTimelineEvents.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByRelativeInteractionID(string applicationID, 
                                                string relativeInteractionID,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(relativeInteractionID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeInteractionID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeTimelineEventDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeTimelineEventDataAdministrator.LoadItemsByRelativeInteractionID(   applicationID, 
                                                                                                    relativeInteractionID);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        #endregion

        #region Private Methods

        private DataJSONWrapper DataToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.Items.Clear();

            // Put the RelativeTimelineEvents in the wrapper
            DataJSONWrapper rtlew = _dataManager.RelativeTimelineEventDataAdministrator.ToWrapper();
            rtlew.ID = "RelativeTimelineEvents";

            dataWrapper.Items.Add(rtlew);

            if (_dataManager.RelativeMemberDataAdministrator != null)
            {
                // Put the RelativeMembers in the wrapper
                DataJSONWrapper rmw = _dataManager.RelativeMemberDataAdministrator.ToWrapper();
                rmw.ID = "RelativeMembers";

                dataWrapper.Items.Add(rmw);
            }

            if (_dataManager.RelativeInteractionDataAdministrator != null)
            {
                // Put the RelativeInteractions in the wrapper
                DataJSONWrapper riw = _dataManager.RelativeInteractionDataAdministrator.ToWrapper();
                riw.ID = "RelativeInteractions";

                dataWrapper.Items.Add(riw);
            }

            return dataWrapper;
        }

        #endregion
    }
}
