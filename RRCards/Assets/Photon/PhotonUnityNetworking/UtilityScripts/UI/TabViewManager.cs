
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Photon.Pun.UtilityScripts
{
    public class TabViewManager : MonoBehaviour
    {
        [System.Serializable]
        public class TabChangeEvent : UnityEvent<string> { }

        [Serializable]
        public class Tab
        {
            public string ID = "";
            public Toggle Toggle;
            public RectTransform View;
        }
        public ToggleGroup ToggleGroup;
        public Tab[] Tabs;
        public TabChangeEvent OnTabChanged;

        protected Tab CurrentTab;

        Dictionary<Toggle, Tab> Tab_lut;

        void Start()
        {

            Tab_lut = new Dictionary<Toggle, Tab>();

            foreach (Tab _tab in this.Tabs)
            {

                Tab_lut[_tab.Toggle] = _tab;

                _tab.View.gameObject.SetActive(_tab.Toggle.isOn);

                if (_tab.Toggle.isOn)
                {
                    CurrentTab = _tab;
                }
                _tab.Toggle.onValueChanged.AddListener((isSelected) =>
                {
                    if (!isSelected)
                    {
                        return;
                    }
                    OnTabSelected(_tab);
                });
            }


        }
        public void SelectTab(string id)
        {
            foreach (Tab _t in Tabs)
            {
                if (_t.ID == id)
                {
                    _t.Toggle.isOn = true;
                    return;
                }
            }
        }
        void OnTabSelected(Tab tab)
        {
            CurrentTab.View.gameObject.SetActive(false);

            CurrentTab = Tab_lut[ToggleGroup.ActiveToggles().FirstOrDefault()];

            CurrentTab.View.gameObject.SetActive(true);

            OnTabChanged.Invoke(CurrentTab.ID);

        }
    }
}