using UnityEngine;

public class HealingItem : MonoBehaviour, IInteractable
{
    public float healAmount = 25f;

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
