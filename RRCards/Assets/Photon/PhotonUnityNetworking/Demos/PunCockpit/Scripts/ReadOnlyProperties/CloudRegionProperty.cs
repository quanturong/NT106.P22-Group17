
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	public class CloudRegionProperty : PropertyListenerBase
    {
        public Text Text;

        string _cache;

        void Update()
        {
            if (PhotonNetwork.CloudRegion != _cache)
            {
                _cache = PhotonNetwork.CloudRegion;
				this.OnValueChanged();
                if (string.IsNullOrEmpty(_cache))
                {
                    Text.text = "n/a";
                }
                else
                {
                    Text.text = _cache;
                }
            }
        }
    }
}