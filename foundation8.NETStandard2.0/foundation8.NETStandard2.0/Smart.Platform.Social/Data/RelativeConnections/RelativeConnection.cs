using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using System;
using System.Globalization;
using System.Xml;

namespace Smart.Platform.Social.Data.RelativeConnections
{
    public enum RelativeConnectionDataParameterKeys
    {
        ID,
        ApplicationID,
        FromRelativeMemberID,
        ToRelativeMemberID,
        ConnectionContractType,
        DateActioned,
        DateLastActive,
        ConnectionStatus
    }

    /// <summary>
    /// Encapsulates an RelativeConnection data item.
    /// </summary>
    public class RelativeConnection : DataItemBase
    {
        #region Constructors

        private RelativeConnection() : base() { }

        public RelativeConnection(  XmlDocument         dataDocument,
                                    IDataItemCollection collection,
                                    string              defaultCultureInfoName)
            : base(dataDocument, collection, defaultCultureInfoName)
        { }

        public RelativeConnection(  XmlNode             dataNode,
                                    IDataItemCollection collection,
                                    string              defaultCultureInfoName)
            : base(dataNode, collection, defaultCultureInfoName)
        { }

        #endregion

        #region Public Methods

        protected const string _applicationidKey = "ApplicationID";

        /// <summary>
        /// Gets or sets the ApplicationID.
        /// </summary>
        /// <value>
        /// The ApplicationID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string ApplicationID
        {
            get
            {
                return _node.Attributes[_applicationidKey].Value;
            }
            set
            {
                this.SetProperty(_applicationidKey, value, false);
            }
        }

        protected const string _fromrelativememberidKey = "FromRelativeMemberID";

        /// <summary>
        /// Gets or sets the FromRelativeMemberID.
        /// </summary>
        /// <value>
        /// The FromRelativeMemberID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string FromRelativeMemberID
        {
            get
            {
                return _node.Attributes[_fromrelativememberidKey].Value;
            }
            set
            {
                this.SetProperty(_fromrelativememberidKey, value, false);
            }
        }

        protected const string _torelativememberidKey = "ToRelativeMemberID";

        /// <summary>
        /// Gets or sets the ToRelativeMemberID.
        /// </summary>
        /// <value>
        /// The ToRelativeMemberID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string ToRelativeMemberID
        {
            get
            {
                return _node.Attributes[_torelativememberidKey].Value;
            }
            set
            {
                this.SetProperty(_torelativememberidKey, value, false);
            }
        }

        protected const string _connectioncontracttypeKey = "ConnectionContractType";

