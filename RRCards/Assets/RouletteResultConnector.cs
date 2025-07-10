using UnityEngine;
using UnityEngine.SceneManagement;
using JSG.FortuneSpinWheel;

public class RouletteResultConnector : MonoBehaviour
{
    [Header("Scene Management")]
    public string gameSceneName = "GameUI";
    public float delayBeforeReturn = 2f;

    [Header("UI References")]
    public GameObject rouletteInstructions;
    public TMPro.TextMeshProUGUI instructionText;

    private FortuneSpinWheel fortuneWheel;
    private bool resultProcessed = false;

    private const string PUNISHMENT_RESULT_KEY = "RouletteResult";
    private const string PUNISHED_PLAYER_KEY = "PunishedPlayer";

    void Start()
    {
        Debug.Log("=== RouletteResultConnector START ===");

        fortuneWheel = FindObjectOfType<FortuneSpinWheel>();

        if (fortuneWheel == null)
        {
            Debug.LogError("FortuneSpinWheel not found in scene!");
            return;
        }

        // CRITICAL: Get punished player from PlayerPrefs
        int punishedPlayer = PlayerPrefs.GetInt(PUNISHED_PLAYER_KEY, -1);
        Debug.Log($"Punished player ActorNumber: {punishedPlayer}");

        if (punishedPlayer == -1)
        {
            Debug.LogError("No punished player found! This shouldn't happen!");
        }

        ShowInstructions();
        SetupRouletteLogic();
    }

    void ShowInstructions()
    {
        if (rouletteInstructions)
        {
            rouletteInstructions.SetActive(true);
        }

        if (instructionText)
        {
            instructionText.text = "🎲 RUSSIAN ROULETTE\n\nSpin the wheel...\nIf you hit the DEATH slot, you're eliminated!\nAny other slot and you survive!";
        }

        Invoke(nameof(HideInstructions), 3f);
    }

    void HideInstructions()
    {
        if (rouletteInstructions)
        {
            rouletteInstructions.SetActive(false);
        }
    }

    void SetupRouletteLogic()
    {
        bool hasDeathSlot = false;
        for (int i = 0; i < fortuneWheel.m_RewardData.Length; i++)
        {
            if (fortuneWheel.m_RewardData[i].m_IsSpecialReset)
            {
                hasDeathSlot = true;
                fortuneWheel.m_RewardData[i].m_Title = "DEATH";
                break;
            }
        }

        if (!hasDeathSlot)
        {
            Debug.LogWarning("No special reset slot found! Russian Roulette won't work properly.");
        }

        for (int i = 0; i < fortuneWheel.m_RewardData.Length; i++)
        {
            fortuneWheel.m_RewardData[i].m_IsObtained = false;
        }

        StartCoroutine(MonitorRouletteResult());
    }

    System.Collections.IEnumerator MonitorRouletteResult()
    {
        while (!resultProcessed)
        {
            if (!fortuneWheel.m_IsSpinning && fortuneWheel.m_RewardNumber >= 0)
            {
                ProcessRouletteResult();
                resultProcessed = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void ProcessRouletteResult()
    {
        if (fortuneWheel.m_RewardNumber < 0 || fortuneWheel.m_RewardNumber >= fortuneWheel.m_RewardData.Length)
        {
            Debug.LogError("Invalid reward number!");
            return;
        }

        var hitReward = fortuneWheel.m_RewardData[fortuneWheel.m_RewardNumber];
        bool died = hitReward.m_IsSpecialReset;

        Debug.Log($"=== ROULETTE RESULT ===");
        Debug.Log($"Reward slot: {fortuneWheel.m_RewardNumber}");
        Debug.Log($"Result: {(died ? "DEATH" : "SAFE")}");

        // CRITICAL: Lưu cả kết quả VÀ player bị phạt
        PlayerPrefs.SetInt(PUNISHMENT_RESULT_KEY, died ? 1 : 0);

        // KHÔNG GHI ĐÈ punished player - giữ nguyên value từ GameManager
        int punishedPlayer = PlayerPrefs.GetInt(PUNISHED_PLAYER_KEY, -1);
        Debug.Log($"Maintaining punished player: {punishedPlayer}");

        // SET FLAG để GameManager biết đã có kết quả roulette
        PlayerPrefs.SetString("RouletteCompleted", "true");
        PlayerPrefs.Save();

        ShowResult(died);
        Invoke(nameof(ReturnToGameScene), delayBeforeReturn);
    }

    void ShowResult(bool died)
    {
        if (rouletteInstructions)
        {
            rouletteInstructions.SetActive(true);
        }

        if (instructionText)
        {
            if (died)
            {
                instructionText.text = "💀 ELIMINATED!\n\nYou hit the death slot...\nBetter luck next game!";
                instructionText.color = Color.red;
            }
            else
            {
                instructionText.text = "😅 YOU SURVIVED!\n\nLucky escape!\nReturning to game...";
                instructionText.color = Color.green;
            }
        }
    }

    void ReturnToGameScene()
    {
        Debug.Log("=== RETURNING TO GAME SCENE ===");

        // Debug final state
        int result = PlayerPrefs.GetInt(PUNISHMENT_RESULT_KEY, -1);
        int player = PlayerPrefs.GetInt(PUNISHED_PLAYER_KEY, -1);
        Debug.Log($"Final result: {result} (0=survived, 1=died)");
        Debug.Log($"Final punished player: {player}");

        SceneManager.LoadScene(gameSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToGameScene();
        }
    }
}