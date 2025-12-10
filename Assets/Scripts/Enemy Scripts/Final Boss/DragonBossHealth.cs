using UnityEngine;

public class DragonBossHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    private DragonBossAI bossAI;

    void Start()
    {
        currentHealth = maxHealth;
        bossAI = GetComponent<DragonBossAI>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            bossAI.KillBoss();
        }
    }
}
