using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserLogin : MonoBehaviour
{
    [SerializeField] GameObject login;
    [SerializeField] GameObject creation;
    void Start()
    {
        creation.SetActive(false);
        login.SetActive(true);
        login.GetComponent<UserSelection>().loginCallback = loadMainMenu;
        login.GetComponent<UserSelection>().newUserCallback = showCreationScreen;
        creation.GetComponent<UserCreation>().createCallback = loadMainMenu;
        creation.GetComponent<UserCreation>().returnCallback = showLoginScreen;
        //creation.GetComponentInChildren<UserCreation>().GetComponent<UserCreation>().createCallback = loadMainMenu;
        //creation.GetComponentInChildren<UserCreation>().GetComponent<UserCreation>().returnCallback = showLoginScreen;

    }

    private void loadMainMenu(bool success)
    {
        if(success)
        {
            SceneManager.LoadSceneAsync("VRMenuScene");
        }
        
    }

    private void showLoginScreen()
    {
        creation.SetActive(false);
        login.SetActive(true);
    }

    private void showCreationScreen()
    {
        creation.SetActive(true);
        login.SetActive(false);
    }
}
