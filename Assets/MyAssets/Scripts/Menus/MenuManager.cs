using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Main;
    public GameObject Settings;
    public GameObject Results;
    public GameObject UserSelection;


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
            if(Settings.activeSelf == true)
            {
                Debug.Log("Go back from settings to Main");
                // Set MainMenu active
                Main.SetActive(true);
                // Set SettingsMenu inactive
                Settings.SetActive(false);
            }
        }
    }

    public void ShowSettingsMenu()
    {
        // Set SettingsMenu active
        Settings.SetActive(true);
        // Set MainMenu inactive
        Main.SetActive(false);
    }

    public void ShowMainMenu()
    {
        // Set MainMenu active
        Main.SetActive(true);
        // Set all other screens inactive
        Settings.SetActive(false);
        Results.SetActive(false);
        UserSelection.SetActive(false);

    }

    public void ShowResults()
    {
        UserSelection.SetActive(false);
        // Set ResutlsScreen active
        Results.SetActive(true);
        // SetMainMenu inactive
        Main.SetActive(false);
    }

    public void ShowUserSelection()
    {
        UserSelection.SetActive(true);
        Main.SetActive(false);
    }
}
