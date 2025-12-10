using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dragon"))
        {
            DragonBossHealth dragon = collision.GetComponent<DragonBossHealth>();
            if (dragon != null)
                dragon.TakeDamage(damage);
        }
    }
}
