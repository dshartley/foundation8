using Smart.Platform.Data.Validation;
using Smart.Platform.Net.Serialization.JSON;
using System.Data;

namespace Smart.Platform.Data
{
    #region Delegates

    /// <summary>
    /// A delegate type for handling data administrator events
    /// </summary>
    public delegate void DataAdministratorEventHandler();

    #endregion

    /// <summary>
    /// Defines a class which provides administrative operations for a collection of data items.
    /// </summary>
    public interface IDataAdministrator
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

        /// <summary>
        /// Occurs when the data is loaded.
        /// </summary>
        event DataAdministratorEventHandler DataLoaded;

        /// <summary>
        /// Occurs when the data is saved.
        /// </summary>
        event DataAdministratorEventHandler DataSaved;

        #endregion

        /// <summary>
        /// Initialises the data administrator
        /// </summary>
        void Initialise();

        /// <summary>
        /// Gets or sets the data access strategy.
        /// </summary>
        /// <value>The data access strategy.</value>
        IDataAccessStrategy DataAccessStrategy { get; set; }

        /// <summary>
        /// Gets or sets the data management policy.
        /// </summary>
        /// <value>The data management policy.</value>
        IDataManagementPolicy DataManagementPolicy { get; set; }

        /// <summary>
        /// Gets the collection of data items.
        /// </summary>
        /// <value>The items.</value>
        IDataItemCollection Items { get; }

        /// <summary>
        /// Gets a value indicating whether the data is loaded.
        /// </summary>
        /// <value><c>true</c> if data is loaded; otherwise, <c>false</c>.</value>
        bool DataIsLoaded { get; }

        /// <summary>
        /// Gets a value indicating whether data is saved.
        /// </summary>
        /// <value><c>true</c> if data is saved; otherwise, <c>false</c>.</value>
        bool DataIsSaved { get; }

        /// <summary>
        /// Loads the data.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads items from the specified data table.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        void Load(DataTable dataTable);

        /// <summary>
        /// Loads the data item with the specified ID.
        /// </summary>
        void Load(int ID);

        /// <summary>
        /// Loads a number of data items before the specified data item.
        /// </summary>
        void LoadItemsBefore(int beforeID, bool includeBeforeIDItem, int numberofItems);

        /// <summary>
        /// Loads a number of data items after the specified data item.
        /// </summary>
        void LoadItemsAfter(int afterID, bool includeAfterIDItem, int numberofItems);

        /// <summary>
        /// Loads the data items between the specified row numbers.
        /// </summary>
        void LoadItemsBetween(int fromRowNumber, int toRowNumber, int sortBy);

        /// <summary>
        /// Returns a count of all items in the dataset.
        /// Note: This is not a count of the items that are loaded.
        /// </summary>
        int CountAll();

        /// <summary>
        /// Saves the data.
        /// </summary>
        void Save();

        /// <summary>
        /// Gets a new data item.
        /// </summary>
        /// <returns></returns>
        IDataItem GetNewItem();

        /// <summary>
        /// Adds the specified item.
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
        /// Copies the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        IDataItem CopyItem(IDataItem item);

        /// <summary>
        /// Copy items to wrapper.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        DataJSONWrapper ToWrapper();

        /// <summary>
        /// Gets the data administrator provider.
        /// </summary>
        /// <value>The data administrator provider.</value>
        IDataAdministratorProvider DataAdministratorProvider { get; }

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
