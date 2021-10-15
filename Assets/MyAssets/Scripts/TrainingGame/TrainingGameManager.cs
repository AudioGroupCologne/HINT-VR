using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public partial class TrainingGameManager : MonoBehaviour
{

    AudioManager audioManager;
    // number of sentences (excluding practice rounds and repetitions) to be played within a session
    [SerializeField] int gameLength;
    // min number of practice rounds to be played (stops @ first mistake after a correct answer)
    [SerializeField] int min_practiceRounds;
    // number of consecutive correct answers to achieve a reward
    [SerializeField] int rewardHits;

    // other components (maybe refactor this a bit)
    [SerializeField] wordSelectionScript wordSel;
    [SerializeField] TrainingGameSettings settingsUI;
    [SerializeField] OverlayManager overlayScript;


    private Sentence sent;
    private LiSN_database lisnData;


    /// Control variables
    // start each session with practice mode
    private bool practiceMode = true;
    // keep track of practice rounds/sentences
    private int practiceRounds = 0;
    // keep track of rounds/sentences played within current session
    private int roundsPlayed = 0;
    // keep track of current consecutive hits
    private int rewardCount = 0;
    // keep track of rewards achieved within current session
    private int currentRewards = 0;
    // repeat last sentence if 'unsure' was selected (do this only once!)
    private bool repeatSentence = false;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Training Game");
        audioManager = GetComponent<AudioManager>();

        // create LiSN_database object
        lisnData = new LiSN_database(1);

        // create sentence object
        sent = new Sentence(lisnData.getSentenceLen());

        // make sure to disable UI at load.
        wordSel.showWordSelectionUI(false);

    }

    private void OnSessionDone()
    {
        Debug.Log("Training session done!");
        settingsUI.gameObject.SetActive(true);
        settingsUI.showResults();
    }


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    public void OnPlayingDone()
    {

        if (repeatSentence)
        {
            wordSel.showWordSelectionUI(true);
            Debug.Log("Repeat sentence...");
            return;
        }
 
        int randGroup = Random.Range(0, 3);
        string[] words;
        Sprite[] icons;

        lisnData.getSelectableWords(randGroup, 4, sent.getSelectableWordIndex(randGroup), out words, out icons);


        Debug.Log("Correct word: " + sent.getSelectableWordString(randGroup));

        // show wordSelection UI elements
        wordSel.startWordSelection(words, icons);

    }

    public void OnStart()
    {
        // generate a new sentence
        sent.createSentence(lisnData);

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(sent.audio);

        // start playing again
        audioManager.startPlaying();
    }

    /// Word Selection UI Callbacks
    public void OnHit()
    {

        audioManager.playOnHit();

        repeatSentence = false;

        if (practiceMode)
        {
            // decrease SNR by reducing talker volume by 3.0 dB
            audioManager.changeLevel(AudioManager.source.talkerSrc, -3.0f);
            return;
        }

        // decrease SNR by reducing talker volume by -1.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, -1.5f);


        DataStorage.TrainingGame_Hits++;

        if (++rewardCount >= rewardHits)
        {
            Debug.Log("Player Reward achieved!");
            audioManager.playOnReward();
            overlayScript.showReward(currentRewards++);
            // reset rewardCount
            rewardCount = 0;
        }

    }

    // Called when the player selected a false word option
    // Increase SNR, play 'false' sound
    public void OnMiss()
    {

        audioManager.playOnMiss();

        repeatSentence = false;

        if (practiceMode)
        {
            // decrease SNR by reducing talker volume by 3.0 dB
            audioManager.changeLevel(AudioManager.source.talkerSrc, -3.0f);
            if (practiceRounds >= min_practiceRounds)
            {
                Debug.Log("Leave practive mode");
                practiceMode = false;
            }

            return;
        }


        // improve SNR by increasing talker volume by 2.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, 2.5f);

        DataStorage.TrainingGame_Misses++;

        rewardCount = 0;

    }

    public void OnUnsure()
    {
        //audioManager.playOnUnsure();

        if (practiceMode)
        {
            // decrease SNR by reducing talker volume by 3.0 dB
            audioManager.changeLevel(AudioManager.source.talkerSrc, -3.0f);
            if (practiceRounds >= min_practiceRounds)
            {
                Debug.Log("Leave practive mode");
                practiceMode = false;
            }
            return;
        }
  

        // improve SNR by increasing talker volume by 1.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, 1.5f);
        

        if(!repeatSentence)
        {
            Debug.Log("Unsure: repeat sentence with increase SNR");
            repeatSentence = true;

            // either give a small delay or wait for another user input...
            
            // this requires a CoRoutine...
            //yield WaitForSeconds(1);

            // start playing again
            audioManager.startPlaying();

            // hide wordSelection UI elements
            wordSel.showWordSelectionUI(false);
        }
        else
        {
            repeatSentence = false;
        }

        //OnContinue();

    }

    public void OnContinue()
    {
        // set buttons back to their default color
        wordSel.reset_buttons_colors();

        // hide wordSelection UI elements
        wordSel.showWordSelectionUI(false);

        if (roundsPlayed >= gameLength)
        {
            OnSessionDone();
            return;
        }

        if(!repeatSentence)
        {
            // generate a new sentence
            sent.createSentence(lisnData);

            // move new sentence audio to audioManager
            audioManager.setTargetSentence(sent.audio);

            if(practiceMode)
            {
                practiceRounds++;
                Debug.Log("Practice round: " + practiceRounds + " of " + min_practiceRounds + " (min)");
            }
            else
            {
                roundsPlayed++;
            }
            
        }

        // start playing again
        audioManager.startPlaying();

    }




}
