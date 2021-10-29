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

    [SerializeField] GameObject wordSelectionUI;
    [SerializeField] Image[] btnIcons;
    [SerializeField] Button[] wordBtns;
    [SerializeField] Button unsureBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] TrainingGameManager masterScript;

    [SerializeField] int wordOptions = 4;
    [SerializeField] float nextDelay = 1.5f;

    // words to be written onto UI
    private string[] words;
    // icons to be showed on UI
    private Sprite[] icons;
    // index of the button, holding the correct option (randomized)
    private int correctBtn;
    // an option or 'unsure' was selected by the user
    private bool selectionMade;

    private void Start()
    {
        wordSelectionUI.SetActive(false);
    }

    public void startWordSelection(string[] randomWords, Sprite[] randomIcons)
    {
        selectionMade = false;
        wordSelectionUI.SetActive(true);
        words = randomWords;
        icons = randomIcons;
        mapWordsToUI();

        Debug.Log("Select first button");
        wordBtns[0].Select();
        wordBtns[0].OnSelect(null);
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

        if (correctBtn == btn_ix)
        {
            masterScript.OnHit();
            wordBtns[btn_ix].GetComponent<Image>().color = Color.green;
        }
        else
        {
            masterScript.OnMiss();
            wordBtns[btn_ix].GetComponent<Image>().color = Color.red;
        }

        selectionMade = true;
        StartCoroutine(showNextWait(nextDelay));


    }

    public void UnsureButtonHanlder()
    {
        if (selectionMade)
            return;

        masterScript.OnUnsure();

        // show 'continue' button
        continueBtn.gameObject.SetActive(true);

    }

    private IEnumerator showNextWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        showNextButton();
    }

    private void showNextButton()
    {

        for (int i = 0; i < wordBtns.Length; i++)
        {
            wordBtns[i].GetComponent<Image>().color = Color.white;
            wordBtns[i].gameObject.SetActive(false);
        }
        unsureBtn.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(true);
        continueBtn.Select();
    }

    public void showWordSelectionUI(bool show)
    {
        continueBtn.gameObject.SetActive(false);
        wordSelectionUI.SetActive(show);
        for (int i = 0; i < wordBtns.Length; i++)
        {
            wordBtns[i].gameObject.SetActive(true);
        }
        unsureBtn.gameObject.SetActive(true);

        wordBtns[0].Select();
    }

}
