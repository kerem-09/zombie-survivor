using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    public float radius = 2f;
    public float speed = 200f;
    public int damage = 1;

    float angle;

    void Update()
    {
        angle += speed * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(rad) * radius;
        float y = Mathf.Sin(rad) * radius;

        transform.localPosition = new Vector3(x, y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Runner") || other.CompareTag("Tank"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
                eh.TakeDamage(damage);
        }
    }
}