using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using System;
using System.Xml;
using System.Globalization;
using Smart.Platform.Net.Serialization.JSON;

namespace Smart.Platform.Social.Data.RelativeMembers
{
    public enum RelativeMemberDataParameterKeys
    {
        ID,
        ApplicationID,
        RowNumber,
        UserProfileID,
        Email,
        FullName,
        AvatarImageFileName,
        ConnectionContractTypes
    }

    /// <summary>
    /// Encapsulates an RelativeMember data item.
    /// </summary>
    public class RelativeMember : DataItemBase
    {
        #region Constructors

        private RelativeMember() : base() { }

        public RelativeMember(  XmlDocument         dataDocument,
                                IDataItemCollection collection,
                                string              defaultCultureInfoName)
            : base(dataDocument, collection, defaultCultureInfoName)
        { }

        public RelativeMember(  XmlNode             dataNode,
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

        protected const string _userprofileidKey = "UserProfileID";

        /// <summary>
        /// Gets or sets the UserProfileID.
        /// </summary>
        /// <value>
        /// The UserProfileID.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string UserProfileID
        {
            get
            {
                return _node.Attributes[_userprofileidKey].Value;
            }
            set
            {
                this.SetProperty(_userprofileidKey, value, false);
            }
        }

        protected const string _emailKey = "Email";

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// The Email.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string Email
        {
            get
            {
                return _node.Attributes[_emailKey].Value;
            }
            set
            {
                this.SetProperty(_emailKey, value, false);
            }
        }

        protected const string _fullnameKey = "FullName";

        /// <summary>
        /// Gets or sets the FullName.
        /// </summary>
        /// <value>
        /// The FullName.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string FullName
        {
            get
            {
                return _node.Attributes[_fullnameKey].Value;
            }
            set
            {
                this.SetProperty(_fullnameKey, value, false);
            }
        }

        protected const string _avatarimagefilenameKey = "AvatarImageFileName";

        /// <summary>
        /// Gets or sets the AvatarImageFileName.
        /// </summary>
        /// <value>
        /// The AvatarImageFileName.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string AvatarImageFileName
        {
            get
            {
                return _node.Attributes[_avatarimagefilenameKey].Value;
            }
            set
            {
                this.SetProperty(_avatarimagefilenameKey, value, false);
            }
        }

        protected const string _connectioncontracttypesKey = "ConnectionContractTypes";

        /// <summary>
        /// Gets or sets the ConnectionContractTypes.
        /// </summary>
        /// <value>
        /// The ConnectionContractTypes.
        /// </value>
        [System.ComponentModel.Browsable(false)]
        public string ConnectionContractTypes
        {
            get
            {
                return _node.Attributes[_connectioncontracttypesKey].Value;
            }
            set
            {
                this.SetProperty(_connectioncontracttypesKey, value, false);
            }
        }

        #endregion

        #region Protected Override Methods

        protected override void InitialiseDataNode()
        {
            // Setup the node data:

            this.SetAttribute(_idKey,                       "0");
            this.SetAttribute(_applicationidKey,            "");
            this.SetAttribute(_rownumberKey,                "0");
            this.SetAttribute(_userprofileidKey,            "");
            this.SetAttribute(_emailKey,                    "");
            this.SetAttribute(_fullnameKey,                 "");
            this.SetAttribute(_avatarimagefilenameKey,      "");
            this.SetAttribute(_connectioncontracttypesKey,  "");
        }

        protected override void InitialiseDataItem()
        {
            // Setup foreign key dependency helpers:
        }

        protected override void InitialisePropertyIndexes()
        {
            // Define the range of the properties using the enum values:

            _startEnumIndex = (int)DataProperties.RelativeMembers_ID;
            _endEnumIndex   = (int)DataProperties.RelativeMembers_ConnectionContractTypes;
        }

        protected override void InitialisePropertyKeys()
        {
            // Populate the dictionary of property keys:

            _keys.Add(_idKey,                       (int)DataProperties.RelativeMembers_ID);
            _keys.Add(_applicationidKey,            (int)DataProperties.RelativeMembers_ApplicationID);
            _keys.Add(_userprofileidKey,            (int)DataProperties.RelativeMembers_UserProfileID);
            _keys.Add(_emailKey,                    (int)DataProperties.RelativeMembers_Email);
            _keys.Add(_fullnameKey,                 (int)DataProperties.RelativeMembers_FullName);
            _keys.Add(_avatarimagefilenameKey,      (int)DataProperties.RelativeMembers_AvatarImageFileName);
            _keys.Add(_connectioncontracttypesKey,  (int)DataProperties.RelativeMembers_ConnectionContractTypes);
        }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeMember"; }
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

            this.ID                         = ((RelativeMember)copy).ID;
            this.ApplicationID              = ((RelativeMember)copy).ApplicationID;
            this.RowNumber                  = ((RelativeMember)copy).RowNumber;
            this.UserProfileID              = ((RelativeMember)copy).UserProfileID;
            this.Email                      = ((RelativeMember)copy).Email;
            this.FullName                   = ((RelativeMember)copy).FullName;
            this.AvatarImageFileName        = ((RelativeMember)copy).AvatarImageFileName;
            this.ConnectionContractTypes    = ((RelativeMember)copy).ConnectionContractTypes;

            this.DoValidations = doValidations;
        }

        public override DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper)
        {
            #region Check Parameters

            if (dataWrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dataWrapper is nothing"));

            #endregion

            dataWrapper.ID = this.ID.ToString();

            // Check RowNumber is set
            if (this.RowNumber > 0)
            {
                dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.RowNumber.ToString(), this.RowNumber.ToString());
            }

            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.ApplicationID.ToString(), this.ApplicationID);
            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.UserProfileID.ToString(), this.UserProfileID);
            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.Email.ToString(), this.Email);
            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.FullName.ToString(), this.FullName);
            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.AvatarImageFileName.ToString(), this.AvatarImageFileName);
            dataWrapper.SetParameterValue(RelativeMemberDataParameterKeys.ConnectionContractTypes.ToString(), this.ConnectionContractTypes);

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

            // Check RowNumber is set
            if (dataWrapper.HasParameterYN(RelativeMemberDataParameterKeys.RowNumber.ToString()))
            {
                this.RowNumber              = Int32.Parse(dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.RowNumber.ToString()));
            }

            this.ApplicationID              = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.ApplicationID.ToString());
            this.UserProfileID              = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.UserProfileID.ToString());
            this.Email                      = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.Email.ToString());
            this.FullName                   = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.FullName.ToString());
            this.AvatarImageFileName        = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.AvatarImageFileName.ToString());
            this.ConnectionContractTypes    = dataWrapper.GetParameterValue(RelativeMemberDataParameterKeys.ConnectionContractTypes.ToString());

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
