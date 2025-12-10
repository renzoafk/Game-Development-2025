using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Feedback")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackTime = 0.15f;
    [SerializeField] private float flashTime = 0.15f;
    [SerializeField] private Color hitColor = Color.red;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Color originalColor;
    private bool isDead = false;

    public UnityEvent OnDied;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        currentHealth = maxHealth;
        Debug.Log($"[EnemyHealth] {name} spawned with {currentHealth} HP");
    }

    public void TakeDamage(int amount, Vector2 hitSourceWorldPos)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"[EnemyHealth] {name} took {amount} damage at {hitSourceWorldPos}. HP now {currentHealth}/{maxHealth}");

        // Knockback + flash coroutine
        StartCoroutine(HitFeedbackCoroutine(hitSourceWorldPos));

        // Check death
        if (currentHealth <= 0)
        {
            // Optional UnityEvent
            OnDied?.Invoke();

            // Always run Die()
            Die();
        }
    }

    private IEnumerator HitFeedbackCoroutine(Vector2 hitSourceWorldPos)
    {
        Vector2 dir = ((Vector2)transform.position - hitSourceWorldPos).normalized;

        // Start knockback
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        // Flash color
        sr.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        sr.color = originalColor;

        // Finish knockback
        yield return new WaitForSeconds(knockbackTime - flashTime);

        // Stop sliding
        rb.linearVelocity = Vector2.zero;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[EnemyHealth] {name} died");

        // ⭐ HEAL PLAYER TO FULL WHEN ENEMY DIES ⭐
        PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
        if (player != null)
        {
            player.HealToFull();
            Debug.Log("Enemy killed -> Player healed to FULL HP!");
        }

        // Disable all colliders so no more hits register
        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        // Play death animation later if needed
        Destroy(gameObject, 0.3f);
    }
}
