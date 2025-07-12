
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	public class IsConnectedProperty : PropertyListenerBase
    {

        public Text Text;

        int _cache = -1;

        void Update()
        {
			if ((PhotonNetwork.IsConnected && _cache != 1) || (!PhotonNetwork.IsConnected && _cache != 0))
            {
				_cache = PhotonNetwork.IsConnected ? 1 : 0;
				Text.text = PhotonNetwork.IsConnected ? "true" : "false";
                this.OnValueChanged();
            }
        }
    }
}