using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Key Settings")]
    [SerializeField] private string requiredKeyId = "Key1";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string openTriggerName = "Open";
    [SerializeField] private AnimationClip openClip;   // <— add this

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;

    private bool isOpen = false;
    private bool playerInRange = false;
    private PlayerInventory currentPlayer;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            playerInRange = true;
            currentPlayer = inv;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null && inv == currentPlayer)
        {
            playerInRange = false;
            currentPlayer = null;
        }
    }

    private void Update()
    {
        if (!playerInRange || isOpen || currentPlayer == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (currentPlayer.HasKey(requiredKeyId))
            {
                StartCoroutine(OpenAndDestroyCoroutine());
            }
        }
    }

    private IEnumerator OpenAndDestroyCoroutine()
    {
        if (isOpen) yield break;
        isOpen = true;

        // trigger animation
        if (animator != null && !string.IsNullOrEmpty(openTriggerName))
        {
            animator.SetTrigger(openTriggerName);
        }

        // play sound
        if (audioSource != null)
        {
            if (openSound != null)
                audioSource.PlayOneShot(openSound);
            else
                audioSource.Play();
        }

        // disable colliders immediately so player can go through
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        // wait for the clip length (fall back to a small time if not set)
        float waitTime = 0.1f;
        if (openClip != null)
            waitTime = openClip.length;

        yield return new WaitForSeconds(waitTime);

        // destroy door after animation finished
        Destroy(gameObject);
    }
}
