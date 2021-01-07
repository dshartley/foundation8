using System;
using System.Runtime.Serialization;

namespace Smart.Platform.Net.Serialization.JSON
{
    /// <summary>
    /// Encapsulates a key/value parameter.
    /// </summary>
    [DataContract]
    public class ParameterJSONWrapper
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterJSONWrapper"/> class.
        /// </summary>
        public ParameterJSONWrapper() 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterJSONWrapper" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public ParameterJSONWrapper(string key, string value)
        {
            _key    = key;
            _value  = value;
        }

        #endregion

        #region Public Methods

        private string _key = string.Empty;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [DataMember]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _value = string.Empty;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }
}
