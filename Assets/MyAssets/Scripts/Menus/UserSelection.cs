using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserSelection : MonoBehaviour
{

    public delegate void OnLoginEvent(bool success);
    public OnLoginEvent loginCallback = delegate { Debug.Log("No Login delegate set!"); };

    public delegate void OnNewUserEvent();
    public OnNewUserEvent newUserCallback = delegate { Debug.Log("No newUser delegate set!"); };

    UserManagement userManager;
    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] TMPro.TMP_InputField password;
    [SerializeField] GameObject loginFailedText;

    private void Start()
    {
        userManager = UserManagement.selfReference;
        if(userManager == null)
        {
            Debug.LogError("userManager not found");
        }
    }

    public void OnSubmit()
    {

        if(userManager.userLogin(username.text, password.text))
        {
            OnLoginSuccess();
            return;
        }

        OnLoginFailed();

    }

    public void OnCreateUser()
    {
        loginFailedText.SetActive(false);
        newUserCallback();
    }

    // Delegate wrapper
    void OnLoginSuccess()
    {
        Debug.Log("Login successful");
        loginCallback(true);
    }

    void OnLoginFailed()
    {
        Debug.Log("Login failed");
        loginCallback(false);
        loginFailedText.SetActive(true);
    }

}
