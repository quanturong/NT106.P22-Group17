using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    public string cardName;     
    public Sprite cardSprite;  

    private void Awake()
    {
        Image img = GetComponent<Image>();
        if (img != null && cardSprite != null)
            img.sprite = cardSprite;
    }
}