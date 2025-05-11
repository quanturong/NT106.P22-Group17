using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // Gán các panel qua Inspector
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject rulePanel;
    public GameObject statsPanel;

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
