using UnityEngine;

public class StartIntroDialogue : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private void Start()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
        }
        else
        {
            Debug.LogWarning("StartIntroDialogue: dialogueTrigger not assigned.");
        }
    }
}