
using UnityEngine.UI;

using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
    public class CountOfPlayersInRoomProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.NetworkingClient.Server == ServerConnection.MasterServer)
            {
                if (PhotonNetwork.CountOfPlayersInRooms != _cache)
                {
                    _cache = PhotonNetwork.CountOfPlayersInRooms;
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