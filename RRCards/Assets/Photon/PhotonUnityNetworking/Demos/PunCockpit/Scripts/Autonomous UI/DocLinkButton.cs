
using Photon.Pun.Demo.Shared;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Photon.Pun.Demo.Cockpit
{
    public class DocLinkButton : MonoBehaviour, IPointerClickHandler
    {
		public DocLinks.DocTypes Type = DocLinks.DocTypes.Doc;

        public string Reference = "getting-started/pun-intro";
		public void Start(){}
        public void OnPointerClick(PointerEventData pointerEventData)
        {
			Application.OpenURL(DocLinks.GetLink(Type,Reference));
        }
    }
}