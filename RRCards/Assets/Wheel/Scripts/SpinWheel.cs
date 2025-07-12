using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace JSG.FortuneSpinWheel
{
    public class FortuneSpinWheel : MonoBehaviour
    {
        public RewardData[] m_RewardData;
        public Image m_CircleBase;
        public Image[] m_RewardPictures;
        public Text[] m_RewardCounts;
        public GameObject m_RewardPanel;
        public Text m_RewardFinalText;
        public Image m_RewardFinalImage;
        public Image m_SpinButton;

        [Header("Visual Effects for Obtained Rewards")]
        public Color m_ObtainedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
        public Color m_NormalColor = Color.white; 

        [Header("Status Display")]
        public Text m_StatusText; 

        [HideInInspector]
        public bool m_IsSpinning = false;
        [HideInInspector]
        public float m_SpinSpeed = 0;
        [HideInInspector]
        public float m_Rotation = 0;
        [HideInInspector]
        public int m_RewardNumber = -1;

        private List<int> m_AvailableSlots = new List<int>();
        private int m_SpecialResetSlot = -1;
        private int m_TargetSlot = -1;
        private float m_TargetRotation = 0;

        void Start()
        {
            InitializeWheel();
            FindSpecialResetSlot();
            UpdateAvailableSlots();
            UpdateVisuals();
        }

        void InitializeWheel()
        {
            m_Rotation = 0;
            m_IsSpinning = false;
            m_RewardNumber = -1;
            m_TargetSlot = -1;

            for (int i = 0; i < m_RewardData.Length; i++)
            {
                m_RewardPictures[i].sprite = m_RewardData[i].m_Icon;
                m_RewardCounts[i].gameObject.SetActive(false);
            }
        }
        void FindSpecialResetSlot()
        {
            for (int i = 0; i < m_RewardData.Length; i++)
            {
                if (m_RewardData[i].m_IsSpecialReset)
                {
                    m_SpecialResetSlot = i;
                    Debug.Log($"Ô đặc biệt reset tại vị trí: {i}");
                    break;
                }
            }
        }

        void UpdateAvailableSlots()
        {
            m_AvailableSlots.Clear();
            if (m_SpecialResetSlot != -1)
            {
                m_AvailableSlots.Add(m_SpecialResetSlot);
            }

            for (int i = 0; i < m_RewardData.Length; i++)
            {
                if (!m_RewardData[i].m_IsObtained && !m_RewardData[i].m_IsSpecialReset)
                {
                    m_AvailableSlots.Add(i);
                }
            }

            Debug.Log($"Số ô có thể quay: {m_AvailableSlots.Count}");
        }

        void UpdateVisuals()
        {
            for (int i = 0; i < m_RewardData.Length; i++)
            {
                Color targetColor = (m_RewardData[i].m_IsObtained && !m_RewardData[i].m_IsSpecialReset)
                    ? m_ObtainedColor : m_NormalColor;

                if (m_RewardPictures[i] != null)
                {
                    m_RewardPictures[i].color = targetColor;
                }
            }

            UpdateStatusText();
        }

        void UpdateStatusText()
        {
            if (m_StatusText != null)
            {
                int obtainedCount = m_RewardData.Where(r => r.m_IsObtained && !r.m_IsSpecialReset).Count();
                int totalNormalRewards = m_RewardData.Where(r => !r.m_IsSpecialReset).Count();

                m_StatusText.text = $"Đã quay: {obtainedCount}/{totalNormalRewards}";

                if (obtainedCount == totalNormalRewards)
                {
                    m_StatusText.text += " - Chỉ còn ô Reset!";
                }
            }
        }

        void Update()
        {
            if (m_IsSpinning)
            {
                m_RewardPanel.gameObject.SetActive(false);
                float remainingRotation = m_TargetRotation - m_Rotation;
                if (remainingRotation > 360f)
                {
                    if (m_SpinSpeed > 3f)
                    {
                        m_SpinSpeed -= 2f * Time.deltaTime;
                    }
                    else
                    {
                        m_SpinSpeed = Mathf.Max(3f, m_SpinSpeed);
                    }
                }
                else if (remainingRotation > 180f) 
                {
                    float targetSpeed = 2f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 2f * Time.deltaTime);
                }
                else if (remainingRotation > 90f) 
                {
                    float targetSpeed = 1f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 3f * Time.deltaTime);
                }
                else if (remainingRotation > 30f)
                {
                    float targetSpeed = 0.5f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 4f * Time.deltaTime);
                }
                else
                {
                    float progress = 1f - (remainingRotation / 30f); 
                    float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
                    m_SpinSpeed = 0.5f * (1f - easedProgress);
                    m_SpinSpeed = Mathf.Max(0.1f, m_SpinSpeed);
                }
                float rotationDelta = 100 * Time.deltaTime * m_SpinSpeed;

                if (m_Rotation + rotationDelta >= m_TargetRotation)
                {
                    m_Rotation = m_TargetRotation; 
                    m_SpinSpeed = 0;
                    m_IsSpinning = false;
                    m_RewardNumber = m_TargetSlot;

                    StartCoroutine(ShowRewardMenu(1));
                    HandleReward();
                }
                else
                {
                    m_Rotation += rotationDelta;
                }

                m_CircleBase.transform.localRotation = Quaternion.Euler(0, 0, m_Rotation);

                for (int i = 0; i < 6; i++)
                {
                    m_RewardPictures[i].transform.rotation = Quaternion.identity;
                }
            }
            else
            {
                if (m_RewardNumber != -1)
                {
                    m_RewardPictures[m_RewardNumber].transform.localScale = (1 + 0.2f * Mathf.Sin(10 * Time.time)) * Vector3.one;
                }
            }
        }

        float CalculateTargetRotation(int targetSlot)
        {
            float baseAngle = targetSlot * 60f;

            int minFullRotations = Random.Range(3, 6);
            float currentFullRotations = Mathf.Floor(m_Rotation / 360f);
            float targetFullRotations = currentFullRotations + minFullRotations;
            float targetRotation = targetFullRotations * 360f + baseAngle;
            if (targetRotation <= m_Rotation)
            {
                targetRotation += 360f;
            }

            return targetRotation;
        }

        public void HandleReward()
        {
            if (m_RewardNumber < 0 || m_RewardNumber >= m_RewardData.Length)
                return;

            RewardData reward = m_RewardData[m_RewardNumber];

            if (reward.m_IsSpecialReset)
            {
                Debug.Log("-1");
                ResetAllObtainedRewards();
                return;
            }

            reward.m_IsObtained = true;

            switch (reward.m_Type)
            {
                case "coin":
                    if (reward.m_Count > 0)
                    {
                        Debug.Log("Gained " + reward.m_Count + " coins!");
                    }
                    else if (reward.m_Count < 0)
                    {
                        Debug.Log("Lost " + Mathf.Abs(reward.m_Count) + " coins!");

                    }
                    break;

                case "gem":
                    if (reward.m_Count > 0)
                    {
                        Debug.Log("Gained " + reward.m_Count + " gems!");
                    }
                    else if (reward.m_Count < 0)
                    {
                        Debug.Log("Lost " + Mathf.Abs(reward.m_Count) + " gems!");
                    }
                    break;
            }
            UpdateAvailableSlots();
            UpdateVisuals();
        }
        public void ResetAllObtainedRewards()
        {
            for (int i = 0; i < m_RewardData.Length; i++)
            {
                if (!m_RewardData[i].m_IsSpecialReset)
                {
                    m_RewardData[i].m_IsObtained = false;
                }
            }

            UpdateAvailableSlots();
            UpdateVisuals();

            Debug.Log("Đã reset tất cả phần thưởng!");
        }

        IEnumerator ShowRewardMenu(int seconds)
        {
            RewardData reward = m_RewardData[m_RewardNumber];
            yield return new WaitForSeconds(seconds);
            if (reward.m_Type != "nothing")
            {
                m_RewardPanel.gameObject.SetActive(true);

                if (reward.m_IsSpecialReset)
                {
                    m_RewardFinalText.text = "DEATH!";
                }
                else
                {
                    if (reward.m_Count > 0)
                    {
                        m_RewardFinalText.text = "+" + reward.m_Count.ToString();
                    }
                    else if (reward.m_Count < 0)
                    {
                        m_RewardFinalText.text = reward.m_Count.ToString();
                    }
                    else
                    {
                        m_RewardFinalText.text = " ";
                    }
                }

                m_RewardFinalImage.sprite = reward.m_Icon;
                yield return new WaitForSeconds(reward.m_IsSpecialReset ? 3 : 2);
            }
            yield return new WaitForSeconds(.1f);
            Reset();
        }

        public void StartSpin()
        {
            if (!m_IsSpinning && m_AvailableSlots.Count > 0)
            {
                m_TargetSlot = m_AvailableSlots[Random.Range(0, m_AvailableSlots.Count)];

                m_TargetRotation = CalculateTargetRotation(m_TargetSlot);

                Debug.Log($"Target slot: {m_TargetSlot}, Target rotation: {m_TargetRotation}");

                m_SpinSpeed = Random.Range(4f, 14f);
                m_IsSpinning = true;
                m_RewardNumber = -1;
                m_SpinButton.gameObject.SetActive(false);

                for (int i = 0; i < m_RewardPictures.Length; i++)
                {
                    m_RewardPictures[i].transform.localScale = Vector3.one;
                }
            }
            else if (m_AvailableSlots.Count == 0)
            {
                Debug.Log("Không có ô nào khả dụng để quay!");
            }
        }

        public void Reset()
        {
            m_CircleBase.transform.localRotation = Quaternion.identity;
            m_IsSpinning = false;
            m_RewardNumber = -1;
            m_TargetSlot = -1;
            m_Rotation = 0;
            m_SpinButton.gameObject.SetActive(true);
            m_RewardPanel.gameObject.SetActive(false);

            for (int i = 0; i < m_RewardPictures.Length; i++)
            {
                m_RewardPictures[i].transform.localScale = Vector3.one;
            }
        }

        public bool HasAvailableSlots()
        {
            return m_AvailableSlots.Count > 0;
        }

        public int GetAvailableSlotCount()
        {
            return m_AvailableSlots.Count;
        }

        public bool IsSpecialResetOnly()
        {
            return m_AvailableSlots.Count == 1 && m_AvailableSlots.Contains(m_SpecialResetSlot);
        }
    }
}