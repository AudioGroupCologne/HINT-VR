using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CustomTypes;
using CustomTypes.VRHINTTypes;
using System.Collections;

public class VRHINTManager : MonoBehaviour
{
    [SerializeField] VRHINTAudioManager audioManager;
    [SerializeField] VRHINTLevelObjectsManager levelManager;
    [SerializeField] FeedbackManager feedbackManager;
    [SerializeField] VRHINTSettings settingsManager;
    [SerializeField] OverviewManager overviewManager;


    // position of the UI in the scene relative to the camera
    [SerializeField] float objectDistance = 10.0f;
    [SerializeField] float interfaceDistance = 9.0f;
    [SerializeField] float interfaceHeight = 2.0f;

    [SerializeField] bool showOverviewUI = true;

    // objects required for 'classicDark' mode
    [SerializeField] GameObject darkRoom;
    [SerializeField] GameObject environment;
    [SerializeField] GameObject directionalLight;

    // database object (loads target sentences from resource system)

    private VRHINTDatabase database;    
    // parameter object (refactor at this at some point)

    private VRHINTParameters parameters;
    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;

    // number of lists that have already been done
    private int listCounter = 0;

    // holds the selected feedbackSystem
    private feedbackSettings feedbackSystem;

    // SNR data for each sentence
    private List<float>[] SNR;
    // hit quite for each sentence
    private List<float>[] hitQuote;

    // estimated SRT for each list
    private List<float> eSRT;

    // timestamps for the completion of each test list
    private List<string> timestamps;

    // noise conditions for each list
    private List<hintConditions> conditions;

    // order of all lists for the test (counter balance stuff)
    private List<int> listOrder;

    // create an List on indices of all sentences of the current list
    // after each round the index of the current sentence is deleted
    // randomly pick an entry from the remaining indices and get AudioClips/Strings that way
    private List<int> listIndices;

    // userIndex used for counterbalancing (determined by number of JSON files in the result directory)
    private int userIndex = 0;

    // state variables
    private hintConditions currentCondition;
    private int currentSentenceIndex;
    private int currentListIndex;
    private int sentenceCounter = 0;
    private int practiceCounter = 0;

    // information about current sentence to be provided to experimenter and feedbackSystem
    private string[] currentSentence;
    private int sentenceLength = 0;
    
    // testOrder (first or second) determines counterbalacning
    private testOrder order = 0;


