
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Photon.Pun.Demo.Cockpit.Forms
{
    public class UserIdUiForm : MonoBehaviour
    {
        public const string UserIdPlayerPref = "PunUserId";

        public InputField idInput;

        [System.Serializable]
        public class OnSubmitEvent : UnityEvent<string> { }

        public OnSubmitEvent OnSubmit;

        public void Start()
        {

            string prefsName = PlayerPrefs.GetString(UserIdUiForm.UserIdPlayerPref);
            if (!string.IsNullOrEmpty(prefsName))
            {
                this.idInput.text = prefsName;
            }
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
            PlayerPrefs.SetString(UserIdUiForm.UserIdPlayerPref, idInput.text);
            OnSubmit.Invoke(idInput.text);
        }
    }
}