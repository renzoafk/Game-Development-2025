using UnityEngine;

public class MiniBossAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float patrolDistance = 4f;
    public float moveSpeed = 2f;
    public float hoverAmount = 0.2f;
    public float hoverSpeed = 2f;

    [Header("Player Detection")]
    public float aggroRange = 6f;   // Boss chases player inside this radius

    [Header("Attack Settings")]
    public float attackRange = 2.5f;
    public float attackCooldown = 2f;

    private float leftBound;
    private float rightBound;
    private bool movingRight = true;
    private float baseY;

    private float lastAttackTime;
    private Transform player;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        baseY = transform.position.y;

        leftBound = transform.position.x - patrolDistance;
        rightBound = transform.position.x + patrolDistance;
    }

    void Update()
    {
        HoverMovement();

        if (player == null)
        {
            Patrol();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= aggroRange)
            ChasePlayer();
        else
            Patrol();

        TryAttack(distance);
    }

    // --------------------------------------

    private void HoverMovement()
    {
        float newY = baseY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void Patrol()
    {
        float x = transform.position.x;

        if (movingRight)
        {
            x += moveSpeed * Time.deltaTime;
            if (x >= rightBound) movingRight = false;
        }
        else
        {
            x -= moveSpeed * Time.deltaTime;
            if (x <= leftBound) movingRight = true;
        }

        // Clamp so patrol never escapes
        x = Mathf.Clamp(x, leftBound, rightBound);

        transform.position = new Vector3(x, transform.position.y, transform.position.z);

        transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
    }

    // --------------------------------------

    private void ChasePlayer()
    {
        float targetX = player.position.x;
        float currentX = transform.position.x;

        float direction = Mathf.Sign(targetX - currentX);
        float newX = currentX + direction * moveSpeed * Time.deltaTime;

        // Clamp inside patrol zone so boss stays on map
        newX = Mathf.Clamp(newX, leftBound, rightBound);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        transform.localScale = new Vector3(direction > 0 ? 1 : -1, 1, 1);
    }

    // --------------------------------------

    private void TryAttack(float distance)
    {
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            if (animator != null)
                animator.SetTrigger("Attack");
        }
    }
}
