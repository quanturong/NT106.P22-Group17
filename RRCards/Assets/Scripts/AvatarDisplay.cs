using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonVersusRoomManager : MonoBehaviourPunCallbacks
{
    [Header("Avatar UI")]
    [SerializeField] private Image leftAvatarSlot;
    [SerializeField] private Image rightAvatarSlot;

    [Header("Avatar Resources")]
    [SerializeField] private Sprite[] availableAvatars;

    [Header("UI Elements")]
    [SerializeField] private Image vsImage;
    [SerializeField] private GameObject readyButton;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float battleStartDelay = 3.0f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private const string AVATAR_INDEX_KEY = "AvatarIndex";
    private const string PLAYER_READY_KEY = "IsReady";
    private int localPlayerAvatarIndex = 0;

    void Start()
    {
        CheckReferences();
        InitializeUI();
        SetLocalPlayerAvatar(0);
        ShowDefaultAvatars();

        if (showDebugLogs)
            Debug.Log("PhotonVersusRoomManager started. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    private void CheckReferences()
    {
        if (leftAvatarSlot == null) Debug.LogError("leftAvatarSlot is not assigned!");
        if (rightAvatarSlot == null) Debug.LogError("rightAvatarSlot is not assigned!");
        if (vsImage == null) Debug.LogError("vsImage is not assigned!");
        if (readyButton == null) Debug.LogError("readyButton is not assigned!");
        if (availableAvatars == null || availableAvatars.Length == 0) Debug.LogError("availableAvatars not assigned!");
    }

    private void InitializeUI()
    {
        if (vsImage != null)
        {
            vsImage.gameObject.SetActive(true);
            StartCoroutine(AnimateVSImage());
        }

        if (readyButton != null)
            readyButton.SetActive(true);
    }

    private void ShowDefaultAvatars()
    {
        if (leftAvatarSlot == null || rightAvatarSlot == null || availableAvatars.Length < 2)
        {
            Debug.LogError("Missing references for default avatars");
            return;
        }

        leftAvatarSlot.sprite = availableAvatars[0];
        leftAvatarSlot.color = Color.white;
        leftAvatarSlot.gameObject.SetActive(true);

        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            rightAvatarSlot.sprite = availableAvatars[1];
            rightAvatarSlot.color = Color.white;
            rightAvatarSlot.gameObject.SetActive(true);
        }
        else
        {
            rightAvatarSlot.sprite = availableAvatars[1];
            Color transparent = rightAvatarSlot.color;
            transparent.a = 0.5f;
            rightAvatarSlot.color = transparent;
            rightAvatarSlot.gameObject.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        UpdatePlayerAvatars();
        if (readyButton != null)
            readyButton.SetActive(true);
        StartCoroutine(WaitAndSetReady());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (showDebugLogs) Debug.Log("Player joined: " + newPlayer.NickName + ". Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        UpdatePlayerAvatars();
        StartCoroutine(DelayedCheckAllPlayersReady());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey(AVATAR_INDEX_KEY))
            UpdatePlayerAvatars();

        if (changedProps.ContainsKey(PLAYER_READY_KEY))
            CheckAllPlayersReady();
    }

    private void UpdatePlayerAvatars()
    {
        if (leftAvatarSlot == null || rightAvatarSlot == null || availableAvatars.Length == 0)
            return;

        Player[] players = PhotonNetwork.PlayerList;

        leftAvatarSlot.sprite = availableAvatars[localPlayerAvatarIndex];
        leftAvatarSlot.gameObject.SetActive(true);
        leftAvatarSlot.color = Color.white;

        if (players.Length > 1)
        {
            Player otherPlayer = null;
            foreach (Player player in players)
            {
                if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    otherPlayer = player;
                    break;
                }
            }

            if (otherPlayer != null)
            {
                int avatarIndex = 1;
                if (otherPlayer.CustomProperties.ContainsKey(AVATAR_INDEX_KEY))
                    avatarIndex = (int)otherPlayer.CustomProperties[AVATAR_INDEX_KEY];

                avatarIndex = Mathf.Clamp(avatarIndex, 0, availableAvatars.Length - 1);
                rightAvatarSlot.sprite = availableAvatars[avatarIndex];
                rightAvatarSlot.color = Color.white;
                rightAvatarSlot.gameObject.SetActive(true);

                if (vsImage != null && !vsImage.gameObject.activeSelf)
                {
                    vsImage.gameObject.SetActive(true);
                    StartCoroutine(AnimateVSImage());
                }
            }
        }
        else
        {
            rightAvatarSlot.sprite = availableAvatars[1];
            Color transparent = rightAvatarSlot.color;
            transparent.a = 0.5f;
            rightAvatarSlot.color = transparent;
            rightAvatarSlot.gameObject.SetActive(true);
        }
    }

    private IEnumerator AnimateVSImage()
    {
        if (vsImage == null) yield break;

        vsImage.color = Color.white;
        vsImage.transform.localScale = Vector3.one * 0.5f;
        vsImage.gameObject.SetActive(true);

        float animTime = 0.5f;
        float elapsed = 0;

        while (elapsed < animTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / animTime;
            vsImage.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * 1.2f, progress);
            yield return null;
        }

        elapsed = 0;
        while (elapsed < animTime * 0.5f)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / (animTime * 0.5f);
            vsImage.transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, progress);
            yield return null;
        }

        vsImage.transform.localScale = Vector3.one;
    }

    public void SetLocalPlayerAvatar(int avatarIndex)
    {
        if (availableAvatars == null || availableAvatars.Length == 0) return;
        if (avatarIndex < 0 || avatarIndex >= availableAvatars.Length) return;

        localPlayerAvatarIndex = avatarIndex;

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add(AVATAR_INDEX_KEY, avatarIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        UpdatePlayerAvatars();
    }

    public void OnReadyButtonClicked()
    {
        StartCoroutine(WaitAndSetReady());
    }

    private IEnumerator WaitAndSetReady()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
            yield return null;

        SetPlayerReady(true);

        if (readyButton != null) readyButton.SetActive(false);
        if (showDebugLogs) Debug.Log("Local player clicked ready");
    }

    private void SetPlayerReady(bool isReady)
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add(PLAYER_READY_KEY, isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (showDebugLogs)
            Debug.Log("Set PLAYER_READY_KEY = " + isReady + " for player: " + PhotonNetwork.LocalPlayer.NickName);
    }

    private void CheckAllPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Player[] players = PhotonNetwork.PlayerList;
        if (players.Length < 2)
        {
            if (showDebugLogs) Debug.Log("Waiting for 2 players to be present");
            return;
        }

        foreach (Player player in players)
        {
            if (!player.CustomProperties.ContainsKey(PLAYER_READY_KEY) || !(bool)player.CustomProperties[PLAYER_READY_KEY])
            {
                if (showDebugLogs) Debug.Log("Player not ready: " + player.NickName);
                return;
            }
        }

        if (showDebugLogs) Debug.Log("All players ready. Starting battle...");
        StartCoroutine(StartBattle());
    }

    private IEnumerator DelayedCheckAllPlayersReady()
    {
        yield return new WaitForSeconds(1f);
        CheckAllPlayersReady();
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(battleStartDelay);
        if (showDebugLogs) Debug.Log("Loading scene: GameUI");
        PhotonNetwork.LoadLevel("GameUI");
    }

    public void CycleLocalPlayerAvatar()
    {
        if (availableAvatars == null || availableAvatars.Length == 0) return;

        localPlayerAvatarIndex = (localPlayerAvatarIndex + 1) % availableAvatars.Length;
        SetLocalPlayerAvatar(localPlayerAvatarIndex);
    }
}