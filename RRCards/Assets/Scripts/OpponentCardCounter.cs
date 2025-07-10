using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpponentCardCounter : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI countText;
    public Image cardIcon; // Icon hiển thị thẻ bài
    public GameObject cardCountPanel; // Panel chứa toàn bộ UI

    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color lowCardsColor = Color.yellow; // Màu cảnh báo khi ít bài
    public Color criticalColor = Color.red; // Màu nguy hiểm khi rất ít bài

    [Header("Animation")]
    public bool enableAnimation = true;
    public float animationDuration = 0.3f;

    private int cardCount = 6;
    private int maxCards = 6;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetMaxCards(6); // Mặc định 6 lá
        SetCardCount(cardCount);

        if (cardCountPanel != null)
            cardCountPanel.SetActive(true);
    }

    /// <summary>
    /// Set số lá bài tối đa (dùng khi bắt đầu round mới)
    /// </summary>
    public void SetMaxCards(int max)
    {
        maxCards = Mathf.Max(1, max);
        Debug.Log($"OpponentCardCounter: Set max cards to {maxCards}");
    }

    /// <summary>
    /// Set số lá bài hiện tại của đối thủ
    /// </summary>
    public void SetCardCount(int count)
    {
        int oldCount = cardCount;
        cardCount = Mathf.Clamp(count, 0, maxCards);

        Debug.Log($"OpponentCardCounter: Updated from {oldCount} to {cardCount} cards");

        UpdateDisplay();

        if (enableAnimation && oldCount != cardCount)
        {
            PlayUpdateAnimation();
        }
    }

    /// <summary>
    /// Giảm số lá bài (khi đối thủ đánh bài)
    /// </summary>
    public void DecreaseCardCount(int amount = 1)
    {
        SetCardCount(cardCount - amount);
    }

    /// <summary>
    /// Tăng số lá bài (hiếm khi dùng)
    /// </summary>
    public void IncreaseCardCount(int amount = 1)
    {
        SetCardCount(cardCount + amount);
    }

    /// <summary>
    /// Reset về số lá bài ban đầu
    /// </summary>
    public void ResetToMaxCards()
    {
        SetCardCount(maxCards);
    }

    void UpdateDisplay()
    {
        if (countText != null)
        {
            countText.text = cardCount.ToString();

            // Thay đổi màu sắc dựa trên số lá bài còn lại
            Color textColor = GetColorForCardCount();
            countText.color = textColor;

            if (cardIcon != null)
                cardIcon.color = textColor;
        }

        // Log để debug
        Debug.Log($"OpponentCardCounter: Display updated - {cardCount}/{maxCards} cards");
    }

    Color GetColorForCardCount()
    {
        float percentage = (float)cardCount / maxCards;

        if (percentage <= 0.33f) // 33% hoặc ít hơn = nguy hiểm
            return criticalColor;
        else if (percentage <= 0.5f) // 50% hoặc ít hơn = cảnh báo
            return lowCardsColor;
        else
            return normalColor;
    }

    void PlayUpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("UpdateCard");
        }
        else if (enableAnimation)
        {
            // Simple scale animation nếu không có Animator
            StartCoroutine(SimpleScaleAnimation());
        }
    }

    System.Collections.IEnumerator SimpleScaleAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float elapsed = 0f;

        // Scale up
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }

        elapsed = 0f;

        // Scale down
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    /// <summary>
    /// Lấy số lá bài hiện tại
    /// </summary>
    public int GetCardCount()
    {
        return cardCount;
    }

    /// <summary>
    /// Lấy số lá bài tối đa
    /// </summary>
    public int GetMaxCards()
    {
        return maxCards;
    }

    /// <summary>
    /// Kiểm tra đối thủ có ít bài không
    /// </summary>
    public bool IsLowOnCards()
    {
        return (float)cardCount / maxCards <= 0.5f;
    }

    /// <summary>
    /// Kiểm tra đối thủ có rất ít bài không
    /// </summary>
    public bool IsCriticallyLowOnCards()
    {
        return (float)cardCount / maxCards <= 0.33f;
    }

    /// <summary>
    /// Kiểm tra đối thủ có hết bài không
    /// </summary>
    public bool IsOutOfCards()
    {
        return cardCount <= 0;
    }

    /// <summary>
    /// Hiển thị/ẩn counter
    /// </summary>
    public void SetVisible(bool visible)
    {
        if (cardCountPanel != null)
            cardCountPanel.SetActive(visible);
        else
            gameObject.SetActive(visible);
    }

    // Debug methods
    [ContextMenu("Test Decrease Card")]
    void TestDecreaseCard()
    {
        DecreaseCardCount();
    }

    [ContextMenu("Test Reset Cards")]
    void TestResetCards()
    {
        ResetToMaxCards();
    }

    [ContextMenu("Test Set Low Cards")]
    void TestSetLowCards()
    {
        SetCardCount(2);
    }
}