using UnityEngine;
using System.Collections;

public class C_HalatSistemi : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject ropePrefab;
    public AudioClip slaksSesi;
    public int ropeDamage = 1;
    public float attackInterval = 3f;

    // --- BU KISIM KISALTILDI ---
    [Tooltip("Halatýn fýrlama mesafesi (Eski: 4f, Yeni: 2.2f)")]
    public float strikeDistance = 2.2f;
    [Tooltip("Halatýn fýrlama hýzý (Eski: 10f, Yeni: 7f)")]
    public float strikeSpeed = 7f;
    // ---------------------------

    private int currentLevel = 0;
    private AudioSource myAudioSource;

    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        if (myAudioSource == null) myAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        StopAllCoroutines();
        if (currentLevel > 0) StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            // 0: Sað Üst (-45), 1: Sað Alt (-135), 2: Sol Alt (135), 3: Sol Üst (45)
            float[] angles = { -45f, -135f, 135f, 45f };

            for (int i = 0; i < currentLevel; i++)
            {
                if (i >= 4) break;
                SpawnRope(angles[i]);
            }
        }
    }

    void SpawnRope(float angle)
    {
        if (ropePrefab == null) return;

        // Halatý tam merkezde oluþtur, yönünü ayarla
        GameObject rope = Instantiate(ropePrefab, transform.position, Quaternion.Euler(0, 0, angle));
        rope.transform.SetParent(this.transform);
        // Lokal pozisyonu sýfýrla (Kritik: Karakterin tam içine oturtur)
        rope.transform.localPosition = Vector3.zero;

        // Ses çal
        if (myAudioSource != null && slaksSesi != null) myAudioSource.PlayOneShot(slaksSesi);

        // Hareket ve hasar mantýðýný yükle
        RopeBehavior rb = rope.AddComponent<RopeBehavior>();
        // setup fonksiyonuna güncel (kýsa) deðerleri gönderiyoruz
        rb.Setup(ropeDamage, strikeDistance, strikeSpeed);
    }
}

// --- YARDIMCI HAREKET VE HASAR SINIFI ---
public class RopeBehavior : MonoBehaviour
{
    private int damage;
    private float speed;
    private float distance;

    public void Setup(int dmg, float dist, float spd)
    {
        damage = dmg;
        distance = dist;
        speed = spd;
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        Vector3 startPos = transform.localPosition;
        // transform.up yönüne (çapraz) doðru fýrlat
        Vector3 endPos = startPos + (transform.up * distance);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Fýrlama bitince çok kýsa süre sonra objeyi sil
        Destroy(gameObject, 0.05f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Boylu boyunca temas ettiði her düþmana hasar verir
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}