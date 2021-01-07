using Smart.Platform.Data;
using Smart.Platform.Data.DataAccessStrategies;
using Smart.Platform.Data.DataAccessStrategies.SQLServer;
using Smart.Platform.Diagnostics;
using System;
using System.Data;
using System.Globalization;

namespace Smart.Platform.Social.Data.RelativeConnectionRequests
{
    /// <summary>
    /// Provides SQL Server data access for RelativeConnectionRequest data.
    /// </summary>
    public class RelativeConnectionRequestSQLServerDataAccessStrategy : SQLServerDataAccessStrategyBase, IRelativeConnectionRequestDataAccessStrategy
    {
        public enum StoredProcedureParameterKeys
        {
            ApplicationID,
            FromRelativeMemberID,
            ToRelativeMemberID,
            RequestType
        }

        #region Constructors

        private RelativeConnectionRequestSQLServerDataAccessStrategy() : base() { }

        public RelativeConnectionRequestSQLServerDataAccessStrategy(   string      connectionString,
                                                            CultureInfo cultureInfo)
            : base(connectionString, cultureInfo, "RelativeConnectionRequests")
        { }

        #endregion

        #region IRelativeConnectionRequestDataAccessStrategy Methods

        /// <summary>
        /// Selects the by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionRequestDataAccessStrategy.SelectByFromRelativeMemberIDToRelativeMemberID(IDataItemCollection collection,
                                                                                                                        string applicationID,
                                                                                                                        string fromRelativeMemberID,
                                                                                                                        string toRelativeMemberID,
                                                                                                                        RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.FromRelativeMemberID.ToString(),    fromRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ToRelativeMemberID.ToString(),      toRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.RequestType.ToString(),             (int)requestType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionRequestsSelectByFromRelativeMemberIDToRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by fromRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionRequestDataAccessStrategy.SelectByFromRelativeMemberID(  IDataItemCollection collection,
                                                                                                        string applicationID,
                                                                                                        string fromRelativeMemberID,
                                                                                                        RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.FromRelativeMemberID.ToString(),    fromRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.RequestType.ToString(),             (int)requestType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionRequestsSelectByFromRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="requestType">The requestType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionRequestDataAccessStrategy.SelectByToRelativeMemberID(IDataItemCollection collection,
                                                                                                    string applicationID,
                                                                                                    string toRelativeMemberID,
                                                                                                    RelativeConnectionRequestTypes requestType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.ToRelativeMemberID.ToString(),  toRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.RequestType.ToString(),         (int)requestType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionRequestsSelectByToRelativeMemberID", parameters);

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

        }

        #endregion

        #region Private Methods

        #endregion
    }
}
