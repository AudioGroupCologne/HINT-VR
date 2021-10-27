using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] MenuManager menu;
    /*
    // do this in an individual component (menuScroll)
    // get buttons via ComponentInChildern
    // fill List with buttons and cycle through entries!
    [SerializeField] Button demoBtn;
    [SerializeField] Button trainingBtn;
    [SerializeField] Button progressBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button quitBtn;

    private int selectedBtn = 0;

    void Start()
    {
        SelectButton();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (++selectedBtn > 4)
            {
                selectedBtn = 0;
            }
            SelectButton();

        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (--selectedBtn < 0)
            {
                selectedBtn = 4;
            }
            SelectButton();
                
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PushButton();
        }
    }

    private void SelectButton()
    {
        switch(selectedBtn)
        {
            case 0:
                demoBtn.Select();
                break;
            case 1:
                trainingBtn.Select();
                break;
            case 2:
                progressBtn.Select();
                break;
            case 3:
                settingsBtn.Select();
                break;
            case 4:
                quitBtn.Select();
                break;
        }
    }

    private void PushButton()
    {
        switch (selectedBtn)
        {
            case 0:
                demoBtn.onClick.Invoke();
                break;
            case 1:
                trainingBtn.onClick.Invoke();
                break;
            case 2:
                progressBtn.onClick.Invoke();
                break;
            case 3:
                settingsBtn.onClick.Invoke();
                break;
            case 4:
                quitBtn.onClick.Invoke();
                break;
        }
    }

    */
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
            // userCreation shall always use the same callback action as login
            menu.UserCreation.GetComponent<UserCreation>().createCallback = LoadTrainingGame;
        }
        else
        {
            LoadTrainingGame(true);
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
            menu.UserCreation.GetComponent<UserCreation>().createCallback = LoadPlayerProgress;
        }
        else
        {
            LoadPlayerProgress(true);
        }
    }

    public void LoadPlayerProgress(bool success)
    {
        if (success)
        {
            menu.ShowProgess();
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
