using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    [Header("Các prefab của bài (K, Q, J, A, Joker)")]
    public GameObject prefabK;
    public GameObject prefabQ;
    public GameObject prefabJ;
    public GameObject prefabA;
    public GameObject prefabJoker;

    [Header("Panel chứa bài của người chơi")]
    public Transform handPanel;
    [Header("Lá bài úp giữa bàn")]
    public GameObject middleCardBack;

    public int numberOfCards = 6;
    private List<CardData> currentHand = new();

    private static List<string> sharedDeck;

    private Dictionary<string, GameObject> cardPrefabMap;

    void Start()
    {
        Debug.Log("[HANDMANAGER] Start() called");

        cardPrefabMap = new Dictionary<string, GameObject>()
        {
            { "K", prefabK },
            { "Q", prefabQ },
            { "J", prefabJ },
            { "A", prefabA },
            { "Joker", prefabJoker }
        };
        ValidatePrefabs();
        CreateFreshDeck();
        DealInitialCards();
    }

    void ValidatePrefabs()
    {
        if (prefabK == null) Debug.LogError("[HANDMANAGER] prefabK is NULL!");
        if (prefabQ == null) Debug.LogError("[HANDMANAGER] prefabQ is NULL!");
        if (prefabJ == null) Debug.LogError("[HANDMANAGER] prefabJ is NULL!");
        if (prefabA == null) Debug.LogError("[HANDMANAGER] prefabA is NULL!");
        if (prefabJoker == null) Debug.LogError("[HANDMANAGER] prefabJoker is NULL!");
        if (handPanel == null) Debug.LogError("[HANDMANAGER] handPanel is NULL!");
    }

    void CreateFreshDeck()
    {
        List<string> freshDeck = new List<string>();
        freshDeck.AddRange(Repeat("K", 8));        freshDeck.AddRange(Repeat("Q", 8));
        freshDeck.AddRange(Repeat("J", 8));
        freshDeck.AddRange(Repeat("A", 8));
        freshDeck.AddRange(Repeat("Joker", 6));
        Shuffle(freshDeck);

        sharedDeck = freshDeck;

        Debug.Log($"[HANDMANAGER] Created fresh deck with {sharedDeck.Count} cards");
    }
    public static void ResetSharedDeck()
    {
        List<string> newDeck = new List<string>();
        newDeck.AddRange(Repeat("K", 4));
        newDeck.AddRange(Repeat("Q", 4));
        newDeck.AddRange(Repeat("J", 4));
        newDeck.AddRange(Repeat("A", 4));
        newDeck.AddRange(Repeat("Joker", 2));
        Shuffle(newDeck);
        sharedDeck = newDeck;

        Debug.Log($"[HANDMANAGER] Reset shared deck with {sharedDeck.Count} cards");
    }

    static List<string> Repeat(string val, int count)
    {
        List<string> list = new();
        for (int i = 0; i < count; i++) list.Add(val);
        return list;
    }

    static void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    void DealInitialCards()
    {
        if (handPanel == null)
        {
            Debug.LogError("[HANDMANAGER] handPanel is NULL! Cannot deal cards.");
            return;
        }

        Debug.Log($"[HANDMANAGER] handPanel name: {handPanel.name}, position: {handPanel.position}");
        if (sharedDeck == null || sharedDeck.Count < numberOfCards)
        {
            Debug.LogWarning($"[HANDMANAGER] Deck has only {(sharedDeck?.Count ?? 0)} cards, reinitializing...");
            CreateFreshDeck();
        }

        Debug.Log($"[HANDMANAGER] Starting to deal {numberOfCards} cards. Deck has {sharedDeck.Count} cards.");

        float delay = 0f;
        float smallSpacing = 80f;        float startX = -(numberOfCards - 1) * smallSpacing / 2f;

        int cardsDealt = 0;
        for (int i = 0; i < numberOfCards; i++)
        {
            if (sharedDeck.Count == 0)
            {
                Debug.LogError($"[HANDMANAGER] Deck is empty at card {i}! Only dealt {cardsDealt} cards.");
                break;
            }

            string cardName = sharedDeck[0];
            sharedDeck.RemoveAt(0);

            if (!cardPrefabMap.ContainsKey(cardName))
            {
                Debug.LogError($"[HANDMANAGER] No prefab found for card: {cardName}");
                continue;
            }

            GameObject prefab = cardPrefabMap[cardName];
            if (prefab == null)
            {
                Debug.LogError($"[HANDMANAGER] Prefab for {cardName} is NULL!");
                continue;
            }

            GameObject cardObj = Instantiate(prefab, handPanel, false);
            if (cardObj == null)
            {
                Debug.LogError($"[HANDMANAGER] Failed to instantiate card: {cardName}");
                continue;
            }
            Debug.Log($"[HANDMANAGER] Created card {cardName} at index {i}");

            RectTransform rt = cardObj.GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError($"[HANDMANAGER] Card {cardName} missing RectTransform!");
                Destroy(cardObj);
                continue;
            }
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;            float targetX = startX + i * smallSpacing;
            rt.anchoredPosition = new Vector2(targetX, 0f);
            Debug.Log($"[HANDMANAGER] Card {cardName} positioned at {rt.anchoredPosition}");

            cardObj.transform.SetAsLastSibling();
            cardObj.SetActive(true);
            if (delay > 0)
            {
                rt.localScale = Vector3.zero;
                rt.DOScale(Vector3.one, 0.3f)
                  .SetDelay(delay * 0.1f)                  .SetEase(Ease.OutBack);
            }
            CardData cardData = cardObj.GetComponent<CardData>();
            if (cardData == null)
            {
                cardData = cardObj.AddComponent<CardData>();
                cardData.cardName = cardName;
                Debug.Log($"[HANDMANAGER] Added CardData component to {cardName}");
            }

            if (cardData != null)
            {
                currentHand.Add(cardData);
                cardsDealt++;
                Debug.Log($"[HANDMANAGER] Successfully dealt card {cardsDealt}: {cardData.cardName}");
            }
        }

        Debug.Log($"[HANDMANAGER] Finished dealing {cardsDealt} cards. Current hand size: {currentHand.Count}. Deck remaining: {sharedDeck.Count}");
        Debug.Log($"[HANDMANAGER] handPanel children count: {handPanel.childCount}");
        for (int i = 0; i < handPanel.childCount; i++)
        {
            Transform child = handPanel.GetChild(i);
            Debug.Log($"[HANDMANAGER] Child {i}: {child.name} at {child.localPosition}");
        }
    }

    public List<CardData> GetCurrentHand() => currentHand;

    public void RemoveCard(CardData card)
    {
        if (currentHand.Contains(card))
            currentHand.Remove(card);
    }
    public List<CardData> GetCardsByType(string cardType)
    {
        List<CardData> result = new List<CardData>();
        foreach (var card in currentHand)
        {
            if (card.cardName == cardType)
                result.Add(card);
        }
        return result;
    }

    public int GetCardCountByType(string cardType)
    {
        int count = 0;
        foreach (var card in currentHand)
        {
            if (card.cardName == cardType)
                count++;
        }
        return count;
    }

    public void SetHand(List<CardData> newHand)
    {
        Debug.Log($"[HANDMANAGER] SetHand called with {newHand.Count} cards");
        Debug.Log("[HANDMANAGER] Available prefab keys:");
        foreach (var kvp in cardPrefabMap)
        {
            Debug.Log($"[HANDMANAGER] - Key: '{kvp.Key}', Prefab: {(kvp.Value != null ? kvp.Value.name : "NULL")}");
        }
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < handPanel.childCount; i++)
        {
            Transform child = handPanel.GetChild(i);
            if (child != null)
            {
                toDestroy.Add(child.gameObject);
            }
        }

        foreach (var obj in toDestroy)
        {
            DestroyImmediate(obj);
        }

        currentHand.Clear();
        float smallSpacing = 80f;        float startX = -(newHand.Count - 1) * smallSpacing / 2f;

        for (int i = 0; i < newHand.Count; i++)
        {
            var cardData = newHand[i];

            Debug.Log($"[HANDMANAGER] Processing card {i}: '{cardData.cardName}'");
            string normalizedCardName = NormalizeCardName(cardData.cardName);
            Debug.Log($"[HANDMANAGER] Normalized '{cardData.cardName}' to '{normalizedCardName}'");

            if (!cardPrefabMap.TryGetValue(normalizedCardName, out GameObject prefab))
            {
                Debug.LogError($"[HANDMANAGER] No prefab found for normalized card: '{normalizedCardName}' (original: '{cardData.cardName}')");
                continue;
            }

            if (prefab == null)
            {
                Debug.LogError($"[HANDMANAGER] Prefab for {normalizedCardName} is NULL!");
                continue;
            }

            GameObject cardObj = Instantiate(prefab, handPanel, false);
            if (cardObj == null)
            {
                Debug.LogError($"[HANDMANAGER] Failed to instantiate card: {normalizedCardName}");
                continue;
            }

            RectTransform rt = cardObj.GetComponent<RectTransform>();
            if (rt == null)
            {
                Debug.LogError($"[HANDMANAGER] Card missing RectTransform: {normalizedCardName}");
                Destroy(cardObj);
                continue;
            }
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;
            float targetX = startX + i * smallSpacing;
            rt.anchoredPosition = new Vector2(targetX, 0f);

            cardObj.SetActive(true);
            CardData data = cardObj.GetComponent<CardData>();
            if (data == null)
            {
                data = cardObj.AddComponent<CardData>();
            }
            data.cardName = normalizedCardName;
            currentHand.Add(data);
            Debug.Log($"[HANDMANAGER] SetHand - Successfully created card {i}: {data.cardName} at position {rt.anchoredPosition}");
        }

        Debug.Log($"[HANDMANAGER] SetHand completed with {currentHand.Count} cards");
    }
    string NormalizeCardName(string cardName)
    {
        if (string.IsNullOrEmpty(cardName)) return "";

        string normalized = cardName.Trim().ToUpper();
        switch (normalized)
        {
            case "KING":
            case "K":
                return "K";

            case "QUEEN":
            case "Q":
                return "Q";

            case "JACK":
            case "J":
                return "J";

            case "ACE":
            case "A":
                return "A";

            case "JOKER":
                return "Joker";
            default:
                Debug.LogWarning($"[HANDMANAGER] Unknown card name: '{cardName}', using as-is");
                return cardName;
        }
    }
    public void CreateNewHand()
    {
        Debug.Log("[HANDMANAGER] CreateNewHand() called - Creating new hand from scratch");

        if (handPanel == null)
        {
            Debug.LogError("[HANDMANAGER] handPanel is NULL! Cannot create new hand.");
            return;
        }
        CreateFreshDeck();
        Debug.Log($"[HANDMANAGER] Clearing current hand. Current count: {currentHand.Count}");
        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < handPanel.childCount; i++)
        {
            Transform child = handPanel.GetChild(i);
            if (child != null)
            {
                toDestroy.Add(child.gameObject);
            }
        }
        foreach (var obj in toDestroy)
        {
            DestroyImmediate(obj);
        }

        currentHand.Clear();

        Debug.Log("[HANDMANAGER] Cleared all children. Starting immediate card deal...");
        DealInitialCards();

        Debug.Log($"[HANDMANAGER] CreateNewHand completed. Final hand count: {currentHand.Count}");
    }

    [ContextMenu("Test Create New Hand")]
    public void TestCreateNewHand()
    {
        Debug.Log("[HANDMANAGER] MANUAL TEST - Creating new hand");
        CreateNewHand();
    }

    [ContextMenu("Debug Hand Panel")]
    public void DebugHandPanel()
    {
        if (handPanel == null)
        {
            Debug.LogError("[HANDMANAGER] handPanel is NULL!");
            return;
        }

        Debug.Log($"[HANDMANAGER] handPanel: {handPanel.name}");
        Debug.Log($"[HANDMANAGER] handPanel position: {handPanel.position}");
        Debug.Log($"[HANDMANAGER] handPanel local position: {handPanel.localPosition}");
        Debug.Log($"[HANDMANAGER] handPanel children: {handPanel.childCount}");
        Debug.Log($"[HANDMANAGER] currentHand count: {currentHand.Count}");

        for (int i = 0; i < handPanel.childCount; i++)
        {
            Transform child = handPanel.GetChild(i);
            Debug.Log($"[HANDMANAGER] Child {i}: {child.name} active: {child.gameObject.activeSelf}");
        }
    }

    [ContextMenu("Force Deal Cards")]
    public void ForceDealCards()
    {
        Debug.Log("[HANDMANAGER] FORCE DEAL - Starting");
        CreateFreshDeck();
        DealInitialCards();
    }
}