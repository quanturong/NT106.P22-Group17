
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class ServerAddressProperty : MonoBehaviour
    {
        public Text Text;

        string _cache;

        void Update()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.ServerAddress != _cache)
                {
                    _cache = PhotonNetwork.ServerAddress;
                    Text.text = _cache;
                }
            }
            else
            {
                if (_cache != "n/a")
                {
                    _cache = "n/a";
                    Text.text = _cache;
                }
            }
        }
    }
}