using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideTutorial : MonoBehaviour
{
    public GameObject tutorialUI;

    void Start()
    {
        tutorialUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowUI());
        }
    }

    IEnumerator ShowUI()
    {
        tutorialUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        tutorialUI.SetActive(false);
    }
}
