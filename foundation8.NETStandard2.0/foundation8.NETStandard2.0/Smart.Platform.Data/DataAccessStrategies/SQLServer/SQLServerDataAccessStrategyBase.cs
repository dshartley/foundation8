using System;
using System.Data;
using System.Data.SqlClient;
using Smart.Platform.Diagnostics;
using System.Globalization;

namespace Smart.Platform.Data.DataAccessStrategies.SQLServer
{
    public enum StoredProcedureParameterKeys
    {
        BeforeID,
        IncludeBeforeIDItem,
        AfterID,
        IncludeAfterIDItem,
        RowCount,
        FromRowNumber,
        ToRowNumber,
        SortBy
    }

    /// <summary>
    /// A base class for classes which handle data access on a SQLServer database.
    /// </summary>
    public abstract class SQLServerDataAccessStrategyBase : DataAccessStrategyBase
    {
        // Note: In keeping with storing data in a universal culture format, abstract the responsibility
        // for culture formatting up to the application data layer
        protected CultureInfo _storageCultureInfo = new CultureInfo("en-US");

        #region Constructors

        protected SQLServerDataAccessStrategyBase() : base() { }

        protected SQLServerDataAccessStrategyBase(  string      connectionString,
                                                    CultureInfo cultureInfo)
            : base(connectionString, cultureInfo)
        { }

        protected SQLServerDataAccessStrategyBase(  string      connectionString,
                                                    CultureInfo cultureInfo,
                                                    string      tableName)
            : base(connectionString, cultureInfo, tableName)
        { }
        
        #endregion

        #region Public Override Methods
        
         /// <summary>
        /// Fills the collection with the data from the specified data table.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <returns></returns>
        public override IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data, bool append)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (data == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "data is nothing"));

            #endregion

            // Nb: .NET Core implicitly sets DataSet formatting to current CultureInfo. Here we change the current CultureInfo
            // so that we can specify GetNewItem parameter fromCultureInfo
            CultureInfo currentCultureInfo  = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture      = this._storageCultureInfo;

            // Clear the collection
            if (!append) collection.Clear();

            // Go through each row
            foreach (DataRow row in data.Rows)
            {
                // Create the item
                IDataItem item = collection.GetNewItem(row, this._storageCultureInfo);
                item.Status = DataItemStatusTypes.Unmodified;

                // Add the item to the collection
                collection.AddItem(item);
            }

            // Nb: Reset current CultureInfo
            CultureInfo.CurrentCulture      = currentCultureInfo;

            return collection;
        }       

        /// <summary>
        /// Fills the collection with the data from the specified data table. By default, don't append is false so that
        /// the collection is refreshed.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public override IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (data == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "data is nothing"));

            #endregion

            return this.FillCollection(collection, data, false);
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Builds and returns an empty collection of parameters.
        /// </summary>
        /// <returns>A <see cref="ParametersCollection"/> instance.</returns>
        protected override IParametersCollection BuildParameters()
        {
            SQLServerParametersCollection parameters = new SQLServerParametersCollection();

            return parameters;
        }

        /// <summary>
        /// Builds and returns the collection of parameters.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <returns>A <see cref="ParametersCollection"/> instance.</returns>
        protected override IParametersCollection BuildParameters(IDataItem item, bool outputParameters)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            IParametersCollection   parameters = new SQLServerParametersCollection();

            // Iterate through the property keys to determine the set of parameters          
            foreach (string key in item.GetPropertyKeys())
            {                
                // Check whether the parameter has been specified to be omitted
                if (!((IDataAccessStrategy)this).IsParameterOmitted(key))
                {
                    // Get the property value
                    string          value = item.GetProperty(key, this._storageCultureInfo);

                    // Create the parameter
                    SqlParameter    parameter = (SqlParameter)parameters.Add(key, value);

                    // If it is the primary key ID property then make it an output parameter of type int
                    if (key.ToUpper() == _primaryKeyColumnName.ToUpper())
                    {
                        if (outputParameters) parameter.Direction = ParameterDirection.Output;
                        parameter.SqlDbType = SqlDbType.Int;
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// Runs a command query.
        /// </summary>
        /// <param name="query">The query.</param>
        protected override void RunQueryCommand(string query)
        {
            #region Check Parameters

            if (query == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "query is nothing"));

            #endregion

            SqlHelper.ExecuteNonQuery(  _connectionString,
                                        CommandType.Text,
                                        query);
        }

        /// <summary>
        /// Runs a scalar query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The value returned by the query.</returns>
        protected override object RunQueryScalar(string query)
        {
            #region Check Parameters

            if (query == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "query is nothing"));

            #endregion

            return SqlHelper.ExecuteScalar( _connectionString,
                                            CommandType.Text,
                                            query);
        }

        /// <summary>
        /// Runs a data load query.
        /// </summary>
        /// <param name="query">The query.</param>
        protected override DataSet RunQueryLoad(string query)
        {
            #region Check Parameters

            if (query == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "query is nothing"));

            #endregion

            return SqlHelper.ExecuteDataset(    _connectionString,
                                                CommandType.Text,
                                                query);
        }

        /// <summary>
        /// Runs a command stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        protected override void RunProcedureCommand(string storedProcedureName, IParametersCollection parameters)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));
            if (parameters == null)                     throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            SqlHelper.ExecuteNonQuery(  _connectionString,
                                        CommandType.StoredProcedure,
                                        storedProcedureName,
                                        (SqlParameter[])parameters.ToArray());
        }

        /// <summary>
        /// Runs a command stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        protected override void RunProcedureCommand(string storedProcedureName)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));

            #endregion

            SqlHelper.ExecuteNonQuery(  _connectionString,
                                        CommandType.StoredProcedure,
                                        storedProcedureName);
        }

        /// <summary>
        /// Runs a scalar stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        /// <returns>The value returned by the query.</returns>
        protected override object RunProcedureScalar(string storedProcedureName, IParametersCollection parameters)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));
            if (parameters == null)                     throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            return SqlHelper.ExecuteScalar( _connectionString,
                                            CommandType.StoredProcedure,
                                            storedProcedureName,
                                            (SqlParameter[])parameters.ToArray());
        }

        /// <summary>
        /// Runs a scalar stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>The value returned by the query.</returns>
        protected override object RunProcedureScalar(string storedProcedureName)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));

            #endregion

            return SqlHelper.ExecuteScalar( _connectionString,
                                            CommandType.StoredProcedure,
                                            storedProcedureName);
        }

        /// <summary>
        /// Runs a data load stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        protected override DataSet RunProcedureLoad(string storedProcedureName, IParametersCollection parameters)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));
            if (parameters == null)                     throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            return SqlHelper.ExecuteDataset(    _connectionString,
                                                CommandType.StoredProcedure,
                                                storedProcedureName,
                                                (SqlParameter[])parameters.ToArray());
        }

        /// <summary>
        /// Runs a data load stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        protected override DataSet RunProcedureLoad(string storedProcedureName)
        {
            #region Check Parameters

            if (storedProcedureName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "storedProcedureName is nothing"));

            #endregion

            return SqlHelper.ExecuteDataset(    _connectionString,
                                                CommandType.StoredProcedure,
                                                storedProcedureName);
        }

        # endregion
    }
}
