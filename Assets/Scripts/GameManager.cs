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

    // LEVEL
    public int level = 1;
    public int xp = 0;
    public int xpToNext = 10;
    public event Action OnLevelUp;

    // COIN MAGNET
    public float coinMagnetRange = 1.5f;

    // UI
    public GameObject gameOverPanel;
    public TMP_Text gameOverStatsText;

    // PLAYER
    public PlayerStats playerStats;

    // BEST SCORE
    int bestKill;
    float bestTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1f;

        // kayýtlarý yükle
        bestKill = PlayerPrefs.GetInt("BestKill", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            gameTime += Time.deltaTime;
        }
    }
    // OYUN BÝTTÝ MÝ?
    public bool isGameOver = false;
    // -------------------------
    // GAME OVER
    // -------------------------
    public void GameOver()
    {
        // OYUN BÝTTÝ
        isGameOver = true;
        // REKOR KONTROL
        if (killCount > bestKill)
        {
            bestKill = killCount;
            PlayerPrefs.SetInt("BestKill", bestKill);
        }

        if (gameTime > bestTime)
        {
            bestTime = gameTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        PlayerPrefs.Save();

        // PANEL AÇ
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // SÜRE HESAPLA
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        int bestMin = Mathf.FloorToInt(bestTime / 60f);
        int bestSec = Mathf.FloorToInt(bestTime % 60f);

        // YETENEKLER
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

        // UI TEXT
        if (gameOverStatsText != null)
        {
            gameOverStatsText.text =
                $"Süre: {minutes:00}:{seconds:00}\n" +
                $"Kill: {killCount}\n" +
                $"Coin: {coinCount}\n" +
                $"Level: {level}\n" +
                $"Yetenekler: {abilities}\n\n" +
                $"En Ýyi Süre: {bestMin:00}:{bestSec:00}\n" +
                $"En Ýyi Kill: {bestKill}";
        }

        Time.timeScale = 0f;
        
        
    }

    // -------------------------
    // GAME FLOW
    // -------------------------
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

    // -------------------------
    // GAMEPLAY
    // -------------------------
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

    // -------------------------
    // UPGRADES
    // -------------------------
    public void UpgradeCoinMagnet(float add)
    {
        coinMagnetRange += add;
    }
}