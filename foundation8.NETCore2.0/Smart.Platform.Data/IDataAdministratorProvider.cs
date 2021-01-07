namespace Smart.Platform.Data
{
    /// <summary>
    /// Defines a class which provides access to data administrators.
    /// </summary>
    public interface IDataAdministratorProvider
    {
        /// <summary>
        /// Gets the data administrator.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        IDataAdministrator GetDataAdministrator(string key);
    }
}
