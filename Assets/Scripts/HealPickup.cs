using UnityEngine;

public class HealPickup : MonoBehaviour
{
    public float healPercent = 0.5f; // %50

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.HealPercent(healPercent);
            }

            Destroy(gameObject);
        }
    }
}