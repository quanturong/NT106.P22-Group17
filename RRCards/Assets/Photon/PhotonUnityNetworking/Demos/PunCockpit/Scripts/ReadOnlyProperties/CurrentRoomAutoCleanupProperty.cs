
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class CurrentRoomAutoCleanupProperty : PropertyListenerBase
    {

        public Text Text;

        int _cache = -1;

        void Update()
        {

            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.AutoCleanUp)
            {
                if ((PhotonNetwork.CurrentRoom.AutoCleanUp && _cache != 1) || (!PhotonNetwork.CurrentRoom.AutoCleanUp && _cache != 0))
                {
                    _cache = PhotonNetwork.CurrentRoom.AutoCleanUp ? 1 : 0;
                    Text.text = PhotonNetwork.CurrentRoom.AutoCleanUp ? "true" : "false";
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