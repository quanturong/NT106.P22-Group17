using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("Player Name Display")]
    public TextMeshProUGUI localPlayerNameText;
    public TextMeshProUGUI opponentPlayerNameText;

    [Header("Game Management")]
    public GameObject[] flaskPrefabs;
    [SerializeField] private RectTransform[] leftSlots;
    [SerializeField] private RectTransform[] rightSlots;

    [Header("Game Result Scenes")]
    public string victorySceneName = "Scenes/Victory";
    public string defeatSceneName = "Scenes/Defeat";

    [Header("DEBUG - Scene Override")]
    public GameObject debugUIPanel;
    public TextMeshProUGUI debugText;

    private Player localPlayer;
    private Player opponentPlayer;
    private bool hasSpawned = false;
    private bool gameEnded = false;
    private bool isQuitting = false;
    private bool sceneAlreadyLoading = false;
    private bool quitGameProcessed = false;

    private const string QUIT_PROPERTY_KEY = "RoomManager_QuitPlayer";
    private const string QUIT_TIME_KEY = "RoomManager_QuitTime";

    void Start()
    {
        Debug.Log("=== ROOM MANAGER STARTED ===");
        localPlayer = PhotonNetwork.LocalPlayer;
        SetupPlayerNames();
        StartCoroutine(CheckAndSpawnCoroutine());

        // Setup debug UI
        if (debugUIPanel) debugUIPanel.SetActive(false);
    }

    private IEnumerator CheckAndSpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        CheckAndSpawn();
    }

    void CheckAndSpawn()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2 && !hasSpawned)
        {
            SpawnFlasksForAllPlayers();
        }
    }

    private void SetupPlayerNames()
    {
        if (localPlayerNameText)
        {
            string localName = localPlayer.NickName ?? "You";
            localPlayerNameText.text = localName;
        }
        UpdateOpponentName();
    }

    private void UpdateOpponentName()
    {
        opponentPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p != localPlayer);

        if (opponentPlayer != null)
        {
            if (opponentPlayerNameText)
            {
                string opponentName = opponentPlayer.NickName ?? "Opponent";
                opponentPlayerNameText.text = opponentName;
            }
        }
        else
        {
            if (opponentPlayerNameText)
            {
                opponentPlayerNameText.text = "Waiting...";
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateOpponentName();

        if (PhotonNetwork.IsMasterClient && !hasSpawned)
        {
            StartCoroutine(CheckAndSpawnCoroutine());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"*** OnPlayerLeftRoom CALLED ***");
        Debug.Log($"Player {otherPlayer.NickName} (ActorNumber: {otherPlayer.ActorNumber}) left the room");
        Debug.Log($"My state - gameEnded: {gameEnded}, isQuitting: {isQuitting}, sceneAlreadyLoading: {sceneAlreadyLoading}");

        if (gameEnded || sceneAlreadyLoading || isQuitting)
        {
            Debug.Log("*** IGNORING OnPlayerLeftRoom - Game ended/loading/quitting ***");
            return;
        }

        opponentPlayer = null;
        UpdateOpponentName();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("*** OPPONENT LEFT - CHECKING QUIT INFO ***");
            bool hasQuitInfo = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(QUIT_PROPERTY_KEY);
            Debug.Log($"Has quit info: {hasQuitInfo}");

            if (!hasQuitInfo)
            {
                Debug.Log("*** NO QUIT INFO - OPPONENT DISCONNECTED - I WIN ***");
                HandleVictory("Opponent disconnected unexpectedly");
            }
        }
    }

    public void QuitGame()
    {
        Debug.Log($"*** QuitGame() CALLED ***");
        Debug.Log($"Current state - gameEnded: {gameEnded}, isQuitting: {isQuitting}, sceneAlreadyLoading: {sceneAlreadyLoading}, quitGameProcessed: {quitGameProcessed}");

        if (gameEnded || isQuitting || quitGameProcessed)
        {
            Debug.Log("*** IGNORING QuitGame - Already processed ***");
            return;
        }

        Debug.Log($"=== QUIT GAME STARTED ===");
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.NickName} (ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}) is quitting the game!");

        quitGameProcessed = true;
        isQuitting = true;
        gameEnded = true;
        sceneAlreadyLoading = true;

        UpdateMatchResult(false);

        Debug.Log("*** SETTING ROOM PROPERTIES WITH UNIQUE KEYS ***");
        ExitGames.Client.Photon.Hashtable roomProps = new ExitGames.Client.Photon.Hashtable();
        roomProps[QUIT_PROPERTY_KEY] = PhotonNetwork.LocalPlayer.ActorNumber;
        roomProps[QUIT_TIME_KEY] = PhotonNetwork.Time;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);

        Debug.Log("*** SHOWING DEBUG UI INSTEAD OF LOADING SCENE ***");
        ShowDebugResult("DEFEAT", "I quit the game", Color.red);

        Debug.Log("*** STARTING DISCONNECT SEQUENCE ***");
        StartCoroutine(DisconnectSequence());
    }

    private IEnumerator DisconnectSequence()
    {
        yield return new WaitForSeconds(3.0f); // Longer wait for debugging

        Debug.Log("*** LEAVING ROOM ***");
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            while (PhotonNetwork.InRoom)
            {
                yield return null;
            }
            Debug.Log("*** LEFT ROOM SUCCESSFULLY ***");
        }

        yield return new WaitForSeconds(1.0f);

        Debug.Log("*** NOW LOADING ACTUAL DEFEAT SCENE ***");
        LoadSceneWithDebug(defeatSceneName, "I quit - final scene load");
    }

    private void HandleVictory(string reason)
    {
        if (gameEnded && sceneAlreadyLoading)
        {
            Debug.Log("*** VICTORY ALREADY HANDLED ***");
            return;
        }

        Debug.Log($"*** HANDLING VICTORY: {reason} ***");
        gameEnded = true;
        sceneAlreadyLoading = true;

        UpdateMatchResult(true);

        Debug.Log("*** SHOWING DEBUG UI INSTEAD OF LOADING SCENE ***");
        ShowDebugResult("VICTORY", reason, Color.green);

        StartCoroutine(LoadVictoryAfterDelay());
    }

    private IEnumerator LoadVictoryAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("*** NOW LOADING ACTUAL VICTORY SCENE ***");
        LoadSceneWithDebug(victorySceneName, "Victory - final scene load");
    }

    private void ShowDebugResult(string result, string reason, Color color)
    {
        if (debugUIPanel)
        {
            debugUIPanel.SetActive(true);
        }

        if (debugText)
        {
            debugText.text = $"RESULT: {result}\nREASON: {reason}\nACTOR: {PhotonNetwork.LocalPlayer.ActorNumber}\nMASTER: {PhotonNetwork.IsMasterClient}";
            debugText.color = color;
        }

        Debug.Log($"*** DEBUG RESULT SHOWN: {result} - {reason} ***");
    }

    private void HandleDefeat(string reason)
    {
        if (gameEnded && sceneAlreadyLoading)
        {
            Debug.Log("*** DEFEAT ALREADY HANDLED ***");
            return;
        }

        Debug.Log($"*** HANDLING DEFEAT: {reason} ***");
        gameEnded = true;
        sceneAlreadyLoading = true;

        ShowDebugResult("DEFEAT", reason, Color.red);
        StartCoroutine(LoadDefeatAfterDelay());
    }

    private IEnumerator LoadDefeatAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("*** NOW LOADING ACTUAL DEFEAT SCENE ***");
        LoadSceneWithDebug(defeatSceneName, "Defeat - final scene load");
    }

    private IEnumerator LeaveRoomAfterResult()
    {
        yield return new WaitForSeconds(0.5f);

        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Leaving room after result scene loaded...");
            PhotonNetwork.LeaveRoom();
        }
    }

    void UpdateMatchResult(bool isWin)
    {
        Debug.Log($"*** UpdateMatchResult: {(isWin ? "WIN" : "LOSE")} ***");

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"LastMatchResult", isWin ? "Win" : "Lose"}
            }
        };
        PlayFabClientAPI.UpdateUserData(request,
            result => Debug.Log("PlayFab result updated: " + (isWin ? "Win" : "Lose")),
            error => Debug.Log("PlayFab error: " + error.GenerateErrorReport()));
    }

    void SpawnFlasksForAllPlayers()
    {
        if (hasSpawned) return;

        hasSpawned = true;

        List<FlaskSpawnData> spawnData = GenerateSpawnData();

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["FlaskData"] = JsonUtility.ToJson(new FlaskDataWrapper { flasks = spawnData.ToArray() });
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        SpawnFromData(spawnData);
    }

    List<FlaskSpawnData> GenerateSpawnData()
    {
        List<FlaskSpawnData> data = new List<FlaskSpawnData>();

        Random.InitState(System.DateTime.Now.Millisecond + PhotonNetwork.LocalPlayer.ActorNumber);

        int maxSlots = Mathf.Min(leftSlots.Length, rightSlots.Length);

        int flaskCountA = Random.Range(1, Mathf.Min(5, maxSlots + 1));
        int flaskCountB = Random.Range(1, Mathf.Min(5, maxSlots + 1));

        List<int> usedSlotsA = new List<int>();
        for (int i = 0; i < flaskCountA; i++)
        {
            int nextSlot = GetNextAvailableSlot(usedSlotsA, maxSlots);
            if (nextSlot != -1)
            {
                usedSlotsA.Add(nextSlot);
                data.Add(new FlaskSpawnData
                {
                    slotIndex = nextSlot,
                    isPlayerA = true,
                    flaskIndex = Random.Range(0, flaskPrefabs.Length)
                });
            }
        }

        List<int> usedSlotsB = new List<int>();
        for (int i = 0; i < flaskCountB; i++)
        {
            int nextSlot = GetNextAvailableSlot(usedSlotsB, maxSlots);
            if (nextSlot != -1)
            {
                usedSlotsB.Add(nextSlot);
                data.Add(new FlaskSpawnData
                {
                    slotIndex = nextSlot,
                    isPlayerA = false,
                    flaskIndex = Random.Range(0, flaskPrefabs.Length)
                });
            }
        }

        return data;
    }

    int GetNextAvailableSlot(List<int> usedSlots, int maxSlots)
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (!usedSlots.Contains(i))
            {
                return i;
            }
        }
        return -1;
    }

    void SpawnFromData(List<FlaskSpawnData> spawnData)
    {
        ClearAllFlasks();

        bool isPlayerA = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        foreach (var data in spawnData)
        {
            RectTransform[] targetSlots;

            if (data.isPlayerA)
            {
                if (isPlayerA)
                {
                    targetSlots = leftSlots;
                }
                else
                {
                    targetSlots = rightSlots;
                }
            }
            else
            {
                if (isPlayerA)
                {
                    targetSlots = rightSlots;
                }
                else
                {
                    targetSlots = leftSlots;
                }
            }

            if (data.slotIndex < targetSlots.Length)
            {
                SpawnFlaskAtSlot(targetSlots[data.slotIndex], data.flaskIndex);
            }
        }
    }

    void SpawnFlaskAtSlot(RectTransform targetSlot, int flaskIndex)
    {
        if (flaskIndex >= flaskPrefabs.Length) return;

        GameObject selectedFlask = flaskPrefabs[flaskIndex];
        GameObject flaskInstance = Instantiate(selectedFlask, targetSlot);

        RectTransform flaskRect = flaskInstance.GetComponent<RectTransform>();
        flaskRect.anchoredPosition = Vector2.zero;
        flaskRect.localScale = Vector3.one;

        Animator anim = flaskInstance.GetComponent<Animator>();
        if (anim != null)
            anim.Play("FlaskPopIn");
    }

    void ClearAllFlasks()
    {
        foreach (var slot in leftSlots)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(slot.GetChild(i).gameObject);
            }
        }

        foreach (var slot in rightSlots)
        {
            for (int i = slot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(slot.GetChild(i).gameObject);
            }
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("*** OnRoomPropertiesUpdate CALLED ***");
        Debug.Log($"Properties changed: {string.Join(", ", propertiesThatChanged.Keys)}");
        Debug.Log($"My state - gameEnded: {gameEnded}, isQuitting: {isQuitting}, sceneAlreadyLoading: {sceneAlreadyLoading}");

        if (propertiesThatChanged.ContainsKey("FlaskData") && !hasSpawned)
        {
            Debug.Log("*** HANDLING FLASK DATA ***");
            hasSpawned = true;
            string jsonData = (string)propertiesThatChanged["FlaskData"];
            FlaskDataWrapper wrapper = JsonUtility.FromJson<FlaskDataWrapper>(jsonData);
            SpawnFromData(wrapper.flasks.ToList());
        }

        if (propertiesThatChanged.ContainsKey(QUIT_PROPERTY_KEY))
        {
            Debug.Log("*** ROOMMANAGER QUIT PROPERTY FOUND ***");

            if (isQuitting || gameEnded && sceneAlreadyLoading)
            {
                Debug.Log("*** IGNORING - Already quitting/ended/loading ***");
                return;
            }

            int quitPlayerActorNumber = (int)propertiesThatChanged[QUIT_PROPERTY_KEY];
            Debug.Log($"Quit player ActorNumber: {quitPlayerActorNumber}");
            Debug.Log($"My ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");

            if (quitPlayerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log("*** OPPONENT QUIT - I WIN! ***");
                HandleVictory("Opponent quit via room properties");
            }
            else
            {
                Debug.Log("*** I QUIT - Ignoring my own quit property ***");
            }
        }

        if (propertiesThatChanged.ContainsKey("curScn"))
        {
            Debug.Log($"*** WARNING: curScn PROPERTY DETECTED! Value: {propertiesThatChanged["curScn"]} ***");
            Debug.Log("*** THIS MIGHT BE THE INTERFERENCE SOURCE! ***");
        }
    }

    private void LoadSceneWithDebug(string sceneName, string reason)
    {
        Debug.Log($"*** LoadSceneWithDebug CALLED ***");
        Debug.Log($"Scene Name: '{sceneName}'");
        Debug.Log($"Reason: {reason}");

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("*** SCENE NAME IS NULL OR EMPTY! ***");
            return;
        }

        string[] possibleNames = {
            sceneName,
            $"Scenes/{sceneName}",
            sceneName.Replace("Scenes/", ""),
        };

        string foundSceneName = null;

        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            string buildSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            foreach (string possibleName in possibleNames)
            {
                if (buildSceneName.Equals(possibleName, System.StringComparison.OrdinalIgnoreCase))
                {
                    foundSceneName = buildSceneName;
                    Debug.Log($"*** SCENE FOUND: '{foundSceneName}' ***");
                    break;
                }
            }

            if (foundSceneName != null) break;
        }

        if (foundSceneName == null)
        {
            Debug.LogError($"*** SCENE '{sceneName}' NOT FOUND IN BUILD SETTINGS! ***");
            return;
        }

        try
        {
            Debug.Log($"*** CALLING SceneManager.LoadScene('{foundSceneName}') ***");
            SceneManager.LoadScene(foundSceneName);
            Debug.Log($"*** SceneManager.LoadScene('{foundSceneName}') COMPLETED! ***");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"*** EXCEPTION LOADING SCENE '{foundSceneName}': {e.Message} ***");
        }
    }

    [ContextMenu("Test Quit")]
    public void TestQuit()
    {
        Debug.Log("*** TESTING QUIT VIA CONTEXT MENU ***");
        QuitGame();
    }
}

[System.Serializable]
public class FlaskSpawnData
{
    public int slotIndex;
    public bool isPlayerA;
    public int flaskIndex;
}

[System.Serializable]
public class FlaskDataWrapper
{
    public FlaskSpawnData[] flasks;
}