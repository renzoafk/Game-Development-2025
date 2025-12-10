using UnityEngine;

public class Victory_trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DeathManager.Instance.ShowVictoryScreen();
        }
    }
}
