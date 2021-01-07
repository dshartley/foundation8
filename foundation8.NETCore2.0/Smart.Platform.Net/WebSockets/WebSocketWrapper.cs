using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;

namespace Smart.Platform.Net.WebSockets
{
    /// <summary>
    /// Encapsulates a Web Socket
    /// </summary>
    public class WebSocketWrapper
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="WebSocketWrapper"/> class from being created.
        /// </summary>
        private WebSocketWrapper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketWrapper"/> class.
        /// </summary>
        /// <param name="webSocketID">The web socket identifier.</param>
        /// <param name="webSocketDefinition">The web socket definition.</param>
        /// <param name="webSocket">The web socket.</param>
        public WebSocketWrapper(Guid webSocketID, WebSocketDefinition webSocketDefinition, WebSocket webSocket)
        {
            _webSocketID            = webSocketID;
            _webSocketDefinition    = webSocketDefinition;
            _webSocket              = webSocket;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            _webSocket              = null;
            _webSocketDefinition    = null;
        }

        private Guid _webSocketID;

        /// <summary>
        /// Gets or sets the web socket identifier.
        /// </summary>
        /// <value>
        /// The web socket identifier.
        /// </value>
        public Guid WebSocketID
        {
            get { return _webSocketID; }
            set { _webSocketID = value; }
        }

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

        private WebSocket _webSocket = null;

        /// <summary>
        /// Gets or sets the web socket.
        /// </summary>
        /// <value>
        /// The web socket.
        /// </value>
        public WebSocket WebSocket
        {
            get { return _webSocket; }
            set { _webSocket = value; }
        }

        #endregion
    }
}
