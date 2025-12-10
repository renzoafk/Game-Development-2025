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

        // hitbox starts OFF
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    // Animation Event: enable hitbox
    public void EnableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    // Animation Event: disable hitbox
    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Filter by layers
        if (((1 << other.gameObject.layer) & enemyLayerMask) == 0)
            return;

        // Handle NORMAL enemies
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        MiniBossHealth miniBoss = other.GetComponentInParent<MiniBossHealth>();
        // Handle BOSS dragon
        DragonBossHealth boss = other.GetComponentInParent<DragonBossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            return;
}

        // Get player position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        Vector2 playerPosition = player.transform.position;

        // Damage regular enemy
        if (enemy != null)
        {
            enemy.TakeDamage(damage, playerPosition);
            Debug.Log("[MeleeHitbox] Hit enemy: " + other.name);
            return;
        }

        // Damage boss
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Debug.Log("[MeleeHitbox] Hit BOSS: " + other.name);
            return;
        }

           if (miniBoss != null)
        {
            miniBoss.TakeDamage(damage);
            Debug.Log("[MeleeHitbox] Hit MINIBOSS: " + other.name);
            return;
        }
    }
}
