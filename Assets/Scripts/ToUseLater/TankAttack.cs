using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    private GameObject target;
    private float damage;
    private float speed = 10f;

    void Update()
    {
        if (target != null)
        {
            // Move towards the target
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Check if we reached the target
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                DealDamage();
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void DealDamage()
    {
        //Health health = target.GetComponent<Health>();
        //if (health != null)
        //{
         //   health.TakeDamage(damage);
       // }
    }
}
