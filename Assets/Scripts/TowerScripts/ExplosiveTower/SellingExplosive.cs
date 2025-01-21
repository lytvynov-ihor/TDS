using System;
using TMPro;
using UnityEngine;

public class SellingExplosive : MonoBehaviour
{
    
    private GameObject gameManager;
    [SerializeField]public TowerExplosive tower;
    [SerializeField]public TextMeshProUGUI sellCostText;
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager");
        sellCostText.text = getSellCost(tower.towerCost).ToString()+"$$";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void sellTower()
    {
        gameManager.GetComponent<Money>().IncreaseCash(getSellCost(tower.towerCost)); 
        Destroy(this.transform.parent.gameObject);

    }

    public void updateCost()
    {
        sellCostText.text = tower.towerCost.ToString()+"$$";
    }

    private int getSellCost(int cost)
    {
        float sellCost = cost;
        return (int)(sellCost*0.85);
    }
}