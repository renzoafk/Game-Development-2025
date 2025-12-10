using UnityEngine;

public class DragonBossCombat : MonoBehaviour
{
    [Header("Hitboxes")]
    public GameObject attackHitbox;
    public GameObject specialHitbox;

    private void Start()
    {
        if (attackHitbox != null) attackHitbox.SetActive(false);
        if (specialHitbox != null) specialHitbox.SetActive(false);
    }

    // Called From Animation Events
    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    public void EnableSpecialHitbox()
    {
        if (specialHitbox != null)
            specialHitbox.SetActive(true);
    }

    public void DisableSpecialHitbox()
    {
        if (specialHitbox != null)
            specialHitbox.SetActive(false);
    }
}
