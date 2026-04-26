using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int knifeLevel = 0;
    public AutoShooter shooter;

    void Awake()
    {
        if (shooter == null)
            shooter = GetComponent<AutoShooter>();
    }

    public void Upgrade_FireRate(float mult)
    {
        if (shooter != null)
            shooter.fireRate *= mult;
    }

    public void Upgrade_Range(float add)
    {
        if (shooter != null)
            shooter.range += add;
    }

    public void UnlockRope()
    {
        if (GetComponent<RopeAttack>() != null) return;
        gameObject.AddComponent<RopeAttack>();
    }

    public void Upgrade_RopeDamage(int add)
    {
        RopeAttack rope = GetComponent<RopeAttack>();
        if (rope != null)
            rope.damage += add;
    }

    public void Upgrade_RopeInterval(float mult)
    {
        RopeAttack rope = GetComponent<RopeAttack>();
        if (rope != null)
            rope.interval *= mult;
    }

    public void Upgrade_RopeWidth(float add)
    {
        RopeAttack rope = GetComponent<RopeAttack>();
        if (rope != null)
            rope.ropeWidth += add;
    }

    public void UnlockKnife()
    {
        if (knifeLevel > 0) return;

        knifeLevel = 1;
        RebuildKnives();
    }
    public void Upgrade_KnifeCount()
    {
        if (knifeLevel == 0)
        {
            UnlockKnife();
            return;
        }

        if (knifeLevel == 1)
            knifeLevel = 2;
        else if (knifeLevel == 2)
            knifeLevel = 3;
        else if (knifeLevel == 3)
            knifeLevel = 5;

        RebuildKnives();
    }
    void RebuildKnives()
    {
        // Eski býçaklarý sil
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Knife"))
                Destroy(child.gameObject);
        }

        int count = knifeLevel;
        if (count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            GameObject knife = new GameObject("Knife_" + i);
            knife.transform.SetParent(transform);
            knife.transform.localPosition = Vector3.zero;
            knife.transform.localScale = new Vector3(0.35f, 0.35f, 1f);

            SpriteRenderer sr = knife.AddComponent<SpriteRenderer>();

            SpriteRenderer playerSR = GetComponent<SpriteRenderer>();
            if (playerSR != null)
                sr.sprite = playerSR.sprite;

            sr.color = Color.yellow;
            sr.sortingOrder = 10;

            CircleCollider2D col = knife.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.5f;

            OrbitWeapon ow = knife.AddComponent<OrbitWeapon>();
            ow.radius = 2f;
            ow.speed = 200f;
            ow.damage = 1;
            ow.angleOffset = (360f / count) * i;
        }
    }

    public void Upgrade_KnifeDamage(int add)
    {
        Transform knife = transform.Find("Knife");
        if (knife == null) return;

        OrbitWeapon ow = knife.GetComponent<OrbitWeapon>();
        if (ow != null)
            ow.damage += add;
    }

    public void Upgrade_KnifeSpeed(float mult)
    {
        Transform knife = transform.Find("Knife");
        if (knife == null) return;

        OrbitWeapon ow = knife.GetComponent<OrbitWeapon>();
        if (ow != null)
            ow.speed *= mult;
    }
}