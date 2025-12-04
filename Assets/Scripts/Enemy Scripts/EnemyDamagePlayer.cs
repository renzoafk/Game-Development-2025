using UnityEngine;

public class EnemyDamagePlayer : MonoBehaviour
{
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float damageCooldown = 0.5f;

    private float lastDamageTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        if (Time.time < lastDamageTime + damageCooldown) return;

        lastDamageTime = Time.time;

        // your player health script must be on the same object as the Player tag
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }
}
