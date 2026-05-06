using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    public GameObject panel;

    public Button option1;
    public Button option2;
    public Button option3;

    public PlayerStats playerStats;

    class UpgradeOption
    {
        public string title;
        public Action action;

        public UpgradeOption(string title, Action action)
        {
            this.title = title;
            this.action = action;
        }
    }

    void Start()
    {
        panel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelUp += ShowUpgrade;
    }

    void ShowUpgrade()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        List<UpgradeOption> upgrades = GetAvailableUpgrades();

        Shuffle(upgrades);

        // Butonlarý doldur (Seçenek azsa hata vermez)
        if (upgrades.Count >= 1) SetButton(option1, upgrades[0]);
        if (upgrades.Count >= 2) SetButton(option2, upgrades[1]);
        if (upgrades.Count >= 3) SetButton(option3, upgrades[2]);
    }

    List<UpgradeOption> GetAvailableUpgrades()
    {
        List<UpgradeOption> list = new List<UpgradeOption>();

        bool hasRope = playerStats.ropeLevel > 0;
        bool hasKnife = playerStats.knifeLevel > 0;

        // --- GENEL GELÝÞTÝRMELER ---
        list.Add(new UpgradeOption("Ateþ Hýzý +25%", () => playerStats.Upgrade_FireRate(1.25f)));
        list.Add(new UpgradeOption("Menzil +1", () => playerStats.Upgrade_Range(1f)));
        list.Add(new UpgradeOption("Hareket Hýzý +10%", () => playerStats.Upgrade_MoveSpeed(1.10f)));
        list.Add(new UpgradeOption("Mýknatýs Alaný +", () => playerStats.Upgrade_CoinMagnet(1f)));

        // --- HALAT SÝSTEMÝ (ÇAPRAZ VÝZYON) ---
        if (!hasRope)
        {
            list.Add(new UpgradeOption("Yeni Silah: Çapraz Halat", () => playerStats.UnlockRope()));
        }
        else
        {
            if (playerStats.ropeLevel < 4) // Maksimum 4 köþe (Çizimindeki gibi)
            {
                list.Add(new UpgradeOption("Ekstra Halat Köþesi", () => playerStats.Upgrade_RopeLevel()));
            }
            list.Add(new UpgradeOption("Halat Hasarý +1", () => playerStats.Upgrade_RopeDamage(1)));
        }

        // --- BIÇAK SÝSTEMÝ ---
        if (!hasKnife)
        {
            list.Add(new UpgradeOption("Yeni Silah: Dönen Býçak", () => playerStats.UnlockKnife()));
        }
        else
        {
            if (playerStats.knifeLevel < 5)
            {
                list.Add(new UpgradeOption("Býçak Sayýsý +", () => playerStats.Upgrade_KnifeCount()));
            }
            list.Add(new UpgradeOption("Býçak Hasarý +1", () => playerStats.Upgrade_KnifeDamage(1)));
            list.Add(new UpgradeOption("Býçak Hýzý +10%", () => playerStats.Upgrade_KnifeSpeed(1.1f)));
        }

        return list;
    }

    void SetButton(Button button, UpgradeOption upgrade)
    {
        if (button == null) return;

        TMP_Text text = button.GetComponentInChildren<TMP_Text>();

        if (text != null)
            text.text = upgrade.title;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            upgrade.action.Invoke();
            CloseUpgrade();
        });
    }

    void Shuffle(List<UpgradeOption> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            UpgradeOption temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void CloseUpgrade()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}