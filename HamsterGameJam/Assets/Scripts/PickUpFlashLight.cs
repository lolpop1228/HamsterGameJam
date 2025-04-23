using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlashLight : MonoBehaviour, IInteractable
{
    public GameObject flashLightObj;
    public GameObject playerFlashlight;
    public GameObject flashLightUI;
    public GameObject flashLightUI2;

    void Start()
    {
        if (playerFlashlight != null && flashLightUI != null)
        {
            playerFlashlight.SetActive(false);
            flashLightUI.SetActive(false);
            flashLightUI2.SetActive(false);
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
        StartCoroutine(TutorialUI());
    }

    IEnumerator TutorialUI()
    {
        flashLightUI2.SetActive(true);
        yield return new WaitForSeconds(5f);
        flashLightUI2.SetActive(false);
    }
}
