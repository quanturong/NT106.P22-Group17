using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class PlayerStatisticsManager : MonoBehaviour
{
    public static PlayerStatisticsManager Instance;

    public TMP_Text totalGamesText;
    public TMP_Text winsText;
    public TMP_Text lossesText;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void InitializeStatisticsIfNeeded()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            bool hasStats = result.Statistics.Any(s => s.StatisticName == "TotalGames");
            if (!hasStats)
            {
                PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
                {
                    Statistics = new List<StatisticUpdate>
                    {
                        new StatisticUpdate { StatisticName = "TotalGames", Value = 0 },
                        new StatisticUpdate { StatisticName = "Wins", Value = 0 },
                        new StatisticUpdate { StatisticName = "Losses", Value = 0 }
                    }
                }, null, null);
            }
        }, error => Debug.LogError("Init Stats Failed: " + error.GenerateErrorReport()));
    }

    public void UpdateMatchResult(bool isWin)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            int total = result.Statistics.FirstOrDefault(s => s.StatisticName == "TotalGames")?.Value ?? 0;
            int wins = result.Statistics.FirstOrDefault(s => s.StatisticName == "Wins")?.Value ?? 0;
            int losses = result.Statistics.FirstOrDefault(s => s.StatisticName == "Losses")?.Value ?? 0;

            total++;
            if (isWin) wins++; else losses++;

            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate { StatisticName = "TotalGames", Value = total },
                    new StatisticUpdate { StatisticName = "Wins", Value = wins },
                    new StatisticUpdate { StatisticName = "Losses", Value = losses }
                }
            }, null, null);
        }, error => Debug.LogError("Update Stats Failed: " + error.GenerateErrorReport()));
    }

    public void LoadAndDisplayStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result =>
        {
            foreach (var stat in result.Statistics)
            {
                switch (stat.StatisticName)
                {
                    case "TotalGames": totalGamesText.text = $"Total games: {stat.Value}"; break;
                    case "Wins": winsText.text = $"Wins: {stat.Value}"; break;
                    case "Losses": lossesText.text = $"Loses: {stat.Value}"; break;
                }
            }
        }, error => Debug.LogError("Load Stats Failed: " + error.GenerateErrorReport()));
    }
}