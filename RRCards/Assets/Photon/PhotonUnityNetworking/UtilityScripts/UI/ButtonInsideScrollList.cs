
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Pun.UtilityScripts
{
	public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		ScrollRect scrollRect;
		void Start () {
			scrollRect = GetComponentInParent<ScrollRect>();
		}

		#region IPointerDownHandler implementation
		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			if (scrollRect !=null)
			{
				scrollRect.StopMovement();
				scrollRect.enabled = false;
			}
		}
		#endregion

		#region IPointerUpHandler implementation

		void IPointerUpHandler.OnPointerUp (PointerEventData eventData)
		{
			if (scrollRect !=null && !scrollRect.enabled)
			{
				scrollRect.enabled = true;
			}
		}

		#endregion
	}
}