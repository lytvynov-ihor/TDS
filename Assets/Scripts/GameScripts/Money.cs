using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    [SerializeField] public int startingCash = 500;
    [SerializeField] Text cashText;
    public int currentCash;
   // public GameObject towerObj;
    private TowerManager towerManager;
    // Start is called before the first frame update
    void Start()
    {
        towerManager = FindObjectOfType<TowerManager>();
        currentCash = startingCash;
        UpdateCashUI();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Tower set: " + towerObj.name);
    }

    public void UpdateCashUI()
    {
        cashText.text = "Cash: " + currentCash.ToString() + "$$";
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

    public void SellTower(GameObject t)
    {
        // Debug.Log("sellTower: " + t.name);
        // int sellcost = t.GetComponent<Tower>().towerCost;
        // IncreaseCash(sellcost);
        // Destroy(t);
        //fuck this shit bruh, we will have to add this later
    }
    
}
