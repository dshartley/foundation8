namespace Smart.Platform.Data
{
    /// <summary>
    /// Defines a class which provides a policy for managing the data.
    /// </summary>
    public interface IDataManagementPolicy
    {
        /// <summary>
        /// Initialises the data management policy.
        /// </summary>
        /// <param name="dataAdministrator">The data administrator.</param>
        void Initialise(IDataAdministrator dataAdministrator);

        /// <summary>
        /// Gets the data administrator.
        /// </summary>
        /// <value>The data administrator.</value>
        IDataAdministrator DataAdministrator { get; }
    }
}
