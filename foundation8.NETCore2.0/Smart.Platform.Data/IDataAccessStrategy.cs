using System.Data;

namespace Smart.Platform.Data
{
    /// <summary>
    /// Defines a class which provides a strategy for accessing a data source.
    /// </summary>
    public interface IDataAccessStrategy
    {
        /// <summary>
        /// Inserts the specified item in the data source.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The new ID of the inserted item in the data source.</returns>
        int Insert(IDataItem item);

        /// <summary>
        /// Updates the specified item in the data source.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(IDataItem item);

        /// <summary>
        /// Commits all items in the collection to the data source.
        /// </summary>
        /// <param name="item">The item.</param>
        void Commit(IDataItemCollection collection);

        /// <summary>
        /// Deletes the specified item from the data source.
        /// </summary>
        /// <param name="item">The item.</param>
        void Delete(IDataItem item);

        /// <summary>
        /// Selects all items from the data source.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        IDataItemCollection Select(IDataItemCollection collection);

        /// <summary>
        /// Selects the item with the specified ID from the data source.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="ID">The ID.</param>
        /// <returns></returns>
        IDataItemCollection Select(IDataItemCollection collection, int ID);

        /// <summary>
        /// Selects a number of items before the specified ID from the data source.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="beforeID">The ID of the specified item.</param>
        /// <param name="includeBeforeIDItem">Indicates whether to include the specified item.</param>
        /// <param name="numberofItems">The number of items.</param>
        /// <returns></returns>
        IDataItemCollection SelectBefore(IDataItemCollection collection, int beforeID, bool includeBeforeIDItem, int numberofItems);

        /// <summary>
        /// Selects a number of items after the specified ID from the data source.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="afterID">The ID of the specified item.</param>
        /// <param name="includeAfterIDItem">Indicates whether to include the specified item.</param>
        /// <param name="numberofItems">The number of items.</param>
        /// <returns></returns>
        IDataItemCollection SelectAfter(IDataItemCollection collection, int afterID, bool includeAfterIDItem, int numberofItems);

        /// <summary>
        /// Selects a number of items after the specified ID from the data source.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="fromRowNumber">The row number of the first item.</param>
        /// <param name="toRowNumber">The row number of the last item.</param>
        /// <param name="sortBy">The sort by key.</param>
        /// <returns></returns>
        IDataItemCollection SelectBetween(IDataItemCollection collection, int fromRowNumber, int toRowNumber, int sortBy);

        /// <summary>
        /// Selects the count of all items in the data source.
        /// </summary>
        /// <returns></returns>
        int SelectCount();

        /// <summary>
        /// Clears the list of omitted parameter keys.
        /// </summary>
        void ClearOmittedParameterKeys();

        /// <summary>
        /// Sets the omitted parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        void SetOmittedParameter(string key);

        /// <summary>
        /// Determines whether [is parameter omitted] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if [is parameter omitted] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        bool IsParameterOmitted(string key);

        /// <summary>
        /// Fills the collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data);

        /// <summary>
        /// Fills the collection with the data from the specified data set.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="data">The data.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <returns></returns>
        IDataItemCollection FillCollection(IDataItemCollection collection, DataTable data, bool append);
    }
}
