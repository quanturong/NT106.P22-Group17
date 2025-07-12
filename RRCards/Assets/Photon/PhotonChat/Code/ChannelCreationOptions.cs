
namespace Photon.Chat
{
    public class ChannelCreationOptions
    {
        public static ChannelCreationOptions Default = new ChannelCreationOptions();
        public bool PublishSubscribers { get; set; }
        public int MaxSubscribers { get; set; }

        #if CHAT_EXTENDED
        public System.Collections.Generic.Dictionary<string, object> CustomProperties { get; set; }
        #endif
    }
}
