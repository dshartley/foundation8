using System;
using System.Data;
using System.Xml;
using System.IO;
using Smart.Platform.Diagnostics;
using System.Globalization;

namespace Smart.Platform.Data.DataAccessStrategies.Xml
{
    /// <summary>
    /// A base class for classes which handle data access of data in an Xml file
    /// </summary>
    public abstract class XmlDataAccessStrategyBase : IDataAccessStrategy
    {
        protected string        _dataFilePath;
        protected CultureInfo   _cultureInfo;

        #region Constructors

        private XmlDataAccessStrategyBase()
        {
        }

        protected XmlDataAccessStrategyBase(CultureInfo cultureInfo) 
        {
            #region Check Parameters

            if (cultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "cultureInfo is nothing"));

            #endregion

            _cultureInfo = cultureInfo;
        }

        protected XmlDataAccessStrategyBase(string dataFilePath, CultureInfo cultureInfo)
        {
            #region Check Parameters

            if (dataFilePath == string.Empty)   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataFilePath is nothing"));
            if (cultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "cultureInfo is nothing"));

            #endregion

            _dataFilePath   = dataFilePath;
            _cultureInfo    = cultureInfo;
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

        int IDataAccessStrategy.Insert(IDataItem item)
        {
            throw new NotImplementedException();
        }

        void IDataAccessStrategy.Update(IDataItem item)
        {
            throw new NotImplementedException();
        }

        void IDataAccessStrategy.Commit(IDataItemCollection collection)
        {
            #region Check Parameters

            if (collection == null)             throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (!File.Exists(_dataFilePath))    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "the file " + _dataFilePath + " does not exist"));

            #endregion

            //Save the Xml data document
            collection.DataDocument.Save(_dataFilePath);
        }

        void IDataAccessStrategy.Delete(IDataItem item)
        {
            throw new NotImplementedException();
        }

        IDataItemCollection IDataAccessStrategy.Select(IDataItemCollection collection)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            // Create the Xml document and load the data
            XmlDocument dataDocument = new XmlDocument();
            dataDocument.Load(_dataFilePath);

            // Fill the collection with the loaded data
            this.FillCollection(collection, dataDocument);

            return collection;
        }

        IDataItemCollection IDataAccessStrategy.Select(IDataItemCollection collection, int ID)
        {
            throw new NotImplementedException();
        }
        
        IDataItemCollection IDataAccessStrategy.SelectBefore(IDataItemCollection collection, int beforeID, bool includeBeforeIDItem, int numberofItems)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            //throw new NotImplementedException();

            return null;
        }

        IDataItemCollection IDataAccessStrategy.SelectAfter(IDataItemCollection collection, int afterID, bool includeAfterIDItem, int numberofItems)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            //throw new NotImplementedException();

            return null;
        }

        IDataItemCollection IDataAccessStrategy.SelectBetween(IDataItemCollection collection, int fromRowNumber, int toRowNumber, int sortBy)
        {
            #region Check Parameters

            if (collection == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));

            #endregion

            //throw new NotImplementedException();

            return null;
        }

        int IDataAccessStrategy.SelectCount()
        {
            //throw new NotImplementedException();

            return 0;
        }

        void IDataAccessStrategy.ClearOmittedParameterKeys()
        {
            //throw new NotImplementedException();
        }

        void IDataAccessStrategy.SetOmittedParameter(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            //throw new NotImplementedException();
        }

        bool IDataAccessStrategy.IsParameterOmitted(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            bool r = false;

            //throw new NotImplementedException();

            return r;
        }

        IDataItemCollection IDataAccessStrategy.FillCollection(IDataItemCollection collection, DataTable data)
        {
            return null;
        }

        IDataItemCollection IDataAccessStrategy.FillCollection(IDataItemCollection collection, DataTable data, bool append)
        {
            return null;
        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Fills the collection with the data from the specified Xml document.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected abstract IDataItemCollection FillCollection(IDataItemCollection collection, XmlDocument data);

        #endregion
    }
}
