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

        SetButton(option1, upgrades[0]);
        SetButton(option2, upgrades[1]);
        SetButton(option3, upgrades[2]);
    }

    List<UpgradeOption> GetAvailableUpgrades()
    {
        List<UpgradeOption> list = new List<UpgradeOption>();

        bool hasRope = playerStats.GetComponent<RopeAttack>() != null;
        bool hasKnife = playerStats.knifeLevel > 0;

        list.Add(new UpgradeOption("Daha Hýzlý Ateþ +25%", () =>
            playerStats.Upgrade_FireRate(1.25f)
        ));

        list.Add(new UpgradeOption("Menzil Artýr +1", () =>
            playerStats.Upgrade_Range(1f)
        ));

        // Hareket hýzý yükseltmesi sadece 1.15x olabilir
        list.Add(new UpgradeOption("Hareket Hýzý +10%", () =>
            playerStats.Upgrade_MoveSpeed(1.10f)
        ));

        // Coin magnet yükseltmesi olabilir
        list.Add(new UpgradeOption("Mýknatýs Alaný +", () =>
            playerStats.Upgrade_CoinMagnet(1f)
        ));

        if (!hasRope)
        {
            list.Add(new UpgradeOption("Yeni Silah: Halat", () =>
                playerStats.UnlockRope()
            ));
        }
        else
        {
            list.Add(new UpgradeOption("Halat Hasarý +1", () =>
                playerStats.Upgrade_RopeDamage(1)
            ));

            list.Add(new UpgradeOption("Halat Hýzý +10%", () =>
                playerStats.Upgrade_RopeInterval(0.9f)
            ));

            list.Add(new UpgradeOption("Halat Alaný +", () =>
                playerStats.Upgrade_RopeWidth(0.15f)
            ));
            // Halat sayýsý yükseltmesi sadece 2'ye kadar olabilir
            RopeAttack rope = playerStats.GetComponent<RopeAttack>();

            if (rope != null && rope.ropeCount < 2)
            {
                list.Add(new UpgradeOption("Çift Halat", () =>
                    playerStats.Upgrade_RopeCount()
                ));
            }
        }

        if (!hasKnife)
        {
            list.Add(new UpgradeOption("Yeni Silah: Dönen Býçak", () =>
                playerStats.UnlockKnife()
            ));
        }
        else
        {
            if (playerStats.knifeLevel < 5)
            {
                list.Add(new UpgradeOption("Býçak Sayýsý Artýr", () =>
                    playerStats.Upgrade_KnifeCount()
                ));
            }

            list.Add(new UpgradeOption("Býçak Hasarý +1", () =>
                playerStats.Upgrade_KnifeDamage(1)
            ));

            list.Add(new UpgradeOption("Býçak Hýzý +10%", () =>
                playerStats.Upgrade_KnifeSpeed(1.1f)
            ));
        }

        return list;
    }

    void SetButton(Button button, UpgradeOption upgrade)
    {
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