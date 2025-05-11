using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Gán các panel qua Inspector
    public GameObject mainPanel;
    public GameObject startPanel;
    public GameObject optionPanel;
    public GameObject rulePanel;
    public GameObject statsPanel;
    public GameObject idPanel;
    public GameObject roomPanel;

    void Start()
    {
        Debug.Log("MainMenu script started");
        ShowMain();
    }

    public void ShowMain()
    {
        startPanel.SetActive(false);
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
        roomPanel.SetActive(false);
        idPanel.SetActive(false);
    }


    public void ShowId()
    {
        idPanel.SetActive(true);    
        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
        roomPanel.SetActive(false);
    }

    public void ShowRoom()
    {
        idPanel.SetActive(false);
        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void ShowStart()
    {
        startPanel.SetActive(true);
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
        roomPanel.SetActive(false);
        idPanel.SetActive(false);

    }

    public void ShowOption()
    {
        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
        rulePanel.SetActive(false);
        statsPanel.SetActive(false);
        roomPanel.SetActive(false);
        idPanel.SetActive(false);
    }

    public void ShowRule()
    {
        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(true);
        statsPanel.SetActive(false);
    }

    public void ShowStats()
    {
        startPanel.SetActive(false);
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        rulePanel.SetActive(false);
        statsPanel.SetActive(true);
        roomPanel.SetActive(false);
        idPanel.SetActive(false);
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
