using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Data.DataAccessStrategies.SQLServer
{
    /// <summary>
    /// Provides methods for handling a collection of stored procedure parameters.
    /// </summary>
    public class SQLServerParametersCollection : IParametersCollection
    {
        private Dictionary<string, SqlParameter>    _parameterDictionary;
        private ArrayList                           _parameterValues;
        
        # region Constructors

        public SQLServerParametersCollection()
        {
            _parameterDictionary    = new Dictionary<string, SqlParameter>();
            _parameterValues        = new ArrayList();
        }

        # endregion

        #region IParametersCollection Methods

        /// <summary>
        /// Gets an array of the parameters in the collection.
        /// </summary>
        /// <remarks>The parameters are maintained in the order that they are added.</remarks>
        /// <returns>An array of parameters.</returns>
        public object[] ToArray()
        {
            return (object[])_parameterValues.ToArray(typeof(SqlParameter));
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public DbParameter Add(string parameterName, object parameterValue)
        {
            #region Check Parameters

            if (parameterName == string.Empty)  throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterName is nothing"));
            if (parameterValue == null)         throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterValue is nothing"));

            #endregion

            // Create the parameter
            SqlParameter parameter = new SqlParameter(  "@" + parameterName,
                                                        parameterValue);

            // Add it to the collections
            _parameterDictionary.Add(parameterName, parameter);
            _parameterValues.Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">The parameter value.</param>
        public DbParameter Add(string parameterName, object parameterValue, ParameterDirection direction)
        {
            #region Check Parameters

            if (parameterName == string.Empty)  throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterName is nothing"));
            if (parameterValue == null)         throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterValue is nothing"));

            #endregion

            // Create the parameter
            SqlParameter parameter = (SqlParameter) this.Add(parameterName, parameterValue);
            parameter.Direction = direction;

            return parameter;
        }

        /// <summary>
        /// Gets the specified parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public DbParameter Get(string parameterName)
        {
            #region Check Parameters

            if (parameterName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterName is nothing"));

            #endregion

            return _parameterDictionary[parameterName];
        }

        /// <summary>
        /// Removes the specified parameter name.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        public void Remove(string parameterName)
        {
            #region Check Parameters

            if (parameterName == string.Empty) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterName is nothing"));

            #endregion

            if (_parameterDictionary.ContainsKey(parameterName)) _parameterDictionary.Remove(parameterName);

            SqlParameter    p       = null;
            bool            found   = false;
            for (int i = 0; (i < _parameterValues.Count) & (!found) ; i++)
			{
			    p = (SqlParameter)_parameterValues[i];
                if (p.ParameterName == "@" + parameterName) found = true;
			}
            if (found && p != null) _parameterValues.Remove(p);
        }

        #endregion
    }
}
