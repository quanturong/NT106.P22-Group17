
#if UNITY_4_7 || UNITY_5 || UNITY_5_3_OR_NEWER
#define SUPPORTED_UNITY
#endif

namespace Photon.Chat
{
    using System.Collections.Generic;
    using System.Text;

    #if SUPPORTED_UNITY || NETFX_CORE
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    using SupportClass = ExitGames.Client.Photon.SupportClass;
    #endif
    public class ChatChannel
    {
        public readonly string Name;
        public readonly List<string> Senders = new List<string>();
        public readonly List<object> Messages = new List<object>();
        public int MessageLimit;
        public int ChannelID;
        public bool IsPrivate { get; protected internal set; }
        public int MessageCount { get { return this.Messages.Count; } }
        public int LastMsgId { get; protected set; }

        private Dictionary<object, object> properties;
        public bool PublishSubscribers { get; protected set; }
        public int MaxSubscribers { get; protected set; }
        public readonly HashSet<string> Subscribers = new HashSet<string>();
        private Dictionary<string, Dictionary<object, object>> usersProperties;
        public ChatChannel(string name)
        {
            this.Name = name;
        }
        public void Add(string sender, object message, int msgId)
        {
            this.Senders.Add(sender);
            this.Messages.Add(message);
            this.LastMsgId = msgId;
            this.TruncateMessages();
        }
        public void Add(string[] senders, object[] messages, int lastMsgId)
        {
            this.Senders.AddRange(senders);
            this.Messages.AddRange(messages);
            this.LastMsgId = lastMsgId;
            this.TruncateMessages();
        }
        public void TruncateMessages()
        {
            if (this.MessageLimit <= 0 || this.Messages.Count <= this.MessageLimit)
            {
                return;
            }

            int excessCount = this.Messages.Count - this.MessageLimit;
            this.Senders.RemoveRange(0, excessCount);
            this.Messages.RemoveRange(0, excessCount);
        }
        public void ClearMessages()
        {
            this.Senders.Clear();
            this.Messages.Clear();
        }
        public string ToStringMessages()
        {
            StringBuilder txt = new StringBuilder();
            for (int i = 0; i < this.Messages.Count; i++)
            {
                txt.AppendLine(string.Format("{0}: {1}", this.Senders[i], this.Messages[i]));
            }
            return txt.ToString();
        }

        internal void ReadChannelProperties(Dictionary<object, object> newProperties)
        {
            if (newProperties != null && newProperties.Count > 0)
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<object, object>(newProperties.Count);
                }
                foreach (var pair in newProperties)
                {
                    if (pair.Value == null)
                    {
                        this.properties.Remove(pair.Key);
                    }
                    else
                    {
                        this.properties[pair.Key] = pair.Value;
                    }
                }
                object temp;
                if (this.properties.TryGetValue(ChannelWellKnownProperties.PublishSubscribers, out temp))
                {
                    this.PublishSubscribers = (bool)temp;
                }
                if (this.properties.TryGetValue(ChannelWellKnownProperties.MaxSubscribers, out temp))
                {
                    this.MaxSubscribers = (int)temp;
                }
            }
        }

        internal bool AddSubscribers(string[] users)
        {
            if (users == null)
            {
                return false;
            }
            bool result = true;
            for (int i = 0; i < users.Length; i++)
            {
                if (!this.Subscribers.Add(users[i]))
                {
                    result = false;
                }
            }
            return result;
        }

        internal bool AddSubscriber(string userId)
        {
            return this.Subscribers.Add(userId);
        }

        internal bool RemoveSubscriber(string userId)
        {
            if (this.usersProperties != null)
            {
                this.usersProperties.Remove(userId);
            }
            return this.Subscribers.Remove(userId);
        }


        #if CHAT_EXTENDED
        internal void ReadUserProperties(string userId, Dictionary<object, object> changedProperties)
        {
            if (this.usersProperties == null)
            {
                this.usersProperties = new Dictionary<string, Dictionary<object, object>>();
            }
            Dictionary<object, object> userProperties;
            if (!this.usersProperties.TryGetValue(userId, out userProperties))
            {
                userProperties = new Dictionary<object, object>();
                this.usersProperties.Add(userId, userProperties);
            }
            foreach (var property in changedProperties)
            {
                if (property.Value == null)
                {
                    userProperties.Remove(property.Key);
                }
                else
                {
                    userProperties[property.Key] = property.Value;
                }
            }
        }

        internal bool TryGetChannelProperty<T>(object propertyKey, out T propertyValue)
        {
            propertyValue = default;
            object temp;
            if (properties != null && properties.TryGetValue(propertyKey, out temp) && temp is T)
            {
                propertyValue = (T)temp;
                return true;
            }
            return false;
        }

        internal bool TryGetUserProperty<T>(string userId, object propertyKey, out T propertyValue)
        {
            propertyValue = default;
            object temp;
            Dictionary<object, object> userProperties;
            if (this.usersProperties != null && usersProperties.TryGetValue(userId, out userProperties) && userProperties.TryGetValue(propertyKey, out temp) && temp is T)
            {
                propertyValue = (T)temp;
                return true;
            }
            return false;
        }

        public bool TryGetCustomChannelProperty<T>(string propertyKey, out T propertyValue)
        {
            return this.TryGetChannelProperty(propertyKey, out propertyValue);
        }

        public bool TryGetCustomUserProperty<T>(string userId, string propertyKey, out T propertyValue)
        {
            return this.TryGetUserProperty(userId, propertyKey, out propertyValue);
        }
        #endif
    }
}