using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCreation : MonoBehaviour
{

    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] TMPro.TMP_InputField password;
    [SerializeField] GameObject userCreationFailedText;

    public delegate void OnCreateEvent(bool success);
    public OnCreateEvent createCallback = delegate { Debug.Log("No create delegate set!"); };

    public delegate void OnReturnEvent();
    public OnReturnEvent returnCallback = delegate { Debug.Log("No return delegate set!"); };

    public void OnCreateUser(int _group)
    {
        if (username.text.Length == 0 || password.text.Length == 0)
        {
            Debug.Log("Both username and password have to be set.");
            return;
        }

        // check if username already exists
        if (!UserManagement.selfReference.addUser(username.text, password.text, _group))
        {
            Debug.Log("Username already exists");
            userCreationFailedText.SetActive(true);
            createCallback(false);
            return;
        }

        userCreationFailedText.SetActive(false);
        createCallback(true);

    }

    public void OnReturn()
    {
        userCreationFailedText.SetActive(false);
        returnCallback();
    }
}
