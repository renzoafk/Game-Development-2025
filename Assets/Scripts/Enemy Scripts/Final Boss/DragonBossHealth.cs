using UnityEngine;
using System.Collections;

public class DragonBossHealth : MonoBehaviour
{
    public int maxHealth = 200;
    private int currentHealth;

    private SpriteRenderer[] sprites;  // <-- All body parts!

    private DragonBossAI bossAI;

    void Start()
    {
        currentHealth = maxHealth;

        // Get ALL SpriteRenderers (including children!)
        sprites = GetComponentsInChildren<SpriteRenderer>();

        bossAI = GetComponent<DragonBossAI>();
    }

    public void TakeDamage(int amount)
    {
    Debug.Log($"DRAGON TOOK DAMAGE: {amount} HP | Current HP BEFORE damage: {currentHealth}");

    currentHealth -= amount;

    Debug.Log($"DRAGON HP AFTER DAMAGE: {currentHealth}");

    StartCoroutine(DamageFlash());

    if (currentHealth <= 0)
    {
        Debug.Log("DRAGON HAS DIED!");
        bossAI.KillBoss();
    }
    }


    private IEnumerator DamageFlash()
    {
        // Turn RED
        foreach (SpriteRenderer sr in sprites)
            sr.color = Color.red;

        yield return new WaitForSeconds(0.15f);

        // Turn back to WHITE
        foreach (SpriteRenderer sr in sprites)
            sr.color = Color.white;
    }
}
