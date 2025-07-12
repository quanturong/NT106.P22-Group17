using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
public class LiarBarHandManager : MonoBehaviour
{
    [Header("Selection UI")]
    public TextMeshProUGUI selectionInfoText;
    public Button clearSelectionButton;
    public GameObject selectionPanel;

    [Header("Quick Select Buttons")]
    public Button selectAllKButton;
    public Button selectAllQButton;
    public Button selectAllJButton;
    public Button selectAllAButton;
    public Button selectAllJokerButton;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private HandManager baseHandManager;
    private List<LiarBarCardClick> selectedCards = new List<LiarBarCardClick>();
    private List<LiarBarCardClick> allCards = new List<LiarBarCardClick>();

    void Start()
    {
        baseHandManager = GetComponent<HandManager>();
        if (baseHandManager == null)
        {
            Debug.LogError("LiarBarHandManager requires HandManager component!");
            return;
        }

        SetupUI();
        Invoke(nameof(InitializeCards), 2f);
    }

    void SetupUI()
    {
        if (clearSelectionButton != null)
            clearSelectionButton.onClick.AddListener(ClearSelection);

        if (selectAllKButton != null)
            selectAllKButton.onClick.AddListener(() => SelectAllOfType("K"));
        if (selectAllQButton != null)
            selectAllQButton.onClick.AddListener(() => SelectAllOfType("Q"));
        if (selectAllJButton != null)
            selectAllJButton.onClick.AddListener(() => SelectAllOfType("J"));
        if (selectAllAButton != null)
            selectAllAButton.onClick.AddListener(() => SelectAllOfType("A"));
        if (selectAllJokerButton != null)
            selectAllJokerButton.onClick.AddListener(() => SelectAllOfType("Joker"));
    }

    void InitializeCards()
    {
        var cardObjects = baseHandManager.handPanel.GetComponentsInChildren<CardData>();
        foreach (var cardData in cardObjects)
        {
            var liarBarClick = cardData.GetComponent<LiarBarCardClick>();
            if (liarBarClick == null)
            {
                liarBarClick = cardData.gameObject.AddComponent<LiarBarCardClick>();
                liarBarClick.handManager = this;
            }
            allCards.Add(liarBarClick);
        }
        UpdateSelectionInfo();
        UpdateQuickSelectButtons();
    }
    public void RestoreHand(List<CardData> handData)
    {
        if (baseHandManager == null)
        {
            Debug.LogError("BaseHandManager is null, cannot restore hand!");
            return;
        }

        Debug.Log($"RestoreHand called with {handData.Count} cards");
        ClearAllCards();
        if (baseHandManager.handPanel != null)
        {
            baseHandManager.SetHand(handData);
            Invoke(nameof(ReinitializeCardsAfterRestore), 0.5f);
        }

        Debug.Log($"Hand restoration initiated with {handData.Count} cards");
    }

    void ReinitializeCardsAfterRestore()
    {
        allCards.Clear();
        selectedCards.Clear();

        var cardObjects = baseHandManager.handPanel.GetComponentsInChildren<CardData>();
        foreach (var cardData in cardObjects)
        {
            var liarBarClick = cardData.GetComponent<LiarBarCardClick>();
            if (liarBarClick == null)
            {
                liarBarClick = cardData.gameObject.AddComponent<LiarBarCardClick>();
                liarBarClick.handManager = this;
            }
            allCards.Add(liarBarClick);
        }

        UpdateSelectionInfo();
        UpdateQuickSelectButtons();

        Debug.Log($"Cards reinitialized after restore: {allCards.Count} cards available");
    }

    void ClearAllCards()
    {
        selectedCards.Clear();
        allCards.Clear();
        if (baseHandManager != null && baseHandManager.handPanel != null)
        {
            var existingCards = baseHandManager.handPanel.GetComponentsInChildren<CardData>();
            foreach (var card in existingCards)
            {
                if (card.gameObject != null)
                {
                    DestroyImmediate(card.gameObject);
                }
            }
        }
    }

    public void AddSelectedCard(LiarBarCardClick card)
    {
        if (!selectedCards.Contains(card))
        {
            selectedCards.Add(card);
            UpdateSelectionInfo();
        }
    }

