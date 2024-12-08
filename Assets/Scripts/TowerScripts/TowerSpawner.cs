using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeSpawnStep
{
    public int healthIncrease;
    public float intervalDecrease;
    public List<GameObject> modelUpgrades;
}

[System.Serializable]
    public class UnitSpawnInfo
    {
        public GameObject unitPrefab;
        public float spawnInterval;
        [HideInInspector] public float nextSpawnTime;
    }

public class TowerSpawner : MonoBehaviour
{
    public List<UnitSpawnInfo> unitSpawnList = new List<UnitSpawnInfo>();
    [SerializeField] public int healthPoints = 450;
    public Transform spawnPoint;
    public Slider healthBar;
    //public float spawnInterval = 10f;
    public float maxHealth;
    public int towerCost;//need to make Money System actually work and not ju   st exist

    private int topPathUpgrades = 0;
    private int bottomPathUpgrades = 0;
    private const int maxUpgrades = 5;
    private const int blockThreshold = 3;

    [SerializeField] private List<UpgradeSpawnStep> topPathUpgradeSteps = new List<UpgradeSpawnStep>();
    [SerializeField] private List<UpgradeSpawnStep> bottomPathUpgradeSteps = new List<UpgradeSpawnStep>();

    void Start()
    {
        foreach (var unitInfo in unitSpawnList)
        {
            unitInfo.nextSpawnTime = Time.time + unitInfo.spawnInterval;
        }
        StartCoroutine(SpawnUnits());

        maxHealth = healthPoints;

        foreach (UpgradeSpawnStep step in topPathUpgradeSteps)
        {
            foreach (GameObject model in step.modelUpgrades)
            {
                if (model != null)
                {
                    model.SetActive(false);
                }
            }
        }

        foreach (UpgradeSpawnStep step in bottomPathUpgradeSteps)
        {
            foreach (GameObject model in step.modelUpgrades)
            {
                if (model != null)
                {
                    model.SetActive(false);
                }
            }
        }

        Debug.Log("Range Upgrades List Length: " + topPathUpgradeSteps.Count);
        Debug.Log("Speed Upgrades List Length: " + bottomPathUpgradeSteps.Count);
    }

    void Update()
    {
        float normalizedHealth = (float)healthPoints / maxHealth;
        healthBar.value = normalizedHealth;
    }

    IEnumerator SpawnUnits()
    {
        while (true)
        {
            float currentTime = Time.time;

            foreach (var unitInfo in unitSpawnList)
            {
                if (currentTime >= unitInfo.nextSpawnTime)
                {
                    SpawnUnit(unitInfo);
                    unitInfo.nextSpawnTime = currentTime + unitInfo.spawnInterval;
                }
            }

            yield return null;
        }
    }

    void SpawnUnit(UnitSpawnInfo unit)
    {
        if (unit.unitPrefab != null && spawnPoint != null)
        {
            Instantiate(unit.unitPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void UpgradeTopPath()
    {
        if (CanUpgradeTopPath())
        {
            if (topPathUpgrades < topPathUpgradeSteps.Count)
            {
                ApplyUpgrade(topPathUpgradeSteps[topPathUpgrades]);
                topPathUpgrades++;
                CheckBlockingCondition();
                Debug.Log("Top path upgraded to level " + topPathUpgrades);

            }
            else
            {
                Debug.Log("No more top path upgrades available.");
            }
        }
        else
        {
            Debug.Log("Cannot upgrade top path further.");
        }
    }

    public void UpgradeBottomPath()
    {
        if (CanUpgradeBottomPath())
        {
            if (bottomPathUpgrades < bottomPathUpgradeSteps.Count)
            {
                ApplyUpgrade(bottomPathUpgradeSteps[bottomPathUpgrades]);
                bottomPathUpgrades++;
                CheckBlockingCondition();
                Debug.Log("Bottom path upgraded to level " + bottomPathUpgrades);
            }
            else
            {
                Debug.Log("No more bottom path upgrades available.");
            }
        }
        else
        {
            Debug.Log("Cannot upgrade bottom path further.");
        }
    }

    private void ApplyUpgrade(UpgradeSpawnStep upgradeStep)
    {

        if (upgradeStep.intervalDecrease != null)
            foreach (var unit in unitSpawnList)
            {
                unit.spawnInterval -= upgradeStep.intervalDecrease;
                Debug.Log(unit.spawnInterval + " spawn interval; ");
            }

        if (upgradeStep.healthIncrease > 0)
        {
            healthPoints += upgradeStep.healthIncrease;
            maxHealth += upgradeStep.healthIncrease;
        }

        foreach (GameObject model in upgradeStep.modelUpgrades)
        {
            if (model != null)
            {
                model.SetActive(true);
                Debug.Log($"Activated model: {model.name}");
            }
            else
            {
                Debug.LogWarning("Model upgrade is null and cannot be activated.");
            }
        }
    }

    private bool CanUpgradeTopPath()
    {
        Debug.Log($"Checking CanUpgradeTopPath: topPathUpgrades = {topPathUpgrades}, bottomPathUpgrades = {bottomPathUpgrades}");
        if (topPathUpgrades >= maxUpgrades)
            return false;

        if (bottomPathUpgrades >= blockThreshold && topPathUpgrades < blockThreshold)
            return false;

        return true;
    }

    private bool CanUpgradeBottomPath()
    {
        Debug.Log($"Checking CanUpgradeSpeed: topPathUpgrades = {topPathUpgrades}, bottomPathUpgrades = {bottomPathUpgrades}");

        if (bottomPathUpgrades >= maxUpgrades)
            return false;

        if (topPathUpgrades >= blockThreshold && bottomPathUpgrades < blockThreshold)
            return false;

        return true;
    }

    private void CheckBlockingCondition()
    {
        if (topPathUpgrades == blockThreshold && bottomPathUpgrades < blockThreshold)
        {
            Debug.Log("Top Path upgrades are now blocked.");
        }

        if (bottomPathUpgrades == blockThreshold && topPathUpgrades < blockThreshold)
        {
            Debug.Log("Bottom Path upgrades are now blocked.");
        }
    }

    public void DisplayUpgradeStatus()
    {
        Debug.Log("Top Path upgrades: " + topPathUpgrades + "/" + maxUpgrades);
        Debug.Log("Bottom Path upgrades: " + bottomPathUpgrades + "/" + maxUpgrades);
    }
}