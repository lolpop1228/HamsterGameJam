using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class uiTriggerOnce : MonoBehaviour
{
    public GameObject uiPanel;
    public float triggerDistance = 10f;
    private Transform player;
    private AudioSource audioSource;
    public AudioClip soundEffect;
    public PlayableDirector playableDirector;
    private bool hasTriggered = false;

    void Start()
    {
        player = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!hasTriggered && Vector3.Distance(player.position, transform.position) <= triggerDistance)
        {
            StartCoroutine(PlaySound());
            hasTriggered = true;
        }
    }

    private IEnumerator PlaySound() 
    {
        audioSource.PlayOneShot(soundEffect);
        yield return new WaitForSeconds(soundEffect.length);
        uiPanel.SetActive(true);

        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }
}
