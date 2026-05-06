using UnityEngine;

public static class SaveManager
{
    // Rekor süreyi saniye cinsinden kaydeder
    public static void SaveBestTime(float timeInSeconds)
    {
        float currentBest = PlayerPrefs.GetFloat("BestTime", 0f);

        if (timeInSeconds > currentBest)
        {
            PlayerPrefs.SetFloat("BestTime", timeInSeconds);
            PlayerPrefs.Save();
            Debug.Log("Yeni Rekor Süre: " + FormatTime(timeInSeconds));
        }
    }

    // Rekor süreyi okumak için
    public static float GetBestTime()
    {
        return PlayerPrefs.GetFloat("BestTime", 0f);
    }

    // Saniyeyi 00:00 formatýna çeviren yardýmcý araç
    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}