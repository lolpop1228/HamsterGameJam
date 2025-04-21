using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform[] patrolPoints;
    public Transform player;
    public Transform eyePoint;

    [Header("Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float lineOfSightLength = 20f;
    public LayerMask playerLayer;
    public LayerMask obstacleMask;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public float attackDamage = 20f;

    private int currentPatrolIndex = 0;
    private Rigidbody rb;
    private bool playerDetected = false;
    private float lastAttackTime;
    private bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (PlayerInSight() && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
        }

        if (playerDetected)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector3 target = patrolPoints[currentPatrolIndex].position;
        MoveTowards(target, patrolSpeed);

        if (Vector3.Distance(transform.position, target) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Stop moving when in attack range
            isAttacking = true;
            rb.velocity = Vector3.zero;

            if (Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            isAttacking = false;
            MoveTowards(player.position, chaseSpeed);
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        if (isAttacking) return; // Prevent movement while attacking

        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log("Attack");
        }
    }

    private bool PlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - eyePoint.position).normalized;

        if (Physics.Raycast(eyePoint.position, directionToPlayer, out RaycastHit hit, lineOfSightLength, ~obstacleMask))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (eyePoint != null && player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(eyePoint.position, player.position);
        }
    }
}
