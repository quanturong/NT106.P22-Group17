using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class Defeat : MonoBehaviourPunCallbacks
{
    public Image defeatImage;
    public float duration = 1f;
    public Button lobbyButton;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool isDisconnecting = false;
    private bool statsUpdated = false; 

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

        Debug.Log($"=== DEFEAT SCENE DEBUG ===");
        Debug.Log($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");
        Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");

        var allStatsManagers = FindObjectsOfType<PlayerStatisticsManager>();
        Debug.Log($"Found {allStatsManagers.Length} PlayerStatisticsManager objects in scene");

        foreach (var manager in allStatsManagers)
        {
            Debug.Log($"  - GameObject: {manager.gameObject.name}, Active: {manager.gameObject.activeInHierarchy}");
        }

        StartCoroutine(PopAndShineLoop());

        if (lobbyButton != null)
            lobbyButton.onClick.AddListener(OnLobbyButtonClicked);

        StartCoroutine(DelayedStatsUpdate());
    }

    private IEnumerator DelayedStatsUpdate()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("=== DELAYED STATS UPDATE START ===");
        Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");
        Debug.Log($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");

        EnsureStatsManager();

        if (PlayerStatisticsManager.Instance != null)
        {
            Debug.Log("✅ Calling UpdateMatchResult for LOSS...");
            PlayerStatisticsManager.Instance.UpdateMatchResult(false, () => {
                Debug.Log("✅ [Defeat] Stats updated successfully - LOSS recorded");
                statsUpdated = true;
            });
        }
        else
        {
            Debug.LogError("❌ [Defeat] PlayerStatisticsManager.Instance is STILL null after delay!");

            if (PlayFabClientAPI.IsClientLoggedIn())
            {
                Debug.Log("🔄 Attempting manual stats update as fallback...");
                ManualStatsUpdate();
            }
            else
            {
                Debug.LogError("❌ PlayFab not logged in - cannot update stats");
                statsUpdated = true; 
            }
        }
    }

    private void EnsureStatsManager()
    {
        Debug.Log("=== ENSURING STATS MANAGER ===");

        if (PlayerStatisticsManager.Instance == null)
        {
            Debug.Log("PlayerStatisticsManager.Instance is null, trying to find or fix...");

            var foundManager = FindObjectOfType<PlayerStatisticsManager>();

            if (foundManager != null)
            {
                Debug.Log("✅ Found PlayerStatisticsManager, setting as Instance");
                PlayerStatisticsManager.Instance = foundManager;

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

            total++;
            losses++;

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
                Debug.Log("✅ Manual stats update successful - LOSS recorded!");
                statsUpdated = true;
            },
            error => {
                Debug.LogError($"❌ Manual stats update failed: {error.ErrorMessage}");
                statsUpdated = true;
            });
        }, error =>
        {
            Debug.LogError($"❌ Failed to get current stats for manual update: {error.ErrorMessage}");
            statsUpdated = true;
        });
    }

    IEnumerator PopAndShineLoop()
    {
        float time = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

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

    private IEnumerator WaitForStatsAndReturn()
    {
        if (lobbyButton != null)
            lobbyButton.interactable = false;

        if (showDebugLogs)
            Debug.Log("Defeat: Waiting for stats to update...");

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

        isDisconnecting = true;

        if (showDebugLogs)
            Debug.Log("Defeat: Stats ready, starting return to lobby process...");

        StartCoroutine(ReturnToLobbySequence());
    }

    private IEnumerator ReturnToLobbySequence()
    {
        if (showDebugLogs)
            Debug.Log("=== DEFEAT: RETURN TO LOBBY SEQUENCE STARTED ===");

        yield return StartCoroutine(CompletePropertyCleanup());

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(SafeLeaveRoom());

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

        var currentProps = PhotonNetwork.LocalPlayer.CustomProperties;
        var keysToRemove = new System.Collections.Generic.List<string>();

        foreach (var key in currentProps.Keys)
        {
            keysToRemove.Add(key.ToString());
        }

        if (showDebugLogs)
            Debug.Log($"Defeat: Found {keysToRemove.Count} properties to clean");

        ExitGames.Client.Photon.Hashtable clearProps = new ExitGames.Client.Photon.Hashtable();

        foreach (string key in keysToRemove)
        {
            clearProps[key] = null;
        }

        foreach (string criticalProp in CRITICAL_PROPERTIES)
        {
            clearProps[criticalProp] = null;
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(clearProps);

        if (showDebugLogs)
            Debug.Log($"Defeat: Cleaned {keysToRemove.Count} existing + {CRITICAL_PROPERTIES.Length} critical properties");
        yield return new WaitForSeconds(0.5f);
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
        yield return StartCoroutine(CompletePropertyCleanup());
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
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
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            yield return new WaitForSeconds(1f);
        }

        LoadLobbyScene();
    }
    #endregion

    #region Debug Methods
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

    [ContextMenu("Debug Stats Manager")]
    public void DebugStatsManager()
    {
        if (showDebugLogs)
        {
            Debug.Log("=== DEFEAT - STATS MANAGER DEBUG ===");
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
            PlayerStatisticsManager.Instance.UpdateMatchResult(false, () => {
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
        Debug.Log("=== DEFEAT - FULL DIAGNOSTIC ===");
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
            Debug.Log("Defeat: OnDestroy called");

        if (lobbyButton != null)
            lobbyButton.onClick.RemoveListener(OnLobbyButtonClicked);
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