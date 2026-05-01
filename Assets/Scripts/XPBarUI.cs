using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarUI : MonoBehaviour
{
    public Slider xpSlider;
    public TMP_Text levelText;

    void Update()
    {
        if (GameManager.Instance == null) return;

        float fill = (float)GameManager.Instance.xp / GameManager.Instance.xpToNext;
        xpSlider.value = fill;

        levelText.text = "LV " + GameManager.Instance.level;
    }
}