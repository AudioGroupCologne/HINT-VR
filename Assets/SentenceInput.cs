using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SentenceInput : MonoBehaviour
{

    public delegate void OnInput(string text);
    public OnInput onInputCallback = delegate { Debug.Log("No onInput delegate set!"); };

    [SerializeField] TMP_InputField input;

    private bool active = false;

    void Update()
    {
        if(active && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Submitting: " + input.text);
            onInputCallback(input.text);
        }

        if (active && Input.GetKeyDown(KeyCode.Tab))
        {
            input.Select();
        }
    }


    public void ShowSentenceInput(bool show)
    {
        active = show;
        gameObject.SetActive(show);

        if(active)
        {
            input.Select();
        }
    }

}
