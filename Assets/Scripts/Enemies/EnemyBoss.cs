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
    public AudioClip victoryTheme;

    private List<Transform> targetsInRange = new List<Transform>();
    private EnemyHealth enemyHealth;
    private Transform targetBase;

    void Start()
    {
        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            targetBase = baseObject.transform;
        }
        else
        {
            Debug.LogError("No object with the 'Base' tag found.");
        }

        enemyHealth = GetComponent<EnemyHealth>();
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
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else if (targetBase != null)
        {
            Vector3 direction = (targetBase.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, targetBase.position, moveSpeed * Time.deltaTime);
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
                Vector3 targetPosition = target.position;
                targetPosition.y = turret.barrels.position.y; // Keep the target's y-position relative to the barrels
                Vector3 barrelDirection = targetPosition - turret.barrels.position;

                // Calculate the rotation needed for X-axis only
                float angleX = Mathf.Atan2(barrelDirection.y, barrelDirection.z) * Mathf.Rad2Deg;
                Quaternion barrelRotation = Quaternion.Euler(angleX, turret.barrels.localEulerAngles.y, turret.barrels.localEulerAngles.z);

                turret.barrels.localRotation = Quaternion.Slerp(
                    turret.barrels.localRotation,
                    barrelRotation,
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            BaseHealth baseHealth = other.gameObject.GetComponent<BaseHealth>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(enemyHealth.publicCurrentHealth);
            }
            else
            {
                Debug.LogError("Base object does not have a BaseHealth component attached.");
            }
            Destroy(gameObject);
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

    private void OnDestroy()
    {
        Spawner waveManager = FindObjectOfType<Spawner>();
        if (waveManager != null)
        {
            waveManager.EnemyDestroyed();
        }
    }
}