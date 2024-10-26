using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private float currentHealth;
    public Slider healthSlider;


    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        float normalizedHealth = currentHealth / maxHealth;
        healthSlider.value = normalizedHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DeathHandler();
        }
    }

    void DeathHandler()
    {
        Destroy(gameObject);
    }
}
