using System;
using System.Collections.Generic;

namespace Smart.Platform.Diagnostics.Logging
{
    /// <summary>
    /// Defines a class which provides a strategy for writing to a log.
    /// </summary>
    public interface ILogWriterStrategy
    {
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="logTimestamp">The log timestamp.</param>
        /// <param name="logText">The log text.</param>
        /// <param name="tags">The tags.</param>
        void Write(int logType, DateTime logTimestamp, string logText, Dictionary<string, string> tags);
    }
}
