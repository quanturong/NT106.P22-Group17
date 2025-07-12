
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    [RequireComponent(typeof(Toggle))]
	public class AutoSyncSceneToggle : MonoBehaviour
    {
        Toggle _toggle;

        bool registered;
        void OnEnable()
        {

            _toggle = GetComponent<Toggle>();

            if (!registered)
            {
                registered = true;
                _toggle.onValueChanged.AddListener(ToggleValue);
            }
        }

        void OnDisable()
        {
            if (_toggle != null)
            {
                registered = false;
                _toggle.onValueChanged.RemoveListener(ToggleValue);
            }
        }

        void Update()
        {

			if (PhotonNetwork.AutomaticallySyncScene != _toggle.isOn)
            {
				_toggle.isOn = PhotonNetwork.AutomaticallySyncScene;
            }
        }


        public void ToggleValue(bool value)
        {
			PhotonNetwork.AutomaticallySyncScene = value;
        }
    }
}