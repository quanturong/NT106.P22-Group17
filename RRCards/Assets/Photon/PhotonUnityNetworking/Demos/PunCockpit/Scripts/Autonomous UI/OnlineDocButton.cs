
using UnityEngine;
using UnityEngine.EventSystems;

namespace Photon.Pun.Demo.Cockpit
{
    public class OnlineDocButton : MonoBehaviour, IPointerClickHandler
    {
        public string Url = "https://doc.photonengine.com/en-us/pun/v2/getting-started/pun-intro";
        public void OnPointerClick(PointerEventData pointerEventData)
        {
            Application.OpenURL(Url);
        }

    }
}