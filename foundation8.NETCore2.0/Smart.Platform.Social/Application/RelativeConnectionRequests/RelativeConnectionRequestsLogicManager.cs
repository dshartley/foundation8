using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Smart.Platform.Social;
using Smart.Platform.Social.Data;
using Smart.Platform.Social.Data.RelativeConnectionRequests;
using System;
using System.Globalization;

namespace Smart.Platform.Social.Application.RelativeConnectionRequests
{
    /// <summary>
    /// Manages the application layer for the RelativeConnectionRequests.
    /// </summary>
    public class RelativeConnectionRequestsLogicManager
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        private CultureInfo _fromCultureInfo = new CultureInfo("en-GB");

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeConnectionRequestsLogicManager"/> class.
        /// </summary>
        public RelativeConnectionRequestsLogicManager() {
        }

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
        /// Insert the RelativeConnectionRequest.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Insert(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnectionRequest rcr = (RelativeConnectionRequest)_dataManager.RelativeConnectionRequestDataAdministrator.Items.AddItem();

            rcr.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is inserted
            rcr.Status = DataItemStatusTypes.New;

            // Save to the database
            _dataManager.RelativeConnectionRequestDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();
            dataWrapper.ID = rcr.ID.ToString();

        }

        /// <summary>
        /// Update the RelativeConnectionRequest.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Update(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnectionRequest rcr = (RelativeConnectionRequest)_dataManager.RelativeConnectionRequestDataAdministrator.Items.AddItem();

            rcr.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is updated
            rcr.Status = DataItemStatusTypes.Modified;

            // Save to the database
            _dataManager.RelativeConnectionRequestDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Delete the RelativeConnectionRequest.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Delete(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnectionRequest rcr = (RelativeConnectionRequest)_dataManager.RelativeConnectionRequestDataAdministrator.Items.AddItem();

            rcr.ID = Int32.Parse(dataWrapper.ID);

            // Set the item status so that the item is deleted
            rcr.Status = DataItemStatusTypes.Deleted;

            // Save to the database
            _dataManager.RelativeConnectionRequestDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Loads the RelativeConnectionRequests.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByFromRelativeMemberIDToRelativeMemberID(   string applicationID,
                                                                    string fromRelativeMemberID,
                                                                    string toRelativeMemberID,
                                                                    RelativeConnectionRequestTypes requestType,
                                                                    DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionRequestDataAdministrator.LoadItemsByFromRelativeMemberIDToRelativeMemberID(  applicationID, 
                                                                                                                        fromRelativeMemberID, 
                                                                                                                        toRelativeMemberID, 
                                                                                                                        requestType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnectionRequests.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByFromRelativeMemberID( string applicationID,
                                                string fromRelativeMemberID,
                                                RelativeConnectionRequestTypes requestType,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionRequestDataAdministrator.LoadItemsByFromRelativeMemberID(applicationID, 
                                                                                                    fromRelativeMemberID, 
                                                                                                    requestType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnectionRequests.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="requestType">Type of the request.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByToRelativeMemberID(   string applicationID, 
                                                string toRelativeMemberID,
                                                RelativeConnectionRequestTypes requestType,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionRequestDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionRequestDataAdministrator.LoadItemsByToRelativeMemberID(  applicationID, 
                                                                                                    toRelativeMemberID, 
                                                                                                    requestType);

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

            // Put the RelativeConnectionRequests in the wrapper
            DataJSONWrapper rcrw = _dataManager.RelativeConnectionRequestDataAdministrator.ToWrapper();
            rcrw.ID = "RelativeConnectionRequests";

            dataWrapper.Items.Add(rcrw);

            if (_dataManager.RelativeMemberDataAdministrator != null)
            {
                // Put the RelativeMembers in the wrapper
                DataJSONWrapper rmw = _dataManager.RelativeMemberDataAdministrator.ToWrapper();
                rmw.ID = "RelativeMembers";

                dataWrapper.Items.Add(rmw);
            }

            return dataWrapper;
        }

        #endregion

    }
}
