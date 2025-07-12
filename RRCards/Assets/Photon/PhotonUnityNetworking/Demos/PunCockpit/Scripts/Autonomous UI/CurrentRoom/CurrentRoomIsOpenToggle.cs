
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
	[RequireComponent(typeof(Toggle))]
	public class CurrentRoomIsOpenToggle : MonoBehaviour, IPointerClickHandler
	{
		Toggle _toggle;
		void OnEnable()
		{
			_toggle = GetComponent<Toggle>();
		}

		void Update()
		{

			if (PhotonNetwork.CurrentRoom == null && _toggle.interactable)
			{
				_toggle.interactable = false;
				
			}
			else if (PhotonNetwork.CurrentRoom != null && !_toggle.interactable)
			{
				_toggle.interactable = true;
			}
			
			if (PhotonNetwork.CurrentRoom!=null && PhotonNetwork.CurrentRoom.IsOpen != _toggle.isOn)
			{
				Debug.Log("Update toggle : PhotonNetwork.CurrentRoom.IsOpen = " + PhotonNetwork.CurrentRoom.IsOpen, this);
				_toggle.isOn = PhotonNetwork.CurrentRoom.IsOpen;
			}
		}


		public void ToggleValue(bool value)
		{
			if (PhotonNetwork.CurrentRoom != null)
			{
				Debug.Log("PhotonNetwork.CurrentRoom.IsOpen = " + value, this);
				PhotonNetwork.CurrentRoom.IsOpen = value;
			}

			
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			ToggleValue(_toggle.isOn);
		}
	}
}