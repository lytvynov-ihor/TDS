using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform turret; // Assign in inspector
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    public Transform firePoint; // Assign in inspector (point from where the beam will be fired)
    public int damage = 20;
    public float range = 100f;

    private float nextFireTime;

    void Update()
    {
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            AimTurret(enemy.transform);
            if (Time.time >= nextFireTime)
            {
                FireBeam();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            return;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void AimTurret(Transform target)
    {
        Vector3 direction = target.position - turret.position;
        direction.y = 0; // Ignore the vertical component to constrain rotation around the Y-axis
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Adjust the rotation to account for the base Y rotation of 90 degrees
        targetRotation *= Quaternion.Euler(0, 90, 0);

        turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Ensure the turret rotates only around the Y-axis
        turret.localEulerAngles = new Vector3(0, turret.localEulerAngles.y, 0);
    }

    void FireBeam()
    {
        // Perform the raycast
        RaycastHit hit;
        Vector3 forward = firePoint.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(firePoint.position, forward, Color.red, 5.0f);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
        else
        {
            return;
        }
    }
}