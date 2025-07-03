using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayfabAuthManager : MonoBehaviourPunCallbacks
{
    public static PlayfabAuthManager Instance;

    [Header("Events")]
    public Action<string> OnLoginSuccess;
    public Action<string> OnLoginFailed;
    public Action<string> OnRegisterSuccess;
    public Action<string> OnRegisterFailed;

    private bool isPhotonConnected = false;
    private bool isInLobby = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePhoton();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("=== INITIALIZING PHOTON CONNECTION ===");
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1.0";
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("=== PHOTON ALREADY CONNECTED ===");
            isPhotonConnected = true;
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            else
            {
                isInLobby = true;
            }
        }
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("=== PHOTON CONNECTED TO MASTER ===");
        Debug.Log("Connected to Master Region: " + PhotonNetwork.CloudRegion);
        isPhotonConnected = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("=== PHOTON JOINED LOBBY ===");
        Debug.Log("Photon ready for room operations!");
        isInLobby = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"=== PHOTON DISCONNECTED: {cause} ===");
        isPhotonConnected = false;
        isInLobby = false;

        // Auto reconnect after 3 seconds
        Invoke("ReconnectPhoton", 3f);
    }

    private void ReconnectPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("=== ATTEMPTING PHOTON RECONNECTION ===");
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region PlayFab Authentication
    public void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserAccountInfo = true,
                GetPlayerProfile = true,
                GetUserData = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginPlayFabSuccess, OnLoginPlayFabError);
    }

    private void OnLoginPlayFabSuccess(LoginResult result)
    {
        Debug.Log("=== PLAYFAB LOGIN SUCCESSFUL ===");

        // Set display name for Photon
        string displayName = result.InfoResultPayload.PlayerProfile?.DisplayName ??
                           result.InfoResultPayload.AccountInfo?.Username ??
                           "Player_" + result.PlayFabId.Substring(0, 6);

        PhotonNetwork.NickName = displayName;
        Debug.Log($"Photon Nickname set to: {displayName}");

        // Store player data
        PlayerPrefs.SetString("PlayFabID", result.PlayFabId);
        PlayerPrefs.SetString("DisplayName", displayName);
        PlayerPrefs.Save();

        OnLoginSuccess?.Invoke("Login successful!");
    }

    private void OnLoginPlayFabError(PlayFabError error)
    {
        Debug.LogError($"=== PLAYFAB LOGIN FAILED ===");
        Debug.LogError(error.GenerateErrorReport());
        OnLoginFailed?.Invoke(error.ErrorMessage);
    }

    public void Register(string email, string password, string username)
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = username,
            DisplayName = username,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterPlayFabSuccess, OnRegisterPlayFabError);
    }

    private void OnRegisterPlayFabSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("=== PLAYFAB REGISTRATION SUCCESSFUL ===");
        OnRegisterSuccess?.Invoke("Registration successful! You can now log in.");
    }

    private void OnRegisterPlayFabError(PlayFabError error)
    {
        Debug.LogError($"=== PLAYFAB REGISTRATION FAILED ===");
        Debug.LogError(error.GenerateErrorReport());
        OnRegisterFailed?.Invoke(error.ErrorMessage);
    }

    public void RecoverPassword(string email)
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email,
            TitleId = PlayFabSettings.staticSettings.TitleId
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnRecoveryError);
    }

    private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        Debug.Log("=== RECOVERY EMAIL SENT ===");
    }

    private void OnRecoveryError(PlayFabError error)
    {
        Debug.LogError($"=== RECOVERY EMAIL FAILED ===");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion

    #region Utility Functions
    public void UpdatePlayerDisplayName(string newDisplayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = newDisplayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnDisplayNameUpdateError);
    }

    private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"Display name updated to: {result.DisplayName}");
        PhotonNetwork.NickName = result.DisplayName;
        PlayerPrefs.SetString("DisplayName", result.DisplayName);
    }

    private void OnDisplayNameUpdateError(PlayFabError error)
    {
        Debug.LogError($"Failed to update display name: {error.GenerateErrorReport()}");
    }

    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        // Clear stored data
        PlayerPrefs.DeleteKey("PlayFabID");
        PlayerPrefs.DeleteKey("DisplayName");

        Debug.Log("=== LOGGED OUT FROM BOTH PLAYFAB AND PHOTON ===");
    }

    public bool IsAuthenticated()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }

    public bool IsPhotonReady()
    {
        return isPhotonConnected && PhotonNetwork.IsConnectedAndReady && isInLobby;
    }

    public string GetPlayFabId()
    {
        return PlayFabSettings.staticPlayer?.PlayFabId ?? PlayerPrefs.GetString("PlayFabID", "");
    }

    public string GetPhotonNickname()
    {
        return PhotonNetwork.NickName ?? PlayerPrefs.GetString("DisplayName", "Unknown");
    }

    public void DebugStatus()
    {
        Debug.Log($"=== AUTH MANAGER STATUS ===");
        Debug.Log($"PlayFab Authenticated: {IsAuthenticated()}");
        Debug.Log($"Photon Connected: {isPhotonConnected}");
        Debug.Log($"Photon In Lobby: {isInLobby}");
        Debug.Log($"Photon Ready: {IsPhotonReady()}");
        Debug.Log($"Photon State: {PhotonNetwork.NetworkClientState}");
        Debug.Log($"Nickname: {GetPhotonNickname()}");
        Debug.Log($"PlayFab ID: {GetPlayFabId()}");
    }
    #endregion
}