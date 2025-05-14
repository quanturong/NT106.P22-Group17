using TMPro;
using UnityEngine;

public class OpponentCardCounter : MonoBehaviour
{
    [Header("Text hiển thị số lượng")]
    public TextMeshProUGUI countText;

    private int cardCount = 6;

    void Start()
    {
        SetCardCount(cardCount); 
    }

    public void SetCardCount(int count)
    {
        cardCount = Mathf.Max(0, count);
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (countText != null)
        {
            countText.text = cardCount.ToString();
        }
    }

    public void DecreaseCardCount()
    {
        SetCardCount(cardCount - 1);
    }
}
