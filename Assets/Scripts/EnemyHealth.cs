using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public GameObject coinPrefab;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Eğer zaten ölüyse hasar almasın (Birden fazla mermi çarparsa skor sapıtmasın)
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 1. Skoru Guncelle (Sadece burada cagirmak yeterli)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKill();
        }

        // 2. Coin (Altin/XP) Olustur
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        // 3. Olum Sesini Cal (Obje yok olsa da sesin calmasi icin ozel scriptini tetikle)
        ZombieAudio zombieAudio = GetComponent<ZombieAudio>();
        if (zombieAudio != null)
        {
            zombieAudio.PlayDeathSoundAndDestroy();
        }

        // 4. Objeyi Yok Et
        // Not: ZombieAudio icindeki fonksiyon objeyi zaten silebilir veya 
        // ses bitene kadar bekletebilir. Ama aninda yok etmek istiyorsan:
        Destroy(gameObject);
    }
}