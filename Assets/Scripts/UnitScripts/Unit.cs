using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float movementSpeed = 5f;
    public int damageToBase = 300;
    public int damageToUnits = 5;
    public float attackRange = 3f;
    public int unitHealth = 120;

    public Transform path;
    private int currentHealth;
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private Transform targetBase;
    private Quaternion initialRotation;

    void Start()
    {
        currentHealth = unitHealth;
        initialRotation = transform.rotation;

        if (path != null)
        {
            foreach (Transform child in path)
            {
                waypoints.Add(child);
            }
        }
        else
        {
            Debug.LogError("Path object not assigned to the enemy.");
            return;
        }
        if (waypoints.Count == 0)
        {
            Debug.LogError("The Path object has no child objects to use as waypoints.");
            return;
        }
    }

    void Update()
    {
        if (waypoints.Count > 0)
        {
            MoveAlongPath();
        }
    }

    void MoveAlongPath()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else if (targetBase != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBase.position, movementSpeed * Time.deltaTime);
            Vector3 direction = (targetBase.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(nameof(other));
        if (other.gameObject.tag == "Enemy")
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            Enemy enemyStuff = other.gameObject.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                TakeDamage(enemyStuff.damageToUnits);
                enemyHealth.TakeDamage(damageToBase);
            }
            else
            {
                Debug.LogError("Enemy object does not have EnemyHealth component attached.");
            }
        }
    }
}