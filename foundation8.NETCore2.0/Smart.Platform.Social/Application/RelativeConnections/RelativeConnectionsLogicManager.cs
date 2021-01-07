using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Smart.Platform.Social;
using Smart.Platform.Social.Data;
using Smart.Platform.Social.Data.RelativeConnections;
using System;
using System.Globalization;

namespace Smart.Platform.Social.Application.RelativeConnections
{
    /// <summary>
    /// Manages the application layer for the RelativeConnections.
    /// </summary>
    public class RelativeConnectionsLogicManager
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        private CultureInfo _fromCultureInfo = new CultureInfo("en-GB");

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeConnectionsLogicManager"/> class.
        /// </summary>
        public RelativeConnectionsLogicManager() { }

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
        /// Insert the RelativeConnection.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Insert(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnection rc = (RelativeConnection)_dataManager.RelativeConnectionDataAdministrator.Items.AddItem();

            rc.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is inserted
            rc.Status = DataItemStatusTypes.New;

            // Save to the database
            _dataManager.RelativeConnectionDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();
            dataWrapper.ID = rc.ID.ToString();

        }

        /// <summary>
        /// Update the RelativeConnection.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Update(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnection rc = (RelativeConnection)_dataManager.RelativeConnectionDataAdministrator.Items.AddItem();

            rc.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is updated
            rc.Status = DataItemStatusTypes.Modified;

            // Save to the database
            _dataManager.RelativeConnectionDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Delete the RelativeConnection.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Delete(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Create a new data item
            RelativeConnection rc = (RelativeConnection)_dataManager.RelativeConnectionDataAdministrator.Items.AddItem();

            rc.ID = Int32.Parse(dataWrapper.ID);

            // Set the item status so that the item is deleted
            rc.Status = DataItemStatusTypes.Deleted;

            // Save to the database
            _dataManager.RelativeConnectionDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Loads the RelativeConnections.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByFromRelativeMemberIDToRelativeMemberID(   string applicationID, 
                                                                    string fromRelativeMemberID, 
                                                                    string toRelativeMemberID, 
                                                                    RelativeConnectionContractTypes connectionContractType, 
                                                                    DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionDataAdministrator.LoadItemsByFromRelativeMemberIDToRelativeMemberID( applicationID, 
                                                                                                                fromRelativeMemberID, 
                                                                                                                toRelativeMemberID, 
                                                                                                                connectionContractType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnections.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">For relative member identifier.</param>
        /// <param name="withRelativeMemberID">The with relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByForRelativeMemberIDWithRelativeMemberID(  string applicationID, 
                                                                    string forRelativeMemberID,
                                                                    string withRelativeMemberID,
                                                                    RelativeConnectionContractTypes connectionContractType,
                                                                    DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionDataAdministrator.LoadItemsByForRelativeMemberIDWithRelativeMemberID(applicationID, 
                                                                                                                forRelativeMemberID, 
                                                                                                                withRelativeMemberID, 
                                                                                                                connectionContractType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnections.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">From relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByFromRelativeMemberID( string applicationID, 
                                                string fromRelativeMemberID,
                                                RelativeConnectionContractTypes connectionContractType,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionDataAdministrator.LoadItemsByFromRelativeMemberID(   applicationID, 
                                                                                                fromRelativeMemberID, 
                                                                                                connectionContractType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnections.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">To relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByToRelativeMemberID(   string applicationID, 
                                                string toRelativeMemberID,
                                                RelativeConnectionContractTypes connectionContractType,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionDataAdministrator.LoadItemsByToRelativeMemberID( applicationID, 
                                                                                            toRelativeMemberID, 
                                                                                            connectionContractType);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeConnections.
        /// </summary>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="withRelativeMemberID">The with relative member identifier.</param>
        /// <param name="connectionContractType">Type of the connection contract.</param>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByWithRelativeMemberID( string applicationID, 
                                                string withRelativeMemberID,
                                                RelativeConnectionContractTypes connectionContractType,
                                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeConnectionDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeConnectionDataAdministrator.LoadItemsByWithRelativeMemberID(   applicationID, 
                                                                                                withRelativeMemberID, 
                                                                                                connectionContractType);

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

            // Put the RelativeConnections in the wrapper
            DataJSONWrapper rcw = _dataManager.RelativeConnectionDataAdministrator.ToWrapper();
            rcw.ID = "RelativeConnections";

            dataWrapper.Items.Add(rcw);

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
