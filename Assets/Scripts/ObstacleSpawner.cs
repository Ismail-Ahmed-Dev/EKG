using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Column Setting")]
    public GameObject columnPrefab;
    public float minYPosition = -5f; 
    public float maxYPosition = -3f; 

    [Header("Spawn Time")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;
    public float spawnXPosition = 20f;

    void Start()
    {
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
                float randomY = Random.Range(minYPosition, maxYPosition);
                Vector3 spawnPosition = new Vector3(spawnXPosition, randomY, 0);
                Instantiate(columnPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}