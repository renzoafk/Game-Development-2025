using UnityEngine;

public class DragonAnimatorController : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // ---- Animation Triggers ----
    public void PlayAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void PlayDeath()
    {
        anim.SetTrigger("Dead");
    }

    public void PlayFlying(bool isFlying)
    {
        anim.SetBool("Flying", isFlying);
    }

    public void PlayLanding()
    {
        anim.SetTrigger("Land");
    }

    public void PlayBossRising()
    {
        anim.SetTrigger("Rise");
    }

    public void PlayBossSpecial()
    {
        anim.SetTrigger("Special");
    }

    public void PlayWalk(bool isWalking)
    {
        anim.SetBool("Walking", isWalking);
    }
}
