using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Platform.Diagnostics
{
    #region Enums

    /// <summary>
    /// Indicates the type of a console message.
    /// </summary>
    public enum ConsoleMessageTypes
    {
        /// <summary>
        /// An item of information.
        /// </summary>
        Information,
        /// <summary>
        /// An item of interest.
        /// </summary>
        Interest,
        /// <summary>
        /// A warning.
        /// </summary>
        Warning,
        /// <summary>
        /// A program error.
        /// </summary>
        ProgramError,
        /// <summary>
        /// A print operation.
        /// </summary>
        Print,
        /// <summary>
        /// A save operation.
        /// </summary>
        Save
    }

    #endregion

    /// <summary>
    /// Manages the application console implementations.
    /// </summary>
    public class ConsoleManager
    {
        #region Constructors

        #endregion

        #region Public Static Methods

        public static void Write(string message, ConsoleMessageTypes messageType)
        {
            // Go through each listener
        }

        #endregion

    }
}
