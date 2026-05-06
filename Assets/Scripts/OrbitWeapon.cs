using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    public float radius = 2f; // Býçaðýn karakterden uzaklýðý (0 YAPMA)
    public float speed = 200f; // Dönüþ hýzý
    public int damage = 1;
    public float angleOffset = 0f;

    private float angle;

    void Update()
    {
        // Yörüngede dönme matematiði
        angle += speed * Time.deltaTime;
        float rad = (angle + angleOffset) * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * radius;
        float y = Mathf.Sin(rad) * radius;

        // Býçaðýn pozisyonunu karaktere göre ayarla
        transform.localPosition = new Vector3(x, y, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
        }
    }
}