using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class LiarBarGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Enums and Constants
    public enum GameState
    {
        WaitingForPlayers,
        RoundStart,
        PlayerPlaying,
        WaitingForChallenge,
        RevealingCards,
        WaitingForRoulette,
        GameOver
    }

    private const string PUNISHMENT_RESULT_KEY = "RouletteResult";
    private const string PUNISHED_PLAYER_KEY = "PunishedPlayer";
    private const string HAND_COUNT_PREFIX = "HandCount_";
    private const string HAND_DATA_PREFIX = "HandData_";
    private const string LAST_ROOM_KEY = "LastGameRoom";

    private readonly string[] CARD_NAMES = { "K", "Q", "J", "A" }; 
    #endregion

    #region Game State Variables
    [Header("Game State")]
    public GameState currentState = GameState.WaitingForPlayers;
    public int currentPlayerIndex = 0;
    public string currentTargetCard = "";
    public int currentRound = 0;
    public readonly List<CardData> middlePile = new List<CardData>();
    private readonly List<CardData> playedCardsThisTurn = new List<CardData>();
    private bool isMyTurn = false;
    private int cardsPlayedThisTurn = 0;
    private bool shouldLoadRoulette = false;
    private int playersReady = 0;
    #endregion

    #region UI References
    [Header("UI References")]
    public TextMeshProUGUI gameStatusText;
    public TextMeshProUGUI roundDisplayText;
    public GameObject playPanel;
    public GameObject challengePanel;
    public Button playButton;
    public Button liarButton;
    public Button skipButton;

    [Header("Popup Info")]
    public GameObject popupInfoPanel;
    public TextMeshProUGUI popupRoundInfo;
    public TextMeshProUGUI popupTargetCardInfo;
    public Button popupOkButton;
    #endregion

    #region Scene and Player Management
    [Header("Scene Names")]
    public string victorySceneName = "Victory";
    public string defeatSceneName = "Defeat";
    public string rouletteSceneName = "SpinWheel";

    [Header("Player Management")]
    public List<PlayerData> players = new List<PlayerData>();
    public LiarBarHandManager localHandManager;
    public LifeManager lifeManager;
    public OpponentCardCounter opponentCardCounter;
    #endregion

    #region Timer Settings
    [Header("Timer Settings")]
    public float playTimeLimit = 15f;
    public float challengeTimeLimit = 10f;
    private float currentTimer = 0f;
    private bool timerActive = false;
    #endregion

    #region Audio and Effects
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cardPlaySound;
    public AudioClip challengeSound;
    public AudioClip roundStartSound;
    public AudioClip timerTickSound;
    public AudioClip victorySound;
    public AudioClip defeatSound;
    public AudioClip loseLifeSound;

    [Header("Visual Effects")]
    public ParticleSystem confettiEffect;
    #endregion

    #region Debug
    [Header("Debug")]
    public bool enableDebugLogs = true;
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        LogDebug($"Start() called on GameObject: {gameObject.name}, Instance ID: {GetInstanceID()}");
        LogDebug($"currentRound at very beginning of Start(): {currentRound}");

        currentRound = 0;
        LogDebug($"FORCED currentRound to 0 in Start()");

        bool isAfterRoulette = PlayerPrefs.HasKey("AfterRoulette_" + PhotonNetwork.LocalPlayer.ActorNumber);
        if (isAfterRoulette)
        {
            LogDebug("⚠️ Start() detected AfterRoulette - will prevent UI flicker during init");
        }

        InitializePhotonSettings();
        InitializeGameSession();
        SetupUI();
        ProcessRouletteResultIfExists();
    }

    void Update()
    {
        UpdateTimerUI();
    }

    void OnDestroy()
    {
        LogDebug("OnDestroy called - cleaning up");

        if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null)
        {
            LogDebug("Disconnected - clearing game data for next session");
            ClearAllGameData();
        }

        HandlePlayerQuitMidGame();
    }
    #endregion

    #region Initialization Methods
    private void InitializePhotonSettings()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("curScn"))
        {
            var roomProps = new ExitGames.Client.Photon.Hashtable();
            roomProps["curScn"] = null;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            LogDebug("Cleared curScn room property");
        }
    }

    private void InitializeGameSession()
    {
        bool isNewSession = CheckIfNewGameSession();

        if (isNewSession)
        {
            LogDebug("NEW GAME SESSION DETECTED - Clearing all persistent data");
            ClearAllGameData();
            currentRound = 0;
            LogDebug($"FORCED currentRound = {currentRound} for new session");
        }
        else
        {
            LogDebug("CONTINUING EXISTING SESSION - Restoring data");
            RestoreGameStateFromPlayerPrefs();
            LogDebug($"After restore: currentRound = {currentRound}");
        }

        InitializeGame();
    }

    private bool CheckIfNewGameSession()
    {
        string roomId = PhotonNetwork.CurrentRoom?.Name ?? "unknown";
        string lastRoom = PlayerPrefs.GetString(LAST_ROOM_KEY, "");

        LogDebug($"Current room: {roomId}, Last room: {lastRoom}");

        if (roomId != lastRoom)
        {
            PlayerPrefs.SetString(LAST_ROOM_KEY, roomId);
            PlayerPrefs.Save();
            return true;
        }

        bool hasRouletteData = PlayerPrefs.HasKey(PUNISHMENT_RESULT_KEY);
        bool hasGameStateData = PlayerPrefs.HasKey("GameState_CurrentRound");

        if (!hasRouletteData && hasGameStateData)
        {
            LogDebug("Found orphaned game state without roulette data - treating as new session");
            return true;
        }

        return false;
    }
    #endregion

    #region Data Management
    private void ClearAllGameData()
    {
        LogDebug("Clearing ALL persistent game data");

        PlayerPrefs.DeleteKey("GameState_CurrentRound");
        PlayerPrefs.DeleteKey("GameState_TargetCard");
        PlayerPrefs.DeleteKey("GameState_PlayerIndex");

        PlayerPrefs.DeleteKey(PUNISHMENT_RESULT_KEY);
        PlayerPrefs.DeleteKey(PUNISHED_PLAYER_KEY);

        for (int actorNumber = 1; actorNumber <= 10; actorNumber++)
        {
            PlayerPrefs.DeleteKey(HAND_COUNT_PREFIX + actorNumber);
            PlayerPrefs.DeleteKey("GameState_Lives_" + actorNumber);

            for (int cardIndex = 0; cardIndex < 10; cardIndex++)
            {
                PlayerPrefs.DeleteKey(HAND_DATA_PREFIX + actorNumber + "_" + cardIndex);
            }
        }

        PlayerPrefs.Save();
        LogDebug("All persistent data cleared - fresh start");
    }

    private void RestoreGameStateFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("GameState_CurrentRound"))
        {
            currentRound = PlayerPrefs.GetInt("GameState_CurrentRound");
            LogDebug($"Restored currentRound: {currentRound}");
        }

        if (PlayerPrefs.HasKey("GameState_TargetCard"))
        {
            currentTargetCard = PlayerPrefs.GetString("GameState_TargetCard");
            LogDebug($"Restored currentTargetCard: {currentTargetCard}");
        }

        if (PlayerPrefs.HasKey("GameState_PlayerIndex"))
        {
            currentPlayerIndex = PlayerPrefs.GetInt("GameState_PlayerIndex");
            LogDebug($"Restored currentPlayerIndex: {currentPlayerIndex}");
        }
    }

    private void SaveGameStateToPlayerPrefs()
    {
        PlayerPrefs.SetInt("GameState_CurrentRound", currentRound);
        PlayerPrefs.SetString("GameState_TargetCard", currentTargetCard);
        PlayerPrefs.SetInt("GameState_PlayerIndex", currentPlayerIndex);

        foreach (var player in players)
        {
            string livesKey = "GameState_Lives_" + player.photonPlayer.ActorNumber;
            PlayerPrefs.SetInt(livesKey, player.lives);
            LogDebug($"Saved {player.photonPlayer.NickName} lives: {player.lives}");
        }

        PlayerPrefs.Save();
        LogDebug($"Saved game state - Round: {currentRound}, Target: {currentTargetCard}");
    }
    #endregion

    #region Player and Game Initialization
    private void InitializeGame()
    {
        LogDebug($"InitializeGame() called! currentRound at start = {currentRound}");

        var photonPlayers = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToArray();

        if (photonPlayers.Length != 2)
        {
            Debug.LogError("Liar's Bar 1v1 requires exactly 2 players!");
            return;
        }

        InitializePlayers(photonPlayers);
        RestoreLivesIfNeeded();
        InitializeLifeManager();

        LogDebug($"Before StartGameFlow: currentRound = {currentRound}");
        StartGameFlow();
    }

    private void InitializePlayers(Photon.Realtime.Player[] photonPlayers)
    {
        players.Clear();
        playersReady = 0;

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            players.Add(new PlayerData
            {
                photonPlayer = photonPlayers[i],
                isAlive = true,
                handCount = 6,
                totalWins = 0,
                lives = 3
            });
        }

        LogDebug("Created fresh player data");
    }

    private void RestoreLivesIfNeeded()
    {
        LogDebug("RestoreLivesIfNeeded called");

        bool anyLivesRestored = false;

        foreach (var player in players)
        {
            string livesKey = "GameState_Lives_" + player.photonPlayer.ActorNumber;
            if (PlayerPrefs.HasKey(livesKey))
            {
                int savedLives = PlayerPrefs.GetInt(livesKey);
                int originalLives = player.lives;
                player.lives = savedLives;
                anyLivesRestored = true;
                LogDebug($"RESTORED {player.photonPlayer.NickName} lives: {originalLives} → {savedLives}");
            }
            else
            {
                LogDebug($"No saved lives found for {player.photonPlayer.NickName}, keeping default: {player.lives}");
            }
        }

        if (anyLivesRestored)
        {
            LogDebug("Lives restoration completed - will sync with UI later");
        }
        else
        {
            LogDebug("No lives restoration needed - all players keep default lives");
        }
    }

    private void InitializeLifeManager()
    {
        LogDebug("=== InitializeLifeManager START ===");

        if (lifeManager != null)
        {
            LogDebug("Lives BEFORE InitializeLifeManager:");
            foreach (var player in players)
            {
                LogDebug($"  {player.photonPlayer.NickName}: {player.lives} lives");
            }

            lifeManager.enableDebugLogs = false;

            bool hasAnyRestoredLives = false;
            foreach (var player in players)
            {
                string livesKey = "GameState_Lives_" + player.photonPlayer.ActorNumber;
                if (PlayerPrefs.HasKey(livesKey))
                {
                    int savedLives = PlayerPrefs.GetInt(livesKey);
                    LogDebug($"Found saved lives for player {player.photonPlayer.ActorNumber}: {savedLives}");
                    player.lives = savedLives;
                    hasAnyRestoredLives = true;
                }
            }

            bool isAfterRoulette = PlayerPrefs.HasKey("AfterRoulette");
            LogDebug($"Is after roulette: {isAfterRoulette}");
            LogDebug($"Has any restored lives: {hasAnyRestoredLives}");

            if (hasAnyRestoredLives || isAfterRoulette)
            {
                LogDebug("🚫 CRITICAL: Lives were restored OR after roulette - COMPLETELY BYPASSING LifeManager.Start()");

                PlayerPrefs.SetString("BypassLifeManagerReset", "true");
                PlayerPrefs.Save();

                LogDebug("⏳ DELAYING LifeManager setup to prevent any resets...");

                Invoke(nameof(SetLifesManagerAfterDelay), 3f);
            }
            else
            {
                LogDebug("Fresh game - allowing normal LifeManager reset");
                PlayerPrefs.DeleteKey("BypassLifeManagerReset");

                foreach (var player in players)
                {
                    player.lives = 3;
                }
            }
        }
        else
        {
            Debug.LogError("LifeManager is NULL! Please assign it in Inspector!");
        }

        LogDebug("=== InitializeLifeManager END ===");
    }

    private void SetLifesManagerAfterDelay()
    {
        LogDebug("=== SetLifesManagerAfterDelay START ===");
        LogDebug("Setting LifeManager AFTER everything has settled...");

        if (lifeManager == null)
        {
            Debug.LogError("LifeManager is NULL in SetLifesManagerAfterDelay!");
            return;
        }

        LogDebug("🔥 OVERRIDING any LifeManager resets that may have occurred...");

        StartCoroutine(OverrideLifeManagerCoroutine());
    }

    private System.Collections.IEnumerator OverrideLifeManagerCoroutine()
    {
        LogDebug("=== OverrideLifeManagerCoroutine START ===");

        foreach (var player in players)
        {
            bool isLocalPlayer = (player.photonPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            LogDebug($"🎯 SETTING INTERNAL: {player.photonPlayer.NickName}: {player.lives} lives, isLocal: {isLocalPlayer}");

            if (player.lives <= 0)
            {
                Debug.LogWarning($"⚠️ Player {player.photonPlayer.NickName} has {player.lives} lives - fixing...");

                string livesKey = "GameState_Lives_" + player.photonPlayer.ActorNumber;
                int savedLives = PlayerPrefs.GetInt(livesKey, -1);

                if (savedLives > 0)
                {
                    player.lives = savedLives;
                    LogDebug($"Fixed using PlayerPrefs: {savedLives}");
                }
                else
                {
                    player.lives = 1;
                    LogDebug($"Fixed using default: 1");
                }
            }

            if (isLocalPlayer)
            {
                lifeManager.SetPlayerLives(player.lives);
            }
            else
            {
                lifeManager.SetEnemyLives(player.lives);
            }
        }

        yield return new WaitForSeconds(0.5f);

        LogDebug("🔓 UNBLOCKING LifeManager and forcing UI update...");
        lifeManager.UnblockAndForceUpdateAll();

        lifeManager.enableDebugLogs = true;
        PlayerPrefs.DeleteKey("AfterRoulette");
        PlayerPrefs.DeleteKey("BypassLifeManagerReset");
        LogDebug("Cleared all flags after successful override");

        if (PhotonNetwork.IsMasterClient)
        {
            Invoke(nameof(BroadcastCurrentLives), 1f);
        }

        LogDebug("=== OverrideLifeManagerCoroutine END ===");
    }

    private void BroadcastCurrentLives()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        LogDebug("🔄 BroadcastCurrentLives - Final sync step");

        bool isAfterRoulette = PlayerPrefs.HasKey("AfterRoulette_" + PhotonNetwork.LocalPlayer.ActorNumber);
        if (isAfterRoulette)
        {
            LogDebug("⚠️ BroadcastCurrentLives called but AfterRoulette flag still exists - this might cause flicker");
        }

        foreach (var player in players)
        {
            LogDebug($"📡 Final broadcast: {player.photonPlayer.NickName}: {player.lives} lives");

            photonView.RPC("FinalSyncLifeUI", RpcTarget.All, player.photonPlayer.ActorNumber, player.lives);
        }
    }

    [PunRPC]
    void FinalSyncLifeUI(int playerActorNumber, int newLives)
    {
        LogDebug($"🎯 FinalSyncLifeUI: Player {playerActorNumber} to {newLives} lives");

        var player = GetPlayerByActorNumber(playerActorNumber);
        if (player != null)
        {
            player.lives = newLives;

            string livesKey = "GameState_Lives_" + playerActorNumber;
            PlayerPrefs.SetInt(livesKey, newLives);
            PlayerPrefs.Save();
        }

        if (lifeManager == null) return;

        bool isTargetPlayerLocal = (playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

        if (isTargetPlayerLocal)
        {
            lifeManager.SetPlayerLives(newLives);
            LogDebug($"🎯 FINAL: Set LOCAL player to {newLives} lives");
        }
        else
        {
            lifeManager.SetEnemyLives(newLives);
            LogDebug($"🎯 FINAL: Set ENEMY to {newLives} lives");
        }
    }

    private void SyncLivesWithUI()
    {
        LogDebug("=== SyncLivesWithUI START ===");
        foreach (var player in players)
        {
            bool isLocalPlayer = (player.photonPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            LogDebug($"Syncing {player.photonPlayer.NickName} (Actor {player.photonPlayer.ActorNumber}): {player.lives} lives, isLocal: {isLocalPlayer}");

            if (isLocalPlayer)
            {
                lifeManager.SetPlayerLives(player.lives);
                LogDebug($"Set LOCAL player lives to {player.lives}");
            }
            else
            {
                lifeManager.SetEnemyLives(player.lives);
                LogDebug($"Set ENEMY lives to {player.lives}");
            }
        }
        LogDebug("=== SyncLivesWithUI END ===");
    }

    private void StartGameFlow()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug($"StartGameFlow: currentRound BEFORE processing = {currentRound}");

            if (currentRound == 0)
            {
                LogDebug("Fresh game - FORCING Round 1 start");
                currentRound = 0;
                StartFirstRoundExplicitly();
            }
            else
            {
                LogDebug($"Continuing existing game at round {currentRound}");
                HandleGameContinuation();
            }
        }
        else
        {
            LogDebug($"Not Master Client, waiting for game continuation. Round: {currentRound}");
        }

        LogDebug($"InitializeGame completed. Players: {players.Count}, Round: {currentRound}");
    }

    private void StartFirstRoundExplicitly()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        LogDebug("StartFirstRoundExplicitly() called! FORCING Round 1");

        currentRound = 1;
        middlePile.Clear();
        playedCardsThisTurn.Clear();
        playersReady = 0;

        HandManager.ResetSharedDeck();
        ResetPlayerHandCounts();

        int randomStartPlayer = GetRandomAlivePlayer();
        string newTargetCard = CARD_NAMES[Random.Range(0, CARD_NAMES.Length)];

        LogDebug($"EXPLICITLY starting ROUND {currentRound}. Start player index: {randomStartPlayer}");

        photonView.RPC("NewRoundStarted", RpcTarget.All, newTargetCard, currentRound, randomStartPlayer);
    }

    private void HandleGameContinuation()
    {
        if (string.IsNullOrEmpty(currentTargetCard))
        {
            Debug.LogWarning("No target card found, starting new round");
            StartNewRound();
        }
        else
        {
            LogDebug($"Resuming game with target card: {currentTargetCard}");
            currentState = GameState.PlayerPlaying;
            Invoke(nameof(ResumeGameFlow), 1f);
        }
    }
    #endregion

    #region UI Setup and Management
    private void SetupUI()
    {
        SetupButtonListeners();
        InitializeUIComponents();
        UpdateUI();
    }

    private void SetupButtonListeners()
    {
        if (playButton != null) playButton.onClick.AddListener(PlayCards);
        if (liarButton != null) liarButton.onClick.AddListener(ChallengeLiar);
        if (skipButton != null) skipButton.onClick.AddListener(SkipAction);
        if (popupOkButton != null) popupOkButton.onClick.AddListener(OnPopupOk);
    }

    private void InitializeUIComponents()
    {
        if (popupInfoPanel) popupInfoPanel.SetActive(false);
        if (playPanel) playPanel.SetActive(false);
        if (challengePanel) challengePanel.SetActive(false);
        if (roundDisplayText) roundDisplayText.text = "";
        if (gameStatusText) gameStatusText.text = "Waiting...";
    }

    private void UpdateUI()
    {
        if (players.Count == 0 || currentPlayerIndex >= players.Count) return;

        isMyTurn = players[currentPlayerIndex].photonPlayer == PhotonNetwork.LocalPlayer;

        switch (currentState)
        {
            case GameState.PlayerPlaying:
                UpdatePlayerPlayingUI();
                break;
            case GameState.WaitingForChallenge:
                UpdateWaitingForChallengeUI();
                break;
            case GameState.RevealingCards:
                UpdateRevealingCardsUI();
                break;
            case GameState.WaitingForRoulette:
                UpdateWaitingForRouletteUI();
                break;
        }
    }

    private void UpdatePlayerPlayingUI()
    {
        if (isMyTurn)
        {
            SetUIState(playPanel: true, challengePanel: false, skipButton: true);
            StartPlayTimer();
        }
        else
        {
            SetUIState(playPanel: false, challengePanel: false, skipButton: false);
        }
    }

    private void UpdateWaitingForChallengeUI()
    {
        if (!isMyTurn)
        {
            SetUIState(playPanel: false, challengePanel: true, skipButton: true);
            StartChallengeTimer();
        }
        else
        {
            SetUIState(playPanel: false, challengePanel: false, skipButton: false);
        }
    }

    private void UpdateRevealingCardsUI()
    {
        if (gameStatusText) gameStatusText.text = "Revealing...";
        SetUIState(playPanel: false, challengePanel: false, skipButton: false);
    }

    private void UpdateWaitingForRouletteUI()
    {
        if (gameStatusText) gameStatusText.text = "Roulette...";
        SetUIState(playPanel: false, challengePanel: false, skipButton: false);
    }

    private void SetUIState(bool playPanel = false, bool challengePanel = false, bool skipButton = false)
    {
        if (this.playPanel) this.playPanel.SetActive(playPanel);
        if (this.challengePanel) this.challengePanel.SetActive(challengePanel);
        if (this.skipButton) this.skipButton.gameObject.SetActive(skipButton);
    }
    #endregion

    #region Timer Management
    private void StartPlayTimer()
    {
        currentTimer = playTimeLimit;
        timerActive = true;
        if (gameStatusText)
            gameStatusText.text = isMyTurn ? "Your Turn" : "Opponent Turn";
    }

    private void StartChallengeTimer()
    {
        currentTimer = challengeTimeLimit;
        timerActive = true;
        if (gameStatusText)
            gameStatusText.text = !isMyTurn ? "Challenge?" : "Waiting...";
    }

    private void StopTimer()
    {
        timerActive = false;
    }

    private void UpdateTimerUI()
    {
        if (timerActive && gameStatusText)
        {
            currentTimer -= Time.deltaTime;

            string baseText = GetTimerBaseText();
            if (!string.IsNullOrEmpty(baseText))
            {
                gameStatusText.text = $"{baseText} ({Mathf.Ceil(currentTimer)}s)";
            }

            HandleTimerEffects();
        }
        else if (!timerActive && gameStatusText)
        {
            UpdateNonTimerUI();
        }
    }

    private string GetTimerBaseText()
    {
        if (currentState == GameState.PlayerPlaying)
        {
            return isMyTurn ? "Your Turn" : "Opponent Turn";
        }
        else if (currentState == GameState.WaitingForChallenge)
        {
            return !isMyTurn ? "Challenge?" : "Waiting...";
        }
        return "";
    }

    private void HandleTimerEffects()
    {
        if (currentTimer <= 3f && currentTimer > 2f)
        {
            PlaySound(timerTickSound);
        }

        if (currentTimer <= 0)
        {
            HandleTimerExpired();
        }
    }

    private void UpdateNonTimerUI()
    {
        switch (currentState)
        {
            case GameState.PlayerPlaying:
                gameStatusText.text = isMyTurn ? "Your Turn" : "Opponent Turn";
                break;
            case GameState.WaitingForChallenge:
                gameStatusText.text = !isMyTurn ? "Challenge?" : "Waiting...";
                break;
            case GameState.RevealingCards:
                gameStatusText.text = "Revealing...";
                break;
            case GameState.WaitingForRoulette:
                gameStatusText.text = "Roulette...";
                break;
            default:
                gameStatusText.text = "Waiting...";
                break;
        }
    }

    private void HandleTimerExpired()
    {
        StopTimer();

        if (currentState == GameState.PlayerPlaying && isMyTurn)
        {
            if (gameStatusText) gameStatusText.text = "Time's up! Turn skipped.";
            Invoke(nameof(NextPlayerTurn), 1.5f);
        }
        else if (currentState == GameState.WaitingForChallenge && !isMyTurn)
        {
            AcceptPlay();
        }
    }
    #endregion

    #region Card Helper Methods
    private bool IsCardMatchingTarget(string cardName, string targetCard)
    {
        string normalizedCard = NormalizeCardName(cardName);
        string normalizedTarget = NormalizeCardName(targetCard);

        LogDebug($"Comparing: '{cardName}' -> '{normalizedCard}' vs '{targetCard}' -> '{normalizedTarget}'");

        if (normalizedCard.Equals("JOKER", System.StringComparison.OrdinalIgnoreCase))
        {
            LogDebug($"🃏 JOKER detected! '{cardName}' matches ANY target (including '{targetCard}')");
            return true;
        }

        return normalizedCard.Equals(normalizedTarget, System.StringComparison.OrdinalIgnoreCase);
    }

    private string NormalizeCardName(string cardName)
    {
        if (string.IsNullOrEmpty(cardName)) return "";

        string normalized = cardName.Trim().ToUpper();

        return normalized switch
        {
            "KING" or "K" => "K",
            "QUEEN" or "Q" => "Q",
            "JACK" or "J" => "J",
            "ACE" or "A" => "A",
            "JOKER" => "JOKER",
            _ => normalized
        };
    }
    #endregion

    #region Round Management
    private void StartNewRound()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        LogDebug($"StartNewRound() called! Current round was: {currentRound}");

        currentRound++;
        middlePile.Clear();
        playedCardsThisTurn.Clear();
        playersReady = 0;

        HandManager.ResetSharedDeck();

        ResetPlayerHandCounts();

        int randomStartPlayer = GetRandomAlivePlayer();
        string newTargetCard = CARD_NAMES[Random.Range(0, CARD_NAMES.Length)];

        LogDebug($"Starting NEW ROUND {currentRound}. RANDOM start player index: {randomStartPlayer}. KEEPING CURRENT LIVES.");

        photonView.RPC("NewRoundStarted", RpcTarget.All, newTargetCard, currentRound, randomStartPlayer);
    }

    private void ResetPlayerHandCounts()
    {
        foreach (var player in players)
        {
            if (player.isAlive)
            {
                int oldLives = player.lives;
                player.handCount = 6;
                LogDebug($"Player {player.photonPlayer.NickName} - keeping {oldLives} lives");
            }
        }
    }

    private int GetRandomAlivePlayer()
    {
        int randomStartPlayer = Random.Range(0, 2);

        while (!players[randomStartPlayer].isAlive)
        {
            randomStartPlayer = (randomStartPlayer + 1) % 2;
        }

        return randomStartPlayer;
    }

    private void ResumeGameFlow()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        LogDebug("ResumeGameFlow - continuing with existing round");

        ValidateCurrentPlayerIndex();
        photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
    }

    private void ValidateCurrentPlayerIndex()
    {
        if (currentPlayerIndex < 0 || currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }

        while (currentPlayerIndex < players.Count && !players[currentPlayerIndex].isAlive)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        }
    }
    #endregion

    #region RPC Methods
    [PunRPC]
    void NewRoundStarted(string targetCard, int roundNumber, int startPlayerIndex)
    {
        LogDebug($"NewRoundStarted RPC - Round: {roundNumber}, Target: {targetCard}, Start Player: {startPlayerIndex}");
        LogDebug($"BEFORE setting: currentRound was {currentRound}");

        LogDebug("=== LIVES BEFORE NewRoundStarted ===");
        foreach (var player in players)
        {
            LogDebug($"Player {player.photonPlayer.NickName}: {player.lives} lives");
        }

        currentTargetCard = targetCard;
        currentRound = roundNumber;
        currentPlayerIndex = startPlayerIndex;
        currentState = GameState.RoundStart;
        playedCardsThisTurn.Clear();

        LogDebug($"AFTER setting: currentRound is now {currentRound}");

        LogDebug("Creating NEW HAND for NEW ROUND");
        CreateNewHandForNewRound();

        LogDebug("=== LIVES AFTER NewRoundStarted ===");
        foreach (var player in players)
        {
            LogDebug($"Player {player.photonPlayer.NickName}: {player.lives} lives");
        }

        Invoke(nameof(ShowRoundPopup), 1f);
        PlaySound(roundStartSound);
    }

    private void CreateNewHandForNewRound()
    {
        if (localHandManager != null)
        {
            var baseHandManager = localHandManager.GetComponent<HandManager>();
            if (baseHandManager != null)
            {
                StartCoroutine(CreateNewHandCoroutine(baseHandManager));
                LogDebug("Creating NEW HAND for NEW ROUND");
            }
        }
    }

    [PunRPC]
    void ReceiveCardPlay(int cardCount, int playerActorNumber, string[] cardNames)
    {
        LogDebug($"ReceiveCardPlay - Player: {playerActorNumber}, Cards: {cardCount}");

        cardsPlayedThisTurn = cardCount;

        var player = GetPlayerByActorNumber(playerActorNumber);
        if (player != null)
        {
            int oldHandCount = player.handCount;
            player.handCount -= cardCount;
            LogDebug($"Updated hand count for player {playerActorNumber}: {oldHandCount} → {player.handCount} cards remaining");

            if (playerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && opponentCardCounter != null)
            {
                opponentCardCounter.SetCardCount(player.handCount);
                LogDebug($"Updated OpponentCardCounter after card play: {player.handCount} cards");

                if (opponentCardCounter.IsCriticallyLowOnCards())
                {
                    LogDebug("⚠️ Opponent is critically low on cards!");
                }
                else if (opponentCardCounter.IsLowOnCards())
                {
                    LogDebug("⚠️ Opponent is low on cards!");
                }
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            UpdatePlayedCardsForMaster(cardNames);
        }

        currentState = GameState.WaitingForChallenge;
        UpdateUI();
    }

    private void UpdatePlayedCardsForMaster(string[] cardNames)
    {
        playedCardsThisTurn.Clear();
        foreach (string cardName in cardNames)
        {
            playedCardsThisTurn.Add(new CardData { cardName = cardName });
        }

        LogDebug($"Master Client updated playedCardsThisTurn with {playedCardsThisTurn.Count} cards");
    }

    [PunRPC]
    void PlayAccepted()
    {
        StopTimer();

        if (gameStatusText) gameStatusText.text = "Play accepted!";

        if (CheckForGameEndingConditions()) return;

        LogDebug("No one finished cards, continuing to next turn");
        Invoke(nameof(NextPlayerTurn), 1.5f);
    }

    private bool CheckForGameEndingConditions()
    {
        var currentPlayer = players[currentPlayerIndex];

        LogDebug("PlayAccepted - checking if anyone finished cards");

        if (CheckLocalPlayerFinished(currentPlayer)) return true;
        if (CheckOpponentFinished()) return true;

        return false;
    }

    private bool CheckLocalPlayerFinished(PlayerData currentPlayer)
    {
        if (currentPlayer.photonPlayer == PhotonNetwork.LocalPlayer)
        {
            int localHandCount = localHandManager?.GetCurrentHand()?.Count ?? -1;
            LogDebug($"Local player hand count: {localHandCount}");

            if (localHandCount == 0)
            {
                LogDebug("Local player finished all cards! Other player must go to roulette!");

                var otherPlayer = players.FirstOrDefault(p => p.photonPlayer != PhotonNetwork.LocalPlayer);
                if (otherPlayer != null)
                {
                    if (gameStatusText)
                        gameStatusText.text = $"{PhotonNetwork.LocalPlayer.NickName} finished cards! {otherPlayer.photonPlayer.NickName} must play roulette!";

                    photonView.RPC("StartRussianRoulette", RpcTarget.All, otherPlayer.photonPlayer.ActorNumber);
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckOpponentFinished()
    {
        foreach (var player in players)
        {
            if (player.photonPlayer != PhotonNetwork.LocalPlayer && player.handCount <= 0)
            {
                LogDebug($"Opponent {player.photonPlayer.NickName} finished cards! Local player must go to roulette!");

                if (gameStatusText)
                    gameStatusText.text = $"{player.photonPlayer.NickName} finished cards! {PhotonNetwork.LocalPlayer.NickName} must play roulette!";

                photonView.RPC("StartRussianRoulette", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    void PlayChallenged(int challengerActorNumber)
    {
        StopTimer();
        currentState = GameState.RevealingCards;

        var challenger = GetPlayerByActorNumber(challengerActorNumber);
        var player = players[currentPlayerIndex];

        if (gameStatusText)
            gameStatusText.text = $"{challenger.photonPlayer.NickName} called LIAR!";

        UpdateUI();
        Invoke(nameof(RevealCardsAndJudge), 2f);
    }

    [PunRPC]
    void UpdateGameState(int newState, int newPlayerIndex)
    {
        currentState = (GameState)newState;
        currentPlayerIndex = newPlayerIndex;
        UpdateUI();
    }

    [PunRPC]
    void SyncHandCount(int playerActorNumber, int handCount)
    {
        var player = GetPlayerByActorNumber(playerActorNumber);
        if (player != null)
        {
            player.handCount = handCount;
            LogDebug($"Synced hand count: Player {playerActorNumber} has {handCount} cards");

            if (playerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber && opponentCardCounter != null)
            {
                opponentCardCounter.SetCardCount(handCount);
                LogDebug($"Updated OpponentCardCounter: {handCount} cards");
            }
        }
    }

    [PunRPC]
    void StartRussianRoulette(int punishedPlayerActorNumber)
    {
        currentState = GameState.WaitingForRoulette;

        var punishedPlayer = GetPlayerByActorNumber(punishedPlayerActorNumber);
        if (gameStatusText)
            gameStatusText.text = $"{punishedPlayer.photonPlayer.NickName} must play Russian Roulette!";

        UpdateUI();
        SaveGameStateToPlayerPrefs();

        shouldLoadRoulette = false;

        LogDebug($"StartRussianRoulette: Punished player = {punishedPlayerActorNumber}");

        PlayerPrefs.DeleteKey(PUNISHMENT_RESULT_KEY);
        PlayerPrefs.DeleteKey(PUNISHED_PLAYER_KEY);

        if (PhotonNetwork.LocalPlayer.ActorNumber == punishedPlayerActorNumber)
        {
            shouldLoadRoulette = true;
            SaveHandDataBeforeRoulette();
            LogDebug($"I am punished player {punishedPlayerActorNumber}, SET FLAG TO LOAD ROULETTE");
            StartCoroutine(LoadRouletteScene());
        }
        else
        {
            shouldLoadRoulette = false;
            LogDebug($"I am NOT punished player. Punished player is {punishedPlayerActorNumber}, I am {PhotonNetwork.LocalPlayer.ActorNumber}");
        }
    }

    [PunRPC]
    void RouletteResult(int playerActorNumber, bool hitSpecialSlot)
    {
        LogDebug($"=== RouletteResult RPC START ===");
        LogDebug($"RouletteResult: Player {playerActorNumber}, hitSpecialSlot: {hitSpecialSlot}");

        var player = GetPlayerByActorNumber(playerActorNumber);
        if (player == null)
        {
            Debug.LogError($"Cannot find player with ActorNumber {playerActorNumber}");
            return;
        }

        LogDebug($"Player {playerActorNumber} current lives BEFORE roulette result: {player.lives}");

        if (hitSpecialSlot)
        {
            HandlePlayerDeath(player, playerActorNumber);
        }
        else
        {
            HandlePlayerSurvival(player, playerActorNumber);
        }

        LogDebug($"=== RouletteResult RPC END ===");
    }

    private void HandlePlayerDeath(PlayerData player, int playerActorNumber)
    {
        int oldLives = player.lives;
        player.lives = Mathf.Max(0, player.lives - 1);

        LogDebug($"Player {playerActorNumber} hit special slot (DIED) - lives: {oldLives} → {player.lives}");

        photonView.RPC("SyncLifeUI", RpcTarget.All, playerActorNumber, player.lives);

        StartCoroutine(DelaySyncLife(playerActorNumber, player.lives));

        PlaySound(loseLifeSound);

        if (gameStatusText)
            gameStatusText.text = $"{player.photonPlayer.NickName} lost a life! ({player.lives} left)";

        if (player.lives <= 0)
        {
            HandlePlayerElimination(player);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            LogDebug($"Player died in roulette → Starting NEW ROUND with current lives ({player.lives})");
            Invoke(nameof(StartNewRound), 3f);
        }
    }

    private System.Collections.IEnumerator DelaySyncLife(int playerActorNumber, int lives)
    {
        yield return new WaitForSeconds(0.5f);

        LogDebug($"DelaySyncLife: Re-syncing player {playerActorNumber} to {lives} lives");
        photonView.RPC("SyncLifeUI", RpcTarget.All, playerActorNumber, lives);

        yield return new WaitForSeconds(0.5f);
        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug("DelaySyncLife: Broadcasting all current lives for double-check");
            BroadcastCurrentLives();
        }
    }

    private void HandlePlayerElimination(PlayerData player)
    {
        player.isAlive = false;
        if (gameStatusText)
            gameStatusText.text = $"{player.photonPlayer.NickName} is eliminated!";

        PlaySound(defeatSound);

        var winner = players.FirstOrDefault(p => p.isAlive);
        if (winner != null)
        {
            photonView.RPC("GameOver", RpcTarget.All, winner.photonPlayer.ActorNumber);
        }
    }

    private void HandlePlayerSurvival(PlayerData player, int playerActorNumber)
    {
        LogDebug($"Player {playerActorNumber} survived roulette - keeping {player.lives} lives");

        if (gameStatusText)
            gameStatusText.text = $"{player.photonPlayer.NickName} survived! Starting new round...";

        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug("🎯 MAJOR CHANGE: Always start new round after roulette (regardless of result)");
            Invoke(nameof(StartNewRound), 3f);
        }
    }

    [PunRPC]
    void SyncLifeUI(int playerActorNumber, int newLives)
    {
        LogDebug($"=== SyncLifeUI START ===");
        LogDebug($"Target Player ActorNumber: {playerActorNumber}, New Lives: {newLives}");
        LogDebug($"My ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");

        bool isAfterRoulette = PlayerPrefs.HasKey("AfterRoulette_" + PhotonNetwork.LocalPlayer.ActorNumber);
        if (isAfterRoulette)
        {
            LogDebug("⚠️ SKIPPING SyncLifeUI - Currently restoring after roulette to prevent flicker");
            return;
        }

        var player = GetPlayerByActorNumber(playerActorNumber);
        if (player != null)
        {
            int oldLives = player.lives;
            player.lives = newLives;
            LogDebug($"Updated player {playerActorNumber} ({player.photonPlayer.NickName}) lives: {oldLives} → {newLives}");

            string livesKey = "GameState_Lives_" + playerActorNumber;
            PlayerPrefs.SetInt(livesKey, newLives);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError($"Cannot find player with ActorNumber {playerActorNumber}!");
            return;
        }

        if (lifeManager == null)
        {
            Debug.LogError("LifeManager is NULL in SyncLifeUI!");
            return;
        }

        bool isTargetPlayerLocal = (playerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

        LogDebug($"Is Target Player Local: {isTargetPlayerLocal}");

        if (isTargetPlayerLocal)
        {
            lifeManager.SetPlayerLives(newLives);
            LogDebug($"✅ Updated LOCAL PLAYER (bottom) hearts to {newLives}");
        }
        else
        {
            lifeManager.SetEnemyLives(newLives);
            LogDebug($"✅ Updated ENEMY (top) hearts to {newLives}");
        }

        LogDebug($"=== SyncLifeUI END ===");
    }

    [PunRPC]
    void PlayerWon(int winnerActorNumber)
    {
        var winner = GetPlayerByActorNumber(winnerActorNumber);
        winner.totalWins++;
        photonView.RPC("GameOver", RpcTarget.All, winnerActorNumber);
    }

    [PunRPC]
    void GameOver(int winnerActorNumber)
    {
        currentState = GameState.GameOver;
        var winner = GetPlayerByActorNumber(winnerActorNumber);

        if (gameStatusText)
            gameStatusText.text = $"{winner.photonPlayer.NickName} WINS!";

        bool isLocalWinner = (winner.photonPlayer == PhotonNetwork.LocalPlayer);

        if (isLocalWinner)
        {
            PlaySound(victorySound);
            if (confettiEffect) confettiEffect.Play();

            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(true, () =>
                {
                    StartCoroutine(LoadSceneAfterDelay(victorySceneName, 1f));
                });
            }
            else
            {
                Debug.Log("[LiarBarGameManager] No PlayerStatisticsManager found, loading victory scene without stats update");
                StartCoroutine(LoadSceneAfterDelay(victorySceneName, 2f));
            }
        }
        else
        {
            PlaySound(defeatSound);

            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(false, () =>
                {
                    StartCoroutine(LoadSceneAfterDelay(defeatSceneName, 1f));
                });
            }
            else
            {
                Debug.Log("[LiarBarGameManager] No PlayerStatisticsManager found, loading defeat scene without stats update");
                StartCoroutine(LoadSceneAfterDelay(defeatSceneName, 2f));
            }
        }
    }

    [PunRPC]
    void PlayerReadyForRound(int playerActorNumber)
    {
        playersReady++;
        LogDebug($"Player {playerActorNumber} ready. Total ready: {playersReady}");

        if (roundDisplayText) roundDisplayText.text = "";

        if (playersReady >= PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
        {
            playersReady = 0;
            StartPlayerTurn();
        }
    }
    #endregion

    #region Simple Stats Integration

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[LiarBarGameManager] Player {otherPlayer.NickName} has left the room.");

        if (currentState != GameState.GameOver && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("[LiarBarGameManager] Opponent left mid-game → local player wins");

            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(true, () =>
                {
                    Debug.Log("[LiarBarGameManager] Stats updated → Loading Victory scene");
                    SceneManager.LoadScene(victorySceneName);
                });
            }
            else
            {
                Debug.Log("[LiarBarGameManager] No PlayerStatisticsManager found, loading scene without stats update");
                SceneManager.LoadScene(victorySceneName);
            }
        }
    }

    private void HandlePlayerQuitMidGame()
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2 && currentState != GameState.GameOver)
        {
            Debug.Log("[LiarBarGameManager] You quit mid-game → Counted as LOSS");
            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(false);
            }
        }
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"[LiarBarGameManager] Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region Hand Data Management
    private void SaveHandDataBeforeRoulette()
    {
        if (localHandManager != null)
        {
            var currentHand = localHandManager.GetCurrentHand();
            int handCount = currentHand.Count;

            LogDebug($"SaveHandDataBeforeRoulette - Current hand has {handCount} cards");

            PlayerPrefs.SetInt(HAND_COUNT_PREFIX + PhotonNetwork.LocalPlayer.ActorNumber, handCount);

            for (int i = 0; i < currentHand.Count; i++)
            {
                string cardName = currentHand[i].cardName;
                string key = HAND_DATA_PREFIX + PhotonNetwork.LocalPlayer.ActorNumber + "_" + i;
                PlayerPrefs.SetString(key, cardName);

                LogDebug($"Saved card {i}: {cardName} with key: {key}");
            }

            photonView.RPC("SyncHandCount", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, handCount);

            LogDebug($"SaveHandDataBeforeRoulette completed: {handCount} cards saved for player {PhotonNetwork.LocalPlayer.ActorNumber}");
        }
        else
        {
            Debug.LogError("localHandManager is NULL in SaveHandDataBeforeRoulette!");
        }
    }

    private void RestoreHandDataAfterRoulette()
    {
        LogDebug("🎯 MAJOR CHANGE: Skipping hand restoration - will create new hand for new round");

        string handCountKey = HAND_COUNT_PREFIX + PhotonNetwork.LocalPlayer.ActorNumber;
        if (PlayerPrefs.HasKey(handCountKey))
        {
            int savedHandCount = PlayerPrefs.GetInt(handCountKey);
            LogDebug($"Clearing saved hand data for {savedHandCount} cards");

            PlayerPrefs.DeleteKey(handCountKey);

            for (int i = 0; i < savedHandCount; i++)
            {
                string cardKey = HAND_DATA_PREFIX + PhotonNetwork.LocalPlayer.ActorNumber + "_" + i;
                PlayerPrefs.DeleteKey(cardKey);
            }

            PlayerPrefs.Save();
            LogDebug("Hand data cleared - new hand will be created in new round");
        }
    }
    #endregion

    #region Game Actions
    public void PlayCards()
    {
        if (!isMyTurn || currentState != GameState.PlayerPlaying) return;

        var selectedCards = GetSelectedCardsFromHand();

        if (!ValidateSelectedCards(selectedCards)) return;

        ExecuteCardPlay(selectedCards);
    }

    private bool ValidateSelectedCards(List<CardData> selectedCards)
    {
        if (selectedCards.Count == 0)
        {
            if (gameStatusText)
                gameStatusText.text = "Select 1-3 cards first!";
            return false;
        }

        if (selectedCards.Count > 3)
        {
            if (gameStatusText)
                gameStatusText.text = "Maximum 3 cards allowed!";
            return false;
        }

        return true;
    }

    private void ExecuteCardPlay(List<CardData> selectedCards)
    {
        StopTimer();

        playedCardsThisTurn.Clear();
        playedCardsThisTurn.AddRange(selectedCards);

        LogDebugCardPlay(selectedCards);

        string[] cardNames = selectedCards.Select(c => c.cardName).ToArray();
        photonView.RPC("ReceiveCardPlay", RpcTarget.All, selectedCards.Count, PhotonNetwork.LocalPlayer.ActorNumber, cardNames);

        RemoveCardsFromHand(selectedCards);
        PlaySound(cardPlaySound);

        LogDebug($"Cards played and removed from UI. Remaining hand count: {localHandManager?.GetCurrentHand()?.Count ?? 0}");
    }

    private void LogDebugCardPlay(List<CardData> selectedCards)
    {
        if (enableDebugLogs)
        {
            LogDebug("=== PLAYING CARDS DEBUG ===");
            LogDebug($"Player: {PhotonNetwork.LocalPlayer.NickName}");
            LogDebug($"Selected cards count: {selectedCards.Count}");
            LogDebug($"Target card: '{currentTargetCard}'");

            for (int i = 0; i < selectedCards.Count; i++)
            {
                var card = selectedCards[i];
                LogDebug($"Selected Card {i}: cardName='{card.cardName}'");
            }
        }
    }

    private void RemoveCardsFromHand(List<CardData> selectedCards)
    {
        foreach (var card in selectedCards)
        {
            if (localHandManager != null)
            {
                localHandManager.RemoveCard(card);
                if (card?.gameObject != null)
                {
                    DestroyImmediate(card.gameObject);
                }
            }
            middlePile.Add(card);
        }

        localHandManager?.ClearSelection();
    }

    private List<CardData> GetSelectedCardsFromHand()
    {
        if (localHandManager == null) return new List<CardData>();

        if (localHandManager.HasSelectedCards())
        {
            return localHandManager.GetSelectedCardData();
        }

        return new List<CardData>();
    }

    public void SkipAction()
    {
        StopTimer();

        if (currentState == GameState.PlayerPlaying && isMyTurn)
        {
            if (gameStatusText) gameStatusText.text = "Turn skipped!";
            Invoke(nameof(NextPlayerTurn), 1f);
        }
        else if (currentState == GameState.WaitingForChallenge && !isMyTurn)
        {
            AcceptPlay();
        }
    }

    public void ChallengeLiar()
    {
        if (currentState != GameState.WaitingForChallenge || isMyTurn) return;

        StopTimer();
        photonView.RPC("PlayChallenged", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        PlaySound(challengeSound);
    }

    private void AcceptPlay()
    {
        StopTimer();
        photonView.RPC("PlayAccepted", RpcTarget.All);
    }
    #endregion

    #region Card Revelation and Judgment
    private void RevealCardsAndJudge()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            LogDebug("RevealCardsAndJudge: Not master client, ignoring");
            return;
        }

        if (!ValidatePlayedCards()) return;

        bool allCardsAreTarget = playedCardsThisTurn.All(card => IsCardMatchingTarget(card.cardName, currentTargetCard));

        LogDebug($"=== FINAL RESULT: allCardsAreTarget = {allCardsAreTarget} ===");

        var currentPlayer = players[currentPlayerIndex];
        var challenger = players.FirstOrDefault(p => p.photonPlayer != currentPlayer.photonPlayer);

        if (challenger == null)
        {
            Debug.LogError("Cannot find challenger!");
            return;
        }

        ProcessChallengeResult(allCardsAreTarget, currentPlayer, challenger);
    }

    private bool ValidatePlayedCards()
    {
        LogDebug($"=== VALIDATING PLAYED CARDS ===");
        LogDebug($"playedCardsThisTurn.Count: {playedCardsThisTurn.Count}");
        LogDebug($"currentTargetCard: '{currentTargetCard}'");

        if (playedCardsThisTurn.Count == 0)
        {
            Debug.LogError("playedCardsThisTurn is empty! This shouldn't happen.");
            return false;
        }

        if (enableDebugLogs)
        {
            for (int i = 0; i < playedCardsThisTurn.Count; i++)
            {
                string cardName = playedCardsThisTurn[i].cardName;
                bool isMatch = IsCardMatchingTarget(cardName, currentTargetCard);
                string cardType = cardName.Equals("Joker", System.StringComparison.OrdinalIgnoreCase) ? "🃏 JOKER (WILDCARD)" : "Regular card";
                LogDebug($"Card {i}: '{cardName}' ({cardType}) -> Matches target '{currentTargetCard}': {isMatch}");
            }
        }

        return true;
    }

    private void ProcessChallengeResult(bool allCardsAreTarget, PlayerData currentPlayer, PlayerData challenger)
    {
        LogDebug($"=== CHALLENGE RESULT ANALYSIS ===");
        LogDebug($"Current Player (played cards): {currentPlayer.photonPlayer.NickName}");
        LogDebug($"Challenger: {challenger.photonPlayer.NickName}");
        LogDebug($"All cards match target: {allCardsAreTarget}");

        if (enableDebugLogs && playedCardsThisTurn.Count > 0)
        {
            LogDebug($"=== PLAYED CARDS BREAKDOWN ===");
            int jokerCount = 0;
            int targetCount = 0;
            int otherCount = 0;

            foreach (var card in playedCardsThisTurn)
            {
                string normalizedCard = NormalizeCardName(card.cardName);
                if (normalizedCard.Equals("JOKER", System.StringComparison.OrdinalIgnoreCase))
                {
                    jokerCount++;
                    LogDebug($"  🃏 Joker: '{card.cardName}' (WILDCARD - always valid)");
                }
                else if (normalizedCard.Equals(NormalizeCardName(currentTargetCard), System.StringComparison.OrdinalIgnoreCase))
                {
                    targetCount++;
                    LogDebug($"  ✅ Target card: '{card.cardName}' matches '{currentTargetCard}'");
                }
                else
                {
                    otherCount++;
                    LogDebug($"  ❌ Other card: '{card.cardName}' does NOT match '{currentTargetCard}'");
                }
            }

            LogDebug($"Summary: {jokerCount} Jokers, {targetCount} Target cards, {otherCount} Other cards");
            LogDebug($"Player was honest: {allCardsAreTarget}");
        }

        ProcessSimplifiedChallenge(allCardsAreTarget, currentPlayer, challenger);
    }

    private void ProcessSimplifiedChallenge(bool allCardsAreTarget, PlayerData currentPlayer, PlayerData challenger)
    {
        LogDebug($"=== SIMPLIFIED CHALLENGE PROCESSING ===");

        if (allCardsAreTarget)
        {
            LogDebug($"RESULT: {currentPlayer.photonPlayer.NickName} was HONEST! {challenger.photonPlayer.NickName} gets punished!");

            if (gameStatusText)
                gameStatusText.text = $"{currentPlayer.photonPlayer.NickName} was honest! {challenger.photonPlayer.NickName} gets punished!";

            photonView.RPC("StartRussianRoulette", RpcTarget.All, challenger.photonPlayer.ActorNumber);
        }
        else
        {
            LogDebug($"RESULT: {currentPlayer.photonPlayer.NickName} was LYING! They get punished!");

            if (gameStatusText)
                gameStatusText.text = $"{currentPlayer.photonPlayer.NickName} was LYING! They get punished!";

            photonView.RPC("StartRussianRoulette", RpcTarget.All, currentPlayer.photonPlayer.ActorNumber);
        }
    }
    #endregion

    #region Game Flow Control
    private void NextPlayerTurn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        playedCardsThisTurn.Clear();

        currentPlayerIndex = (currentPlayerIndex + 1) % 2;

        if (!players[currentPlayerIndex].isAlive)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;
        }

        LogDebug($"NextPlayerTurn: Moving to player index {currentPlayerIndex}");

        photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
    }

    private void StartPlayerTurn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        ValidateCurrentPlayerIndex();

        LogDebug($"Starting player turn. Current player index: {currentPlayerIndex}");

        photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
    }

    private bool CheckForPlayerWithNoCards()
    {
        LogDebug("CheckForPlayerWithNoCards called");

        foreach (var player in players)
        {
            if (!player.isAlive)
            {
                LogDebug($"Player {player.photonPlayer.NickName} is not alive, skipping");
                continue;
            }

            if (HasPlayerFinishedCards(player))
            {
                LogDebug($"Player {player.photonPlayer.NickName} has no cards - FOUND WINNER!");
                return true;
            }
        }

        LogDebug("No player has finished cards");
        return false;
    }

    private bool HasPlayerFinishedCards(PlayerData player)
    {
        if (player.photonPlayer == PhotonNetwork.LocalPlayer)
        {
            int localHandCount = localHandManager?.GetCurrentHand()?.Count ?? -1;
            LogDebug($"Local player {player.photonPlayer.NickName} has {localHandCount} cards");
            return localHandCount == 0;
        }
        else
        {
            LogDebug($"Opponent {player.photonPlayer.NickName} has {player.handCount} cards (synced)");
            return player.handCount <= 0;
        }
    }
    #endregion

    #region Scene Loading and Coroutines
    private System.Collections.IEnumerator LoadRouletteScene()
    {
        LogDebug($"LoadRouletteScene START - GameObject: {gameObject.name}, Instance: {GetInstanceID()}");

        yield return new WaitForSeconds(2f);

        if (!shouldLoadRoulette)
        {
            LogDebug($"PREVENTED ROULETTE LOAD: shouldLoadRoulette = false on Instance {GetInstanceID()}");
            yield break;
        }

        LogDebug($"LOADING ROULETTE SCENE NOW from Instance {GetInstanceID()}");

        PlayerPrefs.SetInt(PUNISHED_PLAYER_KEY, PhotonNetwork.LocalPlayer.ActorNumber);
        SceneManager.LoadScene(rouletteSceneName);
    }

    private System.Collections.IEnumerator LoadVictoryScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(victorySceneName);
    }

    private System.Collections.IEnumerator LoadDefeatScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(defeatSceneName);
    }

    private System.Collections.IEnumerator CreateNewHandCoroutine(HandManager handManager)
    {
        LogDebug("=== CreateNewHandCoroutine START ===");

        LogDebug("Lives BEFORE creating new hand:");
        foreach (var player in players)
        {
            LogDebug($"  {player.photonPlayer.NickName}: {player.lives} lives");
        }

        yield return new WaitForSeconds(0.1f);

        LogDebug("CreateNewHandCoroutine - Creating new hand via coroutine...");

        if (handManager == null)
        {
            Debug.LogError("HandManager is NULL in CreateNewHandCoroutine!");
            yield break;
        }

        handManager.CreateNewHand();

        yield return new WaitForSeconds(1f);

        var localPlayer = GetPlayerByActorNumber(PhotonNetwork.LocalPlayer.ActorNumber);
        if (localPlayer != null)
        {
            int oldLives = localPlayer.lives;
            localPlayer.handCount = 6;
            LogDebug($"Updated local player hand count to 6 (NEW ROUND) - Lives unchanged: {oldLives}");

            if (localPlayer.lives != oldLives)
            {
                Debug.LogError($"LIVES CHANGED UNEXPECTEDLY! Was {oldLives}, now {localPlayer.lives}");
            }
        }

        photonView.RPC("SyncHandCount", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber, 6);

        if (localHandManager != null)
        {
            var currentHand = localHandManager.GetCurrentHand();
            LogDebug($"Hand creation completed. Hand count: {currentHand.Count}");
        }

        LogDebug("Lives AFTER creating new hand:");
        foreach (var player in players)
        {
            LogDebug($"  {player.photonPlayer.NickName}: {player.lives} lives");
        }

        LogDebug("=== CreateNewHandCoroutine END ===");
    }
    #endregion

    #region Popup Management
    private void ShowRoundPopup()
    {
        LogDebug($"ShowRoundPopup called for player {PhotonNetwork.LocalPlayer.NickName}");

        if (popupInfoPanel != null)
        {
            UpdatePopupContent();
            popupInfoPanel.SetActive(true);
            LogDebug($"Popup activated");

            Invoke(nameof(AutoClosePopup), 3f);
        }
        else
        {
            Debug.LogError("popupInfoPanel is NULL! Check Inspector assignment!");
            HandleMissingPopup();
        }
    }

    private void UpdatePopupContent()
    {
        LogDebug($"UpdatePopupContent: currentRound = {currentRound}");

        if (popupRoundInfo != null)
        {
            popupRoundInfo.text = $"ROUND {currentRound}";
            LogDebug($"Set round text: ROUND {currentRound}");
        }

        if (popupTargetCardInfo != null)
        {
            popupTargetCardInfo.text = $"TARGET: {currentTargetCard}";
            LogDebug($"Set target text: TARGET: {currentTargetCard}");
        }
    }

    private void HandleMissingPopup()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug("Popup null but continuing game flow");
            Invoke(nameof(StartPlayerTurn), 1f);
        }
    }

    private void AutoClosePopup()
    {
        if (popupInfoPanel != null && popupInfoPanel.activeInHierarchy)
        {
            LogDebug("Auto closing popup");
            OnPopupOk();
        }
    }

    public void OnPopupOk()
    {
        LogDebug("OnPopupOk called!");

        if (popupInfoPanel != null)
        {
            popupInfoPanel.SetActive(false);
        }

        CancelInvoke(nameof(AutoClosePopup));

        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug("Master Client starting player turn directly");
            StartPlayerTurn();
        }
        else
        {
            LogDebug("Not Master Client, waiting for turn start");
        }
    }
    #endregion

    #region Roulette Result Handling
    private void ProcessRouletteResultIfExists()
    {
        LogDebug("ProcessRouletteResultIfExists() called");

        if (PlayerPrefs.HasKey(PUNISHMENT_RESULT_KEY))
        {
            LogDebug("🎰 Roulette result detected - processing...");

            if (lifeManager != null)
            {
                lifeManager.TemporaryBlock(3f); 
                LogDebug("🚫 Blocked LifeManager UI for 3 seconds during roulette processing");
            }

            ProcessRouletteResult();
        }
        else
        {
            LogDebug("No roulette result found in PlayerPrefs - this is normal for fresh start");
            HandleNoRouletteResult();
        }
    }

    private void CheckRouletteResult()
    {
        LogDebug("CheckRouletteResult() called");

        if (PlayerPrefs.HasKey(PUNISHMENT_RESULT_KEY))
        {
            ProcessRouletteResult();
        }
        else
        {
            LogDebug("No roulette result found in PlayerPrefs - this is normal for fresh start");
            HandleNoRouletteResult();
        }
    }

    private void ProcessRouletteResult()
    {
        bool hitSpecialSlot = PlayerPrefs.GetInt(PUNISHMENT_RESULT_KEY) == 1;
        int punishedPlayer = PlayerPrefs.GetInt(PUNISHED_PLAYER_KEY, -1);

        LogDebug($"=== PROCESSING ROULETTE RESULT ===");
        LogDebug($"Hit special slot (died): {hitSpecialSlot}");
        LogDebug($"Punished player: {punishedPlayer}");
        LogDebug($"My ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");

        if (punishedPlayer == -1)
        {
            Debug.LogError("🚨 CRITICAL: No punished player found! Roulette data corrupted!");

            PlayerPrefs.DeleteKey(PUNISHMENT_RESULT_KEY);
            PlayerPrefs.DeleteKey(PUNISHED_PLAYER_KEY);
            PlayerPrefs.DeleteKey("RouletteCompleted");

            if (lifeManager != null)
            {
                lifeManager.UnblockAndForceUpdateAll();
            }
            return;
        }

        PlayerPrefs.SetString("AfterRoulette", "true");
        PlayerPrefs.Save();
        LogDebug("🎰 Set AfterRoulette flag to prevent lives reset");

        PlayerPrefs.DeleteKey(PUNISHMENT_RESULT_KEY);
        PlayerPrefs.DeleteKey(PUNISHED_PLAYER_KEY);
        PlayerPrefs.DeleteKey("RouletteCompleted");

        LogDebug("🎯 MAJOR CHANGE: Skipping hand restoration - new hand will be created for new round");
        RestoreHandDataAfterRoulette();

        if (punishedPlayer == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            LogDebug("📡 I was the punished player, sending RouletteResult RPC");
            photonView.RPC("RouletteResult", RpcTarget.All, punishedPlayer, hitSpecialSlot);
        }
        else
        {
            LogDebug("👀 I was NOT the punished player, just observing result");
        }
    }

    private void HandleNoRouletteResult()
    {
        if (currentRound > 0 && PhotonNetwork.IsMasterClient)
        {
            LogDebug("Game was restored but no roulette result - may need to resume game flow");
            Invoke(nameof(EnsureGameFlowContinues), 2f);
        }
    }

    private void EnsureGameFlowContinues()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (currentState == GameState.WaitingForPlayers || gameStatusText.text.Contains("Waiting"))
        {
            LogDebug("Game seems stuck, forcing flow continuation");

            if (string.IsNullOrEmpty(currentTargetCard))
            {
                StartNewRound();
            }
            else
            {
                ResumeGameFlow();
            }
        }
    }
    #endregion

    #region Utility Methods
    private PlayerData GetPlayerByActorNumber(int actorNumber)
    {
        return players.FirstOrDefault(p => p.photonPlayer.ActorNumber == actorNumber);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[GAMEMANAGER] {message}");
        }
    }
    #endregion

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        LogDebug("OnLeftRoom called - clearing game data");
        ClearAllGameData();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        LogDebug($"OnDisconnected called with cause: {cause} - clearing game data");
        ClearAllGameData();
    }

    void OnApplicationQuit()
    {
        Debug.Log("[LiarBarGameManager] Application quitting");
        HandlePlayerQuitMidGame();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
 
        bool isAfterRoulette = PlayerPrefs.HasKey("AfterRoulette_" + PhotonNetwork.LocalPlayer.ActorNumber);

        if (stream.IsWriting)
        {
            stream.SendNext(currentPlayerIndex);
            stream.SendNext(currentTargetCard);
            stream.SendNext(currentRound);
            stream.SendNext((int)currentState);

            if (!isAfterRoulette)
            {
                LogDebug($"OnPhotonSerializeView SENDING: currentRound = {currentRound}");
            }
        }
        else
        {

            if (isAfterRoulette)
            {

                LogDebug("⚠️ BLOCKING OnPhotonSerializeView receive during roulette restore");

                stream.ReceiveNext();
                stream.ReceiveNext(); 
                stream.ReceiveNext();
                stream.ReceiveNext(); 
                return;
            }

            int oldRound = currentRound;
            currentPlayerIndex = (int)stream.ReceiveNext();
            currentTargetCard = (string)stream.ReceiveNext();
            currentRound = (int)stream.ReceiveNext();
            currentState = (GameState)stream.ReceiveNext();

            if (oldRound != currentRound)
            {
                LogDebug($"OnPhotonSerializeView RECEIVED: currentRound changed from {oldRound} to {currentRound}");
            }
        }
    }
    #endregion

    #region Debug Methods
    [ContextMenu("Clear All Game Data")]
    public void ManualClearGameData()
    {
        LogDebug("MANUAL: Clearing all game data");
        ClearAllGameData();
    }

    [ContextMenu("Debug Lives vs PlayerPrefs")]
    public void DebugLivesVsPlayerPrefs()
    {
        LogDebug("=== LIVES MEMORY vs PLAYERPREFS COMPARISON ===");

        foreach (var player in players)
        {
            string livesKey = "GameState_Lives_" + player.photonPlayer.ActorNumber;
            int savedLives = PlayerPrefs.GetInt(livesKey, -1);
            int memoryLives = player.lives;

            LogDebug($"Player {player.photonPlayer.NickName} (Actor {player.photonPlayer.ActorNumber}):");
            LogDebug($"  - Lives in memory: {memoryLives}");
            LogDebug($"  - Lives in PlayerPrefs: {savedLives}");

            if (savedLives != -1 && savedLives != memoryLives)
            {
                Debug.LogWarning($"  ⚠️ MISMATCH! Memory={memoryLives}, PlayerPrefs={savedLives}");
            }

            if (memoryLives <= 0)
            {
                Debug.LogError($"  🚨 CRITICAL: Memory lives is {memoryLives} - this will cause 0-lives flicker!");
            }

            if (savedLives == 0)
            {
                Debug.LogError($"  🚨 CRITICAL: PlayerPrefs lives is {savedLives} - this will cause 0-lives flicker!");
            }
        }

        bool afterRouletteFlag = PlayerPrefs.HasKey("AfterRoulette_" + PhotonNetwork.LocalPlayer.ActorNumber);
        bool bypassFlag = PlayerPrefs.HasKey("BypassLifeManagerReset");

        LogDebug($"Flags - AfterRoulette: {afterRouletteFlag}, BypassReset: {bypassFlag}");

        if (lifeManager != null)
        {
            LogDebug($"LifeManager state - Player: {lifeManager.GetPlayerLives()}, Enemy: {lifeManager.GetEnemyLives()}");
        }
    }

    [ContextMenu("Test Card Validation")]
    public void TestCardValidation()
    {
        LogDebug("🧪 TESTING: Card validation with mixed hand");

        playedCardsThisTurn.Clear();
        playedCardsThisTurn.Add(new CardData { cardName = "K" });
        playedCardsThisTurn.Add(new CardData { cardName = "Joker" });
        playedCardsThisTurn.Add(new CardData { cardName = "Q" });

        currentTargetCard = "K";

        LogDebug($"Simulated hand: K, Joker, Q");
        LogDebug($"Target card: {currentTargetCard}");

        bool allMatch = playedCardsThisTurn.All(card => IsCardMatchingTarget(card.cardName, currentTargetCard));
        LogDebug($"All cards match target: {allMatch}");
        LogDebug($"Expected: TRUE (because Joker should match K, even though Q doesn't)");

        playedCardsThisTurn.Clear();
        playedCardsThisTurn.Add(new CardData { cardName = "Joker" });
        playedCardsThisTurn.Add(new CardData { cardName = "joker" });

        bool allJokersMatch = playedCardsThisTurn.All(card => IsCardMatchingTarget(card.cardName, currentTargetCard));
        LogDebug($"\nPure Joker hand vs '{currentTargetCard}': {allJokersMatch}");
        LogDebug($"Expected: TRUE (Jokers should always match)");
    }

    [ContextMenu("Debug LifeManager State")]
    public void DebugLifeManagerState()
    {
        if (lifeManager != null)
        {
            LogDebug("=== LIFEMANAGER STATE ===");
            LogDebug($"Player Lives: {lifeManager.GetPlayerLives()}");
            LogDebug($"Enemy Lives: {lifeManager.GetEnemyLives()}");
            LogDebug($"Max Player Lives: {lifeManager.GetMaxPlayerLives()}");
            LogDebug($"Max Enemy Lives: {lifeManager.GetMaxEnemyLives()}");

            lifeManager.DebugHeartStatus();
        }
        else
        {
            LogDebug("LifeManager is NULL!");
        }
    }

    [ContextMenu("Reset All Lives To 3")]
    public void ResetAllLivesToThree()
    {
        LogDebug("MANUAL: Resetting all lives to 3");

        foreach (var player in players)
        {
            player.lives = 3;
            LogDebug($"Reset {player.photonPlayer.NickName} to 3 lives");
        }

        if (lifeManager != null)
        {
            lifeManager.ResetHearts();
            LogDebug("Reset LifeManager hearts");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            LogDebug("Broadcasting life reset to all players");
            foreach (var player in players)
            {
                photonView.RPC("SyncLifeUI", RpcTarget.All, player.photonPlayer.ActorNumber, 3);
            }
        }
    }

    [ContextMenu("Test Lose My Life")]
    public void TestLoseMyLife()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SyncLifeUI", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, 2);
        }
    }

    [ContextMenu("Test Lose Enemy Life")]
    public void TestLoseEnemyLife()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var enemy = PhotonNetwork.PlayerList.FirstOrDefault(p => p != PhotonNetwork.LocalPlayer);
            if (enemy != null)
            {
                photonView.RPC("SyncLifeUI", RpcTarget.All, enemy.ActorNumber, 1);
            }
        }
    }

    [ContextMenu("Debug All GameManager Instances")]
    public void DebugAllGameManagerInstances()
    {
        var allGameManagers = FindObjectsOfType<LiarBarGameManager>();
        LogDebug($"Found {allGameManagers.Length} GameManager instances:");

        for (int i = 0; i < allGameManagers.Length; i++)
        {
            var gm = allGameManagers[i];
            LogDebug($"Instance {i}: GameObject={gm.gameObject.name}, InstanceID={gm.GetInstanceID()}, shouldLoadRoulette={gm.shouldLoadRoulette}");
        }
    }

    [ContextMenu("Test Victory Stats")]
    public void TestVictoryStats()
    {
        if (Application.isPlaying)
        {
            Debug.Log("[LiarBarGameManager] Testing victory stats update");
            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(true, () =>
                {
                    StartCoroutine(LoadSceneAfterDelay(victorySceneName, 1f));
                });
            }
            else
            {
                Debug.Log("[LiarBarGameManager] No PlayerStatisticsManager found");
                StartCoroutine(LoadSceneAfterDelay(victorySceneName, 2f));
            }
        }
    }

    [ContextMenu("Test Defeat Stats")]
    public void TestDefeatStats()
    {
        if (Application.isPlaying)
        {
            Debug.Log("[LiarBarGameManager] Testing defeat stats update");
            if (PlayerStatisticsManager.Instance != null)
            {
                PlayerStatisticsManager.Instance.UpdateMatchResult(false, () =>
                {
                    StartCoroutine(LoadSceneAfterDelay(defeatSceneName, 1f));
                });
            }
            else
            {
                Debug.Log("[LiarBarGameManager] No PlayerStatisticsManager found");
                StartCoroutine(LoadSceneAfterDelay(defeatSceneName, 2f));
            }
        }
    }

    [ContextMenu("Debug Stats Manager Connection")]
    public void DebugStatsManagerConnection()
    {
        Debug.Log("=== LIAR BAR GAME MANAGER - STATS DEBUG ===");
        Debug.Log($"PlayerStatisticsManager.Instance exists: {PlayerStatisticsManager.Instance != null}");

        if (PlayerStatisticsManager.Instance != null)
        {
            Debug.Log("PlayerStatisticsManager.Instance is available and ready to use");
        }
        else
        {
            Debug.LogWarning("PlayerStatisticsManager.Instance is NULL!");
        }

        var foundInScene = FindObjectOfType<PlayerStatisticsManager>();
        Debug.Log($"Found PlayerStatisticsManager in scene: {foundInScene != null}");

        if (foundInScene != null)
        {
            Debug.Log($"Scene instance GameObject: {foundInScene.gameObject.name}");
            Debug.Log($"Scene instance Active: {foundInScene.gameObject.activeInHierarchy}");
            Debug.Log($"Scene instance Enabled: {foundInScene.enabled}");
        }
        else
        {
            Debug.LogWarning("No PlayerStatisticsManager found in current scene!");
        }
    }
    #endregion
}

[System.Serializable]
public class PlayerData
{
    public Photon.Realtime.Player photonPlayer;
    public bool isAlive = true;
    public int handCount = 6;
    public int totalWins = 0;
    public int lives = 3;
}