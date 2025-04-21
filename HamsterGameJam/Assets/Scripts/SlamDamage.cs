using UnityEngine;

public class SlamDamage : MonoBehaviour
{
    public float minImpactForce = 5f;
    public float damage = 25f;
    public float stunDuration = 2f;  // How long the enemy is stunned

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minImpactForce)
        {
            if (collision.gameObject.TryGetComponent(out EnemyAI enemy))
            {
                enemy.TakeDamage(damage);  // Deal damage to the enemy
                enemy.Stun(stunDuration);  // Stun the enemy for the specified duration
                Debug.Log("Enemy slammed and stunned!");
            }
        }
    }
}
