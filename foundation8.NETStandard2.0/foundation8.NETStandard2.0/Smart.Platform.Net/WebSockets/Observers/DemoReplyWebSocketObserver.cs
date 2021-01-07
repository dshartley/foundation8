using Smart.Platform.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Smart.Platform.Net.WebSockets.Observers
{

    /// <summary>
    /// Observes Web Sockets for the Demo reply implementation
    /// </summary>
    /// <seealso cref="Smart.Platform.Net.WebSockets.IWebSocketObserver" />
    public class DemoReplyWebSocketObserver : IWebSocketObserver
    {
        #region Constructors

        public DemoReplyWebSocketObserver()
        {
        }

        #endregion

        #region IWebSocketObserver Members

        /// <summary>
        /// Called when [message received].
        /// </summary>
        /// <param name="messagePackage">The message package.</param>
        /// <exception cref="ApplicationException"></exception>
        public void OnMessageReceived(WebSocketMessagePackage messagePackage)
        {
            #region Check Parameters

            if (messagePackage == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "messagePackage is nothing"));

            #endregion

            // Create list of messages to send
            List<WebSocketMessagePackage> messages = new List<WebSocketMessagePackage>();

            messagePackage.MessageString = "DemoReply!: " + messagePackage.MessageString;

            // Add message to the list
            messages.Add(messagePackage);

            // Send messages
            Task.Run(() => WebSocketManager.Shared.SendMessages(messages));
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string AppID {
            get {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        /// <value>
        /// The channel identifier.
        /// </value>
        public string ChannelID {
            get
            {
                return String.Empty;
            }
        }

        #endregion
    }
}
