using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string requiredKeyId = "Key1";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;
    private bool playerInRange = false;
    private PlayerInventory currentPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            Debug.Log("Door: player entered trigger");
            playerInRange = true;
            currentPlayer = inv;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv != null && inv == currentPlayer)
        {
            Debug.Log("Door: player left trigger");
            playerInRange = false;
            currentPlayer = null;
        }
    }

    private void Update()
    {
        if (!playerInRange || isOpen || currentPlayer == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Door: E pressed");
            if (currentPlayer.HasKey(requiredKeyId))
            {
                Debug.Log("Door: player has key, opening");
                OpenDoor();
            }
            else
            {
                Debug.Log("Door: missing key " + requiredKeyId);
            }
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;

        // Turn off all colliders on this door
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        // Hide sprite, or replace with open door sprite
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Debug.Log("Door opened");
    }
}
