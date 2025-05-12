using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    private bool isReady = false;

    void Start()
    {
        Debug.Log("Connecting to Photon...");

        PhotonNetwork.AutomaticallySyncScene = true; // Đồng bộ scene giữa các client
        PhotonNetwork.GameVersion = "1.0"; // Quan trọng: 2 máy phải giống GameVersion
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Region: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby. Ready to create or join rooms.");
        isReady = true;
    }

    public void CreateRoom()
    {
        if (!isReady)
        {
            Debug.LogWarning("Not ready to create room. Wait until joined lobby.");
            return;
        }

        if (string.IsNullOrEmpty(input_Create.text))
        {
            Debug.LogWarning("Room name is empty.");
            return;
        }

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.CreateRoom(input_Create.text, options);
    }

    public void JoinRoom()
    {
        if (!isReady)
        {
            Debug.LogWarning("Not ready to join room. Wait until joined lobby.");
            return;
        }

        if (string.IsNullOrEmpty(input_Join.text))
        {
            Debug.LogWarning("Room name is empty.");
            return;
        }

        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room. Loading GamePlay scene...");
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"CreateRoom failed (code {returnCode}): {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"JoinRoom failed (code {returnCode}): {message}");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("Disconnected from Photon: " + cause);
        isReady = false;
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("UI");
    }
}