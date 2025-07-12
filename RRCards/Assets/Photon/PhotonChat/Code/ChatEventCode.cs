
namespace Photon.Chat
{
    public class ChatEventCode
    {
        public const byte ChatMessages = 0;
        public const byte Users = 1;// List of users or List of changes for List of users
        public const byte PrivateMessage = 2;
        public const byte FriendsList = 3;
        public const byte StatusUpdate = 4;
        public const byte Subscribe = 5;
        public const byte Unsubscribe = 6;
        public const byte PropertiesChanged = 7;
        public const byte UserSubscribed = 8;
        public const byte UserUnsubscribed = 9;
        public const byte ErrorInfo = 10;
    }
}
