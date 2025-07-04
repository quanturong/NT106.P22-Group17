using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JSG.FortuneSpinWheel
{

    [CreateAssetMenu(fileName = "RewardData", menuName = "CustomObjects/RewardData", order = 1)]
    public class RewardData : ScriptableObject
    {
        public string m_Title = "coin";
        public string m_Type = "coin";
        public float m_Count = 10;
        public Sprite m_Icon;
    }
}