using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputTab : MonoBehaviour
{

    [SerializeField] TMP_InputField username; // 0
    [SerializeField] TMP_InputField password; // 1
    private int selectedField = 0;

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
