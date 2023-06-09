using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public delegate void OnProgressButton();
    public OnProgressButton progressCallback = delegate { Debug.Log("No OnProgressButton delegate set!"); };

    // this class holds all options to be performed from the main menu
    // each public function is a 'OnClick' button callback

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartTrainingGame();
        }
        */
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))     // 2
        {
            StartTestScene();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartDemoScene();
        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            ShowPlayerProgress();
        }
        */

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))     
        {
            ChangeUser();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))  // 5
        {
            QuitApp();
        }
    }


    public void StartTrainingGame()
    {
        if(!UserManagement.selfReference.LoggedIn())
        {
            Debug.LogError("No user logged in");
            return;
        }

        SceneManager.LoadSceneAsync("TrainingScene");

    }

    public void StartTestScene()
    {
        if (!UserManagement.selfReference.LoggedIn())
        {
            Debug.LogError("No user logged in");
            return;
        }

        SceneManager.LoadSceneAsync("VRHINTScene");
    }

    public void ChangeUser()
    {
        SceneManager.LoadSceneAsync("VRLogin");
    }

    public void StartDemoScene()
    {
        if (!UserManagement.selfReference.LoggedIn())
        {
            Debug.LogError("No user logged in");
            return;
        }

        SceneManager.LoadSceneAsync("DemoAndCalibration");
    }


    public void ShowPlayerProgress()
    {
        if (!UserManagement.selfReference.LoggedIn())
        {
            Debug.LogError("No user logged in");
            return;
        }

        progressCallback();
    }

    public void QuitApp ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
