using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CustomTypes;
using CustomTypes.VRHINTTypes;


public class VRHINTManager : MonoBehaviour
{
    [SerializeField] VRHINTAudioManager audioManager;
    [SerializeField] VRHINTLevelObjectsManager levelManager;
    [SerializeField] FeedbackManager feedbackManager;
    [SerializeField] VRHINTSettings settingsManager;
    [SerializeField] OverviewManager overviewManager;

    // the first 4 sentences are adjusted in 4 dB steps
    [SerializeField] readonly float initSNRStep = 4.0f;
    // the remaining 16 sentences are adjusted in 16 dB steps
    [SerializeField] readonly float adaptiveSNRStep = 2.0f;
    // initial level of Talker channel at the start of each list
    [SerializeField] readonly float targetStartLevel = 0.5f;  // -1.5f would be correct but we're not changing this!
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

    [SerializeField] GameObject darkRoom;
    [SerializeField] GameObject environment;
    [SerializeField] GameObject directionalLight;

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
    private int sentenceCounter = 0;

    private string[] currentSentence;
    private int sentenceLength = 0;
    private int practiceCounter = 0;

    // increased step size (4 dB), no logging of SNR and hitQuotes
    private int calibrationRounds = 4;
    // ratio of correct words required to lower SNR
    private float decisionThreshold = 0.5f;

    private testOrder order = 0;


    void Start()
    {
        // set delegates
        audioManager.OnPlayingDoneCallback = OnPlayingDone;
        feedbackManager.OnFeedback = OnFeedback;
        settingsManager.OnSettingsDoneCallback = OnStart;

        // place userInterface in correct position for setting selection
        levelManager.AngularPosition(hintObjects.userInterface, 0, interfaceDistance, interfaceHeight);

        // show settings screen
        settingsManager.ShowSettings(true);

    }



    // no setup on VRHINT test always starts in the same manner
    void OnStart(testOrder _order, feedbackSettings settings)
    {

        Debug.Log("Start VR HINT procedure");

        /* Determines whether this is the first or second test for the participant.
         * This affects the test conditions the are applied based on the userIndex and the LatinSquares List and Condition order.
        */
        order = _order;


        feedbackSystem = settings;

        if(settings == feedbackSettings.classicDark)
        {
            darkRoom.SetActive(true);
            environment.SetActive(false);
            directionalLight.SetActive(false);
            RenderSettings.ambientLight = Color.black;
        }


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
        ImportCounterBalancedTestSetup();

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
        Debug.Log("Sentence " + sentenceCounter + ": " + currSent + " (" + currSentLen + ")");


        // move new sentence audio to audioManager
        audioManager.SetTargetAudio(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // target & UI are always at front position
        levelManager.AngularPosition(hintObjects.target, 0, objectDistance);

        // VRHINT only uses dist1 in all conditions except 'quiet' (will be overwritten in this case)
        levelManager.ToggleDistractor(true);

        audioManager.SetDistractorAudio(noise, true);
         
        ApplyTestConditions();

        UpdateTestParameterOverview();

        // set target channel to initial level
        if (currentCondition == hintConditions.quiet)
        {
            audioManager.SetChannelLevel(audioChannels.target, targetStartLevel + quietStartingOffset);
        }
        else
        {
            audioManager.SetChannelLevel(audioChannels.target, targetStartLevel);
        }
        
        // ensure that dist channel is set to correct level
        audioManager.SetChannelLevel(audioChannels.distractor, distractorLevel);

        // start playing again
        audioManager.StartPlaying();

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
            case feedbackSettings.classicDark:
                // visualize correct sentence to experimenter
                // sanity check: numHits <= sentenceLength
                feedbackManager.setSentenceLength(sentenceLength);
                break;
            case feedbackSettings.wordSelection:

                // create List of string arrays to hand over to feedbackManager
                List<string[]> randomWords = new List<string[]>();
                
                for (int i = 0; i < sentenceLength; i++)
                {
                    string[] rands = new string[wordOptions];
                    // set correct word to first array index
                    rands[0] = currentSentence[i];

                    // get random words from data base
                    database.getRandomWords(wordOptions - 1, currentSentence[i], database.isCapital(currentSentence[i]), (i == 0)).CopyTo(rands, 1);

                    // copy random words to List
                    randomWords.Add(rands);
                }

                feedbackManager.SetRandomWordProposals(randomWords);
                break;
            case feedbackSettings.comprehensionLevel:
                // nothing to be done here
                break;
        }

        feedbackManager.showFeedbackSystem(feedbackSystem, true);

    }


