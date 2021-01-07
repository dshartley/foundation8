using Smart.Platform.Data;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using System;
using System.Collections.Generic;
using System.Data;

namespace Smart.Platform.Social.Data.RelativeInteractions
{
    /// <summary>
    /// Manages RelativeInteraction data.
    /// </summary>
    public class RelativeInteractionDataAdministrator : DataAdministratorBase
    {
        #region Constructors

        private RelativeInteractionDataAdministrator() : base() { }

        public RelativeInteractionDataAdministrator(IDataManagementPolicy dataManagementPolicy,
                                                    IDataAccessStrategy dataAccessStrategy,
                                                    string defaultCultureInfoName,
                                                    IDataAdministratorProvider dataAdministratorProvider)
            : base(dataManagementPolicy, dataAccessStrategy, defaultCultureInfoName, dataAdministratorProvider)
        { }

        #endregion

        #region Protected Override Methods

        protected override IDataItemCollection NewCollection()
        {
            return new RelativeInteractionCollection(this, _defaultCultureInfoName);
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
        /// Broadcasts the inserted item with the specified ID. Returns a list of Relative Member IDs and Relative Timeline Event IDs
        /// which were created by the broadcast.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="recipientTypes">The recipientTypes.</param>
        /// <param name="degreesofSeparation">The degreesofSeparation.</param>
        public DataJSONWrapper BroadcastOnInsert(   int ID,
                                                    List<RelativeInteractionBroadcastRecipientTypes> recipientTypes,
                                                    int degreesofSeparation)
        {
            #region Check Parameters

            if (recipientTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "recipientTypes is nothing"));

            #endregion

            // Broadcast
            DataSet             data = ((IRelativeInteractionDataAccessStrategy)_dataAccessStrategy).BroadcastOnInsert(ID, recipientTypes, degreesofSeparation);

            // Get data as wrappers
            DataJSONWrapper     result = this.DataToWrapper(data);

            return result;
        }

        #endregion

        #region Private Methods

        private DataJSONWrapper DataToWrapper(DataSet data)
        {
            #region Check Parameters

            if (data == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "data is nothing"));

            #endregion

            DataJSONWrapper result = new DataJSONWrapper();

            // Check number of tables
            if (data.Tables.Count == 0) return result;

            // Go through each table
            for (int i = 0; i <= data.Tables.Count - 1; i++)
            {
                // Create wrapper for the table
                DataJSONWrapper tableWrapper = new DataJSONWrapper();

                switch (i)
                {
                    case 0:
                        tableWrapper.ID = "RelativeTimelineEvents";
                        break;
                    case 1:
                        tableWrapper.ID = "RelativeInteractions";
                        break;
                    case 2:
                        tableWrapper.ID = "RelativeMembers";
                        break;
                    default:
                        break;
                }

                // Go through each row
                foreach (DataRow row in data.Tables[i].Rows)
                {
                    // Create wrapper for the row
                    DataJSONWrapper rowWrapper = new DataJSONWrapper();

                    // Go through each column
                    foreach (DataColumn column in data.Tables[i].Columns)
                    {
                        string key = column.ColumnName;
                        string value = row[column.ColumnName].ToString();

                        // Check ID
                        if (key == "ID") rowWrapper.ID = value;

                        rowWrapper.Params.Add(new ParameterJSONWrapper(key, value));
                    }

                    // Add wrapper
                    tableWrapper.Items.Add(rowWrapper);
                }

                // Add wrapper
                result.Items.Add(tableWrapper);
            }

            return result;
        }

        #endregion
    }
}
