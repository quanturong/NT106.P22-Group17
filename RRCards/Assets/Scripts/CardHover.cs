using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialPos;
    private bool hovering;
    private bool initialized;

    public float hoverHeight = 30f;
    public float hoverSpeed = 8f;

    void Start()
    {
        StartCoroutine(DelayInit());
    }

    System.Collections.IEnumerator DelayInit()
    {
        yield return null;
        initialPos = transform.localPosition;
        initialized = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (initialized) hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (initialized) hovering = false;
    }

    void Update()
    {
        if (!initialized) return;

        Vector3 target = initialPos + (hovering ? Vector3.up * hoverHeight : Vector3.zero);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * hoverSpeed);
    }
}
