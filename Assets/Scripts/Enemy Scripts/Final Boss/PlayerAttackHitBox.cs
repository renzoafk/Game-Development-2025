using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hitbox touched: " + collision.name);

        

        // Mini Boss
        MiniBossHealth miniBoss = collision.GetComponentInParent<MiniBossHealth>();
        if (miniBoss != null)
        {
            miniBoss.TakeDamage(damage);
            return;
        }

        // Dragon boss
        DragonBossHealth dragon = collision.GetComponentInParent<DragonBossHealth>();
        if (dragon != null)
        {
            dragon.TakeDamage(damage);
            return;
        }
    }
}
