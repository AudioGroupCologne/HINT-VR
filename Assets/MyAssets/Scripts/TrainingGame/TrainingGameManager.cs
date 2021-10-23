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
    [SerializeField] TrainingGameSettings settings;
    [SerializeField] wordSelectionScript wordSel;
    [SerializeField] ResultManager resultManager;
    [SerializeField] RewardManager rewardManager;
    


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
    // repeat last sentence if 'unsure' was selected (do this only once!)
    private bool repeatSentence = false;

    // SNR ratio stored for each round (average over whole session will be used)
    private float[] SNR_values;
    private int hits = 0;
    private int misses = 0;
    private int rewards = 0;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Training Game");
        audioManager = GetComponent<AudioManager>();

        // create LiSN_database object
        lisnData = new LiSN_database(1);

        // create sentence object
        sent = new Sentence(lisnData.getSentenceLen());

        SNR_values = new float[gameLength];

        // make sure to disable UI at load.
        wordSel.showWordSelectionUI(false);

        // show settings menu
        settings.gameObject.SetActive(true);

    }

    private void OnSessionDone()
    {
        Debug.Log("Training session done!");

        // calculate average SNR of the session
        float average_SNR = 0;
        for (int i = 0; i < roundsPlayed; i++)
        {
            average_SNR += SNR_values[i];
        }
        average_SNR /= roundsPlayed;
        // set current session data onto result UI
        resultManager.setTrainingGameResults(average_SNR, rewards, hits, misses, roundsPlayed);
        // show result UI
        resultManager.showResults();
        // store session data in userManagement
        UserManagement.selfReference.addUserResults(average_SNR, rewards);

    }


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    public void OnPlayingDone()
    {

        Debug.Log("OnPlayingDone");

        if (repeatSentence)
        {
            wordSel.showWordSelectionUI(true);
            Debug.Log("Repeat sentence...");
            return;
        }

        int randGroup = Random.Range(0, 3);
        string[] words;
        Sprite[] icons;


        if (!practiceMode)
        {
            SNR_values[roundsPlayed-1] = audioManager.getLevel(audioManager.talkerVol);
            Debug.Log("Stored SNR: " + SNR_values[roundsPlayed-1] + " round: " + roundsPlayed);
        }

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
            audioManager.changeLevel(audioManager.talkerVol, -3.0f);
            return;
        }

        // decrease SNR by reducing talker volume by -1.5 dB
        audioManager.changeLevel(audioManager.talkerVol, -1.5f);

        hits++;
        rewardCount++;
        Debug.Log("Hits: " + hits + " Streak: " + rewardCount);

            

        if (rewardCount >= rewardHits)
        {
            Debug.Log("Player Reward achieved!");
            audioManager.playOnReward();
            rewardManager.showReward(rewards++);
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
            audioManager.changeLevel(audioManager.talkerVol, -3.0f);
            if (practiceRounds >= min_practiceRounds)
            {
                Debug.Log("Leave practive mode");
                practiceMode = false;
            }

            return;
        }

        misses++;

        // improve SNR by increasing talker volume by 2.5 dB
        audioManager.changeLevel(audioManager.talkerVol, 2.5f);

        rewardCount = 0;

    }

    public void OnUnsure()
    {

        if (practiceMode)
        {
            // decrease SNR by reducing talker volume by 3.0 dB
            audioManager.changeLevel(audioManager.talkerVol, -3.0f);
            if (practiceRounds >= min_practiceRounds)
            {
                Debug.Log("Leave practive mode");
                practiceMode = false;
            }
            return;
        }
  

        // improve SNR by increasing talker volume by 1.5 dB
        audioManager.changeLevel(audioManager.talkerVol, 1.5f);
        

        if(!repeatSentence)
        {
            Debug.Log("Unsure: repeat sentence with increase SNR");
            repeatSentence = true;

            // start playing again
            audioManager.startPlaying();

            // hide wordSelection UI elements
            wordSel.showWordSelectionUI(false);
        }
        else
        {
            repeatSentence = false;
        }

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
                Debug.Log("Round: " + roundsPlayed + " of " + gameLength);
            }
            
        }

        // start playing again
        audioManager.startPlaying();

    }

}
