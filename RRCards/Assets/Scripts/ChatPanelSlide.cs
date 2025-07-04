using UnityEngine;
using UnityEngine.UI;

public class ChatPanelSlide : MonoBehaviour
{
    public RectTransform chatPanel;
    public Button chatButton;
    public float slideTime = 0.3f;

    [Header("Chat Integration")]
    public ChatManager chatManager;

    private bool isOpen = false;
    private Vector2 panelClosedPos;
    private Vector2 panelOpenPos;
    private Coroutine animCoroutine;

    void Start()
    {
        float panelWidth = chatPanel.rect.width;
        panelClosedPos = new Vector2(-panelWidth, chatPanel.anchoredPosition.y);
        panelOpenPos = new Vector2(0, chatPanel.anchoredPosition.y);
        chatPanel.anchoredPosition = panelClosedPos;

        if (chatButton != null)
            chatButton.onClick.AddListener(TogglePanel);
    }

    void Update()
    {
        // Open chat with Shift + Enter
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Return))
        {
            TogglePanel();
        }

        // Close chat with Escape
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            ClosePanel();
        }
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    public void OpenPanel()
    {
        if (isOpen) return;

        isOpen = true;
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(SlidePanel(panelOpenPos));

        if (chatManager)
            chatManager.OnChatOpened();
    }

    public void ClosePanel()
    {
        if (!isOpen) return;

        isOpen = false;
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(SlidePanel(panelClosedPos));
    }

    System.Collections.IEnumerator SlidePanel(Vector2 targetPos)
    {
        float elapsed = 0f;
        Vector2 startPos = chatPanel.anchoredPosition;

        while (elapsed < slideTime)
        {
            chatPanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / slideTime);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        chatPanel.anchoredPosition = targetPos;
        animCoroutine = null;
    }
}