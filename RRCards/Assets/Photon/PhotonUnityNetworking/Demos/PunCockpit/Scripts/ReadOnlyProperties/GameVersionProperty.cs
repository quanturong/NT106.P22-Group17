
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	public class GameVersionProperty : MonoBehaviour 
    {
		public Text Text;

		string _cache;

		void Update()
		{
			if (PhotonNetwork.GameVersion != _cache) {
				_cache = PhotonNetwork.GameVersion;
				Text.text = _cache;
			}
		}
	}
}