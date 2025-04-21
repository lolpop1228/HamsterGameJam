using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonToAnim : MonoBehaviour
{
    public Animator animator;
    public Animator buttonAnimator;
    public string animToPlay;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (buttonAnimator != null)
            {
                buttonAnimator.Play("Press");
            }
            if (animator != null)
            {
                animator.Play(animToPlay);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            if (animator != null && buttonAnimator != null)
            {
                buttonAnimator.Play("Release");
                animator.Play("Close");
            }
        }
    }
}
