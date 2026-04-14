using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddCoin(value);

            Destroy(gameObject);
        }
    }
}
