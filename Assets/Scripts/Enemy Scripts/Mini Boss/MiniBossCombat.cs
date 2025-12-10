using UnityEngine;

public class MiniBossCombat : MonoBehaviour
{
    [Header("Hitbox")]
    public GameObject attackHitbox;

    private void Start()
    {
        attackHitbox.SetActive(false);
    }

    // Called by Animation Event
    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
    }

    // Called by Animation Event
    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }
}
