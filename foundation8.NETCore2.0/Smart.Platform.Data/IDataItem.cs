using System;
using System.Collections;
using System.Xml;
using System.Globalization;
using Smart.Platform.Data.Validation;
using Smart.Platform.Net.Serialization.JSON;

namespace Smart.Platform.Data
{
    #region Enums

    /// <summary>
    /// Indicates the status of a data item.
    /// </summary>
    public enum DataItemStatusTypes
    {
        /// <summary>
        /// The data item is new and has not been committed.
        /// </summary>
        New = 0,
        /// <summary>
        /// The data item has not been modified since it was committed.
        /// </summary>
        Unmodified = 1,
        /// <summary>
        /// The data item has been modified since it was committed.
        /// </summary>
        Modified = 2,
        /// <summary>
        /// The data item has been deleted since it was committed.
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// The data item has been deleted and this has been committed, so the data item is obsolete.
        /// </summary>
        Obsolete = 4
    }

    #endregion

    #region Delegates

    /// <summary>
    /// A delegate type for handling generic data item events
    /// </summary>
    public delegate void DataItemEventHandler(IDataItem item);

    /// <summary>
    /// A delegate type for handling data item primary key modified events
    /// </summary>
    public delegate void DataItemPrimaryKeyModifiedEventHandler(IDataItem item, int previousID);

    /// <summary>
    /// A delegate type for handling data item modified events
    /// </summary>
    public delegate void DataItemModifiedEventHandler(IDataItem item, int propertyEnum, string message);

    /// <summary>
    /// A delegate type for handling data item status changed events
    /// </summary>
    public delegate void DataItemStatusChangedEventHandler(IDataItem item, string message);

    #endregion

    /// <summary>
    /// Defines a class which encapsulates a data item.
    /// </summary>
    public interface IDataItem : IHasValidations
    {
        #region Events

        /// <summary>
        /// Occurs when the data item is modified.
        /// </summary>
        event DataItemModifiedEventHandler Modified;

        /// <summary>
        /// Occurs when the status of the data item is changed.
        /// </summary>
        event DataItemStatusChangedEventHandler StatusChanged;

        /// <summary>
        /// Occurs when the primary key field is modified.
        /// </summary>
        event DataItemPrimaryKeyModifiedEventHandler PrimaryKeyModified;

        #endregion

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        int ID { get; set; }

        /// <summary>
        /// Gets or sets the RowNumber.
        /// </summary>
        /// <value>The RowNumber.</value>
        int RowNumber { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        DataItemStatusTypes Status { get; set; }

        /// <summary>
        /// Gets the history version of the data item.
        /// </summary>
        /// <value>The history version.</value>
        int HistoryVersion { get; }

        /// <summary>
        /// Gets the history collection of the data item.
        /// </summary>
        /// <value>The history.</value>
        ArrayList History { get; }
        
        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>The node.</value>
        XmlNode Node { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        string DataType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to do saves.
        /// </summary>
        /// <value><c>true</c> if doing saves; otherwise, <c>false</c>.</value>
        Boolean DoSaves { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to do validations.
        /// </summary>
        /// <value><c>true</c> if doing validations; otherwise, <c>false</c>.</value>
        Boolean DoValidations { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        Object Tag { get; set; }
        
        /// <summary>
        /// Validates all the properties of the data item.
        /// </summary>
        void Validate();

        /// <summary>
        /// Ends the creation of the data item, until which it may be undone.
        /// </summary>
        void EndCreate();
        
        /// <summary>
        /// Removes this data item from the parent collection.
        /// </summary>
        void Remove();

        /// <summary>
        /// Copies the properties of the specified data item.
        /// </summary>
        /// <param name="copy">The copy.</param>
        void Copy(IDataItem copy);

        /// <summary>
        /// Copies the item to a wrapper.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        DataJSONWrapper CopyToWrapper(DataJSONWrapper dataWrapper);

        /// <summary>
        /// Copies the item from a wrapper.
        /// </summary>
        /// <param name="dataWrapper">The data wrapper.</param>
        /// <param name="fromCultureInfo">The fromCultureInfo.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        void CopyFromWrapper(DataJSONWrapper dataWrapper, CultureInfo fromCultureInfo);

        /// <summary>
        /// Gets the parent data collection.
        /// </summary>
        /// <value>The parent.</value>
        IDataItemCollection Parent { get; }

        /// <summary>
        /// Gets a list of the property enum values.
        /// </summary>
        /// <returns></returns>
        ArrayList GetPropertyEnums();

        /// <summary>
        /// Gets a list of the property keys.
        /// </summary>
        /// <returns></returns>
        ArrayList GetPropertyKeys();

        /// <summary>
        /// Gets the property of the specified enum value.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <returns></returns>
        string GetProperty(int propertyEnum);

        /// <summary>
        /// Gets the property of the specified enum value, formatted for the specified culture info.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="toCultureInfo">The toCultureInfo.</param>
        /// <returns></returns>
        string GetProperty(int propertyEnum, CultureInfo toCultureInfo);

        /// <summary>
        /// Gets the property with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetProperty(string key);

        /// <summary>
        /// Gets the property with the specified key, formatted for the specified culture info.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="toCultureInfo">The toCultureInfo.</param>
        /// <returns></returns>
        string GetProperty(string key, CultureInfo toCultureInfo);

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="value">The value.</param>
        /// <param name="setWhenInvalid">if set to <c>true</c> set when invalid.</param>
        void SetProperty(int propertyEnum, string value, Boolean setWhenInvalid);

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="propertyEnum">The property enum.</param>
        /// <param name="value">The value.</param>
        /// <param name="setWhenInvalid">if set to <c>true</c> set when invalid.</param>
        /// <param name="fromCultureInfo">The fromCultureInfo.</param>
        void SetProperty(int propertyEnum, string value, Boolean setWhenInvalid, CultureInfo fromCultureInfo);

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="setWhenInvalid">if set to <c>true</c> set when invalid.</param>
        void SetProperty(string key, string value, Boolean setWhenInvalid);

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="setWhenInvalid">if set to <c>true</c> set when invalid.</param>
        /// <param name="fromCultureInfo">The fromCultureInfo.</param>
        void SetProperty(string key, string value, Boolean setWhenInvalid, CultureInfo fromCultureInfo);

        /// <summary>
        /// Determines whether the item contains a property with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the item contains a property with the specified value; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsPropertyValue(string value);
    }
}
