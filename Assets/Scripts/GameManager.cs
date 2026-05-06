using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton Yapýsý: Diðer kodlardan GameManager.Instance ile eriþmeni saðlar.
    public static GameManager Instance;

    [Header("--- ISTATISTIKLER ---")]
    public int killCount = 0;
    public int coinCount = 0;
    public float gameTime = 0f;

    [Header("--- SEVIYE SISTEMI ---")]
    public int level = 1;
    public int xp = 0;
    public int xpToNext = 10;
    public event Action OnLevelUp; // Level atlandýðýnda seçim ekranýný tetikler.

    [Header("--- OYUNCU VE YETENEKLER ---")]
    public float coinMagnetRange = 1.5f;
    public PlayerStats playerStats; // Sahnedeki Player objesini buraya sürükle.

    [Header("--- ARAYÜZ (UI) ---")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverStatsText;

    [Header("--- SES AYARLARI ---")]
    public AudioClip gameOverMusic; // Oyun bitti müziðini Inspector'dan sürükle.
    private AudioSource audioSource;

    [Header("--- VERITABANI (REKORLAR) ---")]
    private int bestKill;
    private float bestTime;

    [Header("--- DURUM ---")]
    public bool isGameOver = false;

    void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1f;

        // Ses motorunu hazýrla
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Zaman durduðunda müziðin kesilmemesi için:
        audioSource.ignoreListenerPause = true;

        // PlayerPrefs üzerinden kayýtlý rekorlarý çek
        bestKill = PlayerPrefs.GetInt("BestKill", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
    }

    void Update()
    {
        // Oyun ölmediysen ve zaman akýyorsa süreyi tut
        if (!isGameOver && Time.timeScale > 0f)
        {
            gameTime += Time.deltaTime;
        }
    }

    // ---------------------------------------------------------
    // OYUNU BÝTÝRME (GAME OVER)
    // ---------------------------------------------------------
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // 1. REKOR KONTROLÜ VE KAYIT
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
        PlayerPrefs.Save(); // Veriyi diske iþle

        // 2. MÜZÝÐÝ BAÞLAT
        if (gameOverMusic != null)
        {
            audioSource.clip = gameOverMusic;
            audioSource.loop = false;
            audioSource.Play();
        }

        // 3. PANELÝ AKTÝF ET
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 4. KULLANILAN YETENEKLERÝ ANALÝZ ET
        string abilitiesUsed = "";
        if (playerStats != null)
        {
            if (playerStats.ropeLevel > 0) abilitiesUsed += $"Halat(Lv{playerStats.ropeLevel}) ";
            if (playerStats.knifeLevel > 0) abilitiesUsed += $"Býçak(Lv{playerStats.knifeLevel}) ";
        }
        if (string.IsNullOrEmpty(abilitiesUsed)) abilitiesUsed = "Yok";

        // 5. ÝSTATÝSTÝKLERÝ UI'YA YAZDIR
        if (gameOverStatsText != null)
        {
            string currentT = FormatTime(gameTime);
            string bestT = FormatTime(bestTime);

            gameOverStatsText.text =
                $"<color=#FF4444><size=140%>OYUN BITTI</size></color>\n\n" +
                $"<align=left>Süre: <color=#FFFFFF>{currentT}</color>\n" +
                $"Öldürme: <color=#FFFFFF>{killCount}</color>\n" +
                $"Altýn: <color=#FFFFFF>{coinCount}</color>\n" +
                $"Seviye: <color=#FFFFFF>{level}</color>\n" +
                $"Yetenekler: <color=#AAAAAA>{abilitiesUsed}</color>\n\n" +
                $"<color=#FFD700>REKOR SÜRE: {bestT}\n" +
                $"REKOR KILL: {bestKill}</color></align>";
        }

        // 6. OYUNU DURDUR
        Time.timeScale = 0f;
    }

    // Saniyeyi 00:00 formatýna sokar
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // ---------------------------------------------------------
    // BUTON AKSÝYONLARI
    // ---------------------------------------------------------
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

    // ---------------------------------------------------------
    // OYNANIÞ TETÝKLEYÝCÝLERÝ
    // ---------------------------------------------------------
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

    private void CheckLevelUp()
    {
        while (xp >= xpToNext)
        {
            xp -= xpToNext;
            level++;
            // Yeni seviye için gereken XP formülü
            xpToNext = Mathf.RoundToInt(10 + (level - 1) * 6);

            // Level seçim panelini açmak için event'i fýrlat
            OnLevelUp?.Invoke();
        }
    }

    public void UpgradeCoinMagnet(float add)
    {
        coinMagnetRange += add;
    }
}