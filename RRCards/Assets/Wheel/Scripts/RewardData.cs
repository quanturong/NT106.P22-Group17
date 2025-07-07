using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JSG.FortuneSpinWheel
{
    [CreateAssetMenu(fileName = "RewardData", menuName = "CustomObjects/RewardData", order = 1)]
    public class RewardData : ScriptableObject
    {
        [Header("Basic Reward Info")]
        public string m_Title = "heart";
        public string m_Type = "heart";
        public int m_Count = 10;
        public Sprite m_Icon;

        [Header("Special Properties")]
        [Tooltip("Đánh dấu ô này là ô đặc biệt - khi quay trúng sẽ reset tất cả ô khác")]
        public bool m_IsSpecialReset = false;

        [Tooltip("Đánh dấu ô này đã được quay trúng - chỉ dành cho runtime, không set trong Inspector")]
        [HideInInspector]
        public bool m_IsObtained = false;
    }
}