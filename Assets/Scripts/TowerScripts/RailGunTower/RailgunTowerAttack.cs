using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTowerAttack : MonoBehaviour
{
    public Transform firePoint; // Where the laser originates
    public float attackRange = 20f; // Range of the tower
    public float rotationSpeed = 5f; // Speed of rotation toward the target
    public float fireCooldown = 2f; // Time between attacks
    public int towerDamage = 50; // Damage dealt by the laser
    public LineRenderer laserBeam; // LineRenderer to visualize the laser
    public GameObject towerModel; // The visual model of the tower

    private Transform target; // Current target
    private Quaternion startRotation; // Initial rotation of the tower
    private float fireTimer;

    void Start()
    {
        fireTimer = fireCooldown;
        startRotation = transform.rotation; // Save the initial rotation
        InvokeRepeating("UpdateTarget", 0f, 0.1f); // Update target every 0.1 seconds
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (target != null)
        {
            RotateTowardsEnemy(); // Rotate toward the target

            // Fire laser if cooldown is complete
            if (fireTimer >= fireCooldown)
            {
                FireLaser(target);
                fireTimer = 0f;
            }
        }
        else
        {
            // Reset rotation to its initial state if no target
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateTarget()
    {
        // Find all objects within range
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, attackRange);

        // Filter for enemies by tag
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider obj in objectsInRange)
        {
            if (obj.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, obj.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = obj.transform;
                }
            }
        }

        // Set the closest enemy as the target
        target = nearestEnemy;
    }

    void RotateTowardsEnemy()
    {
        if (target == null) return;

        // Calculate direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Apply rotation offset if needed (example: adjust 90 degrees if the model is misaligned)
        Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 90f, 0f);

        // Smoothly rotate the tower's base toward the target
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // If the tower model rotates independently (e.g., a turret), adjust this too
        if (towerModel != null)
        {
            towerModel.transform.rotation = Quaternion.Slerp(towerModel.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }



    void FireLaser(Transform target)
    {
        Vector3 laserDirection = (target.position - firePoint.position).normalized;

        // Use a Raycast to find all enemies hit by the laser
        RaycastHit[] hits = Physics.RaycastAll(firePoint.position, laserDirection, attackRange);

        // Damage all enemies hit by the laser
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy")) // Check if the hit object has the "Enemy" tag
            {
                EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(towerDamage);
                    Debug.Log($"Damaged {hit.collider.name} for {towerDamage} damage.");
                }
            }
        }

        // Visualize the laser
        StartCoroutine(LaserVisual(target.position));
    }

    private IEnumerator LaserVisual(Vector3 targetPosition)
    {
        // Enable the laser
        laserBeam.enabled = true;

        // Set the start and end points of the laser
        laserBeam.SetPosition(0, firePoint.position);
        laserBeam.SetPosition(1, targetPosition);

        // Keep the laser visible for a short time
        yield return new WaitForSeconds(0.1f);

        // Disable the laser
        laserBeam.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw the attack range in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
