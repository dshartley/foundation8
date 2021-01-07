using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;

namespace Smart.Platform.Data
{
    /// <summary>
    /// A base class for classes which encapsulate a data item.
    /// </summary>
    public abstract class DataItemBase : HasValidationsBase, IDataItem
    {
        // The data item is kept in an XML node
        protected XmlNode       _node;
        protected bool          _isDataCreated      = false;

        // These indexes define the range of properties in the enum
        protected int           _startEnumIndex     = 0;
        protected int           _endEnumIndex       = 0;

        // These indexes are included in the search operation
        protected ArrayList     _searchPropertyEnumIndexes;

        // Culture information objects are used to define how data is stored and how data is output
        protected CultureInfo   _storageCultureInfo;
        protected CultureInfo   _outputCultureInfo;

        #region Constructors

        protected DataItemBase() 
        {
            _history        = new ArrayList();
            _historyVersion = 1;
        }

        public DataItemBase(XmlDocument         dataDocument,
                            IDataItemCollection collection,
                            string              defaultCultureInfoName) : this()
        {
            #region Check Parameters

            if (dataDocument == null)                   throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataDocument is nothing"));
            if (collection == null)                     throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (defaultCultureInfoName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));

            #endregion

            _collection         = collection;
            _storageCultureInfo = new CultureInfo(defaultCultureInfoName);

            // Create the node, but don't add it to the Xml document yet
            _node = dataDocument.CreateNode(XmlNodeType.Element, this.DataType, this.DataType + "s");

            this.Status = DataItemStatusTypes.New;
            this.Setup();            
        }

