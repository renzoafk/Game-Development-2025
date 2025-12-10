using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage; // optional, just for color
    [SerializeField] private Gradient healthGradient; // Better than Lerp for smooth color transition

    private void Awake()
    {
        // Auto-hook if not set in inspector
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
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

    private void Start()
    {
        if (playerHealth == null || healthSlider == null) return;

        // Initialize slider values
        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;
        
        // Update color immediately
        UpdateHealthColor();
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = max;
        healthSlider.value = current;
        
        UpdateHealthColor();
    }
    
    private void UpdateHealthColor()
    {
        if (fillImage == null || playerHealth == null) return;
        
        if (playerHealth.CurrentHealth <= 0)
        {
            // Hide the bar completely when dead
            fillImage.enabled = false;
        }
        else
        {
            fillImage.enabled = true;
            
            float healthPercentage = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
            
            // Method 1: Using Gradient (recommended)
            if (healthGradient != null)
            {
                fillImage.color = healthGradient.Evaluate(healthPercentage);
            }
            // Method 2: Using Color.Lerp (fallback)
            else
            {
                fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
            }
        }
    }
    
    // Optional: Call this from inspector button for testing
    [ContextMenu("Test Health UI")]
    public void TestHealthUI()
    {
        if (playerHealth != null)
        {
            // Simulate damage
            playerHealth.TakeDamage(1);
        }
    }
    
    // Optional: Add this for debug info in Editor
    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    #endif
}