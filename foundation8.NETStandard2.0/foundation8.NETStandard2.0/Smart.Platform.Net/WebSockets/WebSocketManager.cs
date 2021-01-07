using Smart.Platform.Diagnostics;
using Smart.Platform.Net.WebSockets.Observers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Smart.Platform.Net.WebSockets
{
    /// <summary>
    /// Manages WebSockets
    /// </summary>
    public class WebSocketManager
    {
        private List<IWebSocketObserver>                                                    _webSocketObservers;
        private ConcurrentDictionary<Guid, WebSocketWrapper>                                _webSocketsAll;
        private ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>  _webSocketsByApp;
        private ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>  _webSocketsByAppChannel;
        private ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>  _webSocketsByAppClientRelativeMember;

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="WebSocketManager"/> class from being created.
        /// </summary>
        private WebSocketManager() {

            _webSocketObservers                     = new List<IWebSocketObserver>();
            _webSocketsAll                          = new ConcurrentDictionary<Guid, WebSocketWrapper>();
            _webSocketsByApp                        = new ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>();
            _webSocketsByAppChannel                 = new ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>();
            _webSocketsByAppClientRelativeMember    = new ConcurrentDictionary<String, ConcurrentDictionary<Guid, WebSocketWrapper>>();
        }

        #endregion

        #region Singleton

        private static WebSocketManager     _shared = null;
        private static readonly object      _threadLock = new object();

        /// <summary>
        /// Gets the shared.
        /// </summary>
        /// <value>
        /// The shared.
        /// </value>
        public static WebSocketManager Shared
        {
            get
            {
                lock (_threadLock)
                {
                    if (_shared == null)
                    {
                        _shared = new WebSocketManager();
                    }

                    return _shared;
                }
            }
        }

        #endregion

        #region DEBUG Methods

        // DEBUG: Test function called from WebApi
        public async Task WebSocketTestByIndex(int index, string message)
        {
            // Get the websocket with the specified index
            if (_webSocketsAll.Count <= index) return;

            WebSocketWrapper    wrapper = null;
            int                 counter = 0;

            foreach (WebSocketWrapper w in _webSocketsAll.Values)
            {
                wrapper = w;

                if (counter == index) break;

                counter++;

            }

            if (wrapper != null)
            {
                await this.DoSendString(wrapper.WebSocket, message);
            }

        }

        // DEBUG: Test function called from WebApi
        public async Task WebSocketTestByDefinition(string appID, string channelID, string clientRelativeMemberID, string message)
        {
            // Create the web socket definition
            WebSocketDefinition             definition = new WebSocketDefinition(appID, channelID, clientRelativeMemberID);

            // Create list of web socket message packages
            List<WebSocketMessagePackage>   packages = new List<WebSocketMessagePackage>();

            // Create a message package and add to the list
            WebSocketMessagePackage         package = new WebSocketMessagePackage(definition, message);
            packages.Add(package);

            await this.SendMessages(packages);

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the specified observer.
        /// </summary>
        /// <param name="observer">The observer.</param>
        public void Register(IWebSocketObserver observer)
        {
            _webSocketObservers.Add(observer);
        }

        /// <summary>
        /// Adds the web socket.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="webSocket">The web socket.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">
        /// </exception>
        public WebSocketWrapper AddWebSocket(WebSocketDefinition definition, WebSocket webSocket)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));
            if (webSocket == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "webSocket is nothing"));

            #endregion

            // Create webSocketID
            Guid                webSocketID = Guid.NewGuid();

            // Create web socket wrapper
            WebSocketWrapper    wrapper = new WebSocketWrapper(webSocketID, definition, webSocket);

            // Add web socket to memory
            this.DoAddWebSocketInMemory(wrapper);

            return wrapper;
        }

        /// <summary>
        /// Receives the string message.
        /// </summary>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<string> ReceiveStringMessage(WebSocketWrapper wrapper, CancellationToken cancellationToken)
        {
            #region Check Parameters

            if (wrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "wrapper is nothing"));

            #endregion

            // Get messageString
            string                      messageString = await this.DoReceiveString(wrapper.WebSocket, cancellationToken);

            // Check messageString
            if (String.IsNullOrEmpty(messageString)) return String.Empty;

            // Create messagePackage
            WebSocketMessagePackage     messagePackage = new WebSocketMessagePackage(wrapper.WebSocketDefinition, messageString);

            // Go through each observer
            foreach (IWebSocketObserver observer in _webSocketObservers)
            {
                // Check observer properties
                if ((String.IsNullOrEmpty(observer.AppID) || observer.AppID == wrapper.WebSocketDefinition.AppID)
                    && (String.IsNullOrEmpty(observer.ChannelID) || observer.ChannelID == wrapper.WebSocketDefinition.ChannelID))
                {           
                    // Notify the observer
                    observer.OnMessageReceived(messagePackage);
                }
            }

            return messageString;
        }

        /// <summary>
        /// Closes the web socket.
        /// </summary>
        /// <param name="wrapper">The wrapper.</param>
        /// <param name="webSocket">The web socket.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task CloseWebSocket(WebSocketWrapper wrapper)
        {
            #region Check Parameters

            if (wrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "wrapper is nothing"));

            #endregion

            // Remove from memory
            this.DoRemoveWebSocketInMemory(wrapper);

            // Close the web socket
            await this.DoCloseWebSocket(wrapper.WebSocket);

            wrapper.Dispose();
        }

        /// <summary>
        /// Sends the messages.
        /// </summary>
        /// <param name="messagePackages">The message packages.</param>
        /// <returns></returns>
        public async Task SendMessages(List<WebSocketMessagePackage> messagePackages)
        {
            #region Check Parameters

            if (messagePackages == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "messagePackages is nothing"));

            #endregion

            // Go through each message package
            foreach (WebSocketMessagePackage package in messagePackages)
            {
                WebSocketDefinition     definition = package.WebSocketDefinition;

                // Get web sockets
                List<WebSocketWrapper>  wrappers = this.GetWebSocketsByDefinition(definition, true);

                if (wrappers == null) continue;
                
                // Go through each wrapper
                foreach (WebSocketWrapper wrapper in wrappers)
                {
                    // Send message
                    await this.DoSendString(wrapper.WebSocket, package.MessageString);
                }
            }
        }

        #endregion

        #region Private Methods

        private void DoAddWebSocketInMemory(WebSocketWrapper wrapper)
        {
            #region Check Parameters

            if (wrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "wrapper is nothing"));

            #endregion

            WebSocketDefinition     definition = wrapper.WebSocketDefinition;
            Guid                    webSocketID = wrapper.WebSocketID;

            String                  byAppKey = this.GetByAppKey(definition);
            String                  byAppChannelKey = this.GetByAppChannelKey(definition);
            String                  byAppClientRelativeMemberKey = this.GetByAppClientRelativeMemberKey(definition);

            // Add to _webSocketsAll
            bool                    result = _webSocketsAll.TryAdd(webSocketID, wrapper);

            if (!result) return;

            // Add to _webSocketsByApp
            if (!String.IsNullOrEmpty(byAppKey))
            {
                // Add the collection of web sockets if it does not already exist
                _webSocketsByApp.TryAdd(byAppKey, new ConcurrentDictionary<Guid, WebSocketWrapper>());

                _webSocketsByApp[byAppKey]?.TryAdd(webSocketID, wrapper);
            }               

            // Add to _webSocketsByAppChannel
            if (!String.IsNullOrEmpty(byAppChannelKey))
            {
                // Add the collection of web sockets if it does not already exist
                _webSocketsByAppChannel.TryAdd(byAppChannelKey, new ConcurrentDictionary<Guid, WebSocketWrapper>());

                _webSocketsByAppChannel[byAppChannelKey]?.TryAdd(webSocketID, wrapper);
            }
            

            // Add to _webSocketsByAppClientRelativeMember
            if (!String.IsNullOrEmpty(byAppClientRelativeMemberKey))
            {
                // Add the collection of web sockets if it does not already exist
                _webSocketsByAppClientRelativeMember.TryAdd(byAppClientRelativeMemberKey, new ConcurrentDictionary<Guid, WebSocketWrapper>());

                _webSocketsByAppClientRelativeMember[byAppClientRelativeMemberKey]?.TryAdd(webSocketID, wrapper);
            }
        }

        private void DoRemoveWebSocketInMemory(WebSocketWrapper wrapper)
        {
            #region Check Parameters

            if (wrapper == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "wrapper is nothing"));

            #endregion

            // Remove from _webSocketsAll
            bool    result = _webSocketsAll.TryRemove(wrapper.WebSocketID, out var removedWrapper);

            if (!result) return;

            String  byAppKey = this.GetByAppKey(wrapper.WebSocketDefinition);
            String  byAppChannelKey = this.GetByAppChannelKey(wrapper.WebSocketDefinition);
            String  byAppClientRelativeMemberKey = this.GetByAppClientRelativeMemberKey(wrapper.WebSocketDefinition);

            // Remove from _webSocketsByApp
            if (!String.IsNullOrEmpty(byAppKey))
            {
                _webSocketsByApp[byAppKey]?.TryRemove(wrapper.WebSocketID, out removedWrapper);
            } 

            // Remove from _webSocketsByAppChannel
            if (!String.IsNullOrEmpty(byAppChannelKey))
            {
                _webSocketsByAppChannel[byAppChannelKey]?.TryRemove(wrapper.WebSocketID, out removedWrapper);
            }
            
            // Remove from _webSocketsByAppClientRelativeMember
            if (!String.IsNullOrEmpty(byAppClientRelativeMemberKey))
            {
                _webSocketsByAppClientRelativeMember[byAppClientRelativeMemberKey]?.TryRemove(wrapper.WebSocketID, out removedWrapper);
            }
            
        }

        private async Task DoCloseWebSocket(WebSocket webSocket)
        {
            #region Check Parameters

            if (webSocket == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "webSocket is nothing"));

            #endregion

            // Check state
            if (webSocket?.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
                
            webSocket.Dispose();
        }

        private async Task<string> DoReceiveString(WebSocket webSocket, CancellationToken cancellationToken = default(CancellationToken))
        {
            #region Check Parameters

            if (webSocket == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "webSocket is nothing"));

            #endregion

            var buffer = new ArraySegment<byte>(new byte[8192]);

            using (var memoryStream = new MemoryStream())
            {
                WebSocketReceiveResult result;

                do
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Receive data from web socket
                    result = await webSocket.ReceiveAsync(buffer, cancellationToken);

                    // Write to memory stream
                    memoryStream.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                memoryStream.Seek(0, SeekOrigin.Begin);

                // Check message is text
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                // Read from memory stream
                // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        private async Task DoSendString(WebSocket webSocket, string messageString)
        {
            #region Check Parameters

            if (webSocket == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "webSocket is nothing"));

            #endregion

            // Send data to web socket
            await webSocket.SendAsync(  new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageString)),
                                        WebSocketMessageType.Text,
                                        true,
                                        CancellationToken.None);
        }

        private List<WebSocketWrapper> GetWebSocketsByApp(WebSocketDefinition definition, bool filterYN)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            List<WebSocketWrapper>  result = new List<WebSocketWrapper>();

            String                  key = this.GetByAppKey(definition);
            List<WebSocketWrapper>  unfilteredWebSockets = new List<WebSocketWrapper>();

            // Get web sockets by app
            if (!String.IsNullOrEmpty(key)
                && _webSocketsByApp.ContainsKey(key))
            {
                unfilteredWebSockets.AddRange(_webSocketsByApp[key].Values);
            }

            if (filterYN)
            {
                // Go through each item
                foreach (WebSocketWrapper wrapper in unfilteredWebSockets)
                {
                    // Check the ChannelID and ClientRelativeMemberID not specified
                    if (String.IsNullOrEmpty(wrapper.WebSocketDefinition.ChannelID) && String.IsNullOrEmpty(wrapper.WebSocketDefinition.ClientRelativeMemberID))
                    {
                        // Add the item
                        result.Add(wrapper);
                    }
                }
            }
            else
            {
                result = unfilteredWebSockets;
            }

            return result;
        }

        private List<WebSocketWrapper> GetWebSocketsByAppChannel(WebSocketDefinition definition, bool filterYN)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            List<WebSocketWrapper>  result = new List<WebSocketWrapper>();

            String                  key = this.GetByAppChannelKey(definition);
            List<WebSocketWrapper>  unfilteredWebSockets = new List<WebSocketWrapper>();

            // Get web sockets by app channel
            if (!String.IsNullOrEmpty(key)
                && _webSocketsByAppChannel.ContainsKey(key))
            {
                unfilteredWebSockets.AddRange(_webSocketsByAppChannel[key].Values);
            }

            if (filterYN)
            {
                // Go through each item
                foreach (WebSocketWrapper wrapper in unfilteredWebSockets)
                {
                    // Check the ClientRelativeMemberID not specified
                    if (String.IsNullOrEmpty(wrapper.WebSocketDefinition.ClientRelativeMemberID))
                    {
                        // Add the item
                        result.Add(wrapper);
                    }
                }
            }
            else
            {
                result = unfilteredWebSockets;
            }

            return result;
        }

        private List<WebSocketWrapper> GetWebSocketsByAppClientRelativeMember(WebSocketDefinition definition, bool filterYN)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            List<WebSocketWrapper>  result = new List<WebSocketWrapper>();

            String                  key = this.GetByAppClientRelativeMemberKey(definition);
            List<WebSocketWrapper>  unfilteredWebSockets = new List<WebSocketWrapper>();

            // Get web sockets by app client
            if (!String.IsNullOrEmpty(key)
                && _webSocketsByAppClientRelativeMember.ContainsKey(key))
            {
                unfilteredWebSockets.AddRange(_webSocketsByAppClientRelativeMember[key].Values);
            }

            if (filterYN)
            {
                // Go through each item
                foreach (WebSocketWrapper wrapper in unfilteredWebSockets)
                {
                    // Check the ChannelID not specified
                    if (String.IsNullOrEmpty(wrapper.WebSocketDefinition.ChannelID))
                    {
                        // Add the item
                        result.Add(wrapper);
                    }
                }
            }
            else
            {
                result = unfilteredWebSockets;
            }

            return result;
        }

        private List<WebSocketWrapper> GetWebSocketsByAppChannelClientRelativeMember(WebSocketDefinition definition)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            List<WebSocketWrapper>  result = new List<WebSocketWrapper>();

            String                  key = this.GetByAppClientRelativeMemberKey(definition);
            List<WebSocketWrapper>  unfilteredWebSockets = new List<WebSocketWrapper>();

            // Get web sockets by app client relative member
            if (!String.IsNullOrEmpty(key)
                && _webSocketsByAppClientRelativeMember.ContainsKey(key))
            {
                unfilteredWebSockets.AddRange(_webSocketsByAppClientRelativeMember[key].Values);
            }

            // Go through each item
            foreach (WebSocketWrapper wrapper in unfilteredWebSockets)
            {
                // Check the ChannelID
                if (wrapper.WebSocketDefinition.ChannelID == definition.ChannelID)
                {
                    // Add the item if on the specified channel
                    result.Add(wrapper);
                }
            }

            return result;
        }

        private List<WebSocketWrapper> GetWebSocketsByDefinition(WebSocketDefinition definition, bool filterYN)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            List<WebSocketWrapper> result = new List<WebSocketWrapper>();

            // Check AppID
            if (!String.IsNullOrEmpty(definition.AppID))
            {
                // Check ChannelID
                if (!String.IsNullOrEmpty(definition.ChannelID))
                {
                    if (!String.IsNullOrEmpty(definition.ClientRelativeMemberID))
                    {
                        // Get web sockets by AppID.ChannelID.ClientRelativeMemberID
                       result = this.GetWebSocketsByAppChannelClientRelativeMember(definition);
                    }
                    else
                    {
                        // Get web sockets by AppID.ChannelID
                        result = this.GetWebSocketsByAppChannel(definition, filterYN);
                    }
                }
                else if (!String.IsNullOrEmpty(definition.ClientRelativeMemberID))
                {
                    // Get web sockets by AppID.ClientRelativeMemberID
                    result = this.GetWebSocketsByAppClientRelativeMember(definition, filterYN);
                }
                else
                {
                    // Get web sockets by AppID
                    result = this.GetWebSocketsByApp(definition, filterYN);
                }
            }

            return result;
        }

        private String GetByAppKey(WebSocketDefinition definition)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            String result = String.Empty;

            if (!String.IsNullOrEmpty(definition.AppID))
            {
                result = definition.AppID;
            }

            return result;
        }

        private String GetByAppChannelKey(WebSocketDefinition definition)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            String result = String.Empty;

            if (!String.IsNullOrEmpty(definition.AppID) && !String.IsNullOrEmpty(definition.ChannelID))
            {
                result = definition.AppID + "." + definition.ChannelID;
            }

            return result;
        }

        private String GetByAppClientRelativeMemberKey(WebSocketDefinition definition)
        {
            #region Check Parameters

            if (definition == null) throw new ApplicationException(ExceptionManager.MessageMethodFailed(System.Reflection.MethodBase.GetCurrentMethod(), "definition is nothing"));

            #endregion

            String result = String.Empty;

            if (!String.IsNullOrEmpty(definition.AppID) && !String.IsNullOrEmpty(definition.ClientRelativeMemberID))
            {
                result = definition.AppID + "." + definition.ClientRelativeMemberID;
            }

            return result;
        }

        #endregion
    }
}
