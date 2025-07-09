using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class LiarBarGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Game State")]
    public GameState currentState = GameState.WaitingForPlayers;
    public int currentPlayerIndex = 0;
    public string currentTargetCard = "";
    public int currentRound = 0;
    public List<CardData> middlePile = new List<CardData>();
    private List<CardData> playedCardsThisTurn = new List<CardData>();

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

    [Header("Scene Names")]
    public string victorySceneName = "Victory";
    public string defeatSceneName = "Defeat";
    public string rouletteSceneName = "SpinWheel";

    [Header("Player Management")]
    public List<PlayerData> players = new List<PlayerData>();
    public LiarBarHandManager localHandManager;

    [Header("Timer Settings")]
    public float playTimeLimit = 15f;
    public float challengeTimeLimit = 10f;
    private float currentTimer = 0f;
    private bool timerActive = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cardPlaySound;
    public AudioClip challengeSound;
    public AudioClip roundStartSound;
    public AudioClip timerTickSound;
    public AudioClip victorySound;
    public AudioClip defeatSound;

    [Header("Visual Effects")]
    public ParticleSystem confettiEffect;

    private bool isMyTurn = false;
    private int cardsPlayedThisTurn = 0;
    private int playersReady = 0;

    private const string PUNISHMENT_RESULT_KEY = "RouletteResult";
    private const string PUNISHED_PLAYER_KEY = "PunishedPlayer";

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

    void Start()
    {
        InitializeGame();
        SetupUI();
        CheckRouletteResult();
    }

    void InitializeGame()
    {
        players.Clear();
        currentRound = 0;
        playersReady = 0;

        var photonPlayers = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToArray();

        if (photonPlayers.Length != 2)
        {
            Debug.LogError("Liar's Bar 1v1 requires exactly 2 players!");
            return;
        }

        for (int i = 0; i < photonPlayers.Length; i++)
        {
            players.Add(new PlayerData
            {
                photonPlayer = photonPlayers[i],
                isAlive = true,
                handCount = 6,
                totalWins = 0
            });
        }

        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();
        }
    }

    void SetupUI()
    {
        if (playButton != null)
            playButton.onClick.AddListener(PlayCards);
        if (liarButton != null)
            liarButton.onClick.AddListener(ChallengeLiar);
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipAction);
        if (popupOkButton != null)
            popupOkButton.onClick.AddListener(OnPopupOk);

        if (popupInfoPanel) popupInfoPanel.SetActive(false);
        if (playPanel) playPanel.SetActive(false);
        if (challengePanel) challengePanel.SetActive(false);

        if (roundDisplayText) roundDisplayText.text = "";
        if (gameStatusText) gameStatusText.text = "Waiting...";

        UpdateUI();
    }

    void StartNewRound()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        currentRound++;
        middlePile.Clear();
        playedCardsThisTurn.Clear();
        playersReady = 0;

        string[] cardNames = { "K", "Q", "J", "A", "Joker" };
        string newTargetCard = cardNames[Random.Range(0, cardNames.Length)];

        photonView.RPC("NewRoundStarted", RpcTarget.All, newTargetCard, currentRound);
    }

    [PunRPC]
    void NewRoundStarted(string targetCard, int roundNumber)
    {
        Debug.Log($"NewRoundStarted called - Round: {roundNumber}, Target: {targetCard}");

        currentTargetCard = targetCard;
        currentRound = roundNumber;
        currentState = GameState.RoundStart;
        playedCardsThisTurn.Clear();

        // Force show popup for ALL players
        Invoke(nameof(ShowRoundPopup), 0.1f);
        PlaySound(roundStartSound);
    }

    void ShowRoundPopup()
    {
        Debug.Log("ShowRoundPopup called");

        if (popupInfoPanel != null)
        {
            Debug.Log("Popup panel found, showing popup");

            if (popupRoundInfo != null)
                popupRoundInfo.text = $"ROUND {currentRound}";

            if (popupTargetCardInfo != null)
                popupTargetCardInfo.text = $"TARGET: {currentTargetCard}";

            popupInfoPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("popupInfoPanel is NULL! Assign it in Inspector!");
        }
    }

    public void OnPopupOk()
    {
        Debug.Log("OnPopupOk called!");

        if (popupInfoPanel != null)
        {
            Debug.Log("Hiding popup panel");
            popupInfoPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("popupInfoPanel is NULL!");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Client starting player turn directly");
            StartPlayerTurn();
        }
        else
        {
            Debug.Log("Not Master Client, waiting for turn start");
        }
    }

    [PunRPC]
    void PlayerReadyForRound(int playerActorNumber)
    {
        playersReady++;
        Debug.Log($"Player {playerActorNumber} ready. Total ready: {playersReady}");

        if (roundDisplayText) roundDisplayText.text = "";

        if (playersReady >= PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
        {
            playersReady = 0;
            Debug.Log("All players ready, starting player turn");
            StartPlayerTurn();
        }
    }

    public void TestClosePopup()
    {
        Debug.Log("TestClosePopup called!");
        if (popupInfoPanel != null)
            popupInfoPanel.SetActive(false);
    }

    void StartPlayerTurn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        currentPlayerIndex = 0;
        while (currentPlayerIndex < players.Count && !players[currentPlayerIndex].isAlive)
        {
            currentPlayerIndex++;
        }

        photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
    }

    void CheckRouletteResult()
    {
        if (PlayerPrefs.HasKey(PUNISHMENT_RESULT_KEY))
        {
            bool died = PlayerPrefs.GetInt(PUNISHMENT_RESULT_KEY) == 1;
            int punishedPlayer = PlayerPrefs.GetInt(PUNISHED_PLAYER_KEY, -1);

            PlayerPrefs.DeleteKey(PUNISHMENT_RESULT_KEY);
            PlayerPrefs.DeleteKey(PUNISHED_PLAYER_KEY);

            if (punishedPlayer == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                photonView.RPC("RouletteResult", RpcTarget.All, punishedPlayer, died);
            }
        }
    }

    void UpdateUI()
    {
        if (players.Count == 0 || currentPlayerIndex >= players.Count) return;

        isMyTurn = players[currentPlayerIndex].photonPlayer == PhotonNetwork.LocalPlayer;

        switch (currentState)
        {
            case GameState.RoundStart:
                break;

            case GameState.PlayerPlaying:
                if (isMyTurn)
                {
                    if (playPanel) playPanel.SetActive(true);
                    if (challengePanel) challengePanel.SetActive(false);
                    if (skipButton) skipButton.gameObject.SetActive(true);
                    StartPlayTimer();
                }
                else
                {
                    if (playPanel) playPanel.SetActive(false);
                    if (challengePanel) challengePanel.SetActive(false);
                    if (skipButton) skipButton.gameObject.SetActive(false);
                }
                break;

            case GameState.WaitingForChallenge:
                if (!isMyTurn)
                {
                    if (challengePanel) challengePanel.SetActive(true);
                    if (playPanel) playPanel.SetActive(false);
                    if (skipButton) skipButton.gameObject.SetActive(true);
                    StartChallengeTimer();
                }
                else
                {
                    if (playPanel) playPanel.SetActive(false);
                    if (challengePanel) challengePanel.SetActive(false);
                    if (skipButton) skipButton.gameObject.SetActive(false);
                }
                break;

            case GameState.RevealingCards:
                if (gameStatusText) gameStatusText.text = "Revealing...";
                if (playPanel) playPanel.SetActive(false);
                if (challengePanel) challengePanel.SetActive(false);
                if (skipButton) skipButton.gameObject.SetActive(false);
                break;

            case GameState.WaitingForRoulette:
                if (gameStatusText) gameStatusText.text = "Roulette...";
                if (playPanel) playPanel.SetActive(false);
                if (challengePanel) challengePanel.SetActive(false);
                if (skipButton) skipButton.gameObject.SetActive(false);
                break;
        }
    }

    void StartPlayTimer()
    {
        currentTimer = playTimeLimit;
        timerActive = true;
        if (gameStatusText)
            gameStatusText.text = isMyTurn ? "Your Turn" : "Opponent Turn";
    }

    void StartChallengeTimer()
    {
        currentTimer = challengeTimeLimit;
        timerActive = true;
        if (gameStatusText)
            gameStatusText.text = !isMyTurn ? "Challenge?" : "Waiting...";
    }

    void StopTimer()
    {
        timerActive = false;
    }

    void Update()
    {
        if (timerActive && gameStatusText)
        {
            currentTimer -= Time.deltaTime;

            string baseText = "";
            if (currentState == GameState.PlayerPlaying)
            {
                baseText = isMyTurn ? "Your Turn" : "Opponent Turn";
            }
            else if (currentState == GameState.WaitingForChallenge)
            {
                baseText = !isMyTurn ? "Challenge?" : "Waiting...";
            }

            if (!string.IsNullOrEmpty(baseText))
            {
                gameStatusText.text = $"{baseText} ({Mathf.Ceil(currentTimer)}s)";
            }

            if (currentTimer <= 3f && currentTimer > 2f)
            {
                PlaySound(timerTickSound);
            }

            if (currentTimer <= 0)
            {
                HandleTimerExpired();
            }
        }
        else if (!timerActive && gameStatusText)
        {
            // When timer is not active, show appropriate status
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
    }

    void HandleTimerExpired()
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

    public void PlayCards()
    {
        if (!isMyTurn || currentState != GameState.PlayerPlaying) return;

        var selectedCards = GetSelectedCardsFromHand();

        if (selectedCards.Count == 0)
        {
            if (gameStatusText)
                gameStatusText.text = "Select 1-3 cards first!";
            return;
        }

        if (selectedCards.Count > 3)
        {
            if (gameStatusText)
                gameStatusText.text = "Maximum 3 cards allowed!";
            return;
        }

        StopTimer();

        playedCardsThisTurn.Clear();
        playedCardsThisTurn.AddRange(selectedCards);

        photonView.RPC("ReceiveCardPlay", RpcTarget.All, selectedCards.Count, PhotonNetwork.LocalPlayer.ActorNumber);

        foreach (var card in selectedCards)
        {
            if (localHandManager != null)
                localHandManager.RemoveCard(card);
            middlePile.Add(card);
        }

        if (localHandManager != null)
            localHandManager.ClearSelection();

        PlaySound(cardPlaySound);
    }

    List<CardData> GetSelectedCardsFromHand()
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

    void AcceptPlay()
    {
        StopTimer();
        photonView.RPC("PlayAccepted", RpcTarget.All);
    }

    [PunRPC]
    void ReceiveCardPlay(int cardCount, int playerActorNumber)
    {
        cardsPlayedThisTurn = cardCount;
        currentState = GameState.WaitingForChallenge;

        UpdateUI();
    }

    [PunRPC]
    void PlayAccepted()
    {
        StopTimer();

        if (gameStatusText) gameStatusText.text = "Play accepted!";

        var currentPlayer = players[currentPlayerIndex];
        if (localHandManager != null && localHandManager.GetCurrentHand().Count == 0 &&
            currentPlayer.photonPlayer == PhotonNetwork.LocalPlayer)
        {
            photonView.RPC("PlayerWon", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
            return;
        }

        Invoke(nameof(NextPlayerTurn), 1.5f);
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

    void RevealCardsAndJudge()
    {
        bool allCardsAreTarget = playedCardsThisTurn.All(card => card.cardName == currentTargetCard);

        var player = players[currentPlayerIndex];
        var challenger = GetPlayerByActorNumber(
            PhotonNetwork.PlayerList.FirstOrDefault(p => p != player.photonPlayer)?.ActorNumber ?? -1);

        if (allCardsAreTarget)
        {
            if (gameStatusText)
                gameStatusText.text = $"{player.photonPlayer.NickName} was honest! {challenger.photonPlayer.NickName} gets punished!";
            photonView.RPC("StartRussianRoulette", RpcTarget.All, challenger.photonPlayer.ActorNumber);
        }
        else
        {
            if (gameStatusText)
                gameStatusText.text = $"{player.photonPlayer.NickName} was LYING! They get punished!";
            photonView.RPC("StartRussianRoulette", RpcTarget.All, player.photonPlayer.ActorNumber);
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

        if (PhotonNetwork.LocalPlayer.ActorNumber == punishedPlayerActorNumber)
        {
            StartCoroutine(LoadRouletteScene());
        }
    }

    System.Collections.IEnumerator LoadRouletteScene()
    {
        yield return new WaitForSeconds(2f);

        PlayerPrefs.SetInt(PUNISHED_PLAYER_KEY, PhotonNetwork.LocalPlayer.ActorNumber);
        SceneManager.LoadScene(rouletteSceneName);
    }

    [PunRPC]
    void RouletteResult(int playerActorNumber, bool died)
    {
        var player = GetPlayerByActorNumber(playerActorNumber);

        if (died)
        {
            player.isAlive = false;
            if (gameStatusText)
                gameStatusText.text = $"{player.photonPlayer.NickName} was eliminated!";
            PlaySound(defeatSound);

            var winner = players.FirstOrDefault(p => p.isAlive);
            if (winner != null)
            {
                photonView.RPC("GameOver", RpcTarget.All, winner.photonPlayer.ActorNumber);
                return;
            }
        }
        else
        {
            if (gameStatusText)
                gameStatusText.text = $"{player.photonPlayer.NickName} survived!";

            if (PhotonNetwork.IsMasterClient)
            {
                Invoke(nameof(StartNewRound), 3f);
            }
        }
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

        if (winner.photonPlayer == PhotonNetwork.LocalPlayer)
        {
            PlaySound(victorySound);
            if (confettiEffect) confettiEffect.Play();
            StartCoroutine(LoadVictoryScene());
        }
        else
        {
            PlaySound(defeatSound);
            StartCoroutine(LoadDefeatScene());
        }
    }

    System.Collections.IEnumerator LoadVictoryScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(victorySceneName);
    }

    System.Collections.IEnumerator LoadDefeatScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(defeatSceneName);
    }

    void NextPlayerTurn()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        playedCardsThisTurn.Clear();

        currentPlayerIndex = (currentPlayerIndex + 1) % 2;

        if (!players[currentPlayerIndex].isAlive)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % 2;
        }

        photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
    }

    [PunRPC]
    void UpdateGameState(int newState, int newPlayerIndex)
    {
        currentState = (GameState)newState;
        currentPlayerIndex = newPlayerIndex;
        UpdateUI();
    }

    PlayerData GetPlayerByActorNumber(int actorNumber)
    {
        return players.FirstOrDefault(p => p.photonPlayer.ActorNumber == actorNumber);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentPlayerIndex);
            stream.SendNext(currentTargetCard);
            stream.SendNext(currentRound);
            stream.SendNext((int)currentState);
        }
        else
        {
            currentPlayerIndex = (int)stream.ReceiveNext();
            currentTargetCard = (string)stream.ReceiveNext();
            currentRound = (int)stream.ReceiveNext();
            currentState = (GameState)stream.ReceiveNext();
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public Photon.Realtime.Player photonPlayer;
    public bool isAlive = true;
    public int handCount = 6;
    public int totalWins = 0;
}