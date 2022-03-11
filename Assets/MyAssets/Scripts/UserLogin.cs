using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserLogin : MonoBehaviour
{
    [SerializeField] GameObject login;
    [SerializeField] GameObject creation;
    [SerializeField] GameObject testUser;
    void Start()
    {
        // make sure only one screen object is active
        login.SetActive(true);
        creation.SetActive(false);
        testUser.SetActive(false);

        // set delegates
        login.GetComponent<UserSelection>().loginCallback = loadMainMenu;
        login.GetComponent<UserSelection>().newUserCallback = showCreationScreen;
        login.GetComponent<UserSelection>().testUserCallback = showTestUserScreen;

        creation.GetComponent<UserCreation>().createCallback = loadMainMenu;
        creation.GetComponent<UserCreation>().returnCallback = showLoginScreen;

        testUser.GetComponent<testUser>().testUserCallback = loadVRHINT;
        testUser.GetComponent<testUser>().returnCallback = showLoginScreen;

    }

    private void loadMainMenu(bool success)
    {
        if(success)
        {
            SceneManager.LoadSceneAsync("VRMenuScene");
        }
    }

    private void loadVRHINT()
    {
        SceneManager.LoadSceneAsync("VRHINTScene");
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

    private void showTestUserScreen()
    {
        testUser.SetActive(true);
        login.SetActive(false);
    }
}
