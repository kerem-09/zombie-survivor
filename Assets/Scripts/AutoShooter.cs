using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f;
    public float range = 10f;

    private float fireTimer;

    void Update()
    {
        fireTimer -= Time.deltaTime;

        Transform target = FindClosestTarget();
        if (target == null) return;

        if (fireTimer <= 0f)
        {
            Shoot(target.position);
            fireTimer = 1f / fireRate;
        }
    }

    Transform FindClosestTarget()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        void Check(GameObject[] list)
        {
            foreach (GameObject e in list)
            {
                float d = Vector2.Distance(transform.position, e.transform.position);
                if (d <= range && d < minDist)
                {
                    minDist = d;
                    closest = e.transform;
                }
            }
        }

        Check(GameObject.FindGameObjectsWithTag("Enemy"));
        Check(GameObject.FindGameObjectsWithTag("Tank"));
        Check(GameObject.FindGameObjectsWithTag("Runner"));

        return closest;
    }



    void Shoot(Vector3 targetPos)
    {
        Vector2 dir = (targetPos - transform.position).normalized;

        GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        b.GetComponent<Bullet>().SetDirection(dir);
    }
}
