using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeStep
{
    public float rangeIncrease;
    public float speedDecrease;
    public int healthIncrease;
    public int damageIncrease;
    public List<GameObject> modelUpgrades;
    public List<GameObject> modelToHide;
}

public class Tower : MonoBehaviour
{
    [SerializeField] public int healthPoints = 100;
    public Slider healthBar;
    public int maxHealth;
    public string towerName;
    public int towerCost;

    private TowerAttack towerAttack;
    private GameObject gameManager;

    private int topPathUpgrades = 0;
    private int bottomPathUpgrades = 0;
    private const int maxUpgrades = 5;
    private const int blockThreshold = 3;

    [SerializeField] private List<UpgradeStep> topPathUpgradeSteps = new List<UpgradeStep>();
    [SerializeField] private List<UpgradeStep> bottomPathUpgradeSteps = new List<UpgradeStep>();

    void Start()
    {
        towerAttack = GetComponent<TowerAttack>();
        maxHealth = healthPoints;
        
        gameManager = GameObject.FindWithTag("GameManager");

        foreach (UpgradeStep step in topPathUpgradeSteps)
        {
            foreach (GameObject model in step.modelUpgrades)
            {
                if (model != null)
                {
                    model.SetActive(false);
                }
            }
            foreach (GameObject model in step.modelToHide)
            {
                if (model != null)
                {
                    model.SetActive(false);
                }
            }
        }

        foreach (UpgradeStep step in bottomPathUpgradeSteps)
        {
            foreach (GameObject model in step.modelUpgrades)
            {
                if (model != null)
                {
                    model.SetActive(false);
                }
            }
            foreach (GameObject model in step.modelToHide)
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

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
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
                Debug.Log("Range upgraded to level " + topPathUpgrades);

            }
            else
            {
                Debug.Log("No more range upgrades available.");
            }
        }
        else
        {
            Debug.Log("Cannot upgrade range further.");
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
                Debug.Log("Speed upgraded to level " + bottomPathUpgrades);
            }
            else
            {
                Debug.Log("No more speed upgrades available.");
            }
        }
        else
        {
            Debug.Log("Cannot upgrade speed further.");
        }
    }

    private void ApplyUpgrade(UpgradeStep upgradeStep)
    {
        if (upgradeStep.rangeIncrease != null)
            towerAttack.attackRange += upgradeStep.rangeIncrease;
        if (upgradeStep.speedDecrease != null)
            towerAttack.fireCooldown -= upgradeStep.speedDecrease;
        if (upgradeStep.damageIncrease != null)
            towerAttack.towerDamage += upgradeStep.damageIncrease;
        if (upgradeStep.healthIncrease > 0)
        {
            healthPoints += upgradeStep.healthIncrease;
            maxHealth += upgradeStep.healthIncrease;
        }

        foreach (GameObject model in upgradeStep.modelUpgrades)
        {
            if (model != null)  // Ensure the model is assigned
            {
                model.SetActive(true);
                Debug.Log($"Activated model: {model.name}");
            }
            else
            {
                Debug.LogWarning("Model upgrade is null and cannot be activated.");
            }
        }

        foreach (GameObject model in upgradeStep.modelToHide)
        {
            if (model != null)  // Ensure the model is assigned
            {
                model.SetActive(false);
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

    public void sellTower()
    {
        gameManager.GetComponent<Money>().IncreaseCash(towerCost); 
        Destroy(this.transform.parent.gameObject);

    }
    
}