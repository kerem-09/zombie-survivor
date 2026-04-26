using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int killCount = 0;

    public int coinCount = 0;

    // LEVEL
    public int level = 1;
    public int xp = 0;                 // coin gibi dü?ün
    public int xpToNext = 10;          // ilk level için gereken coin
    public event Action OnLevelUp;     // UI bunu dinleyecek

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKill() => killCount++;

    public void AddCoin(int amount)
    {
        coinCount += amount;

        // coin ayn? zamanda XP
        xp += amount;
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        while (xp >= xpToNext)
        {
            xp -= xpToNext;
            level++;

            // sonraki level daha zor
            xpToNext = Mathf.RoundToInt(10 + (level - 1) * 6);

            OnLevelUp?.Invoke();
        }
    }
    // Coin magnet özellikleri
    public float coinMagnetRange = 1.5f;

    public void UpgradeCoinMagnet(float add)
    {
        coinMagnetRange += add;
    }

}
