

namespace Photon.Chat
{
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    public interface IChatClientListener
    {
        void DebugReturn(DebugLevel level, string message);
        void OnDisconnected();
        void OnConnected();
        void OnChatStateChange(ChatState state);
        void OnGetMessages(string channelName, string[] senders, object[] messages);
        void OnPrivateMessage(string sender, object message, string channelName);
        void OnSubscribed(string[] channels, bool[] results);
        void OnUnsubscribed(string[] channels);
        void OnStatusUpdate(string user, int status, bool gotMessage, object message);
        void OnUserSubscribed(string channel, string user);
        void OnUserUnsubscribed(string channel, string user);


        #if CHAT_EXTENDED
        void OnChannelPropertiesChanged(string channel, string senderUserId, Dictionary<object, object> properties);
        void OnUserPropertiesChanged(string channel, string targetUserId, string senderUserId, Dictionary<object, object> properties);
        void OnErrorInfo(string channel, string error, object data);
        
        #endif


        #if SDK_V4
        void OnReceiveBroadcastMessage(string channel, byte[] message);
        #endif

    }
}