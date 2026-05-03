using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // SKOR
    public int killCount = 0;
    public int coinCount = 0;

    // LEVEL
    public int level = 1;
    public int xp = 0;
    public int xpToNext = 10;
    public event Action OnLevelUp;

    // COIN MAGNET
    public float coinMagnetRange = 1.5f;

    // UI
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1f; // GameScene açýlýnca oyun direkt baþlasýn
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void AddKill()
    {
        killCount++;
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        xp += amount;
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        while (xp >= xpToNext)
        {
            xp -= xpToNext;
            level++;

            xpToNext = Mathf.RoundToInt(10 + (level - 1) * 6);

            OnLevelUp?.Invoke();
        }
    }

    public void UpgradeCoinMagnet(float add)
    {
        coinMagnetRange += add;
    }
}