using UnityEngine;
using UnityEngine.SceneManagement;
using JSG.FortuneSpinWheel;

/// <summary>
/// Script này gắn vào SpinWheel scene để kết nối kết quả Russian Roulette với Liar's Bar
/// </summary>
public class RouletteResultConnector : MonoBehaviour
{
    [Header("Scene Management")]
    public string gameSceneName = "GameUI"; // Tên scene game chính
    public float delayBeforeReturn = 2f; // Thời gian delay trước khi quay về

    [Header("UI References")]
    public GameObject rouletteInstructions; // Panel hiển thị hướng dẫn
    public TMPro.TextMeshProUGUI instructionText;

    private FortuneSpinWheel fortuneWheel;
    private bool resultProcessed = false;

    // Constants
    private const string PUNISHMENT_RESULT_KEY = "RouletteResult";
    private const string PUNISHED_PLAYER_KEY = "PunishedPlayer";

    void Start()
    {
        // Tìm FortuneSpinWheel component
        fortuneWheel = FindObjectOfType<FortuneSpinWheel>();

        if (fortuneWheel == null)
        {
            Debug.LogError("FortuneSpinWheel not found in scene!");
            return;
        }

        // Hiển thị hướng dẫn
        ShowInstructions();

        // Override reward handling
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

        // Ẩn hướng dẫn sau 3 giây
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
        // Đảm bảo wheel có ít nhất 1 ô đặc biệt (death)
        bool hasDeathSlot = false;
        for (int i = 0; i < fortuneWheel.m_RewardData.Length; i++)
        {
            if (fortuneWheel.m_RewardData[i].m_IsSpecialReset)
            {
                hasDeathSlot = true;
                // Đổi tên thành "DEATH" cho rõ ràng
                fortuneWheel.m_RewardData[i].m_Title = "DEATH";
                break;
            }
        }

        if (!hasDeathSlot)
        {
            Debug.LogWarning("No special reset slot found! Russian Roulette won't work properly.");
        }

        // Reset tất cả ô về trạng thái chưa obtained
        for (int i = 0; i < fortuneWheel.m_RewardData.Length; i++)
        {
            fortuneWheel.m_RewardData[i].m_IsObtained = false;
        }

        // Hook vào reward handling
        StartCoroutine(MonitorRouletteResult());
    }

    System.Collections.IEnumerator MonitorRouletteResult()
    {
        // Chờ cho đến khi có kết quả
        while (!resultProcessed)
        {
            // Kiểm tra xem wheel có đang quay không
            if (!fortuneWheel.m_IsSpinning && fortuneWheel.m_RewardNumber >= 0)
            {
                // Có kết quả rồi
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
        bool died = hitReward.m_IsSpecialReset; // Nếu trúng ô đặc biệt = chết

        Debug.Log($"Roulette result: {(died ? "DEATH" : "SAFE")}");

        // Lưu kết quả vào PlayerPrefs
        PlayerPrefs.SetInt(PUNISHMENT_RESULT_KEY, died ? 1 : 0);

        // Hiển thị kết quả
        ShowResult(died);

        // Quay về game scene sau delay
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
        // Quay về scene game chính
        SceneManager.LoadScene(gameSceneName);
    }

    void Update()
    {
        // Cho phép ESC để quay về sớm (debug)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToGameScene();
        }
    }
}