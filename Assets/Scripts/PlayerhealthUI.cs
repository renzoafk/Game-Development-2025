using Microlight.MicroBar;
using UnityEngine;

// Simple standalone driver for a MicroBar (not required if you're using PlayerHealthUI)
public class MicrobarHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MicroBar microBar;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth = 5;

    private void Awake()
    {
        if (microBar == null)
            microBar = GetComponent<MicroBar>();

        if (microBar == null)
        {
            Debug.LogWarning("MicrobarHealth: Missing MicroBar reference.", this);
            enabled = false;
            return;
        }

        // Initialize bar and set starting value (no animation)
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        microBar.Initialize(maxHealth);
        microBar.UpdateBar(currentHealth, true); // skipAnimation = true
    }

    // Public helpers if you ever want to drive this script directly
    public void Damage(int amount)
    {
        SetHealth(currentHealth - amount);
    }

    public void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    public void SetHealth(int value)
    {
        if (microBar == null) return;

        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        microBar.UpdateBar(currentHealth); // uses asset's default animation logic
    }
}
