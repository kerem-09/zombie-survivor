using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;
    // Magnet özellikleri
    public float magnetRange = 1.5f;
    public float magnetSpeed = 6f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddCoin(value);

            Destroy(gameObject);
        }
    }
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= GameManager.Instance.coinMagnetRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                magnetSpeed * Time.deltaTime
            );
        }
    }
}
