using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TutorialFadeText tutorialText;
    [SerializeField] private bool triggerOnce = true;

    [Header("Optional key requirement")]
    [SerializeField] private string requiredKeyId = "";   // leave empty for normal tutorials

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (!other.CompareTag("Player"))
            return;

        // If this tutorial needs a key, check the player's inventory
        if (!string.IsNullOrEmpty(requiredKeyId))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory == null || !inventory.HasKey(requiredKeyId))
            {
                // Player doesn't have the key yet -> do nothing, and do NOT mark hasTriggered
                return;
            }
        }

        // From here on, all conditions are satisfied
        if (triggerOnce && hasTriggered)
            return;

        hasTriggered = true;

        if (tutorialText != null)
        {
            tutorialText.Show();
        }
        else
        {
            Debug.LogWarning("TutorialTrigger: tutorialText reference not set.", this);
        }
    }
}
