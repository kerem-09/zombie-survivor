using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopeAttack : MonoBehaviour
{
    public float interval = 2f;
    public float range = 3f;
    public float ropeWidth = 0.75f;
    public int damage = 2;

    public int ropeCount = 1; // 1 = tek halat, 2 = çift halat

    public float ropeVisibleTime = 0.1f;

    float timer;
    LineRenderer baseLine;

    void Awake()
    {
        baseLine = GetComponent<LineRenderer>();
        if (baseLine != null)
            baseLine.enabled = false;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;

        timer = interval;

        HashSet<EnemyHealth> damagedEnemies = new HashSet<EnemyHealth>();

        FireRope(new Vector2(-1f, 0.6f).normalized, damagedEnemies);

        if (ropeCount >= 2)
            FireRope(new Vector2(1f, 0.6f).normalized, damagedEnemies);
    }

    void FireRope(Vector2 dir, HashSet<EnemyHealth> damagedEnemies)
    {
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(dir * range);

        StartCoroutine(ShowRope(start, end));
        DamageAlongLine((Vector2)start, (Vector2)end, dir, damagedEnemies);
    }

    IEnumerator ShowRope(Vector3 start, Vector3 end)
    {
        GameObject ropeObj = new GameObject("RopeVisual");
        LineRenderer lr = ropeObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        if (baseLine != null)
        {
            lr.material = baseLine.material;
            lr.colorGradient = baseLine.colorGradient;
            lr.widthCurve = baseLine.widthCurve;
            lr.sortingLayerID = baseLine.sortingLayerID;
            lr.sortingOrder = baseLine.sortingOrder;
        }

        yield return new WaitForSeconds(ropeVisibleTime);

        Destroy(ropeObj);
    }

    void DamageAlongLine(Vector2 start, Vector2 end, Vector2 dir, HashSet<EnemyHealth> damagedEnemies)
    {
        Vector2 center = (start + end) * 0.5f;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector2 size = new Vector2(range, ropeWidth);

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            center,
            size,
            CapsuleDirection2D.Horizontal,
            angle
        );

        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;

            if (h.CompareTag("Enemy") || h.CompareTag("Runner") || h.CompareTag("Tank"))
            {
                EnemyHealth eh = h.GetComponent<EnemyHealth>();

                if (eh != null && !damagedEnemies.Contains(eh))
                {
                    eh.TakeDamage(damage);
                    damagedEnemies.Add(eh);
                }
            }
        }
    }
}