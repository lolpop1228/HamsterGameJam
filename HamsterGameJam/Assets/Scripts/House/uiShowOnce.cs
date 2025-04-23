using UnityEngine;

public class uiShowOnce : MonoBehaviour
{
    public GameObject uiPanel;
    public float triggerDistance = 10f;
    private Transform player;
    private bool hasTriggered = false;

    void Start()
    {
        player = Camera.main.transform;
    }

    void Update()
    {
        if (!hasTriggered && Vector3.Distance(player.position, transform.position) <= triggerDistance)
        {
            uiPanel.SetActive(true);
            hasTriggered = true;
        }
    }
}
