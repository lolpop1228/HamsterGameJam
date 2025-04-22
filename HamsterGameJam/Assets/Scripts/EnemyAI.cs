using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Default")]
    public float detectionRange = 20f;
    public float attackRange = 2f;
    public float moveSpeed = 4f;
    public float acceleration = 16f;
    public float angularSpeed = 720f;
    public float stoppingDistance = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Stun")]
    public bool isStunned = false;
    private float stunTimer = 0f;

    [Header("Audio")]
    public AudioClip chaseSound;
    private AudioSource audioSource;

    [Header("Unpredictable Movement")]
    public float strafeDistance = 2f;
    public float directionChangeInterval = 1.5f;
    private float nextDirectionChangeTime = 0f;
    private Vector3 offsetDirection = Vector3.zero;

    private Transform player;
    private NavMeshAgent agent;
    private float lastAttackTime;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

        if (agent)
        {
            agent.speed = moveSpeed;
            agent.acceleration = acceleration;
            agent.angularSpeed = angularSpeed;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = true;
            agent.updatePosition = true;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = chaseSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isDead || !player) return;

        if (isStunned)
        {
            HandleStun();
        }
        else
        {
            HandleMovement();
        }
    }

    void HandleStun()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (agent != null) agent.isStopped = true;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            isStunned = false;
            if (agent != null) agent.isStopped = false;
        }
    }

    void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (!audioSource.isPlaying && chaseSound != null)
            {
                audioSource.Play();
            }

            // Change strafe direction at intervals
            if (Time.time >= nextDirectionChangeTime)
            {
                Vector3 right = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized);
                offsetDirection = right * Random.Range(-strafeDistance, strafeDistance);
                nextDirectionChangeTime = Time.time + directionChangeInterval;
            }

            Vector3 targetPos = player.position + offsetDirection;
            agent.SetDestination(targetPos);

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
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (agent.hasPath)
            {
                agent.ResetPath();
            }
        }
    }

    public void Stun(float duration)
    {
        if (isDead) return;

        isStunned = true;
        stunTimer = duration;
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void AttackPlayer()
    {
        if (isDead) return;

        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;

        Destroy(gameObject, 0.5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (Application.isPlaying && player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up, player.position + Vector3.up);
        }
    }
}
