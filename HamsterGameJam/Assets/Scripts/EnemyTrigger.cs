using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject enemy;

    void Start()
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
    }
}
