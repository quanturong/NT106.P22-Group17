using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeManager : MonoBehaviour
{
    [Header("Heart Arrays")]
    public Image[] playerHearts;
    public Image[] enemyHearts;

    [Header("Heart Colors")]
    public Color normalHeartColor = Color.red;
    public Color darkHeartColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    [Header("Debug")]
    public bool enableDebugLogs = true;
    public bool blockAllUpdates = false; // NEW: Block all UI updates

    private int playerLives;
    private int enemyLives;

    void Start()
    {
        // CHECK FLAG để tránh reset khi đang restore sau roulette
        bool shouldBypassReset = PlayerPrefs.HasKey("BypassLifeManagerReset");

        if (shouldBypassReset)
        {
            if (enableDebugLogs)
                Debug.Log("LifeManager: BYPASSING ResetHearts() due to restore flag");

            // CHỈ BLOCK khi restore, không block normal gameplay
            blockAllUpdates = true;

            // CHỈ validate, không reset
            ValidateSetup();

            // AUTO UNBLOCK sau 5 giây để tránh bị stuck
            StartCoroutine(AutoUnblockAfterDelay());
        }
        else
        {
            if (enableDebugLogs)
                Debug.Log("LifeManager: Normal start - calling ResetHearts()");

            blockAllUpdates = false;
            ResetHearts();
            ValidateSetup();
        }
    }

    private System.Collections.IEnumerator AutoUnblockAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (blockAllUpdates)
        {
            if (enableDebugLogs)
                Debug.Log("⏰ LifeManager: AUTO UNBLOCK after 5 seconds to prevent permanent block");

            blockAllUpdates = false;
        }
    }

    public void SetPlayerLives(int lives)
    {
        int clampedLives = Mathf.Clamp(lives, 0, playerHearts != null ? playerHearts.Length : 3);

        if (enableDebugLogs)
            Debug.Log($"LifeManager: SetPlayerLives called with {lives}, clamped to {clampedLives}, blockAllUpdates={blockAllUpdates}");

        // ALWAYS update internal value
        playerLives = clampedLives;

        // BUT only update UI if not blocked
        if (!blockAllUpdates)
        {
            StartCoroutine(ForceUpdatePlayerHearts());
        }
        else
        {
            if (enableDebugLogs)
                Debug.Log($"⚠️ LifeManager: BLOCKED UI update for SetPlayerLives({clampedLives}) - will update later");
        }
    }

    public void SetEnemyLives(int lives)
    {
        int clampedLives = Mathf.Clamp(lives, 0, enemyHearts != null ? enemyHearts.Length : 3);

        if (enableDebugLogs)
            Debug.Log($"LifeManager: SetEnemyLives called with {lives}, clamped to {clampedLives}, blockAllUpdates={blockAllUpdates}");

        // ALWAYS update internal value
        enemyLives = clampedLives;

        // BUT only update UI if not blocked
        if (!blockAllUpdates)
        {
            StartCoroutine(ForceUpdateEnemyHearts());
        }
        else
        {
            if (enableDebugLogs)
                Debug.Log($"⚠️ LifeManager: BLOCKED UI update for SetEnemyLives({clampedLives}) - will update later");
        }
    }

    // NEW: Method to unblock and force update all
    public void UnblockAndForceUpdateAll()
    {
        if (enableDebugLogs)
            Debug.Log($"🔓 LifeManager: UNBLOCKING and force updating all UI - Player:{playerLives}, Enemy:{enemyLives}");

        blockAllUpdates = false;

        // Force update both immediately
        StartCoroutine(ForceUpdatePlayerHearts());
        StartCoroutine(ForceUpdateEnemyHearts());
    }

    // NEW: Temporary block for critical updates
    public void TemporaryBlock(float duration = 1f)
    {
        if (enableDebugLogs)
            Debug.Log($"⏸️ LifeManager: TEMPORARY BLOCK for {duration} seconds");

        blockAllUpdates = true;
        StartCoroutine(UnblockAfterDuration(duration));
    }

    private System.Collections.IEnumerator UnblockAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (enableDebugLogs)
            Debug.Log($"🔓 LifeManager: AUTO UNBLOCK after {duration} seconds");

        blockAllUpdates = false;

        // Force update với current values
        StartCoroutine(ForceUpdatePlayerHearts());
        StartCoroutine(ForceUpdateEnemyHearts());
    }

    public void LosePlayerLife()
    {
        SetPlayerLives(playerLives - 1);
    }

    public void LoseEnemyLife()
    {
        SetEnemyLives(enemyLives - 1);
    }

    private IEnumerator ForceUpdatePlayerHearts()
    {
        yield return null;

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
                    Debug.Log($"Player Heart {i}: Lives={playerLives}, Color={targetColor}");
            }
            else
            {
                Debug.LogError($"LifeManager: playerHearts[{i}] is null!");
            }
        }

        if (enableDebugLogs)
            Debug.Log($"✅ COMPLETED: Force updated PLAYER hearts display to {playerLives} lives");
    }

    private IEnumerator ForceUpdateEnemyHearts()
    {
        yield return null;

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
                    Debug.Log($"Enemy Heart {i}: Lives={enemyLives}, Color={targetColor}");
            }
            else
            {
                Debug.LogError($"LifeManager: enemyHearts[{i}] is null!");
            }
        }

        if (enableDebugLogs)
            Debug.Log($"✅ COMPLETED: Force updated ENEMY hearts display to {enemyLives} lives");
    }

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

    public void ResetHearts()
    {
        playerLives = playerHearts != null ? playerHearts.Length : 3;
        enemyLives = enemyHearts != null ? enemyHearts.Length : 3;

        UpdatePlayerHeartsDisplay();
        UpdateEnemyHeartsDisplay();

        if (enableDebugLogs)
            Debug.Log($"LifeManager: Reset hearts - Player: {playerLives}, Enemy: {enemyLives}");
    }

    public void SetHeartColors(Color normal, Color dark)
    {
        normalHeartColor = normal;
        darkHeartColor = dark;

        UpdatePlayerHeartsDisplay();
        UpdateEnemyHeartsDisplay();

        if (enableDebugLogs)
            Debug.Log($"LifeManager: Updated heart colors - Normal: {normal}, Dark: {dark}");
    }

    public int GetPlayerLives() => playerLives;
    public int GetEnemyLives() => enemyLives;
    public int GetMaxPlayerLives() => playerHearts != null ? playerHearts.Length : 3;
    public int GetMaxEnemyLives() => enemyHearts != null ? enemyHearts.Length : 3;
    public bool IsPlayerAlive() => playerLives > 0;
    public bool IsEnemyAlive() => enemyLives > 0;

    public bool ValidateSetup()
    {
        bool isValid = true;

        if (playerHearts == null || playerHearts.Length == 0)
        {
            Debug.LogError("LifeManager: playerHearts array is null or empty!");
            isValid = false;
        }

        if (enemyHearts == null || enemyHearts.Length == 0)
        {
            Debug.LogError("LifeManager: enemyHearts array is null or empty!");
            isValid = false;
        }

        if (playerHearts != null)
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                if (playerHearts[i] == null)
                {
                    Debug.LogError($"LifeManager: playerHearts[{i}] is null!");
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
                    Debug.LogError($"LifeManager: enemyHearts[{i}] is null!");
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

    public void DebugHeartStatus()
    {
        Debug.Log("=================== LIFE MANAGER STATUS ===================");
        Debug.Log($"Player Lives: {playerLives}/{GetMaxPlayerLives()} (Alive: {IsPlayerAlive()})");
        Debug.Log($"Enemy Lives: {enemyLives}/{GetMaxEnemyLives()} (Alive: {IsEnemyAlive()})");
        Debug.Log($"Normal Heart Color: {normalHeartColor}");
        Debug.Log($"Dark Heart Color: {darkHeartColor}");
        Debug.Log($"Setup Valid: {ValidateSetup()}");
        Debug.Log($"Debug Logs Enabled: {enableDebugLogs}");

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

    [ContextMenu("Test Lose Player Life")]
    public void TestLosePlayerLife()
    {
        Debug.Log("🧪 TESTING: Lose Player Life");
        LosePlayerLife();
        DebugHeartStatus();
    }

    [ContextMenu("Test Lose Enemy Life")]
    public void TestLoseEnemyLife()
    {
        Debug.Log("🧪 TESTING: Lose Enemy Life");
        LoseEnemyLife();
        DebugHeartStatus();
    }

    [ContextMenu("Test Reset Hearts")]
    public void TestResetHearts()
    {
        Debug.Log("🧪 TESTING: Reset Hearts");
        ResetHearts();
        DebugHeartStatus();
    }

    [ContextMenu("Test Set Player Lives to 1")]
    public void TestSetPlayerLives()
    {
        Debug.Log("🧪 TESTING: Set Player Lives to 1");
        SetPlayerLives(1);
        DebugHeartStatus();
    }

    [ContextMenu("Test Set Enemy Lives to 2")]
    public void TestSetEnemyLives()
    {
        Debug.Log("🧪 TESTING: Set Enemy Lives to 2");
        SetEnemyLives(2);
        DebugHeartStatus();
    }

    [ContextMenu("Force Update All Hearts")]
    public void ForceUpdateAllHearts()
    {
        Debug.Log("🧪 TESTING: Force Update All Hearts");
        StartCoroutine(ForceUpdatePlayerHearts());
        StartCoroutine(ForceUpdateEnemyHearts());
    }

    void Awake()
    {
        if (enableDebugLogs)
            Debug.Log("LifeManager: Awake called");
    }

    void OnValidate()
    {
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