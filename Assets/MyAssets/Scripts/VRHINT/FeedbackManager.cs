using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomTypes.VRHINTTypes;

public class FeedbackManager : MonoBehaviour
{

    [SerializeField] Button[] wordBtns;
    [SerializeField] GameObject wordSelectionUI;
    [SerializeField] GameObject classicUI;
    [SerializeField] GameObject comprehensionUI;
    [SerializeField] GameObject fourWayComprehensionUI;
    [SerializeField] GameObject continueUI;
    [SerializeField] bool useFourWayComprehension = false;

    public delegate void onFeedback(float hitQuote);
    public onFeedback OnFeedback = delegate { Debug.Log("No OnFeedback delegate set!"); };

    public delegate void onContinue();
    public onContinue OnContinue = delegate { Debug.Log("No OnContinue delegate set!"); };

    int correctBtn;
    int sentenceLen;
    // used keeping track of sentence words in wordSelection feedback
    private int wordCounter = 0;
    private int hitCounter = 0;
    private List<string[]> words;

    void Start()
    {
        wordSelectionUI.SetActive(false);
        classicUI.SetActive(false);
        comprehensionUI.SetActive(false);
        fourWayComprehensionUI.SetActive(false);
    }

    /**
     * KeyBoard Controls for debugging
     */
    void Update()
    {

        if (wordSelectionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                wordSelectionHandler(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                wordSelectionHandler(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                wordSelectionHandler(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                wordSelectionHandler(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                wordSelectionHandler(4);
            }

        }
        else if(classicUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                classicHandler(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                classicHandler(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                classicHandler(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                classicHandler(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                classicHandler(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                classicHandler(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                classicHandler(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                classicHandler(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                classicHandler(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                classicHandler(9);
            }
        }
        else if (comprehensionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                comprehensionHandler(1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                comprehensionHandler(0.0f);
            }
        }
        else if (fourWayComprehensionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                comprehensionHandler(1.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                comprehensionHandler(0.75f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                comprehensionHandler(0.25f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                comprehensionHandler(0.0f);
            }
        }
        else if(continueUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                continueHanlder();
            }
        }


    }

    /**
     * Toggle the UI for the selected feedback system
     */
    public void ShowFeedbackUI(feedbackSettings setting, bool show)
    {
        switch (setting)
        {
            case feedbackSettings.classic:
            case feedbackSettings.classicDark:
                classicUI.SetActive(show);
                break;
            case feedbackSettings.wordSelection:
                wordSelectionUI.SetActive(show);
                break;
            case feedbackSettings.comprehensionLevel:
                if (useFourWayComprehension)
                {
                    fourWayComprehensionUI.SetActive(show);
                }
                else
                {
                    comprehensionUI.SetActive(show);
                }
                break;
            default:
                Debug.LogError("Invalid feedback system: " + setting);
                return;
        }

        // make sure to hide continueUI
        if(!show)
        {
            ShowContinue(false);
        }
    }


    public void ShowContinue(bool show)
    {
        continueUI.SetActive(show);
    }


    /**
     * Set the length of the current target sentence (required for classic and classicDark)
     */
    public void SetSentenceLength(int len)
    {
        sentenceLen = len;
    }

    /**
     * Set the random words (required for the wordSelection)
     */
    public void SetRandomWordProposals(List<string[]> _words)
    {
        // create storage for word proposals
        words = new List<string[]>();

        // reset variables
        wordCounter = 0;
        hitCounter = 0;

        // set sentenceLen based on list entries in _words
        sentenceLen = _words.Count;

        for (int i = 0; i < sentenceLen; i++)
        {
            words.Add(_words[i]);
        }

        AssignWordsToButtons(words[wordCounter]);
    }


    ///// Button handlers
    public void wordSelectionHandler(int index)
    {
        // deselect button
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        OnWordGuess(index == correctBtn);
    }

    public void comprehensionHandler(float hitQuote)
    {
        OnFeedback(hitQuote);
    }

    public void classicHandler(int correctWords)
    {
        if(correctWords > sentenceLen)
        {
            Debug.LogWarning("Invalid entry. 'correctWords' " + correctWords + " > sentence length " + sentenceLen);
            return;
        }

        OnFeedback((float)correctWords / (float)sentenceLen);
    }

    public void continueHanlder()
    {
        OnContinue();
    }

    //// Private methods
    private void OnWordGuess(bool correct)
    {
        wordCounter++;

        if (correct)
        {
            hitCounter++;
        }

        if (wordCounter >= sentenceLen)
        {
            float _hitQuote = ((float)hitCounter / (float)sentenceLen);
            OnFeedback(_hitQuote);
        }
        else
        {
            AssignWordsToButtons(words[wordCounter]);
        }

    }

    private void AssignWordsToButtons(string[] words)
    {
        if (words.Length != wordBtns.Length)
        {
            Debug.LogWarning("Strings does not match number of buttons: " + words.Length);
        }

        // get random position for correct Btn (max is exclusive so omit -1)
        correctBtn = Random.Range(0, words.Length);

        // first assign correct word
        wordBtns[correctBtn].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[0];

        int k = 0;

        // assign remaining words
        for (int i = 1; i < words.Length; i++)
        {
            // skip the correct Button
            if (k == correctBtn)
            {
                k++;
            }
            wordBtns[k].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = words[i];
            k++;
        }
    }

}
