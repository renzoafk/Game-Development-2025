using UnityEngine;

public class FinalMachine : MonoBehaviour
{
    [SerializeField] private string[] requiredKeyIds;   // All required keycard IDs
    [SerializeField] private Door finalDoor;            // Drag your final door here in Inspector

    private bool used;
    private bool playerInRange;
    private PlayerInventory playerInventory;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            playerInRange = true;
            playerInventory = inv;
            Debug.Log("Machine: player in range");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null && inv == playerInventory)
        {
            playerInRange = false;
            playerInventory = null;
            Debug.Log("Machine: player left range");
        }
    }

    private void Update()
    {
        if (used) return;
        if (!playerInRange) return;
        if (playerInventory == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Machine: E pressed");

            if (requiredKeyIds == null || requiredKeyIds.Length == 0)
            {
                Debug.LogWarning("FinalMachine: requiredKeyIds is empty in Inspector");
                return;
            }

            bool hasAll = true;

            // Check and log missing keys
            foreach (var key in requiredKeyIds)
            {
                if (!playerInventory.HasKey(key))
                {
                    hasAll = false;
                    Debug.Log("Machine: missing key '" + key + "'");
                }
            }

            if (!hasAll)
            {
                Debug.Log("Machine: not all keys collected");
                return;
            }

            used = true;
            Debug.Log("Machine: all keys collected, opening door");

            if (finalDoor == null)
            {
                Debug.LogError("FinalMachine: finalDoor is NOT assigned in Inspector");
            }
            else
            {
                // Use the public method on Door so animation and sound play
                finalDoor.OpenDoor();
            }
        }
    }
}
