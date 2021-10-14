using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;


public partial class TrainingGameManager : MonoBehaviour
{

    AudioManager audioManager;
    [SerializeField] int gameLength;
    private int roundsPlayed = 0;
    private bool practiceMode = true;
    private int practiveRounds = 0;

    [SerializeField] wordSelectionScript wordSel;
    // ### refactor this!
    [SerializeField] TrainingGameSettings settingsUI;


    private Sentence sent;
    private LiSN_database lisnData;

    private bool unsureHandling = false;


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


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    public void OnPlayingDone()
    {
        // do something to randonly select a group based on the options
        // there are always 3 selectable groups within each word list so do: 0,1,2
        // simply exclude non-selectable groups from API?
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
        // decrease SNR by reducing talker volume by -1.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, -1.5f);
    }

    // Called when the player selected a false word option
    // Increase SNR, play 'false' sound
    public void OnMiss()
    {
        // stop distracter (do this wihtin audio manager?)
        audioManager.playOnMiss();
        // improve SNR by increasing talker volume by 2.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, 2.5f);
    }

    public void OnUnsure()
    {   
        // improve SNR by increasing talker volume by 1.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, 1.5f);

        if(unsureHandling)
        {
            unsureHandling = false;
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

        // play same sentence again (this does not count as a separate round! don't increase 'roundsPlayed')
        if (unsureHandling)
        {
            // start playing again
            audioManager.startPlaying();

            return;
        }


        // generate a new sentence
        sent.createSentence(lisnData);

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(sent.audio);


        // start playing again
        audioManager.startPlaying();

        roundsPlayed++;

    }

    private void OnSessionDone()
    {
        Debug.Log("Training session done!");
        settingsUI.gameObject.SetActive(true);
        settingsUI.showResults();
    }


}
