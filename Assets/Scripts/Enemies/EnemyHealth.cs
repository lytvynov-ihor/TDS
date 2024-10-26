using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    private float currentHealth;
    public Slider healthSlider;
    public int reward = 25;


    private Money money;

    void Start()
    {
        money = FindObjectOfType<Money>();
        currentHealth = maxHealth; // Initialize current health to max health
    }

    private void Update()
    {
        float normalizedHealth = currentHealth / maxHealth;
        healthSlider.value = normalizedHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce current health by the damage amount

        if (currentHealth <= 0)
        {
            Die(); // If current health is less than or equal to 0, enemy dies
        }
    }

    void Die()
    {
        // Handle enemy death
        Destroy(gameObject); // Destroy the enemy GameObject
        money.IncreaseCash(reward);
    }
}
