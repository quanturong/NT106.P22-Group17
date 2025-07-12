
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif

namespace Photon.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using ExitGames.Client.Photon;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class ChatClient : IPhotonPeerListener
    {
        const int FriendRequestListMax = 1024;
        public const int DefaultMaxSubscribers = 100;

        private const byte HttpForwardWebFlag = 0x01;
        public bool EnableProtocolFallback { get; set; }
        public string NameServerAddress { get; private set; }
        public string FrontendAddress { get; private set; }
        private string chatRegion = "EU";
        public string ChatRegion
        {
            get { return this.chatRegion; }
            set { this.chatRegion = value; }
        }
        public string ProxyServerAddress;
        public ChatState State { get; private set; }
        public ChatDisconnectCause DisconnectedCause { get; private set; }
        public bool CanChat
        {
            get { return this.State == ChatState.ConnectedToFrontEnd && this.HasPeer; }
        }
        public bool CanChatInChannel(string channelName)
        {
            return this.CanChat && this.PublicChannels.ContainsKey(channelName) && !this.PublicChannelsUnsubscribing.Contains(channelName);
        }

        private bool HasPeer
        {
            get { return this.chatPeer != null; }
        }
        public string AppVersion { get; private set; }
        public string AppId { get; private set; }
        public AuthenticationValues AuthValues { get; set; }
        public string UserId
        {
            get
            {
                return (this.AuthValues != null) ? this.AuthValues.UserId : null;
            }
            private set
            {
                if (this.AuthValues == null)
                {
                    this.AuthValues = new AuthenticationValues();
                }
                this.AuthValues.UserId = value;
            }
        }
        public int MessageLimit;
        public int PrivateChatHistoryLength = -1;
        public readonly Dictionary<string, ChatChannel> PublicChannels;
        public readonly Dictionary<string, ChatChannel> PrivateChannels;
        private readonly HashSet<string> PublicChannelsUnsubscribing;

        private readonly IChatClientListener listener = null;
        public ChatPeer chatPeer = null;
        private const string ChatAppName = "chat";
        private bool didAuthenticate;

        private int? statusToSetWhenConnected;
        private object messageToSetWhenConnected;

        private int msDeltaForServiceCalls = 50;
        private int msTimestampOfLastServiceCall;
        public bool UseBackgroundWorkerForSending { get; set; }
        public ConnectionProtocol TransportProtocol
        {
            get { return this.chatPeer.TransportProtocol; }
            set
            {
                if (this.chatPeer == null || this.chatPeer.PeerState != PeerStateValue.Disconnected)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "Can't set TransportProtocol. Disconnect first! " + ((this.chatPeer != null) ? "PeerState: " + this.chatPeer.PeerState : "The chatPeer is null."));
                    return;
                }
                this.chatPeer.TransportProtocol = value;
            }
        }
        public Dictionary<ConnectionProtocol, Type> SocketImplementationConfig
        {
            get { return this.chatPeer.SocketImplementationConfig; }
        }
        public ChatClient(IChatClientListener listener, ConnectionProtocol protocol = ConnectionProtocol.Udp)
        {
            this.listener = listener;
            this.State = ChatState.Uninitialized;

            this.chatPeer = new ChatPeer(this, protocol);
            this.chatPeer.SerializationProtocolType = SerializationProtocol.GpBinaryV18;

            this.PublicChannels = new Dictionary<string, ChatChannel>();
            this.PrivateChannels = new Dictionary<string, ChatChannel>();

            this.PublicChannelsUnsubscribing = new HashSet<string>();
        }


        public bool ConnectUsingSettings(ChatAppSettings appSettings)
        {
            if (appSettings == null)
            {
                this.listener.DebugReturn(DebugLevel.ERROR, "ConnectUsingSettings failed. The appSettings can't be null.'");
                return false;
            }

            if (!string.IsNullOrEmpty(appSettings.FixedRegion))
            {
                this.ChatRegion = appSettings.FixedRegion;
            }

            this.DebugOut = appSettings.NetworkLogging;

            this.TransportProtocol = appSettings.Protocol;
            this.EnableProtocolFallback = appSettings.EnableProtocolFallback;

            if (!appSettings.IsDefaultNameServer)
            {
                this.chatPeer.NameServerHost = appSettings.Server;
                this.chatPeer.NameServerPortOverride = appSettings.Port;
            }

            this.ProxyServerAddress = appSettings.ProxyServer;

            return this.Connect(appSettings.AppIdChat, appSettings.AppVersion, this.AuthValues);
        }
        public bool Connect(string appId, string appVersion, AuthenticationValues authValues)
        {
            this.chatPeer.TimePingInterval = 3000;
            this.DisconnectedCause = ChatDisconnectCause.None;

            if (authValues != null)
            {
                this.AuthValues = authValues;
            }

            this.AppId = appId;
            this.AppVersion = appVersion;
            this.didAuthenticate = false;
            this.chatPeer.QuickResendAttempts = 2;
            this.chatPeer.SentCountAllowance = 7;
            this.PublicChannels.Clear();
            this.PrivateChannels.Clear();
            this.PublicChannelsUnsubscribing.Clear();

            #if UNITY_WEBGL
            if (this.TransportProtocol == ConnectionProtocol.Tcp || this.TransportProtocol == ConnectionProtocol.Udp)
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "WebGL requires WebSockets. Switching TransportProtocol to WebSocketSecure.");
                this.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }
            #endif

            this.NameServerAddress = this.chatPeer.NameServerAddress;

            bool isConnecting = this.chatPeer.Connect(this.NameServerAddress, this.ProxyServerAddress, "NameServer", null);
            if (isConnecting)
            {
                this.State = ChatState.ConnectingToNameServer;
            }

            if (this.UseBackgroundWorkerForSending)
            {
                #if UNITY_SWITCH
                SupportClass.StartBackgroundCalls(this.SendOutgoingInBackground, this.msDeltaForServiceCalls);  // as workaround, we don't name the Thread.
                #else
                SupportClass.StartBackgroundCalls(this.SendOutgoingInBackground, this.msDeltaForServiceCalls, "ChatClient Service Thread");
                #endif
            }

            return isConnecting;
        }
        public bool ConnectAndSetStatus(string appId, string appVersion, AuthenticationValues authValues,
            int status = ChatUserStatus.Online, object message = null)
        {
            statusToSetWhenConnected = status;
            messageToSetWhenConnected = message;
            return Connect(appId, appVersion, authValues);
        }
        public void Service()
        {
            while (this.HasPeer && this.chatPeer.DispatchIncomingCommands())
            {
            }
            if (!this.UseBackgroundWorkerForSending)
            {
                if (Environment.TickCount - this.msTimestampOfLastServiceCall > this.msDeltaForServiceCalls || this.msTimestampOfLastServiceCall == 0)
                {
                    this.msTimestampOfLastServiceCall = Environment.TickCount;

                    while (this.HasPeer && this.chatPeer.SendOutgoingCommands())
                    {
                    }
                }
            }
        }
        private bool SendOutgoingInBackground()
        {
            while (this.HasPeer && this.chatPeer.SendOutgoingCommands())
            {
            }

            return this.State != ChatState.Disconnected;
        }
        [Obsolete("Better use UseBackgroundWorkerForSending and Service().")]
        public void SendAcksOnly()
        {
            if (this.HasPeer) this.chatPeer.SendAcksOnly();
        }
        public void Disconnect(ChatDisconnectCause cause = ChatDisconnectCause.DisconnectByClientLogic)
        {
            if (this.HasPeer && this.chatPeer.PeerState != PeerStateValue.Disconnected)
            {
                this.State = ChatState.Disconnecting;
                this.DisconnectedCause = cause;
                this.chatPeer.Disconnect();
            }
        }
        public void StopThread()
        {
            if (this.HasPeer)
            {
                this.chatPeer.StopThread();
            }
        }
        public bool Subscribe(string[] channels)
        {
            return this.Subscribe(channels, 0);
        }
        public bool Subscribe(string[] channels, int[] lastMsgIds)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe called while not connected to front end server.");
                }
                return false;
            }

            if (channels == null || channels.Length == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "Subscribe can't be called for empty or null channels-list.");
                }
                return false;
            }

            for (int i = 0; i < channels.Length; i++)
            {
                if (string.IsNullOrEmpty(channels[i]))
                {
                    if (this.DebugOut >= DebugLevel.ERROR)
                    {
                        this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Subscribe can't be called with a null or empty channel name at index {0}.", i));
                    }
                    return false;
                }
            }

            if (lastMsgIds == null || lastMsgIds.Length != channels.Length)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe can't be called when \"lastMsgIds\" array is null or does not have the same length as \"channels\" array.");
                }
                return false;
            }

            Dictionary<byte, object> opParameters = new Dictionary<byte, object>
            {
                { ChatParameterCode.Channels, channels },
                { ChatParameterCode.MsgIds,  lastMsgIds},
                { ChatParameterCode.HistoryLength, -1 } // server will decide how many messages to send to client
            };

            return this.chatPeer.SendOperation(ChatOperationCode.Subscribe, opParameters, SendOptions.SendReliable);
        }
        public bool Subscribe(string[] channels, int messagesFromHistory)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe called while not connected to front end server.");
                }
                return false;
            }

            if (channels == null || channels.Length == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "Subscribe can't be called for empty or null channels-list.");
                }
                return false;
            }

            return this.SendChannelOperation(channels, (byte)ChatOperationCode.Subscribe, messagesFromHistory);
        }
        public bool Unsubscribe(string[] channels)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Unsubscribe called while not connected to front end server.");
                }
                return false;
            }

            if (channels == null || channels.Length == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "Unsubscribe can't be called for empty or null channels-list.");
                }
                return false;
            }

            foreach (string ch in channels)
            {
                this.PublicChannelsUnsubscribing.Add(ch);
            }
            return this.SendChannelOperation(channels, ChatOperationCode.Unsubscribe, 0);
        }
        public bool PublishMessage(string channelName, object message, bool forwardAsWebhook = false)
        {
            return this.publishMessage(channelName, message, true, forwardAsWebhook);
        }

        internal bool PublishMessageUnreliable(string channelName, object message, bool forwardAsWebhook = false)
        {
            return this.publishMessage(channelName, message, false, forwardAsWebhook);
        }

        private bool publishMessage(string channelName, object message, bool reliable, bool forwardAsWebhook = false)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "PublishMessage called while not connected to front end server.");
                }
                return false;
            }

            if (string.IsNullOrEmpty(channelName) || message == null)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "PublishMessage parameters must be non-null and not empty.");
                }
                return false;
            }

            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                {
                    { (byte)ChatParameterCode.Channel, channelName },
                    { (byte)ChatParameterCode.Message, message }
                };
            if (forwardAsWebhook)
            {
                parameters.Add(ChatParameterCode.WebFlags, (byte)0x1);
            }

            return this.chatPeer.SendOperation(ChatOperationCode.Publish, parameters, new SendOptions() { Reliability = reliable });
        }
        public bool SendPrivateMessage(string target, object message, bool forwardAsWebhook = false)
        {
            return this.SendPrivateMessage(target, message, false, forwardAsWebhook);
        }
        public bool SendPrivateMessage(string target, object message, bool encrypt, bool forwardAsWebhook)
        {
            return this.sendPrivateMessage(target, message, encrypt, true, forwardAsWebhook);
        }

        internal bool SendPrivateMessageUnreliable(string target, object message, bool encrypt, bool forwardAsWebhook = false)
        {
            return this.sendPrivateMessage(target, message, encrypt, false, forwardAsWebhook);
        }

        private bool sendPrivateMessage(string target, object message, bool encrypt, bool reliable, bool forwardAsWebhook = false)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "SendPrivateMessage called while not connected to front end server.");
                }
                return false;
            }

            if (string.IsNullOrEmpty(target) || message == null)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "SendPrivateMessage parameters must be non-null and not empty.");
                }
                return false;
            }

            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                {
                    { ChatParameterCode.UserId, target },
                    { ChatParameterCode.Message, message }
                };
            if (forwardAsWebhook)
            {
                parameters.Add(ChatParameterCode.WebFlags, (byte)0x1);
            }

            return this.chatPeer.SendOperation(ChatOperationCode.SendPrivate, parameters, new SendOptions() { Reliability = reliable, Encrypt = encrypt });
        }
        private bool SetOnlineStatus(int status, object message, bool skipMessage)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "SetOnlineStatus called while not connected to front end server.");
                }
                return false;
            }

            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                {
                    { ChatParameterCode.Status, status },
                };

            if (skipMessage)
            {
                parameters[ChatParameterCode.SkipMessage] = true;
            }
            else
            {
                parameters[ChatParameterCode.Message] = message;
            }

            return this.chatPeer.SendOperation(ChatOperationCode.UpdateStatus, parameters, SendOptions.SendReliable);
        }
        public bool SetOnlineStatus(int status)
        {
            return this.SetOnlineStatus(status, null, true);
        }
        public bool SetOnlineStatus(int status, object message)
        {
            return this.SetOnlineStatus(status, message, false);
        }
        public bool AddFriends(string[] friends)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "AddFriends called while not connected to front end server.");
                }
                return false;
            }

            if (friends == null || friends.Length == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "AddFriends can't be called for empty or null list.");
                }
                return false;
            }
            if (friends.Length > FriendRequestListMax)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "AddFriends max list size exceeded: " + friends.Length + " > " + FriendRequestListMax);
                }
                return false;
            }

            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                {
                    { ChatParameterCode.Friends, friends },
                };

            return this.chatPeer.SendOperation(ChatOperationCode.AddFriends, parameters, SendOptions.SendReliable);
        }
        public bool RemoveFriends(string[] friends)
        {
            if (!this.CanChat)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "RemoveFriends called while not connected to front end server.");
                }
                return false;
            }

            if (friends == null || friends.Length == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "RemoveFriends can't be called for empty or null list.");
                }
                return false;
            }
            if (friends.Length > FriendRequestListMax)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "RemoveFriends max list size exceeded: " + friends.Length + " > " + FriendRequestListMax);
                }
                return false;
            }

            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                {
                    { ChatParameterCode.Friends, friends },
                };

            return this.chatPeer.SendOperation(ChatOperationCode.RemoveFriends, parameters, SendOptions.SendReliable);
        }
        public string GetPrivateChannelNameByUser(string userName)
        {
            return string.Format("{0}:{1}", this.UserId, userName);
        }
        public bool TryGetChannel(string channelName, bool isPrivate, out ChatChannel channel)
        {
            if (!isPrivate)
            {
                return this.PublicChannels.TryGetValue(channelName, out channel);
            }
            else
            {
                return this.PrivateChannels.TryGetValue(channelName, out channel);
            }
        }
        public bool TryGetChannel(string channelName, out ChatChannel channel)
        {
            bool found = false;
            found = this.PublicChannels.TryGetValue(channelName, out channel);
            if (found) return true;

            found = this.PrivateChannels.TryGetValue(channelName, out channel);
            return found;
        }
        public bool TryGetPrivateChannelByUser(string userId, out ChatChannel channel)
        {
            channel = null;
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }
            string channelName = this.GetPrivateChannelNameByUser(userId);
            return this.TryGetChannel(channelName, true, out channel);
        }
        public DebugLevel DebugOut
        {
            set { this.chatPeer.DebugOut = value; }
            get { return this.chatPeer.DebugOut; }
        }

        #region Private methods area

        #region IPhotonPeerListener implementation

        void IPhotonPeerListener.DebugReturn(DebugLevel level, string message)
        {
            this.listener.DebugReturn(level, message);
        }

        void IPhotonPeerListener.OnEvent(EventData eventData)
        {
            switch (eventData.Code)
            {
                case ChatEventCode.ChatMessages:
                    this.HandleChatMessagesEvent(eventData);
                    break;
                case ChatEventCode.PrivateMessage:
                    this.HandlePrivateMessageEvent(eventData);
                    break;
                case ChatEventCode.StatusUpdate:
                    this.HandleStatusUpdate(eventData);
                    break;
                case ChatEventCode.Subscribe:
                    this.HandleSubscribeEvent(eventData);
                    break;
                case ChatEventCode.Unsubscribe:
                    this.HandleUnsubscribeEvent(eventData);
                    break;
                case ChatEventCode.UserSubscribed:
                    this.HandleUserSubscribedEvent(eventData);
                    break;
                case ChatEventCode.UserUnsubscribed:
                    this.HandleUserUnsubscribedEvent(eventData);
                    break;
                #if CHAT_EXTENDED
                case ChatEventCode.PropertiesChanged:
                    this.HandlePropertiesChanged(eventData);
                    break;
                case ChatEventCode.ErrorInfo:
                    this.HandleErrorInfoEvent(eventData);
                    break;
                #endif
            }
        }

        void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
        {
            switch (operationResponse.OperationCode)
            {
                case (byte)ChatOperationCode.Authenticate:
                    this.HandleAuthResponse(operationResponse);
                    break;
                case (byte)ChatOperationCode.Subscribe:
                case (byte)ChatOperationCode.Unsubscribe:
                case (byte)ChatOperationCode.Publish:
                case (byte)ChatOperationCode.SendPrivate:
                default:
                    if ((operationResponse.ReturnCode != 0) && (this.DebugOut >= DebugLevel.ERROR))
                    {
                        if (operationResponse.ReturnCode == -2)
                        {
                            this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} unknown on server. Check your AppId and make sure it's for a Chat application.", operationResponse.OperationCode));
                        }
                        else
                        {
                            this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} failed (Code: {1}). Debug Message: {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage));
                        }
                    }
                    break;
            }
        }

        void IPhotonPeerListener.OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    if (!this.chatPeer.IsProtocolSecure)
                    {
                        if (!this.chatPeer.EstablishEncryption())
                        {
                            if (this.DebugOut >= DebugLevel.ERROR)
                            {
                                this.listener.DebugReturn(DebugLevel.ERROR, "Error establishing encryption");
                            }
                        }
                    }
                    else
                    {
                        this.TryAuthenticateOnNameServer();
                    }

                    if (this.State == ChatState.ConnectingToNameServer)
                    {
                        this.State = ChatState.ConnectedToNameServer;
                        this.listener.OnChatStateChange(this.State);
                    }
                    else if (this.State == ChatState.ConnectingToFrontEnd)
                    {
                        if (!this.AuthenticateOnFrontEnd())
                        {
                            if (this.DebugOut >= DebugLevel.ERROR)
                            {
                                this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Error authenticating on frontend! Check log output, AuthValues and if you're connected. State: {0}", this.State));
                            }
                        }
                    }
                    break;
                case StatusCode.EncryptionEstablished:
                    this.TryAuthenticateOnNameServer();
                    break;
                case StatusCode.Disconnect:
                    switch (this.State)
                    {
                        case ChatState.ConnectWithFallbackProtocol:
                            this.EnableProtocolFallback = false;        // the client does a fallback only one time
                            this.chatPeer.NameServerPortOverride = 0;   // resets a value in the peer only (as we change the protocol, the port has to change, too)
                            this.chatPeer.TransportProtocol = (this.chatPeer.TransportProtocol == ConnectionProtocol.Tcp) ? ConnectionProtocol.Udp : ConnectionProtocol.Tcp;
                            this.Connect(this.AppId, this.AppVersion, null);
                            return;

                        case ChatState.Authenticated:
                            this.ConnectToFrontEnd();
                            return;
                        case ChatState.Disconnecting:
                            break;
                        default:
                            string stacktrace = string.Empty;
                            #if DEBUG && !NETFX_CORE
                            stacktrace = new System.Diagnostics.StackTrace(true).ToString();
                            #endif
                            this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Got a unexpected Disconnect in ChatState: {0}. Server: {1} Trace: {2}", this.State, this.chatPeer.ServerAddress, stacktrace));
                            break;
                    }
                    if (this.AuthValues != null)
                    {
                        this.AuthValues.Token = null; // when leaving the server, invalidate the secret (but not the auth values)
                    }
                    this.State = ChatState.Disconnected;
                    this.listener.OnChatStateChange(ChatState.Disconnected);
                    this.listener.OnDisconnected();
                    break;
                case StatusCode.DisconnectByServerUserLimit:
                    this.listener.DebugReturn(DebugLevel.ERROR, "This connection was rejected due to the apps CCU limit.");
                    this.Disconnect(ChatDisconnectCause.MaxCcuReached);
                    break;
                case StatusCode.ExceptionOnConnect:
                case StatusCode.SecurityExceptionOnConnect:
                case StatusCode.EncryptionFailedToEstablish:
                    this.DisconnectedCause = ChatDisconnectCause.ExceptionOnConnect;
                    if (this.EnableProtocolFallback && this.State == ChatState.ConnectingToNameServer)
                    {
                        this.State = ChatState.ConnectWithFallbackProtocol;
                    }
                    else
                    {
                        this.Disconnect(ChatDisconnectCause.ExceptionOnConnect);
                    }

                    break;
                case StatusCode.Exception:
                case StatusCode.ExceptionOnReceive:
                    this.Disconnect(ChatDisconnectCause.Exception);
                    break;
                case StatusCode.DisconnectByServerTimeout:
                    this.Disconnect(ChatDisconnectCause.ServerTimeout);
                    break;
                case StatusCode.DisconnectByServerLogic:
                    this.Disconnect(ChatDisconnectCause.DisconnectByServerLogic);
                    break;
                case StatusCode.DisconnectByServerReasonUnknown:
                    this.Disconnect(ChatDisconnectCause.DisconnectByServerReasonUnknown);
                    break;
                case StatusCode.TimeoutDisconnect:
                    this.DisconnectedCause = ChatDisconnectCause.ClientTimeout;
                    if (this.EnableProtocolFallback && this.State == ChatState.ConnectingToNameServer)
                    {
                        this.State = ChatState.ConnectWithFallbackProtocol;
                    }
                    else
                    {
                        this.Disconnect(ChatDisconnectCause.ClientTimeout);
                    }
                    break;
            }
        }

        #if SDK_V4
        void IPhotonPeerListener.OnMessage(object msg)
        {
            string channelName = null;
            var receivedBytes = (byte[])msg;
            var channelId = BitConverter.ToInt32(receivedBytes, 0);
            var messageBytes = new byte[receivedBytes.Length - 4];
            Array.Copy(receivedBytes, 4, messageBytes, 0, receivedBytes.Length - 4);

            foreach (var channel in this.PublicChannels)
            {
                if (channel.Value.ChannelID == channelId)
                {
                    channelName = channel.Key;
                    break;
                }
            }

            if (channelName != null)
            {
                this.listener.DebugReturn(DebugLevel.ALL, string.Format("got OnMessage in channel {0}", channelName));
            }
            else
            {
                this.listener.DebugReturn(DebugLevel.WARNING, string.Format("got OnMessage in unknown channel {0}", channelId));
            }

            this.listener.OnReceiveBroadcastMessage(channelName, messageBytes);
        }
        #endif

        #endregion

        private void TryAuthenticateOnNameServer()
        {
            if (!this.didAuthenticate)
            {
                this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.ChatRegion, this.AuthValues);
                if (!this.didAuthenticate)
                {
                    if (this.DebugOut >= DebugLevel.ERROR)
                    {
                        this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Error calling OpAuthenticate! Did not work on NameServer. Check log output, AuthValues and if you're connected. State: {0}", this.State));
                    }
                }
            }
        }

        private bool SendChannelOperation(string[] channels, byte operation, int historyLength)
        {
            Dictionary<byte, object> opParameters = new Dictionary<byte, object> { { (byte)ChatParameterCode.Channels, channels } };

            if (historyLength != 0)
            {
                opParameters.Add((byte)ChatParameterCode.HistoryLength, historyLength);
            }

            return this.chatPeer.SendOperation(operation, opParameters, SendOptions.SendReliable);
        }

        private void HandlePrivateMessageEvent(EventData eventData)
        {

            object message = (object)eventData.Parameters[(byte)ChatParameterCode.Message];
            string sender = (string)eventData.Parameters[(byte)ChatParameterCode.Sender];
            int msgId = (int)eventData.Parameters[ChatParameterCode.MsgId];

            string channelName;
            if (this.UserId != null && this.UserId.Equals(sender))
            {
                string target = (string)eventData.Parameters[(byte)ChatParameterCode.UserId];
                channelName = this.GetPrivateChannelNameByUser(target);
            }
            else
            {
                channelName = this.GetPrivateChannelNameByUser(sender);
            }

            ChatChannel channel;
            if (!this.PrivateChannels.TryGetValue(channelName, out channel))
            {
                channel = new ChatChannel(channelName);
                channel.IsPrivate = true;
                channel.MessageLimit = this.MessageLimit;
                this.PrivateChannels.Add(channel.Name, channel);
            }

            channel.Add(sender, message, msgId);
            this.listener.OnPrivateMessage(sender, message, channelName);
        }

        private void HandleChatMessagesEvent(EventData eventData)
        {
            object[] messages = (object[])eventData.Parameters[(byte)ChatParameterCode.Messages];
            string[] senders = (string[])eventData.Parameters[(byte)ChatParameterCode.Senders];
            string channelName = (string)eventData.Parameters[(byte)ChatParameterCode.Channel];
            int lastMsgId = (int)eventData.Parameters[ChatParameterCode.MsgId];

            ChatChannel channel;
            if (!this.PublicChannels.TryGetValue(channelName, out channel))
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "Channel " + channelName + " for incoming message event not found.");
                }
                return;
            }

            channel.Add(senders, messages, lastMsgId);
            this.listener.OnGetMessages(channelName, senders, messages);
        }

        private void HandleSubscribeEvent(EventData eventData)
        {
            string[] channelsInResponse = (string[])eventData.Parameters[ChatParameterCode.Channels];
            bool[] results = (bool[])eventData.Parameters[ChatParameterCode.SubscribeResults];
            for (int i = 0; i < channelsInResponse.Length; i++)
            {
                if (results[i])
                {
                    string channelName = channelsInResponse[i];
                    ChatChannel channel;
                    if (!this.PublicChannels.TryGetValue(channelName, out channel))
                    {
                        channel = new ChatChannel(channelName);
                        channel.MessageLimit = this.MessageLimit;
                        this.PublicChannels.Add(channel.Name, channel);
                    }
                    object temp;
                    if (eventData.Parameters.TryGetValue(ChatParameterCode.Properties, out temp))
                    {
                        Dictionary<object, object> channelProperties = temp as Dictionary<object, object>;
                        channel.ReadChannelProperties(channelProperties);
                    }
                    if (channel.PublishSubscribers) // or maybe remove check & always add anyway?
                    {
                        channel.AddSubscriber(this.UserId);
                    }
                    if (eventData.Parameters.TryGetValue(ChatParameterCode.ChannelSubscribers, out temp))
                    {
                        string[] subscribers = temp as string[];
                        channel.AddSubscribers(subscribers);
                    }
                    #if CHAT_EXTENDED
                    if (eventData.Parameters.TryGetValue(ChatParameterCode.UserProperties, out temp))
                    {
                        Dictionary<string, object> userProperties = temp as Dictionary<string, object>;
                        foreach (var pair in userProperties)
                        {
                            channel.ReadUserProperties(pair.Key, pair.Value as Dictionary<object, object>);
                        }
                    }
                    #endif
                }
            }

            this.listener.OnSubscribed(channelsInResponse, results);
        }


        private void HandleUnsubscribeEvent(EventData eventData)
        {
            string[] channelsInRequest = (string[])eventData[ChatParameterCode.Channels];
            for (int i = 0; i < channelsInRequest.Length; i++)
            {
                string channelName = channelsInRequest[i];
                this.PublicChannels.Remove(channelName);
                this.PublicChannelsUnsubscribing.Remove(channelName);
            }

            this.listener.OnUnsubscribed(channelsInRequest);
        }

        private void HandleAuthResponse(OperationResponse operationResponse)
        {
            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.listener.DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull() + " on: " + this.chatPeer.NameServerAddress);
            }

            if (operationResponse.ReturnCode == 0)
            {
                if (this.State == ChatState.ConnectedToNameServer)
                {
                    this.State = ChatState.Authenticated;
                    this.listener.OnChatStateChange(this.State);

                    if (operationResponse.Parameters.ContainsKey(ParameterCode.Secret))
                    {
                        if (this.AuthValues == null)
                        {
                            this.AuthValues = new AuthenticationValues();
                        }
                        this.AuthValues.Token = operationResponse[ParameterCode.Secret];

                        this.FrontendAddress = (string)operationResponse[ParameterCode.Address];
                        this.chatPeer.Disconnect();
                    }
                    else
                    {
                        if (this.DebugOut >= DebugLevel.ERROR)
                        {
                            this.listener.DebugReturn(DebugLevel.ERROR, "No secret in authentication response.");
                        }
                    }
                    if (operationResponse.Parameters.ContainsKey(ParameterCode.UserId))
                    {
                        string incomingId = operationResponse.Parameters[ParameterCode.UserId] as string;
                        if (!string.IsNullOrEmpty(incomingId))
                        {
                            this.UserId = incomingId;
                            this.listener.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", this.UserId));
                        }
                    }
                }
                else if (this.State == ChatState.ConnectingToFrontEnd)
                {
                    this.State = ChatState.ConnectedToFrontEnd;
                    this.listener.OnChatStateChange(this.State);
                    this.listener.OnConnected();
                    if (statusToSetWhenConnected.HasValue)
                    {
                        SetOnlineStatus(statusToSetWhenConnected.Value, messageToSetWhenConnected);
                        statusToSetWhenConnected = null;
                    }
                }
            }
            else
            {

                switch (operationResponse.ReturnCode)
                {
                    case ErrorCode.InvalidAuthentication:
                        this.DisconnectedCause = ChatDisconnectCause.InvalidAuthentication;
                        break;
                    case ErrorCode.CustomAuthenticationFailed:
                        this.DisconnectedCause = ChatDisconnectCause.CustomAuthenticationFailed;
                        break;
                    case ErrorCode.InvalidRegion:
                        this.DisconnectedCause = ChatDisconnectCause.InvalidRegion;
                        break;
                    case ErrorCode.MaxCcuReached:
                        this.DisconnectedCause = ChatDisconnectCause.MaxCcuReached;
                        break;
                    case ErrorCode.OperationNotAllowedInCurrentState:
                        this.DisconnectedCause = ChatDisconnectCause.OperationNotAllowedInCurrentState;
                        break;
                    case ErrorCode.AuthenticationTicketExpired:
                        this.DisconnectedCause = ChatDisconnectCause.AuthenticationTicketExpired;
                        break;
                }

                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, string.Format("{0} ClientState: {1} ServerAddress: {2}", operationResponse.ToStringFull(), this.State, this.chatPeer.ServerAddress));
                }


                this.Disconnect(this.DisconnectedCause);
            }
        }

        private void HandleStatusUpdate(EventData eventData)
        {
            string user = (string)eventData.Parameters[ChatParameterCode.Sender];
            int status = (int)eventData.Parameters[ChatParameterCode.Status];

            object message = null;
            bool gotMessage = eventData.Parameters.ContainsKey(ChatParameterCode.Message);
            if (gotMessage)
            {
                message = eventData.Parameters[ChatParameterCode.Message];
            }

            this.listener.OnStatusUpdate(user, status, gotMessage, message);
        }

        private bool ConnectToFrontEnd()
        {
            this.State = ChatState.ConnectingToFrontEnd;

            if (this.DebugOut >= DebugLevel.INFO)
            {
                this.listener.DebugReturn(DebugLevel.INFO, "Connecting to frontend " + this.FrontendAddress);
            }

            #if UNITY_WEBGL
            if (this.TransportProtocol == ConnectionProtocol.Tcp || this.TransportProtocol == ConnectionProtocol.Udp)
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "WebGL requires WebSockets. Switching TransportProtocol to WebSocketSecure.");
                this.TransportProtocol = ConnectionProtocol.WebSocketSecure;
            }
            #endif

            if (!this.chatPeer.Connect(this.FrontendAddress, this.ProxyServerAddress, ChatAppName, null))
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Connecting to frontend {0} failed.", this.FrontendAddress));
                }
                return false;
            }

            return true;
        }

        private bool AuthenticateOnFrontEnd()
        {
            if (this.AuthValues != null)
            {
                if (this.AuthValues.Token == null)
                {
                    if (this.DebugOut >= DebugLevel.ERROR)
                    {
                        this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Secret (AuthValues.Token) is not set");
                    }
                    return false;
                }
                else
                {
                    Dictionary<byte, object> opParameters = new Dictionary<byte, object> { { (byte)ChatParameterCode.Secret, this.AuthValues.Token } };
                    if (this.PrivateChatHistoryLength > -1)
                    {
                        opParameters[(byte)ChatParameterCode.HistoryLength] = this.PrivateChatHistoryLength;
                    }

                    return this.chatPeer.SendOperation(ChatOperationCode.Authenticate, opParameters, SendOptions.SendReliable);
                }
            }
            else
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Authentication Values are not set");
                }
                return false;
            }
        }

        private void HandleUserUnsubscribedEvent(EventData eventData)
        {
            string channelName = eventData.Parameters[ChatParameterCode.Channel] as string;
            string userId = eventData.Parameters[ChatParameterCode.UserId] as string;
            ChatChannel channel;
            if (this.PublicChannels.TryGetValue(channelName, out channel))
            {
                if (!channel.PublishSubscribers)
                {
                    if (this.DebugOut >= DebugLevel.WARNING)
                    {
                        this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" for incoming UserUnsubscribed (\"{1}\") event does not have PublishSubscribers enabled.", channelName, userId));
                    }
                }
                if (!channel.RemoveSubscriber(userId)) // user not found!
                {
                    if (this.DebugOut >= DebugLevel.WARNING)
                    {
                        this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" does not contain unsubscribed user \"{1}\".", channelName, userId));
                    }
                }
            }
            else
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" not found for incoming UserUnsubscribed (\"{1}\") event.", channelName, userId));
                }
            }
            this.listener.OnUserUnsubscribed(channelName, userId);
        }

        private void HandleUserSubscribedEvent(EventData eventData)
        {
            string channelName = eventData.Parameters[ChatParameterCode.Channel] as string;
            string userId = eventData.Parameters[ChatParameterCode.UserId] as string;
            ChatChannel channel;
            if (this.PublicChannels.TryGetValue(channelName, out channel))
            {
                if (!channel.PublishSubscribers)
                {
                    if (this.DebugOut >= DebugLevel.WARNING)
                    {
                        this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" for incoming UserSubscribed (\"{1}\") event does not have PublishSubscribers enabled.", channelName, userId));
                    }
                }
                if (!channel.AddSubscriber(userId)) // user came back from the dead ?
                {
                    if (this.DebugOut >= DebugLevel.WARNING)
                    {
                        this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" already contains newly subscribed user \"{1}\".", channelName, userId));
                    }
                }
                else if (channel.MaxSubscribers > 0 && channel.Subscribers.Count > channel.MaxSubscribers)
                {
                    if (this.DebugOut >= DebugLevel.WARNING)
                    {
                        this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\"'s MaxSubscribers exceeded. count={1} > MaxSubscribers={2}.", channelName, channel.Subscribers.Count, channel.MaxSubscribers));
                    }
                }
                #if CHAT_EXTENDED
                object temp;
                if (eventData.Parameters.TryGetValue(ChatParameterCode.UserProperties, out temp))
                {
                    Dictionary<object, object> userProperties = temp as Dictionary<object, object>;
                    channel.ReadUserProperties(userId, userProperties);
                }
                #endif
            }
            else
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel \"{0}\" not found for incoming UserSubscribed (\"{1}\") event.", channelName, userId));
                }
            }
            this.listener.OnUserSubscribed(channelName, userId);
        }

        #endregion
        public bool Subscribe(string channel, int lastMsgId = 0, int messagesFromHistory = -1, ChannelCreationOptions creationOptions = null)
        {
            if (creationOptions == null)
            {
                creationOptions = ChannelCreationOptions.Default;
            }
            int maxSubscribers = creationOptions.MaxSubscribers;
            bool publishSubscribers = creationOptions.PublishSubscribers;
            if (maxSubscribers < 0)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "Cannot set MaxSubscribers < 0.");
                }
                return false;
            }
            if (lastMsgId < 0)
            {
                if (this.DebugOut >= DebugLevel.ERROR)
                {
                    this.listener.DebugReturn(DebugLevel.ERROR, "lastMsgId cannot be < 0.");
                }
                return false;
            }
            if (messagesFromHistory < -1)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "messagesFromHistory < -1, setting it to -1");
                }
                messagesFromHistory = -1;
            }
            if (lastMsgId > 0 && messagesFromHistory == 0)
            {
                if (this.DebugOut >= DebugLevel.WARNING)
                {
                    this.listener.DebugReturn(DebugLevel.WARNING, "lastMsgId will be ignored because messagesFromHistory == 0");
                }
                lastMsgId = 0;
            }
            Dictionary<object, object> properties = null;
            if (publishSubscribers)
            {
                if (maxSubscribers > DefaultMaxSubscribers)
                {
                    if (this.DebugOut >= DebugLevel.ERROR)
                    {
                        this.listener.DebugReturn(DebugLevel.ERROR,
                            string.Format("Cannot set MaxSubscribers > {0} when PublishSubscribers == true.", DefaultMaxSubscribers));
                    }
                    return false;
                }
                properties = new Dictionary<object, object>();
                properties[ChannelWellKnownProperties.PublishSubscribers] = true;
            }
            if (maxSubscribers > 0)
            {
                if (properties == null)
                {
                    properties = new Dictionary<object, object>();
                }
                properties[ChannelWellKnownProperties.MaxSubscribers] = maxSubscribers;
            }
            #if CHAT_EXTENDED
            if (creationOptions.CustomProperties != null && creationOptions.CustomProperties.Count > 0)
            {
                foreach (var pair in creationOptions.CustomProperties)
                {
                    properties.Add(pair.Key, pair.Value);
                }
            }
            #endif
            Dictionary<byte, object> opParameters = new Dictionary<byte, object> { { ChatParameterCode.Channels, new[] { channel } } };
            if (messagesFromHistory != 0)
            {
                opParameters.Add(ChatParameterCode.HistoryLength, messagesFromHistory);
            }
            if (lastMsgId > 0)
            {
                opParameters.Add(ChatParameterCode.MsgIds, new[] { lastMsgId });
            }
            if (properties != null && properties.Count > 0)
            {
                opParameters.Add(ChatParameterCode.Properties, properties);
            }

            return this.chatPeer.SendOperation(ChatOperationCode.Subscribe, opParameters, SendOptions.SendReliable);
        }

        #if CHAT_EXTENDED

        internal bool SetChannelProperties(string channelName, Dictionary<object, object> channelProperties, Dictionary<object, object> expectedProperties = null, bool httpForward = false)
        {
            if (!this.CanChat)
            {
                this.listener.DebugReturn(DebugLevel.ERROR, "SetChannelProperties called while not connected to front end server.");
                return false;
            }

            if (string.IsNullOrEmpty(channelName) || channelProperties == null || channelProperties.Count == 0)
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "SetChannelProperties parameters must be non-null and not empty.");
                return false;
            }
            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                                                  {
                                                      { ChatParameterCode.Channel, channelName },
                                                      { ChatParameterCode.Properties, channelProperties },
                                                      { ChatParameterCode.Broadcast, true }
                                                  };
            if (httpForward)
            {
                parameters.Add(ChatParameterCode.WebFlags, HttpForwardWebFlag);
            }
            if (expectedProperties != null && expectedProperties.Count > 0)
            {
                parameters.Add(ChatParameterCode.ExpectedValues, expectedProperties);
            }
            return this.chatPeer.SendOperation(ChatOperationCode.SetProperties, parameters, SendOptions.SendReliable);
        }

        public bool SetCustomChannelProperties(string channelName, Dictionary<string, object> channelProperties, Dictionary<string, object> expectedProperties = null, bool httpForward = false)
        {
            if (channelProperties != null && channelProperties.Count > 0)
            {
                Dictionary<object, object> properties = new Dictionary<object, object>(channelProperties.Count);
                foreach (var pair in channelProperties)
                {
                    properties.Add(pair.Key, pair.Value);
                }
                Dictionary<object, object> expected = null;
                if (expectedProperties != null && expectedProperties.Count > 0)
                {
                    expected = new Dictionary<object, object>(expectedProperties.Count);
                    foreach (var pair in expectedProperties)
                    {
                        expected.Add(pair.Key, pair.Value);
                    }
                }
                return this.SetChannelProperties(channelName, properties, expected, httpForward);
            }
            return this.SetChannelProperties(channelName, null);
        }

        public bool SetCustomUserProperties(string channelName, string userId, Dictionary<string, object> userProperties, Dictionary<string, object> expectedProperties = null, bool httpForward = false)
        {
            if (userProperties != null && userProperties.Count > 0)
            {
                Dictionary<object, object> properties = new Dictionary<object, object>(userProperties.Count);
                foreach (var pair in userProperties)
                {
                    properties.Add(pair.Key, pair.Value);
                }
                Dictionary<object, object> expected = null;
                if (expectedProperties != null && expectedProperties.Count > 0)
                {
                    expected = new Dictionary<object, object>(expectedProperties.Count);
                    foreach (var pair in expectedProperties)
                    {
                        expected.Add(pair.Key, pair.Value);
                    }
                }
                return this.SetUserProperties(channelName, userId, properties, expected, httpForward);
            }
            return this.SetUserProperties(channelName, userId, null);
        }

        internal bool SetUserProperties(string channelName, string userId, Dictionary<object, object> channelProperties, Dictionary<object, object> expectedProperties = null, bool httpForward = false)
        {
            if (!this.CanChat)
            {
                this.listener.DebugReturn(DebugLevel.ERROR, "SetUserProperties called while not connected to front end server.");
                return false;
            }
            if (string.IsNullOrEmpty(channelName))
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "SetUserProperties \"channelName\" parameter must be non-null and not empty.");
                return false;
            }
            if (channelProperties == null || channelProperties.Count == 0)
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "SetUserProperties \"channelProperties\" parameter must be non-null and not empty.");
                return false;
            }
            if (string.IsNullOrEmpty(userId))
            {
                this.listener.DebugReturn(DebugLevel.WARNING, "SetUserProperties \"userId\" parameter must be non-null and not empty.");
                return false;
            }
            Dictionary<byte, object> parameters = new Dictionary<byte, object>
                                                  {
                                                      { ChatParameterCode.Channel, channelName },
                                                      { ChatParameterCode.Properties, channelProperties },
                                                      { ChatParameterCode.UserId, userId },
                                                      { ChatParameterCode.Broadcast, true }
                                                  };
            if (httpForward)
            {
                parameters.Add(ChatParameterCode.WebFlags, HttpForwardWebFlag);
            }
            if (expectedProperties != null && expectedProperties.Count > 0)
            {
                parameters.Add(ChatParameterCode.ExpectedValues, expectedProperties);
            }
            return this.chatPeer.SendOperation(ChatOperationCode.SetProperties, parameters, SendOptions.SendReliable);
        }

        private void HandlePropertiesChanged(EventData eventData)
        {
            string channelName = eventData.Parameters[ChatParameterCode.Channel] as string;
            ChatChannel channel;
            if (!this.PublicChannels.TryGetValue(channelName, out channel))
            {
                this.listener.DebugReturn(DebugLevel.WARNING, string.Format("Channel {0} for incoming ChannelPropertiesUpdated event not found.", channelName));
                return;
            }
            string senderId = eventData.Parameters[ChatParameterCode.Sender] as string;
            Dictionary<object, object> changedProperties = eventData.Parameters[ChatParameterCode.Properties] as Dictionary<object, object>;
            object temp;
            if (eventData.Parameters.TryGetValue(ChatParameterCode.UserId, out temp))
            {
                string targetUserId = temp as string;
                channel.ReadUserProperties(targetUserId, changedProperties);
                this.listener.OnUserPropertiesChanged(channelName, targetUserId, senderId, changedProperties);
            }
            else
            {
                channel.ReadChannelProperties(changedProperties);
                this.listener.OnChannelPropertiesChanged(channelName, senderId, changedProperties);
            }
        }

        private void HandleErrorInfoEvent(EventData eventData)
        {
            string channel = eventData.Parameters[ChatParameterCode.Channel] as string;
            string msg = eventData.Parameters[ChatParameterCode.DebugMessage] as string;
            object data = eventData.Parameters[ChatParameterCode.DebugData];
            this.listener.OnErrorInfo(channel, msg, data);
        }

        #endif
    }
}
