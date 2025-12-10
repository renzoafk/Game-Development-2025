using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;

    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueManager : MonoBehaviour
{
    // Singleton so other scripts can call DialogueManager.Instance
    public static DialogueManager Instance { get; private set; }

    // Other scripts (intro cutscene, etc) can listen to this
    public static event System.Action OnDialogueEnd;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;

    [Header("Player references")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Attack playerAttack;   // your attack script

    // Full dialogue state
    private Dialogue currentDialogue;
    private int index = 0;
    public bool IsDialogueOpen { get; private set; }

    // Ambient one shot state
    private Coroutine oneShotRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Only advance when a real dialogue is open
        if (!IsDialogueOpen) return;

        // Left mouse to advance
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    // =========================================================
    // FULL DIALOGUE (cutscenes, conversations)
    // =========================================================

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines == null || dialogue.dialogueLines.Count == 0)
        {
            Debug.LogWarning("DialogueManager: Dialogue is null or has no lines.");
            return;
        }

        // Stop any ambient timer so it does not interfere
        if (oneShotRoutine != null)
        {
            StopCoroutine(oneShotRoutine);
            oneShotRoutine = null;
        }

        currentDialogue = dialogue;
        index = 0;
        IsDialogueOpen = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        FreezePlayer(true);
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentDialogue == null) return;

        if (index < 0 || index >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.dialogueLines[index];

        if (nameText != null)
            nameText.text = line.character != null ? line.character.name : "";

        if (dialogueText != null)
            dialogueText.text = line.line;

        if (portraitImage != null)
            portraitImage.sprite = line.character != null ? line.character.icon : null;
    }

    private void NextLine()
    {
        index++;

        if (currentDialogue == null || index >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }

    private void EndDialogue()
    {
        IsDialogueOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        currentDialogue = null;

        FreezePlayer(false);

        // Notify listeners (intro cutscene, etc)
        OnDialogueEnd?.Invoke();
    }

    /// <summary>
    /// Freezes or unfreezes player movement and attack.
    /// </summary>
    private void FreezePlayer(bool frozen)
    {
        // Auto find if not wired in the Inspector
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerAttack == null)
            playerAttack = FindObjectOfType<Attack>();

        if (playerMovement != null)
            playerMovement.enabled = !frozen;

        if (playerAttack != null)
            playerAttack.enabled = !frozen;
    }

    // =========================================================
    // AMBIENT ONE SHOT LINES (no freeze, auto hide)
    // =========================================================

    /// <summary>
    /// Show a single line that auto hides after duration.
    /// Does nothing if a full dialogue is currently open.
    /// </summary>
    public void ShowAmbient(string name, Sprite portrait, string text, float duration)
    {
        // Do not interrupt real dialogue
        if (IsDialogueOpen) return;

        if (nameText != null)
            nameText.text = string.IsNullOrEmpty(name) ? "" : name;

        if (dialogueText != null)
            dialogueText.text = text;

        if (portraitImage != null)
            portraitImage.sprite = portrait;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (oneShotRoutine != null)
            StopCoroutine(oneShotRoutine);

        oneShotRoutine = StartCoroutine(HideOneShotAfter(duration));
    }


    /// <summary>
    /// Hide ambient text immediately, but will not close an active full dialogue.
    /// </summary>
    public void HideOneShotImmediate()
    {
        // If a full dialogue is open, do not touch it
        if (IsDialogueOpen) return;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (oneShotRoutine != null)
        {
            StopCoroutine(oneShotRoutine);
            oneShotRoutine = null;
        }
    }

    private IEnumerator HideOneShotAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Only hide if a full dialogue did not start in between
        if (!IsDialogueOpen && dialoguePanel != null)
            dialoguePanel.SetActive(false);

        oneShotRoutine = null;
    }
}
