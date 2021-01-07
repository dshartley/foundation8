using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using System;
using System.Globalization;
using System.Xml;

namespace Smart.Platform.Social.Data.RelativeInteractions
{
    public enum RelativeInteractionDataParameterKeys
    {
        ID,
        ApplicationID,
        FromRelativeMemberID,
        ToRelativeMemberID,
        InteractionType,
        DateActioned,
        InteractionStatus,
        Text
    }

    /// <summary>
    /// Encapsulates an RelativeInteraction data item.
    /// </summary>
    public class RelativeInteraction : DataItemBase
    {
        #region Constructors

        private RelativeInteraction() : base() { }

        public RelativeInteraction(  XmlDocument         dataDocument,
                                IDataItemCollection collection,
                                string              defaultCultureInfoName)
            : base(dataDocument, collection, defaultCultureInfoName)
        { }

        public RelativeInteraction(  XmlNode             dataNode,
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

        protected const string _interactiontypeKey = "InteractionType";

        /// <summary>
        /// Gets or sets the InteractionType.
        /// </summary>
        /// <value>The InteractionType.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeInteractionTypes InteractionType
        {
            get
            {
                return (RelativeInteractionTypes)Enum.Parse(typeof(RelativeInteractionTypes), _node.Attributes[_interactiontypeKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_interactiontypeKey, i.ToString(), false);
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

        protected const string _interactionstatusKey = "InteractionStatus";

        /// <summary>
        /// Gets or sets the InteractionStatus.
        /// </summary>
        /// <value>The InteractionStatus.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeInteractionStatus InteractionStatus
        {
            get
            {
                return (RelativeInteractionStatus)Enum.Parse(typeof(RelativeInteractionStatus), _node.Attributes[_interactionstatusKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_interactionstatusKey, i.ToString(), false);
            }
        }

        protected const string _textKey = "Text";

        /// <summary>
        /// Gets or sets the Text.
        /// </summary>
        /// <value>
        /// The Text.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string Text
        {
            get
            {
                return _node.Attributes[_textKey].Value;
            }
            set
            {
                this.SetProperty(_textKey, value, false);
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
            this.SetAttribute(_interactiontypeKey,      "1");
            this.SetAttribute(_dateactionedKey,         DateTime.Now.ToString());
            this.SetAttribute(_interactionstatusKey,    "1");
            this.SetAttribute(_textKey,                 "");
        }

        protected override void InitialiseDataItem()
        {
            // Setup foreign key dependency helpers:
        }

        protected override void InitialisePropertyIndexes()
        {
            // Define the range of the properties using the enum values:

            _startEnumIndex = (int)DataProperties.RelativeInteractions_ID;
            _endEnumIndex   = (int)DataProperties.RelativeInteractions_Text;
        }

        protected override void InitialisePropertyKeys()
        {
            // Populate the dictionary of property keys:

            _keys.Add(_idKey,                       (int)DataProperties.RelativeInteractions_ID);
            _keys.Add(_applicationidKey,            (int)DataProperties.RelativeInteractions_ApplicationID);
            _keys.Add(_fromrelativememberidKey,     (int)DataProperties.RelativeInteractions_FromRelativeMemberID);
            _keys.Add(_torelativememberidKey,       (int)DataProperties.RelativeInteractions_ToRelativeMemberID);
            _keys.Add(_interactiontypeKey,          (int)DataProperties.RelativeInteractions_InteractionType);
            _keys.Add(_dateactionedKey,             (int)DataProperties.RelativeInteractions_DateActioned);
            _keys.Add(_interactionstatusKey,        (int)DataProperties.RelativeInteractions_InteractionStatus);
            _keys.Add(_textKey,                     (int)DataProperties.RelativeInteractions_Text);
        }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeInteraction"; }
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

            this.ID                     = ((RelativeInteraction)copy).ID;
            this.ApplicationID          = ((RelativeInteraction)copy).ApplicationID;
            this.FromRelativeMemberID   = ((RelativeInteraction)copy).FromRelativeMemberID;
            this.ToRelativeMemberID     = ((RelativeInteraction)copy).ToRelativeMemberID;
            this.InteractionType        = ((RelativeInteraction)copy).InteractionType;
            this.DateActioned           = ((RelativeInteraction)copy).DateActioned;
            this.InteractionStatus      = ((RelativeInteraction)copy).InteractionStatus;
            this.Text                   = ((RelativeInteraction)copy).Text;

            this.DoValidations = doValidations;
        }

        public override DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.ID = this.ID.ToString();

            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.ApplicationID.ToString(), this.ApplicationID);
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.FromRelativeMemberID.ToString(), this.FromRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.ToRelativeMemberID.ToString(), this.ToRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.InteractionType.ToString(), ((int)this.InteractionType).ToString());
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.DateActioned.ToString(), this.DateActioned.ToString(this._outputCultureInfo));
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.InteractionStatus.ToString(), ((int)this.InteractionStatus).ToString());
            dataWrapper.SetParameterValue(RelativeInteractionDataParameterKeys.Text.ToString(), this.Text);

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
            this.ApplicationID          = dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.ApplicationID.ToString());
            this.FromRelativeMemberID   = dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.FromRelativeMemberID.ToString());
            this.ToRelativeMemberID     = dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.ToRelativeMemberID.ToString());
            this.InteractionType        = (RelativeInteractionTypes)Enum.Parse(typeof(RelativeInteractionTypes), dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.InteractionType.ToString()));
            this.DateActioned           = DateTime.Parse(dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.DateActioned.ToString()), fromCultureInfo);
            this.InteractionStatus      = (RelativeInteractionStatus)Enum.Parse(typeof(RelativeInteractionStatus), dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.InteractionStatus.ToString()));
            this.Text                   = dataWrapper.GetParameterValue(RelativeInteractionDataParameterKeys.Text.ToString());

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
                case (int)DataProperties.RelativeInteractions_DateActioned:
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
                case (int)DataProperties.RelativeInteractions_DateActioned:

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
