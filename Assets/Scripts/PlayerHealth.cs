using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections; // Keep this namespace

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private Animator animator;
    
    private SpriteRenderer spriteRenderer;

    // Events (add these)
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action<int> OnDamageTaken;
    public event Action OnDeath;

    // Public properties (add these)
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        
        // Initialize UI with current health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage, current HP: " + currentHealth);

        animator?.SetTrigger("Hurt");
        StartCoroutine(FlashRed());
        // Notify about damage
        OnDamageTaken?.Invoke(amount);
        
        // Notify about health change
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        
        // Trigger death event
        OnDeath?.Invoke();
        
        // TODO restart scene or trigger death animation
    }
    
    // Optional: Healing method (update to trigger event)
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed, current HP: " + currentHealth);
        
        // Notify about health change
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    // Optional: Set health directly
    public void SetHealth(int newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        
    }
}