    void Start()
    {

        // create parameters object
        parameters = GetComponent<VRHINTParameters>();

        // set delegates
        audioManager.OnPlayingDone = OnPlayingDone;
        feedbackManager.OnFeedback = OnFeedback;
        feedbackManager.OnContinue = OnContinue;
        settingsManager.OnSettingsDone = OnStart;

        overviewManager.ShowOverview(showOverviewUI);

        // place userInterface in correct position for setting selection
        levelManager.ShowLevelObject(hintObjects.target, false);
        levelManager.SetRelativePosition(hintObjects.userInterface, 0, interfaceDistance, interfaceHeight);
        levelManager.ShowLevelObject(hintObjects.userInterface, true);

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


        // create database to hold target sentence lists and noise signal
        database = new VRHINTDatabase(parameters.targetAudioPath, parameters.noisePath, parameters.numLists, parameters.numSentences);

        // hold every SNR datapoint that goes into SRT calculation
        SNR = new List<float>[parameters.numTestLists];
        for(int i = 0; i < parameters.numTestLists; i++)
        {
            SNR[i] = new List<float>();
        }

        // hold hit/miss relation for each sentence (e.g. 5 hits, 1 miss on 6 word sentence: 5/6 hitQuote)
        hitQuote = new List<float>[parameters.numTestLists];
        for (int i = 0; i < parameters.numTestLists; i++)
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

        feedbackManager.ShowFeedbackUI(feedbackSystem, false);

        // randomly sort test conditions and sentence lists with no direct repetitions
        //ImportCounterBalancedTestSetup();
        Counterbalancing.ImportLatinSquares(userIndex, parameters.numTestLists, order, conditions, listOrder);

        practiceMode = true;
        if (showOverviewUI)
        {
            overviewManager.ShowOverview(true);
            overviewManager.ShowPractice(true);
        }


        // keep track of current assets
        currentListIndex = parameters.practiceList;
        currentCondition = parameters.practiceCondition;
  
        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));

        currentSentenceIndex = Random.Range(0, listIndices.Count);
        string currSent = database.getSentenceString(currentListIndex, currentSentenceIndex);
        int currSentLen = database.getSentenceWords(currentListIndex, currentSentenceIndex).Length;
        Debug.Log("Sentence " + sentenceCounter + ": " + currSent + " (" + currSentLen + ")");


        // move new sentence audio to audioManager
        audioManager.SetTargetAudio(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // target & UI are always at front position
        levelManager.ShowLevelObject(hintObjects.target, true);
        levelManager.SetRelativePosition(hintObjects.target, 0, objectDistance);

        audioManager.SetDistractorAudio(database.getNoise(), true);
         
        // setup initial test condition
        ApplyTestConditions(currentCondition);

        UpdateTestParameterOverview();

        // set target channel to initial level
        if (currentCondition == hintConditions.quiet)
        {
            audioManager.SetChannelLevel(audioChannels.target, parameters.targetStartLevel + parameters.quietStartingOffset);
        }
        else
        {
            audioManager.SetChannelLevel(audioChannels.target, parameters.targetStartLevel);
        }
        
        // ensure that dist channel is set to correct level
        audioManager.SetChannelLevel(audioChannels.distractor, parameters.distractorLevel);

        // small back-off before playing first sentence
        StartCoroutine(PlaybackDelay(1.0f));
       

    }

    private IEnumerator PlaybackDelay(float delay)
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(delay);

        // start playing first sentence
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
                feedbackManager.SetSentenceLength(sentenceLength);
                break;
            case feedbackSettings.wordSelection:

                // create List of string arrays to hand over to feedbackManager
                List<string[]> randomWords = new List<string[]>();
                
                for (int i = 0; i < sentenceLength; i++)
                {
                    string[] rands = new string[parameters.wordOptions];
                    // set correct word to first array index
                    rands[0] = currentSentence[i];

                    // get random words from data base
                    database.getRandomWords(parameters.wordOptions - 1, currentSentence[i], (i == 0)).CopyTo(rands, 1);

                    // copy random words to List
                    randomWords.Add(rands);
                }

                feedbackManager.SetRandomWordProposals(randomWords);
                break;
            case feedbackSettings.comprehensionLevel:
                // nothing to be done here
                break;
        }

        feedbackManager.ShowFeedbackUI(feedbackSystem, true);

    }


    /**
     * Take user feedback and trigger next round.
     * ToDo: Add setting if next rounds should be triggered immediately!
     */
    void OnFeedback(float _hitQuote)
    {
        FeedbackHelper(_hitQuote);

        // directly continue for user-controlled systems
        if(feedbackSystem == feedbackSettings.wordSelection || feedbackSystem == feedbackSettings.comprehensionLevel)
        {
            OnContinue();
        }
        else
        {
            feedbackManager.ShowFeedbackUI(feedbackSystem, false);
            feedbackManager.ShowContinue(true);
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
        // hide feedbackUI
        feedbackManager.ShowFeedbackUI(feedbackSystem, false);
        // remove index of last sentence from list
        listIndices.Remove(currentSentenceIndex);

        // check if practice mode is done
        if(practiceMode)
        {
            if (++practiceCounter >= parameters.numPracticeRounds)
            {
                //Debug.Log("Leaving practice mode");
                overviewManager.ShowPractice(false);
                OnListDone();
                return;
            }
                
        }

        // check if list is done
        if(++sentenceCounter >= parameters.numTestSentences)
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
        if (listCounter >= parameters.numTestLists)
        {
            OnSessionDone();
            return;
        }

        // get next test parameters
        currentCondition = conditions[listCounter];
        currentListIndex = listOrder[listCounter];
        Debug.Log("New condition: " + currentCondition + " new List: " + currentListIndex);

        // apply new test condition
        ApplyTestConditions(currentCondition);

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
            audioManager.SetChannelLevel(audioChannels.target, parameters.targetStartLevel + parameters.quietStartingOffset); 
        }
        else
        {
            audioManager.SetChannelLevel(audioChannels.target, parameters.targetStartLevel);
        }
        
        // start playing again
        audioManager.StartPlaying();

    }



    /// <summary>
    /// Utilities
    /// Private methods used to clean up the event based code-structure of 'Manager' components
    /// </summary>

    /**
     * Relocate distractor according to hintCondition
     */
    private void ApplyTestConditions(hintConditions cond)
    {

        levelManager.ShowLevelObject(hintObjects.distractor, false);

        switch (cond)
        {
            case hintConditions.quiet:
                break;
            case hintConditions.noiseFront:
                levelManager.SetRelativePosition(hintObjects.distractor, 0, objectDistance);
                levelManager.ShowLevelObject(hintObjects.distractor, true);
                break;
            case hintConditions.noiseLeft:
                levelManager.SetRelativePosition(hintObjects.distractor, 270, objectDistance);
                levelManager.ShowLevelObject(hintObjects.distractor, true);
                break;
            case hintConditions.noiseRight:
                levelManager.SetRelativePosition(hintObjects.distractor, 90, objectDistance);
                levelManager.ShowLevelObject(hintObjects.distractor, true);
                break;
            default:
                Debug.LogError("Invalid locationCondition: " + currentCondition);
                break;
        }


    }

    /**
     * Adjust test parameters (target volume) based on the listeners performance and store data point for current sentence.
     * Also consider 'calibrationRounds' (different SNR step size and no data logging) 
     */
    private void FeedbackHelper(float _hitQuote)
    {
        // handle calibration rounds
        if(sentenceCounter < parameters.calibrationRounds)
        {
            if (_hitQuote < parameters.decisionThreshold)
            {
                audioManager.ChangeChannelLevel(audioChannels.target, parameters.initSNRStep);
            }
            else
            {
                audioManager.ChangeChannelLevel(audioChannels.target, -parameters.initSNRStep);
            }
        }
        else
        {
            // store current SNR data point
            SNR[listCounter].Add(audioManager.GetChannelLevel(audioChannels.target) - parameters.targetStartLevel);
            // store current hitQuote data point
            hitQuote[listCounter].Add(_hitQuote);

            if (_hitQuote < parameters.decisionThreshold)
            {
                audioManager.ChangeChannelLevel(audioChannels.target, parameters.adaptiveSNRStep);
            }
            else
            {
                audioManager.ChangeChannelLevel(audioChannels.target, -parameters.adaptiveSNRStep);
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
            overviewManager.SetRounds(practiceCounter + 1, parameters.numPracticeRounds);
            // always display "List 1 of 1" for practice mode
            overviewManager.SetLists(1, 1);
        }
        else
        {
            overviewManager.SetRounds(sentenceCounter, parameters.numTestSentences);
            overviewManager.SetLists(listCounter + 1, parameters.numTestLists);
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
