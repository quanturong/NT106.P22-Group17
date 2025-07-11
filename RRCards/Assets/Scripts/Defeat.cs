using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Defeat : MonoBehaviourPunCallbacks
{
    public Image defeatImage;
    public float duration = 1f;
    public Button lobbyButton;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool isDisconnecting = false;
    private bool statsUpdated = false; // Thêm flag để track việc update stats

    // Critical properties to clean
    private readonly string[] CRITICAL_PROPERTIES = {
        "IsReady", "AvatarIndex", "PlayerRole", "IsHost",
        "TeamId", "Score", "GameState", "PlayerIndex",
        "CharacterSelected", "LoadingComplete", "GameReady",
        "InGame", "HasJoinedGame", "GamePosition"
    };

    private void Start()
    {
        if (showDebugLogs)
            Debug.Log("=== DEFEAT SCREEN STARTED ===");

        StartCoroutine(PopAndShineLoop());

        if (lobbyButton != null)
            lobbyButton.onClick.AddListener(OnLobbyButtonClicked);

        // CHỈ update stats, KHÔNG tự động return to lobby
        if (PlayerStatisticsManager.Instance != null)
        {
            PlayerStatisticsManager.Instance.UpdateMatchResult(false, () => {
                Debug.Log("Defeat stats updated successfully");
                statsUpdated = true; // Đánh dấu là đã update stats
            });
        }
        else
        {
            statsUpdated = true; // Nếu không có stats manager thì coi như đã xong
        }
    }

    IEnumerator PopAndShineLoop()
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        // Pop animation
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // Shine loop
        while (true)
        {
            float loopTime = 0f;
            while (loopTime < 1f)
            {
                loopTime += Time.deltaTime;
                float alpha = Mathf.PingPong(loopTime * 2f, 1f);

                if (defeatImage != null)
                {
                    Color color = defeatImage.color;
                    color.a = alpha;
                    defeatImage.color = color;
                }

                yield return null;
            }
        }
    }

    public void OnLobbyButtonClicked()
    {
        if (isDisconnecting)
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Already disconnecting, ignoring button click");
            return;
        }

        // Kiểm tra xem stats đã được update chưa
        if (!statsUpdated)
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Stats not updated yet, waiting...");

            StartCoroutine(WaitForStatsAndReturn());
            return;
        }

        isDisconnecting = true;

        if (lobbyButton != null)
            lobbyButton.interactable = false;

        if (showDebugLogs)
            Debug.Log("Defeat: Starting return to lobby process...");

        StartCoroutine(ReturnToLobbySequence());
    }

    // Coroutine để đợi stats update xong rồi mới return
    private IEnumerator WaitForStatsAndReturn()
    {
        if (lobbyButton != null)
            lobbyButton.interactable = false;

        if (showDebugLogs)
            Debug.Log("Defeat: Waiting for stats to update...");

        // Đợi stats update xong (tối đa 5 giây)
        float timeout = 5f;
        float timer = 0f;
        while (!statsUpdated && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!statsUpdated)
        {
            if (showDebugLogs)
                Debug.LogWarning("Defeat: Stats update timeout, proceeding anyway");
            statsUpdated = true;
        }

        // Bây giờ mới bắt đầu return to lobby
        isDisconnecting = true;

        if (showDebugLogs)
            Debug.Log("Defeat: Stats ready, starting return to lobby process...");

        StartCoroutine(ReturnToLobbySequence());
    }

    private IEnumerator ReturnToLobbySequence()
    {
        if (showDebugLogs)
            Debug.Log("=== DEFEAT: RETURN TO LOBBY SEQUENCE STARTED ===");

        // Step 1: Complete property cleanup
        yield return StartCoroutine(CompletePropertyCleanup());

        // Step 2: Wait for properties to sync
        yield return new WaitForSeconds(1f);

        // Step 3: Leave room safely
        yield return StartCoroutine(SafeLeaveRoom());

        // Step 4: Final wait and load lobby
        yield return new WaitForSeconds(0.5f);
        LoadLobbyScene();
    }

    private IEnumerator CompletePropertyCleanup()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Starting COMPLETE property cleanup...");

        if (PhotonNetwork.LocalPlayer == null)
        {
            if (showDebugLogs)
                Debug.LogWarning("Defeat: No local player found during cleanup");
            yield break;
        }

        // Get all current properties
        var currentProps = PhotonNetwork.LocalPlayer.CustomProperties;
        var keysToRemove = new System.Collections.Generic.List<string>();

        foreach (var key in currentProps.Keys)
        {
            keysToRemove.Add(key.ToString());
        }

        if (showDebugLogs)
            Debug.Log($"Defeat: Found {keysToRemove.Count} properties to clean");

        // Create hashtable to clear all properties
        ExitGames.Client.Photon.Hashtable clearProps = new ExitGames.Client.Photon.Hashtable();

        // Remove all existing properties by setting them to null
        foreach (string key in keysToRemove)
        {
            clearProps[key] = null;
        }

        // Ensure critical game-related properties are definitely null
        foreach (string criticalProp in CRITICAL_PROPERTIES)
        {
            clearProps[criticalProp] = null;
        }

        // Apply the property changes
        PhotonNetwork.LocalPlayer.SetCustomProperties(clearProps);

        if (showDebugLogs)
            Debug.Log($"Defeat: Cleaned {keysToRemove.Count} existing + {CRITICAL_PROPERTIES.Length} critical properties");

        // Wait for properties to sync
        yield return new WaitForSeconds(0.5f);

        // Verify cleanup
        yield return StartCoroutine(VerifyPropertyCleanup());
    }

    private IEnumerator VerifyPropertyCleanup()
    {
        yield return new WaitForSeconds(0.2f);

        if (PhotonNetwork.LocalPlayer != null && showDebugLogs)
        {
            var remainingProps = PhotonNetwork.LocalPlayer.CustomProperties;
            Debug.Log($"Defeat: Verification - {remainingProps.Count} properties remaining after cleanup");

            if (remainingProps.Count > 0)
            {
                foreach (var prop in remainingProps)
                {
                    if (prop.Value != null)
                        Debug.LogWarning($"Defeat: Property still exists: {prop.Key} = {prop.Value}");
                }
            }
            else
            {
                Debug.Log("Defeat: Property cleanup SUCCESSFUL - all properties cleared");
            }
        }
    }

    private IEnumerator SafeLeaveRoom()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Starting safe room leave process...");

        if (PhotonNetwork.InRoom)
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Leaving room: " + PhotonNetwork.CurrentRoom.Name);

            PhotonNetwork.LeaveRoom();

            // Wait for leave room confirmation or timeout
            float timeout = 5f;
            float timer = 0f;
            while (PhotonNetwork.InRoom && timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (PhotonNetwork.InRoom)
            {
                if (showDebugLogs)
                    Debug.LogWarning("Defeat: Leave room timeout, forcing disconnect");
                PhotonNetwork.Disconnect();

                // Wait for disconnect
                timer = 0f;
                while (PhotonNetwork.IsConnected && timer < 3f)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }
        else
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Not in room, no need to leave");
        }

        if (showDebugLogs)
            Debug.Log("Defeat: Room leave process completed");
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Successfully left room");

        // Don't load scene here, let the sequence handle it
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Other player left room: " + otherPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Disconnected with cause: " + cause);

        // If we're not already in the process of returning to lobby, do it now
        if (!isDisconnecting)
        {
            LoadLobbyScene();
        }
    }
    #endregion

    #region Scene Management
    private void LoadLobbyScene()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Loading lobby scene");

        SceneManager.LoadScene("Lobby");
    }
    #endregion

    #region Emergency Methods
    // Force disconnect method for emergency cases
    public void ForceDisconnectAndLoadLobby()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Force disconnect initiated");

        isDisconnecting = true;

        if (lobbyButton != null)
            lobbyButton.interactable = false;

        StartCoroutine(ForceDisconnectSequence());
    }

    private IEnumerator ForceDisconnectSequence()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: Starting force disconnect sequence");

        // Clean properties first
        yield return StartCoroutine(CompletePropertyCleanup());

        // Force disconnect immediately
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();

            // Wait for disconnect or timeout
            float timeout = 3f;
            float timer = 0f;
            while (PhotonNetwork.IsConnected && timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        LoadLobbyScene();
    }

    // Emergency method accessible from inspector/debug
    [ContextMenu("Emergency Return to Lobby")]
    public void EmergencyReturnToLobby()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: EMERGENCY return to lobby triggered");

        StopAllCoroutines();
        isDisconnecting = true;

        if (lobbyButton != null)
            lobbyButton.interactable = false;

        StartCoroutine(EmergencyReturnSequence());
    }

    private IEnumerator EmergencyReturnSequence()
    {
        // Quick property cleanup
        if (PhotonNetwork.LocalPlayer != null)
        {
            ExitGames.Client.Photon.Hashtable clearProps = new ExitGames.Client.Photon.Hashtable();
            foreach (string prop in CRITICAL_PROPERTIES)
            {
                clearProps[prop] = null;
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(clearProps);
        }

        yield return new WaitForSeconds(0.2f);

        // Force disconnect
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            yield return new WaitForSeconds(1f);
        }

        LoadLobbyScene();
    }
    #endregion

    #region Debug Methods
    // Debug method to check current properties
    [ContextMenu("Debug Current Properties")]
    public void DebugCurrentProperties()
    {
        if (PhotonNetwork.LocalPlayer != null && showDebugLogs)
        {
            Debug.Log("=== DEFEAT - CURRENT PROPERTIES ===");
            var props = PhotonNetwork.LocalPlayer.CustomProperties;

            if (props.Count == 0)
            {
                Debug.Log("No properties found (CLEAN STATE)");
            }
            else
            {
                foreach (var prop in props)
                {
                    Debug.Log($"Key: {prop.Key}, Value: {prop.Value}");
                }
            }
        }
        else
        {
            Debug.Log("No local player or debug logs disabled");
        }
    }

    [ContextMenu("Debug Network State")]
    public void DebugNetworkState()
    {
        if (showDebugLogs)
        {
            Debug.Log("=== DEFEAT - NETWORK STATE ===");
            Debug.Log($"IsConnected: {PhotonNetwork.IsConnected}");
            Debug.Log($"InLobby: {PhotonNetwork.InLobby}");
            Debug.Log($"InRoom: {PhotonNetwork.InRoom}");
            Debug.Log($"NetworkClientState: {PhotonNetwork.NetworkClientState}");
            Debug.Log($"IsDisconnecting: {isDisconnecting}");
        }
    }
    #endregion

    #region Cleanup
    private void OnDestroy()
    {
        if (showDebugLogs)
            Debug.Log("Defeat: OnDestroy called");

        if (lobbyButton != null)
            lobbyButton.onClick.RemoveListener(OnLobbyButtonClicked);

        // Stop all coroutines to prevent errors
        StopAllCoroutines();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && !isDisconnecting)
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Application paused, initiating safe disconnect");

            ForceDisconnectAndLoadLobby();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !isDisconnecting)
        {
            if (showDebugLogs)
                Debug.Log("Defeat: Application lost focus");
        }
    }
    #endregion
}