using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyNormal;
    public GameObject enemyRunner;
    public GameObject enemyTank;
    public GameObject enemyZigzag;

    public Transform player;

    public float spawnRadius = 10f;
    public float baseInterval = 1.5f;
    public float minX = -14f;
    public float maxX = 14f;
    public float minY = -8f;
    public float maxY = 8f;


    public int maxEnemiesOnScreen = 35;
    public int maxTanksOnScreen = 4;
    public int maxRunnersOnScreen = 10;

    float timer;
    float elapsed;

    void Update()
    {
        if (player == null) return;

        elapsed += Time.deltaTime;
        timer -= Time.deltaTime;

        float currentInterval = Mathf.Max(0.35f, baseInterval - elapsed * 0.02f);

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = currentInterval;
        }
    }

    void SpawnEnemy()
    {
        int total = GameObject.FindGameObjectsWithTag("Enemy").Length
                  + GameObject.FindGameObjectsWithTag("Tank").Length
                  + GameObject.FindGameObjectsWithTag("Runner").Length;

        if (total >= maxEnemiesOnScreen)
            return;

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = player.position + (Vector3)(dir * spawnRadius);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);


        GameObject chosen = ChooseByTime();

        if (chosen == enemyTank)
        {
            int tankCount = GameObject.FindGameObjectsWithTag("Tank").Length;
            if (tankCount >= maxTanksOnScreen)
                chosen = enemyNormal;
        }

        if (chosen == enemyRunner)
        {
            int runnerCount = GameObject.FindGameObjectsWithTag("Runner").Length;
            if (runnerCount >= maxRunnersOnScreen)
                chosen = enemyNormal;
        }

        Instantiate(chosen, pos, Quaternion.identity);
    }

    GameObject ChooseByTime()
    {
        int kills = (GameManager.Instance != null) ? GameManager.Instance.killCount : 0;

        // 0-10 sn: sadece Normal
        if (elapsed < 10f)
            return enemyNormal;

        // 10-30 sn: az Runner
        if (elapsed < 30f)
            return Random.value < 0.80f ? enemyNormal : enemyRunner; // %20 runner

        // 30-55 sn: Runner biraz arts?n
        if (elapsed < 55f)
            return Random.value < 0.65f ? enemyNormal : enemyRunner; // %35 runner

        // 55-80 sn: Zigzag devreye girsin (tank hâlâ yok)
        if (elapsed < 80f)
        {
            float r = Random.value;
            if (r < 0.55f) return enemyNormal;   // %55
            if (r < 0.85f) return enemyRunner;   // %30
            return enemyZigzag;                  // %15
        }

        // 80+ sn: Tank en son, az
        {
            float r = Random.value;
            if (r < 0.45f) return enemyNormal;   // %45
            if (r < 0.70f) return enemyRunner;   // %25
            if (r < 0.88f) return enemyZigzag;   // %18
            return enemyTank;                    // %12
        }
    }

}
