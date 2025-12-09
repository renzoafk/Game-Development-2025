using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage; // optional, just for color

    private void Awake()
    {
        // Auto-hook if not set in inspector
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
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

        healthSlider.minValue = 0;
        healthSlider.maxValue = playerHealth.MaxHealth;
        healthSlider.value = playerHealth.CurrentHealth;
    }

    private void HandleHealthChanged(int current, int max)
    {
        if (healthSlider == null) return;

        healthSlider.maxValue = max;
        healthSlider.value = current;

        if (fillImage != null)
        {
            if (current <= 0)
            {
                // hide the bar completely when dead
                fillImage.enabled = false;
            }
            else
            {
                fillImage.enabled = true;

                float t = (float)current / max; // 1 = full, 0 = empty
                fillImage.color = Color.Lerp(Color.red, Color.green, t);
            }
        }
    }

}
