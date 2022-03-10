using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{

    [SerializeField] Button[] wordBtns;
    [SerializeField] GameObject selectionUI;

    public delegate void OnWordGuess(bool correct);
    public OnWordGuess onWordGuessCallback = delegate { Debug.Log("No onWordGuess delegate set!"); };

    int correctBtn;


    // new interface: string coorectWord, string[] randomSelection
    public void assignWordsToButtons(string[] words)
    {
        if(words.Length != wordBtns.Length)
        {
            Debug.LogWarning("Strings does not match number of buttons: " + words.Length);
        }

        // get random position for correct Btn
        correctBtn = Random.Range(0, words.Length - 1);

        Debug.Log("Correct Word: " + words[0]);

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

    public void buttonHandler(int index)
    {
        onWordGuessCallback(index == correctBtn);
    }

    public void showFeedbackUI(bool show)
    {
        selectionUI.SetActive(show);
    }

}
