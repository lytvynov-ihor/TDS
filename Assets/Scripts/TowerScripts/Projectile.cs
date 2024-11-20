using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 10; // Damage dealt by the projectile

    private Transform target; // Reference to the target enemy

    public void Seek(Transform _target)
    {
        target = _target; // Set the target enemy
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // If target is null, destroy the projectile
            return;
        }

        // Calculate direction to the target
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // If the projectile reaches the target, damage the enemy and destroy the projectile
        if (direction.magnitude <= distanceThisFrame)
        {
            DamageTarget();
            return;
        }

        // Move the projectile towards the target
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void DamageTarget()
    {
        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>(); // Get the EnemyHealth component of the target

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // Damage the enemy
        }

        Destroy(gameObject); // Destroy the projectile after hitting the enemy
    }
}
