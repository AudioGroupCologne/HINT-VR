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
    [SerializeField] SentenceInput inputManager;
    [SerializeField] FeedbackManager feedbackManager;
    

    // the first 4 sentences are adjusted in 4 dB steps
    [SerializeField] readonly float initSNRStep = 4.0f;
    // the remaining 16 sentences are adjusted in 16 dB steps
    [SerializeField] readonly float adaptiveSNRStep = 2.0f;
    
    // database object (loads target sentences from resource system)
    private VRHINTDatabase database;

    [SerializeField] float objectDistance = 10.0f;
    [SerializeField] float interfaceDistance = 9.0f;
    [SerializeField] float interFaceHeight = 2.0f;

    [SerializeField] string targetAudioPath = "audio/german-hint/";
    [SerializeField] int numLists = 12;
    [SerializeField] int numSentences = 20;
    [SerializeField] int numTestLists = 5;
    [SerializeField] int wordOptions = 5;
   
    [SerializeField] AudioClip noise;

    [SerializeField] int practiceList =  12;
    [SerializeField] int practiceRounds = 5;
    [SerializeField] hintConditions practiceCondition =  hintConditions.noiseRight;


    [SerializeField] API_3DTI_HA hearingAids;
    [SerializeField] API_3DTI_HL hearingLoss;

    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;

    private int listCounter = 0;

    // SNR data for each sentence
    //private List<List<float>> SNR;
    private List<float>[] SNR;

    // estimated SRT for each list
    private List<float> eSRT;

    // noise conditions for each list
    private List<hintConditions> conditions;


    // order of all lists for the test (counter balance stuff)
    private List<int> listOrder;


    // create an List on indices of all sentences of the current list
    // after each round the index of the current sentence is deleted
    // randomly pick an entry from the remaining indices and get AudioClips/Strings that way
    private List<int> listIndices;

    //private int sentenceMisses;
    private List<float> hitQuote;


    private hintConditions currentCondition;
    private int currentSentenceIndex;
    private int currentListIndex;

    private string[] currentSentence;
    private int sentenceLength;
    private int wordCounter = 0;
    private int sentenceHits = 0;


    void Start()
    {
        audioManager.onPlayingDoneCallback = OnPlayingDone;
        feedbackManager.onWordGuessCallback = onWordGuess;

        OnStart();
       
    }


    // no setup on VRHINT test always starts in the same manner
    void OnStart()
    {

        Debug.Log("Start VR HINT procedure");

        hearingAids.EnableHAInBothEars(false);
        hearingLoss.EnableHearingLossInBothEars(false);

        // create database to hold target sentence lists
        database = new VRHINTDatabase(targetAudioPath, numLists, numSentences);

        // hold every SNR datapoint that goes into SRT calculation
        SNR = new List<float>[numTestLists];
        for(int i = 0; i < numTestLists; i++)
        {
            SNR[i] = new List<float>();
        }

        // hold SRT values for each list 
        eSRT = new List<float>();

        // hold hit/miss relation for each sentence (e.g. 5 hits, 1 miss on 6 word sentence: 5/6 hitQuote)
        hitQuote = new List<float>();

        // hold HINT condition for each list
        conditions = new List<hintConditions>();

        // hold order of sentence lists for test procedure
        listOrder = new List<int>();

        // hold indices of sentences from currentList
        listIndices = new List<int>();

        // copy practiceLists into new list
        List<int> tmp = new List<int>(practiceList);

        int cnt = 0;

        while(cnt < numTestLists)
        {
            int randList = Random.Range(1, numLists);

            // exclude practiceList
            if (randList == practiceList)
                continue;

            // exclude previously selected lists
            for(int i = 0; i < cnt; i++)
            {
                if (randList == listOrder[i])
                    continue;
            }

            listOrder.Add(randList);
            cnt++;

        }

        /*
        for (int i = 1; i <= numLists; i++)
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
                listOrder.Add(i);
            }
            
        }
        */

        feedbackManager.showFeedbackUI(false);

        // randomly sort test conditions and sentence lists with no direct repetitions
        createCounterBalancedTest();

        practiceMode = true;

        // keep track of current assets
        currentListIndex = practiceList;
        currentCondition = practiceCondition;

  
        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));

        currentSentenceIndex = Random.Range(0, listIndices.Count);
        Debug.Log("Sentence " + currentSentenceIndex + ": " + database.getSentenceString(currentListIndex, currentSentenceIndex));


        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // target & UI are always at front position
        levelManager.angularPosition(levelObjects.target, 0, objectDistance);
        levelManager.angularPosition(levelObjects.userInterface, 0, interfaceDistance, interFaceHeight);

        // VRHINT only uses dist1 in all conditions except 'quiet' (will be overwritten in this case)
        levelManager.setDistractorSettings(distractorSettings.dist1);

        audioManager.setDistractorAudio(levelObjects.distractor1, noise, true);

        ApplyTestConditions();

        // start playing again
        audioManager.startPlaying();

    }


    // assign order of lists and conditions based on userID through a latin square system
    // store the general order in a separate file
    private void importCounterBalancedTestSetup()
    {

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
                    conditions.Add(hintConditions.quiet);
                    break;
                case 1:
                    conditions.Add(hintConditions.noiseFront);
                    break;
                case 2:
                    conditions.Add(hintConditions.noiseLeft);
                    break;
                case 3:
                    conditions.Add(hintConditions.noiseRight);
                    break;
            }
            _tmp = tmp;
        }
        
        // randomize oder of sentence lists
        int n = listOrder.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = listOrder[k];
            listOrder[k] = listOrder[n];
            listOrder[n] = value;
        }

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
                break;
            case hintConditions.noiseLeft:
                levelManager.angularPosition(levelObjects.distractor1, 270, objectDistance);
                break;
            case hintConditions.noiseRight:
                levelManager.angularPosition(levelObjects.distractor1, 90, objectDistance);
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
        currentSentence = database.getSentenceWords(currentListIndex, currentSentenceIndex);
        sentenceLength = currentSentence.Length;
        // always start with the first word of the sentence, so set sentenceStart as true
        string[] rand = database.getRandomWords(wordOptions - 1, currentSentence[0], database.isCapital(currentSentence[0]), true);

        string[] test = new string[wordOptions];
        test[0] = currentSentence[0];
        rand.CopyTo(test, 1);


        feedbackManager.showFeedbackUI(true);
        feedbackManager.assignWordsToButtons(test);

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

            // first four sentences
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
                SNR[listCounter].Add(audioManager.getTalkerVolume());

                hitQuote.Add(_hitQuote);
                if (_hitQuote < 0.5f)
                {
                    audioManager.changeTalkerVolume(adaptiveSNRStep);
                }
                else
                {
                    audioManager.changeTalkerVolume(-adaptiveSNRStep);
                }
            }          
            
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


    void OnSessionDone()
    {
        Debug.Log("VRHINT procedure done!");

        UserManagement.selfReference.addTestResults(listOrder, conditions, hitQuote, hitQuote);

        SceneManager.LoadSceneAsync("VRMenuScene");

    }



    void OnContinue()
    {
        feedbackManager.showFeedbackUI(false);

        listIndices.Remove(currentSentenceIndex);

        if(practiceMode)
        {
            if (practiceRounds-- == 0)
            {
                Debug.Log("Leaving practice mode");
                OnListDone();
                return;
            }
                
        }


        if(listIndices.Count == 0)
        {
            OnListDone();
            return;
        }

        // randomly select next sentence (repetition impossibile due to removal of already played sentences)
        currentSentenceIndex = listIndices[Random.Range(0, listIndices.Count)];
        Debug.Log("Sentences remaining: " + listIndices.Count);
        Debug.Log("Sentence " + currentSentenceIndex + ": " + database.getSentenceString(currentListIndex, currentSentenceIndex));

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // start playing again
        audioManager.startPlaying();

    }

    void OnListDone()
    {
        Debug.Log("On List Done!");

        if(listIndices.Count > 0 && !practiceMode)
        {
            Debug.LogWarning("testList is not empty: " + listIndices.Count);
        }

        if(practiceMode)
        {
            practiceMode = false;
            listIndices.Clear();
        }
        else
        {
            // calculate average SRT
            float _SRT = 0.0f;
            for(int i = 0; i < SNR[listCounter].Count; i++)
            {
                _SRT += SNR[listCounter][i]; 
            }
            _SRT /= SNR[listCounter].Count;
            Debug.Log("List eSRT: " + _SRT);
            eSRT.Add(_SRT);
        }

        if(listCounter >= numTestLists - 1)
        {
            OnSessionDone();
            return;
        }


        currentCondition = conditions[listCounter];
        currentListIndex = listOrder[listCounter];
        Debug.Log("New condition: " + currentCondition + " new List: " + currentListIndex);

        listCounter++;

        listIndices.AddRange(System.Linq.Enumerable.Range(0, 20));        
        currentSentenceIndex = Random.Range(0, listIndices.Count);

        ApplyTestConditions();

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(database.getSentenceAudio(currentListIndex, currentSentenceIndex));

        // start playing again
        audioManager.startPlaying();

    }
}
