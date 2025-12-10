using UnityEngine;

public class IntroDialogueFreeze : MonoBehaviour
{
    private PlayerMovement movement;

    private void Awake()
    {
        movement = FindFirstObjectByType<PlayerMovement>();

        if (movement != null)
            movement.SetCanMove(false);
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnd += UnfreezePlayer;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnd -= UnfreezePlayer;
    }

    private void UnfreezePlayer()
    {
        if (movement != null)
            movement.SetCanMove(true);

        gameObject.SetActive(false);
    }
}
