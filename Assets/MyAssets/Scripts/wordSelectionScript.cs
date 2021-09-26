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


    [SerializeField] Image[] btnIcons;
    [SerializeField] Button[] wordBtns;
    [SerializeField] Button continueBtn;
    [SerializeField] TrainingGameManager masterScript;

    [SerializeField] int wordOptions = 4;

    private string[] words;
    private Sprite[] icons;
    private int correctBtn;
    private bool selectionMade;

    public void startWordSelection(int wordGroup)
    {
        selectionMade = false;
        gameObject.SetActive(true);
        getWordOptions(wordGroup);
        mapWordsToUI();
    }

    void mapWordsToUI()
    {
        // get random poosition for correct Btn
        correctBtn = Random.Range(0, wordOptions - 1);

        // first assign correct word
        wordBtns[correctBtn].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[0];
        //wordBtns[correctBtn].GetComponentInChildren<Image>().sprite = icons[0];
        btnIcons[correctBtn].sprite = icons[0];

        int k = 0;

        // assign remaining words
        for (int i = 1; i < wordOptions; i++)
        {
            // skip the correct Button
            if (k == correctBtn)
            {
                k++;
            }
            wordBtns[k].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[i];
            //wordBtns[k].GetComponentInChildren<Image>().sprite = icons[k];
            btnIcons[k].sprite = icons[i];
            k++;
        }
    }


    void getWordOptions(int wordGroup)
    {
        Debug.Log("Get word options from master...");

        // alter the word selection: subject [1], count [3], object [5]
        words = masterScript.getUserWordSelection(wordGroup, wordOptions);
        Debug.Log(words[0] + words[1] + words[2] + words[3]);
        icons = masterScript.getUserIconSelection(wordGroup, words);
        Debug.Log(icons[0].ToString() + icons[1].ToString() + icons[2].ToString() + icons[3].ToString());
    }

    public void ButtonHander(int btn_ix)
    {
        if (selectionMade)
            return;

        DataStorage.TrainingGame_Total++;

        if (correctBtn == btn_ix)
        {
            DataStorage.TrainingGame_Hits++;
            masterScript.OnHit();
        }
        else
        {
            DataStorage.TrainingGame_Misses++;
            masterScript.OnMiss();
        }

        selectionMade = true;
        show_results_on_buttons();
        continueBtn.gameObject.SetActive(true);
    }

    public void UnsureButtonHanlder()
    {
        if (selectionMade)
            return;

        // TODO: Generate new sentence after 2 consecutive unsures, otherwise repeat with improved SNR
        DataStorage.TrainingGame_Unsure++;
        // show 'continue' button
        continueBtn.gameObject.SetActive(true);
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

    public void showWordSelectionUI(bool show)
    {
        continueBtn.gameObject.SetActive(false);
        gameObject.SetActive(show);
    }

}
