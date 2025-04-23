using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDisplayText : MonoBehaviour, IInteractable
{
    public GameObject textUI;

    public void Interact()
    {
        StartCoroutine(TextDisplay());
    }

    IEnumerator TextDisplay()
    {
        textUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        textUI.SetActive(false);
    }
}
