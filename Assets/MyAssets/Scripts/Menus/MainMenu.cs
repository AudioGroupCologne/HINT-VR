using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] MenuManager menu;


    // this class holds all options to be performed from the main menu
    // each public function is a 'OnClick' button callback
    public void StartDemoGame ()
    {
        SceneManager.LoadSceneAsync("DemoScene");
        Debug.Log("lock cursor");
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartTrainingGame()
    {
        if(!UserManagement.selfReference.LoggedIn())
        {
            menu.ShowUserSelection();
            menu.UserSelection.GetComponent<UserSelection>().loginCallback = LoadTrainingGame;
        }
    }

    public void LoadTrainingGame(bool success)
    {
        Debug.Log("Enter callback");
        if(success)
        {
            SceneManager.LoadSceneAsync("TrainingScene");
            return;
        }

        Debug.Log("Login Failed. Dont load Scene...");
    }

    public void ShowPlayerProgress()
    {
        if(!UserManagement.selfReference.LoggedIn())
        {
            menu.ShowUserSelection();
            menu.UserSelection.GetComponent<UserSelection>().loginCallback = LoadPlayerProgress;
        }
    }

    public void LoadPlayerProgress(bool success)
    {
        if (success)
        {
            menu.ShowResults();
            return;
        }

        Debug.Log("Login Failed. Dont show player progress...");
    }

    public void QuitApp ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
