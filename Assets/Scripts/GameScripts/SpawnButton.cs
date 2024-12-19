using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] public bool canSpawn;
    [SerializeField] GameObject objectToSpawn;
    public int towersPlaced;
    private TowerManager towerManager;
    private Tower tower;
    private Money money;

    public void SetTowerManager(TowerManager manager)
    {
        towerManager = manager;
    }

    void Start()
    {
        money = FindObjectOfType<Money>();
        tower = objectToSpawn.GetComponent<Tower>(); // Get Tower component from objectToSpawn
        towersPlaced = 0;
        canSpawn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            canSpawn = false;
        }

        // Spawn tower and update towers/cash amount 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 intersectionPoint = hit.point;

                if (canSpawn == true)
                {
                    if(money.currentCash >= tower.towerCost)
                    {
                        Instantiate(objectToSpawn, intersectionPoint, Quaternion.identity);
                        canSpawn = false;
                        towersPlaced++;
                        money.DeductCash(tower.towerCost);                       
                        towerManager.IncreasePlacedTowersAmount(1);
                        Debug.Log(money.currentCash);
                    }
                }
            }
        }
    }

    public void CanSpawn()
    {
        canSpawn = true;
    }

    public void CantSpawn()
    {
        canSpawn = false;
    }
}