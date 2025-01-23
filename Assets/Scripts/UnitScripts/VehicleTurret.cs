using UnityEngine;

public class VehicleTurret : MonoBehaviour
{
    //public Transform turret;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    public Transform firePoint;
    public int damage = 20;
    public float range = 100f;
    //public GameObject projectilePrefab;

    private float nextFireTime;

    void Update()
    {
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            AimTurret(enemy.transform);
            if (Time.time >= nextFireTime)
            {
                FireBeam();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            return;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void AimTurret(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, smoothedRotation.eulerAngles.y, 0); // Only Y-axis rotates
    }

    void FireBeam()
    {
        RaycastHit hit;
        Vector3 forward = firePoint.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(firePoint.position, forward, Color.red, 5.0f);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
        else
        {
            return;
        }
    }
}