        public DataItemBase(XmlNode             dataNode,
                            IDataItemCollection collection,
                            string              defaultCultureInfoName) : this()
        {
            #region Check Parameters

            if (dataNode == null)                       throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataNode is nothing"));
            if (collection == null)                     throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "collection is nothing"));
            if (defaultCultureInfoName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "defaultCultureInfoName is nothing"));

            #endregion

            _collection             = collection;
            _node                   = dataNode;
            _storageCultureInfo     = new CultureInfo(defaultCultureInfoName);

            // The existing data item is already in the Xml document
            _isDataCreated = true;

            this.Status = DataItemStatusTypes.Unmodified;
            this.Setup();
        }

        #endregion

        #region Protected Methods

        protected void Setup()
        {
            // Setup the data node
            this.InitialiseDataNode();

            // Setup the data item
            this.InitialiseDataItem();

            // Setup the property indexes and keys
            this.InitialisePropertyIndexes();
            this.InitialisePropertyKeys();
            this.InitialiseSearchPropertyIndexes();

            // Setup the culture
            _outputCultureInfo = _storageCultureInfo;
        }

        protected void SetAttribute(string key, string value)
        {
            #region Check Parameters

            if (key == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (value == null)          throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            // If the attribute doesn't exist then create it
            if (_node.Attributes[key] == null)
            {
                _node.Attributes.Append(_node.OwnerDocument.CreateAttribute(key));
            }
            
            _node.Attributes[key].Value = value;
        }

        protected Dictionary<string, int> _keys = new Dictionary<string, int>();

        protected void OnModified(IDataItem item, int propertyEnum, string message)
        {
            #region Check Parameters

            if (item == null)               throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty)    throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (Modified != null) Modified(item, propertyEnum, message);

            if (this.Status == DataItemStatusTypes.Unmodified) this.Status = DataItemStatusTypes.Modified;
        }

        protected void OnStatusChanged(IDataItem item, string message)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));
            if (message == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "message is nothing"));

            #endregion

            if (StatusChanged != null) StatusChanged(item, message);
        }

        protected void OnPrimaryKeyModified(IDataItem item, int previousID)
        {
            #region Check Parameters

            if (item == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "item is nothing"));

            #endregion

            if (PrimaryKeyModified != null) PrimaryKeyModified(item, previousID);
        }

        protected String DoParseToStorageDateString(string value, CultureInfo fromCultureInfo)
        {
            String      result = "";

            // Parse the value to a DateTime using fromCultureInfo
            DateTime    d;
            bool        parseResult = DateTime.TryParse(value, fromCultureInfo, DateTimeStyles.NoCurrentDateDefault, out d);

            if (parseResult)
            {
                // Get string from DateTime using the storageCultureInfo
                result = d.ToString(this._storageCultureInfo);
            }

            return result;
        }

        #endregion

        #region Protected Virtual Methods

        protected virtual void InitialiseSearchPropertyIndexes()
        {
            _searchPropertyEnumIndexes = new ArrayList();
        }

        /// <summary>
        /// Gets the null value for the property with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected virtual string GetNullValue(string key)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            return string.Empty;
        }

        #endregion

        #region Public Virtual Methods

        public virtual DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            return new DataJSONWrapper();

        }

        public virtual void CopyFromWrapper(DataJSONWrapper dataWrapper, CultureInfo fromCultureInfo)
        {

        }

        #endregion

        #region Protected Abstract Methods

        /// <summary>
        /// Initialises the data node.
        /// </summary>
        protected abstract void InitialiseDataNode();

        /// <summary>
        /// Initialises the data item.
        /// </summary>
        protected abstract void InitialiseDataItem();

        /// <summary>
        /// Initialises the property indexes.
        /// </summary>
        protected abstract void InitialisePropertyIndexes();

        /// <summary>
        /// Initialises the property keys.
        /// </summary>
        protected abstract void InitialisePropertyKeys();

        #endregion

        #region Public Abstract Methods

        public abstract string DataType { get; }

        public abstract void Copy(IDataItem copy);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the property key for the specified property.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <returns></returns>
        public string ToKey(int propertyEnum)
        {
            // Go through each key
            foreach (string value in _keys.Keys)
            {
                if (_keys[value] == propertyEnum) return value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the property enum value for the specified property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public int ToEnum(string key)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            // Get the enum value for the specified key
            foreach (string value in _keys.Keys)
            {
                if (value == key) return _keys[value];
            }

            return 0;
        }

        #endregion

        #region IDataItem Members

        public event DataItemModifiedEventHandler Modified;

        public event DataItemStatusChangedEventHandler StatusChanged;

        public event DataItemPrimaryKeyModifiedEventHandler PrimaryKeyModified;

        protected const string _idKey = "ID";

        [System.ComponentModel.ReadOnly(true)] 
        [System.ComponentModel.DisplayName("ID")]
        [System.ComponentModel.Category("Item")]
        [System.ComponentModel.Description("The ID of the item.")]
        public int ID
        {
            get
            {
                return Int32.Parse(_node.Attributes[_idKey].Value);
            }
            set
            {
                int previousID = Int32.Parse(_node.Attributes[_idKey].Value);

                this.SetProperty(_idKey, value.ToString(), true);

                // Raise the primary key modified event
                this.OnPrimaryKeyModified(this, previousID);
            }
        }

        protected const string _rownumberKey = "RowNumber";

        [System.ComponentModel.ReadOnly(true)]
        [System.ComponentModel.DisplayName("RowNumber")]
        [System.ComponentModel.Category("Item")]
        [System.ComponentModel.Description("The RowNumber of the item.")]
        public int RowNumber
        {
            get
            {
                return Int32.Parse(_node.Attributes[_rownumberKey].Value);
            }
            set
            {
                this.SetProperty(_rownumberKey, value.ToString(), true);
            }
        }

        protected const string _statusKey = "Status";

        [System.ComponentModel.Browsable(false)]
        public DataItemStatusTypes Status
        {
            get
            {
                return (DataItemStatusTypes) Enum.Parse(typeof(DataItemStatusTypes), _node.Attributes[_statusKey].Value);
            }
            set
            {
                int i = (int) value;
                this.SetAttribute(_statusKey, i.ToString());
                this.OnStatusChanged(this, value.ToString());
            }
        }

        protected int _historyVersion = 1;

        [System.ComponentModel.Browsable(false)]
        public int HistoryVersion
        {
            get { return _historyVersion; }
        }

        protected ArrayList _history;

        [System.ComponentModel.Browsable(false)]
        public ArrayList History
        {
            get { return _history; }
        }

        [System.ComponentModel.Browsable(false)]
        public XmlNode Node
        {
            get { return _node; }
        }

        protected bool _doSaves = true;

        [System.ComponentModel.Browsable(false)]
        public bool DoSaves
        {
            get
            {
                return _doSaves;
            }
            set
            {
                _doSaves = value;
            }
        }

        protected bool _doValidations = true;

        [System.ComponentModel.Browsable(false)]
        public bool DoValidations
        {
            get
            {
                return _doValidations;
            }
            set
            {
                _doValidations = value;
            }
        }

        protected object _tag;

        [System.ComponentModel.Browsable(false)]
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        public void Validate()
        {
            // Go through each enum item
            for (int i = _startEnumIndex; i <= _endEnumIndex; i++)
            {
                // Get the value of the property
                string value = this.GetProperty(i);

                // Perform the validation by setting the property
                if (value != null) this.SetProperty(i, value, false);
            }
        }

        // When the data item node is created it is not initially added to the Xml document
        // because the add operation may be cancelled
        public void EndCreate()
        {
            if (_isDataCreated) return;

            // Add the node to the Xml document
            _node.OwnerDocument.DocumentElement.AppendChild(_node);
            _isDataCreated = true;
        }

        public void Remove()
        {
            if (!_isDataCreated) return;

            this.Status = DataItemStatusTypes.Deleted;
        }

        protected IDataItemCollection _collection;

        public IDataItemCollection Parent
        {
            get { return _collection; }
        }

        public ArrayList GetPropertyEnums()
        {
            ArrayList values = new ArrayList();

            // Go through each enum item
            for (int i = _startEnumIndex; i <= _endEnumIndex; i++)
            {
                values.Add(i);
            }

            return values;
        }

        public ArrayList GetPropertyKeys()
        {
            ArrayList values = new ArrayList();

            // Go through each enum item
            for (int i = _startEnumIndex; i <= _endEnumIndex; i++)
            {
                values.Add(this.ToKey(i));
            }

            return values;
        }

        public string GetProperty(int propertyEnum)
        {
            // Get the key for the Xml attribute of the property
            string key = this.ToKey(propertyEnum);

            // Get the Xml attribute
            XmlAttribute attribute = _node.Attributes[key];
            string value = null;
            if (attribute != null) value = _node.Attributes[key].Value;

            return value;
        }

        // Note: This method may be overridden to implement culture specific formatting
        public virtual string GetProperty(int propertyEnum, CultureInfo toCultureInfo)
        {
            #region Check Parameters

            if (toCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "toCultureInfo is nothing"));

            #endregion

            return this.GetProperty(propertyEnum);
        }

        public string GetProperty(string key)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            // Get the Xml attribute
            XmlAttribute attribute = _node.Attributes[key];
            string value = null;
            if (attribute != null) value = _node.Attributes[key].Value;

            return value;
        }

        // Note: This method may be overridden to implement culture specific formatting
        public virtual string GetProperty(string key, CultureInfo toCultureInfo)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (toCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "toCultureInfo is nothing"));

            #endregion

            int i = this.ToEnum(key);

            return this.GetProperty(i, toCultureInfo);
        }

        public void SetProperty(int propertyEnum, string value, bool setWhenInvalid)
        {
            #region Check Parameters

            if (value == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            this.SetProperty(this.ToKey(propertyEnum), value, setWhenInvalid);
        }

        // Note: This method may be overridden to implement culture specific formatting
        public virtual void SetProperty(int propertyEnum, string value, bool setWhenInvalid, CultureInfo fromCultureInfo)
        {
            #region Check Parameters

            if (fromCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fromCultureInfo is nothing"));

            #endregion

            this.SetProperty(this.ToKey(propertyEnum), value, setWhenInvalid, fromCultureInfo);
        }

        public void SetProperty(string key, string value, bool setWhenInvalid)
        {
            #region Check Parameters

            if (key == string.Empty)            throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            // DEBUG:
            if (_node.Attributes[key] == null) return; //throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));

            #endregion

            bool modified = true;

            // Get the property's null value
            if (value == null || String.IsNullOrEmpty(value)) value = this.GetNullValue(key);

            // Get the enum value for the property
            int propertyEnum = this.ToEnum(key);

            if (_doValidations)
            {
                // Perform the validation
                ValidationResultTypes validationResult = this.IsValid(propertyEnum, value);

                switch (validationResult)
                {
                    case ValidationResultTypes.Passed:
                        // Set the value
                        _node.Attributes[key].Value = value;

                        // Actions following a successful validation
                        string message = String.Format(Resources.DataItemBase.Messages.SetProperty, new string[] { key, this.DataType, this.ID.ToString(), value });
                        this.AfterValidationPassed(propertyEnum, message, validationResult);

                        break;

                    case ValidationResultTypes.Warning:
                        // Set the value
                        _node.Attributes[key].Value = value;

                        // Actions following a warning
                        this.AfterValidationPassed(propertyEnum, _validationMessage, validationResult);

                        break;

                    case ValidationResultTypes.Failed:
                        // Set the value if required
                        if (setWhenInvalid) _node.Attributes[key].Value = value;
                        else modified = false;

                        // Actions following a failed validation
                        this.AfterValidationFailed(propertyEnum, _validationMessage, validationResult);

                        break;

                    default:
                        break;
                }
            }
            else
            {
                // Set the value
                _node.Attributes[key].Value = value;
            }

            // Raise the event
            if (modified) this.OnModified(this, propertyEnum, string.Format(Resources.DataItemBase.Messages.SetProperty, new string[] { key, this.DataType, this.ID.ToString(), value }));
        }

        // Note: This method may be overridden to implement culture specific formatting
        public virtual void SetProperty(string key, string value, bool setWhenInvalid, CultureInfo fromCultureInfo)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (fromCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fromCultureInfo is nothing"));

            #endregion

            if (_node.Attributes[key] == null) return;

            this.SetProperty(key, value, setWhenInvalid);
        }

        public bool ContainsPropertyValue(string value)
        {
            // Go through each property
            for (int i = _startEnumIndex; i <= _endEnumIndex; i++)
            {
                // Only query the property if it has been specified in the search property indexes list
                if (_searchPropertyEnumIndexes.Contains(i))
                {
                    // Get the property value
                    string v = this.GetProperty(i);

                    if (!string.IsNullOrEmpty(v))
                    {
                        // If the property contains the specified search value then return true
                        if (v.ToLower().Contains(value.ToLower())) return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
