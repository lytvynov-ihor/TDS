using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
    }

    public Wave[] waves;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    private int currentWaveIndex = 0;
    private int enemiesToSpawn = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("all waves completed");
            return;
        }

        Debug.Log($"Starting Wave {currentWaveIndex + 1}");
        enemiesToSpawn = waves[currentWaveIndex].enemies.Length;
        enemiesRemaining = enemiesToSpawn;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(waves[currentWaveIndex].enemies[i]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            currentWaveIndex++;
            StartWave();
        }
    }
}
