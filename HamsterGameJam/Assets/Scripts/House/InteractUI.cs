using UnityEngine;

public class InteractUI : MonoBehaviour, IInteractable
{
    public GameObject uiPanel;
    private bool hasTriggered = false;

    public void Interact()
    {
        if (!hasTriggered)
        {
            uiPanel.SetActive(true);
            hasTriggered = true;
        }
    }
}
