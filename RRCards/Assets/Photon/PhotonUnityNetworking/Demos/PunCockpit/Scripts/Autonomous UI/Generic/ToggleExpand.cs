
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class ToggleExpand : MonoBehaviour
    {
        public GameObject Content;

        public Toggle Toggle;

        bool _init;

        void OnEnable()
        {
            Content.SetActive(Toggle.isOn);

            if (!_init)
            {
                _init = true;
                Toggle.onValueChanged.AddListener(HandleToggleOnValudChanged);
            }

            HandleToggleOnValudChanged(Toggle.isOn);

        }


        void HandleToggleOnValudChanged(bool value)
        {
            Content.SetActive(value);
        }

    }
}