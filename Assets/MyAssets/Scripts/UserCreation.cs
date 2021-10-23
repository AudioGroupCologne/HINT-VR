using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCreation : MonoBehaviour
{

    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] TMPro.TMP_InputField password;

    public delegate void OnCreateEvent(bool success);
    public OnCreateEvent createCallback = delegate { Debug.Log("No create delegate set!"); };

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
            Debug.LogError("Username already exists");
            createCallback(false);
            return;
        }

        createCallback(true);

    }
}
