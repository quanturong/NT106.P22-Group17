
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Photon.Pun.Demo.Cockpit.Forms
{
	public class LoadLevelUIForm : MonoBehaviour
    {
        public InputField PropertyValueInput;

        [System.Serializable]
        public class OnSubmitEvent : UnityEvent<string> { }

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

        public void SubmitForm()
        {
            OnSubmit.Invoke(PropertyValueInput.text);
        }
    }
}