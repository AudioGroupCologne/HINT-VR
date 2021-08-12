using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordSelectionScript : MonoBehaviour
{

    public Button[] wordBtns;
    public DemoGameManager masterScript;

    private string[] words;
    private int correctBtn;


    // Start is called before the first frame update
    void OnEnable()
    {
        testfunc();
        correctBtn = Random.Range(0, 3);
        Debug.Log("correctIx: " + correctBtn);
        wordBtns[correctBtn].GetComponentInChildren<Text>().text = words[0];
        int k = 0;
        for(int i = 1; i < 4; i++)
        {
            // skip the correct Button
            if(k == correctBtn)
            {
                k++;
            }
            wordBtns[k].GetComponentInChildren<Text>().text = words[i];
            k++;
        }

    }

    void testfunc()
    {
        Debug.Log("Try to get string");
        string[] currentSentence = masterScript.getSentenceString();
        Debug.Log(currentSentence[0] + currentSentence[1] + currentSentence[2]);

        words = masterScript.getUserWordSelection(1, 4);
        Debug.Log("WordSelection: " + words[0] + words[1] + words[2] + words[3]);
    }

    public void Button0OnClick()
    {
        if(correctBtn == 0)
        {
            wordBtns[0].GetComponent<Image>().color = Color.green;
        }
        else
        {
            wordBtns[0].GetComponent<Image>().color = Color.red;
        }
    }

    public void Button1OnClick()
    {
        if (correctBtn == 1)
        {
            wordBtns[1].GetComponent<Image>().color = Color.green;
        }
        else
        {
            wordBtns[1].GetComponent<Image>().color = Color.red;
        }
    }

    public void Button2OnClick()
    {
        if (correctBtn == 2)
        {
            wordBtns[2].GetComponent<Image>().color = Color.green;
        }
        else
        {
            wordBtns[2].GetComponent<Image>().color = Color.red;
        }
    }

    public void Button3OnClick()
    {
        if (correctBtn == 3)
        {
            wordBtns[3].GetComponent<Image>().color = Color.green;
        }
        else
        {
            wordBtns[3].GetComponent<Image>().color = Color.red;
        }
    }


    // get correct word(i) from DemoGameManager
    // get 3 other random words of group i from DemoGameManager
    // assign the 4 words to the buttons and change their UI.Text
    // Turn button 'green' when correct and 'red' when false
    // repeat until wordCount is reached


}
