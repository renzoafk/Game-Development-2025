using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Collider2D hitboxCollider;

    private void Awake()
    {
        if (hitboxCollider == null)
            hitboxCollider = GetComponent<Collider2D>();

        // hitbox should start OFF
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    // called by Animation Event when the swing becomes active
    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    // called by Animation Event when swing ends
    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
    // filter by layer
    if (((1 << other.gameObject.layer) & enemyLayerMask) == 0)
        return;

    EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
    
    // FIX THIS: Use FindGameObjectWithTag, not FindGameObjectsWithTag
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player == null) return; // Safety check
    
    Vector2 playerPosition = player.transform.position;
    
    if (enemy != null)
    {
        enemy.TakeDamage(damage, playerPosition); // Use player position, not hitbox position
        Debug.Log("[MeleeHitbox] Hit enemy: " + other.name);
    }
    }
}   
