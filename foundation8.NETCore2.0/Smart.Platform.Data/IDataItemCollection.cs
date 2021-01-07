using System.Collections;
using System.Data;
using System.Globalization;
using System.Xml;
using Smart.Platform.Data.Validation;

namespace Smart.Platform.Data
{
    /// <summary>
    /// Defines a class which encapsulates a collection of data items.
    /// </summary>
    public interface IDataItemCollection
    {
        #region Events

        /// <summary>
        /// Occurs when a validation passes.
        /// </summary>
        event ValidationEventHandler ValidationPassed;

        /// <summary>
        /// Occurs when a validation fails.
        /// </summary>
        event ValidationEventHandler ValidationFailed;

        /// <summary>
        /// Occurs when a data item is modified.
        /// </summary>
        event DataItemModifiedEventHandler DataItemModified;

        /// <summary>
        /// Occurs when a data item status is changed.
        /// </summary>
        event DataItemStatusChangedEventHandler DataItemStatusChanged;

        /// <summary>
        /// Occurs when a data item's primary key is modified.
        /// </summary>
        event DataItemPrimaryKeyModifiedEventHandler DataItemPrimaryKeyModified;

        #endregion

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        string DataType { get; }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the next ID.
        /// </summary>
        /// <returns></returns>
        int GetNextID();

        /// <summary>
        /// Gets a new item.
        /// </summary>
        /// <returns></returns>
        IDataItem GetNewItem();

        /// <summary>
        /// Gets a new item.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        IDataItem GetNewItem(XmlNode node);

        /// <summary>
        /// Gets a new item.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="fromCultureInfo">The fromCultureInfo.</param>
        /// <returns></returns>
        IDataItem GetNewItem(DataRow row, CultureInfo fromCultureInfo);

        /// <summary>
        /// Adds a new item.
        /// </summary>
        /// <returns></returns>
        IDataItem AddItem();

        /// <summary>
        /// Adds a specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        IDataItem AddItem(IDataItem item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void RemoveItem(IDataItem item);

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <returns></returns>
        IDataItem GetItem(int ID);

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        IDataItem GetItem(int propertyEnum, string value);

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        ArrayList GetItems(int propertyEnum, string value);

        /// <summary>
        /// Sorts the by.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns></returns>
        ArrayList SortBy(int propertyEnum, bool ascending);

        /// <summary>
        /// Sorts the by.
        /// </summary>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns></returns>
        ArrayList SortBy(DataItemCollectionSortType sortType);

        /// <summary>
        /// Gets the data document.
        /// </summary>
        /// <value>The data document.</value>
        XmlDocument DataDocument { get; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        ArrayList Items { get; }

        /// <summary>
        /// Gets the data administrator.
        /// </summary>
        /// <value>The data administrator.</value>
        IDataAdministrator DataAdministrator { get; }

        /// <summary>
        /// Sets a setting
        /// </summary>
        /// <param name="key">The setting key</param>
        /// <param name="value">The setting value</param>
        void SetSetting(string key, object value);

        /// <summary>
        /// Gets a settings
        /// </summary>
        /// <param name="key">The setting key</param>
        /// <returns></returns>
        object GetSetting(string key);
    }
}
