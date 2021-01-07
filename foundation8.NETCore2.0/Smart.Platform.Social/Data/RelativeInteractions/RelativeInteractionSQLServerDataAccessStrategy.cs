using Smart.Platform.Data;
using Smart.Platform.Data.DataAccessStrategies;
using Smart.Platform.Data.DataAccessStrategies.SQLServer;
using Smart.Platform.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Smart.Platform.Social.Data.RelativeInteractions
{
    /// <summary>
    /// Provides SQL Server data access for RelativeInteraction data.
    /// </summary>
    public class RelativeInteractionSQLServerDataAccessStrategy : SQLServerDataAccessStrategyBase, IRelativeInteractionDataAccessStrategy
    {
        public enum StoredProcedureParameterKeys
        {
            ID,
            ApplicationID,
            RecipientTypes,
            DegreesofSeparation
        }

        #region Constructors

        private RelativeInteractionSQLServerDataAccessStrategy() : base() { }

        public RelativeInteractionSQLServerDataAccessStrategy(   string      connectionString,
                                                            CultureInfo cultureInfo)
            : base(connectionString, cultureInfo, "RelativeInteractions")
        { }

        #endregion

        #region IRelativeInteractionDataAccessStrategy Methods

        /// <summary>
        /// Broadcasts the inserted item with the specified ID in the data source.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="recipientTypes">The recipientTypes.</param>
        /// <param name="degreesofSeparation">The degreesofSeparation.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        DataSet IRelativeInteractionDataAccessStrategy.BroadcastOnInsert(   int ID,
                                                                            List<RelativeInteractionBroadcastRecipientTypes> recipientTypes,
                                                                            int degreesofSeparation)
        {
            #region Check Parameters

            if (recipientTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "recipientTypes is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ID.ToString(), ID);
            parameters.Add(StoredProcedureParameterKeys.RecipientTypes.ToString(), this.relativeInteractionBroadcastRecipientTypesFromList(recipientTypes));
            parameters.Add(StoredProcedureParameterKeys.DegreesofSeparation.ToString(), degreesofSeparation);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeInteractionsBroadcastOnInsert", parameters);

            return data;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Builds and returns the collection of parameters. Remove the DateCreated parameter, because this data column 
        /// is only returned by select stored procedures and is not a column in the table.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <param name="outputParameters"></param>
        /// <returns>
        /// A <see cref="ParametersCollection"/> instance.
        /// </returns>
        protected override IParametersCollection BuildParameters(IDataItem item, bool outputParameters)
        {
            // Build the parameter collection in the base class
            IParametersCollection p = base.BuildParameters(item, outputParameters);

            // Remove the DateCreated parameter, because this data column is only returned by select stored procedures
            // and is not a column in the table
            //p.Remove(StoredProcedureParameterKeys.DateCreated.ToString());

            return p;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Does the load data set tables.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.ApplicationException"></exception>
        private void DoLoadDataSetTables(IDataItemCollection collection, DataSet data)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (data == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "data is nothing"));

            #endregion

            // Get dataAdministratorProvider
            IDataAdministratorProvider dataAdministratorProvider = collection.DataAdministrator.DataAdministratorProvider;

            // Fill the RelativeMembers collection
            if (data.Tables.Count >= 2)
            {
                IDataAdministrator relativeMembersDA = dataAdministratorProvider.GetDataAdministrator("RelativeMembers");
                if (relativeMembersDA != null)
                {
                    relativeMembersDA.Load(data.Tables[1]);
                }
            }
        }

        private string relativeInteractionBroadcastRecipientTypesFromList(List<RelativeInteractionBroadcastRecipientTypes> recipientTypes)
        {
            string result = "";

            // Go through each item
            recipientTypes.ForEach(item =>
            {
                if (result.Length > 0)
                {
                    result += ",";
                }

                result += ((int)item).ToString();

            });

            return result;
        }

        #endregion
    }
}
