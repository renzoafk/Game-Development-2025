using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [SerializeField] private string keyId;
    [SerializeField] private AudioClip pickupSound;   // drag your sound here in the Inspector
    [SerializeField] private float pickupVolume = 1f; // optional

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        inventory.AddKey(keyId);

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupVolume);
        }

        Destroy(gameObject);
    }
}
