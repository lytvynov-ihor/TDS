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
    private GameObject[] cashFarms;
    public AdvisorScript advisor; // Reference to the Advisor script

    private int currentWaveIndex = -1;
    private int enemiesRemaining = 0;
    private bool isWaveActive = false;

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

        isWaveActive = true;
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
            isWaveActive = false;
            ShowStartWaveButtonIfPlayerAlive();
        }
    }

    void ShowStartWaveButtonIfPlayerAlive()
    {
        if (player != null)
        {
            BaseHealth health = player.GetComponent<BaseHealth>();
            cashFarms = GameObject.FindGameObjectsWithTag("CashFarm");

            if (health != null && health.healthPositive())
            {
                if (startWaveButton != null)
                    startWaveButton.SetActive(true);
                if (cashFarms.Length != 0)
                {
                    for (int i = 0; i < cashFarms.Length; i++)
                    {
                        CoinFarmScript t = cashFarms[i].GetComponent<CoinFarmScript>();
                        t.wasCashCollected = false;
                    }
                }
            }
        }
    }

    public void OnStartNextWaveButtonPressed()
    {
        if (player != null)
        {
            BaseHealth health = player.GetComponent<BaseHealth>();
            if (health != null && health.healthPositive())
            {
                startWaveButton.SetActive(false);
                currentWaveIndex++;
                StartWave();
            }
        }
    }

    public bool IsWaveActive()
    {
        return isWaveActive; // Getter for isWaveActive
    }
}
