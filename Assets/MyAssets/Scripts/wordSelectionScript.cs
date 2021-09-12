using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordSelectionScript : MonoBehaviour
{

    /**
     * Get n words from a group including the correct one (index 0)
     * Map these words randomly to the presented buttons
     * Offer a callback for each button, in which it is checked if the selected word was 'hit' or a 'miss'
     * Give visual and auditorial feedback for each case (grn/red color, success/failure sound)
     * 
     * 
     */

    public Button[] wordBtns;
    public TrainingGameManager masterScript;

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
        Debug.Log("Get new sentence string");
        string[] currentSentence = masterScript.getCurrentSentence();
        Debug.Log(currentSentence[0] + currentSentence[1] + currentSentence[2]);

        // alter the word selection: subject [1], count [3], object [5]
        // but based on what?
        // also alter the text on the UI to 'select the subject from the last sentence' or something like that...
        words = masterScript.getUserWordSelection(1, 4);
        Debug.Log("WordSelection: " + words[0] + words[1] + words[2] + words[3]);
    }

    public void ButtonHander(int btn_ix)
    {
        DataStorage.DemoGameTotal++;

        Debug.Log("Btn: " + btn_ix);
        if (correctBtn == btn_ix)
        {
            // add correct
            Debug.Log("correct");
            DataStorage.DemoGameHits++;
            masterScript.OnHit();
        }
        else
        {
            // add miss
            Debug.Log("miss");
            DataStorage.DemoGameMisses++;
            masterScript.OnMiss();
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

}
