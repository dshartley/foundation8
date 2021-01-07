using Smart.Platform.Data;
using Smart.Platform.Data.DataAccessStrategies;
using Smart.Platform.Data.DataAccessStrategies.SQLServer;
using Smart.Platform.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Smart.Platform.Social.Data.RelativeTimelineEvents
{
    /// <summary>
    /// Provides SQL Server data access for RelativeTimelineEvent data.
    /// </summary>
    public class RelativeTimelineEventSQLServerDataAccessStrategy : SQLServerDataAccessStrategyBase, IRelativeTimelineEventDataAccessStrategy
    {
        public enum StoredProcedureParameterKeys
        {
            ApplicationID,
            ForRelativeMemberID,
            CurrentRelativeMemberID,
            RelativeInteractionID,
            PreviousRelativeTimelineEventID,
            NumberOfItemsToLoad,
            SelectItemsAfterPreviousYN,
            ScopeType,
            RelativeTimelineEventTypes
        }

        #region Constructors

        private RelativeTimelineEventSQLServerDataAccessStrategy() : base() { }

        public RelativeTimelineEventSQLServerDataAccessStrategy(   string      connectionString,
                                                            CultureInfo cultureInfo)
            : base(connectionString, cultureInfo, "RelativeTimelineEvents")
        { }

        #endregion

        #region IRelativeTimelineEventDataAccessStrategy Methods

        /// <summary>
        /// Selects the by forRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">The forRelativeMemberID.</param>
        /// <param name="currentRelativeMemberID">The currentRelativeMemberID.</param>
        /// <param name="scopeType">The scopeType.</param>
        /// <param name="relativeTimelineEventTypes">The relativeTimelineEventTypes.</param>
        /// <param name="previousRelativeTimelineEventID">The previousRelativeTimelineEventID.</param>
        /// <param name="numberOfItemsToLoad">The numberOfItemsToLoad.</param>
        /// <param name="selectItemsAfterPreviousYN">The selectItemsAfterPreviousYN.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        /// <exception cref="System.ApplicationException"></exception>
        IDataItemCollection IRelativeTimelineEventDataAccessStrategy.SelectByForRelativeMemberID(   IDataItemCollection collection,
                                                                                                    string applicationID,
                                                                                                    string forRelativeMemberID,
                                                                                                    string currentRelativeMemberID,
                                                                                                    RelativeTimelineEventScopeTypes scopeType,
                                                                                                    List<RelativeTimelineEventTypes> relativeTimelineEventTypes,
                                                                                                    string previousRelativeTimelineEventID,
                                                                                                    int numberOfItemsToLoad,
                                                                                                    bool selectItemsAfterPreviousYN)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (String.IsNullOrEmpty(forRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "forRelativeMemberID is nothing"));
            if (String.IsNullOrEmpty(currentRelativeMemberID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "currentRelativeMemberID is nothing"));
            if (relativeTimelineEventTypes == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeTimelineEventTypes is nothing"));
            if (String.IsNullOrEmpty(previousRelativeTimelineEventID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "previousRelativeTimelineEventID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.ForRelativeMemberID.ToString(), forRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.CurrentRelativeMemberID.ToString(), currentRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ScopeType.ToString(), (int)scopeType);
            parameters.Add(StoredProcedureParameterKeys.RelativeTimelineEventTypes.ToString(), this.relativeTimelineEventTypesFromList(relativeTimelineEventTypes));
            parameters.Add(StoredProcedureParameterKeys.PreviousRelativeTimelineEventID.ToString(), previousRelativeTimelineEventID);
            parameters.Add(StoredProcedureParameterKeys.NumberOfItemsToLoad.ToString(), numberOfItemsToLoad);
            parameters.Add(StoredProcedureParameterKeys.SelectItemsAfterPreviousYN.ToString(), selectItemsAfterPreviousYN);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeTimelineEventsSelectByForRelativeMemberIDPreviousRelativeTimelineEventID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by relativeInteractionID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="relativeInteractionID">The relativeInteractionID.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        /// <exception cref="System.ApplicationException"></exception>
        IDataItemCollection IRelativeTimelineEventDataAccessStrategy.SelectByRelativeInteractionID( IDataItemCollection collection,
                                                                                                    string applicationID,
                                                                                                    string relativeInteractionID)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));
            if (String.IsNullOrEmpty(relativeInteractionID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "relativeInteractionID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.RelativeInteractionID.ToString(), relativeInteractionID);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeTimelineEventsSelectByRelativeInteractionID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
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

        /// <summary>
        /// Does the load data set tables.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <exception cref="System.ApplicationException"></exception>
        protected override void DoLoadDataSetTables(IDataItemCollection collection, DataSet data)
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

            // Fill the RelativeInteractions collection
            if (data.Tables.Count >= 3)
            {
                IDataAdministrator relativeInteractionsDA = dataAdministratorProvider.GetDataAdministrator("RelativeInteractions");
                if (relativeInteractionsDA != null)
                {
                    relativeInteractionsDA.Load(data.Tables[2]);
                }
            }

        }

        #endregion

        #region Private Methods

        private string relativeTimelineEventTypesFromList(List<RelativeTimelineEventTypes> relativeTimelineEventTypes)
        {
            string result = "";

            // Go through each item
            relativeTimelineEventTypes.ForEach(item =>
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
