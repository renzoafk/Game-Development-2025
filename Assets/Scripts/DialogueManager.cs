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
    // Singleton so DialogueTrigger can call DialogueManager.Instance
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
    [SerializeField] private Attack playerAttack;   // your attack script is called Attack

    private Dialogue currentDialogue;
    private int index = 0;
    public bool IsDialogueOpen { get; private set; }

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
        if (!IsDialogueOpen) return;

        // Left mouse to advance
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines == null || dialogue.dialogueLines.Count == 0)
        {
            Debug.LogWarning("DialogueManager: Dialogue is null or has no lines.");
            return;
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
        // Auto-find if not wired in the Inspector
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerAttack == null)
            playerAttack = FindObjectOfType<Attack>();

        if (playerMovement != null)
            playerMovement.enabled = !frozen;

        if (playerAttack != null)
            playerAttack.enabled = !frozen;
    }
}
