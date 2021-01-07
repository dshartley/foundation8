using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;

namespace Smart.Platform.Data
{
    /// <summary>
    /// A base class for data administrators.
    /// </summary>
    public abstract class DataAdministratorBase : IDataAdministrator
    {
        protected string                        _defaultCultureInfoName;
        protected IDataAdministratorProvider    _dataAdministratorProvider;

        #region Constructors

        protected DataAdministratorBase() { }

        public DataAdministratorBase(   IDataManagementPolicy       dataManagementPolicy,
                                        IDataAccessStrategy         dataAccessStrategy,
                                        string                      defaultCultureInfoName,
                                        IDataAdministratorProvider  dataAdministratorProvider)
        {
            #region Check Parameters

            if (dataManagementPolicy == null)           throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataManagementPolicy is nothing"));
            if (dataAccessStrategy == null)             throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataAccessStrategy is nothing"));
            if (defaultCultureInfoName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));
            if (dataAdministratorProvider == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataAdministratorProvider is nothing"));

            #endregion

            _dataManagementPolicy       = dataManagementPolicy;
            _dataManagementPolicy.Initialise(this);
            _dataAccessStrategy         = dataAccessStrategy;
            _defaultCultureInfoName     = defaultCultureInfoName;
            _dataAdministratorProvider  = dataAdministratorProvider;

            this.SetupCollection();
            this.SetupForeignKeys();
            this.SetupOmittedKeys();
        }

        #endregion

        #region IDataAdministrator Members

        public event ValidationEventHandler ValidationPassed;

        public event ValidationEventHandler ValidationFailed;

        public event DataItemModifiedEventHandler DataItemModified;

        public event DataItemStatusChangedEventHandler DataItemStatusChanged;

        public event DataItemPrimaryKeyModifiedEventHandler DataItemPrimaryKeyModified;

        public event DataAdministratorEventHandler DataLoaded;

        public event DataAdministratorEventHandler DataSaved;

        public void Initialise()
        {
            // Default flags
            _dataIsLoaded   = false;
            _dataIsSaved    = true;

            // Setup the collection
            this.SetupCollection();
        }

        protected IDataAccessStrategy _dataAccessStrategy;

        public IDataAccessStrategy DataAccessStrategy
        {
            get
            {
                return _dataAccessStrategy;
            }
            set
            {
                _dataAccessStrategy = value;
            }
        }

        protected IDataManagementPolicy _dataManagementPolicy;

        public IDataManagementPolicy DataManagementPolicy
        {
            get
            {
                return _dataManagementPolicy;
            }
            set
            {
                _dataManagementPolicy = value;
            }
        }

        protected IDataItemCollection _items;

        public IDataItemCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }

        protected bool _dataIsLoaded = false;

        public bool DataIsLoaded
        {
            get { return _dataIsLoaded; }
        }

        protected bool _dataIsSaved = true;

        public bool DataIsSaved
        {
            get { return _dataIsSaved; }
        }

        public void Load()
        {
            this.SetupCollection();

            // Load the data
            _items = _dataAccessStrategy.Select(_items);

            this.DoAfterLoad();
        }

        public void Load(DataTable dataTable)
        {
            #region Check Parameters

            if (dataTable == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataTable is nothing"));

            #endregion

            this.SetupCollection();

            // Load the data
            _dataAccessStrategy.FillCollection(_items, dataTable);

            this.DoAfterLoad();
        }

        public void Load(int ID)
        {
            this.SetupCollection();

            // Load the data
            _items = _dataAccessStrategy.Select(_items, ID);

            this.DoAfterLoad();
        }
        
        public void LoadItemsBefore(int beforeID, bool includeBeforeIDItem, int numberofItems)
        {
            this.SetupCollection();

            // Load the data
            _items = _dataAccessStrategy.SelectBefore(_items, beforeID, includeBeforeIDItem, numberofItems);

            this.DoAfterLoad();
        }

        public void LoadItemsAfter(int afterID, bool includeAfterIDItem, int numberofItems)
        {
            this.SetupCollection();

            // Load the data
            _items = _dataAccessStrategy.SelectAfter(_items, afterID, includeAfterIDItem, numberofItems);

            this.DoAfterLoad();
        }

        public void LoadItemsBetween(int fromRowNumber, int toRowNumber, int sortBy)
        {
            this.SetupCollection();

            // Load the data
            _items = _dataAccessStrategy.SelectBetween(_items, fromRowNumber, toRowNumber, sortBy);

            this.DoAfterLoad();
        }

        public int CountAll()
        {
            int i = _dataAccessStrategy.SelectCount();

            return i;
        }

        public virtual void Save()
        {
            // Save the data
            _dataAccessStrategy.Commit(_items);

            this.DoAfterSave();
        }

        public IDataItem GetNewItem()
        {
            return _items.GetNewItem();
        }

        public IDataItem AddItem(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            return _items.AddItem(item);
        }

        public void RemoveItem(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            _items.RemoveItem(item);
        }

        public IDataItem CopyItem(IDataItem item)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            // Get a copy of the Xml node
            XmlNode copy = item.Node.Clone();

            // Get the copy of the item by asking the collection for a new item using the specified Xml node
            IDataItem returnItem = _items.GetNewItem(copy);

            return returnItem;
        }

