using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healtg : MonoBehaviour
{
    public float health = 100f;

    void Update()
    {
        Debug.Log(health);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
