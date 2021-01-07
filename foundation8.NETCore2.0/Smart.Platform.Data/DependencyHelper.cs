using System;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Data
{
    /// <summary>
    /// Helps to manage dependency items based on a foreign key
    /// </summary>
    public class DependencyHelper
    {
        private IDataAdministrator  _dependencyDataAdministrator;
        private IDataItem           _parentDataItem;
        private int                 _foreignKeyIDPropertyEnum;

        #region Constructors

        private DependencyHelper() { }

        public DependencyHelper(  IDataAdministrator  dependencyDataAdministrator,
                                   IDataItem           parentDataItem,
                                   int                 foreignKeyIDPropertyEnum)
        {
            #region Check Parameters

            if (dependencyDataAdministrator == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "dependencyDataAdministrator is nothing"));
            if (parentDataItem == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parentDataItem is nothing"));

            #endregion

            _dependencyDataAdministrator    = dependencyDataAdministrator;
            _parentDataItem                 = parentDataItem;
            _foreignKeyIDPropertyEnum       = foreignKeyIDPropertyEnum;
        }

        #endregion

        #region Public Methods

        public bool SetForeignKeyByPropertyValue(int propertyEnum, string value)
        {
            #region Check Parameters

            if (value == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "value is nothing"));

            #endregion

            // Get the foreign key item with the specified property value
            IDataItem item = _dependencyDataAdministrator.Items.GetItem(propertyEnum, value);

            // If a foreign key item was found, then set the foreign key id
            if (item != null)
            {
                // Set the foreign key id
                _parentDataItem.SetProperty(_foreignKeyIDPropertyEnum, item.ID.ToString(), true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetForeignKeyPropertyValue(int propertyEnum)
        {
            // Get the foreign key id
            int id = int.Parse(_parentDataItem.GetProperty(_foreignKeyIDPropertyEnum));

            // Get the item from the collection
            IDataItem item = _dependencyDataAdministrator.Items.GetItem(id);
            
            string value = "";
            if (item != null) value = item.GetProperty(propertyEnum);

            return value;
        }

        #endregion
    }
}
