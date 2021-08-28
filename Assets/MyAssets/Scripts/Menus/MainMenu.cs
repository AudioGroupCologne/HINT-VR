using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartEnvironment ()
    {
        SceneManager.LoadSceneAsync("BrightDay");
        Debug.Log("lock cursor");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartTrainingGame()
    {
        SceneManager.LoadSceneAsync("TrainingGame");
    }

    public void StartDemoGame()
    {
        SceneManager.LoadSceneAsync("DemonstrationGame");
    }

    public void StartVRRoom()
    {
        SceneManager.LoadSceneAsync("VRRoom");
    }

    public void QuitApp ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
