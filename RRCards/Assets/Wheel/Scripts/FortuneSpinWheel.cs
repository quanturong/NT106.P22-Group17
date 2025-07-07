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
        public Color m_ObtainedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Màu làm mờ cho ô đã quay
        public Color m_NormalColor = Color.white; // Màu bình thường

        [Header("Status Display")]
        public Text m_StatusText; // Text hiển thị trạng thái vòng quay

        [HideInInspector]
        public bool m_IsSpinning = false;
        [HideInInspector]
        public float m_SpinSpeed = 0;
        [HideInInspector]
        public float m_Rotation = 0;
        [HideInInspector]
        public int m_RewardNumber = -1;

        // Danh sách các ô có thể quay trúng
        private List<int> m_AvailableSlots = new List<int>();
        // Vị trí ô đặc biệt reset
        private int m_SpecialResetSlot = -1;
        // Ô đích được chọn trước khi quay
        private int m_TargetSlot = -1;
        // Góc đích cần đạt được
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

                // Ẩn tất cả text hiển thị số lượng
                m_RewardCounts[i].gameObject.SetActive(false);
            }
        }

        // Tìm ô đặc biệt reset
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

        // Cập nhật danh sách ô có thể quay trúng
        void UpdateAvailableSlots()
        {
            m_AvailableSlots.Clear();

            // Luôn thêm ô đặc biệt nếu có
            if (m_SpecialResetSlot != -1)
            {
                m_AvailableSlots.Add(m_SpecialResetSlot);
            }

            // Thêm các ô chưa được quay trúng (trừ ô đặc biệt)
            for (int i = 0; i < m_RewardData.Length; i++)
            {
                if (!m_RewardData[i].m_IsObtained && !m_RewardData[i].m_IsSpecialReset)
                {
                    m_AvailableSlots.Add(i);
                }
            }

            Debug.Log($"Số ô có thể quay: {m_AvailableSlots.Count}");
        }

        // Cập nhật hiệu ứng visual
        void UpdateVisuals()
        {
            for (int i = 0; i < m_RewardData.Length; i++)
            {
                // Làm mờ các ô đã quay trúng (trừ ô đặc biệt)
                Color targetColor = (m_RewardData[i].m_IsObtained && !m_RewardData[i].m_IsSpecialReset)
                    ? m_ObtainedColor : m_NormalColor;

                if (m_RewardPictures[i] != null)
                {
                    m_RewardPictures[i].color = targetColor;
                }
            }

            UpdateStatusText();
        }

        // Cập nhật text trạng thái
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

                // Tính khoảng cách còn lại đến đích
                float remainingRotation = m_TargetRotation - m_Rotation;

                // Điều chỉnh tốc độ dựa trên khoảng cách còn lại
                if (remainingRotation > 360f) // Còn nhiều hơn 1 vòng
                {
                    // Giai đoạn quay nhanh
                    if (m_SpinSpeed > 3f)
                    {
                        m_SpinSpeed -= 2f * Time.deltaTime; // Giảm tốc độ chậm hơn
                    }
                    else
                    {
                        m_SpinSpeed = Mathf.Max(3f, m_SpinSpeed); // Duy trì tốc độ tối thiểu
                    }
                }
                else if (remainingRotation > 180f) // Còn khoảng nửa vòng
                {
                    // Giai đoạn giảm tốc trung bình
                    float targetSpeed = 2f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 2f * Time.deltaTime);
                }
                else if (remainingRotation > 90f) // Còn khoảng 1/4 vòng
                {
                    // Giai đoạn giảm tốc mạnh
                    float targetSpeed = 1f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 3f * Time.deltaTime);
                }
                else if (remainingRotation > 30f) // Gần đến đích
                {
                    // Giai đoạn giảm tốc rất chậm
                    float targetSpeed = 0.5f;
                    m_SpinSpeed = Mathf.Lerp(m_SpinSpeed, targetSpeed, 4f * Time.deltaTime);
                }
                else // Rất gần đích
                {
                    // Sử dụng easing để dừng mượt mà
                    float progress = 1f - (remainingRotation / 30f); // 0 -> 1
                    float easedProgress = 1f - Mathf.Pow(1f - progress, 3f); // Ease out cubic
                    m_SpinSpeed = 0.5f * (1f - easedProgress);

                    // Đảm bảo tốc độ tối thiểu để không bị kẹt
                    m_SpinSpeed = Mathf.Max(0.1f, m_SpinSpeed);
                }

                // Tính toán và áp dụng rotation
                float rotationDelta = 100 * Time.deltaTime * m_SpinSpeed;

                // Kiểm tra xem có vượt quá góc đích không
                if (m_Rotation + rotationDelta >= m_TargetRotation)
                {
                    m_Rotation = m_TargetRotation; // Đặt chính xác góc đích
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

                // Giữ hình ảnh thẳng đứng
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

        // Tính toán góc đích dựa trên ô được chọn
        float CalculateTargetRotation(int targetSlot)
        {
            // Mỗi ô có góc 60 độ (360/6 = 60)
            // Ô 0 ở góc 0, ô 1 ở góc 60, v.v.
            float baseAngle = targetSlot * 60f;

            // Thêm ít nhất 3-5 vòng quay để tạo hiệu ứng dài hơn
            int minFullRotations = Random.Range(3, 6); // Ngẫu nhiên 3-5 vòng
            float currentFullRotations = Mathf.Floor(m_Rotation / 360f);
            float targetFullRotations = currentFullRotations + minFullRotations;

            // Đảm bảo góc đích lớn hơn góc hiện tại
            float targetRotation = targetFullRotations * 360f + baseAngle;

            // Nếu vẫn nhỏ hơn góc hiện tại, thêm thêm 1 vòng
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
                // Xử lý ô đặc biệt - Reset tất cả
                Debug.Log("Quay trúng ô đặc biệt! Reset tất cả phần thưởng.");
                ResetAllObtainedRewards();
                return;
            }

            // Đánh dấu ô này đã được quay trúng
            reward.m_IsObtained = true;

            switch (reward.m_Type)
            {
                case "coin":
                    // Xử lý cả trường hợp cộng và trừ coin
                    if (reward.m_Count > 0)
                    {
                        // Thêm coin vào inventory
                        Debug.Log("Gained " + reward.m_Count + " coins!");
                    }
                    else if (reward.m_Count < 0)
                    {
                        // Trừ coin từ inventory
                        Debug.Log("Lost " + Mathf.Abs(reward.m_Count) + " coins!");
                        // Kiểm tra xem player có đủ coin để trừ không
                        // if (PlayerInventory.coins >= Mathf.Abs(reward.m_Count))
                        // {
                        //     PlayerInventory.coins += reward.m_Count; // Cộng số âm = trừ
                        // }

                    }
                    break;

                case "gem":
                    // Xử lý cả trường hợp cộng và trừ gem
                    if (reward.m_Count > 0)
                    {
                        // Thêm gem vào inventory
                        Debug.Log("Gained " + reward.m_Count + " gems!");
                    }
                    else if (reward.m_Count < 0)
                    {
                        // Trừ gem từ inventory
                        Debug.Log("Lost " + Mathf.Abs(reward.m_Count) + " gems!");
                        // Kiểm tra xem player có đủ gem để trừ không
                        // if (PlayerInventory.gems >= Mathf.Abs(reward.m_Count))
                        // {
                        //     PlayerInventory.gems += reward.m_Count; // Cộng số âm = trừ
                        // }
                    }
                    break;
            }

            // Cập nhật danh sách ô khả dụng và visual sau khi nhận thưởng
            UpdateAvailableSlots();
            UpdateVisuals();
        }

        // Reset tất cả phần thưởng (trừ ô đặc biệt)
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

            // Hiển thị reward panel cho tất cả loại thưởng
            if (reward.m_Type != "nothing")
            {
                m_RewardPanel.gameObject.SetActive(true);

                if (reward.m_IsSpecialReset)
                {
                    // Hiển thị thông báo đặc biệt cho ô reset
                    m_RewardFinalText.text = "DEATH!";
                }
                else
                {
                    // Hiển thị số âm với định dạng phù hợp
                    if (reward.m_Count > 0)
                    {
                        m_RewardFinalText.text = "+" + reward.m_Count.ToString();
                    }
                    else if (reward.m_Count < 0)
                    {
                        m_RewardFinalText.text = reward.m_Count.ToString(); // Số âm tự có dấu -
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
                // Chọn ô đích ngẫu nhiên từ danh sách khả dụng
                m_TargetSlot = m_AvailableSlots[Random.Range(0, m_AvailableSlots.Count)];

                // Tính toán góc đích
                m_TargetRotation = CalculateTargetRotation(m_TargetSlot);

                Debug.Log($"Target slot: {m_TargetSlot}, Target rotation: {m_TargetRotation}");

                m_SpinSpeed = Random.Range(4f, 14f);
                m_IsSpinning = true;
                m_RewardNumber = -1;
                m_SpinButton.gameObject.SetActive(false);

                // Reset scale của tất cả các ô
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

            // Reset scale của tất cả các ô
            for (int i = 0; i < m_RewardPictures.Length; i++)
            {
                m_RewardPictures[i].transform.localScale = Vector3.one;
            }
        }

        // Phương thức tiện ích
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