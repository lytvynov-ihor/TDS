using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private AudioClip shootingAudio;
    [SerializeField] private ParticleSystem muzzleFlash;
    
    public Transform firePoint;
    public float attackRange = 20f;
    public float rotationSpeed = 5f;
    public float fireCooldown = 1f;
    public int towerDamage = 10;
    public Transform rangeOfAttack;

    private Transform target;
    private Quaternion startRotation;
    private float fireTimer;

    void Start()
    {
        startRotation = transform.rotation;//initial rotation of the tower
        fireTimer = fireCooldown;
        InvokeRepeating("UpdateTarget", 0f, 0.001f);//update the target every N seconds 
        rangeOfAttack.localScale = new Vector3(attackRange * 10, attackRange * 10, 3f);

        muzzleFlash.GetComponent<MuzzleTRX>().muzzle = firePoint;
    }

    void Update()
    {
        rangeOfAttack.localScale = new Vector3(attackRange * 10, attackRange * 10, 3f);
        if (target != null)
        {
            RotateTowardsEnemy();

            if (fireTimer >= fireCooldown)
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
        if(Camera.main !=null)
            AudioSource.PlayClipAtPoint(shootingAudio,Camera.main.transform.position, 0.10f);
        
        Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
        
        RaycastHit hit;
        Vector3 forward = firePoint.TransformDirection(Vector3.forward) * attackRange;
        Debug.DrawRay(firePoint.position, forward, Color.red, 5.0f);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.GetComponent<Immunities>().bulletProof)
                    return;
                
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