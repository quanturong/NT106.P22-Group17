
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class CurrentRoomMaxPlayersProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                if (PhotonNetwork.CurrentRoom.MaxPlayers != _cache)
                {
                    _cache = PhotonNetwork.CurrentRoom.MaxPlayers;
                    Text.text = _cache.ToString();
                    this.OnValueChanged();
                }
            }
            else
            {
                if (_cache != -1)
                {
                    _cache = -1;
                    Text.text = "n/a";
                }
            }
        }
    }
}