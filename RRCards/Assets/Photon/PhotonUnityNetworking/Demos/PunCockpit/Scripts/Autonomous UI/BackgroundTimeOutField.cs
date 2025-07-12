
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class BackgroundTimeOutField : MonoBehaviour
    {
        public InputField PropertyValueInput;

        float _cache;

        bool registered;

        void OnEnable()
        {
            if (!registered)
            {
                registered = true;
                PropertyValueInput.onEndEdit.AddListener(OnEndEdit);
            }
        }

        void OnDisable()
        {
            registered = false;
            PropertyValueInput.onEndEdit.RemoveListener(OnEndEdit);
        }

        void Update()
        {
            if (PhotonNetwork.KeepAliveInBackground != _cache)
            {
                _cache = PhotonNetwork.KeepAliveInBackground;
                PropertyValueInput.text = _cache.ToString("F1");
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
            _cache = float.Parse(value);
            PhotonNetwork.KeepAliveInBackground = _cache;
        }
    }
}