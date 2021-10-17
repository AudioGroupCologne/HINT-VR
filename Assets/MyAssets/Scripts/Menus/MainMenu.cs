using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] MenuManager menu;

    public void StartDemoGame ()
    {
        SceneManager.LoadSceneAsync("DemoScene");
        Debug.Log("lock cursor");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartTrainingGame()
    {
        // go to userseleciton
        menu.ShowUserSelection();

        //SceneManager.LoadSceneAsync("TrainingScene");
    }

    public void QuitApp ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
