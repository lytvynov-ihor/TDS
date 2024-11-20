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

    public void SpawnUnit1()
    {
        if (unitList.Count > 0)
        {
            int unitPrice = unitList[0].unitPrice;
            if (money != null && money.currentCash >= unitPrice && unitsSpawned < unitLimit)
            {
                Instantiate(unitList[0], basePosition, baseRotation);
                money.DeductCash(unitPrice);
                unitsSpawned++;
            }
            else
            {
                Debug.LogWarning("Not enough cash or free space to spawn the unit.");
            }
        }
        else
        {
            Debug.LogWarning("No units available to spawn.");
        }
    }
}
