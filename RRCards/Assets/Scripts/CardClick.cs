using UnityEngine;
using UnityEngine.EventSystems;

public class CardClick : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Lá bài úp ở giữa bàn (GameObject có Image)")]
    public GameObject middleCardBack;

    [Tooltip("Tham chiếu tới HandManager")]
    public HandManager handManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);

        if (middleCardBack != null)
        {
            middleCardBack.SetActive(true);
        }

        if (handManager != null)
        {
            CardData data = GetComponent<CardData>();
            if (data != null)
            {
                handManager.RemoveCard(data);
            }
            else
            {
                Debug.LogWarning("CardData không tồn tại trên lá bài được click.");
            }
        }
        else
        {
            Debug.LogWarning("HandManager chưa được gán trong CardClick.");
        }
    }
}
