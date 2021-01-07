using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using System;
using System.Globalization;
using System.Xml;

namespace Smart.Platform.Social.Data.RelativeConnectionRequests
{
    public enum RelativeConnectionRequestDataParameterKeys
    {
        ID,
        ApplicationID,
        FromRelativeMemberID,
        ToRelativeMemberID,
        RequestType,
        DateActioned,
        RequestStatus
    }

    /// <summary>
    /// Encapsulates an RelativeConnectionRequest data item.
    /// </summary>
    public class RelativeConnectionRequest : DataItemBase
    {
        #region Constructors

        private RelativeConnectionRequest() : base() { }

        public RelativeConnectionRequest(  XmlDocument         dataDocument,
                                IDataItemCollection collection,
                                string              defaultCultureInfoName)
            : base(dataDocument, collection, defaultCultureInfoName)
        { }

        public RelativeConnectionRequest(  XmlNode             dataNode,
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

        protected const string _requesttypeKey = "RequestType";

        /// <summary>
        /// Gets or sets the RequestType.
        /// </summary>
        /// <value>The RequestType.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeConnectionRequestTypes RequestType
        {
            get
            {
                return (RelativeConnectionRequestTypes)Enum.Parse(typeof(RelativeConnectionRequestTypes), _node.Attributes[_requesttypeKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_requesttypeKey, i.ToString(), false);
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

        protected const string _requeststatusKey = "RequestStatus";

        /// <summary>
        /// Gets or sets the RequestStatus.
        /// </summary>
        /// <value>The RequestStatus.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeConnectionRequestStatus RequestStatus
        {
            get
            {
                return (RelativeConnectionRequestStatus)Enum.Parse(typeof(RelativeConnectionRequestStatus), _node.Attributes[_requeststatusKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_requeststatusKey, i.ToString(), false);
            }
        }

        #endregion

        #region Protected Override Methods

        protected override void InitialiseDataNode()
        {
            // Setup the node data:

            this.SetAttribute(_idKey,                   "0");
            this.SetAttribute(_applicationidKey,        "");
            this.SetAttribute(_fromrelativememberidKey, "0");
            this.SetAttribute(_torelativememberidKey,   "0");
            this.SetAttribute(_requesttypeKey,          "1");
            this.SetAttribute(_dateactionedKey,         DateTime.Now.ToString());
            this.SetAttribute(_requeststatusKey,        "1");
        }

        protected override void InitialiseDataItem()
        {
            // Setup foreign key dependency helpers:
        }

        protected override void InitialisePropertyIndexes()
        {
            // Define the range of the properties using the enum values:

            _startEnumIndex = (int)DataProperties.RelativeConnectionRequests_ID;
            _endEnumIndex   = (int)DataProperties.RelativeConnectionRequests_RequestStatus;
        }

        protected override void InitialisePropertyKeys()
        {
            // Populate the dictionary of property keys:

            _keys.Add(_idKey,                       (int)DataProperties.RelativeConnectionRequests_ID);
            _keys.Add(_applicationidKey,            (int)DataProperties.RelativeConnectionRequests_ApplicationID);
            _keys.Add(_fromrelativememberidKey,     (int)DataProperties.RelativeConnectionRequests_FromRelativeMemberID);
            _keys.Add(_torelativememberidKey,       (int)DataProperties.RelativeConnectionRequests_ToRelativeMemberID);
            _keys.Add(_requesttypeKey,              (int)DataProperties.RelativeConnectionRequests_RequestType);
            _keys.Add(_dateactionedKey,             (int)DataProperties.RelativeConnectionRequests_DateActioned);
            _keys.Add(_requeststatusKey,            (int)DataProperties.RelativeConnectionRequests_RequestStatus);
        }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeConnectionRequest"; }
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

            this.ID                     = ((RelativeConnectionRequest)copy).ID;
            this.ApplicationID          = ((RelativeConnectionRequest)copy).ApplicationID;
            this.FromRelativeMemberID   = ((RelativeConnectionRequest)copy).FromRelativeMemberID;
            this.ToRelativeMemberID     = ((RelativeConnectionRequest)copy).ToRelativeMemberID;
            this.RequestType            = ((RelativeConnectionRequest)copy).RequestType;
            this.DateActioned           = ((RelativeConnectionRequest)copy).DateActioned;
            this.RequestStatus          = ((RelativeConnectionRequest)copy).RequestStatus;

            this.DoValidations = doValidations;
        }

        public override DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.ID = this.ID.ToString();

            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.ApplicationID.ToString(), this.ApplicationID);
            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.FromRelativeMemberID.ToString(), this.FromRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.ToRelativeMemberID.ToString(), this.ToRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.RequestType.ToString(), ((int)this.RequestType).ToString());
            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.DateActioned.ToString(), this.DateActioned.ToString(this._outputCultureInfo));
            dataWrapper.SetParameterValue(RelativeConnectionRequestDataParameterKeys.RequestStatus.ToString(), ((int)this.RequestStatus).ToString());

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

            this.ID                     = Int32.Parse(dataWrapper.ID);
            this.ApplicationID          = dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.ApplicationID.ToString());
            this.FromRelativeMemberID   = dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.FromRelativeMemberID.ToString());
            this.ToRelativeMemberID     = dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.ToRelativeMemberID.ToString());
            this.RequestType            = (RelativeConnectionRequestTypes)Enum.Parse(typeof(RelativeConnectionRequestTypes), dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.RequestType.ToString()));
            this.DateActioned           = DateTime.Parse(dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.DateActioned.ToString()), fromCultureInfo);
            this.RequestStatus          = (RelativeConnectionRequestStatus)Enum.Parse(typeof(RelativeConnectionRequestStatus), dataWrapper.GetParameterValue(RelativeConnectionRequestDataParameterKeys.RequestStatus.ToString()));

            this.DoValidations          = doValidations;
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
                case (int)DataProperties.RelativeConnectionRequests_DateActioned:
                    s = this.DateActioned.ToString("g", toCultureInfo); // MM/dd/yyyy HH:mm
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
                case (int)DataProperties.RelativeConnectionRequests_DateActioned:

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
