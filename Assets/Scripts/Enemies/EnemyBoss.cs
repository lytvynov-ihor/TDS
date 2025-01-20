using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public Transform waypointsParent; // Parent GameObject with child waypoints
    public List<GameObject> turrets = new List<GameObject>(); // Turrets to rotate
    public float rotationSpeed = 2f; // Speed of turret rotation

    public float areaAttackRadius = 10f; // Radius for the area attack
    public int areaAttackDamage = 30; // Damage dealt by the area attack

    public float smokeShellRadius = 10f; // Radius for the smoke shell
    public float turretRangeReduction = 5f; // Amount to reduce tower attack range in Smoke Shell

    private List<Transform> waypoints = new List<Transform>(); // Waypoints for movement
    private int currentWaypointIndex = 0; // Index of the current waypoint

    public AudioClip bossTheme;

    void Start()
    {
        if (bossTheme != null)
        {
            AudioManager.Instance.PlayClip(bossTheme);
        }


        // Gather waypoints from the parent object
        if (waypointsParent != null)
        {
            foreach (Transform waypoint in waypointsParent)
            {
                waypoints.Add(waypoint);
            }
        }

        // Position the Boss at the first waypoint if available
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        MoveToNextWaypoint();
        RotateTurrets();
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, 5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }

    void RotateTurrets()
    {
        foreach (GameObject turret in turrets)
        {
            if (turret != null)
            {
                // Make turrets rotate towards the forward direction
                Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
                turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void initiateAreaAttack()
    {
        // Damage all towers within the area attack radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaAttackRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            Tower tower = hitCollider.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(areaAttackDamage);
                Debug.Log($"Tower hit by Area Attack! Remaining Health: {tower.healthPoints}");
            }
        }
    }

    public void deploySmokeShell()
    {
        // Reduce the range of all towers within the smoke shell radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, smokeShellRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            Tower tower = hitCollider.GetComponent<Tower>();
            if (tower != null && tower.GetComponent<TowerAttack>() != null)
            {
                TowerAttack towerAttack = tower.GetComponent<TowerAttack>();
                towerAttack.attackRange -= turretRangeReduction;
                Debug.Log($"Tower hit by Smoke Shell! New Attack Range: {towerAttack.attackRange}");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the area attack and smoke shell radii in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaAttackRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, smokeShellRadius);
    }

    void OnTriggerEnter(Collider other)
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

    private void OnDestroy()
    {
        Spawner waveManager = FindObjectOfType<Spawner>();
        if (waveManager != null)
        {
            waveManager.EnemyDestroyed();
        }
    }
}