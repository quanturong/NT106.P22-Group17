using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStatisticsManager : MonoBehaviour
{
    public static PlayerStatisticsManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI winsText;
    public TextMeshProUGUI lossesText;

    [Header("Debug Settings")]
    public bool enableDebugLogs = true;
    public bool autoInitializeOnStart = true;

    private bool isInitialized = false;
    private bool isUpdatingStats = false;

    #region Unity Lifecycle
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DebugLog("PlayerStatisticsManager Instance created and marked as DontDestroyOnLoad");
        }
        else
        {
            DebugLog("Duplicate PlayerStatisticsManager destroyed");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        DebugLog("PlayerStatisticsManager Start() called");

        if (autoInitializeOnStart)
        {
            StartCoroutine(DelayedInitialization());
        }
    }

    private IEnumerator DelayedInitialization()
    {
        DebugLog("Waiting for PlayFab login to complete...");
        float timeout = 15f;
        float timer = 0f;

        while (!PlayFabClientAPI.IsClientLoggedIn() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.5f);        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("✅ PlayFab login detected, initializing statistics...");
            yield return new WaitForSeconds(1f);            InitializeStatisticsIfNeeded();
        }
        else
        {
            DebugLog("❌ PlayFab login timeout - statistics initialization skipped");
            DebugLog("You can manually initialize by calling InitializeStatisticsIfNeeded() after login");
        }
    }
    #endregion

    #region Statistics Initialization
    public void InitializeStatisticsIfNeeded()
    {
        if (isInitialized)
        {
            DebugLog("Statistics already initialized, loading current stats...");
            LoadAndDisplayStats();
            return;
        }

        DebugLog("=== INITIALIZING STATISTICS ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ PlayFab not logged in - cannot initialize statistics");
            return;
        }

        DebugLog("Checking existing player statistics...");

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            DebugLog($"✅ Successfully retrieved {result.Statistics.Count} existing statistics");

            foreach (var stat in result.Statistics)
            {
                DebugLog($"   Found: {stat.StatisticName} = {stat.Value}");
            }

            bool hasTotal = result.Statistics.Any(s => s.StatisticName == "TotalGames");
            bool hasWins = result.Statistics.Any(s => s.StatisticName == "Wins");
            bool hasLosses = result.Statistics.Any(s => s.StatisticName == "Losses");

            DebugLog($"Statistics check - TotalGames: {hasTotal}, Wins: {hasWins}, Losses: {hasLosses}");

            if (!hasTotal || !hasWins || !hasLosses)
            {
                DebugLog("⚠️ Some statistics missing - auto-creating them...");
                AutoCreateMissingStatistics();
            }
            else
            {
                DebugLog("✅ All required statistics exist");
                isInitialized = true;
                LoadAndDisplayStats();
            }
        }, error =>
        {
            DebugLog($"❌ Failed to get existing statistics: {error.ErrorMessage}");
            DebugLog($"Error details: {error.GenerateErrorReport()}");
            DebugLog("Attempting to auto-create statistics despite error...");
            AutoCreateMissingStatistics();
        });
    }

    private void AutoCreateMissingStatistics()
    {
        DebugLog("=== AUTO-CREATING MISSING STATISTICS ===");

        var initialStats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "TotalGames", Value = 0 },
            new StatisticUpdate { StatisticName = "Wins", Value = 0 },
            new StatisticUpdate { StatisticName = "Losses", Value = 0 }
        };

        DebugLog("Sending auto-create request to PlayFab...");

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = initialStats
        }, result =>
        {
            DebugLog("✅ Successfully auto-created missing statistics!");
            DebugLog("PlayFab automatically created statistic definitions");
            DebugLog("All required statistics (TotalGames, Wins, Losses) are now available");

            isInitialized = true;
            StartCoroutine(DelayedLoadStats(2f));
        }, error =>
        {
            DebugLog($"❌ Failed to auto-create statistics: {error.ErrorMessage}");
            DebugLog($"Full error: {error.GenerateErrorReport()}");

            if (error.ErrorMessage.Contains("not found") || error.ErrorMessage.Contains("not configured"))
            {
                DebugLog("💡 MANUAL SETUP REQUIRED:");
                DebugLog("   Statistics must be manually created in PlayFab Dashboard");
                DebugLog("   Go to: Economy > Statistics (or use the 'Force Manual Setup' context menu)");
            }
            else if (error.ErrorMessage.Contains("permission") || error.ErrorMessage.Contains("access"))
            {
                DebugLog("💡 PERMISSION ISSUE:");
                DebugLog("   Enable 'Allow client to post player statistics' in PlayFab Settings > API Features");
            }
        });
    }

    private IEnumerator DelayedLoadStats(float delaySeconds)
    {
        DebugLog($"Waiting {delaySeconds} seconds before loading stats...");
        yield return new WaitForSeconds(delaySeconds);
        LoadAndDisplayStats();
    }
    #endregion

    #region Statistics Updates
    public void UpdateMatchResult(bool isWin, System.Action onComplete = null)
    {
        if (isUpdatingStats)
        {
            DebugLog("⚠️ Already updating stats, skipping duplicate request");
            onComplete?.Invoke();
            return;
        }

        DebugLog($"=== UPDATE MATCH RESULT: {(isWin ? "WIN" : "LOSS")} ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ Cannot update match result - PlayFab not logged in!");
            onComplete?.Invoke();
            return;
        }

        isUpdatingStats = true;

        DebugLog("Getting current statistics for update...");

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = result.Statistics.FirstOrDefault(s => s.StatisticName == "TotalGames")?.Value ?? 0;
            int wins = result.Statistics.FirstOrDefault(s => s.StatisticName == "Wins")?.Value ?? 0;
            int losses = result.Statistics.FirstOrDefault(s => s.StatisticName == "Losses")?.Value ?? 0;

            DebugLog($"📊 Current stats - Total: {total}, Wins: {wins}, Losses: {losses}");
            total++;
            if (isWin)
            {
                wins++;
                DebugLog($"🏆 Recording WIN - New totals will be: {total} games, {wins} wins, {losses} losses");
            }
            else
            {
                losses++;
                DebugLog($"💀 Recording LOSS - New totals will be: {total} games, {wins} wins, {losses} losses");
            }

            var updatedStats = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "TotalGames", Value = total },
                new StatisticUpdate { StatisticName = "Wins", Value = wins },
                new StatisticUpdate { StatisticName = "Losses", Value = losses }
            };

            DebugLog("Sending statistics update to PlayFab...");

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = updatedStats
            }, updateResult =>
            {
                DebugLog("✅ Match result updated successfully!");
                DebugLog($"📊 New stats confirmed - Total: {total}, Wins: {wins}, Losses: {losses}");

                isUpdatingStats = false;
                LoadAndDisplayStats();
                onComplete?.Invoke();
            }, error =>
            {
                DebugLog($"❌ Failed to update match result: {error.ErrorMessage}");
                DebugLog($"Full error: {error.GenerateErrorReport()}");

                isUpdatingStats = false;
                onComplete?.Invoke();
            });
        }, error =>
        {
            DebugLog($"❌ Failed to get current stats for update: {error.ErrorMessage}");
            DebugLog($"Full error: {error.GenerateErrorReport()}");

            isUpdatingStats = false;
            onComplete?.Invoke();
        });
    }
    #endregion

    #region Statistics Display
    public void LoadAndDisplayStats()
    {
        DebugLog("=== LOADING AND DISPLAYING STATS ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ Cannot load stats - PlayFab not logged in!");
            return;
        }

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = 0, wins = 0, losses = 0;

            DebugLog($"Retrieved {result.Statistics.Count} statistics for display:");

            foreach (var stat in result.Statistics)
            {
                DebugLog($"   {stat.StatisticName}: {stat.Value}");

                if (stat.StatisticName == "TotalGames") total = stat.Value;
                else if (stat.StatisticName == "Wins") wins = stat.Value;
                else if (stat.StatisticName == "Losses") losses = stat.Value;
            }

            DebugLog($"📊 Final display values - Total: {total}, Wins: {wins}, Losses: {losses}");
            UpdateUI(total, wins, losses);

        }, error =>
        {
            DebugLog($"❌ Failed to load stats for display: {error.ErrorMessage}");
            DebugLog($"Full error: {error.GenerateErrorReport()}");
        });
    }

    private void UpdateUI(int total, int wins, int losses)
    {
        if (totalGamesText != null)
        {
            totalGamesText.text = $"TOTAL GAMES: {total}";
            DebugLog($"Updated totalGamesText: {totalGamesText.text}");
        }
        else
        {
            DebugLog("⚠️ totalGamesText is NULL! Assign it in Inspector.");
        }

        if (winsText != null)
        {
            winsText.text = $"WINS: {wins}";
            DebugLog($"Updated winsText: {winsText.text}");
        }
        else
        {
            DebugLog("⚠️ winsText is NULL! Assign it in Inspector.");
        }

        if (lossesText != null)
        {
            lossesText.text = $"LOSSES: {losses}";
            DebugLog($"Updated lossesText: {lossesText.text}");
        }
        else
        {
            DebugLog("⚠️ lossesText is NULL! Assign it in Inspector.");
        }

        DebugLog("✅ UI update completed");
    }
    #endregion

    #region Public API Methods
    public void ManualInitialize()
    {
        DebugLog("Manual initialization requested");
        InitializeStatisticsIfNeeded();
    }
    public void RefreshDisplay()
    {
        DebugLog("Manual refresh requested");
        LoadAndDisplayStats();
    }
    public void ResetAllStatistics()
    {
        DebugLog("=== RESETTING ALL STATISTICS ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ Cannot reset - PlayFab not logged in!");
            return;
        }

        var resetStats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "TotalGames", Value = 0 },
            new StatisticUpdate { StatisticName = "Wins", Value = 0 },
            new StatisticUpdate { StatisticName = "Losses", Value = 0 }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = resetStats
        }, result =>
        {
            DebugLog("✅ All statistics reset to 0");
            LoadAndDisplayStats();
        }, error =>
        {
            DebugLog($"❌ Failed to reset statistics: {error.ErrorMessage}");
        });
    }
    #endregion

    #region Context Menu Debug Methods
    [ContextMenu("Auto Create Statistics")]
    public void AutoCreateStatistics()
    {
        DebugLog("=== MANUAL AUTO-CREATE STATISTICS ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ PlayFab not logged in!");
            return;
        }

        AutoCreateMissingStatistics();
    }

    [ContextMenu("Test PlayFab Connection")]
    public void TestPlayFabConnection()
    {
        DebugLog("=== TESTING PLAYFAB CONNECTION ===");

        DebugLog($"PlayFab IsClientLoggedIn: {PlayFabClientAPI.IsClientLoggedIn()}");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ PlayFab NOT logged in!");
            return;
        }

        DebugLog("✅ PlayFab is logged in - testing statistics API...");

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            result => {
                DebugLog($"✅ Successfully retrieved {result.Statistics.Count} statistics");
                foreach (var stat in result.Statistics)
                {
                    DebugLog($"   - {stat.StatisticName}: {stat.Value}");
                }
            },
            error => {
                DebugLog($"❌ Failed to get statistics: {error.ErrorMessage}");
                DebugLog($"Full error: {error.GenerateErrorReport()}");
            });
    }

    [ContextMenu("Force Create Test Stats")]
    public void ForceCreateTestStats()
    {
        DebugLog("=== FORCE CREATING TEST STATS ===");

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ PlayFab not logged in!");
            return;
        }

        var testStats = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "TotalGames", Value = 10 },
            new StatisticUpdate { StatisticName = "Wins", Value = 7 },
            new StatisticUpdate { StatisticName = "Losses", Value = 3 }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = testStats
        },
        result => {
            DebugLog("✅ Test statistics created successfully!");
            DebugLog("Test values: 10 total games, 7 wins, 3 losses");
            LoadAndDisplayStats();
        },
        error => {
            DebugLog($"❌ Failed to create test statistics: {error.ErrorMessage}");
            DebugLog($"Full error: {error.GenerateErrorReport()}");
        });
    }

    [ContextMenu("Debug UI References")]
    public void DebugUIReferences()
    {
        DebugLog("=== DEBUGGING UI REFERENCES ===");
        DebugLog($"totalGamesText assigned: {totalGamesText != null}");
        DebugLog($"winsText assigned: {winsText != null}");
        DebugLog($"lossesText assigned: {lossesText != null}");

        if (totalGamesText != null) DebugLog($"totalGamesText current text: '{totalGamesText.text}'");
        if (winsText != null) DebugLog($"winsText current text: '{winsText.text}'");
        if (lossesText != null) DebugLog($"lossesText current text: '{lossesText.text}'");

        DebugLog($"GameObject name: {gameObject.name}");
        DebugLog($"Is active: {gameObject.activeInHierarchy}");
        DebugLog($"Instance is correct: {Instance == this}");
    }

    [ContextMenu("Manual Refresh Stats")]
    public void ManualRefreshStats()
    {
        DebugLog("=== MANUAL REFRESH TRIGGERED ===");
        LoadAndDisplayStats();
    }

    [ContextMenu("Test Win")]
    public void TestWin()
    {
        DebugLog("=== TESTING WIN ===");
        UpdateMatchResult(true, () => DebugLog("Test win completed"));
    }

    [ContextMenu("Test Loss")]
    public void TestLoss()
    {
        DebugLog("=== TESTING LOSS ===");
        UpdateMatchResult(false, () => DebugLog("Test loss completed"));
    }

    [ContextMenu("Reset All Stats")]
    public void ContextMenuResetStats()
    {
        DebugLog("=== CONTEXT MENU RESET STATS ===");
        ResetAllStatistics();
    }

    [ContextMenu("Full Diagnostic")]
    public void FullDiagnostic()
    {
        DebugLog("=== FULL DIAGNOSTIC ===");
        DebugLog($"Instance exists: {Instance != null}");
        DebugLog($"Instance is this: {Instance == this}");
        DebugLog($"GameObject active: {gameObject.activeInHierarchy}");
        DebugLog($"Component enabled: {enabled}");
        DebugLog($"Is initialized: {isInitialized}");
        DebugLog($"Is updating stats: {isUpdatingStats}");
        DebugLog($"Auto initialize on start: {autoInitializeOnStart}");
        DebugLog($"Debug logs enabled: {enableDebugLogs}");

        TestPlayFabConnection();
        DebugUIReferences();
    }
    #endregion

    #region Utility Methods
    private void DebugLog(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[PlayerStatisticsManager] {message}");
        }
    }
    public void GetCurrentStats(System.Action<int, int, int> onComplete)
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            DebugLog("❌ Cannot get stats - PlayFab not logged in!");
            onComplete?.Invoke(0, 0, 0);
            return;
        }

        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = result.Statistics.FirstOrDefault(s => s.StatisticName == "TotalGames")?.Value ?? 0;
            int wins = result.Statistics.FirstOrDefault(s => s.StatisticName == "Wins")?.Value ?? 0;
            int losses = result.Statistics.FirstOrDefault(s => s.StatisticName == "Losses")?.Value ?? 0;

            onComplete?.Invoke(total, wins, losses);
        }, error =>
        {
            DebugLog($"❌ Failed to get current stats: {error.ErrorMessage}");
            onComplete?.Invoke(0, 0, 0);
        });
    }
    public bool IsInitialized()
    {
        return isInitialized && PlayFabClientAPI.IsClientLoggedIn();
    }
    #endregion

    #region Cleanup
    private void OnDestroy()
    {
        DebugLog("PlayerStatisticsManager OnDestroy called");

        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            DebugLog("Application paused - statistics state preserved");
        }
        else
        {
            DebugLog("Application resumed - statistics ready");
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            DebugLog("Application lost focus");
        }
        else
        {
            DebugLog("Application gained focus");
        }
    }
    #endregion
}