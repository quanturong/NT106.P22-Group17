using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpponentCardCounter : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI countText;
    public Image cardIcon;    public GameObject cardCountPanel;
    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color lowCardsColor = Color.yellow;    public Color criticalColor = Color.red;
    [Header("Animation")]
    public bool enableAnimation = true;
    public float animationDuration = 0.3f;

    private int cardCount = 6;
    private int maxCards = 6;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetMaxCards(6);        SetCardCount(cardCount);

        if (cardCountPanel != null)
            cardCountPanel.SetActive(true);
    }
    public void SetMaxCards(int max)
    {
        maxCards = Mathf.Max(1, max);
        Debug.Log($"OpponentCardCounter: Set max cards to {maxCards}");
    }
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
    public void DecreaseCardCount(int amount = 1)
    {
        SetCardCount(cardCount - amount);
    }
    public void IncreaseCardCount(int amount = 1)
    {
        SetCardCount(cardCount + amount);
    }
    public void ResetToMaxCards()
    {
        SetCardCount(maxCards);
    }

    void UpdateDisplay()
    {
        if (countText != null)
        {
            countText.text = cardCount.ToString();
            Color textColor = GetColorForCardCount();
            countText.color = textColor;

            if (cardIcon != null)
                cardIcon.color = textColor;
        }
        Debug.Log($"OpponentCardCounter: Display updated - {cardCount}/{maxCards} cards");
    }

    Color GetColorForCardCount()
    {
        float percentage = (float)cardCount / maxCards;

        if (percentage <= 0.33f)            return criticalColor;
        else if (percentage <= 0.5f)            return lowCardsColor;
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
            StartCoroutine(SimpleScaleAnimation());
        }
    }

    System.Collections.IEnumerator SimpleScaleAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < animationDuration / 2)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animationDuration / 2);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
    }
    public int GetCardCount()
    {
        return cardCount;
    }
    public int GetMaxCards()
    {
        return maxCards;
    }
    public bool IsLowOnCards()
    {
        return (float)cardCount / maxCards <= 0.5f;
    }
    public bool IsCriticallyLowOnCards()
    {
        return (float)cardCount / maxCards <= 0.33f;
    }
    public bool IsOutOfCards()
    {
        return cardCount <= 0;
    }
    public void SetVisible(bool visible)
    {
        if (cardCountPanel != null)
            cardCountPanel.SetActive(visible);
        else
            gameObject.SetActive(visible);
    }
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