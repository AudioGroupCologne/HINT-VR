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
    [SerializeField] bool useFourWayComprehension = false;

    public delegate void OnWordGuess(bool correct);
    public OnWordGuess onWordGuessCallback = delegate { Debug.Log("No onWordGuess delegate set!"); };

    public delegate void OnClassicFeedback(int numHits);
    public OnClassicFeedback onClassicFeedback = delegate { Debug.Log("No onClassicFeedback delegate set!"); };

    public delegate void OnComprehension(float rate);
    public OnComprehension onComprehensionCallback = delegate { Debug.Log("No onWordGuess delegate set!"); };

    int correctBtn;
    int sentenceLen;

    void Start()
    {
        wordSelectionUI.SetActive(false);
        classicUI.SetActive(false);
        comprehensionUI.SetActive(false);
        fourWayComprehensionUI.SetActive(false);
    }

    void Update()
    {
        if (wordSelectionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                buttonHandler(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buttonHandler(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buttonHandler(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buttonHandler(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buttonHandler(4);
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


    }

    // new interface: string coorectWord, string[] randomSelection
    public void assignWordsToButtons(string[] words)
    {
        if(words.Length != wordBtns.Length)
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

    public void setSentenceLength(int len)
    {
        sentenceLen = len;

    }

    public void buttonHandler(int index)
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        onWordGuessCallback(index == correctBtn);
    }

    public void comprehensionHandler(float rate)
    {
        onComprehensionCallback(rate);
    }

    public void classicHandler(int correctWords)
    {
        if(correctWords > sentenceLen)
        {
            Debug.LogWarning("Invalid entry. 'correctWords' " + correctWords + " > sentence length " + sentenceLen);
            return;
        }
        onClassicFeedback(correctWords);
    }

    public void showFeedbackSystem(feedbackSettings setting, bool show)
    {
        switch(setting)
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
    }

}
