using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MiniBossAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float hoverAmplitude = 0.15f;
    public float hoverFrequency = 2f;
    public float stopDistance = 3f;

    [Header("Attack")]
    public float attackRange = 3f;
    public float attackCooldown = 2f;

    private Animator anim;
    private Transform player;
    private float lastAttackTime;

    [Header("Patrol Limit")]
    public float patrolRadius = 5f;
    private Vector3 homePosition;


    private void Start()
    {

        homePosition = transform.position;

        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Attack logic
        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            return; // stop movement during attack
        }

        // Fly movement when not attacking
        if (dist > stopDistance)
            FlyTowardsPlayer();
        else
            HoverInPlace();
    }

    void FlyTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        // Move only if within patrol area
        if (Vector2.Distance(transform.position, homePosition) < patrolRadius)
        {
            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
        }

        HoverInPlace();
        Flip(dir.x);
    }


    void HoverInPlace()
    {
        float hover = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x,
                                         transform.position.y + hover * Time.deltaTime,
                                         transform.position.z);
    }

    void Flip(float dirX)
    {
        if (dirX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dirX > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
}
