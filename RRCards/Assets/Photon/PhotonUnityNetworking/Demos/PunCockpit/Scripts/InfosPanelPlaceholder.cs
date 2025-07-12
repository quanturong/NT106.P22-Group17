
using UnityEngine;

namespace Photon.Pun.Demo.Cockpit
{
    public class InfosPanelPlaceholder : MonoBehaviour
    {
        public PunCockpit Manager;
        void OnEnable()
        {
            Manager.RequestInfosPanel(this.gameObject);
        }
    }
}
