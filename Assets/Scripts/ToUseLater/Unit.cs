using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float movementSpeed = 5f;
    public int damageToEnemyBase = 10;
    public int damageToEnemyUnits = 5;
    public float attackRange = 3f;
    public int unitPrice = 50;

    private Transform targetBase; // Reference to the target base
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
            Debug.LogError("No object with the 'Base' tag found.");
        }
    }

    void Update()
    {
        if (targetBase != null)
        {
            // Move towards the target base
            transform.position = Vector3.MoveTowards(transform.position, targetBase.position, movementSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // Check if the enemy has collided with the base
        if (other.gameObject.tag == "Base")
        {
            // Deal damage to the base
            BaseHealth baseHealth = other.gameObject.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(damageToEnemyBase);
            }
            else
            {
                Debug.LogError("Base object does not have a BaseHealth component attached.");
            }
            Destroy(gameObject);
        }
    }
}
