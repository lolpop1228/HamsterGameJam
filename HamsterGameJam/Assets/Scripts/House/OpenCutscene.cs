using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class OpenCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera targetCamera;
    public Vector3 targetRotation = new Vector3(0f, 0f, 0f);
    public GameObject objectToEnable1;
    public GameObject objectToEnable2;
    public GameObject objectToEnable3;
    public GameObject objectToEnable4;
    public float delayBeforeActivate = 1f;
    public float fadeDuration = 1f;

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
            playableDirector.stopped += OnTimelineEnd;
        }

        if (objectToEnable1 != null)
        {
            objectToEnable1.SetActive(false);
        }

        if (objectToEnable2 != null)
        {
            objectToEnable2.SetActive(false);
        }

        if (objectToEnable3 != null)
        {
            objectToEnable3.SetActive(false);
        }

        if (objectToEnable4 != null)
        {
            objectToEnable4.SetActive(false);
        }
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        if (targetCamera != null)
        {
            targetCamera.transform.rotation = Quaternion.Euler(targetRotation);
        }

        if (objectToEnable1 != null)
        {
            StartCoroutine(ActivateAndFadeIn(objectToEnable1, delayBeforeActivate, fadeDuration));
        }

        if (objectToEnable2 != null)
        {
            StartCoroutine(ActivateAndFadeIn(objectToEnable2, delayBeforeActivate, fadeDuration));
        }

        if (objectToEnable3 != null)
        {
            objectToEnable3.SetActive(true);
        }

        if (objectToEnable4 != null)
        {
            objectToEnable4.SetActive(false);
        }

        playableDirector.stopped -= OnTimelineEnd;
    }

    private IEnumerator ActivateAndFadeIn(GameObject uiObject, float delayBeforeActivate, float fadeDuration)
    {
        yield return new WaitForSeconds(delayBeforeActivate);

        uiObject.SetActive(true);

        yield return StartCoroutine(FadeInUI(uiObject, fadeDuration));
    }

    private IEnumerator FadeInUI(GameObject uiObject, float duration)
    {
        CanvasGroup canvasGroup = uiObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
