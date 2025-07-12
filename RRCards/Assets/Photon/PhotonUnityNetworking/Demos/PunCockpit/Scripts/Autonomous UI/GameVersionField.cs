 
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class GameVersionField : MonoBehaviour
    {
        public InputField PropertyValueInput;

        private string _cache;

        private bool registered;

        private void OnEnable()
        {
            if (!this.registered)
            {
                this.registered = true;
                this.PropertyValueInput.onEndEdit.AddListener(this.OnEndEdit);
            }
        }

        private void OnDisable()
        {
            this.registered = false;
            this.PropertyValueInput.onEndEdit.RemoveListener(this.OnEndEdit);
        }

        private void Update()
        {
			if (PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion != this._cache)
            {
				this._cache = PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
                this.PropertyValueInput.text = this._cache;
            }
        }
        public void OnEndEdit(string value)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Tab))
            {
                this.SubmitForm(value.Trim());
            }
            else
            {
                this.SubmitForm(value);
            }
        }

        public void SubmitForm(string value)
        {
            this._cache = value;
			PunCockpit.Instance.GameVersionOverride = this._cache;
        }
    }
}