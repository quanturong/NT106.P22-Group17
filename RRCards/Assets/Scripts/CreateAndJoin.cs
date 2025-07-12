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

    [Header("Debug")]
    public bool showDebugLogs = true;

    private bool isReady = false;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private bool isSettingUpRoom = false;
    private readonly string[] CRITICAL_PROPERTIES = {
        "IsReady", "AvatarIndex", "PlayerRole", "IsHost",
        "TeamId", "Score", "GameState", "PlayerIndex",
        "CharacterSelected", "LoadingComplete", "GameReady",
        "InGame", "HasJoinedGame", "GamePosition"
    };

    void Start()
    {
        if (showDebugLogs)
            Debug.Log("=== CREATE AND JOIN STARTED ===");

        SetButtonsInteractable(false);
        UpdateStatusText("Checking connection...");
        StartCoroutine(InitializeLobbyState());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugCurrentStatus();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugPlayerProperties();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ForceCleanAllProperties();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            DebugAllPlayersReadyState();
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            ForceFixReadyState();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            TestReadyState();
        }
    }

    #region Initialization
    private IEnumerator InitializeLobbyState()
    {
        if (showDebugLogs)
            Debug.Log("=== INITIALIZING LOBBY STATE ===");
        yield return StartCoroutine(CompletePropertyCleanup());
        yield return new WaitForSeconds(0.5f);
        CheckPhotonStatus();
    }

    private IEnumerator CompletePropertyCleanup()
    {
        if (showDebugLogs)
            Debug.Log("=== COMPLETE PROPERTY CLEANUP ===");
        if (PhotonNetwork.InRoom)
        {
            if (showDebugLogs)
                Debug.Log("Still in room, leaving...");

            PhotonNetwork.LeaveRoom();

            float timeout = 3f;
            float timer = 0f;
            while (PhotonNetwork.InRoom && timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.2f);
        ForceCleanAllProperties();

        yield return new WaitForSeconds(0.3f);

        if (showDebugLogs)
            Debug.Log("=== PROPERTY CLEANUP COMPLETE ===");
    }

    private void CheckPhotonStatus()
    {
        if (PlayfabAuthManager.Instance != null && PlayfabAuthManager.Instance.IsPhotonReady())
        {
            if (showDebugLogs)
                Debug.Log("Photon is ready!");
            OnPhotonReady();
        }
        else
        {
            if (showDebugLogs)
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

        UpdateStatusText("Connection failed. Please restart.");
        if (showDebugLogs)
            Debug.LogError("Photon connection timeout!");
    }

    private void OnPhotonReady()
    {
        if (showDebugLogs)
            Debug.Log("=== PHOTON IS READY FOR ROOM OPERATIONS ===");
        ForceCleanAllProperties();

        isReady = true;
        SetButtonsInteractable(true);
        UpdateStatusText("Ready to play!");

        if (showDebugLogs)
        {
            Debug.Log("=== LOBBY STATE INITIALIZED ===");
            DebugPlayerProperties();
        }
    }
    #endregion

    #region Property Management
    private void ForceCleanAllProperties()
    {
        if (PhotonNetwork.LocalPlayer == null) return;

        if (showDebugLogs)
            Debug.Log("=== FORCE CLEANING ALL PROPERTIES ===");
        var currentProps = PhotonNetwork.LocalPlayer.CustomProperties;
        var keysToRemove = new List<string>();

        foreach (var key in currentProps.Keys)
        {
            keysToRemove.Add(key.ToString());
        }
        ExitGames.Client.Photon.Hashtable clearProps = new ExitGames.Client.Photon.Hashtable();
        foreach (string key in keysToRemove)
        {
            clearProps[key] = null;
        }
        foreach (string criticalProp in CRITICAL_PROPERTIES)
        {
            clearProps[criticalProp] = null;
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(clearProps);

        if (showDebugLogs)
            Debug.Log($"Cleaned {keysToRemove.Count} existing + {CRITICAL_PROPERTIES.Length} critical properties");
    }

    private IEnumerator VerifyCleanState()
    {
        yield return new WaitForSeconds(0.2f);

        if (PhotonNetwork.LocalPlayer != null && showDebugLogs)
        {
            var remainingProps = PhotonNetwork.LocalPlayer.CustomProperties;
            Debug.Log($"Verification: {remainingProps.Count} properties remaining");

            foreach (var prop in remainingProps)
            {
                if (prop.Value != null)
                    Debug.LogWarning($"Property still exists: {prop.Key} = {prop.Value}");
            }
        }
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        if (showDebugLogs)
            Debug.Log("=== CONNECTED TO MASTER ===");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (showDebugLogs)
            Debug.Log("=== JOINED LOBBY ===");
        StartCoroutine(OnJoinedLobbyCleanup());
    }

    private IEnumerator OnJoinedLobbyCleanup()
    {
        ForceCleanAllProperties();
        yield return new WaitForSeconds(0.3f);
        OnPhotonReady();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (showDebugLogs)
            Debug.Log($"=== ROOM LIST UPDATED: {roomList.Count} rooms ===");

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
                if (showDebugLogs)
                    Debug.Log($"Room removed: {info.Name}");
            }
            else
            {
                cachedRoomList[info.Name] = info;
                if (showDebugLogs)
                    Debug.Log($"Room updated: {info.Name} ({info.PlayerCount}/{info.MaxPlayers})");
            }
        }
    }

    public override void OnCreatedRoom()
    {
        if (showDebugLogs)
        {
            Debug.Log($"=== ROOM CREATED SUCCESSFULLY ===");
            Debug.Log($"Room name: {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"Max players: {PhotonNetwork.CurrentRoom.MaxPlayers}");
            Debug.Log("Creator will have IsReady = false initially");
        }
        UpdateStatusText($"Room '{PhotonNetwork.CurrentRoom.Name}' created!");
    }

    public override void OnJoinedRoom()
    {
        if (showDebugLogs)
        {
            Debug.Log("=== JOINED ROOM SUCCESSFULLY ===");
            Debug.Log($"Room: {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
            Debug.Log("Starting SAFE room setup...");
        }

        if (!isSettingUpRoom)
        {
            isSettingUpRoom = true;
            StartCoroutine(SafeRoomSetup());
        }
    }

    private IEnumerator SafeRoomSetup()
    {
        if (showDebugLogs)
            Debug.Log("=== SAFE ROOM SETUP STARTED ===");
        yield return new WaitForSeconds(0.5f);
        ForceCleanAllProperties();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(SetPlayerReadyStateCoroutine());
        yield return new WaitForSeconds(0.5f);
        if (showDebugLogs)
        {
            DebugAllPlayersReadyState();
            Debug.Log($"CanLoadGame: {CanLoadGame()}");
        }
        UpdateStatusText("Joined room! Loading game...");
        PhotonNetwork.LoadLevel("Room");

        isSettingUpRoom = false;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (showDebugLogs)
        {
            Debug.LogError($"=== CREATE ROOM FAILED ===");
            Debug.LogError($"Code: {returnCode}, Message: {message}");
        }
        UpdateStatusText($"Failed to create room: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (showDebugLogs)
        {
            Debug.LogError($"=== JOIN ROOM FAILED ===");
            Debug.LogError($"Code: {returnCode}, Message: {message}");
        }
        UpdateStatusText($"Failed to join room: {message}");
        ListAvailableRooms();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (showDebugLogs)
            Debug.LogError($"=== PHOTON DISCONNECTED: {cause} ===");

        isReady = false;
        isSettingUpRoom = false;
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

        if (showDebugLogs)
        {
            Debug.Log($"=== CREATING ROOM: {roomName} ===");
            Debug.Log("Properties before creating room:");
            DebugPlayerProperties();
        }

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

        if (showDebugLogs)
        {
            Debug.Log($"=== JOINING ROOM: {roomName} ===");
            Debug.Log("Properties before joining room:");
            DebugPlayerProperties();
        }

        UpdateStatusText($"Joining room '{roomName}'...");
        PhotonNetwork.JoinRoom(roomName);
    }

    private bool ValidateRoomOperation()
    {
        if (!isReady)
        {
            UpdateStatusText("Not ready. Please wait...");
            if (showDebugLogs)
                Debug.LogWarning("Room operation attempted while not ready");
            return false;
        }

        if (PlayfabAuthManager.Instance == null || !PlayfabAuthManager.Instance.IsPhotonReady())
        {
            UpdateStatusText("Network not ready");
            if (showDebugLogs)
                Debug.LogWarning("PlayfabAuthManager not ready for room operations");
            return false;
        }

        return true;
    }
    #endregion

    #region Ready State Management
    private IEnumerator SetPlayerReadyStateCoroutine()
    {
        if (showDebugLogs)
            Debug.Log("=== SETTING PLAYER READY STATE COROUTINE ===");
        yield return new WaitForSeconds(0.2f);

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        bool isCreator = PhotonNetwork.LocalPlayer.IsMasterClient;

        if (isCreator)
        {
            props["IsReady"] = false;
            props["PlayerRole"] = "Creator";
            props["PlayerIndex"] = 0;

            if (showDebugLogs)
                Debug.Log("=== SETTING CREATOR STATE: IsReady = FALSE ===");
        }
        else
        {
            props["IsReady"] = true;
            props["PlayerRole"] = "Joiner";
            props["PlayerIndex"] = 1;

            if (showDebugLogs)
                Debug.Log("=== SETTING JOINER STATE: IsReady = TRUE ===");
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        yield return new WaitForSeconds(0.3f);

        if (showDebugLogs)
        {
            Debug.Log($"Player ready state SET - IsMasterClient: {isCreator}, IsReady: {props["IsReady"]}");
            DebugPlayerProperties();
        }
    }
    public void SetCreatorReady()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient) return;

        if (showDebugLogs)
            Debug.Log("=== SETTING CREATOR AS READY ===");

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["IsReady"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (showDebugLogs)
            Debug.Log("=== CREATOR IS NOW READY ===");
    }
    public bool AreAllPlayersReady()
    {
        if (PhotonNetwork.PlayerList.Length < 2)
        {
            if (showDebugLogs)
                Debug.Log("AreAllPlayersReady: Not enough players");
            return false;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady"))
            {
                if (showDebugLogs)
                    Debug.Log($"AreAllPlayersReady: Player {player.NickName} missing IsReady property");
                return false;
            }

            bool isReady = (bool)player.CustomProperties["IsReady"];
            if (!isReady)
            {
                if (showDebugLogs)
                    Debug.Log($"AreAllPlayersReady: Player {player.NickName} not ready");
                return false;
            }
        }

        if (showDebugLogs)
            Debug.Log("AreAllPlayersReady: ALL PLAYERS READY!");
        return true;
    }

    public bool CanLoadGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            if (showDebugLogs)
                Debug.LogWarning("CanLoadGame: Not enough players");
            return false;
        }
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") ||
                !player.CustomProperties.ContainsKey("PlayerRole"))
            {
                if (showDebugLogs)
                    Debug.LogWarning($"CanLoadGame: Player {player.NickName} missing required properties");
                return false;
            }
        }

        if (showDebugLogs)
            Debug.Log("CanLoadGame: All checks passed!");
        return true;
    }
    #endregion

    #region Debug and Utility Methods
    public void ForceFixReadyState()
    {
        if (!PhotonNetwork.InRoom) return;

        if (showDebugLogs)
            Debug.Log("=== FORCE FIXING READY STATE ===");

        StartCoroutine(ForceFixReadyStateCoroutine());
    }

    private IEnumerator ForceFixReadyStateCoroutine()
    {
        ForceCleanAllProperties();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(SetPlayerReadyStateCoroutine());
        DebugAllPlayersReadyState();
    }

    public void DebugAllPlayersReadyState()
    {
        if (!showDebugLogs) return;

        Debug.Log("=== ALL PLAYERS READY STATE ===");
        Debug.Log($"Total players: {PhotonNetwork.PlayerList.Length}");

        foreach (var player in PhotonNetwork.PlayerList)
        {
            bool hasReady = player.CustomProperties.ContainsKey("IsReady");
            bool isReady = hasReady ? (bool)player.CustomProperties["IsReady"] : false;
            string role = player.CustomProperties.ContainsKey("PlayerRole") ?
                         player.CustomProperties["PlayerRole"].ToString() : "Unknown";

            Debug.Log($"Player: {player.NickName}, Role: {role}, HasReady: {hasReady}, IsReady: {isReady}, IsMaster: {player.IsMasterClient}");
        }

        Debug.Log($"AreAllPlayersReady: {AreAllPlayersReady()}");
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void TestReadyState()
    {
        if (Application.isPlaying && PhotonNetwork.InRoom)
        {
            Debug.Log("=== TESTING READY STATE ===");
            DebugAllPlayersReadyState();
            Debug.Log($"CanLoadGame: {CanLoadGame()}");
        }
    }

    private void DebugCurrentStatus()
    {
        if (showDebugLogs)
        {
            Debug.Log($"=== CURRENT STATUS ===");
            Debug.Log($"isReady: {isReady}");
            Debug.Log($"isSettingUpRoom: {isSettingUpRoom}");
            Debug.Log($"PhotonNetwork.IsConnected: {PhotonNetwork.IsConnected}");
            Debug.Log($"PhotonNetwork.InLobby: {PhotonNetwork.InLobby}");
            Debug.Log($"PhotonNetwork.InRoom: {PhotonNetwork.InRoom}");
            Debug.Log($"PhotonNetwork.NetworkClientState: {PhotonNetwork.NetworkClientState}");
            Debug.Log($"Available rooms: {cachedRoomList.Count}");

            if (PlayfabAuthManager.Instance != null)
            {
                PlayfabAuthManager.Instance.DebugStatus();
            }
        }
    }

    private void DebugPlayerProperties()
    {
        if (PhotonNetwork.LocalPlayer != null && showDebugLogs)
        {
            Debug.Log("=== CURRENT PLAYER PROPERTIES ===");
            var props = PhotonNetwork.LocalPlayer.CustomProperties;
            if (props.Count == 0)
            {
                Debug.Log("No properties found (CLEAN STATE)");
            }
            else
            {
                foreach (var prop in props)
                {
                    Debug.Log($"Key: {prop.Key}, Value: {prop.Value}");
                }
            }
        }
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
        if (showDebugLogs)
            Debug.Log($"Status: {message}");
    }

    public void ListAvailableRooms()
    {
        if (showDebugLogs)
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