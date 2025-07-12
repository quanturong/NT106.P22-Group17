
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class AppVersionProperty : MonoBehaviour
    {

        public Text Text;

        string _cache;

        void Update()
        {
            if (PhotonNetwork.AppVersion != _cache)
            {
                _cache = PhotonNetwork.AppVersion;
                Text.text = _cache;
            }
        }
    }
}