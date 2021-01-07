using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Smart.Platform.Diagnostics;

namespace Smart.Platform.Net.Serialization.JSON
{
    /// <summary>
    /// Encapsulates a generic serializable JSON data item.
    /// </summary>
    [DataContract]
    public class DataJSONWrapper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataJSONWrapper"/> class.
        /// </summary>
        public DataJSONWrapper() 
        {
        }

        #endregion

        #region Public Methods

        private string _ID = string.Empty;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private List<DataJSONWrapper> _items = new List<DataJSONWrapper>();

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [DataMember]
        public List<DataJSONWrapper> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private List<ParameterJSONWrapper> _params = new List<ParameterJSONWrapper>();

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [DataMember]
        public List<ParameterJSONWrapper> Params
        {
            get { return _params; }
            set { _params = value; }
        }

        /// <summary>
        /// Determines whether [has paramater asynchronous] [the specified parameter key].
        /// </summary>
        /// <param name="parameterKey">The parameter key.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public Boolean HasParameterYN(string parameterKey)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(parameterKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterKey is nothing"));

            #endregion

            Boolean r = false;

            foreach (ParameterJSONWrapper pw in _params)
            {
                if (pw.Key.ToLower() == parameterKey.ToLower()) r = true;
            }

            return r;
        }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <param name="parameterKey">The parameter key.</param>
        /// <returns></returns>
        public string GetParameterValue(string parameterKey)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(parameterKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterKey is nothing"));

            #endregion

            string r = string.Empty;

            foreach (ParameterJSONWrapper pw in _params)
            {
                if (!string.IsNullOrEmpty(pw.Key) && pw.Key.ToLower() == parameterKey.ToLower()) r = pw.Value;                
            }

            return r;
        }

        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        /// <param name="parameterKey">The parameter key.</param>
        /// <param name="parameterValue">The parameter value.</param>
        /// <exception cref="System.ApplicationException">
        /// </exception>
        public void SetParameterValue(string parameterKey, string parameterValue)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(parameterKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterKey is nothing"));

            #endregion

            ParameterJSONWrapper r = null;

            foreach (ParameterJSONWrapper pw in _params)
            {
                if (!string.IsNullOrEmpty(pw.Key) && pw.Key.ToLower() == parameterKey.ToLower()) r = pw;
            }

            // If the parameter was not found then create it
            if (r == null)
            {
                r = new ParameterJSONWrapper(parameterKey, parameterValue);
                _params.Add(r);
            }
            else
            {
                r.Value = parameterValue;
            }
        }

        /// <summary>
        /// Deletes the parameter value.
        /// </summary>
        /// <param name="parameterKey">The parameter key.</param>
        /// <exception cref="System.ApplicationException"></exception>
        public void DeleteParameterValue(string parameterKey)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(parameterKey)) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "parameterKey is nothing"));

            #endregion

            ParameterJSONWrapper r = null;

            foreach (ParameterJSONWrapper pw in _params)
            {
                if (!string.IsNullOrEmpty(pw.Key) && pw.Key.ToLower() == parameterKey.ToLower()) r = pw;
            }

            // If the parameter was found then remove it
            if (r != null) _params.Remove(r);
        }

        #endregion
    }
}
