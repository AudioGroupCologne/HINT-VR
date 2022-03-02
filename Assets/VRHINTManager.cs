using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTypes.VRHINTTypes;

public class VRHINTManager : MonoBehaviour
{
    [SerializeField] CustomAudioManager audioManager;
    //[SerializeField] TestSceneSettings settingsManager;
    [SerializeField] LevelObjectManager levelManager;
    [SerializeField] SentenceInput inputManager;
    //[SerializeField] UserFeedback selectionManager;

    // the first 4 sentences are adjusted in 4 dB steps
    [SerializeField] readonly float initSNRStep = 4.0f;
    // the remaining 16 sentences are adjusted in 16 dB steps
    [SerializeField] readonly float adaptiveSNRStep = 2.0f;
    
    // database object (loads target sentences from resource system)
    private VRHINTDatabase database;

    [SerializeField] string targetAudioPath = "audio/german-hint/";
    [SerializeField] int numLists = 12;
    [SerializeField] int numSentences = 20;
    [SerializeField] int numTestLists = 10;
   
    [SerializeField] AudioClip noise;

    [SerializeField] int[] practiceLists = { 11, 12 };
    [SerializeField] hintConditions[] practiceConditions = { hintConditions.noiseFront, hintConditions.noiseRight };
  

    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;


    private List<float[]> SNRdata;
    // SNR ratio stored for each round (average over whole session will be used)
    private List<float> eSRT;
    private List<hintConditions> conditions;
    private List<int> sentenceLists;

    private AudioClip[] currentSentences;
    private List<AudioClip> testLists;


    private hintConditions currentCondition;
    private int currentSentenceIndex;
    private int currentListIndex;

    // how many sentences have been played in current list
    private int sentencesCounter = 0;

    void Start()
    {

        OnStart();
        inputManager.onInputCallback = onSubmission;
        audioManager.onPlayingDoneCallback = OnPlayingDone;

        return;


        //settingsManager.settingsDoneCallback = OnStart;
        //settingsManager.gameObject.SetActive(true);
        //settingsManager.Init();

        // must not be active before settings have been done!
        //selectionManager.gameObject.SetActive(false);

        // ToDo: UI after sentence presentation?
        /*
        selectionManager.onGoodCallback = OnGood;
        selectionManager.onMediumCallback = OnMedium;
        selectionManager.onBadCallback = OnBad;
        */
        
    }


    // no setup on VRHINT test always starts in the same manner
    void OnStart()
    {

        Debug.Log("Start VR HINT procedure");

        // create database to hold target sentence lists
        database = new VRHINTDatabase(targetAudioPath, numLists, numSentences);

        // hold every SNR datapoint that goes into SRT calculation
        SNRdata = new List<float[]>();

        // hold SRT values for each list 
        eSRT = new List<float>();

        // hold HINT condition for each list
        conditions = new List<hintConditions>();

        // hold order of sentence lists for test procedure
        sentenceLists = new List<int>();

        // copy practiceLists into new list
        List<int> tmp = new List<int>(practiceLists);

        for (int i = 0; i < numLists; i++)
        {
            
            // remove detected practiceLists content
            for(int k = 0; k < tmp.Count; k++)
            {
                if (i == tmp[k])
                {
                    tmp.RemoveAt(k);
                    i++;
                }      
            }

            if(i < numLists)
            {
                sentenceLists.Add(i);
                Debug.Log("Sentence entry: " + i);
            }
            
        }

        // hold current sentence list (List allows simple removal of entries)
        testLists = new List<AudioClip>();

        // make sure to disable UI at load.
        //selectionManager.ShowUserInterface(false);

        // randomly sort test conditions and sentence lists with no direct repetitions
        createCounterBalancedTest();

        practiceMode = true;

        // keep track of current assets
        currentListIndex = practiceLists[0];
        currentListIndex = practiceLists[0];
        currentCondition = practiceConditions[0];

        //currentSentences = database.getList(practiceLists[0]);

        testLists.AddRange(database.getList(practiceLists[0]));

        currentSentenceIndex = Random.Range(0, testLists.Count);
        
        // move new sentence audio to audioManager
        audioManager.setTargetSentence(testLists[currentSentenceIndex]);

        ApplyTestConditions();

        // start playing again
        audioManager.startPlaying();

        // enable selectionManager
        //selectionManager.gameObject.SetActive(true);
    }


