using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputTab : MonoBehaviour
{

    [SerializeField] TMP_InputField username; // 0
    [SerializeField] TMP_InputField password; // 1
    [SerializeField] Button submit;
    [SerializeField] Button createUser;
    private int selectedField = 0;

    private void Awake()
    {
        username.Select();
        selectedField = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (++selectedField > 3)
            {
                selectedField = 0;
            }
            SelectField();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(selectedField)
            {
                case 0:
                    break;
                case 1:
                case 2:
                    submit.onClick.Invoke();
                    break;
                case 3:
                    createUser.onClick.Invoke();
                    break;
            }
            
        }

    }

    void SelectField()
    {
        switch(selectedField)
        {
            case 0:
                username.Select();
                break;
            case 1:
                password.Select();
                break;
            case 2:
                submit.Select();
                break;
            case 3:
                createUser.Select();
                break;
        }
    }

    // react to manual selection
    public void OnUsernameSelected()
    {
        selectedField = 0;
    }

    public void OnPasswordSelected()
    {
        selectedField = 1;
    }
}
