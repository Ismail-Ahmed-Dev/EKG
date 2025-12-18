using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("ÅÚÏÇÏÇÊ ÇáÚãáÉ")]
    public GameObject coinPrefab;
    public float minYPosition = -1f; 
    public float maxYPosition = 2f;  

    [Header("ÊæÞíÊ ÇáÅäÔÇÁ")]
    public float minSpawnTime = 1f; 
    public float maxSpawnTime = 3f;
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
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}