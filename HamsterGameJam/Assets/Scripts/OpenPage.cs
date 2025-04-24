using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPage : MonoBehaviour, IInteractable
{
    public GameObject lorePage;
    private AudioSource audioSource;
    public AudioClip flipSound;
    private bool isOpen = false;
    private bool hasScored = false; // Track if score was already given

    void Start()
    {
        if (lorePage != null)
        {
            lorePage.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on " + gameObject.name);
        }
    }

    public void Interact()
    {
        if (lorePage != null && !isOpen)
        {
            lorePage.SetActive(true);
            isOpen = true;

            if (flipSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(flipSound);
            }

            if (!hasScored)
            {
                ScoreManager.Instance.AddScore(1); // Call your scoring method
                hasScored = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) && isOpen)
        {
            if (lorePage != null)
            {
                lorePage.SetActive(false);
                isOpen = false;

                if (flipSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(flipSound);
                }
            }
        }
    }
}
