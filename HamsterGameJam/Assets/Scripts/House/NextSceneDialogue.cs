using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextSceneDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public float lineDelay = 1.5f;
    public PlayerMovement playerMovement;
    public GameObject oldDialogue;
    public GameObject newDialogue;
    public AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip openSound;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;

            if (typingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typingSound);
            }

            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(lineDelay);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("Level1");

            if (oldDialogue != null) {
                oldDialogue.SetActive(false);
            }
            
            if (newDialogue != null) {
                newDialogue.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
