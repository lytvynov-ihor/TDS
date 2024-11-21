using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 5f;
    public int damageToBase = 10;
    public int damageToUnits = 5;
    public float attackRange = 3f;

    public Transform path;
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    private Transform targetBase;

    void Start()
    {
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

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            targetBase = baseObject.transform;
        }
        else
        {
            Debug.LogError("No object with the 'Base' tag found.");
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
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else if (targetBase != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetBase.position, movementSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Base")
        {
            BaseHealth baseHealth = other.gameObject.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(damageToBase);
            }
            else
            {
                Debug.LogError("Base object does not have a BaseHealth component attached.");
            }
            Destroy(gameObject);
        }
    }
}