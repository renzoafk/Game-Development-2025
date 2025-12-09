using UnityEngine;
using System;   // for Action

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float invincibilityTime = 0.3f;

    private int currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;
    private float invincibleUntil;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    // UI / other systems can subscribe to this
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (isInvincible && Time.time >= invincibleUntil)
            isInvincible = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"[PlayerHealth] Took {amount} damage. Current = {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
        else
            MakeTemporarilyInvincible();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log($"[PlayerHealth] Healed {amount}. Current = {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void MakeTemporarilyInvincible()
    {
        isInvincible = true;
        invincibleUntil = Time.time + invincibilityTime;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[PlayerHealth] Player died!");

        // Optional: trigger death animation
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("die");

        OnDeath?.Invoke();

        if (DeathManager.Instance != null)
            DeathManager.Instance.ShowDeathScreen();
        else
            Time.timeScale = 0f;
    }
}
