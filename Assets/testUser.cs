using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testUser : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] GameObject testUserFailedText;

    public delegate void OnTestUserEvent();
    public OnTestUserEvent testUserCallback = delegate { Debug.Log("No create delegate set!"); };

    public delegate void OnReturnEvent();
    public OnReturnEvent returnCallback = delegate { Debug.Log("No return delegate set!"); };

    public void OnAddTestUser()
    {
        if (username.text.Length == 0)
        {
            Debug.Log("Both username and password have to be set.");
            return;
        }

        /*
        // check if username already exists
        if (!UserManagement.selfReference.addTestUser(username.text))
        {
            Debug.Log("Username already exists");
            testUserFailedText.SetActive(true);
            return;
        }
        */
        
        testUserFailedText.SetActive(false);
        testUserCallback();

    }

    public void OnReturn()
    {
        testUserFailedText.SetActive(false);
        returnCallback();
    }
}

