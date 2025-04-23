using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPage : MonoBehaviour, IInteractable
{
    public GameObject lorePage;

    void Start()
    {
        if (lorePage != null)
        {
            lorePage.SetActive(false);
        }
    }

    public void Interact()
    {
        if (lorePage != null)
        {
            lorePage.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (lorePage != null)
            {
                lorePage.SetActive(false);
            }
        }
    }
}
