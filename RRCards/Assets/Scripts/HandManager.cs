using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Header("Các prefab của bài")]
    public GameObject[] cardPrefabs; // Gồm Jack, Queen, King, Joker

    [Header("Vị trí chứa bài trong UI")]
    public Transform handPanel;

    [Header("Thiết lập số lượng bài")]
    public int numberOfCards = 6;

    [Header("Lá bài úp giữa bàn")]
    public GameObject middleCardBack; // Gán sẵn GameObject Image ở giữa bàn

    private List<CardData> currentHand = new List<CardData>();

    void Start()
    {
        DealInitialCards();
    }

    void DealInitialCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            int randIndex = Random.Range(0, cardPrefabs.Length);
            GameObject cardObj = Instantiate(cardPrefabs[randIndex], handPanel);
            cardObj.SetActive(true);

            // Lưu vào danh sách
            CardData cardData = cardObj.GetComponent<CardData>();
            if (cardData != null)
                currentHand.Add(cardData);

            // Gán middleCard cho từng card
            CardClick click = cardObj.GetComponent<CardClick>();
            if (click != null)
            {
                click.middleCardBack = middleCardBack;
                click.handManager = this; // Cho phép xóa khỏi danh sách khi đánh
            }
        }
    }

    // Gọi khi một lá bài bị đánh
    public void RemoveCard(CardData card)
    {
        if (currentHand.Contains(card))
        {
            currentHand.Remove(card);
        }
    }

    public List<CardData> GetCurrentHand()
    {
        return currentHand;
    }
}
