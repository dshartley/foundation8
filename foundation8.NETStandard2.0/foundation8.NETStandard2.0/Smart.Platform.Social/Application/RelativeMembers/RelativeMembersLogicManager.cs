using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Smart.Platform.Social;
using Smart.Platform.Social.Data;
using Smart.Platform.Social.Data.RelativeMembers;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Smart.Platform.Social.Application.RelativeMembers
{
    /// <summary>
    /// Manages the application layer for the RelativeMembers.
    /// </summary>
    public class RelativeMembersApiLogicManager
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        private CultureInfo _fromCultureInfo = new CultureInfo("en-GB");

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeMembersApiLogicManager"/> class.
        /// </summary>
        public RelativeMembersApiLogicManager() { }

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
        /// Insert the RelativeMember.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Insert(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Create a new data item
            RelativeMember rm = (RelativeMember)_dataManager.RelativeMemberDataAdministrator.Items.AddItem();

            rm.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is inserted
            rm.Status = DataItemStatusTypes.New;

            // Save to the database
            _dataManager.RelativeMemberDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();
            dataWrapper.ID = rm.ID.ToString();

        }

        /// <summary>
        /// Update the RelativeMember.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Update(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Create a new data item
            RelativeMember rm = (RelativeMember)_dataManager.RelativeMemberDataAdministrator.Items.AddItem();

            rm.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is updated
            rm.Status = DataItemStatusTypes.Modified;

            // Save to the database
            _dataManager.RelativeMemberDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Delete the RelativeMember.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Delete(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Create a new data item
            RelativeMember rm = (RelativeMember)_dataManager.RelativeMemberDataAdministrator.Items.AddItem();

            rm.ID = Int32.Parse(dataWrapper.ID);

            // Set the item status so that the item is deleted
            rm.Status = DataItemStatusTypes.Deleted;

            // Save to the database
            _dataManager.RelativeMemberDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByID(   int id,
                                RelativeConnectionContractTypes connectionContractType,
                                string currentRelativeMemberID,
                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByID(id, connectionContractType, currentRelativeMemberID);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByUserProfileID(string applicationID, 
                                        string userProfileID,
                                        DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByUserProfileID(  applicationID, 
                                                                                    userProfileID);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByEmail(string applicationID, 
                                string email,
                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByEmail(  applicationID, 
                                                                            email);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByEmail(string applicationID,
                                string email,
                                RelativeConnectionContractTypes connectionContractType,
                                string currentRelativeMemberID,
                                DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(email)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "email is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByEmail(  applicationID, 
                                                                            email, 
                                                                            connectionContractType, 
                                                                            currentRelativeMemberID);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByFindText( string applicationID, 
                                    string findText,
                                    string currentRelativeMemberID,
                                    RelativeMemberScopeTypes scopeType,
                                    string previousRelativeMemberID, 
                                    int numberOfItemsToLoad, 
                                    bool selectItemsAfterPreviousYN,
                                    DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (string.IsNullOrEmpty(findText)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "findText is nothing"));
            if (string.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (string.IsNullOrEmpty(previousRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeMemberID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByFindText(   applicationID, 
                                                                                findText, 
                                                                                currentRelativeMemberID,
                                                                                scopeType,
                                                                                previousRelativeMemberID,
                                                                                numberOfItemsToLoad,
                                                                                selectItemsAfterPreviousYN);

            // Put the data in a wrapper
            dataWrapper = this.DataToWrapper(dataWrapper);
        }

        /// <summary>
        /// Loads the RelativeMembers.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void LoadByAspects(  string applicationID,
                                    List<RelativeMemberQueryAspectTypes> aspectTypes,
                                    int maxResults,
                                    string currentRelativeMemberID,
                                    RelativeMemberScopeTypes scopeType,
                                    string previousRelativeMemberID,
                                    int numberOfItemsToLoad,
                                    bool selectItemsAfterPreviousYN,
                                    DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (aspectTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "aspectTypes is nothing"));
            if (string.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (string.IsNullOrEmpty(previousRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeMemberID is nothing"));
            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeMemberDataAdministrator.Initialise();

            // Load the data
            _dataManager.RelativeMemberDataAdministrator.LoadItemsByAspects(    applicationID,
                                                                                aspectTypes,
                                                                                maxResults,
                                                                                currentRelativeMemberID,
                                                                                scopeType,
                                                                                previousRelativeMemberID,
                                                                                numberOfItemsToLoad,
                                                                                selectItemsAfterPreviousYN);

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

            // Put the RelativeMembers in the wrapper
            DataJSONWrapper rmw = _dataManager.RelativeMemberDataAdministrator.ToWrapper();
            rmw.ID = "RelativeMembers";

            dataWrapper.Items.Add(rmw);

            return dataWrapper;
        }

        #endregion
    }
}
