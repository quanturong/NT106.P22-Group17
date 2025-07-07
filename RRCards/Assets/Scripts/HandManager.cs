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
    public Transform handPanel; // panel của người chơi này

    [Header("Lá bài úp giữa bàn")]
    public GameObject middleCardBack;

    public int numberOfCards = 6;
    private List<CardData> currentHand = new();

    private static List<string> sharedDeck;

    private Dictionary<string, GameObject> cardPrefabMap;

    void Start()
    {
        if (sharedDeck == null || sharedDeck.Count < 12)
            InitDeck();

        cardPrefabMap = new Dictionary<string, GameObject>()
        {
            { "K", prefabK },
            { "Q", prefabQ },
            { "J", prefabJ },
            { "A", prefabA },
            { "Joker", prefabJoker }
        };

        DealInitialCards();
    }

    void InitDeck()
    {
        sharedDeck = new List<string>();
        sharedDeck.AddRange(Repeat("K", 4));
        sharedDeck.AddRange(Repeat("Q", 4));
        sharedDeck.AddRange(Repeat("J", 4));
        sharedDeck.AddRange(Repeat("A", 4));
        sharedDeck.AddRange(Repeat("Joker", 2));
        Shuffle(sharedDeck);
    }

    List<string> Repeat(string val, int count)
    {
        List<string> list = new();
        for (int i = 0; i < count; i++) list.Add(val);
        return list;
    }

    void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    void DealInitialCards()
    {
        float delay = 0f;
        float smallSpacing = 30f;  // Khoảng cách nhỏ giữa 5 lá đầu


        // Tính tổng chiều rộng của 5 lá đầu
        float firstFiveWidth = smallSpacing * 4; // 4 khoảng cách giữa 5 lá

        // Tính tổng chiều rộng của cả 6 lá
        float totalWidth = firstFiveWidth;

        // Tính vị trí bắt đầu để canh giữa
        float startX = -totalWidth / 6f;

        for (int i = 0; i < numberOfCards; i++)
        {
            string cardName = sharedDeck[0];
            sharedDeck.RemoveAt(0);

            GameObject prefab = cardPrefabMap[cardName];
            GameObject cardObj = Instantiate(prefab, handPanel, false);

            RectTransform rt = cardObj.GetComponent<RectTransform>();

            // Đồng bộ anchor và pivot giữa tất cả lá bài
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localRotation = Quaternion.identity;

            // Gán vị trí ban đầu từ giữa bàn (middleCardBack)


            rt.anchoredPosition = Vector2.zero;

            rt.localScale = Vector3.zero;

            cardObj.transform.SetAsLastSibling();
            cardObj.SetActive(true);

            // Tính vị trí đích
            float targetX = startX + i * smallSpacing;


            Vector2 targetPos = new Vector2(targetX, 0f);

            // Tween hiệu ứng chia bài - đồng bộ hóa animation
            rt.DOAnchorPos(targetPos, 1f).SetDelay(delay).SetEase(Ease.OutBack);
            rt.DOScale(Vector3.one, 1f).SetDelay(delay); // Cùng duration với position
            delay += 0.2f; // Tăng delay để rõ ràng hơn

            if (cardObj.TryGetComponent(out CardData cardData))
                currentHand.Add(cardData);

            if (cardObj.TryGetComponent(out CardClick click))
            {
                click.middleCardBack = middleCardBack;
                click.handManager = this;
            }
        }
    }

    public List<CardData> GetCurrentHand() => currentHand;

    public void RemoveCard(CardData card)
    {
        if (currentHand.Contains(card))
            currentHand.Remove(card);
    }
}