    /**
     * Idea: assign pattern of test conditions like: Q, NF, NL, NR, NF, Q, NR, ...
     * - make sure no conditions are directly repeated
     * - make sure that each condition is assigned at least twice
     * - 
     **/
    // Latin Squares: each entry only once per row and col...
    // - this was used to determine List order across subjects... so yeah no idea
    /**
     * Q F R L
     * F R L Q
     * L Q F R
     * R L Q F
     */
    private void createCounterBalancedTest()
    {

        int tmp = 0;
        int _tmp = 0;
        System.Random rng = new System.Random();

        for (int i = 0; i < numTestLists; i++)
        {
            // make sure that there are no direct repetitions
            while(tmp == _tmp)
            {
                tmp = Random.Range(0, 3);
            }

            // map int to enum
            switch(tmp)
            {
                case 0:
                    //conditions[i] = hintConditions.quiet;
                    conditions.Add(hintConditions.quiet);
                    break;
                case 1:
                    //conditions[i] = hintConditions.noiseFront;
                    conditions.Add(hintConditions.noiseFront);
                    break;
                case 2:
                    //conditions[i] = hintConditions.noiseLeft;
                    conditions.Add(hintConditions.noiseLeft);
                    break;
                case 3:
                    //conditions[i] = hintConditions.noiseRight;
                    conditions.Add(hintConditions.noiseRight);
                    break;
            }
            _tmp = tmp;
        }
        
        // randomize oder of sentence lists
        int n = sentenceLists.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = sentenceLists[k];
            sentenceLists[k] = sentenceLists[n];
            sentenceLists[n] = value;
        }

    }

    /*
    static void LatinSquare(int n)
    {
        if (n <= 0)
        {
            return;
        }

        // var latin = new Matrix();
        List<List<int>> latin = List<List<int>>();

        for (int i = 0; i < n; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < n; j++)
            {
                temp.Add(j);
            }
            latin.Add(temp);
        }
        // first row
        latin[0].Shuffle();

        // middle row(s)
        for (int i = 1; i < n - 1; i++)
        {
            bool shuffled = false;

            while (!shuffled)
            {
                latin[i].Shuffle();
                for (int k = 0; k < i; k++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (latin[k][j] == latin[i][j])
                        {
                            goto shuffling;
                        }
                    }
                }
                shuffled = true;

            shuffling: { }
            }
        }

        // last row
        for (int j = 0; j < n; j++)
        {
            List<bool> used = new List<bool>();
            for (int i = 0; i < n; i++)
            {
                used.Add(false);
            }

            for (int i = 0; i < n - 1; i++)
            {
                used[latin[i][j]] = true;
            }
            for (int k = 0; k < n; k++)
            {
                if (!used[k])
                {
                    latin[n - 1][j] = k;
                    break;
                }
            }
        }
    }
    */

    private void ApplyTestConditions()
    {
        // target is always at front position
        levelManager.setLevelObjectPosition(CustomTypes.TestSceneTypes.levelObjects.target, CustomTypes.TestSceneTypes.levelPositions.front);

        // VRHINT only uses dist1 in all conditions except 'quiet' (will be overwritten in this case)
        levelManager.setDistractorSettings(CustomTypes.TrainingGameTypes.distractorSettings.dist1);

        switch (currentCondition)
        {
            case hintConditions.quiet:
                levelManager.setDistractorSettings(CustomTypes.TrainingGameTypes.distractorSettings.noDist);
                break;
            case hintConditions.noiseFront:
                levelManager.setLevelObjectPosition(CustomTypes.TestSceneTypes.levelObjects.target, CustomTypes.TestSceneTypes.levelPositions.front);
                levelManager.setLevelObjectPosition(CustomTypes.TestSceneTypes.levelObjects.distractor1, CustomTypes.TestSceneTypes.levelPositions.front);
                
                break;
            case hintConditions.noiseLeft:
                levelManager.setLevelObjectPosition(CustomTypes.TestSceneTypes.levelObjects.distractor1, CustomTypes.TestSceneTypes.levelPositions.left);
                break;
            case hintConditions.noiseRight:
                levelManager.setLevelObjectPosition(CustomTypes.TestSceneTypes.levelObjects.distractor1, CustomTypes.TestSceneTypes.levelPositions.right);
                break;
            default:
                Debug.LogError("Invalid locationCondition: " + currentCondition);
                break;
        }

        levelManager.showLevelObjects(true);
        

    }


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    void OnPlayingDone()
    {

        Debug.Log("OnPlayingDone");
        inputManager.ShowSentenceInput(true);

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
        Debug.Log("VRHINT procedure done!");

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

    // enter how many words have been repeated correclty
    void onSubmission(int correctWords)
    {

    }

    // enter sentences repetion as array of strings an let app compute errors
    void onSubmission(string sentence)
    {
        Debug.Log("On submission: " + sentence);
        inputManager.ShowSentenceInput(false);
    }


    void OnContinue()
    {
        // hide wordSelection UI elements
        //selectionManager.ShowUserInterface(false);
        sentencesCounter++;

        // load new list
        if(sentencesCounter >= 20)
        {
            OnListDone();
        }

        /*
        if (roundsPlayed >= gameLength)
        {
            OnSessionDone();
            return;
        }
        */

        // remove last played sentence from list
        testLists.RemoveAt(currentSentenceIndex);

        // randomly select next sentence (repetition impossibile due to removal of already played sentences)
        currentSentenceIndex = Random.Range(0, testLists.Count);

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(testLists[currentSentenceIndex]);

        // start playing again
        audioManager.startPlaying();

    }

    void OnListDone()
    {
        if(testLists.Count > 0)
        {
            Debug.LogWarning("testList is not empty: " + testLists.Count);
        }

        if(practiceMode)
        {
            // ToDo: make this work with variable number of practice lists!
            currentListIndex = practiceLists[1];
            currentCondition = practiceConditions[1];
        }
        else
        {
            // ToDo: make sure that each condition is used at least twice (don't know what should happen then...)
            // randomly select next test condition
            currentCondition = hintConditions.quiet;
        }

        // reset sentecesCounter
        sentencesCounter = 0;

        // copy new AudioClips into local testList
        testLists.AddRange(database.getList(currentListIndex));

        

        currentSentenceIndex = Random.Range(0, testLists.Count);



    }
}
