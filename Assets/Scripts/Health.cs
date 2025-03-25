using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float lerpSpeed = 0.03f;
    
    [SerializeField] private Canvas _healthBar;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    
    public Action OnHeal;
    public Action OnDamage;
    public Action OnDie;
    private Camera _cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        _cam = Camera.main;
    }

    void Update()
    {
        if (healthSlider != null && healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Damage(10);
        }

        if (easeHealthSlider != null && healthSlider != null && healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
        
        if (_healthBar != null)
        {
            _healthBar.transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        }
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