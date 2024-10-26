using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [SerializeField] List<SpawnButton> btnList = new List<SpawnButton>();
    [SerializeField] public int TowerLimit = 10;
    public Text text;
    int totalTowers;

    void Start()
    {
        totalTowers = 0;
        btnList.AddRange(FindObjectsOfType<SpawnButton>());
        foreach (var btn in btnList)
        {
            btn.SetTowerManager(this);
        }
    }

    void Update()
    {
        text.text = "Tower Limit: " + totalTowers.ToString() + "/" + TowerLimit.ToString();
        if (totalTowers < TowerLimit)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                btnList[0].CanSpawn();
                btnList[1].CantSpawn();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                btnList[0].CantSpawn();
                btnList[1].CanSpawn();
            }
        }
    }

    public void IncreasePlacedTowersAmount(int amount)
    {
        totalTowers += amount; 
    }

    public void DecreasePlacedTowersAmount(int amount)
    {
        totalTowers -= amount;
    }
}
