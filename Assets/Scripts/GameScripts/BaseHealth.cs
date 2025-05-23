using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;// Maximum health of the base
    public Text baseHealthText;
    public Slider healthSlider;//actually don't remove
    private float currentHealth; // Current health of the base
    public AudioClip loseTheme;
    public GameObject loseScreen;

    void Start()
    {
        if (loseScreen != null)
            loseScreen.SetActive(false);

        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateHealth();
        float normalizedHealth = currentHealth / maxHealth;
        healthSlider.value = normalizedHealth;
    }

    // Function to take damage to the base
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Decrease the current health by the damage amount

        // Check if the base is destroyed
        if (currentHealth <= 0)
        {
            DestroyBase(); // Call the function to destroy the base
        }
    }

    // Function to destroy the base
    void DestroyBase()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the base game object
            AudioManager.Instance.PlayClip(loseTheme);
            loseScreen.SetActive(true);
            // You can add additional functionality here such as game over state or effects
        }
       
    }

    public void UpdateHealth()
    {
        baseHealthText.text = "Reputation Points: " + currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public bool healthPositive()
    {
        return currentHealth > 0;
    }

    //private void OnDestroy()
    //{
    //    AudioManager.Instance.PlayClip(loseTheme);
    //    loseScreen.SetActive(true);
    //}
}
