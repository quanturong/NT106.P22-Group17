using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [Header("Chat UI References")]
    public TMP_InputField messageInput;
    public Button sendButton;
    public TextMeshProUGUI chatDisplay;
    public ScrollRect scrollRect;

    [Header("Settings")]
    public int maxMessages = 30;

    private List<string> messages = new List<string>();

    void Start()
    {
        if (sendButton)
            sendButton.onClick.AddListener(SendMessage);

        if (messageInput)
            messageInput.onEndEdit.AddListener(OnInputSubmit);

        if (chatDisplay)
            chatDisplay.text = "";
    }

    void Update()
    {
        if (messageInput && messageInput.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    void OnInputSubmit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }

    public void SendMessage()
    {
        if (messageInput == null || string.IsNullOrEmpty(messageInput.text.Trim()))
            return;

        string message = messageInput.text.Trim();
        string playerName = PhotonNetwork.LocalPlayer.NickName ?? "Player";

        photonView.RPC("ReceiveChatMessage", RpcTarget.All, playerName, message);

        messageInput.text = "";
        messageInput.Select();
        messageInput.ActivateInputField();
    }

    [PunRPC]
    void ReceiveChatMessage(string playerName, string message)
    {
        string formattedMessage = $"<color=#C2572B><b>{playerName}</b></color>  <color=#89534D>{message}</color>";

        messages.Add(formattedMessage);

        if (messages.Count > maxMessages)
            messages.RemoveAt(0);

        UpdateChatDisplay();
    }

    void UpdateChatDisplay()
    {
        if (chatDisplay == null) return;

        chatDisplay.text = string.Join("\n", messages);

        StartCoroutine(ScrollToBottomAfterLayout());
    }

    IEnumerator ScrollToBottomAfterLayout()
    {
        yield return null;
        yield return null;

        if (scrollRect)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f; 
        }
    }

    public void AddSystemMessage(string message)
    {
        string systemMsg = $"<color=#888888><i>{message}</i></color>";
        messages.Add(systemMsg);

        if (messages.Count > maxMessages)
            messages.RemoveAt(0);

        UpdateChatDisplay();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        AddSystemMessage($"{newPlayer.NickName} joined the game");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        AddSystemMessage($"{otherPlayer.NickName} left the game");
    }

    public void OnChatOpened()
    {
        if (messageInput)
        {
            messageInput.Select();
            messageInput.ActivateInputField();
        }
    }
}
