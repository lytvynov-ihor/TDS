using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesBackgroundText : MonoBehaviour
{
    /*
     goal of the script is to make UpgradesDescriptionText show actual values tower has(damage, attack speed, etc,etc)     
     
     */

    [SerializeField] private List<Text> textFields = new List<Text>(); //no need to use List at all


    private TowerAttack towerAttack;
    private Tower tower;

    void Start()
    {
        towerAttack = GetComponent<TowerAttack>();
        tower = GetComponent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
