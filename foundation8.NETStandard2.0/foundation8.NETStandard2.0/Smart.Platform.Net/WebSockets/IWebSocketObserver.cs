using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Platform.Net.WebSockets
{
    /// <summary>
    /// Defines a class which observes a Web Socket.
    /// </summary>
    public interface IWebSocketObserver
    {
        /// <summary>
        /// Called when [message received].
        /// </summary>
        /// <param name="messagePackage">The message package.</param>
        void OnMessageReceived(WebSocketMessagePackage messagePackage);

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        string AppID { get; }

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        /// <value>
        /// The channel identifier.
        /// </value>
        string ChannelID { get; }
    }
}
