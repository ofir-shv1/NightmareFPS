using UnityEngine;

// Simple enemy AI for the Mutant character.
// Behavior:
//   - Player far away  -> stay Idle
//   - Player in chase range -> walk toward player (Walking animation)
//   - Player in attack range -> stop and attack on a cooldown (Mutant Swiping animation)
public class MutantAI : MonoBehaviour
{
    [Header("Target")]
    // The player to chase. Auto-found by "Player" tag if left empty.
    public Transform player;

    [Header("Ranges")]
    public float attackRange = 2f;   // How close the player must be to attack (Mutant always chases otherwise)

    [Header("Movement")]
    public float moveSpeed = 2.5f;        // Walking speed (slower than the player)
    public float rotationSpeed = 5f;      // How fast the Mutant turns toward the player

    [Header("Attack")]
    public int attackDamage = 10;         // HP removed from the player per hit
    public float attackCooldown = 1.5f;   // Seconds between attacks
    public float damageDelay = 1.3f;      // Wait this long after the swipe starts before dealing damage (matches the strike moment in the animation)

    private Animator animator;
    private float lastAttackTime = -999f; // Set far in the past so the first attack isn't blocked by cooldown

    void Start()
    {
        // Cache the Animator (sits on the same GameObject as this script)
        animator = GetComponent<Animator>();

        // If no player was assigned in the Inspector, find one by tag
        if (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) player = playerGO.transform;
        }
    }

    void Update()
    {
        // Safety - no player, do nothing
        if (player == null) return;

        // Don't act if the game is over (Win or Lose)
        if (GameManager.Instance != null &&
            (GameManager.Instance.isWin || GameManager.Instance.currentHP <= 0))
        {
            animator.SetBool("isChasing", false);
            return;
        }

        // While the attack animation is playing, stand still and face the player.
        // Without this the Mutant would slide forward without a walking animation
        // during the brief window after the swipe ends.
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Mutant Swiping"))
        {
            animator.SetBool("isChasing", false);
            FacePlayer();
            return;
        }

        // How far is the player?
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Player is close enough to hit - stop and attack
            animator.SetBool("isChasing", false);
            FacePlayer();
            TryAttack();
        }
        else
        {
            // Otherwise - always chase the player (Idle plays only at game start before Update runs)
            animator.SetBool("isChasing", true);
            FacePlayer();
            MoveTowardPlayer();
        }
    }

    // Rotates the Mutant smoothly to face the player on the horizontal plane
    private void FacePlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // Ignore vertical - we don't want the Mutant to tilt up/down
        if (lookDirection.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Moves the Mutant toward the player on the horizontal plane
    private void MoveTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Stay on the ground - no flying
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    // Plays the attack animation and schedules the damage to land mid-swing
    private void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("attack");

        // Wait until the swipe animation has visually connected before applying damage
        Invoke(nameof(ApplyAttackDamage), damageDelay);
    }

    // Called via Invoke after the damage delay. Damage lands only if the player is still
    // close enough - small movement won't dodge, but actually running away will.
    private void ApplyAttackDamage()
    {
        if (player == null || GameManager.Instance == null) return;
        if (GameManager.Instance.isWin || GameManager.Instance.currentHP <= 0) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange + 1f)
        {
            GameManager.Instance.TakeDamage(attackDamage);
        }
    }
}
