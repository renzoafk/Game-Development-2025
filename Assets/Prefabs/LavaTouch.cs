using UnityEngine;

public class LavaDamagePlayer : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private float lastDamageTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log($"[LavaDamagePlayer] {name} collided with {collision.collider.name}, tag={collision.collider.tag}");

        if (!collision.collider.CompareTag("Player")) return;

        if (Time.time < lastDamageTime + damageCooldown)
        {
            Debug.Log("[LavaDamagePlayer] Still on cooldown, no damage");
            return;
        }
        lastDamageTime = Time.time;

        // your player health script must be on the same object as the Player tag
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log($"[LavaDamagePlayer] Damaging PLAYER for {contactDamage}");
            playerHealth.TakeDamage(contactDamage);
        }
        else
        {
            Debug.Log("[LavaDamagePlayer] PlayerHealth NOT found on collider tagged Player");
        }
    }
}
