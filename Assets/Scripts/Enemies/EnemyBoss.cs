using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    [System.Serializable]
    public class Turret
    {
        public GameObject turretObject;
        public Transform barrels;
        public float fireRate = 1f;
        public int turretDamage = 10;
        public Transform firePoint;
        private float nextFireTime = 0f;

        public void TryFire(Transform target)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Fire(target);
            }
        }


        //
        //
        //TO DO: actually make Boss use Special Attacks and not what it is using right now
        //
        //


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

    public List<Turret> turrets = new List<Turret>();
    public float attackRange = 15f;
    public float rotationSpeed = 2f;

    public float areaAttackRadius = 10f;
    public int areaAttackDamage = 30;

    public float smokeShellRadius = 10f;
    public float turretRangeReduction = 5f;

    public Transform waypointsParent;
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;
    public float moveSpeed = 5f;

    public AudioClip bossTheme;

    private List<Transform> targetsInRange = new List<Transform>();

    void Start()
    {
        if (bossTheme != null)
        {
            AudioManager.Instance.PlayClip(bossTheme);
        }

        initializeWaypoints();
    }

    void Update()
    {
        moveWaypoints();
        findTargetsInRange();
        rotateAndFire();
    }

    void initializeWaypoints()
    {
        if (waypointsParent != null)
        {
            foreach (Transform waypoint in waypointsParent)
            {
                waypoints.Add(waypoint);
            }
        }
    }

    void moveWaypoints()
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

    void findTargetsInRange()
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

    void rotateAndFire()
    {
        foreach (Turret turret in turrets)
        {
            Transform closestTarget = getClosestTarget(turret.turretObject.transform);
            if (closestTarget != null)
            {
                rotateTurrets(turret, closestTarget);
                turret.TryFire(closestTarget);
            }
        }
    }

    Transform getClosestTarget(Transform turretTransform)
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

    void rotateTurrets(Turret turret, Transform target)
    {
        if (turret.turretObject != null && target != null)
        {
            Vector3 direction = (target.position - turret.turretObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            turret.turretObject.transform.rotation = Quaternion.Slerp(
                turret.turretObject.transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );

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

    public void initiateAreaAttack()
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

    public void deploySmokeShell()
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