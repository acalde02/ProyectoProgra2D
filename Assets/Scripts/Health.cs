using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private Image healthBar;
    private int _currentHealth;
    private float _maxHealthInverse;

    protected void Start()
    {
        _currentHealth = maxHealth;
        _maxHealthInverse = 1f/maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Death();
        }
    }

    public void Heal(int heal)
    {
        _currentHealth += heal;
    }

    protected void UpdateHealthBar()
    {
        healthBar.fillAmount = _currentHealth * _maxHealthInverse;
    }

    protected virtual void Death()
    {
        
    }
    
    
}
