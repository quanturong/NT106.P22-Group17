
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    public class LayoutElementMatchSize : MonoBehaviour
    {

        public LayoutElement layoutElement;
        public RectTransform Target;


        public bool MatchHeight = true;
        public bool MatchWidth;


        void Update()
        {

            if (MatchHeight)
            {
                if (layoutElement.minHeight != Target.sizeDelta.y)
                {
                    layoutElement.minHeight = Target.sizeDelta.y;
                }
            }

        }
    }
}