        public DataJSONWrapper ToWrapper()
        {
            #region Check Parameters

            #endregion

            DataJSONWrapper dataWrapper = new DataJSONWrapper();

            // Check the data
            if (this.Items.Items != null && this.Items.Items.Count > 0)
            {
                // Go through each item
                foreach (IDataItem dataItem in this.Items.Items)
                {
                    // Put the item in a wrapper
                    DataJSONWrapper wrapper = new DataJSONWrapper();
                    dataItem.CopyToWrapper(wrapper);

                    dataWrapper.Items.Add(wrapper);
                }
            }

            return dataWrapper;
        }

        public IDataAdministratorProvider DataAdministratorProvider
        {
            get { return _dataAdministratorProvider; }
        }
        
        protected Dictionary<string, object> _settings;

        public void SetSetting(string key, object value)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            // Create the settings collection if it is null
            if (_settings == null)
            {
                _settings = new Dictionary<string, object>();
            }

            if (_settings.ContainsKey(key))
            {
                _settings[key] = value;
            }
            else
            {
                _settings.Add(key, value);
            }
        }

        public object GetSetting(string key)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            if (_settings != null && _settings.ContainsKey(key))
            {
                return _settings[key];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Public Abstract Methods

        public abstract void HandleDataItemModified(IDataItem item, int propertyEnum, string message);

        #endregion

        #region Protected Abstract Methods

        protected abstract IDataItemCollection NewCollection();

        /// <summary>
        /// Sets up the foreign keys. To set up a foreign key get the data administrator for the relevant foreign key
        /// from the data administrator provider and handle the DataItemPrimaryKeyModified event. In handling this event
        /// update the foreign key of items in the collection accordingly.
        /// </summary>
        protected abstract void SetupForeignKeys();

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

            ConsoleManager.Write(message, ConsoleMessageTypes.Information);

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

        protected void OnDataLoaded()
        {
            if (DataLoaded != null) DataLoaded();
        }

        protected void OnDataSaved()
        {
            if (DataSaved != null) DataSaved();
        }

        protected void SetupCollection()
        {
            _items = this.NewCollection();

            if (_items != null)
            {
                // Set up the event handlers for the collection
                _items.DataItemModified += new DataItemModifiedEventHandler(OnDataItemModified);
                _items.DataItemStatusChanged += new DataItemStatusChangedEventHandler(OnDataItemStatusChanged);
                _items.ValidationPassed += new ValidationEventHandler(OnValidationPassed);
                _items.ValidationFailed += new ValidationEventHandler(OnValidationFailed);
                _items.DataItemPrimaryKeyModified += new DataItemPrimaryKeyModifiedEventHandler(OnDataItemPrimaryKeyModified);
            }
        }

        protected void DoAfterLoad()
        {
            _dataIsLoaded = true;
            this.OnDataLoaded();
        }

        protected void DoAfterSave()
        {
            _dataIsSaved = true;
            ConsoleManager.Write(String.Format(Resources.DataAdministratorBase.Messages.Saved, _items.DataType), ConsoleMessageTypes.Save);

            this.OnDataSaved();
        }

        /// <summary>
        /// Sets up the omitted keys.
        /// </summary>
        protected virtual void SetupOmittedKeys()
        {

        }

        #endregion
    }
}
