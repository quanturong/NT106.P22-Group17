using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeManager : MonoBehaviour
{
    [Header("Heart Arrays")]
    public Image[] playerHearts;
    public Image[] enemyHearts;

    [Header("Heart Colors")]
    public Color normalHeartColor = Color.red; // Màu đỏ mặc định cho tim
    public Color darkHeartColor = new Color(0.3f, 0.3f, 0.3f, 1f); // Màu tối

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private int playerLives;
    private int enemyLives;

    void Start()
    {
        ResetHearts();
        ValidateSetup();
    }

    // ========== MAIN METHODS ĐỂ SET LIVES ==========

    /// <summary>
    /// Set số mạng cho người chơi với force update
    /// </summary>
    public void SetPlayerLives(int lives)
    {
        int clampedLives = Mathf.Clamp(lives, 0, playerHearts != null ? playerHearts.Length : 3);

        if (enableDebugLogs)
            Debug.Log($"LifeManager: SetPlayerLives called with {lives}, clamped to {clampedLives}");

        playerLives = clampedLives;

        // Force update ngay lập tức
        StartCoroutine(ForceUpdatePlayerHearts());
    }

    /// <summary>
    /// Set số mạng cho đối thủ với force update
    /// </summary>
    public void SetEnemyLives(int lives)
    {
        int clampedLives = Mathf.Clamp(lives, 0, enemyHearts != null ? enemyHearts.Length : 3);

        if (enableDebugLogs)
            Debug.Log($"LifeManager: SetEnemyLives called with {lives}, clamped to {clampedLives}");

        enemyLives = clampedLives;

        // Force update ngay lập tức
        StartCoroutine(ForceUpdateEnemyHearts());
    }

    // ========== BACKWARD COMPATIBILITY METHODS ==========

    /// <summary>
    /// Trừ 1 mạng của người chơi (method cũ)
    /// </summary>
    public void LosePlayerLife()
    {
        SetPlayerLives(playerLives - 1);
    }

    /// <summary>
    /// Trừ 1 mạng của đối thủ (method cũ)
    /// </summary>
    public void LoseEnemyLife()
    {
        SetEnemyLives(enemyLives - 1);
    }

    // ========== FORCE UPDATE COROUTINES ==========

    /// <summary>
    /// Force update player hearts với coroutine để đảm bảo UI cập nhật
    /// </summary>
    private IEnumerator ForceUpdatePlayerHearts()
    {
        yield return null; // Wait 1 frame để đảm bảo UI ready

        if (playerHearts == null)
        {
            Debug.LogError("LifeManager: playerHearts array is null!");
            yield break;
        }

        if (enableDebugLogs)
            Debug.Log($"Force updating PLAYER hearts: {playerLives} lives out of {playerHearts.Length}");

        for (int i = 0; i < playerHearts.Length; i++)
        {
            if (playerHearts[i] != null)
            {
                Color targetColor = i < playerLives ? normalHeartColor : darkHeartColor;
                playerHearts[i].color = targetColor;
                playerHearts[i].gameObject.SetActive(true);

                if (enableDebugLogs)
                    Debug.Log($"Player Heart {i}: Lives={playerLives}, Color={targetColor}, Active={i < playerLives}");
            }
            else
            {
                Debug.LogError($"LifeManager: playerHearts[{i}] is null!");
            }
        }

        if (enableDebugLogs)
            Debug.Log($"✅ COMPLETED: Force updated PLAYER hearts display to {playerLives} lives");
    }

    /// <summary>
    /// Force update enemy hearts với coroutine để đảm bảo UI cập nhật
    /// </summary>
    private IEnumerator ForceUpdateEnemyHearts()
    {
        yield return null; // Wait 1 frame để đảm bảo UI ready

        if (enemyHearts == null)
        {
            Debug.LogError("LifeManager: enemyHearts array is null!");
            yield break;
        }

        if (enableDebugLogs)
            Debug.Log($"Force updating ENEMY hearts: {enemyLives} lives out of {enemyHearts.Length}");

        for (int i = 0; i < enemyHearts.Length; i++)
        {
            if (enemyHearts[i] != null)
            {
                Color targetColor = i < enemyLives ? normalHeartColor : darkHeartColor;
                enemyHearts[i].color = targetColor;
                enemyHearts[i].gameObject.SetActive(true);

                if (enableDebugLogs)
                    Debug.Log($"Enemy Heart {i}: Lives={enemyLives}, Color={targetColor}, Active={i < enemyLives}");
            }
            else
            {
                Debug.LogError($"LifeManager: enemyHearts[{i}] is null!");
            }
        }

        if (enableDebugLogs)
            Debug.Log($"✅ COMPLETED: Force updated ENEMY hearts display to {enemyLives} lives");
    }

    // ========== TRADITIONAL UPDATE METHODS (FALLBACK) ==========

    /// <summary>
    /// Cập nhật hiển thị hearts của player (method truyền thống)
    /// </summary>
    private void UpdatePlayerHeartsDisplay()
    {
        if (playerHearts == null) return;

        for (int i = 0; i < playerHearts.Length; i++)
        {
            if (playerHearts[i] != null)
            {
                playerHearts[i].color = i < playerLives ? normalHeartColor : darkHeartColor;
                playerHearts[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Cập nhật hiển thị hearts của enemy (method truyền thống)
    /// </summary>
    private void UpdateEnemyHeartsDisplay()
    {
        if (enemyHearts == null) return;

        for (int i = 0; i < enemyHearts.Length; i++)
        {
            if (enemyHearts[i] != null)
            {
                enemyHearts[i].color = i < enemyLives ? normalHeartColor : darkHeartColor;
                enemyHearts[i].gameObject.SetActive(true);
            }
        }
    }

    // ========== RESET VÀ KHỞI TẠO ==========

    /// <summary>
    /// Reset tất cả hearts về trạng thái ban đầu (full lives)
    /// </summary>
    public void ResetHearts()
    {
        // Set về số mạng tối đa
        playerLives = playerHearts != null ? playerHearts.Length : 3;
        enemyLives = enemyHearts != null ? enemyHearts.Length : 3;

        // Cập nhật hiển thị
        UpdatePlayerHeartsDisplay();
        UpdateEnemyHeartsDisplay();

        if (enableDebugLogs)
            Debug.Log($"LifeManager: Reset hearts - Player: {playerLives}, Enemy: {enemyLives}");
    }

    // ========== CONFIGURATION ==========

    /// <summary>
    /// Thay đổi màu sắc của hearts
    /// </summary>
    public void SetHeartColors(Color normal, Color dark)
    {
        normalHeartColor = normal;
        darkHeartColor = dark;

        // Cập nhật lại hiển thị với màu mới
        UpdatePlayerHeartsDisplay();
        UpdateEnemyHeartsDisplay();

        if (enableDebugLogs)
            Debug.Log($"LifeManager: Updated heart colors - Normal: {normal}, Dark: {dark}");
    }

    // ========== GETTER METHODS ==========

    /// <summary>
    /// Lấy số mạng hiện tại của player
    /// </summary>
    public int GetPlayerLives() => playerLives;

    /// <summary>
    /// Lấy số mạng hiện tại của enemy
    /// </summary>
    public int GetEnemyLives() => enemyLives;

    /// <summary>
    /// Lấy số mạng tối đa của player
    /// </summary>
    public int GetMaxPlayerLives() => playerHearts != null ? playerHearts.Length : 3;

    /// <summary>
    /// Lấy số mạng tối đa của enemy
    /// </summary>
    public int GetMaxEnemyLives() => enemyHearts != null ? enemyHearts.Length : 3;

    // ========== VALIDATION METHODS ==========

    /// <summary>
    /// Kiểm tra xem player còn sống không
    /// </summary>
    public bool IsPlayerAlive() => playerLives > 0;

    /// <summary>
    /// Kiểm tra xem enemy còn sống không
    /// </summary>
    public bool IsEnemyAlive() => enemyLives > 0;

    /// <summary>
    /// Kiểm tra setup hearts có hợp lệ không
    /// </summary>
    public bool ValidateSetup()
    {
        bool isValid = true;

        if (playerHearts == null || playerHearts.Length == 0)
        {
            Debug.LogError("LifeManager: playerHearts array is null or empty! Please assign in Inspector!");
            isValid = false;
        }

        if (enemyHearts == null || enemyHearts.Length == 0)
        {
            Debug.LogError("LifeManager: enemyHearts array is null or empty! Please assign in Inspector!");
            isValid = false;
        }

        // Kiểm tra các hearts có null không
        if (playerHearts != null)
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                if (playerHearts[i] == null)
                {
                    Debug.LogError($"LifeManager: playerHearts[{i}] is null! Please assign all heart images!");
                    isValid = false;
                }
            }
        }

        if (enemyHearts != null)
        {
            for (int i = 0; i < enemyHearts.Length; i++)
            {
                if (enemyHearts[i] == null)
                {
                    Debug.LogError($"LifeManager: enemyHearts[{i}] is null! Please assign all heart images!");
                    isValid = false;
                }
            }
        }

        if (enableDebugLogs)
        {
            if (isValid)
                Debug.Log("✅ LifeManager setup validation PASSED");
            else
                Debug.LogError("❌ LifeManager setup validation FAILED");
        }

        return isValid;
    }

    // ========== DEBUG METHODS ==========

    /// <summary>
    /// Debug hiển thị trạng thái hiện tại
    /// </summary>
    public void DebugHeartStatus()
    {
        Debug.Log("=================== LIFE MANAGER STATUS ===================");
        Debug.Log($"Player Lives: {playerLives}/{GetMaxPlayerLives()} (Alive: {IsPlayerAlive()})");
        Debug.Log($"Enemy Lives: {enemyLives}/{GetMaxEnemyLives()} (Alive: {IsEnemyAlive()})");
        Debug.Log($"Normal Heart Color: {normalHeartColor}");
        Debug.Log($"Dark Heart Color: {darkHeartColor}");
        Debug.Log($"Setup Valid: {ValidateSetup()}");
        Debug.Log($"Debug Logs Enabled: {enableDebugLogs}");

        // Debug individual hearts
        if (playerHearts != null)
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                if (playerHearts[i] != null)
                {
                    Debug.Log($"Player Heart {i}: Active={playerHearts[i].gameObject.activeSelf}, Color={playerHearts[i].color}");
                }
            }
        }

        if (enemyHearts != null)
        {
            for (int i = 0; i < enemyHearts.Length; i++)
            {
                if (enemyHearts[i] != null)
                {
                    Debug.Log($"Enemy Heart {i}: Active={enemyHearts[i].gameObject.activeSelf}, Color={enemyHearts[i].color}");
                }
            }
        }

        Debug.Log("========================================================");
    }

    // ========== TEST METHODS (Context Menu) ==========

    /// <summary>
    /// Test method để kiểm tra việc mất mạng player
    /// </summary>
    [ContextMenu("Test Lose Player Life")]
    public void TestLosePlayerLife()
    {
        Debug.Log("🧪 TESTING: Lose Player Life");
        LosePlayerLife();
        DebugHeartStatus();
    }

    /// <summary>
    /// Test method để kiểm tra việc mất mạng enemy
    /// </summary>
    [ContextMenu("Test Lose Enemy Life")]
    public void TestLoseEnemyLife()
    {
        Debug.Log("🧪 TESTING: Lose Enemy Life");
        LoseEnemyLife();
        DebugHeartStatus();
    }

    /// <summary>
    /// Test method để reset hearts
    /// </summary>
    [ContextMenu("Test Reset Hearts")]
    public void TestResetHearts()
    {
        Debug.Log("🧪 TESTING: Reset Hearts");
        ResetHearts();
        DebugHeartStatus();
    }

    /// <summary>
    /// Test method để set player lives trực tiếp
    /// </summary>
    [ContextMenu("Test Set Player Lives to 1")]
    public void TestSetPlayerLives()
    {
        Debug.Log("🧪 TESTING: Set Player Lives to 1");
        SetPlayerLives(1);
        DebugHeartStatus();
    }

    /// <summary>
    /// Test method để set enemy lives trực tiếp
    /// </summary>
    [ContextMenu("Test Set Enemy Lives to 2")]
    public void TestSetEnemyLives()
    {
        Debug.Log("🧪 TESTING: Set Enemy Lives to 2");
        SetEnemyLives(2);
        DebugHeartStatus();
    }

    /// <summary>
    /// Test method để force update tất cả
    /// </summary>
    [ContextMenu("Force Update All Hearts")]
    public void ForceUpdateAllHearts()
    {
        Debug.Log("🧪 TESTING: Force Update All Hearts");
        StartCoroutine(ForceUpdatePlayerHearts());
        StartCoroutine(ForceUpdateEnemyHearts());
    }

    // ========== UNITY LIFECYCLE ==========

    void Awake()
    {
        if (enableDebugLogs)
            Debug.Log("LifeManager: Awake called");
    }

    void OnValidate()
    {
        // Cập nhật colors khi thay đổi trong Inspector
        if (Application.isPlaying)
        {
            UpdatePlayerHeartsDisplay();
            UpdateEnemyHeartsDisplay();

            if (enableDebugLogs)
                Debug.Log("LifeManager: OnValidate - Updated heart colors");
        }
    }

    void OnDestroy()
    {
        if (enableDebugLogs)
            Debug.Log("LifeManager: OnDestroy called");
    }
}