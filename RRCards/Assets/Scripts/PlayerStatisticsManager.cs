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

    public TextMeshProUGUI totalGamesText;
    public TextMeshProUGUI winsText;
    public TextMeshProUGUI lossesText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Gọi khi login xong để tạo thống kê nếu chưa có
    public void InitializeStatisticsIfNeeded()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            bool hasTotal = result.Statistics.Any(s => s.StatisticName == "TotalGames");
            bool hasWins = result.Statistics.Any(s => s.StatisticName == "Wins");
            bool hasLosses = result.Statistics.Any(s => s.StatisticName == "Losses");

            if (!hasTotal || !hasWins || !hasLosses)
            {
                PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate { StatisticName = "TotalGames", Value = 0 },
                    new StatisticUpdate { StatisticName = "Wins", Value = 0 },
                    new StatisticUpdate { StatisticName = "Losses", Value = 0 }
                }
                }, result =>
                {
                    Debug.Log("Initialized missing statistics.");
                    StartCoroutine(DelayedLoadStats(2f)); // <-- đợi PlayFab cập nhật rồi mới load
                }, error => Debug.LogError("Failed to init stats: " + error.GenerateErrorReport()));
            }
            else
            {
                LoadAndDisplayStats(); // Nếu đã tồn tại thì load ngay
            }
        }, error => Debug.LogError("Init Stats Failed: " + error.GenerateErrorReport()));
    }
    private IEnumerator DelayedLoadStats(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        LoadAndDisplayStats();
    }

    // Gọi sau mỗi trận thắng/thua
    public void UpdateMatchResult(bool isWin, System.Action onComplete = null)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = result.Statistics.FirstOrDefault(s => s.StatisticName == "TotalGames")?.Value ?? 0;
            int wins = result.Statistics.FirstOrDefault(s => s.StatisticName == "Wins")?.Value ?? 0;
            int losses = result.Statistics.FirstOrDefault(s => s.StatisticName == "Losses")?.Value ?? 0;

            total++;
            if (isWin) wins++;
            else losses++;

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "TotalGames", Value = total },
                new StatisticUpdate { StatisticName = "Wins", Value = wins },
                new StatisticUpdate { StatisticName = "Losses", Value = losses }
            }
            }, updateResult =>
            {
                Debug.Log("Statistics updated successfully.");
                LoadAndDisplayStats();
                onComplete?.Invoke(); // callback thành công
            }, error =>
            {
                Debug.LogError("Update Stats Failed: " + error.GenerateErrorReport());
                onComplete?.Invoke(); // vẫn gọi để không bị kẹt
            });
        }, error =>
        {
            Debug.LogError("Get Stats Failed: " + error.GenerateErrorReport());
            onComplete?.Invoke();
        });
    }

    // Load từ PlayFab và gán vào UI
    public void LoadAndDisplayStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = 0, wins = 0, losses = 0;

            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "TotalGames") total = stat.Value;
                else if (stat.StatisticName == "Wins") wins = stat.Value;
                else if (stat.StatisticName == "Losses") losses = stat.Value;
            }

            if (totalGamesText != null) totalGamesText.text = $"TOTAL GAMES: {total}";
            if (winsText != null) winsText.text = $"WINS: {wins}";
            if (lossesText != null) lossesText.text = $"LOSES: {losses}";
        }, error => Debug.LogError("Load Stats Failed: " + error.GenerateErrorReport()));
    }
}
