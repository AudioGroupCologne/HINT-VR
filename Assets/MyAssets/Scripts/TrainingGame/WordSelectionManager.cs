using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordSelectionManager : MonoBehaviour
{

    /**
     * Get n words from a group including the correct one (index 0)
     * Map these words randomly to the presented buttons
     * Offer a callback for each button, in which it is checked if the selected word was 'hit' or a 'miss'
     * Give visual and auditorial feedback for each case (grn/red color, success/failure sound)
     */

    [SerializeField] GameObject wordSelectionUI;
    [SerializeField] Button[] wordBtns;
    [SerializeField] Button unsureBtn;
    [SerializeField] Button continueBtn;

    [SerializeField] float nextDelay = 1.5f;

    public delegate void OnHit();
    public OnHit onHitCallback = delegate { Debug.Log("No onHit delegate set!"); };
    public delegate void OnMiss();
    public OnHit onMissCallback = delegate { Debug.Log("No onMiss delegate set!"); };
    public delegate void OnUnsure();
    public OnHit onUnsureCallback = delegate { Debug.Log("No onUnsure delegate set!"); };
    public delegate void OnContinue();
    public OnHit onContinueCallback = delegate { Debug.Log("No onContinue delegate set!"); };

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

    // keyboard controls
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ButtonHander(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            ButtonHander(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ButtonHander(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            ButtonHander(3);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnsureButtonHanlder();
        }
        if (continueBtn.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ContinueButtonHandler();
        }
    }

    private void mapWordsToUI(int num_words)
    {
        // get random position for correct Btn
        correctBtn = Random.Range(0, num_words - 1);

        // first assign correct word
        wordBtns[correctBtn].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[0];
        // button with icon is a prefab, therefore the child index (1) is known!
        wordBtns[correctBtn].transform.GetChild(1).GetComponent<Image>().sprite = icons[0];

        int k = 0;

        // assign remaining words
        for (int i = 1; i < num_words; i++)
        {
            // skip the correct Button
            if (k == correctBtn)
            {
                k++;
            }
            wordBtns[k].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[i];
            // button with icon is a prefab, therefore the child index (1) is known!
            wordBtns[k].transform.GetChild(1).GetComponent<Image>().sprite = icons[i];
            k++;
        }
    }
    private IEnumerator showContinueWait(float delay)
    {
        yield return new WaitForSeconds(delay);
        showContinueButton();
    }

    private void showContinueButton()
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


    // offer strings and icons which shall be presented to the player
    public void startWordSelection(string[] randomWords, Sprite[] randomIcons)
    {
        selectionMade = false;
        wordSelectionUI.SetActive(true);
        words = randomWords;
        icons = randomIcons;
        mapWordsToUI(randomWords.Length);
    }

    // callback method for all word selection buttons (to be assigned via inspector)
    public void ButtonHander(int btn_ix)
    {
        if (selectionMade)
            return;

        if (correctBtn == btn_ix)
        {
            onHitCallback();
            wordBtns[btn_ix].GetComponent<Image>().color = Color.green;
        }
        else
        {
            onMissCallback();
            wordBtns[btn_ix].GetComponent<Image>().color = Color.red;
        }

        selectionMade = true;
        StartCoroutine(showContinueWait(nextDelay));

    }

    public void UnsureButtonHanlder()
    {
        if (selectionMade)
            return;

        onUnsureCallback();

        StartCoroutine(showContinueWait(nextDelay));
    }

    public void ContinueButtonHandler()
    {
        onContinueCallback();
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
    }

}
