using UnityEngine;
using System.Collections;

public class DragonBossAI : MonoBehaviour
{
    public DragonAnimatorController animatorController;

    [Header("Player Detection")]
    public float detectionRange = 6f;
    private Transform player;

    [Header("Movement Speeds")]
    public float walkSpeed = 2f;
    public float flySpeed = 3f;
    public float flyBobAmount = 0.5f;

    [Header("Timing")]
    public float behaviorDuration = 3f;
    public float attackCooldown = 2f;

    [Header("Movement Boundaries (REAL VALUES)")]
    public float leftLimit = 91.38f;
    public float rightLimit = 140.67f;
    public float bottomLimit = -131.99f;
    public float topLimit = -100f; // Set ceiling higher if needed

    private bool isDead = false;
    private bool isFlying = false;
    private bool isWalking = false;
    private bool canAttack = true;

    private float bobTimer = 0f;

    private void Start()
    {
        animatorController = GetComponent<DragonAnimatorController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(BehaviorLoop());
    }

    private void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandlePlayerTracking();
    }

    // ---------------------------------------------------------------
    // PLAYER DETECTION
    // ---------------------------------------------------------------
    private void HandlePlayerTracking()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && canAttack)
        {
            StartCoroutine(DoAttack());
        }
    }

    // ---------------------------------------------------------------
    // RANDOM BEHAVIOR LOOP
    // ---------------------------------------------------------------
    private IEnumerator BehaviorLoop()
    {
        while (!isDead)
        {
            if (!canAttack)
            {
                yield return null;
                continue;
            }

            int choice = Random.Range(0, 3);

            if (choice == 0)
                StartCoroutine(DoIdle());
            else if (choice == 1)
                StartCoroutine(DoWalk());
            else if (choice == 2)
                StartCoroutine(DoFly());

            yield return new WaitForSeconds(behaviorDuration);
        }
    }

    // ---------------------------------------------------------------
    // BEHAVIORS
    // ---------------------------------------------------------------
    private IEnumerator DoIdle()
    {
        isWalking = false;
        isFlying = false;

        animatorController.PlayWalk(false);
        animatorController.PlayFlying(false);

        yield break;
    }

    private IEnumerator DoWalk()
    {
        isFlying = false;
        isWalking = true;

        animatorController.PlayFlying(false);
        animatorController.PlayWalk(true);

        yield break;
    }

    private IEnumerator DoFly()
    {
        isWalking = false;
        isFlying = true;

        animatorController.PlayWalk(false);
        animatorController.PlayFlying(true);

        yield break;
    }

    private IEnumerator DoAttack()
    {
        canAttack = false;
        isWalking = false;
        isFlying = false;

        animatorController.PlayWalk(false);
        animatorController.PlayFlying(false);

        FlipTowardPlayer();
        animatorController.PlayAttack();

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    // ---------------------------------------------------------------
    // MOVEMENT HANDLING (WALK + FLY + BOUNDARIES)
    // ---------------------------------------------------------------
    private void HandleMovement()
    {
        float facing = Mathf.Sign(transform.localScale.x);

        // FLYING
        if (isFlying)
        {
            bobTimer += Time.deltaTime;

            transform.position += new Vector3(
                facing * flySpeed * Time.deltaTime,
                Mathf.Sin(bobTimer * 3f) * flyBobAmount * Time.deltaTime,
                0f
            );
        }

        // WALKING
        if (isWalking)
        {
            transform.position += new Vector3(
                facing * walkSpeed * Time.deltaTime,
                0f,
                0f
            );
        }

        // BOUNDARY CLAMP
        float clampedX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        // AUTO-FLIP AT WALLS
        if (transform.position.x <= leftLimit)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (transform.position.x >= rightLimit)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    // ---------------------------------------------------------------
    // FACE PLAYER
    // ---------------------------------------------------------------
    private void FlipTowardPlayer()
    {
        if (player == null) return;

        float newDir = (player.position.x > transform.position.x) ? 1 : -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * newDir;
        transform.localScale = scale;
    }

    // ---------------------------------------------------------------
    // DEATH
    // ---------------------------------------------------------------
    public void KillBoss()
    {
        if (isDead) return;

        isDead = true;
        StopAllCoroutines();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        animatorController.PlayDeath();

        // TODO: set this to your death animation's length
        float deathAnimLength = 2.0f;
        yield return new WaitForSeconds(deathAnimLength);

        if (VictoryManager.Instance != null)
            VictoryManager.Instance.ShowVictoryScreen();
    }
}
