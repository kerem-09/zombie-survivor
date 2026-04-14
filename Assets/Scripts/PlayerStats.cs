using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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
        if (transform.Find("Knife") != null) return;

        GameObject knife = new GameObject("Knife");
        knife.transform.SetParent(transform);
        knife.transform.localPosition = Vector3.zero;
        knife.transform.localScale = new Vector3(0.4f, 0.4f, 1f);

        SpriteRenderer sr = knife.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");

        CircleCollider2D col = knife.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.5f;

        OrbitWeapon ow = knife.AddComponent<OrbitWeapon>();
        ow.radius = 2f;
        ow.speed = 200f;
        ow.damage = 1;
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