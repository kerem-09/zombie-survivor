using UnityEngine;
using System.Collections;

public class RopeAttack : MonoBehaviour
{
    public float interval = 2f;        // her 2 saniyede 1
    public float range = 3f;           // halat uzunlu?u
    public float ropeWidth = 0.75f;     // çizginin "hitbox" kal?nl???
    public int damage = 2;

    [Header("Visual")]
    public float ropeVisibleTime = 0.10f;

    float timer;
    LineRenderer lr;
    PlayerMovement pm;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.positionCount = 2;
            lr.enabled = false;
        }

        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = interval;

        Vector2 dir = new Vector2(-1f, 0.6f).normalized; // her zaman sol-üst


        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(dir * range);

        // Görsel
        if (lr != null)
        {
            StopAllCoroutines();
            StartCoroutine(ShowRope(start, end));
        }

        // Hasar: çizgi boyunca kapsül alan?nda kim varsa vur
        DamageAlongLine((Vector2)start, (Vector2)end, dir);
    }

    IEnumerator ShowRope(Vector3 start, Vector3 end)
    {
        lr.enabled = true;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        yield return new WaitForSeconds(ropeVisibleTime);

        lr.enabled = false;
    }

    void DamageAlongLine(Vector2 start, Vector2 end, Vector2 dir)
    {
        // Kapsülün merkezi ve aç?s?
        Vector2 center = (start + end) * 0.5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Kapsül boyutu: uzunluk = range, kal?nl?k = ropeWidth
        Vector2 size = new Vector2(range, ropeWidth);

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            center,
            size,
            CapsuleDirection2D.Horizontal,
            angle
        );

        foreach (var h in hits)
        {
            if (h == null) continue;

            // Player kendini vurmas?n
            if (h.gameObject == gameObject) continue;

            // Dü?man tag kontrolü
            if (h.CompareTag("Enemy") || h.CompareTag("Runner") || h.CompareTag("Tank"))
            {
                EnemyHealth eh = h.GetComponent<EnemyHealth>();
                if (eh != null)
                    eh.TakeDamage(damage);
            }
        }
    }
}
