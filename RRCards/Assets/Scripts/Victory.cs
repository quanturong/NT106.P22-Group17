using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class Victory : MonoBehaviourPunCallbacks
{
    public Image victoryImage;
    public float duration = 1f;
    public Button lobbyButton;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool isDisconnecting = false;
    private bool statsUpdated = false; // Flag để track việc update stats

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
            Debug.Log("=== VICTORY SCREEN STARTED ===");

        // COMPREHENSIVE DEBUG
        Debug.Log($"=== VICTORY SCENE DEBUG ===");
        Debug.Log($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");
        Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");

        // Find all PlayerStatisticsManager objects
        var allStatsManagers = FindObjectsOfType<PlayerStatisticsManager>();
        Debug.Log($"Found {allStatsManagers.Length} PlayerStatisticsManager objects in scene");

        foreach (var manager in allStatsManagers)
        {
            Debug.Log($"  - GameObject: {manager.gameObject.name}, Active: {manager.gameObject.activeInHierarchy}");
        }

        StartCoroutine(PopAndShineLoop());

        if (lobbyButton != null)
            lobbyButton.onClick.AddListener(OnLobbyButtonClicked);

        // DELAY stats update to ensure everything is loaded
        StartCoroutine(DelayedStatsUpdate());
    }

    private IEnumerator DelayedStatsUpdate()
    {
        // Wait a bit for everything to settle
        yield return new WaitForSeconds(1f);

        Debug.Log("=== DELAYED STATS UPDATE START ===");
        Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");
        Debug.Log($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");

        // Try to ensure stats manager exists
        EnsureStatsManager();

        if (PlayerStatisticsManager.Instance != null)
        {
            Debug.Log("✅ Calling UpdateMatchResult for WIN...");
            PlayerStatisticsManager.Instance.UpdateMatchResult(true, () => {
                Debug.Log("✅ [Victory] Stats updated successfully - WIN recorded");
                statsUpdated = true;
            });
        }
        else
        {
            Debug.LogError("❌ [Victory] PlayerStatisticsManager.Instance is STILL null after delay!");

            // Try manual stats update as fallback
            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                Debug.Log("🔄 Attempting manual stats update as fallback...");
                ManualStatsUpdate();
            }
            else
            {
                Debug.LogError("❌ PlayFab not logged in - cannot update stats");
                statsUpdated = true; // Skip stats update
            }
        }
    }

    private void EnsureStatsManager()
    {
        Debug.Log("=== ENSURING STATS MANAGER ===");

        if (PlayerStatisticsManager.Instance == null)
        {
            Debug.Log("PlayerStatisticsManager.Instance is null, trying to find or fix...");

            // Try to find existing one
            var foundManager = FindObjectOfType<PlayerStatisticsManager>();

            if (foundManager != null)
            {
                Debug.Log("✅ Found PlayerStatisticsManager, setting as Instance");
                PlayerStatisticsManager.Instance = foundManager;

                // Initialize if needed
                if (!foundManager.IsInitialized() && PlayFabClientAPI.IsClientLoggedIn())
                {
                    Debug.Log("Initializing found PlayerStatisticsManager...");
                    foundManager.ManualInitialize();
                }
            }
            else
            {
                Debug.LogWarning("⚠️ No PlayerStatisticsManager found in scene");
                Debug.Log("This explains why stats aren't updating - PlayerStatisticsManager was destroyed during scene transition");
            }
        }
        else
        {
            Debug.Log("✅ PlayerStatisticsManager.Instance already exists");
        }
    }

    private void ManualStatsUpdate()
    {
        Debug.Log("=== MANUAL STATS UPDATE ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.LogError("❌ PlayFab not logged in for manual update!");
            statsUpdated = true;
            return;
        }

        // Get current stats first
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = 0, wins = 0, losses = 0;

            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "TotalGames") total = stat.Value;
                else if (stat.StatisticName == "Wins") wins = stat.Value;
                else if (stat.StatisticName == "Losses") losses = stat.Value;
            }

            Debug.Log($"📊 Current manual stats - Total: {total}, Wins: {wins}, Losses: {losses}");

            // Update with win
            total++;
            wins++;

            Debug.Log($"📊 New manual stats - Total: {total}, Wins: {wins}, Losses: {losses}");

            var updatedStats = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "TotalGames", Value = total },
                new StatisticUpdate { StatisticName = "Wins", Value = wins },
                new StatisticUpdate { StatisticName = "Losses", Value = losses }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = updatedStats
            },
            updateResult => {
                Debug.Log("✅ Manual stats update successful - WIN recorded!");
                statsUpdated = true;
            },
            error => {
                Debug.LogError($"❌ Manual stats update failed: {error.ErrorMessage}");
                statsUpdated = true; // Continue anyway
            });
        }, error =>
        {
            Debug.LogError($"❌ Failed to get current stats for manual update: {error.ErrorMessage}");
            statsUpdated = true; // Continue anyway
        });
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

                if (victoryImage != null)
                {
                    Color color = victoryImage.color;
                    color.a = alpha;
                    victoryImage.color = color;
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
                Debug.Log("Victory: Already disconnecting, ignoring button click");
            return;
        }

        // Kiểm tra xem stats đã được update chưa
        if (!statsUpdated)
        {
            if (showDebugLogs)
                Debug.Log("Victory: Stats not updated yet, waiting...");

            StartCoroutine(WaitForStatsAndReturn());
            return;
        }

        isDisconnecting = true;

        if (lobbyButton != null)
            lobbyButton.interactable = false;

        if (showDebugLogs)
            Debug.Log("Victory: Starting return to lobby process...");

        StartCoroutine(ReturnToLobbySequence());
    }

    // Coroutine để đợi stats update xong rồi mới return
    private IEnumerator WaitForStatsAndReturn()
    {
        if (lobbyButton != null)
            lobbyButton.interactable = false;

        if (showDebugLogs)
            Debug.Log("Victory: Waiting for stats to update...");

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
                Debug.LogWarning("Victory: Stats update timeout, proceeding anyway");
            statsUpdated = true;
        }

        // Bây giờ mới bắt đầu return to lobby
        isDisconnecting = true;

        if (showDebugLogs)
            Debug.Log("Victory: Stats ready, starting return to lobby process...");

        StartCoroutine(ReturnToLobbySequence());
    }

    private IEnumerator ReturnToLobbySequence()
    {
        if (showDebugLogs)
            Debug.Log("=== VICTORY: RETURN TO LOBBY SEQUENCE STARTED ===");

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
            Debug.Log("Victory: Starting COMPLETE property cleanup...");

        if (PhotonNetwork.LocalPlayer == null)
        {
            if (showDebugLogs)
                Debug.LogWarning("Victory: No local player found during cleanup");
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
            Debug.Log($"Victory: Found {keysToRemove.Count} properties to clean");

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
            Debug.Log($"Victory: Cleaned {keysToRemove.Count} existing + {CRITICAL_PROPERTIES.Length} critical properties");

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
            Debug.Log($"Victory: Verification - {remainingProps.Count} properties remaining after cleanup");

            if (remainingProps.Count > 0)
            {
                foreach (var prop in remainingProps)
                {
                    if (prop.Value != null)
                        Debug.LogWarning($"Victory: Property still exists: {prop.Key} = {prop.Value}");
                }
            }
            else
            {
                Debug.Log("Victory: Property cleanup SUCCESSFUL - all properties cleared");
            }
        }
    }

    private IEnumerator SafeLeaveRoom()
    {
        if (showDebugLogs)
            Debug.Log("Victory: Starting safe room leave process...");

        if (PhotonNetwork.InRoom)
        {
            if (showDebugLogs)
                Debug.Log("Victory: Leaving room: " + PhotonNetwork.CurrentRoom.Name);

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
                    Debug.LogWarning("Victory: Leave room timeout, forcing disconnect");
                PhotonNetwork.Disconnect();

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
                Debug.Log("Victory: Not in room, no need to leave");
        }

        if (showDebugLogs)
            Debug.Log("Victory: Room leave process completed");
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        if (showDebugLogs)
            Debug.Log("Victory: Successfully left room");

        // Don't load scene here, let the sequence handle it
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (showDebugLogs)
            Debug.Log("Victory: Other player left room: " + otherPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (showDebugLogs)
            Debug.Log("Victory: Disconnected with cause: " + cause);

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
            Debug.Log("Victory: Loading lobby scene");

        SceneManager.LoadScene("Lobby");
    }
    #endregion

    #region Emergency Methods
    // Force disconnect method for emergency cases
    public void ForceDisconnectAndLoadLobby()
    {
        if (showDebugLogs)
            Debug.Log("Victory: Force disconnect initiated");

        isDisconnecting = true;

        if (lobbyButton != null)
            lobbyButton.interactable = false;

        StartCoroutine(ForceDisconnectSequence());
    }

    private IEnumerator ForceDisconnectSequence()
    {
        if (showDebugLogs)
            Debug.Log("Victory: Starting force disconnect sequence");

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
            Debug.Log("Victory: EMERGENCY return to lobby triggered");

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
            Debug.Log("=== VICTORY - CURRENT PROPERTIES ===");
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
            Debug.Log("=== VICTORY - NETWORK STATE ===");
            Debug.Log($"IsConnected: {PhotonNetwork.IsConnected}");
            Debug.Log($"InLobby: {PhotonNetwork.InLobby}");
            Debug.Log($"InRoom: {PhotonNetwork.InRoom}");
            Debug.Log($"NetworkClientState: {PhotonNetwork.NetworkClientState}");
            Debug.Log($"IsDisconnecting: {isDisconnecting}");
        }
    }

    [ContextMenu("Debug Stats Manager")]
    public void DebugStatsManager()
    {
        if (showDebugLogs)
        {
            Debug.Log("=== VICTORY - STATS MANAGER DEBUG ===");
            Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");
            Debug.Log($"Stats updated flag: {statsUpdated}");
            Debug.Log($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");

            if (PlayerStatisticsManager.Instance != null)
            {
                Debug.Log("PlayerStatisticsManager is available and ready to use");
                Debug.Log($"Is Initialized: {PlayerStatisticsManager.Instance.IsInitialized()}");
            }
            else
            {
                Debug.LogWarning("PlayerStatisticsManager.Instance is NULL!");

                var foundManager = FindObjectOfType<PlayerStatisticsManager>();
                if (foundManager != null)
                {
                    Debug.Log($"Found PlayerStatisticsManager in scene: {foundManager.gameObject.name}");
                }
                else
                {
                    Debug.LogError("No PlayerStatisticsManager found in scene at all!");
                }
            }
        }
    }

    [ContextMenu("Force Stats Update")]
    public void ForceStatsUpdate()
    {
        Debug.Log("=== CONTEXT MENU: FORCE STATS UPDATE ===");
        EnsureStatsManager();

        if (PlayerStatisticsManager.Instance != null)
        {
            PlayerStatisticsManager.Instance.UpdateMatchResult(true, () => {
                Debug.Log("✅ Context menu stats update completed");
            });
        }
        else
        {
            Debug.Log("Attempting manual stats update...");
            ManualStatsUpdate();
        }
    }

    [ContextMenu("Test Manual Stats")]
    public void TestManualStats()
    {
        Debug.Log("=== TESTING MANUAL STATS ===");
        ManualStatsUpdate();
    }

    [ContextMenu("Full Diagnostic")]
    public void FullDiagnostic()
    {
        Debug.Log("=== VICTORY - FULL DIAGNOSTIC ===");
        DebugNetworkState();
        DebugStatsManager();
        DebugCurrentProperties();

        Debug.Log($"GameObject active: {gameObject.activeInHierarchy}");
        Debug.Log($"Component enabled: {enabled}");
        Debug.Log($"Is disconnecting: {isDisconnecting}");
        Debug.Log($"Stats updated: {statsUpdated}");
    }
    #endregion

    #region Cleanup
    private void OnDestroy()
    {
        if (showDebugLogs)
            Debug.Log("Victory: OnDestroy called");

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
                Debug.Log("Victory: Application paused, initiating safe disconnect");

            ForceDisconnectAndLoadLobby();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && !isDisconnecting)
        {
            if (showDebugLogs)
                Debug.Log("Victory: Application lost focus");
        }
    }
    #endregion
}