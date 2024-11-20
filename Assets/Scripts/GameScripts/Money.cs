using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    [SerializeField] public int startingCash = 500;
    [SerializeField] Text cashText;
    public int currentCash;
    private Tower tower;
    private TowerManager towerManager;
    // Start is called before the first frame update
    void Start()
    {
        tower = FindObjectOfType<Tower>();
        towerManager = FindObjectOfType<TowerManager>();
        currentCash = startingCash;
        UpdateCashUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(tower.name.ToString());
    }

    public void UpdateCashUI()
    {
        cashText.text = "Cash: " + currentCash.ToString();
    }

    public void DeductCash(int amount)
    {
        currentCash -= amount;
        UpdateCashUI();
    }

    public void IncreaseCash(int amount)
    {
        currentCash += amount;
        UpdateCashUI();
    }

    public void SellTower()
    {
        //Destroy(tower);
        //IncreaseCash(tower.towerCost);
        //fuck this shit bruh, we will have to add this later
    }
}
