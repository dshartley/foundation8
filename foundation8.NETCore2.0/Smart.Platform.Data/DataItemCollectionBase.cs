using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Data
{
    /// <summary>
    /// A base class for classes which encapsulate a collection of data items.
    /// </summary>
    public abstract class DataItemCollectionBase : IDataItemCollection
    {
        protected string _defaultCultureInfoName;

        #region Constructors

        protected DataItemCollectionBase() { }

        public DataItemCollectionBase(  IDataAdministrator  dataAdministrator,
                                        string              defaultCultureInfoName)
            : this(new XmlDocument(), dataAdministrator, defaultCultureInfoName)
        { }

        public DataItemCollectionBase(  XmlDocument dataDocument,
                                        string      defaultCultureInfoName)
        {
            #region Check Parameters

            if (dataDocument == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataDocument is nothing"));
            if (defaultCultureInfoName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));

            #endregion

            _dataDocument           = dataDocument;
            _defaultCultureInfoName = defaultCultureInfoName;

            // Create a new collection
            _items = new ArrayList();

            if (_dataDocument.DocumentElement == null)
            {
                _dataDocument.LoadXml("<?xml version='1.0' ?><root></root>");

            }
            // For each node in the data create an item in the collection
            foreach (XmlNode node in _dataDocument.DocumentElement.ChildNodes)
            {
                IDataItem item = this.GetNewItem(node);
                this.AddItemLowOverhead(item);
            }
        }

        public DataItemCollectionBase(  XmlDocument         dataDocument, 
                                        IDataAdministrator  dataAdministrator,
                                        string              defaultCultureInfoName)
            : this(dataDocument, defaultCultureInfoName)
        {
            #region Check Parameters

            if (dataDocument == null)                   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataDocument is nothing"));
            if (dataAdministrator == null)              throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataAdministrator is nothing"));
            if (defaultCultureInfoName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));

            #endregion

            _dataAdministrator = dataAdministrator;
        }

        #endregion

        #region IDataItemCollection Members

        public event ValidationEventHandler ValidationPassed;

        public event ValidationEventHandler ValidationFailed;
        
        public event DataItemModifiedEventHandler DataItemModified;

        public event DataItemStatusChangedEventHandler DataItemStatusChanged;

        public event DataItemPrimaryKeyModifiedEventHandler DataItemPrimaryKeyModified;

        public void Clear()
        {
            foreach (IDataItem item in _items)
            {
                this.RemoveItem(item);
            }
        }

        public int GetNextID()
        {
            int highest = 0;

            // Find the highest current ID
            foreach (IDataItem item in _items)
            {
                if (item.ID >= highest) highest = item.ID;
            }

            return highest + 1;
        }

        public IDataItem AddItem()
        {
            // Create a new item
            IDataItem item = this.GetNewItem();

            return this.AddItem(item);
        }

        public IDataItem AddItem(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if ((_allowDuplicateEmptyIDs & item.ID != 0) && this.Exists(item.ID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "the item id already exists"));
            if (!_allowDuplicateEmptyIDs && this.Exists(item.ID)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "the item id already exists"));

            #endregion

            this.AddItemLowOverhead(item);

            this.OnDataItemStatusChanged(item, String.Format(Resources.DataItemCollectionBase.Messages.AddedItem, new string[] {item.DataType, item.ID.ToString()}));

            return item;
        }

        public void RemoveItem(IDataItem item)
        {
            #region Check Parameters

            if (item == null)           throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (!this.Exists(item.ID))  throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "the item id does not exist"));

            #endregion
                        
            // Get the item from the collection
            IDataItem actualItem = this.GetItem(item.ID);

            // Remove the node
            item.Remove();

            this.OnDataItemStatusChanged(item, String.Format(Resources.DataItemCollectionBase.Messages.RemovedItem, new string[] { item.DataType, item.ID.ToString() }));
        }

        public IDataItem GetItem(int ID)
        {
            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.ID == ID) return item;
            }

            return null;
        }

        public IDataItem GetItem(int propertyEnum, string value)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.GetProperty(propertyEnum).ToLower() == value.ToLower()) return item;
            }

            return null;
        }

        public IDataItem GetItem(string propertyKey, string value)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(propertyKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "propertyKey is nothing"));
            if (string.IsNullOrEmpty(value)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.GetProperty(propertyKey).ToLower() == value.ToLower()) return item;
            }

            return null;
        }

        public ArrayList GetItems(int propertyEnum, string value)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(value)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ArrayList items = new ArrayList();
            
            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.GetProperty(propertyEnum).ToLower() == value.ToLower()) items.Add(item);
            }

            return items;
        }

        public ArrayList GetItems(string propertyKey, string value)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(propertyKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "propertyKey is nothing"));
            if (string.IsNullOrEmpty(value)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ArrayList items = new ArrayList();

            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.GetProperty(propertyKey).ToLower() == value.ToLower()) items.Add(item);
            }

            return items;
        }

        public ArrayList SortBy(int propertyEnum, bool ascending)
        {
            // Sort the array list
            DataItemComparerBase comparer = new DataItemComparerBase(propertyEnum);
            _items.Sort(comparer);
            if (!ascending) _items.Reverse();

            return _items;
        }

        public ArrayList SortBy(DataItemCollectionSortType sortType)
        {
            #region Check Parameters

            if (sortType == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "sortType is nothing"));

            #endregion

            return this.SortBy(sortType.SortPropertyEnum, sortType.Ascending);
        }

        protected XmlDocument _dataDocument;

        public XmlDocument DataDocument
        {
            get { return _dataDocument; }
        }

        protected ArrayList _items;

        public ArrayList Items
        {
            get { return _items; }
        }

        protected IDataAdministrator _dataAdministrator;

        public IDataAdministrator DataAdministrator
        {
            get { return _dataAdministrator; }
        }

        public void SetSetting(string key, object value)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            if (_dataAdministrator == null) throw new ApplicationException(Resources.DataItemCollectionBase.Messages.DataAdministratorIsNull);

            _dataAdministrator.SetSetting(key, value);
        }

        public object GetSetting(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            if (_dataAdministrator == null) throw new ApplicationException(Resources.DataItemCollectionBase.Messages.DataAdministratorIsNull);

            return _dataAdministrator.GetSetting(key);
        }
        
        #endregion

        #region Public Abstract Methods

        public abstract string DataType { get; }

        public abstract IDataItem GetNewItem();

        public abstract IDataItem GetNewItem(XmlNode node);

        public abstract IDataItem GetNewItem(DataRow row, CultureInfo fromCultureInfo);

        #endregion

        #region Protected Abstract Methods

        // Handle the validation passed event
        protected abstract void HandleValidationPassed(object item, int propertyEnum, string message, ValidationResultTypes validationResult);

        // Handle the validation failed event
        protected abstract void HandleValidationFailed(object item, int propertyEnum, string message, ValidationResultTypes validationResult);

        #endregion

        #region Protected Methods

        protected void OnValidationPassed(IHasValidations item, int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (ValidationPassed != null) ValidationPassed(item, propertyEnum, message, resultType);
        }

        protected void OnValidationFailed(IHasValidations item, int propertyEnum, string message, ValidationResultTypes resultType)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (ValidationFailed != null) ValidationFailed(item, propertyEnum, message, resultType);
        }

        protected void OnDataItemModified(IDataItem item, int propertyEnum, string message)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (DataItemModified != null) DataItemModified(item, propertyEnum, message);
        }

        protected void OnDataItemStatusChanged(IDataItem item, string message)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (DataItemStatusChanged != null) DataItemStatusChanged(item, message);
        }

        protected void OnDataItemPrimaryKeyModified(IDataItem item, int previousID)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            if (DataItemPrimaryKeyModified != null) DataItemPrimaryKeyModified(item, previousID);
        }

        protected bool Exists(int ID)
        {
            // Go through each item
            foreach (IDataItem item in _items)
            {
                if (item.ID == ID) return true;
            }

            // If it wasn't found then return false
            return false;
        }

        protected IDataItem AddItemLowOverhead(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            // Set up the event handlers for the item
            item.StatusChanged      += new DataItemStatusChangedEventHandler(OnDataItemStatusChanged);
            item.ValidationPassed   += new ValidationEventHandler(OnValidationPassed);
            item.ValidationFailed   += new ValidationEventHandler(OnValidationFailed);
            item.PrimaryKeyModified += new DataItemPrimaryKeyModifiedEventHandler(OnDataItemPrimaryKeyModified);

            // Add it to the collection
            _items.Add(item);

            // Make sure the item is fully created
            item.EndCreate();

            return item;
        }

        private bool _allowDuplicateEmptyIDs = false;

        protected bool AllowDuplicateEmptyIDs
        {
            get { return _allowDuplicateEmptyIDs; }
            set { _allowDuplicateEmptyIDs = value; }
        }
        
        #endregion
    }
}
