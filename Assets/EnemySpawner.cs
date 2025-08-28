using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 20f;
    public float minSpawnDistance = 5f; 

    [Header("Spawning Settings")]
    public float baseSpawnInterval = 5f;
    public int baseSpawnCount = 3;
    public int baseMaxEnemies = 10;

    [Header("Scaling Settings")]
    public float difficultyRampTime = 30f;
    public int spawnCountIncrease = 1;
    public int maxEnemiesIncrease = 2;
    public float spawnIntervalDecrease = 0.5f;

    public int maxSpawnCountLimit = 10;
    public int maxEnemiesLimit = 30;
    public float minSpawnInterval = 1f;

    private float timer;
    private float difficultyTimer;
    private float currentSpawnInterval;
    private int currentSpawnCount;
    private int currentMaxEnemies;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        currentSpawnInterval = baseSpawnInterval;
        currentSpawnCount = baseSpawnCount;
        currentMaxEnemies = baseMaxEnemies;
    }

    void Update()
    {
        timer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        activeEnemies.RemoveAll(enemy => enemy == null);

        if (timer >= currentSpawnInterval && activeEnemies.Count < currentMaxEnemies)
        {
            timer = 0f;
            int spawnable = Mathf.Min(currentSpawnCount, currentMaxEnemies - activeEnemies.Count);
            for (int i = 0; i < spawnable; i++)
            {
                GameObject enemy = SpawnEnemy();
                if (enemy != null)
                    activeEnemies.Add(enemy);
            }
        }

        if (difficultyTimer >= difficultyRampTime)
        {
            difficultyTimer = 0f;
            currentSpawnCount = Mathf.Min(currentSpawnCount + spawnCountIncrease, maxSpawnCountLimit);
            currentMaxEnemies = Mathf.Min(currentMaxEnemies + maxEnemiesIncrease, maxEnemiesLimit);
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecrease, minSpawnInterval);
        }
    }

    GameObject SpawnEnemy()
    {
        for (int attempt = 0; attempt < 10; attempt++) 
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = 0;
            Vector3 spawnPosition = player.position + randomDirection;

            if (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance)
                continue; 

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
            {
                return Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            }
        }

        return null; 
    }
}