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


    // Start is called before the first frame update
    void Start()
    {
        wordBtn1.GetComponentInChildren<Text>().text = "word1";
        wordBtn2.GetComponentInChildren<Text>().text = "word2";
        wordBtn3.GetComponentInChildren<Text>().text = "word3";
        wordBtn4.GetComponentInChildren<Text>().text = "word4";
        testfunc();
    }

    void testfunc()
    {
        Debug.Log("Try to get string");
        string[] currentSentence = masterScript.getSentenceString();
        Debug.Log(currentSentence[0] + currentSentence[1] + currentSentence[2]);
    }


    // get correct word(i) from DemoGameManager
    // get 3 other random words of group i from DemoGameManager
    // assign the 4 words to the buttons and change their UI.Text
    // Turn button 'green' when correct and 'red' when false
    // repeat until wordCount is reached


}
