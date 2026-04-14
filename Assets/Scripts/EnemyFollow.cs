using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 2.5f;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }
}
