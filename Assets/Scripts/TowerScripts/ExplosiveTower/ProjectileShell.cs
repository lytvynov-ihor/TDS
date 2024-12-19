using System;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileShell : MonoBehaviour
{
public QuadraticCurve curve;
public int attackDamage;
public float speed;
public ParticleSystem particle;

private float sampleTime;

void Start()
{
    sampleTime = 0f;
}

void Update()
{
    sampleTime += Time.deltaTime * speed;
    transform.position = curve.evaluate(sampleTime);
    transform.forward = curve.evaluate(sampleTime+0.001f) - transform.position;

    if (sampleTime >= 0.5f)
    {
        speed += 0.03f;
    }

    if (sampleTime >= 1f)
    {
        Instantiate(particle, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Enemy"))
            {
                EnemyHealth health = c.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(attackDamage);
                }
            }
        }
        Destroy(curve.GameObject());
        Destroy(curve.b.GameObject());
        Destroy(curve.control.GameObject());
        Destroy(gameObject);
    }
}
}
