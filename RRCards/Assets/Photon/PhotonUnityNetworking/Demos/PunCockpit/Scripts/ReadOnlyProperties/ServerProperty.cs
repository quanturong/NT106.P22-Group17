
using UnityEngine.UI;
using Photon.Realtime;

namespace Photon.Pun.Demo.Cockpit
{
	public class ServerProperty : PropertyListenerBase
    {

        public Text Text;

		ServerConnection _cache;


        void Update()
        {

			if (PhotonNetwork.Server != _cache)
            {
				_cache = PhotonNetwork.Server;
				Text.text = PhotonNetwork.Server.ToString();
                this.OnValueChanged();
            }
        }
    }
}