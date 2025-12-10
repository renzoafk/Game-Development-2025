using UnityEngine;

public class MiniBossCombat : MonoBehaviour
{
    [Header("Attack Hitbox")]
    public GameObject attackHitbox;

    private void Awake()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    // Called by Animation Event at start of the attack swing
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    // Called by Animation Event at end of the attack swing
    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
