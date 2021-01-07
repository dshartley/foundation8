using System.Data;
using System.Data.Common;

namespace Smart.Platform.Data.DataAccessStrategies
{
    /// <summary>
    /// Defines a class which provides a collection of parameters.
    /// </summary>
    public interface IParametersCollection
    {
        /// <summary>
        /// Gets an array of the parameters in the collection.
        /// </summary>
        /// <remarks>The parameters are maintained in the order that they are added.</remarks>
        /// <returns>An array of parameters.</returns>
        object[] ToArray();

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        DbParameter Add(string parameterName, object parameterValue);

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        DbParameter Add(string parameterName, object parameterValue, ParameterDirection direction);

        /// <summary>
        /// Gets the specified parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        DbParameter Get(string parameterName);

        /// <summary>
        /// Removes the specified parameter name.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        void Remove(string parameterName);
    }
}
