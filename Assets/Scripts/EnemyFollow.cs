using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    private Transform player;
    private SpriteRenderer spriteRenderer; // Yönü çevirmek için bu lazým

    void Start()
    {
        // Oyuncuyu bul
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // SpriteRenderer bileþenini al
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // 1. HAREKET MANTIÐI
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // 2. YÖNÜ ÇEVÝRME (FLIP) MANTIÐI
        // Eðer oyuncu zombinin saðýndaysa (x pozisyonu daha büyükse)
        if (player.position.x > transform.position.x)
        {
            // SpriteRenderer'ýn flipX özelliðini kapat (Saða bak)
            // Not: Sprite'ýn orijinal hali saða bakýyorsa böyle, sola bakýyorsa tam tersi yapýlýr.
            spriteRenderer.flipX = false;
        }
        // Eðer oyuncu zombinin solundaysa
        else
        {
            // SpriteRenderer'ýn flipX özelliðini aç (Sola bak)
            spriteRenderer.flipX = true;
        }
    }
}