        /// <summary>
        /// Gets or sets the ConnectionContractType.
        /// </summary>
        /// <value>The ConnectionContractType.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeConnectionContractTypes ConnectionContractType
        {
            get
            {
                return (RelativeConnectionContractTypes)Enum.Parse(typeof(RelativeConnectionContractTypes), _node.Attributes[_connectioncontracttypeKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_connectioncontracttypeKey, i.ToString(), false);
            }
        }

        protected const string _dateactionedKey = "DateActioned";

        /// <summary>
        /// Gets or sets the DateActioned.
        /// </summary>
        /// <value>
        /// The DateActioned.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public DateTime DateActioned
        {
            get
            {
                return DateTime.Parse(_node.Attributes[_dateactionedKey].Value, this._storageCultureInfo);
            }
            set
            {
                this.SetProperty(_dateactionedKey, value.ToString(this._storageCultureInfo), false);
            }
        }

        protected const string _datelastactiveKey = "DateLastActive";

        /// <summary>
        /// Gets or sets the DateLastActive.
        /// </summary>
        /// <value>
        /// The DateLastActive.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public DateTime DateLastActive
        {
            get
            {
                return DateTime.Parse(_node.Attributes[_datelastactiveKey].Value, this._storageCultureInfo);
            }
            set
            {
                this.SetProperty(_datelastactiveKey, value.ToString(this._storageCultureInfo), false);
            }
        }

        protected const string _connectionstatusKey = "ConnectionStatus";

        /// <summary>
        /// Gets or sets the ConnectionStatus.
        /// </summary>
        /// <value>The ConnectionStatus.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeConnectionStatus ConnectionStatus
        {
            get
            {
                return (RelativeConnectionStatus)Enum.Parse(typeof(RelativeConnectionStatus), _node.Attributes[_connectionstatusKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_connectionstatusKey, i.ToString(), false);
            }
        }

        #endregion

        #region Protected Override Methods

        protected override void InitialiseDataNode()
        {
            // Setup the node data:

            this.SetAttribute(_idKey,                       "0");
            this.SetAttribute(_applicationidKey,            "");
            this.SetAttribute(_fromrelativememberidKey,     "0");
            this.SetAttribute(_torelativememberidKey,       "0");
            this.SetAttribute(_connectioncontracttypeKey,   "1");
            this.SetAttribute(_dateactionedKey,             DateTime.Now.ToString());
            this.SetAttribute(_datelastactiveKey,           DateTime.Now.ToString());
            this.SetAttribute(_connectionstatusKey,         "1");
        }

        protected override void InitialiseDataItem()
        {
            // Setup foreign key dependency helpers:
        }

        protected override void InitialisePropertyIndexes()
        {
            // Define the range of the properties using the enum values:

            _startEnumIndex = (int)DataProperties.RelativeConnections_ID;
            _endEnumIndex   = (int)DataProperties.RelativeConnections_ConnectionStatus;
        }

        protected override void InitialisePropertyKeys()
        {
            // Populate the dictionary of property keys:

            _keys.Add(_idKey,                       (int)DataProperties.RelativeConnections_ID);
            _keys.Add(_applicationidKey,            (int)DataProperties.RelativeConnections_ApplicationID);
            _keys.Add(_fromrelativememberidKey,     (int)DataProperties.RelativeConnections_FromRelativeMemberID);
            _keys.Add(_torelativememberidKey,       (int)DataProperties.RelativeConnections_ToRelativeMemberID);
            _keys.Add(_connectioncontracttypeKey,   (int)DataProperties.RelativeConnections_ConnectionContractType);
            _keys.Add(_dateactionedKey,             (int)DataProperties.RelativeConnections_DateActioned);
            _keys.Add(_datelastactiveKey,           (int)DataProperties.RelativeConnections_DateLastActive);
            _keys.Add(_connectionstatusKey,         (int)DataProperties.RelativeConnections_ConnectionStatus);
        }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeConnection"; }
        }

        public override void Copy(IDataItem copy)
        {
            #region Check Parameters

            if (copy == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "copy is nothing"));

            #endregion

            // Validations should not be performed when copying the item
            bool doValidations = this.DoValidations;
            this.DoValidations = false;

            // Copy all properties from the copy data item to this data item:

            this.ID                         = ((RelativeConnection)copy).ID;
            this.ApplicationID              = ((RelativeConnection)copy).ApplicationID;
            this.FromRelativeMemberID       = ((RelativeConnection)copy).FromRelativeMemberID;
            this.ToRelativeMemberID         = ((RelativeConnection)copy).ToRelativeMemberID;
            this.ConnectionContractType     = ((RelativeConnection)copy).ConnectionContractType;
            this.DateActioned               = ((RelativeConnection)copy).DateActioned;
            this.DateLastActive             = ((RelativeConnection)copy).DateLastActive;
            this.ConnectionStatus           = ((RelativeConnection)copy).ConnectionStatus;

            this.DoValidations = doValidations;
        }

