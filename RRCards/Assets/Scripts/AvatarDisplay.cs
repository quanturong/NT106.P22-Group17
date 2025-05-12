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

    // Photon custom property keys
    private const string AVATAR_INDEX_KEY = "AvatarIndex";
    private const string PLAYER_READY_KEY = "IsReady";

    // Keep track of local player avatar selection
    private int localPlayerAvatarIndex = 0;

    void Start()
    {
        // Check for null references
        CheckReferences();

        // Initialize UI
        InitializeUI();

        // Set default avatar for local player
        SetLocalPlayerAvatar(0);

        // Force immediate update of avatars
        ShowDefaultAvatars();

        if (showDebugLogs)
        {
            Debug.Log("PhotonVersusRoomManager started. Current player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
    }

    private void CheckReferences()
    {
        if (leftAvatarSlot == null)
        {
            Debug.LogError("leftAvatarSlot is not assigned in the Inspector!");
        }

        if (rightAvatarSlot == null)
        {
            Debug.LogError("rightAvatarSlot is not assigned in the Inspector!");
        }

        if (vsImage == null)
        {
            Debug.LogError("vsImage is not assigned in the Inspector!");
        }

        if (readyButton == null)
        {
            Debug.LogError("readyButton is not assigned in the Inspector!");
        }

        if (availableAvatars == null || availableAvatars.Length == 0)
        {
            Debug.LogError("availableAvatars is not assigned in the Inspector!");
        }
    }

    private void InitializeUI()
    {
        // Make sure vsImage is visible
        if (vsImage != null)
        {
            vsImage.gameObject.SetActive(true);
            StartCoroutine(AnimateVSImage());
        }

        // Hide ready button if not master client
        if (readyButton != null)
        {
            readyButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    // Show default avatars immediately
    private void ShowDefaultAvatars()
    {
        if (leftAvatarSlot == null || rightAvatarSlot == null || availableAvatars == null || availableAvatars.Length < 2)
        {
            Debug.LogError("Cannot show default avatars: missing references");
            return;
        }

        // Set default avatars visible immediately
        leftAvatarSlot.sprite = availableAvatars[0];
        leftAvatarSlot.color = Color.white;
        leftAvatarSlot.gameObject.SetActive(true);

        // If we're in a room with multiple players, show both avatars
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            rightAvatarSlot.sprite = availableAvatars[1];
            rightAvatarSlot.color = Color.white;
            rightAvatarSlot.gameObject.SetActive(true);

            if (showDebugLogs)
            {
                Debug.Log("Showing both avatars for multiple players");
            }
        }
        else
        {
            // If we're alone, only show the left avatar initially
            if (showDebugLogs)
            {
                Debug.Log("Only showing left avatar since player count is: " +
                    (PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.PlayerCount.ToString() : "null room"));
            }

            // Make right avatar semi-transparent or use a placeholder
            rightAvatarSlot.sprite = availableAvatars[1];
            Color transparentColor = rightAvatarSlot.color;
            transparentColor.a = 0.5f;
            rightAvatarSlot.color = transparentColor;
            rightAvatarSlot.gameObject.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (showDebugLogs)
        {
            Debug.Log("Joined room. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        // CHỈ gọi UpdatePlayerAvatars
        UpdatePlayerAvatars();

        if (readyButton != null)
        {
            readyButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    // Called when a player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (showDebugLogs)
        {
            Debug.Log("Player entered room: " + newPlayer.NickName);
        }

        // KHÔNG gọi ShowDefaultAvatars
        UpdatePlayerAvatars();
    }


    // Called when player properties are updated
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey(AVATAR_INDEX_KEY))
        {
            UpdatePlayerAvatars();
        }

        if (changedProps.ContainsKey(PLAYER_READY_KEY))
        {
            CheckAllPlayersReady();
        }
    }

    // Update UI to show player avatars
    private void UpdatePlayerAvatars()
    {
        // Ensure references are valid
        if (leftAvatarSlot == null || rightAvatarSlot == null || availableAvatars == null || availableAvatars.Length == 0)
        {
            Debug.LogError("Missing required references for UpdatePlayerAvatars!");
            return;
        }

        Player[] players = PhotonNetwork.PlayerList;

        if (showDebugLogs)
        {
            Debug.Log("Updating player avatars. Player count: " + players.Length);
        }

        // Always show at least one avatar (local player)
        leftAvatarSlot.sprite = availableAvatars[localPlayerAvatarIndex];
        leftAvatarSlot.gameObject.SetActive(true);
        leftAvatarSlot.color = Color.white;

        // If we have a second player, show their avatar
        if (players.Length > 1)
        {
            // Find the other player (not the local player)
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
                int avatarIndex = 1; // Default to second avatar if not specified

                // Get avatar index from player properties
                if (otherPlayer.CustomProperties.ContainsKey(AVATAR_INDEX_KEY))
                {
                    avatarIndex = (int)otherPlayer.CustomProperties[AVATAR_INDEX_KEY];
                }
                else
                {
                    // If the other player hasn't set an avatar yet, log this for debugging
                    if (showDebugLogs)
                    {
                        Debug.Log("Other player doesn't have an avatar index set yet, using default: " + avatarIndex);
                    }
                }

                // Clamp avatar index to valid range
                avatarIndex = Mathf.Clamp(avatarIndex, 0, availableAvatars.Length - 1);

                // Assign avatar to right slot
                rightAvatarSlot.sprite = availableAvatars[avatarIndex];
                rightAvatarSlot.gameObject.SetActive(true);
                rightAvatarSlot.color = Color.white;

                if (showDebugLogs)
                {
                    Debug.Log("Setting other player avatar to index: " + avatarIndex);
                }

                // Make sure VS is visible and animated
                if (vsImage != null && vsImage.gameObject.activeSelf == false)
                {
                    vsImage.gameObject.SetActive(true);
                    StartCoroutine(AnimateVSImage());
                }
            }
        }
        else
        {
            // If we're alone, only show the left avatar
            // Or show the right avatar as semi-transparent
            rightAvatarSlot.sprite = availableAvatars[1]; // Default avatar
            Color transparentColor = rightAvatarSlot.color;
            transparentColor.a = 0.5f;
            rightAvatarSlot.color = transparentColor;
            rightAvatarSlot.gameObject.SetActive(true);

            if (showDebugLogs)
            {
                Debug.Log("Solo player mode, showing semi-transparent right avatar");
            }
        }
    }
    private IEnumerator AnimateVSImage()
    {
        if (vsImage == null)
        {
            Debug.LogError("vsImage is null in AnimateVSImage!");
            yield break;
        }

        // Initial setup
        vsImage.color = Color.white;
        vsImage.transform.localScale = Vector3.one * 0.5f;
        vsImage.gameObject.SetActive(true);

        // Scale up animation
        float animTime = 0.5f;
        float elapsed = 0;

        while (elapsed < animTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / animTime;

            // Scale up and fade in
            vsImage.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * 1.2f, progress);

            yield return null;
        }

        // Scale back to normal
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

    // Set avatar for local player and sync to network
    public void SetLocalPlayerAvatar(int avatarIndex)
    {
        if (availableAvatars == null || availableAvatars.Length == 0)
        {
            Debug.LogError("availableAvatars is not assigned!");
            return;
        }

        if (avatarIndex < 0 || avatarIndex >= availableAvatars.Length)
            return;

        localPlayerAvatarIndex = avatarIndex;

        // Update local player's custom properties
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add(AVATAR_INDEX_KEY, avatarIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // Update UI immediately
        UpdatePlayerAvatars();
    }

    // Called when the ready button is clicked
    public void OnReadyButtonClicked()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props.Add(PLAYER_READY_KEY, true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // Hide ready button after clicking
        if (readyButton != null)
        {
            readyButton.SetActive(false);
        }

        if (showDebugLogs)
        {
            Debug.Log("Local player is now ready");
        }
    }

    // Check if all players are ready
    private void CheckAllPlayersReady()
    {
        // Only host should start the game
        if (!PhotonNetwork.IsMasterClient)
            return;

        bool allReady = true;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey(PLAYER_READY_KEY))
            {
                if (!(bool)player.CustomProperties[PLAYER_READY_KEY])
                {
                    allReady = false;
                    break;
                }
            }
            else
            {
                allReady = false;
                break;
            }
        }

        if (allReady && PhotonNetwork.PlayerList.Length >= 2)
        {
            if (showDebugLogs)
            {
                Debug.Log("All players ready, starting battle!");
            }
            StartCoroutine(StartBattle());
        }
    }

    // Start the battle after a delay
    private IEnumerator StartBattle()
    {
        // Wait for animation time
        yield return new WaitForSeconds(battleStartDelay);

        // Load the battle scene
        PhotonNetwork.LoadLevel("GameUI");
    }

    // For testing - cycle through available avatars
    public void CycleLocalPlayerAvatar()
    {
        if (availableAvatars == null || availableAvatars.Length == 0)
            return;

        localPlayerAvatarIndex = (localPlayerAvatarIndex + 1) % availableAvatars.Length;
        SetLocalPlayerAvatar(localPlayerAvatarIndex);
    }
}