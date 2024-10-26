using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttack : MonoBehaviour
{
    public Transform firePoint; // Point where the projectile is fired from
    public float attackRange = 10f; // Attack range of the tower
    public float rotationSpeed = 5f; // Rotation speed of the tower
    public float fireCooldown = 1f; // Cooldown period between shots
    public int towerDamage = 10;
    public Transform rangeOfAttack;

    private Transform target; // Reference to the target enemy
    private Quaternion startRotation; // Initial rotation of the tower
    private float fireTimer; // Timer to track the time since the last shot

    void Start()
    {
        startRotation = transform.rotation;//initial rotation of the tower
        fireTimer = fireCooldown;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);//update the target every 0.5 seconds 
        rangeOfAttack.localScale = new Vector3(attackRange*5, attackRange*5, 1f);
    }

    void Update()
    {
        rangeOfAttack.localScale = new Vector3(attackRange * 5, attackRange * 5, 1f);
        if (target != null)
        {
            RotateTowardsEnemy(); // Rotate the tower towards the enemy

            // Check if the tower can fire
            if (fireTimer >= fireCooldown)
            {
                FireProjectile(); // Fire a projectile at the enemy
                fireTimer = 0f; // Reset the fire timer
            }
            else
            {
                fireTimer += Time.deltaTime; // Increment the fire timer
            }
        }
        else
        {
            // If there is no target, reset the rotation to its initial rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * rotationSpeed);
        }
    }


    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemies with the tag "Enemy"
        float shortestDistance = Mathf.Infinity; // Initialize the shortest distance to infinity
        GameObject nearestEnemy = null; // Initialize the nearest enemy to null

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform; // Set the target to the nearest enemy transform
        }
        else
        {
            target = null; // If no enemy found, set target to null
        }
    }


    void RotateTowardsEnemy()
    {
        Vector3 direction = (target.position - transform.position).normalized; // Calculate direction to the enemy
        Quaternion lookRotation = Quaternion.LookRotation(direction); // Calculate the rotation to look at the enemy

        // Smoothly rotate towards the enemy using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void FireProjectile()
    {
        RaycastHit hit;
        Vector3 forward = firePoint.TransformDirection(Vector3.forward) * attackRange;
        Debug.DrawRay(firePoint.position, forward, Color.red, 5.0f);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(towerDamage);
                }
            }
        }
        else
        {
            return;
        }
    }
}