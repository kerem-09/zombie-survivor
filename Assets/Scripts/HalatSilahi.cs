using UnityEngine;
using System.Collections;

public class HalatSilahi : MonoBehaviour
{
    public int damage = 1;
    public float activeDuration = 0.5f; // Halatýn ekranda kalma süresi
    public float waitDuration = 3f;     // Vuruþlar arasý bekleme süresi (Ýstediðin 3 saniye)

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        // Oyuna kapalý baþla ve döngüyü baþlat
        StartCoroutine(RopeAttackRoutine());
    }

    IEnumerator RopeAttackRoutine()
    {
        while (true)
        {
            // 1. BEKLE (3 Saniye)
            sr.enabled = false;
            col.enabled = false;
            yield return new WaitForSeconds(waitDuration);

            // 2. VUR (Spawn ol)
            sr.enabled = true;
            col.enabled = true;

            // Buraya istersen küçük bir ses efekti kodu da ekleyebilirsin
            Debug.Log("Halat vurdu!");

            // 3. EKRANDA KAL
            yield return new WaitForSeconds(activeDuration);

            // Döngü baþa döner ve halat kapanýr
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}