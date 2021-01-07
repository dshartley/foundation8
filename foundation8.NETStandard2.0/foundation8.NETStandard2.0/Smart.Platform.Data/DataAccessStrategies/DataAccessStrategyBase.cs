using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Smart.Platform.Diagnostics;
using System.Globalization;

namespace Smart.Platform.Data.DataAccessStrategies
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
    /// A base class for data access strategy classes
    /// </summary>
    public abstract class DataAccessStrategyBase : IDataAccessStrategy
    {
        protected string                        _connectionString;
        protected CultureInfo                   _cultureInfo;
        protected string                        _tableName;
        protected const string                  _primaryKeyColumnName = "ID";
        protected ArrayList                     _omittedParameterKeys;

        #region Constructors

        protected DataAccessStrategyBase() { }

        protected DataAccessStrategyBase(   string      connectionString,
                                            CultureInfo cultureInfo)
        {
            #region Check Parameters

            if (connectionString == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "connectionString is nothing"));
            if (cultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "cultureInfo is nothing"));

            #endregion

            _connectionString   = connectionString;
            _cultureInfo        = cultureInfo;
        }

        protected DataAccessStrategyBase(   string      connectionString,
                                            CultureInfo cultureInfo,
                                            string      tableName) : this(connectionString, cultureInfo)
        {
            #region Check Parameters

            if (connectionString == string.Empty)   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "connectionString is nothing"));
            if (cultureInfo == null)                throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "cultureInfo is nothing"));
            if (tableName == string.Empty)          throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "tableName is nothing"));

            #endregion

            _tableName          = tableName;
        }

        #endregion

        #region IDataAccessStrategy Methods

        protected IDataAdministratorProvider _dataAdministratorProvider;

        public IDataAdministratorProvider DataAdministratorProvider
        {
            get
            {
                return _dataAdministratorProvider;
            }
            set
            {
                _dataAdministratorProvider = value;
            }
        }

        public virtual int Insert(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters(item, true);

            // Call the stored procedure
            this.RunProcedureCommand("sp_" + _tableName + "Insert", parameters);

            // Set the new primary key in the data item
            this.SetPrimaryKeyOutput(item, parameters);

            // Update the status of the item
            item.Status = DataItemStatusTypes.Unmodified;

            return item.ID;
        }

        void IDataAccessStrategy.Update(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters(item, false);

            // Call the stored procedure
            this.RunProcedureCommand("sp_" + _tableName + "Update", parameters);

            // Update the status of the item
            item.Status = DataItemStatusTypes.Unmodified;
        }

        void IDataAccessStrategy.Commit(IDataItemCollection collection)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Go through each item in the collection
            foreach (IDataItem item in collection.Items)
            {
                // Check the status of the item to determine whether to perform an operation
                switch (item.Status)
                {
                    case DataItemStatusTypes.New:
                        // Insert the item
                        (this as IDataAccessStrategy).Insert(item);
                        break;
                    case DataItemStatusTypes.Unmodified:
                        break;
                    case DataItemStatusTypes.Modified:
                        // Update the item
                        (this as IDataAccessStrategy).Update(item);
                        break;
                    case DataItemStatusTypes.Deleted:
                        // Delete the item
                        (this as IDataAccessStrategy).Delete(item);
                        break;
                    case DataItemStatusTypes.Obsolete:
                        break;
                    default:
                        break;
                }
            }
        }

        void IDataAccessStrategy.Delete(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters();
            parameters.Add(_primaryKeyColumnName, item.ID);

            // Call the stored procedure
            this.RunProcedureCommand("sp_" + _tableName + "Delete", parameters);

            // Update the status of the item
            item.Status = DataItemStatusTypes.Obsolete;
        }

        IDataItemCollection IDataAccessStrategy.Select(IDataItemCollection collection)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_" + _tableName + "Select");
           
            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        IDataItemCollection IDataAccessStrategy.Select(IDataItemCollection collection, int ID)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters();
            parameters.Add(_primaryKeyColumnName, ID);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_" + _tableName + "SelectByID", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        IDataItemCollection IDataAccessStrategy.SelectBefore(IDataItemCollection collection, int beforeID, bool includeBeforeIDItem, int numberofItems)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters();
            parameters.Add(StoredProcedureParameterKeys.BeforeID.ToString(), beforeID);
            parameters.Add(StoredProcedureParameterKeys.IncludeBeforeIDItem.ToString(), includeBeforeIDItem);
            parameters.Add(StoredProcedureParameterKeys.RowCount.ToString(), numberofItems);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_" + _tableName + "SelectBefore", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        IDataItemCollection IDataAccessStrategy.SelectAfter(IDataItemCollection collection, int afterID, bool includeAfterIDItem, int numberofItems)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters();
            parameters.Add(StoredProcedureParameterKeys.AfterID.ToString(), afterID);
            parameters.Add(StoredProcedureParameterKeys.IncludeAfterIDItem.ToString(), includeAfterIDItem);
            parameters.Add(StoredProcedureParameterKeys.RowCount.ToString(), numberofItems);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_" + _tableName + "SelectAfter", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        IDataItemCollection IDataAccessStrategy.SelectBetween(IDataItemCollection collection, int fromRowNumber, int toRowNumber, int sortBy)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Get the parameters collection
            IParametersCollection parameters = this.BuildParameters();
            parameters.Add(StoredProcedureParameterKeys.FromRowNumber.ToString(), fromRowNumber);
            parameters.Add(StoredProcedureParameterKeys.ToRowNumber.ToString(), toRowNumber);
            parameters.Add(StoredProcedureParameterKeys.SortBy.ToString(), sortBy);

            // Call the stored procedure
            DataSet data = this.RunProcedureLoad("sp_" + _tableName + "SelectBetween", parameters);

            // Fill the collection with the loaded data
            this.FillCollection(collection, data.Tables[0]);

            // Process the additional dataset tables
            this.DoLoadDataSetTables(collection, data);

            return collection;
        }

        int IDataAccessStrategy.SelectCount()
        {
            // Call the stored procedure
            int i = (int)this.RunProcedureScalar("sp_" + _tableName + "SelectCount");

            return i;
        }

        void IDataAccessStrategy.ClearOmittedParameterKeys()
        {
            _omittedParameterKeys = new ArrayList();
        }

        void IDataAccessStrategy.SetOmittedParameter(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            if (_omittedParameterKeys == null) _omittedParameterKeys = new ArrayList();
            
            if (!((IDataAccessStrategy)this).IsParameterOmitted(key)) _omittedParameterKeys.Add(key);
        }

        bool IDataAccessStrategy.IsParameterOmitted(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            bool r = false;

            if (_omittedParameterKeys != null)
            {
                foreach (string k in _omittedParameterKeys)
                {
                    if (k == key) r = true;
                }
            }

            return r;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Sets the primary key in the data item using the primary key output parameter.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="parameters">The parameters.</param>
        protected void SetPrimaryKeyOutput(IDataItem item, IParametersCollection parameters)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (parameters == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameters is nothing"));

            #endregion

            // Get the primary key parameter and set the ID of the data item
            DbParameter pkParameter = parameters.Get(_primaryKeyColumnName);
            if (pkParameter == null) return;

            item.ID = Int32.Parse(pkParameter.Value.ToString());
        }

        /// <summary>
        /// Does the load data set tables.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        protected virtual void DoLoadDataSetTables(IDataItemCollection collection, DataSet data)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (data == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "data is nothing"));

            #endregion

            // Not implemented
        }

        # endregion

        #region Public Abstract Methods

        /// <summary>
        /// Fills the collection with the data from the specified data set.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public abstract IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data);

        /// <summary>
        /// Fills the collection with the data from the specified data set.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <returns></returns>
        public abstract IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data, bool append);

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Builds and returns an empty collection of parameters.
        /// </summary>
        /// <returns>A <see cref="ParametersCollection"/> instance.</returns>
        protected abstract IParametersCollection BuildParameters();

        /// <summary>
        /// Builds and returns the collection of parameters.
        /// </summary>
        /// <param name="item">The data item.</param>
        /// <returns>A <see cref="ParametersCollection"/> instance.</returns>
        protected abstract IParametersCollection BuildParameters(IDataItem item, bool outputParameters);
        
        /// <summary>
        /// Runs a command query.
        /// </summary>
        /// <param name="query">The query.</param>
        protected abstract void RunQueryCommand(string query);

        /// <summary>
        /// Runs a scalar query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The value returned by the query.</returns>
        protected abstract object RunQueryScalar(string query);

        /// <summary>
        /// Runs a data load query.
        /// </summary>
        /// <param name="query">The query.</param>
        protected abstract DataSet RunQueryLoad(string query);

        /// <summary>
        /// Runs a command stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        protected abstract void RunProcedureCommand(string storedProcedureName, IParametersCollection parameters);

        /// <summary>
        /// Runs a command stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        protected abstract void RunProcedureCommand(string storedProcedureName);

        /// <summary>
        /// Runs a scalar stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        /// <returns>The value returned by the query.</returns>
        protected abstract object RunProcedureScalar(string storedProcedureName, IParametersCollection parameters);

        /// <summary>
        /// Runs a scalar stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>The value returned by the query.</returns>
        protected abstract object RunProcedureScalar(string storedProcedureName);

        /// <summary>
        /// Runs a data load stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The collection of parameters.</param>
        protected abstract DataSet RunProcedureLoad(string storedProcedureName, IParametersCollection parameters);

        /// <summary>
        /// Runs a data load stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        protected abstract DataSet RunProcedureLoad(string storedProcedureName);

        #endregion
    }
}
