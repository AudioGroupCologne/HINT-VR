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
     */


    [SerializeField] Image[] btnIcons;
    [SerializeField] Button[] wordBtns;
    [SerializeField] Button continueBtn;
    [SerializeField] TrainingGameManager masterScript;

    [SerializeField] int wordOptions = 4;

    // words to be written onto UI
    private string[] words;
    // icons to be showed on UI
    private Sprite[] icons;
    // index of the button, holding the correct option (randomized)
    private int correctBtn;
    // an option or 'unsure' was selected by the user
    private bool selectionMade;


    public void startWordSelection(string[] randomWords, Sprite[] randomIcons)
    {
        selectionMade = false;
        gameObject.SetActive(true);
        words = randomWords;
        icons = randomIcons;
        mapWordsToUI();
    }

    void mapWordsToUI()
    {
        // get random poosition for correct Btn
        correctBtn = Random.Range(0, wordOptions - 1);

        // first assign correct word
        wordBtns[correctBtn].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[0];
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
            btnIcons[k].sprite = icons[i];
            k++;
        }
    }

    public void ButtonHander(int btn_ix)
    {
        if (selectionMade)
            return;

        DataStorage.TrainingGame_Total++;

        if (correctBtn == btn_ix)
        {
            masterScript.OnHit();
        }
        else
        {
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

        DataStorage.TrainingGame_Unsure++;

        masterScript.OnUnsure();

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
