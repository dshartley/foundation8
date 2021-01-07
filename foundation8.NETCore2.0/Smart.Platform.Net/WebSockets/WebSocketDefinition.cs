using System.Collections.Generic;
using System;

namespace Smart.Platform.Net.WebSockets
{
    /// <summary>
    /// Encapsulates a Web Socket definition
    /// </summary>
    public class WebSocketDefinition
    {
        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="WebSocketDefinition"/> class from being created.
        /// </summary>
        private WebSocketDefinition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketDefinition"/> class.
        /// </summary>
        /// <param name="appID">The application identifier.</param>
        public WebSocketDefinition(string appID)
        {
            _appID = appID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketDefinition"/> class.
        /// </summary>
        /// <param name="appID">The application identifier.</param>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="clientRelativeMemberID">The client relative member identifier.</param>
        public WebSocketDefinition(string appID, string channelID, string clientRelativeMemberID)
        {
            _appID                  = appID;
            _channelID              = channelID;
            _clientRelativeMemberID = clientRelativeMemberID;
        }

        #endregion

        #region Public Methods

        private string _appID = string.Empty;

        /// <summary>
        /// Gets or sets the App ID.
        /// </summary>
        /// <value>
        /// The App ID.
        /// </value>
        public string AppID
        {
            get { return _appID; }
            set { _appID = value; }
        }

        private string _channelID = string.Empty;

        /// <summary>
        /// Gets or sets the Channel ID.
        /// </summary>
        /// <value>
        /// The Channel ID.
        /// </value>
        public string ChannelID
        {
            get { return _channelID; }
            set { _channelID = value; }
        }

        private string _clientRelativeMemberID = string.Empty;

        /// <summary>
        /// Gets or sets the client relative member identifier.
        /// </summary>
        /// <value>
        /// The client relative member identifier.
        /// </value>
        public string ClientRelativeMemberID
        {
            get { return _clientRelativeMemberID; }
            set { _clientRelativeMemberID = value; }
        }

        Dictionary<String, String> _params = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<String, String> Params
        {
            get { return _params; }
            set { _params = value; }
        }

        #endregion
    }
}
