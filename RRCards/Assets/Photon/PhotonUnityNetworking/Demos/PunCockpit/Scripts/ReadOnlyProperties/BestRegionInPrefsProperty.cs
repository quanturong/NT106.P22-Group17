
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	public class BestRegionInPrefsProperty : PropertyListenerBase
    {
        public Text Text;

        string _cache;

        void Update()
        {
			if (PhotonNetwork.BestRegionSummaryInPreferences != _cache)
            {
				_cache = PhotonNetwork.BestRegionSummaryInPreferences;

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