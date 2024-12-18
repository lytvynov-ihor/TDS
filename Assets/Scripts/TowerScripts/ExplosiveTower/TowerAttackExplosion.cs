using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttackExplosion : MonoBehaviour
{
    public Transform firePoint;
    public float attackRange = 20f;
    public float rotationSpeed = 5f;
    public float fireCooldown = 1f;
    public int towerDamage = 50;
    public Transform rangeOfAttack;

    public Transform target;
    private Quaternion startRotation;
    private float fireTimer;
    public QuadraticCurve trajectory;
    public ProjectileShell shell;

    void Start()
    {
        startRotation = transform.rotation;//initial rotation of the tower
        fireTimer = fireCooldown;
        InvokeRepeating("UpdateTarget", 0f, 0.05f);//update the target every N seconds 
        rangeOfAttack.localScale = new Vector3(attackRange, attackRange, 3f);
    }

    void Update()
    {
        rangeOfAttack.localScale = new Vector3(attackRange, attackRange, 3f);
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
        float shortestDistance = Mathf.Infinity; //Initialize the shortest distance to infinity
        GameObject nearestEnemy = null; //Initialize the nearest enemy to null

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
            target = nearestEnemy.transform; //Set the target to the nearest enemy transform
        }
        else
        {
            target = null; //If no enemy found - set target to null
        }
    }

    void RotateTowardsEnemy()
    {
        Vector3 direction = (target.position - transform.position).normalized; //Calculate direction to the enemy
        Quaternion lookRotation = Quaternion.LookRotation(direction); //Calculate the rotation to look at the enemy

        //Smoothly rotate towards the enemy using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    
    void FireProjectile()
    {
        GameObject control = new GameObject("Control");
        GameObject enemyPos = new GameObject("EnemyPos");
        enemyPos.transform.position=target.transform.position;
        control.transform.position = firePoint.position + new Vector3(0f,40f,0f);
        trajectory.a = firePoint;
        trajectory.b = enemyPos.transform;
        trajectory.control = control.transform;
        QuadraticCurve newTrajectory = Instantiate(trajectory,firePoint.position,Quaternion.identity);
        shell.curve = newTrajectory;
        shell.attackDamage = towerDamage;
        Instantiate(shell,firePoint.position,Quaternion.identity);
        
    }
}