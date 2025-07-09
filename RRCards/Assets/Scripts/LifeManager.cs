using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public Image[] playerHearts; 
    public Image[] enemyHearts;  

    private int playerLives;
    private int enemyLives;

    void Start()
    {
        playerLives = playerHearts.Length;
        enemyLives = enemyHearts.Length;
        foreach (var img in playerHearts) img.gameObject.SetActive(true);
        foreach (var img in enemyHearts) img.gameObject.SetActive(true);
    }

    public void LosePlayerLife()
    {
        if (playerLives > 0)
        {
            playerLives--;
            playerHearts[playerLives].gameObject.SetActive(false);
        }
    }

    public void LoseEnemyLife()
    {
        if (enemyLives > 0)
        {
            enemyLives--;
            enemyHearts[enemyLives].gameObject.SetActive(false);
        }
    }

    public void ResetHearts()
    {
        playerLives = playerHearts.Length;
        enemyLives = enemyHearts.Length;
        foreach (var img in playerHearts) img.gameObject.SetActive(true);
        foreach (var img in enemyHearts) img.gameObject.SetActive(true);
    }
}
