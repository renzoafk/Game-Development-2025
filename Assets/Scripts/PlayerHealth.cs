using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;      // reference to HealthBar Slider
    [SerializeField] private Image healthFillImage;    // Fill image of the slider
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    private void Awake()
    {
        currentHealth = maxHealth;

        // set up slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player took damage, current HP: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthFillImage != null)
        {
            float t = (float)currentHealth / maxHealth;
            healthFillImage.color = Color.Lerp(lowHealthColor, fullHealthColor, t);
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: play death animation, disable movement, fade out, etc.
    }

    // Optional helpers if you ever need them from other scripts
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
