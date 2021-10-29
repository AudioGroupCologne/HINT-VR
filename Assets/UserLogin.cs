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
        login.SetActive(true);
        login.GetComponent<UserSelection>().loginCallback = loadMainMenu;
        creation.GetComponentInChildren<UserCreation>().GetComponent<UserCreation>().createCallback = loadMainMenu;
    }

    private void loadMainMenu(bool success)
    {
        if(success)
        {
            SceneManager.LoadSceneAsync("VRMenuScene");
        }
        
    }
}
