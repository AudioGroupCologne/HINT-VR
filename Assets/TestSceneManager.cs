using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CustomTypes.TestSceneTypes;


public class TestSceneManager : MonoBehaviour
{

    [SerializeField] TrainingGameAudioManager audioManager;
    [SerializeField] TrainingGameSettings settingsManager;
    [SerializeField] LevelObjectManager levelManager;
    [SerializeField] WordSelectionManager selectionManager;


    [SerializeField] float testSNR = 1.0f;
    [SerializeField] float practiceSNR = 2.0f;


    // there can only be one wordList (even only one voice)
    [SerializeField] string targetAudioPathMale;
    [SerializeField] string targetAudioPathFemale;


    [SerializeField] AudioClip[] femaleDistractor;
    [SerializeField] AudioClip[] maleDistractor;
    [SerializeField] AudioClip[] targetSentences;


    // how often shall SNR be changed on reaching "good" or "bad"
    private int totalReversals = 6;
    // how many reversals shall be used for SRT calculation
    private int srtReversals = 4;


    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;
    // keep track of practice rounds/sentences
    private int practiceReversals = 4;

    // SNR ratio stored for each round (average over whole session will be used)
    private List<float> eSRT;

    void Start()
    {
        //settingsManager.settingsDoneCallback = OnStart;
        settingsManager.gameObject.SetActive(true);
        settingsManager.Init();

        // must not be active before settings have been done!
        selectionManager.gameObject.SetActive(false);
        /*
        selectionManager.onHitCallback = OnHit;
        selectionManager.onMissCallback = OnMiss;
        selectionManager.onUnsureCallback = OnUnsure;
        */
        selectionManager.onContinueCallback = OnContinue;

        audioManager.onPlayingDoneCallback = OnPlayingDone;
    }


    void OnStart(experiments exp)
    {

        Debug.Log("Start Listening Test");

        levelManager.showLevelObjects(true);

        eSRT = new List<float>();

        // make sure to disable UI at load.
        selectionManager.showWordSelectionUI(false);

        // move new sentence audio to audioManager
        //audioManager.setTargetSentence(sent.audio);

        // start playing again
        audioManager.startPlaying();

        // enable selectionManager
        selectionManager.gameObject.SetActive(true);
    }

    void SetTestCondiitons()
    {
        switch (1)
        {
            default:
                break;
        }
    }

    void OnSessionDone()
    {
        Debug.Log("Training session done!");

        // calculate average SNR of the session
        /*
        float average_SNR = 0;
        for (int i = 0; i < roundsPlayed; i++)
        {
            average_SNR += SNR_values[i];
        }
        average_SNR /= roundsPlayed;
        */
        // set current session data onto result UI
        //resultManager.setTrainingGameResults(average_SNR, 0, hits, misses, roundsPlayed);
        // store session data in userManagement
        //UserManagement.selfReference.addUserResults(average_SNR, 0);

    }


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    void OnPlayingDone()
    {

        Debug.Log("OnPlayingDone");

        /*
        if (!practiceMode)
        {
            SNR_values[roundsPlayed - 1] = audioManager.getTalkerVolume();
            Debug.Log("Stored SNR: " + SNR_values[roundsPlayed - 1] + " round: " + roundsPlayed);
        }

        Debug.Log("randGroup: " + randGroup);
        lisnData.getSelectableWords(randGroup, selectionOptions, sent.getSelectableWordIndex(randGroup), out words, out icons);
        */
        // show wordSelection UI elements
        //selectionManager.startWordSelection(words, icons);

    }

    void OnGood()
    {


    }

    void OnMedium()
    {

    }

    void OnBad()
    {

    }

    void OnContinue()
    {
        // hide wordSelection UI elements
        selectionManager.showWordSelectionUI(false);
        /*
        if (roundsPlayed >= gameLength)
        {
            OnSessionDone();
            return;
        }

        if (!repeatSentence)
        {
            // generate a new sentence
            sent.createSentence(lisnData);

            // move new sentence audio to audioManager
            audioManager.setTargetSentence(sent.audio);

            if (practiceMode)
            {
                practiceRounds++;
                Debug.Log("Practice round: " + practiceRounds + " of " + minPracticeRounds + " (min)");
            }
            else
            {
                roundsPlayed++;
                Debug.Log("Round: " + roundsPlayed + " of " + gameLength);
            }

        }
        */
        // start playing again
        audioManager.startPlaying();

    }

}