    /**
     * Take user feedback and trigger next round.
     * ToDo: Add setting if next rounds should be triggered immediately!
     */
    void OnFeedback(float _hitQuote)
    {
        FeedbackHelper(_hitQuote);
        OnContinue();
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
        // hide feedbackUI
        feedbackManager.showFeedbackSystem(feedbackSystem, false);
        // remove index of last sentence from list
        listIndices.Remove(currentSentenceIndex);

        // check if practice mode is done
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

        // check if list is done
        if(++sentenceCounter >= numTestSentences)
        {
            OnListDone();
            return;
        }

        // randomly select next sentence (repetition impossibile due to removal of already played sentences)
        currentSentenceIndex = listIndices[Random.Range(0, listIndices.Count)];
        string currSent = database.getSentenceString(currentListIndex, currentSentenceIndex);
        int currSentLen = database.getSentenceWords(currentListIndex, currentSentenceIndex).Length;
        Debug.Log("Sentence " + sentenceCounter + ": " + currSent + " (" + currSentLen + ")");

        // update UI
        UpdateTestParameterOverview();

        // move new sentence audio to audioManager
        audioManager.SetTargetAudio(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // start playing again
        audioManager.StartPlaying();

    }

    void OnListDone()
    {
        // Issue warning if not all sentences from a list have been played and clean up (this could lead to wrong results if 'numTestSentences' has been lower for debugging)
        if(listIndices.Count > 0 && !practiceMode)
        {
            Debug.LogWarning("listIndices is not empty: " + listIndices.Count + ". Clearing up manually!");
            listIndices.Clear();
        }

        // leave practice mode (multiple practice lists are not supported!)
        if (practiceMode)
        {
            practiceMode = false;
            listIndices.Clear();
        }
        else
        {
            AddListResults();
            listCounter++;

        }

        // check if test procedure is done
        if (listCounter >= numTestLists)
        {
            OnSessionDone();
            return;
        }

        // get next test parameters
        currentCondition = conditions[listCounter];
        currentListIndex = listOrder[listCounter];
        Debug.Log("New condition: " + currentCondition + " new List: " + currentListIndex);

        // apply new test condition
        ApplyTestConditions();

        // refill listIndices with random range
        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));        
        currentSentenceIndex = Random.Range(0, listIndices.Count);
        sentenceCounter = 0;
        
        UpdateTestParameterOverview();

        // move new sentence audio to audioManager
        audioManager.SetTargetAudio(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // set target channel to initial level
        if(currentCondition == hintConditions.quiet)
        {
            audioManager.SetChannelLevel(audioChannels.target, targetStartLevel + quietStartingOffset); 
        }
        else
        {
            audioManager.SetChannelLevel(audioChannels.target, targetStartLevel);
        }
        
        // start playing again
        audioManager.StartPlaying();

    }



    /// <summary>
    /// Utilities
    /// Private methods used to clean up the event based code-structure of 'Manager' components
    /// </summary>


