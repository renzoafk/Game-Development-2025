using UnityEngine;
using System.Collections;

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


        StartCoroutine(HitFeedbackCoroutine(hitSourceWorldPos));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFeedbackCoroutine(Vector2 hitSourceWorldPos)
    {
        Vector2 dir = ((Vector2)transform.position - hitSourceWorldPos).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        sr.color = hitColor;

        yield return new WaitForSeconds(knockbackTime);

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(flashTime);

        sr.color = originalColor;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log($"[EnemyHealth] {name} died");
        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        Destroy(gameObject, 0.3f);
    }
}
