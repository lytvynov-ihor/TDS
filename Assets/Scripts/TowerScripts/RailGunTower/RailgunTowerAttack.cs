using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunTowerAttack : MonoBehaviour
{
    public Transform firePoint; // Where the laser originates
    public float attackRange = 20f; // Range of the tower
    public float fireCooldown = 2f; // Time between attacks
    public int towerDamage = 50; // Damage dealt by the laser
    public LineRenderer laserBeam; // LineRenderer to visualize the laser

    private float fireTimer;

    void Start()
    {
        fireTimer = fireCooldown;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            TargetAndShoot();
            fireTimer = 0f;
        }
    }

    void TargetAndShoot()
    {
        // Find all objects within range
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, attackRange);

        // Filter for enemies by tag
        List<Transform> enemiesInRange = new List<Transform>();
        foreach (Collider obj in objectsInRange)
        {
            if (obj.CompareTag("Enemy"))
            {
                enemiesInRange.Add(obj.transform);
            }
        }

        // If no enemies are in range, return
        if (enemiesInRange.Count == 0)
        {
            return;
        }

        // Find the closest enemy
        Transform closestEnemy = FindClosestEnemy(enemiesInRange);

        // Fire the laser at the closest enemy
        if (closestEnemy != null)
        {
            FireLaser(closestEnemy);
        }
    }

    Transform FindClosestEnemy(List<Transform> enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
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
