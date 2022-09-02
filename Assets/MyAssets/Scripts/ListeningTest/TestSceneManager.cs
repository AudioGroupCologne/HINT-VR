using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CustomTypes;
using CustomTypes.TestSceneTypes;


public class TestSceneManager : MonoBehaviour
{

    [SerializeField] CustomAudioManager audioManager;
    [SerializeField] TestSceneSettings settingsManager;
    [SerializeField] LevelObjectManager levelManager;
    [SerializeField] UserFeedback selectionManager;


    //[SerializeField] readonly float testSNR = 1.0f;
    //[SerializeField] readonly float practiceSNR = 2.0f;


    // Pfade für target und dist audio angeben
    // Audio manager lädt dann nach anfrage entsprechende files?

    [SerializeField] string targetAudioPath;


    [SerializeField] AudioClip female1Distractor;
    [SerializeField] AudioClip female2Distractor;
    [SerializeField] AudioClip female3Distractor;

    [SerializeField] AudioClip[] maleDistractor;
    [SerializeField] AudioClip[] targetSentences;


    // a min. of practice rounds have to be done. Continued until first upwards reversal?
    //[SerializeField] readonly int minPracticeRounds = 5;
    //[SerializeField] readonly int practiceReversals = 2;

    private enum testState { practice, init, preTest, test};
    private testState t_state = testState.practice;

    private TestSceneData database;

    // how often shall SNR be changed on reaching "good" or "bad"
    //private readonly int totalReversals = 6;
    // how many reversals shall be used for SRT calculation
    //private readonly int srtReversals = 4;

    private int round = 0;
    private int currentCondition = 0;

    /// Control variables
    // start each session with practice mode
    //private bool practiceMode = true;


    // export this to a different file, header, static class, whatever
    [SerializeField]
    List<testCondition> EXP1PracticeConditions = new List<testCondition>() {
                                new testCondition(voices.male1, voices.female2, locationConditions.sameLocation),
                                new testCondition(voices.male1, voices.female2, locationConditions.differentLocations) };


    [SerializeField]
    List<testCondition> EXP1Conditions = new List<testCondition>() {
                                new testCondition(voices.female1, voices.female1, locationConditions.sameLocation),
                                new testCondition(voices.female1, voices.female1, locationConditions.differentLocations),
                                new testCondition(voices.female2, voices.female3, locationConditions.sameLocation),
                                new testCondition(voices.female2, voices.female3, locationConditions.differentLocations),
                                new testCondition(voices.female2, voices.male2, locationConditions.sameLocation),
                                new testCondition(voices.female2, voices.male2, locationConditions.differentLocations)};

    [SerializeField]
    List<testCondition> EXP2Conditions = new List<testCondition>() {
                                new testCondition(voices.female1, voices.female1, locationConditions.sameLocation),
                                new testCondition(voices.female1, voices.female1, locationConditions.differentLocations),
                                new testCondition(voices.female2, voices.male2, locationConditions.sameLocation),
                                new testCondition(voices.female2, voices.male2, locationConditions.differentLocations)};




    // SNR ratio stored for each round (average over whole session will be used)
    private List<float> eSRT;

    private experiment activeExp;

    void Start()
    {
        //settingsManager.settingsDoneCallback = OnStart;
        settingsManager.gameObject.SetActive(true);
        settingsManager.Init();

        // must not be active before settings have been done!
        selectionManager.gameObject.SetActive(false);
        
        selectionManager.onGoodCallback = OnGood;
        selectionManager.onMediumCallback = OnMedium;
        selectionManager.onBadCallback = OnBad;

        audioManager.OnPlayingDoneCallback = OnPlayingDone;
    }


    void OnStart(experiments exp)
    {

        Debug.Log("Start Listening Test");
        switch(exp)
        {
            case experiments.experiment1:
                activeExp = new experiment(EXP1PracticeConditions, EXP1Conditions);
                break;
            case experiments.experiment2:
                activeExp = new experiment(EXP2Conditions);
                break;
            default:
                Debug.LogError("Invalid experiment selection: " + exp);
                return;
        }



        ApplyTestConditions();
        
        levelManager.showLevelObjects(true);

        // database shall simply load all assets for all purposes (female1/2/3, male1/2/3)
        // interface: GetDistractor(male1, story2) or GetDistractor(male2, story2) and GetTargetSentences(index)
        database = new TestSceneData("audio/test/female");

        eSRT = new List<float>();

        // make sure to disable UI at load.
        selectionManager.ShowUserInterface(false);

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.GetTargetSentences(round));

        // start playing again
        audioManager.startPlaying();

        // enable selectionManager
        selectionManager.gameObject.SetActive(true);
    }


    private void ApplyTestConditions()
    {

        testCondition tmp = activeExp.GetTestConditions(currentCondition);
        
        // set locationCondition
        switch (tmp.loc)
        {
            case locationConditions.sameLocation:
                levelManager.setLevelObjectPosition(levelObjects.target, levelPositions.front);
                levelManager.setLevelObjectPosition(levelObjects.distractor1, levelPositions.front);
                levelManager.setLevelObjectPosition(levelObjects.distractor2, levelPositions.front);
                break;
            case locationConditions.differentLocations:
                levelManager.setLevelObjectPosition(levelObjects.target, levelPositions.front);
                levelManager.setLevelObjectPosition(levelObjects.distractor1, levelPositions.left);
                levelManager.setLevelObjectPosition(levelObjects.distractor2, levelPositions.right);
                break;
            default:
                Debug.LogError("Invalid locationCondition: " + tmp.loc);
                return;
        }

        // set voiceCondition
        
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

    void OnGood()
    {
        switch(t_state)
        {
            case testState.init:
                break;
            case testState.practice:
                break;
            case testState.preTest:
                break;
            case testState.test:
                break;
        }
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
        selectionManager.ShowUserInterface(false);
        round++;

        /*
        if (roundsPlayed >= gameLength)
        {
            OnSessionDone();
            return;
        }
        */

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.GetTargetSentences(round));

        // start playing again
        audioManager.startPlaying();

    }

}
