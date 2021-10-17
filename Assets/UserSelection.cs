using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserSelection : MonoBehaviour
{

    [SerializeField] UserManagement userManager;
    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] TMPro.TMP_InputField password;

    public void OnSubmit()
    {
        Debug.Log("Username: " + username.text);
        Debug.Log("Password: " + password.text);

        if(userManager.CheckPassword(username.text, password.text))
        {
            OnLoginSuccess();
            return;
        }

        OnLoginFailed();

    }

    public void OnCreateUser()
    {
        // check if username already exists
        if(!userManager.AddUser(username.text, password.text))
        {
            Debug.LogError("Username already exists");
            return;
        }

        OnLoginSuccess();
    }


    // Move these via Interface to a different file/class (?)
    void OnLoginSuccess()
    {
        SceneManager.LoadSceneAsync("TrainingScene");
    }

    void OnLoginFailed()
    {
        Debug.Log("Login failed. Submit data again or create a new user");
    }

}
