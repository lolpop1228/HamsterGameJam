using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Default")]
    public float detectionRange = 20f;
    public float attackRange = 2f;
    public float moveSpeed = 4f;
    public float attackCooldown = 1.5f;
    public int damage = 10;
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    [Header("Stun")]
    public bool isStunned = false;
    private float stunTimer = 0f;

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!player) return;

        if (isStunned)
        {
            // If the enemy is stunned, stop all movement and actions
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }
            return; // Skip the rest of the update if stunned
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            if (distance <= attackRange)
            {
                agent.ResetPath();

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        agent.SetDestination(transform.position);
        // Optionally play a stun animation or sound here
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        // You can add an animation trigger here
        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
