
using UnityEngine.UI;

using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
    public class CountOfPlayersOnMasterProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.NetworkingClient.Server == ServerConnection.MasterServer)
            {
                if (PhotonNetwork.CountOfPlayersOnMaster != _cache)
                {
                    _cache = PhotonNetwork.CountOfPlayersOnMaster;
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