using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;
    public Button createButton;
    public Button joinButton;
    public TextMeshProUGUI statusText;

    [Header("Room Settings")]
    public int maxPlayers = 2;

    private bool isReady = false;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    void Start()
    {
        Debug.Log("=== CREATE AND JOIN STARTED ===");

        // Disable buttons initially
        SetButtonsInteractable(false);
        UpdateStatusText("Checking connection...");

        // Check current Photon status
        CheckPhotonStatus();

        // Debug current state
        if (PlayfabAuthManager.Instance != null)
        {
            PlayfabAuthManager.Instance.DebugStatus();
        }
    }

    void Update()
    {
        // Debug key for status check
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugCurrentStatus();
        }
    }

    private void CheckPhotonStatus()
    {
        if (PlayfabAuthManager.Instance != null && PlayfabAuthManager.Instance.IsPhotonReady())
        {
            Debug.Log("Photon is ready!");
            OnPhotonReady();
        }
        else
        {
            Debug.Log("Waiting for Photon connection...");
            UpdateStatusText("Connecting to network...");
            StartCoroutine(WaitForPhotonConnection());
        }
    }

    private IEnumerator WaitForPhotonConnection()
    {
        float timeout = 15f;
        float timer = 0f;

        while (timer < timeout)
        {
            if (PlayfabAuthManager.Instance != null && PlayfabAuthManager.Instance.IsPhotonReady())
            {
                OnPhotonReady();
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Timeout
        UpdateStatusText("Connection failed. Please restart.");
        Debug.LogError("Photon connection timeout!");
    }

    private void OnPhotonReady()
    {
        Debug.Log("=== PHOTON IS READY FOR ROOM OPERATIONS ===");
        isReady = true;
        SetButtonsInteractable(true);
        UpdateStatusText("Ready to play!");
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("=== CONNECTED TO MASTER ===");
        Debug.Log("Connected to Master Region: " + PhotonNetwork.CloudRegion);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("=== JOINED LOBBY ===");
        OnPhotonReady();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"=== ROOM LIST UPDATED: {roomList.Count} rooms ===");

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
                Debug.Log($"Room removed: {info.Name}");
            }
            else
            {
                cachedRoomList[info.Name] = info;
                Debug.Log($"Room updated: {info.Name} ({info.PlayerCount}/{info.MaxPlayers})");
            }
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"=== ROOM CREATED SUCCESSFULLY ===");
        Debug.Log($"Room name: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Max players: {PhotonNetwork.CurrentRoom.MaxPlayers}");
        UpdateStatusText($"Room '{PhotonNetwork.CurrentRoom.Name}' created!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("=== JOINED ROOM SUCCESSFULLY ===");
        Debug.Log($"Room: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
        Debug.Log("Loading gameplay scene...");

        UpdateStatusText("Joined room! Loading game...");
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"=== CREATE ROOM FAILED ===");
        Debug.LogError($"Code: {returnCode}, Message: {message}");
        UpdateStatusText($"Failed to create room: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"=== JOIN ROOM FAILED ===");
        Debug.LogError($"Code: {returnCode}, Message: {message}");
        UpdateStatusText($"Failed to join room: {message}");

        // List available rooms for debugging
        ListAvailableRooms();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"=== PHOTON DISCONNECTED: {cause} ===");
        isReady = false;
        SetButtonsInteractable(false);
        UpdateStatusText($"Disconnected: {cause}");
    }
    #endregion

    #region Room Operations
    public void CreateRoom()
    {
        if (!ValidateRoomOperation())
            return;

        if (string.IsNullOrEmpty(input_Create.text))
        {
            UpdateStatusText("Please enter a room name");
            return;
        }

        string roomName = input_Create.text.Trim();
        Debug.Log($"=== CREATING ROOM: {roomName} ===");

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayers,
            IsVisible = true,
            IsOpen = true
        };

        UpdateStatusText($"Creating room '{roomName}'...");
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinRoom()
    {
        if (!ValidateRoomOperation())
            return;

        if (string.IsNullOrEmpty(input_Join.text))
        {
            UpdateStatusText("Please enter a room name");
            return;
        }

        string roomName = input_Join.text.Trim();
        Debug.Log($"=== JOINING ROOM: {roomName} ===");

        UpdateStatusText($"Joining room '{roomName}'...");
        PhotonNetwork.JoinRoom(roomName);
    }

    private bool ValidateRoomOperation()
    {
        if (!isReady)
        {
            UpdateStatusText("Not ready. Please wait...");
            Debug.LogWarning("Room operation attempted while not ready");
            return false;
        }

        if (PlayfabAuthManager.Instance == null || !PlayfabAuthManager.Instance.IsPhotonReady())
        {
            UpdateStatusText("Network not ready");
            Debug.LogWarning("PlayfabAuthManager not ready for room operations");
            return false;
        }

        return true;
    }
    #endregion

    #region UI Helpers
    private void SetButtonsInteractable(bool interactable)
    {
        if (createButton) createButton.interactable = interactable;
        if (joinButton) joinButton.interactable = interactable;
    }

    private void UpdateStatusText(string message)
    {
        if (statusText) statusText.text = message;
        Debug.Log($"Status: {message}");
    }

    public void ListAvailableRooms()
    {
        Debug.Log("=== AVAILABLE ROOMS ===");
        if (cachedRoomList.Count == 0)
        {
            Debug.Log("No rooms available");
            return;
        }

        foreach (var room in cachedRoomList.Values)
        {
            Debug.Log($"- {room.Name} ({room.PlayerCount}/{room.MaxPlayers})");
        }
    }

    private void DebugCurrentStatus()
    {
        Debug.Log($"=== CURRENT STATUS ===");
        Debug.Log($"isReady: {isReady}");
        Debug.Log($"PhotonNetwork.IsConnected: {PhotonNetwork.IsConnected}");
        Debug.Log($"PhotonNetwork.InLobby: {PhotonNetwork.InLobby}");
        Debug.Log($"PhotonNetwork.NetworkClientState: {PhotonNetwork.NetworkClientState}");
        Debug.Log($"Available rooms: {cachedRoomList.Count}");

        if (PlayfabAuthManager.Instance != null)
        {
            PlayfabAuthManager.Instance.DebugStatus();
        }
    }
    #endregion

    #region Scene Management
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("UI");
    }

    public void Logout()
    {
        if (PlayfabAuthManager.Instance != null)
        {
            PlayfabAuthManager.Instance.Logout();
        }

        SceneManager.LoadScene(0);
    }
    #endregion
}