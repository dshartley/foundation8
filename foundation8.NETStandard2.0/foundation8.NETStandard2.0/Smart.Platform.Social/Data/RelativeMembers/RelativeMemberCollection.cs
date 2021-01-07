using Smart.Platform.Data;
using Smart.Platform.Data.Validation;
using Smart.Platform.Diagnostics;
using System;
using System.Data;
using System.Xml;
using System.Globalization;

namespace Smart.Platform.Social.Data.RelativeMembers
{
    /// <summary>
    /// Encapsulates a collection of RelativeMember items.
    /// </summary>
    public class RelativeMemberCollection : DataItemCollectionBase
    {
        #region Constructors

        private RelativeMemberCollection() : base() { }

        public RelativeMemberCollection(IDataAdministrator  dataAdministrator,
                                        string              defaultCultureInfoName)
            : base(dataAdministrator, defaultCultureInfoName)
        { }

        public RelativeMemberCollection(XmlDocument         dataDocument,
                                        IDataAdministrator  dataAdministrator,
                                        string              defaultCultureInfoName)
            : base(dataDocument, dataAdministrator, defaultCultureInfoName)
        { }

        #endregion

        #region Public Override Methods

        public override string DataType
        {
            get { return "RelativeMember"; }
        }

        public override IDataItem GetNewItem()
        {
            // Create the new item
            RelativeMember item = new RelativeMember(_dataDocument, this, _defaultCultureInfoName);

            // Set the ID
            item.ID = this.GetNextID();

            return item;
        }

        public override IDataItem GetNewItem(XmlNode node)
        {
            #region Check Parameters

            if (node == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "node is nothing"));

            #endregion

            // Create the item
            RelativeMember item = new RelativeMember(node, this, _defaultCultureInfoName);

            return item;
        }
        
        public override IDataItem GetNewItem(DataRow row, CultureInfo fromCultureInfo)
        {
            #region Check Parameters

            if (row == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "row is nothing"));
            if (fromCultureInfo == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "fromCultureInfo is nothing"));

            #endregion

            // Create the new item
            RelativeMember item = (RelativeMember)this.GetNewItem();

            // Go through each column
            foreach (DataColumn column in row.Table.Columns)
            {
                // Set the property in the item
                item.SetProperty(column.ColumnName, row[column.ColumnName].ToString(), false, fromCultureInfo);
            }

            return item;
        }

        #endregion

        #region Protected Override Methods

        protected override void HandleValidationPassed(object item, int propertyEnum, string message, ValidationResultTypes validationResult)
        {
            this.OnValidationPassed((IHasValidations)item, propertyEnum, message, validationResult);
        }

        protected override void HandleValidationFailed(object item, int propertyEnum, string message, ValidationResultTypes validationResult)
        {
            this.OnValidationFailed((IHasValidations)item, propertyEnum, message, validationResult);
        }

        #endregion
    }
}
