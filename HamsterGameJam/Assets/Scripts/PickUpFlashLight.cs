using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlashLight : MonoBehaviour, IInteractable
{
    public GameObject flashLightObj;
    public GameObject playerFlashlight;
    public GameObject flashLightUI;

    void Start()
    {
        if (playerFlashlight != null && flashLightUI != null)
        {
            playerFlashlight.SetActive(false);
            flashLightUI.SetActive(false);
        }
    }

    public void Interact()
    {
        if (flashLightObj != null && playerFlashlight != null && flashLightUI != null)
        {
            flashLightObj.SetActive(false);
            playerFlashlight.SetActive(true);
            flashLightUI.SetActive(true);
        }
    }
}
