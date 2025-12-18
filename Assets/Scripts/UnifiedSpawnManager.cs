using System.Collections;
using UnityEngine;

public class UnifiedSpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject obstaclePrefab;

    [Header("Coin Setting")]
    public float coinMinY = -1f;
    public float coinMaxY = 2f;

    [Header("Obstacle Setting")]
    public float obstacleMinY = -5f;
    public float obstacleMaxY = -3f;

    [Header("Spawn Time")]
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 3f;
    public float spawnXPosition = 20f;

    [Header("Coin Chance (0-100)")]
    [Range(0, 100)]
    public int coinChance = 60; 

    [Header("Min Distance Between Spawns")]
    public float minDistanceBetweenSpawns = 3f;

    private float lastSpawnX;
    private float lastSpawnTime;

    void Start()
    {
        lastSpawnX = spawnXPosition;
        lastSpawnTime = Time.time;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (Mathf.Abs(InfiniteMapManager.CurrentMoveVelocity) > 0.1f)
            {
                float timeSinceLastSpawn = Time.time - lastSpawnTime;
                float estimatedDistance = Mathf.Abs(InfiniteMapManager.CurrentMoveVelocity) * timeSinceLastSpawn;

                if (estimatedDistance >= minDistanceBetweenSpawns)
                {
                    SpawnRandomObject();
                    lastSpawnTime = Time.time;
                }
            }
        }
    }

    void SpawnRandomObject()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < coinChance)
        {
            float randomY = Random.Range(coinMinY, coinMaxY);
            Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            float randomY = Random.Range(obstacleMinY, obstacleMaxY);
            Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }

        lastSpawnX = spawnXPosition;
    }
}