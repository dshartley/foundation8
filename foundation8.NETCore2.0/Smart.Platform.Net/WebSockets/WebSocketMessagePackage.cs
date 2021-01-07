using System;

namespace Smart.Platform.Net.WebSockets
{
    /// <summary>
    /// Encapsulates a Web Socket message package
    /// </summary>
    public class WebSocketMessagePackage
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="WebSocketMessagePackage"/> class from being created.
        /// </summary>
        private WebSocketMessagePackage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketMessagePackage"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="messageString">The message string.</param>
        public WebSocketMessagePackage(WebSocketDefinition definition, String messageString)
        {
            _webSocketDefinition    = definition;
            _messageString          = messageString;
        }

        #endregion

        #region Public Methods

        private WebSocketDefinition _webSocketDefinition = null;

        /// <summary>
        /// Gets or sets the web socket definition.
        /// </summary>
        /// <value>
        /// The web socket definition.
        /// </value>
        public WebSocketDefinition WebSocketDefinition
        {
            get { return _webSocketDefinition; }
            set { _webSocketDefinition = value; }
        }

        private String _messageString = null;

        /// <summary>
        /// Gets or sets the message string.
        /// </summary>
        /// <value>
        /// The message string.
        /// </value>
        public String MessageString
        {
            get { return _messageString; }
            set { _messageString = value; }
        }

        #endregion
    }
}
