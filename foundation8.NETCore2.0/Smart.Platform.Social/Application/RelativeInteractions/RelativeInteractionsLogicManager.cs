using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using Smart.Platform.Social.Data;
using Smart.Platform.Social.Data.RelativeInteractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Smart.Platform.Social.Application.RelativeInteractions
{
    /// <summary>
    /// A delegate type for a callback on broadcast for relative interaction inserted.
    /// </summary>
    /// <param name="result">The result.</param>
    public delegate void RelativeInteractionBroadcastOnInsertDelegate(RelativeInteraction ri, DataJSONWrapper broadcastResult);

    /// <summary>
    /// Manages the application layer for the RelativeInteractions.
    /// </summary>
    public class RelativeInteractionsLogicManager
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        private CultureInfo _fromCultureInfo = new CultureInfo("en-GB");

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeInteractionsLogicManager"/> class.
        /// </summary>
        public RelativeInteractionsLogicManager() { }

        #endregion

        #region Public Methods

        private RelativeInteractionBroadcastOnInsertDelegate _relativeInteractionBroadcastOnInsertDelegate;

        /// <summary>
        /// Gets or sets the relative interaction broadcast on insert delegate.
        /// </summary>
        /// <value>
        /// The relative interaction broadcast on insert delegate.
        /// </value>
        public RelativeInteractionBroadcastOnInsertDelegate RelativeInteractionBroadcastOnInsertDelegate
        {
            get { return _relativeInteractionBroadcastOnInsertDelegate; }
            set { _relativeInteractionBroadcastOnInsertDelegate = value; }
        }

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
        /// Insert the RelativeInteraction.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Insert(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeInteractionDataAdministrator.Initialise();

            // Create a new data item
            RelativeInteraction ri = (RelativeInteraction)_dataManager.RelativeInteractionDataAdministrator.Items.AddItem();

            ri.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is inserted
            ri.Status = DataItemStatusTypes.New;

            // Save to the database
            _dataManager.RelativeInteractionDataAdministrator.Save();

            // Broadcast
            this.BroadcastOnInsert(ri);

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();
            dataWrapper.ID = ri.ID.ToString();

        }

        /// <summary>
        /// Update the RelativeInteraction.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Update(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeInteractionDataAdministrator.Initialise();

            // Create a new data item
            RelativeInteraction ri = (RelativeInteraction)_dataManager.RelativeInteractionDataAdministrator.Items.AddItem();

            ri.CopyFromWrapper(dataWrapper, this._fromCultureInfo);

            // Set the item status so that the item is updated
            ri.Status = DataItemStatusTypes.Modified;

            // Save to the database
            _dataManager.RelativeInteractionDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        /// <summary>
        /// Delete the RelativeInteraction.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void Delete(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            _dataManager.RelativeInteractionDataAdministrator.Initialise();

            // Create a new data item
            RelativeInteraction ri = (RelativeInteraction)_dataManager.RelativeInteractionDataAdministrator.Items.AddItem();

            ri.ID = Int32.Parse(dataWrapper.ID);

            // Set the item status so that the item is deleted
            ri.Status = DataItemStatusTypes.Deleted;

            // Save to the database
            _dataManager.RelativeInteractionDataAdministrator.Save();

            // Set the parameters in the JSON wrapper to be returned
            dataWrapper.Items.Clear();
            dataWrapper.Params.Clear();

        }

        #endregion

        #region Private Methods

        private DataJSONWrapper DataToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.Items.Clear();

            // Put the RelativeInteractions in the wrapper
            DataJSONWrapper riw = _dataManager.RelativeInteractionDataAdministrator.ToWrapper();
            riw.ID = "RelativeInteractions";

            dataWrapper.Items.Add(riw);

            if (_dataManager.RelativeMemberDataAdministrator != null)
            {
                // Put the RelativeMembers in the wrapper
                DataJSONWrapper rmw = _dataManager.RelativeMemberDataAdministrator.ToWrapper();
                rmw.ID = "RelativeMembers";

                dataWrapper.Items.Add(rmw);
            }

            return dataWrapper;
        }

        private void BroadcastOnInsert(RelativeInteraction item)
        {
            // Create thread
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoBroadcastOnInsert), item);
        }

        private void DoBroadcastOnInsert(object arg)
        {
            // Get RelativeInteraction from arg
            RelativeInteraction ri = arg as RelativeInteraction;

            // recipientTypes
            List<RelativeInteractionBroadcastRecipientTypes> recipientTypes = new List<RelativeInteractionBroadcastRecipientTypes>();

            // degreesofSeparation
            int degreesofSeparation = 1;

            // Configure the broadcast depending on the InteractionType
            switch (ri.InteractionType)
            {
                case RelativeInteractionTypes.PostFriendContractMyFeed:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.FriendContractMembers);

                    break;

                case RelativeInteractionTypes.PostFriendContractFriendFeed:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.ToMember);
                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.FriendContractMembers);
                    degreesofSeparation = 2;

                    break;

                case RelativeInteractionTypes.PostFollowContractMyFeed:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.FollowContractMembers);

                    break;

                case RelativeInteractionTypes.Handshake:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.ToMember);

                    break;

                case RelativeInteractionTypes.PostContractlessMyFeed:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.ContractlessContractMembers);

                    break;

                case RelativeInteractionTypes.PostContractlessMemberFeed:

                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.ToMember);
                    recipientTypes.Add(RelativeInteractionBroadcastRecipientTypes.ContractlessContractMembers);
                    degreesofSeparation = 2;

                    break;

                default:

                    break;
            }

            // Broadcast
            DataJSONWrapper broadcastResult = _dataManager.RelativeInteractionDataAdministrator.BroadcastOnInsert(ri.ID, recipientTypes, degreesofSeparation);

            // Call delegate
            _relativeInteractionBroadcastOnInsertDelegate?.Invoke(ri, broadcastResult);
        }

        #endregion
    }
}
