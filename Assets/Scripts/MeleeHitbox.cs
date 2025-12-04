using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, transform.position);
            Debug.Log("Hit enemy: " + other.name);
        }
    }
}
