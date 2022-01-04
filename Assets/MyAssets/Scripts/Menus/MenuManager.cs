using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Main;
    //public GameObject Settings;
    public GameObject Progress;
    public GameObject UserSelection;
    public GameObject UserCreation;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Main.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }

    
    public void ShowSettingsMenu()
    {
        // Set SettingsMenu active
        //Settings.SetActive(true);
        // Set MainMenu inactive
        Main.SetActive(false);
    }
    

    public void ShowMainMenu()
    {
        // Set MainMenu active
        Main.SetActive(true);

        // Set all other screens inactive
        //Settings.SetActive(false);
        Progress.SetActive(false);
        UserSelection.SetActive(false);
        UserCreation.SetActive(false);

    }

    public void ShowProgess()
    {
        UserSelection.SetActive(false);
        UserCreation.SetActive(false);
        Main.SetActive(false);

        Progress.SetActive(true);
        
    }

    public void ShowUserSelection()
    {
        Main.SetActive(false);
        UserSelection.SetActive(true);
    }

    public void ShowUserCreation()
    {
        UserSelection.SetActive(false);
        UserCreation.SetActive(true);   
    }
}
