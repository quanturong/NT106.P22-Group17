
using UnityEngine.UI;

using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
    public class CountOfPlayersProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.NetworkingClient.Server == ServerConnection.MasterServer)
            {
                if (PhotonNetwork.CountOfPlayers != _cache)
                {
                    _cache = PhotonNetwork.CountOfPlayers;
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