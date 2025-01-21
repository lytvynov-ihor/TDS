using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [System.Serializable]
    public class Turret
    {
        public GameObject turretObject; // The turret GameObject
        public Transform barrels; // The child object responsible for vertical aiming
        public float fireRate = 1f; // How often the turret fires
        public int turretDamage = 10; // Damage dealt by the turret
        public Transform firePoint; // Where the turret fires from
        private float nextFireTime = 0f; // Time tracking for firing

        public void TryFire(Transform target)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Fire(target);
            }
        }

        private void Fire(Transform target)
        {
            if (target != null)
            {
                Debug.Log($"{turretObject.name} is firing at {target.name}!");
                Tower tower = target.GetComponent<Tower>();
                if (tower != null)
                {
                    tower.TakeDamage(turretDamage);
                }
            }
        }
    }

    public List<Turret> turrets = new List<Turret>(); // List of turrets
    public float attackRange = 15f; // Shared attack range for all turrets
    public float rotationSpeed = 2f; // Speed of turret rotation

    public float areaAttackRadius = 10f; // Radius for the area attack
    public int areaAttackDamage = 30; // Damage dealt by the area attack

    public float smokeShellRadius = 10f; // Radius for the smoke shell
    public float turretRangeReduction = 5f; // Amount to reduce tower attack range in Smoke Shell

    public Transform waypointsParent; // Parent object containing waypoints as children
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    public float moveSpeed = 5f;

    public AudioClip bossTheme; // Boss theme to play

    private List<Transform> targetsInRange = new List<Transform>(); // List of targets in range

    void Start()
    {
        if (bossTheme != null)
        {
            AudioManager.Instance.PlayClip(bossTheme);
        }

        InitializeWaypoints();
    }

    void Update()
    {
        MoveAlongWaypoints();
        FindTargetsInRange();
        RotateAndFireTurrets();
    }

    void InitializeWaypoints()
    {
        if (waypointsParent != null)
        {
            foreach (Transform waypoint in waypointsParent)
            {
                waypoints.Add(waypoint);
            }
        }
    }

    void MoveAlongWaypoints()
    {
        if (waypoints.Count == 0 || currentWaypointIndex >= waypoints.Count) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex++;
        }
    }

    void FindTargetsInRange()
    {
        targetsInRange.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            Tower tower = hitCollider.GetComponent<Tower>();
            if (tower != null)
            {
                targetsInRange.Add(tower.transform);
            }
        }
    }

    void RotateAndFireTurrets()
    {
        foreach (Turret turret in turrets)
        {
            Transform closestTarget = GetClosestTarget(turret.turretObject.transform);
            if (closestTarget != null)
            {
                RotateTurretTowardsTarget(turret, closestTarget);
                turret.TryFire(closestTarget);
            }
        }
    }

    Transform GetClosestTarget(Transform turretTransform)
    {
        Transform closestTarget = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform target in targetsInRange)
        {
            float distanceToTarget = Vector3.Distance(turretTransform.position, target.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    void RotateTurretTowardsTarget(Turret turret, Transform target)
    {
        if (turret.turretObject != null && target != null)
        {
            // Rotate the turret horizontally (Y-axis only)
            Vector3 direction = (target.position - turret.turretObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            turret.turretObject.transform.rotation = Quaternion.Slerp(
                turret.turretObject.transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );

            // Rotate the barrels vertically (X-axis)
            if (turret.barrels != null)
            {
                Vector3 barrelDirection = (target.position - turret.barrels.position).normalized;
                Quaternion barrelLookRotation = Quaternion.LookRotation(barrelDirection);
                turret.barrels.rotation = Quaternion.Slerp(
                    turret.barrels.rotation,
                    barrelLookRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    public void AreaAttack()
    {
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

    public void SmokeShell()
    {
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, areaAttackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, smokeShellRadius);
    }
}