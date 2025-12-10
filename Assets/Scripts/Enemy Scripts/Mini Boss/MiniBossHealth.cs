using System.Collections;
using UnityEngine;

public class MiniBossHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 20;
    [HideInInspector] public int currentHealth;

    [Header("Player Heal Reward")]
    public int healAmount = 5;   // how much HP player gets when boss dies

    [Header("Components")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public MiniBossAI ai;
    public MiniBossCombat combat;
    public Rigidbody2D rb;
    public Collider2D col;

    private bool isDead = false;
    private Color originalColor;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
        if (ai == null) ai = GetComponent<MiniBossAI>();
        if (combat == null) combat = GetComponent<MiniBossCombat>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (col == null) col = GetComponent<Collider2D>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

   public void TakeDamage(int amount)
    {
    if (isDead) return;

    // Ignore hit triggers that do NO damage
    if (amount <= 0)
        return;

    currentHealth -= amount;
    Debug.Log($"MiniBoss took {amount} damage. HP = {currentHealth}");

    // Flash only when hit for real
    if (spriteRenderer != null)
        StartCoroutine(DamageFlash());

    if (currentHealth <= 0)
        Die();
    }


    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("MiniBoss died.");

        HealPlayerOnDeath();

        // Turn off behavior
        if (ai != null) ai.enabled = false;
        if (combat != null) combat.enabled = false;
        if (rb != null) rb.simulated = false;
        if (col != null) col.enabled = false;

        // Optional death animation
        if (animator != null && HasParameter(animator, "Die"))
            animator.SetTrigger("Die");

        Destroy(gameObject, 1.5f);
    }

    private void HealPlayerOnDeath()
    {
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();

        if (player != null)
        {
            player.Heal(healAmount);
            Debug.Log($"Player healed for {healAmount} HP!");
        }
        else
        {
            Debug.LogWarning("PlayerHealth NOT found in scene.");
        }
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private bool HasParameter(Animator anim, string paramName)
    {
        foreach (var p in anim.parameters)
            if (p.name == paramName)
                return true;

        return false;
    }
}
