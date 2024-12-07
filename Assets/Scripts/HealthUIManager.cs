using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthBarFill; // The green health bar fill

    private float _maxHealthInverse;

    // Initialize the health bar with the maximum health
    public void InitializeHealthBar(int maxHealth)
    {
        _maxHealthInverse = 1f / maxHealth;
        UpdateHealthBar(maxHealth); // Start with full health
    }

    // Update the health bar fill amount based on current health
    public void UpdateHealthBar(int currentHealth)
    {
        healthBarFill.fillAmount = currentHealth * _maxHealthInverse;
    }
}