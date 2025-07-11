using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayFab;
using Photon.Pun;
using System;

public class MainMenu : MonoBehaviour
{
    // Gán các panel qua Inspector
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject rulePanel;
    public GameObject statsPanel;
    private bool isLoggingOut = false;
    void Start()
    {
        Debug.Log("MainMenu script started");
        ShowMain();
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
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(true);

        if (PlayerStatisticsManager.Instance != null)
            PlayerStatisticsManager.Instance.LoadAndDisplayStats();
    }
    public void OnLogout()
    {
        if (isLoggingOut) return; // tránh double click
        isLoggingOut = true;

        Debug.Log("=== LOGGING OUT FROM MAIN MENU ===");

        // Xoá thông tin PlayFab
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey("PlayFabID");
        PlayerPrefs.DeleteKey("DisplayName");

        // Ngắt kết nối Photon nếu đang kết nối
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
}
