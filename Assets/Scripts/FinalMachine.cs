using UnityEngine;

public class FinalMachine : MonoBehaviour
{
    [SerializeField] private string[] requiredKeyIds;  // All keys needed
    [SerializeField] private Door finalDoor;           // Reference to the final door

    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null && inventory.HasAllKeys(requiredKeyIds))
        {
            used = true;
            Debug.Log("All keys collected, using machine");

            if (finalDoor != null)
            {
                // Call OpenDoor on the final door
                finalDoor.SendMessage("OpenDoor", SendMessageOptions.DontRequireReceiver);
            }
        }
        else if (inventory != null)
        {
            Debug.Log("Machine: not all keys collected");
            // You could show UI here, for example "You still need more keycards"
        }
    }
}
