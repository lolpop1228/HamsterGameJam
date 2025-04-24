using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenDoor : MonoBehaviour, IInteractable
{
    public GameObject uiPanel;
    private bool hasTriggered = false;

    public void Interact() {
        
        if (!hasTriggered) {
            uiPanel.SetActive(true);
        }
        
        hasTriggered = true;
    }
}
