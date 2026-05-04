using UnityEngine;

public class HealSpawner : MonoBehaviour
{
    public GameObject healPrefab;

    // HEAL SPAWN SÜRESÝ (saniye)
    public float spawnInterval = 60f;

    // MAP SINIRLARI
    public float minX = -20f;
    public float maxX = 20f;
    public float minY = -10f;
    public float maxY = 10f;

    float timer = 0f;

    void Update()
    {
        if (GameManager.Instance == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnHeal();
            timer = 0f;
        }
    }

    void SpawnHeal()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(x, y, 0);

        Instantiate(healPrefab, spawnPos, Quaternion.identity);
    }
}