using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordSelectionScript : MonoBehaviour
{

    public Button wordBtn1;
    public Button wordBtn2;
    public Button wordBtn3;
    public Button wordBtn4;
    public DemoGameManager masterScript;

    private string[] words;


    // Start is called before the first frame update
    void OnEnable()
    {
        testfunc();
        wordBtn1.GetComponentInChildren<Text>().text = words[0];
        wordBtn2.GetComponentInChildren<Text>().text = words[1];
        wordBtn3.GetComponentInChildren<Text>().text = words[2];
        wordBtn4.GetComponentInChildren<Text>().text = words[3];   
    }

    void testfunc()
    {
        Debug.Log("Try to get string");
        string[] currentSentence = masterScript.getSentenceString();
        Debug.Log(currentSentence[0] + currentSentence[1] + currentSentence[2]);

        words = masterScript.getUserWordSelection(1, 4);
        Debug.Log("WordSelection: " + words[0] + words[1] + words[2] + words[3]);
    }


    // get correct word(i) from DemoGameManager
    // get 3 other random words of group i from DemoGameManager
    // assign the 4 words to the buttons and change their UI.Text
    // Turn button 'green' when correct and 'red' when false
    // repeat until wordCount is reached


}
