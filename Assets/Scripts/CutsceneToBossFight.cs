using System.Collections;
using UnityEngine;

public class FinalCutsceneZone : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueTrigger dialogueTrigger;  // dialogue for this cutscene

    [Header("NPC")]
    [SerializeField] private GameObject npcToDisappear;        // yellow character to vanish
    [SerializeField] private Collider2D bossGateCollider;      // collider blocking boss hallway

    [Header("Player auto move")]
    [SerializeField] private Transform walkStartPoint;         // player position when shot begins
    [SerializeField] private Transform walkStopPoint;          // where player stops near NPC
    [SerializeField] private float autoWalkSpeed = 3f;

    [Header("Player animation")]
    [SerializeField] private string speedFloatName = "Speed";  // Animator parameter that drives walk

    [Header("General")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float fadeDuration = 0.7f;

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;
        if (!other.CompareTag(playerTag)) return;

        hasPlayed = true;
        StartCoroutine(PlayCutscene(other));
    }

    private IEnumerator PlayCutscene(Collider2D playerCollider)
    {
        Transform player = playerCollider.transform;
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();

        // Stop player input immediately
        if (movement != null)
            movement.enabled = false;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (anim != null && !string.IsNullOrEmpty(speedFloatName))
            anim.SetFloat(speedFloatName, 0f);

        // Fade to black
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeToBlack(fadeDuration);

        // Snap player to starting point for the shot
        if (walkStartPoint != null)
            player.position = walkStartPoint.position;

        // Fade back in to show the setup shot
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeFromBlack(fadeDuration);

        // Auto walk the player toward the NPC
        if (walkStopPoint != null)
        {
            // Turn walking animation on
            if (anim != null && !string.IsNullOrEmpty(speedFloatName))
                anim.SetFloat(speedFloatName, Mathf.Abs(autoWalkSpeed));

            while (Vector2.Distance(player.position, walkStopPoint.position) > 0.05f)
            {
                player.position = Vector2.MoveTowards(
                    player.position,
                    walkStopPoint.position,
                    autoWalkSpeed * Time.deltaTime
                );
                yield return null;
            }

            // Hard stop
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            // Turn walking animation off so Idle plays
            if (anim != null && !string.IsNullOrEmpty(speedFloatName))
                anim.SetFloat(speedFloatName, 0f);
        }

        // Start dialogue
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
        }
        else
        {
            Debug.LogWarning("FinalCutsceneZone: dialogueTrigger not set on " + gameObject.name);
        }

        // Wait for dialogue to finish
        while (DialogueManager.Instance != null &&
               DialogueManager.Instance.IsDialogueOpen)
        {
            yield return null;
        }

        // Freeze player while we do the outro fade
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.SetPlayerFrozenExternally(true);

        // Fade out again
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeToBlack(fadeDuration);

        // Hide the NPC, not the player
        if (npcToDisappear != null)
            npcToDisappear.SetActive(false);

        // Open the path to the boss
        if (bossGateCollider != null)
            bossGateCollider.enabled = false;

        // Fade back in to gameplay
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeFromBlack(fadeDuration);

        // Unfreeze player and restore control
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.SetPlayerFrozenExternally(false);

        if (movement != null)
            movement.enabled = true;

        if (anim != null && !string.IsNullOrEmpty(speedFloatName))
            anim.SetFloat(speedFloatName, 0f);

        // Disable this trigger forever
        Collider2D myCol = GetComponent<Collider2D>();
        if (myCol != null)
            myCol.enabled = false;
    }
}
