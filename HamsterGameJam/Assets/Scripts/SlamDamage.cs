using UnityEngine;

public class SlamDamage : MonoBehaviour
{
    public float minImpactForce = 5f;
    public float damage = 25f;
    public float selfDamage = 50f;
    public float stunDuration = 2f;  // How long the enemy is stunned
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject breakParticle;
    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip breakSound;
    public AudioClip impactSound;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minImpactForce)
        {
            if (collision.gameObject.TryGetComponent(out EnemyAI enemy))
            {
                enemy.TakeDamage(damage);  // Deal damage to the enemy
                enemy.Stun(stunDuration);  // Stun the enemy for the specified duration
                currentHealth -= selfDamage;
                Debug.Log("Enemy slammed and stunned!");
            }

            if (impactSound != null)
            {
                audioSource.PlayOneShot(impactSound);
            }
        }
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            if (breakParticle != null)
            {
                GameObject effect = Instantiate(breakParticle, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
            if (breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, transform.position, 5f);
            }

            Destroy(gameObject);
        }
    }
}
