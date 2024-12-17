using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemies;
        public string advisorMessage; // New field for Advisor messages
        public float advisorDuration = 10f; // How long the advisor message stays
    }

    public Wave[] waves;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    public GameObject startWaveButton;
    public GameObject player;
    public AdvisorScript advisor; // Reference to the Advisor script

    private int currentWaveIndex = -1;
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

        // Display the Advisor message if one is provided for this wave
        if (!string.IsNullOrEmpty(waves[currentWaveIndex].advisorMessage))
        {
            advisor.ShowAdvisor(waves[currentWaveIndex].advisorMessage);
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
