
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Photon.Pun.Demo.Cockpit.Forms
{
	public class ConnectToRegionUIForm : MonoBehaviour
    {
		public InputField RegionInput;
		public Dropdown RegionListInput;

		[System.Serializable]
		public class OnSubmitEvent : UnityEvent<string>{}

		public OnSubmitEvent OnSubmit;

		public void Start()
		{
			
		}
		public void EndEditOnEnter()
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				this.SubmitForm();
			}
		}

		public void SetRegionFromDropDown(int index)
		{
			if (index == 0) {
				return;
			}

			RegionInput.text =	RegionListInput.options[index].text;
			RegionListInput.value = 0;

		}

		public void SubmitForm()
		{
			OnSubmit.Invoke (RegionInput.text);
		}
	}
}