using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using Smart.Platform.Net.Serialization.JSON;
using System;
using System.Globalization;
using System.Xml;

namespace Smart.Platform.Social.Data.RelativeTimelineEvents
{
    public enum RelativeTimelineEventDataParameterKeys
    {
        ID,
        ApplicationID,
        ForRelativeMemberID,
        RelativeInteractionID,
        EventType,
        DateActioned,
        EventStatus
    }

    /// <summary>
    /// Encapsulates an RelativeTimelineEvent data item.
    /// </summary>
    public class RelativeTimelineEvent : DataItemBase
    {
        #region Constructors

        private RelativeTimelineEvent() : base() { }

        public RelativeTimelineEvent(  XmlDocument         dataDocument,
                                IDataItemCollection collection,
                                string              defaultCultureInfoName)
            : base(dataDocument, collection, defaultCultureInfoName)
        { }

        public RelativeTimelineEvent(  XmlNode             dataNode,
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

        protected const string _forrelativememberidKey = "ForRelativeMemberID";

        /// <summary>
        /// Gets or sets the ForRelativeMemberID.
        /// </summary>
        /// <value>
        /// The ForRelativeMemberID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string ForRelativeMemberID
        {
            get
            {
                return _node.Attributes[_forrelativememberidKey].Value;
            }
            set
            {
                this.SetProperty(_forrelativememberidKey, value, false);
            }
        }

        protected const string _relativeinteractionidKey = "RelativeInteractionID";

        /// <summary>
        /// Gets or sets the RelativeInteractionID.
        /// </summary>
        /// <value>
        /// The RelativeInteractionID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string RelativeInteractionID
        {
            get
            {
                return _node.Attributes[_relativeinteractionidKey].Value;
            }
            set
            {
                this.SetProperty(_relativeinteractionidKey, value, false);
            }
        }

        protected const string _eventtypeKey = "EventType";

        /// <summary>
        /// Gets or sets the EventType.
        /// </summary>
        /// <value>The EventType.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeTimelineEventTypes EventType
        {
            get
            {
                return (RelativeTimelineEventTypes)Enum.Parse(typeof(RelativeTimelineEventTypes), _node.Attributes[_eventtypeKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_eventtypeKey, i.ToString(), false);
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

        protected const string _eventstatusKey = "EventStatus";

        /// <summary>
        /// Gets or sets the EventStatus.
        /// </summary>
        /// <value>The EventStatus.</value>
        [System.ComponentModel.Browsable(false)]
        public RelativeTimelineEventStatus EventStatus
        {
            get
            {
                return (RelativeTimelineEventStatus)Enum.Parse(typeof(RelativeTimelineEventStatus), _node.Attributes[_eventstatusKey].Value);
            }
            set
            {
                int i = (int)value;
                this.SetProperty(_eventstatusKey, i.ToString(), false);
            }
        }

        #endregion

        #region Protected Override Methods

        protected override void InitialiseDataNode()
        {
            // Setup the node data:

            this.SetAttribute(_idKey,                       "0");
            this.SetAttribute(_applicationidKey,            "");
            this.SetAttribute(_forrelativememberidKey,      "0");
            this.SetAttribute(_relativeinteractionidKey,    "0");
            this.SetAttribute(_eventtypeKey,                "1");
            this.SetAttribute(_dateactionedKey,             DateTime.Now.ToString());
            this.SetAttribute(_eventstatusKey,              "1");
        }

        protected override void InitialiseDataItem()
        {
            // Setup foreign key dependency helpers:
        }

        protected override void InitialisePropertyIndexes()
        {
            // Define the range of the properties using the enum values:

            _startEnumIndex = (int)DataProperties.RelativeTimelineEvents_ID;
            _endEnumIndex   = (int)DataProperties.RelativeTimelineEvents_EventStatus;
        }

        protected override void InitialisePropertyKeys()
        {
            // Populate the dictionary of property keys:

            _keys.Add(_idKey,                       (int)DataProperties.RelativeTimelineEvents_ID);
            _keys.Add(_applicationidKey,            (int)DataProperties.RelativeTimelineEvents_ApplicationID);
            _keys.Add(_forrelativememberidKey,      (int)DataProperties.RelativeTimelineEvents_ForRelativeMemberID);
            _keys.Add(_relativeinteractionidKey,    (int)DataProperties.RelativeTimelineEvents_RelativeInteractionID);
            _keys.Add(_eventtypeKey,                (int)DataProperties.RelativeTimelineEvents_EventType);
            _keys.Add(_dateactionedKey,             (int)DataProperties.RelativeTimelineEvents_DateActioned);
            _keys.Add(_eventstatusKey,              (int)DataProperties.RelativeTimelineEvents_EventStatus);
        }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeTimelineEvent"; }
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

            this.ID                     = ((RelativeTimelineEvent)copy).ID;
            this.ApplicationID          = ((RelativeTimelineEvent)copy).ApplicationID;
            this.ForRelativeMemberID    = ((RelativeTimelineEvent)copy).ForRelativeMemberID;
            this.RelativeInteractionID  = ((RelativeTimelineEvent)copy).RelativeInteractionID;
            this.EventType              = ((RelativeTimelineEvent)copy).EventType;
            this.DateActioned           = ((RelativeTimelineEvent)copy).DateActioned;
            this.EventStatus            = ((RelativeTimelineEvent)copy).EventStatus;

            this.DoValidations = doValidations;
        }

        public override DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.ID = this.ID.ToString();

            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.ApplicationID.ToString(), this.ApplicationID);
            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.ForRelativeMemberID.ToString(), this.ForRelativeMemberID);
            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.RelativeInteractionID.ToString(), this.RelativeInteractionID);
            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.EventType.ToString(), ((int)this.EventType).ToString());
            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.DateActioned.ToString(), this.DateActioned.ToString(this._outputCultureInfo));
            dataWrapper.SetParameterValue(RelativeTimelineEventDataParameterKeys.EventStatus.ToString(), ((int)this.EventStatus).ToString());

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
            this.ApplicationID              = dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.ApplicationID.ToString());
            this.ForRelativeMemberID        = dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.ForRelativeMemberID.ToString());
            this.RelativeInteractionID      = dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.RelativeInteractionID.ToString());
            this.EventType                  = (RelativeTimelineEventTypes)Enum.Parse(typeof(RelativeTimelineEventTypes), dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.EventType.ToString()));
            this.DateActioned               = DateTime.Parse(dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.DateActioned.ToString()), fromCultureInfo);
            this.EventStatus                = (RelativeTimelineEventStatus)Enum.Parse(typeof(RelativeTimelineEventStatus), dataWrapper.GetParameterValue(RelativeTimelineEventDataParameterKeys.EventStatus.ToString()));

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
                case (int)DataProperties.RelativeTimelineEvents_DateActioned:
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
                case (int)DataProperties.RelativeTimelineEvents_DateActioned:

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
