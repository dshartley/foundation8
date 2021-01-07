using Smart.Platform.Data;
using Smart.Platform.Data.DataAccessStrategies;
using Smart.Platform.Data.DataAccessStrategies.SQLServer;
using Smart.Platform.Diagnostics;
using System;
using System.Data;
using System.Globalization;

namespace Smart.Platform.Social.Data.RelativeConnections
{
    /// <summary>
    /// Provides SQL Server data access for RelativeConnection data.
    /// </summary>
    public class RelativeConnectionSQLServerDataAccessStrategy : SQLServerDataAccessStrategyBase, IRelativeConnectionDataAccessStrategy
    {
        public enum StoredProcedureParameterKeys
        {
            ApplicationID,
            FromRelativeMemberID,
            ToRelativeMemberID,
            WithRelativeMemberID,
            ForRelativeMemberID,
            ConnectionContractType
        }

        #region Constructors

        private RelativeConnectionSQLServerDataAccessStrategy() : base() { }

        public RelativeConnectionSQLServerDataAccessStrategy(   string      connectionString,
                                                            CultureInfo cultureInfo)
            : base(connectionString, cultureInfo, "RelativeConnections")
        { }

        #endregion

        #region IRelativeConnectionDataAccessStrategy Methods

        /// <summary>
        /// Selects the by fromRelativeMemberID and toRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="fromRelativeMemberID">The fromRelativeMemberID.</param>
        /// <param name="toRelativeMemberID">The toRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionDataAccessStrategy.SelectByFromRelativeMemberIDToRelativeMemberID(   IDataItemCollection collection,
                                                                                                                    string applicationID,
                                                                                                                    string fromRelativeMemberID,
                                                                                                                    string toRelativeMemberID,
                                                                                                                    RelativeConnectionContractTypes connectionContractType)
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
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(),  (int)connectionContractType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionsSelectByFromRelativeMemberIDToRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by forRelativeMemberID and withRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="forRelativeMemberID">The forRelativeMemberID.</param>
        /// <param name="withRelativeMemberID">The withRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionDataAccessStrategy.SelectByForRelativeMemberIDWithRelativeMemberID(  IDataItemCollection collection,
                                                                                                                    string applicationID,
                                                                                                                    string forRelativeMemberID,
                                                                                                                    string withRelativeMemberID,
                                                                                                                    RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.ForRelativeMemberID.ToString(),     forRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.WithRelativeMemberID.ToString(),    withRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(),  (int)connectionContractType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionsSelectByForRelativeMemberIDWithRelativeMemberID", parameters);

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
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionDataAccessStrategy.SelectByFromRelativeMemberID( IDataItemCollection collection,
                                                                                                string applicationID,
                                                                                                string fromRelativeMemberID,
                                                                                                RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.FromRelativeMemberID.ToString(),    fromRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(),  (int)connectionContractType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionsSelectByFromRelativeMemberID", parameters);

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
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionDataAccessStrategy.SelectByToRelativeMemberID(   IDataItemCollection collection,
                                                                                                string applicationID,
                                                                                                string toRelativeMemberID,
                                                                                                RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.ToRelativeMemberID.ToString(),      toRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(),  (int)connectionContractType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionsSelectByToRelativeMemberID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        /// <summary>
        /// Selects the by withRelativeMemberID.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="applicationID">The application identifier.</param>
        /// <param name="withRelativeMemberID">The withRelativeMemberID.</param>
        /// <param name="connectionContractType">The connectionContractType.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        IDataItemCollection IRelativeConnectionDataAccessStrategy.SelectByWithRelativeMemberID( IDataItemCollection collection,
                                                                                                string applicationID,
                                                                                                string withRelativeMemberID,
                                                                                                RelativeConnectionContractTypes connectionContractType)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (String.IsNullOrEmpty(applicationID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "applicationID is nothing"));

            #endregion

            // Get the parameters collection
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();
            parameters.Add(StoredProcedureParameterKeys.ApplicationID.ToString(), applicationID);
            parameters.Add(StoredProcedureParameterKeys.WithRelativeMemberID.ToString(),    withRelativeMemberID);
            parameters.Add(StoredProcedureParameterKeys.ConnectionContractType.ToString(),  (int)connectionContractType);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_RelativeConnectionsSelectByWithRelativeMemberID", parameters);

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

        #endregion
    }
}