    /**
     * Load LatinSquare matrices for list and condition order.
     * Assign correct order based on current userIndex
     */
    private void ImportCounterBalancedTestSetup()
    {

        // helper variables
        int overhang = 0;
        int orderedUserIndex = 0;

        // get userIndex
        userIndex = UserManagement.selfReference.getNumTests();

        // create storage
        List<string[]> lqConditions = new List<string[]>();
        List<int[]> lqLists = new List<int[]>();


        // to ensure that all conditions are included at least once (as long as test length is at least the amount of condtition)
        // simply jump a row if it's the second part of the test procedure
        // ToDo: make this optional for a full-VR HINT procedure!
        if (order == testOrder.first)
        {
            orderedUserIndex = userIndex;
        }
        else if (order == testOrder.second)
        {
            orderedUserIndex = userIndex + 1;
        }

        // load lqConditions.csv as TextAsset
        // Convert into array of strings ("quiet,noiseFront,noiseRight,noiseLeft\n" "noiseFront,noiseLeft,quiet,noiseRight\n" ...) 
        TextAsset lqConditionsRaw = Resources.Load("others/lqConditions") as TextAsset;
        string[] lqConditionsSplit = lqConditionsRaw.ToString().Replace("\r", string.Empty).Split('\n');

        // get individual conditions by splitting strings at ',' separator
        for (int i = 0; i < lqConditionsSplit.Length; i++)
        {
            if (lqConditionsSplit[i].Length > 1)
            {
                lqConditions.Add(lqConditionsSplit[i].Split(','));
            }
        }
        
        for (int i = 0; i < numTestLists; i++)
        {
            // jump into the next row if the length of a row has been exceed (e.g. take conds [0...3] from row 2 and conds [4..5] from row 3
            if (i > 0 && i % lqConditions[0].Length == 0)
            {
                overhang++;
            }

            // assing hintCondition based on the strings from the lq matrix
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

        // load lqLists.csv as TextAsset
        TextAsset lqListsRaw = Resources.Load("others/lqLists") as TextAsset;
        // Convert TextAsset into array of strings ("1,2,10,...6", "2,3,1,...,7" ...)
        string[] lqListsSplit = lqListsRaw.ToString().Replace("\r", string.Empty).Split('\n');

        // Split at separator ',' and convert into array of integers
        for (int i = 0; i < lqListsSplit.Length; i++)
        {
            if (lqListsSplit[i].Length > 1)
            {
                lqLists.Add(System.Array.ConvertAll(lqListsSplit[i].Split(','), int.Parse));
            }
        }

        // temporary storage
        int[] tmp = new int[numTestLists];

        // if more than half of lqLists dim is used, take 'fresh' rows for both tests
        if(numTestLists > lqLists[0].Length / 2)
        {
            for (int i = 0; i < numTestLists; i++)
            {
                if (order == testOrder.first)
                {
                    tmp[i] = lqLists[orderedUserIndex % lqLists.Count][i];
                }
                else if (order == testOrder.second)
                {
                    tmp[i] = lqLists[(orderedUserIndex) % lqLists.Count][i];
                }
            }
        }
        // split row into two parts (ideally 5 + 5 for optimal counter-balancing)
        // example: 1, 2, 10, 3, 9, 4, 8, 5, 7, 6
        // - first: 1, 2, 10, 3, 9
        // - second: 4, 8, 5, 7, 6
        else
        {
            for (int i = 0; i < numTestLists; i++)
            {
                if (order == testOrder.first)
                {
                    tmp[i] = lqLists[userIndex % lqLists.Count][i];
                }
                else if (order == testOrder.second)
                {
                    tmp[i] = lqLists[userIndex % lqLists.Count][i + numTestLists];
                }
            }
        }


        Debug.Log("Test Lists = " + string.Join(" ", new List<int>(tmp).ConvertAll(i => i.ToString()).ToArray()));

        // Move lists from tmp to listOrder variable 
        listOrder.AddRange(tmp);
    }


    /**
     * Relocate distractor according to hintCondition
     */
    private void ApplyTestConditions()
    {
        // disable all objects before relocation to avoid collisions
        levelManager.ShowLevelObjects(false);

        // always show distractor object if hintCondition is not 'quiet'
        levelManager.ToggleDistractor(true);

        switch (currentCondition)
        {
            case hintConditions.quiet:
                // deactive distractor object
                levelManager.ToggleDistractor(false);
                break;
            case hintConditions.noiseFront:
                levelManager.AngularPosition(hintObjects.distractor, 0, objectDistance);
                break;
            case hintConditions.noiseLeft:
                levelManager.AngularPosition(hintObjects.distractor, 270, objectDistance);
                break;
            case hintConditions.noiseRight:
                levelManager.AngularPosition(hintObjects.distractor, 90, objectDistance);
                break;
            default:
                Debug.LogError("Invalid locationCondition: " + currentCondition);
                break;
        }

        // re-active objects after movement
        levelManager.ShowLevelObjects(true);

    }

    /**
     * Adjust test parameters (target volume) based on the listeners performance and store data point for current sentence.
     * Also consider 'calibrationRounds' (different SNR step size and no data logging) 
     */
    private void FeedbackHelper(float _hitQuote)
    {
        // handle calibration rounds
        if(sentenceCounter < calibrationRounds)
        {
            if (_hitQuote < decisionThreshold)
            {
                audioManager.ChangeChannelLevel(audioChannels.target, initSNRStep);
            }
            else
            {
                audioManager.ChangeChannelLevel(audioChannels.target, -initSNRStep);
            }
        }
        else
        {
            // store current SNR data point
            SNR[listCounter].Add(audioManager.GetChannelLevel(audioChannels.target) - targetStartLevel);
            // store current hitQuote data point
            hitQuote[listCounter].Add(_hitQuote);

            if (_hitQuote < decisionThreshold)
            {
                audioManager.ChangeChannelLevel(audioChannels.target, adaptiveSNRStep);
            }
            else
            {
                audioManager.ChangeChannelLevel(audioChannels.target, -adaptiveSNRStep);
            }
        }
    }

    /**
     * Update overview UI panel to show current state of the test procedure
     */
    private void UpdateTestParameterOverview()
    {
        if (practiceMode)
        {
            overviewManager.SetRounds(practiceCounter + 1, numPracticeRounds);
            // always display "List 1 of 1" for practice mode
            overviewManager.SetLists(1, 1);
        }
        else
        {
            overviewManager.SetRounds(sentenceCounter, numTestSentences);
            overviewManager.SetLists(listCounter + 1, numTestLists);
        }

        overviewManager.SetCond(currentCondition);
        overviewManager.SetListIndex(currentListIndex);
    }

    /**
     * Called upon completion of a list.
     * Add list-based results to storage variables: SRT, timestamp
     * Extend as needed.
     */
    private void AddListResults()
    {
        
        float _SRT = 0.0f;

        // calculate average SRT using SNRs
        for (int i = 0; i < SNR[listCounter].Count; i++)
        {
            _SRT += SNR[listCounter][i];
        }
        _SRT /= SNR[listCounter].Count;

        Debug.Log("List eSRT: " + _SRT);

        // add SRT and timeStamp to result variables
        eSRT.Add(_SRT);
        timestamps.Add(System.DateTime.Now.ToString("dd-MM-yy-HH-mm-ss"));
    }

}
