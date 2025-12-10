using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StarfishEnemyAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.0f;


    private Animator anim;
    private Transform player;
    private float lastAttackTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
        }
    }
}
