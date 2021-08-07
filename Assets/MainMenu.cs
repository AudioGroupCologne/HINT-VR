using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartEnvironment ()
    {
        //Debug.Log("Go from Scene: " + SceneManager.GetActiveScene().buildIndex + " to " + (SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadSceneAsync("TestEnvironment1");
        // unload Menu Scene (what about stored settings?)
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartTrainingGame()
    {
        //Debug.Log("Go from Scene: " + SceneManager.GetActiveScene().buildIndex + " to " + (SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadSceneAsync("TrainingGame");
        // unload Menu Scene (what about stored settings?)
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartDemoGame()
    {
        //Debug.Log("Go from Scene: " + SceneManager.GetActiveScene().buildIndex + " to " + (SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadSceneAsync("DemonstrationGame");
        // unload Menu Scene (what about stored settings?)
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApp ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    // Settings can be done within Unity

}
