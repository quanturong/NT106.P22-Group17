
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class CurrentRoomNameProperty : PropertyListenerBase
    {
        public Text Text;

        string _cache = null;

        void Update()
        {

            if (PhotonNetwork.CurrentRoom != null)
            {
                if ((PhotonNetwork.CurrentRoom.Name != _cache))
                {
                    _cache = PhotonNetwork.CurrentRoom.Name;
                    Text.text = _cache.ToString();
                    this.OnValueChanged();
                }
            }
            else
            {
                if (_cache == null)
                {
                    _cache = null;
                    Text.text = "n/a";
                }
            }
        }
    }
}