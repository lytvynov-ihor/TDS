using UnityEngine;

public class EnemyVehicleTurret : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    public Transform firePoint;
    public int damage = 20;
    public float range = 100f;

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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Tower");
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
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }

    void FireBeam()
    {
        RaycastHit hit;
        Vector3 forward = firePoint.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(firePoint.position, forward, Color.red, 5.0f);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Tower"))
            {
                Tower health = hit.collider.GetComponent<Tower>();
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