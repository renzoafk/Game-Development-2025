using UnityEngine;

public class AmbientDialogueZone : MonoBehaviour
{
    [Header("Dialogue Information")]
    public string characterName;       // optional
    public Sprite portrait;            // optional
    [TextArea]
    public string line;

    [Header("Timing")]
    public float visibleTime = 2.5f;

    [Header("Trigger Behavior")]
    public bool triggerOnce = false;

    private bool hasTriggered = false;
    private bool playerInside = false;
    private Coroutine autoHideRoutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && hasTriggered) return;

        hasTriggered = true;
        playerInside = true;

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowAmbient(
                characterName,
                portrait,
                line,
                visibleTime
            );
        }

        if (autoHideRoutine != null)
            StopCoroutine(autoHideRoutine);

        autoHideRoutine = StartCoroutine(AutoHide());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideOneShotImmediate();
        }

        if (autoHideRoutine != null)
        {
            StopCoroutine(autoHideRoutine);
            autoHideRoutine = null;
        }
    }

    private System.Collections.IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(visibleTime);

        if (playerInside && DialogueManager.Instance != null)
        {
            DialogueManager.Instance.HideOneShotImmediate();
            playerInside = false;
        }

        autoHideRoutine = null;
    }
}
