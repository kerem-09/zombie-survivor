using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;
    public float hitCooldown = 1f;

    float timer;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (timer > 0f) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                timer = hitCooldown;
            }
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }
}