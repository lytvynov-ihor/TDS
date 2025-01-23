using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttackExplosion : MonoBehaviour
{
    [SerializeField] AudioClip fireSound;
    private new GameObject camera;
    
    public Transform firePoint;
    public float attackRange = 20f;
    public float rotationSpeed = 5f;
    public float fireCooldown = 1f;
    public int towerDamage = 50;
    public GameObject towerModel;
    public Transform rangeOfAttack;

    public Transform target;
    private Quaternion startRotation;
    private float fireTimer;
    public QuadraticCurve trajectory;
    public ProjectileShell shell;

    void Start()
    {
        camera = GameObject.Find("Main Camera");
        startRotation = transform.rotation; // Initial rotation of the tower
        fireTimer = fireCooldown;
        InvokeRepeating("UpdateTarget", 0f, 0.05f); // Update the target every N seconds
        UpdateRangeOfAttackScale();
    }

    void Update()
    {
        UpdateRangeOfAttackScale();
        if (target != null)
        {
            RotateTowardsEnemy();

            if (fireTimer >= fireCooldown && target != null)
            {
                FireProjectile();
                fireTimer = 0f;
            }
            else
            {
                fireTimer += Time.deltaTime;
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        towerModel.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        RotateFirePoint(direction, 3f);
    }

    void FireProjectile()
    {
        if(Camera.main!=null)
            AudioSource.PlayClipAtPoint(fireSound, Camera.main.transform.position, 0.4f);
        
        GameObject control = new GameObject("Control");
        GameObject enemyPos = new GameObject("EnemyPos");
        enemyPos.transform.position = target.transform.position;
        control.transform.position = firePoint.position + new Vector3(0f, 15f, 0f);
        trajectory.a = firePoint;
        trajectory.b = enemyPos.transform;
        trajectory.control = control.transform;
        QuadraticCurve newTrajectory = Instantiate(trajectory, firePoint.position, Quaternion.identity);
        shell.curve = newTrajectory;
        shell.attackDamage = towerDamage;
        Instantiate(shell, firePoint.position, Quaternion.identity);
    }

    void RotateFirePoint(Vector3 enemyPos, float radius)
    {
        float targetAngleInRadians = Mathf.Atan2(enemyPos.z, enemyPos.x);

        float targetAngle = 0f;
        targetAngle = Mathf.LerpAngle(targetAngle, targetAngleInRadians, Time.deltaTime * rotationSpeed);

        float x = towerModel.transform.position.x + Mathf.Cos(targetAngleInRadians) * 3f;
        float z = towerModel.transform.position.z + Mathf.Sin(targetAngleInRadians) * 3f;

        firePoint.transform.position = new Vector3(x, firePoint.transform.position.y, z);
    }

    void UpdateRangeOfAttackScale()
    {
        if (rangeOfAttack != null)
        {
            rangeOfAttack.localScale = new Vector3(attackRange * 10, attackRange * 10, attackRange * 10);
        }
    }

    // Add Gizmos to visualize attack range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color for the Gizmos
        Gizmos.DrawWireSphere(transform.position, attackRange); // Draw the attack range as a wireframe sphere
    }
}