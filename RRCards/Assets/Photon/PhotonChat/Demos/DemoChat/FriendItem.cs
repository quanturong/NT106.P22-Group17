

using UnityEngine;
using UnityEngine.UI;


namespace Photon.Chat.Demo
{
    public class FriendItem : MonoBehaviour
    {
        [HideInInspector]
        public string FriendId
        {
            set { this.NameLabel.text = value; }
            get { return this.NameLabel.text; }
        }

        public Text NameLabel;
        public Text StatusLabel;
        public Text Health;

        public void Awake()
        {
            this.Health.text = string.Empty;
        }

        public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
        {
            string _status;

            switch (status)
            {
                case 1:
                    _status = "Invisible";
                    break;
                case 2:
                    _status = "Online";
                    break;
                case 3:
                    _status = "Away";
                    break;
                case 4:
                    _status = "Do not disturb";
                    break;
                case 5:
                    _status = "Looking For Game/Group";
                    break;
                case 6:
                    _status = "Playing";
                    break;
                default:
                    _status = "Offline";
                    break;
            }

            this.StatusLabel.text = _status;

            if (gotMessage)
            {
                string _health = string.Empty;
                if (message != null)
                {
                    string[] _messages = message as string[];
                    if (_messages != null && _messages.Length >= 2)
                    {
                        _health = (string)_messages[1] + "%";
                    }
                }

                this.Health.text = _health;
            }
        }
    }
}