using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player took damage, current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");

        // Optional: trigger death animation
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("die");

        if (DeathManager.Instance != null)
            DeathManager.Instance.ShowDeathScreen();
        else
            Time.timeScale = 0f;
    }
}
