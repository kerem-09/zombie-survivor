using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // SKOR
    public int killCount = 0;
    public int coinCount = 0;

    // OYUN SÜRESÝ
    public float gameTime = 0f;

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            gameTime += Time.deltaTime;
        }
    }

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
    // GAME OVER
    public TMP_Text gameOverStatsText;
    public PlayerStats playerStats;

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverStatsText != null)
        {
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);

            string abilities = "";

            if (playerStats != null)
            {
                if (playerStats.GetComponent<RopeAttack>() != null)
                    abilities += "Halat ";

                if (playerStats.knifeLevel > 0)
                    abilities += "Dönen Býçak ";
            }

            if (abilities == "")
                abilities = "Yok";

            gameOverStatsText.text =
                $"Süre: {minutes:00}:{seconds:00}\n" +
                $"Kill: {killCount}\n" +
                $"Coin: {coinCount}\n" +
                $"Level: {level}\n" +
                $"Yetenekler: {abilities}";
        }

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