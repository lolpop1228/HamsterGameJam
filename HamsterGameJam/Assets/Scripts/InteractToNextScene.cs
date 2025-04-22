using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractToNextScene : MonoBehaviour, IInteractable
{
    public string sceneToLoad;

    public void Interact()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
