using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject panel;

    public Button option1;
    public Button option2;
    public Button option3;

    public PlayerStats playerStats;

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

        bool hasRope = playerStats.GetComponent<RopeAttack>() != null;
        bool hasKnife = playerStats.transform.Find("Knife") != null;

        SetButton(option1, "Daha Hýzlý Ateþ +25%", () =>
            playerStats.Upgrade_FireRate(1.25f)
        );

        if (!hasRope && !hasKnife)
        {
            SetButton(option2, "Yeni Silah: Halat", () =>
                playerStats.UnlockRope()
            );

            SetButton(option3, "Yeni Silah: Dönen Býçak", () =>
                playerStats.UnlockKnife()
            );
        }
        else if (hasRope && !hasKnife)
        {
            SetButton(option2, "Halat Hasarý +1", () =>
                playerStats.Upgrade_RopeDamage(1)
            );

            SetButton(option3, "Yeni Silah: Dönen Býçak", () =>
                playerStats.UnlockKnife()
            );
        }
        else if (!hasRope && hasKnife)
        {
            SetButton(option2, "Yeni Silah: Halat", () =>
                playerStats.UnlockRope()
            );

            SetButton(option3, "Býçak Hasarý +1", () =>
                playerStats.Upgrade_KnifeDamage(1)
            );
        }
        else
        {
            SetButton(option2, "Halat Hasarý +1", () =>
                playerStats.Upgrade_RopeDamage(1)
            );

            SetButton(option3, "Býçak Hasarý +1", () =>
                playerStats.Upgrade_KnifeDamage(1)
            );
        }
    }

    void SetButton(Button button, string text, System.Action action)
    {
        TMP_Text t = button.GetComponentInChildren<TMP_Text>();

        if (t != null)
            t.text = text;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            action.Invoke();
            CloseUpgrade();
        });
    }

    void CloseUpgrade()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}