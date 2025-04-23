using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPage : MonoBehaviour, IInteractable
{
    public GameObject lorePage;
    private AudioSource audioSource;
    public AudioClip flipSound;

    void Start()
    {
        if (lorePage != null)
        {
            lorePage.SetActive(false);
        }
        
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (lorePage != null)
        {
            lorePage.SetActive(true);
        }

        if (flipSound != null)
        {
            audioSource.PlayOneShot(flipSound);
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

            if (flipSound != null)
            {
                audioSource.PlayOneShot(flipSound);
            }
        }
    }
}
