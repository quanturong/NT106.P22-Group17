using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
public class UIManager : MonoBehaviourPunCallbacks
{
    [Header("Các panel UI")]
    public GameObject mainPanel;
    public GameObject optionPanel;
    public void ShowOption()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionPanel != null) optionPanel.SetActive(true);
    }
    public void ShowMain()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionPanel != null) optionPanel.SetActive(false);
    }
    public void ShowStart()
    {
        StartCoroutine(DisconnectAndGoToLobby());
    }
    public void QuitGame()
    {
        var roomManager = Object.FindFirstObjectByType<RoomManager>();

        bool shouldCountAsLoss = false;
        if (PhotonNetwork.InRoom && roomManager != null)
        {
            var liarBarGameManager = FindObjectOfType<LiarBarGameManager>();
            if (liarBarGameManager != null && liarBarGameManager.currentState != LiarBarGameManager.GameState.GameOver)
            {
                shouldCountAsLoss = true;
            }

            if (shouldCountAsLoss && PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(false);
                Debug.Log("[UIManager] Quit mid-game → Counted as LOSS");
            }

            roomManager.QuitGame();
        }
        else
        {
            ShowStart();        }
    }
    private IEnumerator DisconnectAndGoToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
                yield return null;
            }
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }
        SceneManager.LoadScene("Lobby");
    }
    public override void OnLeftRoom()
    {
    }
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        if (cause != Photon.Realtime.DisconnectCause.DisconnectByClientLogic)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}