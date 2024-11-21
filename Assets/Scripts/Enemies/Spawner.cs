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

    public GameObject startWaveButton;
    public GameObject player;

    private int currentWaveIndex = -1;
    private int enemiesToSpawn = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        if (startWaveButton != null)
            startWaveButton.SetActive(true);
    }

    void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }

        enemiesRemaining = waves[currentWaveIndex].enemies.Length;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
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
            ShowStartWaveButtonIfPlayerAlive();
        }
    }

    void ShowStartWaveButtonIfPlayerAlive()
    {
        if (player != null)
        {
            BaseHealth health = player.GetComponent<BaseHealth>();
            if (health != null && health.IsAlive())
            {
                if (startWaveButton != null)
                    startWaveButton.SetActive(true);
            }
        }
    }

    public void OnStartNextWaveButtonPressed()
    {
        if (player != null)
        {
            BaseHealth health = player.GetComponent<BaseHealth>();
            if (health != null && health.IsAlive())
            {
                startWaveButton.SetActive(false);
                currentWaveIndex++;
                StartWave();
            }
        }
    }
}
