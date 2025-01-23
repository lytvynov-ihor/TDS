using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeSpawnStep
{
    public int cost;
    public int healthIncrease;
    public float intervalDecrease;
    public List<GameObject> modelUpgrades;
    //public List<GameObject> additionalUnits;
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
    public float maxHealth;
    public int towerCost;
    
    private GameObject gameManager;
    private UIStatsUpdateFactory uiUpdate;

    private int topPathUpgrades = 0;
    private int bottomPathUpgrades = 0;
    private const int maxUpgrades = 5;
    private const int blockThreshold = 3;

    [SerializeField] private List<UpgradeSpawnStep> topPathUpgradeSteps = new List<UpgradeSpawnStep>();
    [SerializeField] private List<UpgradeSpawnStep> bottomPathUpgradeSteps = new List<UpgradeSpawnStep>();

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        
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
        
        uiUpdate = GetComponent<UIStatsUpdateFactory>();
        
        UpdateUIStats();
        uiUpdate.UpdateTopCost(topPathUpgradeSteps[0].cost);
        uiUpdate.UpdateBottomCost(bottomPathUpgradeSteps[0].cost);
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

    /* not working yet, WIP
    void AddNewUnit(GameObject unitPrefab, float spawnCooldown)
    {
        unitSpawnList.Add(new UnitSpawnInfo
        {
            unitPrefab = unitPrefab,
            spawnInterval = spawnCooldown,
            nextSpawnTime = Time.time + spawnCooldown // Ensures the initial cooldown is applied
        });
        Debug.Log($"Added unit: {unitPrefab.name} with a cooldown of {spawnCooldown} seconds.");
    }
    */

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
            if (topPathUpgrades < topPathUpgradeSteps.Count && topPathUpgradeSteps[topPathUpgrades].cost < gameManager.GetComponent<Money>().currentCash)
            {
                ApplyUpgrade(topPathUpgradeSteps[topPathUpgrades]);
                PayForUpgradesTop(topPathUpgradeSteps);
                topPathUpgrades++;
                CheckBlockingCondition();
                UpdateUIStats();
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
            if (bottomPathUpgrades < bottomPathUpgradeSteps.Count && bottomPathUpgradeSteps[bottomPathUpgrades].cost <= gameManager.GetComponent<Money>().currentCash)
            {
                ApplyUpgrade(bottomPathUpgradeSteps[bottomPathUpgrades]);
                PayForUpgradesBottom(bottomPathUpgradeSteps);
                bottomPathUpgrades++;
                CheckBlockingCondition();
                UpdateUIStats();
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
        if (upgradeStep.intervalDecrease != 0)
        {
            foreach (var unit in unitSpawnList)
            {
                unit.spawnInterval -= upgradeStep.intervalDecrease;
                Debug.Log(unit.spawnInterval + " spawn interval; ");
            }
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

        /* not working yet
        //add new units to the spawn list
        foreach (var unit in upgradeStep.additionalUnits)
        {
            if (unit != null)
            {
                unitSpawnList.Add(new UnitSpawnInfo
                {
                    unitPrefab = unit,
                    spawnInterval = 10f // Default interval; adjust as needed
                });
                Debug.Log($"Added unit: {unit.name} to spawn list.");
            }
        }
        */
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

    public void UpdateUIStats()
    {
        uiUpdate.UpdateSpawnTime(unitSpawnList[0].spawnInterval);
        uiUpdate.UpdateSpawnTime(unitSpawnList[0].unitPrefab.GetComponent<Unit>().unitHealth);
    }
    
    private void PayForUpgradesTop(List<UpgradeSpawnStep> upgradeStep)
    {
        int cost = upgradeStep[topPathUpgrades].cost;
        gameManager.GetComponent<Money>().DeductCash(cost);
        double addCost = cost * 0.5;
        towerCost += (int)addCost;
        if (uiUpdate != null)
        {
            if (topPathUpgrades + 1 < topPathUpgradeSteps.Count)
                uiUpdate.UpdateTopCost(upgradeStep[topPathUpgrades + 1].cost);
            else
            {
                uiUpdate.BottomCostMax();
                uiUpdate.TopCostMax();
            }

            GetComponent<SellingSpawner>().updateCost();
        }
    }
    
    private void PayForUpgradesBottom(List<UpgradeSpawnStep> upgradeStep)
    {
        int cost = upgradeStep[bottomPathUpgrades].cost;
        gameManager.GetComponent<Money>().DeductCash(cost);
        double addCost = cost * 0.5;
        towerCost += (int)addCost;
        if (uiUpdate != null)
        {
            if (bottomPathUpgrades + 1 < bottomPathUpgradeSteps.Count)
                uiUpdate.UpdateBottomCost(upgradeStep[bottomPathUpgrades + 1].cost);
            else
            {
                uiUpdate.BottomCostMax();
                uiUpdate.TopCostMax();
            }

            GetComponent<SellingSpawner>().updateCost();
        }
    }
}