using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Bicak Ayarlari")]
    public GameObject knifePrefab;
    public int knifeLevel = 0;

    [Header("Halat Ayarlari")]
    public GameObject ropePrefab;
    public int ropeLevel = 0;
    private C_HalatSistemi halatSistemi; // Yeni çapraz sistemin beyni

    [Header("Bilesenler")]
    public AutoShooter shooter;

    void Awake()
    {
        if (shooter == null)
            shooter = GetComponent<AutoShooter>();

        // Yeni halat sistemini kontrol et, yoksa ekle
        halatSistemi = GetComponent<C_HalatSistemi>();
        if (halatSistemi == null)
            halatSistemi = gameObject.AddComponent<C_HalatSistemi>();
    }

    // --- BIÇAK SÝSTEMÝ (DOKUNULMADI) ---
    public void UnlockKnife()
    {
        if (knifeLevel > 0) return;
        knifeLevel = 1;
        RebuildKnives();
    }

    public void Upgrade_KnifeCount()
    {
        if (knifeLevel == 0) { UnlockKnife(); return; }
        if (knifeLevel == 1) knifeLevel = 2;
        else if (knifeLevel == 2) knifeLevel = 3;
        else if (knifeLevel == 3) knifeLevel = 5;
        RebuildKnives();
    }

    void RebuildKnives()
    {
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Knife"))
                Destroy(child.gameObject);
        }

        if (knifeLevel <= 0 || knifePrefab == null) return;

        for (int i = 0; i < knifeLevel; i++)
        {
            GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);
            knife.name = "Knife_" + i;
            knife.transform.SetParent(transform);
            knife.transform.localPosition = Vector3.zero;
            knife.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

            OrbitWeapon ow = knife.GetComponent<OrbitWeapon>();
            if (ow != null) ow.angleOffset = (360f / knifeLevel) * i;
        }
    }

    public void Upgrade_KnifeDamage(int add)
    {
        foreach (Transform t in transform)
        {
            if (t.name.StartsWith("Knife"))
            {
                OrbitWeapon ow = t.GetComponent<OrbitWeapon>();
                if (ow != null) ow.damage += add;
            }
        }
    }

    public void Upgrade_KnifeSpeed(float mult)
    {
        foreach (Transform t in transform)
        {
            if (t.name.StartsWith("Knife"))
            {
                OrbitWeapon ow = t.GetComponent<OrbitWeapon>();
                if (ow != null) ow.speed *= mult;
            }
        }
    }

    // --- HALAT SÝSTEMÝ (ÇAPRAZ VÝZYONA GÖRE DÜZENLENDÝ) ---
    public void UnlockRope()
    {
        if (ropeLevel > 0) return;
        ropeLevel = 1;
        SyncRopeSystem();
    }

    public void Upgrade_RopeLevel() // UpgradeUI'dan bu çaðrýlacak
    {
        if (ropeLevel == 0) { UnlockRope(); return; }
        ropeLevel++;
        if (ropeLevel > 4) ropeLevel = 4; // Çizimindeki 4 köþe max
        SyncRopeSystem();
    }

    public void Upgrade_RopeDamage(int add)
    {
        if (halatSistemi != null) halatSistemi.ropeDamage += add;
    }

    void SyncRopeSystem()
    {
        if (halatSistemi != null)
        {
            halatSistemi.ropePrefab = ropePrefab;
            halatSistemi.SetLevel(ropeLevel); // C_HalatSistemi'ndeki SetLevel'ý tetikler
        }
    }

    // --- GENEL ÝSTATÝSTÝKLER (DOKUNULMADI) ---
    public void Upgrade_FireRate(float mult) { if (shooter != null) shooter.fireRate *= mult; }
    public void Upgrade_Range(float add) { if (shooter != null) shooter.range += add; }
    public void Upgrade_MoveSpeed(float mult)
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.moveSpeed *= mult;
    }
    public void Upgrade_CoinMagnet(float add)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UpgradeCoinMagnet(add);
    }
}