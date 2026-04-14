using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject player;

    bool ropeUnlocked = false;
    bool fireRateUpgraded = false;

    void Update()
    {
        if (GameManager.Instance == null || player == null) return;

        int c = GameManager.Instance.coinCount;

        if (!ropeUnlocked && GameManager.Instance.coinCount >= 20)
        {
            ropeUnlocked = true;

            if (player.GetComponent<RopeAttack>() == null)
                player.AddComponent<RopeAttack>();
        }


        // 50 coin -> AutoShooter bekleme azal?r (daha h?zl? atar)
        if (!fireRateUpgraded && c >= 50)
        {
            fireRateUpgraded = true;

            AutoShooter shooter = player.GetComponent<AutoShooter>();
            if (shooter != null)
            {
                shooter.fireRate *= 1.25f; // %25 daha h?zl? ate?

            }
        }
    }
}
