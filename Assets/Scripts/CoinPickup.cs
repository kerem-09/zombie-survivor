using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;
    public float magnetSpeed = 6f;
    public AudioClip collectSound; // XP/Coin toplama sesini buraya sürükle

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // SES ÇALMA MANTIÐI
            // Obje yok olmadan önce Player üzerindeki AudioSource'u kullanýyoruz
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (playerAudio != null && collectSound != null)
            {
                playerAudio.PlayOneShot(collectSound);
            }

            if (GameManager.Instance != null)
                GameManager.Instance.AddCoin(value);

            // Veritabaný için toplam toplanan coin sayýsýný artýrabilirsin
            // SaveManager.AddTotalCoins(value); 

            Destroy(gameObject);
        }
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || GameManager.Instance == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // GameManager üzerindeki magnetRange deðerini kullanýyor
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