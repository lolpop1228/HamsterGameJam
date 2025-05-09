using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonToAnim : MonoBehaviour
{
    public Animator animator;
    public Animator buttonAnimator;
    public string animToPlay;
    private AudioSource audioSource;
    public AudioClip openSound;

     void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (buttonAnimator != null)
            {
                buttonAnimator.Play("Press");
            }
            if (animator != null && openSound != null)
            {
                animator.Play(animToPlay);
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (animator != null && buttonAnimator != null && openSound != null)
            {
                buttonAnimator.Play("Release");
                animator.Play("Close");
                audioSource.PlayOneShot(openSound);
            }
        }
    }
}
