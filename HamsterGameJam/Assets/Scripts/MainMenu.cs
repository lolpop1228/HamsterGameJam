using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneToPlay;
    public GameObject controlsUI;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        controlsUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneToPlay);
    }

    public void ControlsUI()
    {
        controlsUI.SetActive(true);
    }

    public void CloseUI()
    {
        controlsUI.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
