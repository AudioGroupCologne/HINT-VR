using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordSelectionScript : MonoBehaviour
{

    public Button[] wordBtns;
    public DemoGameManager masterScript;
    public GameMangerScript gmScript;

    private string[] words;
    private int correctBtn;

    private int total = 0;
    private int hits = 0;
    private int misses = 0;


    // Start is called before the first frame update
    void OnEnable()
    {
        total = PlayerPrefs.GetInt("total");
        hits = PlayerPrefs.GetInt("hits");
        misses = PlayerPrefs.GetInt("misses");


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

        // alter the word selection: subject [1], count [3], object [5]
        // but based on what?
        // also alter the text on the UI to 'select the subject from the last sentence' or something like that...
        words = masterScript.getUserWordSelection(1, 4);
        Debug.Log("WordSelection: " + words[0] + words[1] + words[2] + words[3]);
    }

    public void ButtonHander(int btn_ix)
    {

        total++;
        PlayerPrefs.SetInt("total", total);

        Debug.Log("Btn: " + btn_ix);
        if (correctBtn == btn_ix)
        {
            // add correct
            Debug.Log("correct");
            hits++;
            PlayerPrefs.SetInt("hits", hits);
        }
        else
        {
            // add miss
            Debug.Log("miss");
            misses++;
            PlayerPrefs.SetInt("misses", misses);
        }
        show_results_on_buttons();
    }

    private void show_results_on_buttons()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == correctBtn)
            {
                wordBtns[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                wordBtns[i].GetComponent<Image>().color = Color.red;
            }

        }
    }

    public void reset_buttons_colors()
    {
        for(int i = 0; i < 4; i++)
        {
            wordBtns[i].GetComponent<Image>().color = Color.white;
        }
    }


    // get correct word(i) from DemoGameManager
    // get 3 other random words of group i from DemoGameManager
    // assign the 4 words to the buttons and change their UI.Text
    // Turn button 'green' when correct and 'red' when false
    // repeat until wordCount is reached


}
