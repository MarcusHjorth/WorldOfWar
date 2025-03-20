using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    public Action OnHeal;
    public Action OnDamage;
    public Action OnDie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(float amount)
    {
        AdjustHealth(amount);

        if (!IsDead())
        {
            OnHeal?.Invoke();
        }
    }

    public void Damage(float damage)
    {
        AdjustHealth(-damage);

        if (!IsDead())
        {
            OnDamage?.Invoke();
        }
    }

    private void AdjustHealth(float health)
    {
        if (IsDead()) return;
        
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}