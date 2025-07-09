using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class LiarBarCardClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Visual Settings")]
    public Color selectedColor = new Color(1f, 1f, 0.5f, 1f);
    public Color normalColor = Color.white;
    public float selectedScale = 1.1f;
    public float animationDuration = 0.2f;

    [Header("References")]
    public LiarBarHandManager handManager;
    public LiarBarGameManager gameManager;

    private Image cardImage;
    private CardData cardData;
    private bool isSelected = false;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        cardImage = GetComponent<Image>();
        cardData = GetComponent<CardData>();
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;

        // Tự động tìm HandManager và GameManager nếu chưa assign
        if (handManager == null)
            handManager = FindFirstObjectByType<LiarBarHandManager>();

        if (gameManager == null)
            gameManager = FindFirstObjectByType<LiarBarGameManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Chỉ cho phép click khi đến lượt mình và đang ở trạng thái PlayerPlaying
        if (gameManager != null &&
            (gameManager.currentState != LiarBarGameManager.GameState.PlayerPlaying ||
             !IsMyTurn()))
        {
            return;
        }

        ToggleSelection();
    }

    bool IsMyTurn()
    {
        if (gameManager == null) return false;

        var players = gameManager.players;
        if (gameManager.currentPlayerIndex >= 0 && gameManager.currentPlayerIndex < players.Count)
        {
            return players[gameManager.currentPlayerIndex].photonPlayer == Photon.Pun.PhotonNetwork.LocalPlayer;
        }

        return false;
    }

    public void ToggleSelection()
    {
        isSelected = !isSelected;
        UpdateVisual();

        // Thông báo cho hand manager về việc select/deselect
        if (handManager != null)
        {
            if (isSelected)
                handManager.AddSelectedCard(this);
            else
                handManager.RemoveSelectedCard(this);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (cardImage == null) return;

        // Animate color change
        cardImage.DOColor(isSelected ? selectedColor : normalColor, animationDuration);

        // Animate scale change
        Vector3 targetScale = isSelected ? originalScale * selectedScale : originalScale;
        transform.DOScale(targetScale, animationDuration).SetEase(Ease.OutBack);

        // Animate position change (slight upward movement when selected)
        Vector3 targetPosition = isSelected ?
            originalPosition + Vector3.up * 20f : originalPosition;
        transform.DOLocalMove(targetPosition, animationDuration).SetEase(Ease.OutBack);
    }

    public CardData GetCardData()
    {
        return cardData;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    // Method để play card (sử dụng khi submit claim)
    public void PlayCard()
    {
        if (handManager != null && cardData != null)
        {
            handManager.RemoveCard(cardData);
        }

        // Animation khi play card
        PlayCardAnimation();
    }

    void PlayCardAnimation()
    {
        // Animate card flying to middle
        transform.DOMove(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack)
            .OnComplete(() => {
                gameObject.SetActive(false);
            });
    }

    void OnDestroy()
    {
        // Cleanup DOTween animations
        transform.DOKill();
        if (cardImage != null)
            cardImage.DOKill();
    }
}