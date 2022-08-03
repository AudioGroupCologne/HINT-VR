using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CustomTypes;
using CustomTypes.VRHINTTypes;


public class VRHINTManager : MonoBehaviour
{
    [SerializeField] CustomAudioManager audioManager;
    [SerializeField] LevelObjectManager levelManager;
    [SerializeField] FeedbackManager feedbackManager;
    [SerializeField] VRHINTSettings settingsManager;
    [SerializeField] OverviewManager overviewManager;

    // the first 4 sentences are adjusted in 4 dB steps
    [SerializeField] readonly float initSNRStep = 4.0f;
    // the remaining 16 sentences are adjusted in 16 dB steps
    [SerializeField] readonly float adaptiveSNRStep = 2.0f;
    // initial level of Talker channel at the start of each list
    [SerializeField] readonly float targetStartLevel = 0.5f;
    // fixed level of dist channel (has to be calibrated!)
    [SerializeField] readonly float distractorLevel = 0.0f;
    // Noise condition have the same starting level for speech and noise
    // For the quiet condition this won't make sense
    // Instead set starting level to 25 dBA (65 dBA [calib] - 40 dB)
    [SerializeField] readonly float quietStartingOffset = -40.0f;

    // database object (loads target sentences from resource system)
    private VRHINTDatabase database;

    [SerializeField] float objectDistance = 10.0f;
    [SerializeField] float interfaceDistance = 9.0f;
    [SerializeField] float interfaceHeight = 2.0f;

    [SerializeField] string targetAudioPath = "audio/german-hint/";
    [SerializeField] int numLists = 12;
    // this determines the number of sentences within each list!
    // If there is a mismatch between this and the actual number of files there will be errors during asset loading
    [SerializeField] int numSentences = 20;

    // Number of sentences played from each list during test procedure (must be smaller than numSentences)
    [SerializeField] int numTestSentences = 20;
    // Number of lists used during test procedure (must be smaller than numLists)
    [SerializeField] int numTestLists = 5;
    [SerializeField] int wordOptions = 5;
   
    [SerializeField] AudioClip noise;

    [SerializeField] int practiceList =  12;
    [SerializeField] int numPracticeRounds = 5;
    [SerializeField] hintConditions practiceCondition =  hintConditions.noiseRight;

    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;

    private int listCounter = 0;

    private feedbackSettings feedbackSystem;

    // SNR data for each sentence
    private List<float>[] SNR;
    // hit quite for each sentence
    private List<float>[] hitQuote;

    // estimated SRT for each list
    private List<float> eSRT;

    private List<string> timestamps;

    // noise conditions for each list
    private List<hintConditions> conditions;


    // order of all lists for the test (counter balance stuff)
    private List<int> listOrder;


    // create an List on indices of all sentences of the current list
    // after each round the index of the current sentence is deleted
    // randomly pick an entry from the remaining indices and get AudioClips/Strings that way
    private List<int> listIndices;

    private int userIndex = 0;


    private hintConditions currentCondition;
    private int currentSentenceIndex;
    private int currentListIndex;

    private string[] currentSentence;
    private int sentenceLength = 0;
    private int wordCounter = 0;
    private int sentenceHits = 0;
    private int practiceCounter = 0;

    private testOrder order = 0;


    void Start()
    {
        // set delegates
        audioManager.onPlayingDoneCallback = OnPlayingDone;
        feedbackManager.onWordGuessCallback = onWordGuess;
        feedbackManager.onClassicFeedback = onClassicFeedback;
        feedbackManager.onComprehensionCallback = onComprehensionFeedback;
        settingsManager.OnSettingsDoneCallback = OnStart;
        settingsManager.OnTestOrderCallback = SetTestOrder;

        // place userInterface in correct position for setting selection
        levelManager.angularPosition(levelObjects.userInterface, 0, interfaceDistance, interfaceHeight);

        // show settings screen
        settingsManager.ShowSettings(true);

    }

    /**
     * Determines whether this is the first or second test for the participant.
     * This affects the test conditions the are applied based on the userIndex and the LatinSquares List and Condition order.
     */
    void SetTestOrder(testOrder _order)
    {
        order = _order;
    }


