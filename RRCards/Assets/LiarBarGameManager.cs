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
    public int currentRound = 1;
    public List<CardData> middlePile = new List<CardData>();
    private List<CardData> playedCardsThisTurn = new List<CardData>();

    [Header("UI References")]
    public TextMeshProUGUI gameStatusText;
    public GameObject playCardPanel;
    public GameObject challengePanel;
    public Button challengeButton;
    public Image targetCardImage;

    [Header("Popup Info")]
    public GameObject popupInfoPanel;
    public TextMeshProUGUI popupInfoText;
    public Button popupOkButton;

    [Header("Scene Names")]
    public string victorySceneName = "Victory";
    public string defeatSceneName = "Defeat";
    public string rouletteSceneName = "SpinWheel";

    [Header("Player Management")]
    public List<PlayerData> players = new List<PlayerData>();
    public LiarBarHandManager localHandManager;
    public TextMeshProUGUI[] playerNameTexts;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip cardPlaySound;
    public AudioClip challengeSound;
    public AudioClip roundStartSound;
    public AudioClip victorySound;
    public AudioClip defeatSound;

    [Header("Card Sprites for Target Display")]
    public Sprite[] cardSprites;
    public string[] cardNames = { "K", "Q", "J", "A", "Joker" };

    [Header("Visual Effects")]
    public ParticleSystem confettiEffect;
    public Image backgroundImage;
    public Color normalBgColor = Color.white;
    public Color tenseBgColor = Color.red;

    private bool isMyTurn = false;
    private bool waitingForResponse = false;
    private int punishedPlayerActorNumber = -1;
    private int lastChallengedActor = -1;

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
        RoundEnd,
        GameOver
    }

    void Start()
    {
        InitializeGame();
        SetupUI();
        ShowRoundPopup(); // Hiện popup khi start game/round
    }

    void InitializeGame()
    {
        players.Clear();
        var photonPlayers = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToArray();
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
        UpdatePlayerDisplay();
        if (PhotonNetwork.IsMasterClient)
        {
            StartNewRound();
        }
    }

    void SetupUI()
    {
        if (challengeButton != null)
            challengeButton.onClick.AddListener(ChallengePlay);

        if (popupOkButton != null)
            popupOkButton.onClick.AddListener(HidePopupInfo);

        UpdateUI();
    }

    void StartNewRound()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        currentRound++;
        middlePile.Clear();

        string newTargetCard = cardNames[Random.Range(0, cardNames.Length)];
        photonView.RPC("NewRoundStarted", RpcTarget.All, newTargetCard, currentRound);
    }

    [PunRPC]
    void NewRoundStarted(string targetCard, int roundNumber)
    {
        currentTargetCard = targetCard;
        currentRound = roundNumber;
        currentState = GameState.RoundStart;
        playedCardsThisTurn.Clear();
        UpdateTargetCardDisplay();
        ShowRoundPopup();
        UpdateUI();
        PlaySound(roundStartSound);

        Invoke(nameof(StartPlayerTurn), 2f);
    }

    void ShowRoundPopup()
    {
        // Gộp thông tin round, target, player list vào popup
        if (popupInfoPanel != null && popupInfoText != null)
        {
            string popup = $"<b>ROUND {currentRound}</b>\n" +
                $"<b>Target:</b> {currentTargetCard}\n\n<b>Players:</b>\n";
            foreach (var p in players)
            {
                string status = p.isAlive ? "🟢" : "💀";
                popup += $"{status} {p.photonPlayer.NickName}\n";
            }
            popupInfoText.text = popup;
            popupInfoPanel.SetActive(true);
        }
    }

    void HidePopupInfo()
    {
        if (popupInfoPanel != null)
            popupInfoPanel.SetActive(false);
    }

    void UpdateTargetCardDisplay()
    {
        if (targetCardImage != null && cardSprites != null)
        {
            for (int i = 0; i < cardNames.Length; i++)
            {
                if (cardNames[i] == currentTargetCard && i < cardSprites.Length)
                {
                    targetCardImage.sprite = cardSprites[i];
                    break;
                }
            }
        }
    }

    void StartPlayerTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Chọn player còn sống tiếp theo
            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
            while (!players[currentPlayerIndex].isAlive);

            photonView.RPC("UpdateGameState", RpcTarget.All, (int)GameState.PlayerPlaying, currentPlayerIndex);
        }
    }

    void UpdatePlayerDisplay()
    {
        // Nếu cần update UI khác cho player, thêm tại đây
    }

    void UpdateUI()
    {
        if (players.Count == 0 || currentPlayerIndex >= players.Count) return;
        isMyTurn = players[currentPlayerIndex].photonPlayer == Photon.Pun.PhotonNetwork.LocalPlayer;
        switch (currentState)
        {
            case GameState.RoundStart:
                if (gameStatusText)
                    gameStatusText.text = $"🎯 NEW ROUND!\nTarget Card: {currentTargetCard}\nEveryone must play this card!";
                if (playCardPanel) playCardPanel.SetActive(false);
                if (challengePanel) challengePanel.SetActive(false);
                break;
            case GameState.PlayerPlaying:
                if (isMyTurn)
                {
                    if (gameStatusText)
                        gameStatusText.text = $"🃏 Your turn!\nPlay 1-3 {currentTargetCard} cards\n(You can lie about having them!)";
                    if (playCardPanel) playCardPanel.SetActive(true);
                    if (challengePanel) challengePanel.SetActive(false);
                }
                else
                {
                    if (gameStatusText)
                        gameStatusText.text = $"⏳ {players[currentPlayerIndex].photonPlayer.NickName} is playing...";
                    if (playCardPanel) playCardPanel.SetActive(false);
                    if (challengePanel) challengePanel.SetActive(false);
                }
                break;
            case GameState.WaitingForChallenge:
                if (!isMyTurn)
                {
                    if (gameStatusText)
                        gameStatusText.text = $"🎭 {players[currentPlayerIndex].photonPlayer.NickName} played! Do you believe them?";
                    if (playCardPanel) playCardPanel.SetActive(false);
                    if (challengePanel) challengePanel.SetActive(true);
                }
                else
                {
                    if (gameStatusText) gameStatusText.text = "⏳ Waiting for others to decide...";
                    if (playCardPanel) playCardPanel.SetActive(false);
                    if (challengePanel) challengePanel.SetActive(false);
                }
                break;
                // ... (Các state khác giữ nguyên, nếu có)
        }
    }

    public void ChallengePlay()
    {
        if (currentState != GameState.WaitingForChallenge) return;
        photonView.RPC("PlayChallenged", RpcTarget.All, Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);
        PlaySound(challengeSound);
    }

    // ... (Các hàm còn lại như RevealCardsAndJudge, StartRussianRoulette, v.v. giữ nguyên logic)
    // ...

    void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
            audioSource.PlayOneShot(clip);
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
