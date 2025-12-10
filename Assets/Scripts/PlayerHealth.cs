using UnityEngine;
using System;
using System.Collections; // Keep this namespace

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private bool isDead;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Events
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action<int> OnDamageTaken;
    public event Action OnDeath;

    // Public properties
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;

        // Initialize UI with current health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        // If already dead, ignore any more damage
        if (isDead) return;

        // Clamp so we never go below zero
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log("Player took damage, current HP: " + currentHealth);

        // Only play hurt feedback while still alive
        if (currentHealth > 0)
        {
            animator?.SetTrigger("Hurt");
            StartCoroutine(FlashRed());
        }

        // Notify listeners
        OnDamageTaken?.Invoke(amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // If we hit 0, die once
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;   // make sure this only runs once
        isDead = true;

        Debug.Log("Player died!");

        // Trigger death event
        OnDeath?.Invoke();

        // Tell DeathManager to show the death screen
        if (DeathManager.Instance != null)
        {
            DeathManager.Instance.ShowDeathScreen();
        }
        else
        {
            Debug.LogWarning("Player died but no DeathManager instance found in the scene.");
        }

        // (Optional) disable movement / input here if you want
        // GetComponent<PlayerMovement>()?.enabled = false;
    }

    // Optional: Healing method
    public void Heal(int amount)
    {
        if (isDead) return; // usually you don't heal corpses lol

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed, current HP: " + currentHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void HealToFull()
    {
    if (isDead) return;

    currentHealth = maxHealth;
    Debug.Log("Player healed to FULL HP: " + currentHealth);

    OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }


    // Optional: Set health directly
    public void SetHealth(int newHealth)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null)
            yield break;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
