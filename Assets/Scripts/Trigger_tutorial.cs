using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialFadeText tutorialText;
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnce && hasTriggered) return;
        if (!other.CompareTag("Player")) return;

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
