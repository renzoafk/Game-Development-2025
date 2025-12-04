using UnityEngine;

public class DebugTrigger2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DebugTrigger2D ENTER: " + other.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("DebugTrigger2D EXIT: " + other.name);
    }
}
