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

    public void Heal(float amount)
    {
        AdjustHealth(amount);
        OnHeal?.Invoke();
    }

    public void Damage(float damage)
    {
        AdjustHealth(-damage);
        OnDamage?.Invoke();
    }

    private void AdjustHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
    }
}