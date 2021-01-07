using System;
using System.Collections.Generic;
using Smart.Platform.Diagnostics;
using System.Reflection;

namespace Smart.Platform.Diagnostics.Logging
{
    public enum LogTypes
    {
        Error           = 0,
        UserNew         = 10,
        UserLoggedIn    = 11,
        Search          = 20,
        ViewedDemo      = 30
    }

    /// <summary>
    /// Manages application logging functions.
    /// </summary>
    public class LoggingManager
    {
        private Dictionary<string, ILogWriterStrategy> _strategies;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingManager"/> class.
        /// </summary>
        public LoggingManager() 
        {
            this.Initialise();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialises this instance. The collection of log writer strategies is cleared.
        /// </summary>
        public void Initialise()
        {
            _strategies = new Dictionary<string, ILogWriterStrategy>();
        }

        /// <summary>
        /// Registers the specified log writer strategy.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="strategy">The log writer strategy.</param>
        public void Register(string key, ILogWriterStrategy strategy)
        {
            #region Check Parameters

            if (string.IsNullOrEmpty(key)) throw new Exception(ExceptionManager.MessageMethodFailed(typeof(LoggingManager).GetMethod(nameof(Register)), "key is nothing"));
            if (strategy == null) throw new Exception(ExceptionManager.MessageMethodFailed(typeof(ExceptionManager).GetMethod(nameof(Register)), "strategy is nothing"));
            if (_strategies.ContainsKey(key)) throw new Exception(ExceptionManager.MessageMethodFailed(typeof(ExceptionManager).GetMethod(nameof(Register)), "key already exists"));

            #endregion

            _strategies.Add(key, strategy);
        }

        /// <summary>
        /// Writes a log entry using all registered log writer strategies.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="logText">The log text.</param>
        /// <param name="tags">The tags.</param>
        public void Write(LogTypes logType, string logText, Dictionary<string, string> tags)
        {
            // Go through each log writer strategy
            foreach (string key in _strategies.Keys)
            {
                ILogWriterStrategy lws = _strategies[key];

                // Write the log entry
                lws.Write((int)logType, DateTime.Now, logText, tags);
            }
        }

        #endregion
    }
}
