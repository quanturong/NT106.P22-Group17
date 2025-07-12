using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using Photon.Pun;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject rulePanel;
    public GameObject statsPanel;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private bool isLoggingOut = false;

    void Start()
    {
        DebugLog("MainMenu script started");
        ShowMain();
        StartCoroutine(CheckAndInitializeStats());
    }

    private IEnumerator CheckAndInitializeStats()
    {
        DebugLog("Checking PlayFab login status for stats initialization...");
        float timeout = 15f;
        float timer = 0f;

        while (!PlayFabClientAPI.IsClientLoggedIn() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("✅ PlayFab login detected in MainMenu");
            if (PlayerStatisticsManager.Instance != null)
            {
                DebugLog("PlayerStatisticsManager.Instance found, manually initializing...");
                PlayerStatisticsManager.Instance.ManualInitialize();
            }
            else
            {
                DebugLog("⚠️ PlayerStatisticsManager.Instance is null in MainMenu");
                var foundManager = FindObjectOfType<PlayerStatisticsManager>();
                if (foundManager != null)
                {
                    DebugLog("Found PlayerStatisticsManager in scene, setting as Instance");
                    PlayerStatisticsManager.Instance = foundManager;
                    foundManager.ManualInitialize();
                }
                else
                {
                    DebugLog("❌ No PlayerStatisticsManager found in MainMenu scene!");
                }
            }
        }
        else
        {
            DebugLog("❌ PlayFab login timeout in MainMenu - stats won't be initialized");
        }
    }

    public void ShowMain()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    public void ShowRoom()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    public void ShowStart()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ShowOption()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
    }

    public void ShowRule()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(true);
        statsPanel.SetActive(false);
    }

    public void ShowStats()
    {
        DebugLog("ShowStats called");

        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(true);
        if (PlayerStatisticsManager.Instance != null)
        {
            DebugLog("Loading stats via PlayerStatisticsManager.Instance");
            PlayerStatisticsManager.Instance.LoadAndDisplayStats();
        }
        else
        {
            DebugLog("⚠️ PlayerStatisticsManager.Instance is null when ShowStats called");
            var foundManager = FindObjectOfType<PlayerStatisticsManager>();
            if (foundManager != null)
            {
                DebugLog("Found PlayerStatisticsManager, attempting to load stats");
                foundManager.LoadAndDisplayStats();
            }
            else
            {
                DebugLog("❌ No PlayerStatisticsManager found - cannot load stats");
            }
        }
    }

    public void OnLogout()
    {
        if (isLoggingOut) return;        isLoggingOut = true;

        DebugLog("=== LOGGING OUT FROM MAIN MENU ===");
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey("PlayFabID");
        PlayerPrefs.DeleteKey("DisplayName");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(WaitForPhotonDisconnectThen(() =>
            {
                isLoggingOut = false;
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                SceneManager.LoadScene(0);
#endif
            }));
        }
        else
        {
            isLoggingOut = false;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            SceneManager.LoadScene(0);
#endif
        }
    }

    private IEnumerator WaitForPhotonDisconnectThen(Action callback)
    {
        float timeout = 5f;
        float timer = 0f;

        while (PhotonNetwork.IsConnected && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Debug Methods
    [ContextMenu("Debug PlayFab State")]
    public void DebugPlayFabState()
    {
        DebugLog("=== MAIN MENU - PLAYFAB STATE ===");
        DebugLog($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");
        DebugLog($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");

        if (PlayerStatisticsManager.Instance != null)
        {
            DebugLog($"PlayerStatisticsManager IsInitialized: {PlayerStatisticsManager.Instance.IsInitialized()}");
        }

        var foundManager = FindObjectOfType<PlayerStatisticsManager>();
        DebugLog($"PlayerStatisticsManager found in scene: {foundManager != null}");

        if (foundManager != null)
        {
            DebugLog($"Found GameObject: {foundManager.gameObject.name}");
            DebugLog($"GameObject active: {foundManager.gameObject.activeInHierarchy}");
        }
    }

    [ContextMenu("Force Stats Refresh")]
    public void ForceStatsRefresh()
    {
        DebugLog("=== FORCE STATS REFRESH ===");

        if (PlayerStatisticsManager.Instance != null)
        {
            PlayerStatisticsManager.Instance.LoadAndDisplayStats();
            DebugLog("✅ Forced stats refresh via Instance");
        }
        else
        {
            var foundManager = FindObjectOfType<PlayerStatisticsManager>();
            if (foundManager != null)
            {
                foundManager.LoadAndDisplayStats();
                DebugLog("✅ Forced stats refresh via found manager");
            }
            else
            {
                DebugLog("❌ No PlayerStatisticsManager available for refresh");
            }
        }
    }

    [ContextMenu("Test Stats Manager")]
    public void TestStatsManager()
    {
        DebugLog("=== TESTING STATS MANAGER ===");

        if (PlayerStatisticsManager.Instance != null)
        {
            DebugLog("Testing win update...");
            PlayerStatisticsManager.Instance.UpdateMatchResult(true, () => {
                DebugLog("✅ Test win update completed");
            });
        }
        else
        {
            DebugLog("❌ Cannot test - PlayerStatisticsManager.Instance is null");
        }
    }

    [ContextMenu("Initialize Stats Manager")]
    public void InitializeStatsManager()
    {
        DebugLog("=== MANUAL STATS MANAGER INITIALIZATION ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ PlayFab not logged in - cannot initialize stats");
            return;
        }

        if (PlayerStatisticsManager.Instance != null)
        {
            DebugLog("Reinitializing existing PlayerStatisticsManager...");
            PlayerStatisticsManager.Instance.ManualInitialize();
        }
        else
        {
            var foundManager = FindObjectOfType<PlayerStatisticsManager>();
            if (foundManager != null)
            {
                DebugLog("Setting found manager as Instance and initializing...");
                PlayerStatisticsManager.Instance = foundManager;
                foundManager.ManualInitialize();
            }
            else
            {
                DebugLog("❌ No PlayerStatisticsManager found to initialize");
            }
        }
    }
    #endregion

    private void DebugLog(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[MainMenu] {message}");
        }
    }
}