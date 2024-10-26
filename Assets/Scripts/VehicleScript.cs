using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour
{
    private Transform targetBase;
    public float movementSpeed = 1f;
    public float turretRange;

    // Start is called before the first frame update
    void Start()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Enemy Base");
        if (baseObject != null)
        {
            targetBase = baseObject.transform; // Set the target base
        }
        else
        {
            Debug.LogError("No 'Enemy Base' tag found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(targetBase != null)
        {
            // Move towards the target base
            transform.position = Vector3.MoveTowards(transform.position, targetBase.position, movementSpeed * Time.deltaTime);
        }
    }
}
