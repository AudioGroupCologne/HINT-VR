using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputTab : MonoBehaviour
{

    [SerializeField] TMP_InputField username; // 0
    [SerializeField] TMP_InputField password; // 1
    [SerializeField] Button onPasswordEnteredBtn;
    [SerializeField] Button[] additionalBtns;
    private int selectedField = 0;
    private int staticOptions = 2;
    private int options;

    private void Awake()
    {
        options = staticOptions + additionalBtns.Length;
        Debug.Log("InputTab: Options: " + options);
        if(onPasswordEnteredBtn != null)
        {
            options++;
            staticOptions++;
            Debug.Log("InputTab: onPassword Btn set! " + options);
        }

        // short delay makes highlight on selection visible
        StartCoroutine(InitialSelect());
    }

    private IEnumerator InitialSelect( )
    {
        yield return new WaitForSeconds(0.1f);

        username.Select();
        Debug.Log("Select username field");
        selectedField = 0;
    }

// Update is called once per frame
void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab between fields");
            if (++selectedField >= options)
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
                    if(onPasswordEnteredBtn != null)
                    {
                        onPasswordEnteredBtn.onClick.Invoke();
                    }
                    break;
                default:
                    if(additionalBtns[selectedField - staticOptions] != null)
                    {
                        additionalBtns[selectedField - staticOptions].onClick.Invoke();
                    }                   
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
                if(onPasswordEnteredBtn != null)
                {
                    onPasswordEnteredBtn.Select();
                }
                else
                {
                    additionalBtns[selectedField - staticOptions].Select();
                }
                break;
            default:
                if(additionalBtns[selectedField - staticOptions] != null)
                {
                    additionalBtns[selectedField - staticOptions].Select();
                }
                
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