    // no setup on VRHINT test always starts in the same manner
    void OnStart(feedbackSettings settings)
    {

        Debug.Log("Start VR HINT procedure");


        feedbackSystem = settings;

        // create database to hold target sentence lists
        database = new VRHINTDatabase(targetAudioPath, numLists, numSentences);

        // hold every SNR datapoint that goes into SRT calculation
        SNR = new List<float>[numTestLists];
        for(int i = 0; i < numTestLists; i++)
        {
            SNR[i] = new List<float>();
        }

        // hold hit/miss relation for each sentence (e.g. 5 hits, 1 miss on 6 word sentence: 5/6 hitQuote)
        hitQuote = new List<float>[numTestLists];
        for (int i = 0; i < numTestLists; i++)
        {
            hitQuote[i] = new List<float>();
        }


        // hold SRT values for each list 
        eSRT = new List<float>();

        // hold timestamps for each list
        timestamps = new List<string>();

        // hold HINT condition for each list
        conditions = new List<hintConditions>();

        // hold order of sentence lists for test procedure
        listOrder = new List<int>();

        // hold indices of sentences from currentList
        listIndices = new List<int>();

        feedbackManager.showFeedbackSystem(feedbackSystem, false);

        // randomly sort test conditions and sentence lists with no direct repetitions
        importCounterBalancedTestSetup();

        practiceMode = true;
        overviewManager.ShowOverview(true);
        overviewManager.ShowPractice(true);

        // keep track of current assets
        currentListIndex = practiceList;
        currentCondition = practiceCondition;
  
        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));

        currentSentenceIndex = Random.Range(0, listIndices.Count);
        string currSent = database.getSentenceString(currentListIndex, currentSentenceIndex);
        int currSentLen = database.getSentenceWords(currentListIndex, currentSentenceIndex).Length;
        Debug.Log("Sentence " + currentSentenceIndex + ": " + currSent + " (" + currSentLen + ")");


        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // target & UI are always at front position
        levelManager.angularPosition(levelObjects.target, 0, objectDistance);

        // VRHINT only uses dist1 in all conditions except 'quiet' (will be overwritten in this case)
        levelManager.setDistractorSettings(distractorSettings.dist1);

        audioManager.setDistractorAudio(levelObjects.distractor1, noise, true);
         
        ApplyTestConditions();

        updateTestParameterOverview();

        // set target channel to initial level
        if (currentCondition == hintConditions.quiet)
        {
            audioManager.setChannelVolume(audioChannels.target, targetStartLevel + quietStartingOffset);
        }
        else
        {
            audioManager.setChannelVolume(audioChannels.target, targetStartLevel);
        }
        
        // ensure that dist channel is set to correct level
        audioManager.setChannelVolume(audioChannels.distractor, distractorLevel);

        // start playing again
        audioManager.startPlaying();

    }


    // assign order of lists and conditions based on userID through a latin square system
    // store the general order in a separate file
    private void importCounterBalancedTestSetup()
    {

        userIndex = UserManagement.selfReference.getNumTests();

        List<string[]> lqConditions = new List<string[]>();
        List<int[]> lqLists = new List<int[]>();

        TextAsset lqConditionsRaw = Resources.Load("others/lqConditions") as TextAsset;
        string[] lqConditionsSplit = lqConditionsRaw.ToString().Replace("\r", string.Empty).Split('\n');



        for (int i = 0; i < lqConditionsSplit.Length; i++)
        {
            if(lqConditionsSplit[i].Length > 1)
            {
                lqConditions.Add(lqConditionsSplit[i].Split(','));
            }   
        }

        int overhang = 0;
        int orderedUserIndex = 0;

        if (order == testOrder.first)
        {
            orderedUserIndex = userIndex;
        }
        else if (order == testOrder.second)
        {
            orderedUserIndex = userIndex + 1;
        }


        for (int i = 0; i < numTestLists; i++)
        {
            if(i > 0 && i % lqConditions[0].Length == 0)
            {
                overhang++;
            }

            switch (lqConditions[(orderedUserIndex + overhang) % lqConditions.Count][i - (overhang * lqConditions[0].Length)])
            {
                case "noiseFront":
                    conditions.Add(hintConditions.noiseFront);
                    break;
                case "noiseLeft":
                    conditions.Add(hintConditions.noiseLeft);
                    break;
                case "noiseRight":
                    conditions.Add(hintConditions.noiseRight);
                    break;
                case "quiet":
                    conditions.Add(hintConditions.quiet);
                    break;
                default:
                    Debug.LogError("Unrecognized condition: " + lqConditions[orderedUserIndex % lqConditions.Count][i]);
                    break;
            }
        }

        Debug.Log("Test Conditions = " + string.Join(" ", new List<hintConditions>(conditions).ConvertAll(i => i.ToString()).ToArray()));


        TextAsset lqListsRaw = Resources.Load("others/lqLists") as TextAsset;
        string[] lqListsSplit = lqListsRaw.ToString().Replace("\r", string.Empty).Split('\n');

        for (int i = 0; i < lqListsSplit.Length; i++)
        {
            if (lqListsSplit[i].Length > 1)
            {
                lqLists.Add(System.Array.ConvertAll(lqListsSplit[i].Split(','), int.Parse));
            }
        }
        int[] tmp = new int[numTestLists];

        // Warning: This does only work with numTestLists <= 5!!!
        for (int i = 0; i < numTestLists; i++)
        {
            if(order == testOrder.first)
            {
                tmp[i] = lqLists[userIndex % lqLists.Count][i];
            }
            else if(order == testOrder.second)
            {
                tmp[i] = lqLists[userIndex % lqLists.Count][i + numTestLists];
            }
        }

        Debug.Log("Test Lists = " + string.Join(" ", new List<int>(tmp).ConvertAll(i => i.ToString()).ToArray()));

        listOrder.AddRange(tmp);
    }


    private void ApplyTestConditions()
    {

        levelManager.showLevelObjects(false);

        switch (currentCondition)
        {
            case hintConditions.quiet:
                levelManager.setDistractorSettings(distractorSettings.noDist);
                break;
            case hintConditions.noiseFront:
                levelManager.angularPosition(levelObjects.distractor1, 0, objectDistance);
                levelManager.angularPosition(levelObjects.distractor1, 0, objectDistance);
                levelManager.setDistractorSettings(distractorSettings.dist1);
                break;
            case hintConditions.noiseLeft:
                levelManager.angularPosition(levelObjects.distractor1, 270, objectDistance);
                levelManager.setDistractorSettings(distractorSettings.dist1);
                break;
            case hintConditions.noiseRight:
                levelManager.angularPosition(levelObjects.distractor1, 90, objectDistance);
                levelManager.setDistractorSettings(distractorSettings.dist1);
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
        currentSentence = database.getSentenceWords(currentListIndex, currentSentenceIndex);
        sentenceLength = currentSentence.Length;

        switch(feedbackSystem)
        {
            case feedbackSettings.classic:
                // visualize correct sentence to experimenter
                // sanity check: numHits <= sentenceLength
                feedbackManager.setSentenceLength(sentenceLength);
                break;
            case feedbackSettings.wordSelection:
                // always start with the first word of the sentence, so set sentenceStart as true
                string[] rand = database.getRandomWords(wordOptions - 1, currentSentence[0], database.isCapital(currentSentence[0]), true);
                // allocate memory for full word selection
                string[] wordSelection = new string[wordOptions];
                // store correct word at index 0
                wordSelection[0] = currentSentence[0];
                // copy remaining strings into array
                rand.CopyTo(wordSelection, 1);
                // map strings to UI
                feedbackManager.assignWordsToButtons(wordSelection);
                break;
            case feedbackSettings.comprehensionLevel:
                break;
        }

        feedbackManager.showFeedbackSystem(feedbackSystem, true);

    }

    void onWordGuess(bool correct)
    {
        wordCounter++;

        if(correct)
        {
            sentenceHits++;
        }
        
        if(wordCounter >= sentenceLength)
        {
            float _hitQuote = ((float)sentenceHits / (float)sentenceLength);
            Debug.Log("Hit quote: " + _hitQuote);

            feedbackHelper(_hitQuote);       
            
            wordCounter = 0;
            sentenceHits = 0;
            OnContinue();
        }
        else
        {
            // get new random selection
            string[] rand = database.getRandomWords(wordOptions - 1, currentSentence[wordCounter], database.isCapital(currentSentence[wordCounter]), false);

            string[] test = new string[wordOptions];
            test[0] = currentSentence[wordCounter];
            rand.CopyTo(test, 1);
            feedbackManager.assignWordsToButtons(test);
        }

    }

    void onComprehensionFeedback(float rate)
    {
        feedbackHelper(rate);
        OnContinue();
    }

    void onClassicFeedback(int correctWords)
    {
        float _hitQuote = ((float)correctWords / (float)sentenceLength);
        feedbackHelper(_hitQuote);
        OnContinue();
    }

    // store _hitQuote data, store current SNR, change SNR (if sentence[4...20]
    void feedbackHelper(float _hitQuote)
    {
        if (listIndices.Count > 16)
        {
            if (_hitQuote < 0.5f)
            {
                audioManager.changeTalkerVolume(initSNRStep);
            }
            else
            {
                audioManager.changeTalkerVolume(-initSNRStep);
            }
        }
        else
        {
            // store current SNR data point
            SNR[listCounter].Add(audioManager.getTalkerVolume() - targetStartLevel);
            // store current hitQuote data point
            hitQuote[listCounter].Add(_hitQuote);

            if (_hitQuote < 0.5f)
            {
                audioManager.changeTalkerVolume(adaptiveSNRStep);
            }
            else
            {
                audioManager.changeTalkerVolume(-adaptiveSNRStep);
            }
        }
    }


    void OnSessionDone()
    {
        Debug.Log("VRHINT procedure done!");
        VRHintResults tmp = new VRHintResults(feedbackSystem, userIndex, listOrder, conditions, eSRT, SNR, hitQuote, timestamps);
        jsonFiles.saveVRHintResults(tmp, UserManagement.selfReference.getNumTests(), UserManagement.selfReference.getUserName());
        SceneManager.LoadSceneAsync("VRMenuScene");

    }


    void OnContinue()
    {
        feedbackManager.showFeedbackSystem(feedbackSystem, false);

        listIndices.Remove(currentSentenceIndex);

        if(practiceMode)
        {
            if (++practiceCounter >= numPracticeRounds)
            {
                //Debug.Log("Leaving practice mode");
                overviewManager.ShowPractice(false);
                OnListDone();
                return;
            }
                
        }

        
        if(numSentences - listIndices.Count >= numTestSentences)
        {
            OnListDone();
            return;
        }

        // randomly select next sentence (repetition impossibile due to removal of already played sentences)
        currentSentenceIndex = listIndices[Random.Range(0, listIndices.Count)];
        //Debug.Log("Sentences remaining: " + (numTestSentences - (numSentences - listIndices.Count)));
        string currSent = database.getSentenceString(currentListIndex, currentSentenceIndex);
        int currSentLen = database.getSentenceWords(currentListIndex, currentSentenceIndex).Length;
        Debug.Log("Sentence " + (numSentences - listIndices.Count) + ": " + currSent + " (" + currSentLen + ")");

        updateTestParameterOverview();

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // start playing again
        audioManager.startPlaying();

    }

    void OnListDone()
    {
        if(listIndices.Count > 0 && !practiceMode)
        {
            Debug.LogWarning("listIndices is not empty: " + listIndices.Count + ". Clearing up manually!");
            listIndices.Clear();
        }

        if (practiceMode)
        {
            practiceMode = false;
            listIndices.Clear();
        }
        else
        {
            // calculate average SRT
            float _SRT = 0.0f;
            for (int i = 0; i < SNR[listCounter].Count; i++)
            {
                _SRT += SNR[listCounter][i];
            }
            _SRT /= SNR[listCounter].Count;
            Debug.Log("List eSRT: " + _SRT);
            eSRT.Add(_SRT);
            timestamps.Add(System.DateTime.Now.ToString("dd-MM-yy-HH-mm-ss"));

            listCounter++;

        }

        if (listCounter >= numTestLists)
        {
            OnSessionDone();
            return;
        }

        currentCondition = conditions[listCounter];
        currentListIndex = listOrder[listCounter];
        Debug.Log("New condition: " + currentCondition + " new List: " + currentListIndex);

        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));        
        currentSentenceIndex = Random.Range(0, listIndices.Count);

        ApplyTestConditions();

        updateTestParameterOverview();

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // set target channel to initial level
        if(currentCondition == hintConditions.quiet)
        {
            audioManager.setChannelVolume(audioChannels.target, targetStartLevel + quietStartingOffset); 
        }
        else
        {
            audioManager.setChannelVolume(audioChannels.target, targetStartLevel);
        }
        
        // start playing again
        audioManager.startPlaying();

    }

    private void updateTestParameterOverview()
    {
        if (practiceMode)
        {
            overviewManager.SetRounds(practiceCounter + 1, numPracticeRounds);
            overviewManager.SetLists(1, 1);
        }
        else
        {
            overviewManager.SetRounds(numSentences - listIndices.Count, numTestSentences);
            overviewManager.SetLists(listCounter + 1, numTestLists);
        }

        overviewManager.SetCond(currentCondition);
        overviewManager.SetListIndex(currentListIndex);
    }

}
