using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class customTabControl : MonoBehaviour
{

    [SerializeField] TMP_InputField[] inputFields;
    // Determines for which inputFields the 'onReturn' feature shall be activated
    // this has to be the same size as 'inputFields'!
    [SerializeField] bool[] enableOnReturn;
    [SerializeField] Button[] buttons;

    // this button can be used to trigger a callback when 'return' is pressed while a given inputField is selected
    [SerializeField] Button onReturn;



    private int selectedField = 0;
    private int options;
    private int btnOffset = 0;

    private void Awake()
    {

        options = inputFields.Length + buttons.Length;
        btnOffset = inputFields.Length;

        if(onReturn != null)
        {
            options++;
            btnOffset++;
        }

        if(inputFields.Length != enableOnReturn.Length)
        {
            Debug.LogError("Mismatch: enableOnReturn and inputFields!");
        }
        

        // short delay makes highlight on selection visible
        if(inputFields.Length >= 1)
        {
            StartCoroutine(InitialSelect());
        }

    }

    private IEnumerator InitialSelect()
    {
        yield return new WaitForSeconds(0.1f);
        inputFields[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (++selectedField >= options)
            {
                selectedField = 0;
            }
            SelectField();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {           
            if(selectedField < inputFields.Length)
            {
                if(enableOnReturn[selectedField])
                {
                    onReturn.onClick.Invoke();
                }
                
            }
            
        }
    }

    void SelectField()
    {
        if (selectedField < inputFields.Length)
        {
            inputFields[selectedField].Select();
        }
        else if(onReturn != null && selectedField < inputFields.Length + 1)
        {
            onReturn.Select();
        }
        else
        {
            buttons[selectedField - btnOffset].Select();
        }
    }
}
