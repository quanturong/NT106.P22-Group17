using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject[] flaskPrefabs; // Các prefab flask (UI Image)

    [SerializeField] private RectTransform[] leftSlots;
    [SerializeField] private RectTransform[] rightSlots;


    void Start()
    {
        if (leftSlots.Length == 0 || rightSlots.Length == 0 || flaskPrefabs.Length == 0)
        {
            Debug.LogError("Slots hoặc Prefabs chưa được gán hoặc tìm thấy.");
            return;
        }

        SpawnFlasksInFrame(leftSlots);
        SpawnFlasksInFrame(rightSlots);
    }


    void SpawnFlasksInFrame(RectTransform[] slots)
    {
        List<RectTransform> orderedSlots = new List<RectTransform>(slots);

        // Sort theo index thủ công
        orderedSlots.Sort((a, b) =>
        {
            int indexA = a.GetComponent<SlotIndex>().index;
            int indexB = b.GetComponent<SlotIndex>().index;
            return indexA.CompareTo(indexB);
        });

        int flaskCount = Random.Range(1, Mathf.Min(5, orderedSlots.Count + 1));

        for (int i = 0; i < flaskCount; i++)
        {
            RectTransform targetSlot = orderedSlots[i];

            int randomFlaskIndex = Random.Range(0, flaskPrefabs.Length);
            GameObject selectedFlask = flaskPrefabs[randomFlaskIndex];

            GameObject flaskInstance = Instantiate(selectedFlask, targetSlot);
            RectTransform flaskRect = flaskInstance.GetComponent<RectTransform>();
            flaskRect.anchoredPosition = Vector2.zero;
            flaskRect.localScale = Vector3.one;

            Animator anim = flaskInstance.GetComponent<Animator>();
            if (anim != null)
                anim.Play("FlaskPopIn");
        }
    }


}