        public override DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.ID = this.ID.ToString();

            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.ApplicationID.ToString(), this.ApplicationID);
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.FromRelativeMemberID.ToString(), this.FromRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.ToRelativeMemberID.ToString(), this.ToRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.ConnectionContractType.ToString(), ((int)this.ConnectionContractType).ToString());
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.DateActioned.ToString(), this.DateActioned.ToString(this._outputCultureInfo));
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.DateLastActive.ToString(), this.DateLastActive.ToString(this._outputCultureInfo));
            dataWrapper.SetParameterValue(RelativeConnectionDataParameterKeys.ConnectionStatus.ToString(), ((int)this.ConnectionStatus).ToString());

            return dataWrapper;
        }

        public override void CopyFromWrapper(DataJSONWrapper dataWrapper, CultureInfo fromCultureInfo)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));
            if (fromCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fromCultureInfo is nothing"));

            #endregion

            // Validations should not be performed when copying the item
            bool doValidations = this.DoValidations;
            this.DoValidations = false;

            // Copy all properties from the copy data item to this data item:

            this.ID                         = Int32.Parse(dataWrapper.ID);
            this.ApplicationID              = dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.ApplicationID.ToString());
            this.FromRelativeMemberID       = dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.FromRelativeMemberID.ToString());
            this.ToRelativeMemberID         = dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.ToRelativeMemberID.ToString());
            this.ConnectionContractType     = (RelativeConnectionContractTypes)Enum.Parse(typeof(RelativeConnectionContractTypes), dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.ConnectionContractType.ToString()));
            this.DateActioned               = DateTime.Parse(dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.DateActioned.ToString()), fromCultureInfo);
            this.DateLastActive             = DateTime.Parse(dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.DateLastActive.ToString()), fromCultureInfo);
            this.ConnectionStatus           = (RelativeConnectionStatus)Enum.Parse(typeof(RelativeConnectionStatus), dataWrapper.GetParameterValue(RelativeConnectionDataParameterKeys.ConnectionStatus.ToString()));

            this.DoValidations = doValidations;
        }

        public override ValidationResultTypes IsValid(int propertyEnum, string value)
        {
            #region Check Parameters

            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            ValidationResultTypes result = ValidationResultTypes.Passed;

            // Perform validations for the specified property
            switch (ToProperty(propertyEnum))
            {
                default:
                    break;
            }

            return result;
        }

        // Note: This method is overridden to implement culture specific formatting
        public override string GetProperty(int propertyEnum, CultureInfo toCultureInfo)
        {
            #region Check Parameters

            if (toCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "toCultureInfo is nothing"));

            #endregion

            string s = string.Empty;

            switch (propertyEnum)
            {
                case (int)DataProperties.RelativeConnections_DateActioned:
                    s = this.DateActioned.ToString("g", toCultureInfo); // MM/dd/yyyy HH:mm
                    break;
                case (int)DataProperties.RelativeConnections_DateLastActive:
                    s = this.DateLastActive.ToString("g", toCultureInfo); // MM/dd/yyyy HH:mm
                    break;
                default:
                    s = this.GetProperty(propertyEnum);
                    break;
            }

            return s;
        }

        // Note: This method is overridden to implement culture specific formatting
        public override void SetProperty(string key, string value, bool setWhenInvalid, CultureInfo fromCultureInfo)
        {
            #region Check Parameters

            if (key == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "key is nothing"));
            if (fromCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fromCultureInfo is nothing"));

            #endregion

            string              s = string.Empty;
            int                 propertyEnum = this.ToEnum(key);

            switch (propertyEnum)
            {
                case (int)DataProperties.RelativeConnections_DateActioned:

                    // Parse the value using fromCultureInfo
                    s = this.DoParseToStorageDateString(value, fromCultureInfo);

                    break;
                case (int)DataProperties.RelativeConnections_DateLastActive:

                    // Parse the value using fromCultureInfo
                    s = this.DoParseToStorageDateString(value, fromCultureInfo);

                    break;
                default:
                    s = value;
                    break;
            }


            this.SetProperty(key, s, setWhenInvalid);

        }

        #endregion

        #region Private Methods

        private DataProperties ToProperty(int propertyEnum)
        {
            return (DataProperties)propertyEnum;
        }

        #endregion

        #region Validations

        #endregion
    }
}
