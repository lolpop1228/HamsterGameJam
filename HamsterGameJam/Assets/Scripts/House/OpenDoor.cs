using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenDoor : MonoBehaviour, IInteractable
{
    private AudioSource audioSource;
    public AudioClip openSound;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact() {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    private IEnumerator PlaySoundAndLoadScene() {
        audioSource.PlayOneShot(openSound);
        yield return new WaitForSeconds(openSound.length);
        SceneManager.LoadScene("Level1");
    }
}
