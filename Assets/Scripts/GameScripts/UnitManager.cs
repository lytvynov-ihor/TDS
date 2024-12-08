using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] List<Unit> unitList = new List<Unit>();
    [SerializeField] GameObject spawnPosition;
    [SerializeField] int unitLimit = 5;

    private Vector3 basePosition;
    private Quaternion baseRotation;
    private Money money;
    private int unitsSpawned;
    
    /*
     
    A completely fucking useless script, MIGHT(perhaps, I hope, maybe) be used in the future 

     */

    // Start is called before the first frame update
    void Start()
    {
        unitsSpawned = 0;
        money = FindObjectOfType<Money>();
        basePosition = spawnPosition.transform.position + spawnPosition.transform.right * 1.5f; // Adjust as needed
        baseRotation = spawnPosition.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
