using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    private int _currentHealth;

    // Reference to the Health UI Manager
    private HealthUIManager _healthUIManager;

    private void Start()
    {
        _currentHealth = maxHealth;

        // Find the Health UI Manager in the scene or set it via the inspector
        _healthUIManager = FindObjectOfType<HealthUIManager>();
        if (_healthUIManager != null)
        {
            _healthUIManager.InitializeHealthBar(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage taken: " + damage);
        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        // Update the health bar
        if (_healthUIManager != null)
        {
            _healthUIManager.UpdateHealthBar(_currentHealth);
        }

        if (_currentHealth == 0)
        {
            Death();
        }
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }

        // Update the health bar
        if (_healthUIManager != null)
        {
            _healthUIManager.UpdateHealthBar(_currentHealth);
        }
    }

    private void Death()
    {
        Debug.Log("Player has died!");
        // Add death logic here
    }
}