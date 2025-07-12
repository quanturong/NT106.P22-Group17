
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class SendRateOnSerializeField : MonoBehaviour
    {

        public InputField PropertyValueInput;

        int _cache;

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
            if (PhotonNetwork.SerializationRate != _cache)
            {
                _cache = PhotonNetwork.SerializationRate;
                PropertyValueInput.text = _cache.ToString();
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
            _cache = int.Parse(PropertyValueInput.text);
            PhotonNetwork.SerializationRate = _cache;
        }
    }
}