    public void RemoveSelectedCard(LiarBarCardClick card)
    {
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            UpdateSelectionInfo();
        }
    }

    public void ClearSelection()
    {
        foreach (var card in selectedCards.ToList())
        {
            card.SetSelected(false);
        }
        selectedCards.Clear();
        UpdateSelectionInfo();
    }

    public void SelectAllOfType(string cardType)
    {
        ClearSelection();
        var cardsOfType = allCards.Where(c => c.GetCardData().cardName == cardType).ToList();
        foreach (var card in cardsOfType)
        {
            card.SetSelected(true);
        }
        UpdateSelectionInfo();
    }

    void UpdateSelectionInfo()
    {
        if (selectionInfoText != null)
        {
            if (selectedCards.Count == 0)
            {
                selectionInfoText.text = "No cards selected";
            }
            else
            {
                var cardCounts = selectedCards
                    .GroupBy(c => c.GetCardData().cardName)
                    .ToDictionary(g => g.Key, g => g.Count());

                string info = $"Selected: {selectedCards.Count} cards\n";
                foreach (var kvp in cardCounts)
                {
                    info += $"{kvp.Key}: {kvp.Value}  ";
                }
                selectionInfoText.text = info;
            }
        }
        if (selectionPanel != null)
            selectionPanel.SetActive(selectedCards.Count > 0);
    }

    void UpdateQuickSelectButtons()
    {
        var cardCounts = allCards
            .GroupBy(c => c.GetCardData().cardName)
            .ToDictionary(g => g.Key, g => g.Count());

        UpdateButtonText(selectAllKButton, "K", cardCounts.GetValueOrDefault("K", 0));
        UpdateButtonText(selectAllQButton, "Q", cardCounts.GetValueOrDefault("Q", 0));
        UpdateButtonText(selectAllJButton, "J", cardCounts.GetValueOrDefault("J", 0));
        UpdateButtonText(selectAllAButton, "A", cardCounts.GetValueOrDefault("A", 0));
        UpdateButtonText(selectAllJokerButton, "Joker", cardCounts.GetValueOrDefault("Joker", 0));
    }

    void UpdateButtonText(Button button, string cardType, int count)
    {
        if (button != null)
        {
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = $"{cardType} ({count})";
            }
            button.interactable = count > 0;
        }
    }

    public List<CardData> GetSelectedCardData()
    {
        return selectedCards.Select(c => c.GetCardData()).ToList();
    }

    public int GetSelectedCount()
    {
        return selectedCards.Count;
    }

    public int GetSelectedCardCount()
    {
        return selectedCards.Count;
    }

    public int GetSelectedCountOfType(string cardType)
    {
        return selectedCards.Count(c => c.GetCardData().cardName == cardType);
    }

    public bool HasSelectedCards()
    {
        return selectedCards.Count > 0;
    }

    public void PlaySelectedCards()
    {
        var cardsToPlay = selectedCards.ToList();
        foreach (var card in cardsToPlay)
        {
            card.PlayCard();
            allCards.Remove(card);
        }
        selectedCards.Clear();
        UpdateSelectionInfo();
        UpdateQuickSelectButtons();
    }

    public List<CardData> GetCurrentHand()
    {
        return baseHandManager.GetCurrentHand();
    }

    public void RemoveCard(CardData card)
    {
        if (enableDebugLogs)
            Debug.Log($"[LIARBARHANDMANAGER] RemoveCard called for: {card.cardName}");
        var cardToRemove = allCards.FirstOrDefault(c =>
            c.GetCardData() != null &&
            c.GetCardData().cardName == card.cardName &&
            c.gameObject == card.gameObject);

        if (cardToRemove != null)
        {
            allCards.Remove(cardToRemove);
            if (selectedCards.Contains(cardToRemove))
            {
                selectedCards.Remove(cardToRemove);
            }

            if (enableDebugLogs)
                Debug.Log($"[LIARBARHANDMANAGER] Removed {card.cardName} from allCards and selectedCards");
        }
        if (baseHandManager != null)
        {
            baseHandManager.RemoveCard(card);
        }
        UpdateSelectionInfo();
        UpdateQuickSelectButtons();

        if (enableDebugLogs)
            Debug.Log($"[LIARBARHANDMANAGER] RemoveCard completed. Remaining cards: {allCards.Count}");
    }

    public bool ValidateSelection(string claimedCardType, int claimedCount)
    {
        if (selectedCards.Count != claimedCount)
        {
            return false;
        }
        return true;
    }

    public string GetSelectionSummary()
    {
        if (selectedCards.Count == 0)
            return "No cards selected";

        var cardCounts = selectedCards
            .GroupBy(c => c.GetCardData().cardName)
            .ToDictionary(g => g.Key, g => g.Count());

        var summary = string.Join(", ", cardCounts.Select(kvp => $"{kvp.Value}x {kvp.Key}"));
        return summary;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearSelection();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectAllOfType("K");
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectAllOfType("Q");
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectAllOfType("J");
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectAllOfType("A");
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectAllOfType("Joker");
    }
}

public static class DictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
    {
        return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
    }
}