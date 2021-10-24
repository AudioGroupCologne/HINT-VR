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
            if (++selectedField > 1)
            {
                selectedField = 0;
            }
            SelectField();
        }

        if(selectedField == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            submit.onClick.Invoke();
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
