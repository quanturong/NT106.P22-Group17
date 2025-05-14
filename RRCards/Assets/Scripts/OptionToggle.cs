using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Các panel UI")]
    public GameObject mainPanel;
    public GameObject optionPanel;


    public void ShowOption()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionPanel != null) optionPanel.SetActive(true);
        Debug.Log("Option panel hiển thị.");
    }

    public void ShowMain()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionPanel != null) optionPanel.SetActive(false);
        Debug.Log("Main panel hiển thị.");
    }

    public void ShowStart()
    {
        SceneManager.LoadScene("Lobby");

    }
}
