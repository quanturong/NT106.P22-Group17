
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class PingProperty : PropertyListenerBase
    {
        public Text Text;

        int _cache = -1;

        void Update()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.GetPing() != _cache)
                {
                    _cache = PhotonNetwork.GetPing();
                    Text.text = _cache.ToString() + " ms";
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