using Microlight.MicroBar;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private MicroBar microBar;

    private void Awake()
    {
        // Auto-grab references if not set in Inspector
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (microBar == null)
            microBar = GetComponent<MicroBar>();

        // Safety check
        if (playerHealth == null || microBar == null)
        {
            Debug.LogWarning("PlayerHealthUI is missing references.", this);
            enabled = false;
            return;
        }

        // Initialize MicroBar with the player's max health
        microBar.Initialize(playerHealth.MaxHealth);

        // Set starting value WITHOUT animation
        microBar.UpdateBar(playerHealth.CurrentHealth, true); // skipAnimation = true
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    // This is called whenever PlayerHealth changes (current, max)
    private void HandleHealthChanged(int current, int max)
    {
        if (microBar == null)
            return;

        // Keep MicroBar's max HP in sync
        microBar.SetNewMaxHP(max);

        // Animate bar to new HP (uses default damage animation from the asset)
        microBar.UpdateBar(current);
    }
}
