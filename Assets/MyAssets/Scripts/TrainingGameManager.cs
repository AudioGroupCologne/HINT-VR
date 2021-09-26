using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;


public partial class TrainingGameManager : MonoBehaviour
{

    AudioManager audioManager;
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] wordSelectionScript wordSel;

    [SerializeField] GameObject TalkerObj;
    [SerializeField] GameObject DistractorObj;
    
    [SerializeField] Vector3 talkerPos;
    [SerializeField] Vector3 distractorPos1;
    [SerializeField] Vector3 distractorPos2;
    [SerializeField] Vector3 distractorPos3;

    private Sentence sent;
    private LiSN_database lisnData;


    private bool sentenceReady = false;
    private bool sceneEntered = false;
    private bool isPlaying = false;

    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Training Game");
        audioManager = GetComponent<AudioManager>();

        // create LiSN_database object
        lisnData = new LiSN_database(1);

        // create sentence object
        sent = new Sentence(lisnData.getLen());

        // make sure to disable UI at load.
        wordSel.showWordSelectionUI(false);

        showObjects(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!sceneEntered)
            return;

        // nothing is being played and a new sentence is available
        if (!isPlaying && sentenceReady)
        {
           // if (Input.GetKeyDown(KeyCode.E))
           // {
                audioManager.startPlaying();
                isPlaying = true;
           // }
        }
    }


    /// Audio Manager Callbacks
    // when audio manager has finished playing, reset control variable
    public void onPlayingDone()
    {
        isPlaying = false;

        // a new sentence has to be created, after user input
        sentenceReady = false;

        // show wordSelection UI elements
        wordSel.startWordSelection(1);

    }

    /// Word Selection UI Callbacks
    public void OnHit()
    {
        audioManager.playOnHit();
        // decrease SNR by reducing talker volume by -2 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, -2.0f);
    }

    // Called when the player selected a false word option
    // Increase SNR, play 'false' sound
    public void OnMiss()
    {
        // stop distracter (do this wihtin audio manager?)
        audioManager.playOnMiss();
        // improve SNR by increasing talker volume by 1.5 dB
        audioManager.changeLevel(AudioManager.source.talkerSrc, 1.5f);
    }

    public void OnContinue()
    {
        // set buttons back to their default color
        wordSel.reset_buttons_colors();

        // hide wordSelection UI elements
        wordSel.showWordSelectionUI(false);

        // generate a new sentence
        sent.createSentence(lisnData);

        // move new sentence audio to audioManager
        audioManager.setTargetSentence(sent.audio);

        // set 'sentenceReady' flag
        sentenceReady = true;

    }


    /**
     * Return 'count' words of a group (determined by 'wordIndex') from the current database.
     * The "correct" word (which is used in the current sentence) will always be the first one (index 0 of returned string array)
     * There are no doubling of words allowed.
     * 
     * @param wordIndex - determines word group by index (e.g. 'adjective' or 'object') 
     * @param count     - determines how many words shall be returned
     * return           - array of strings, with correct word a index 0
     *                    OR: null if wordIndex or count was invalid.
     * 
     */
    public string[] getUserWordSelection(int wordIndex, int count)
    {
        // hold selected strings to be returned
        string[] retStr = new string[count];
        // keep track of already selected words via their indices
        int[] wordIxs = new int[count];

        // invalid parameters (also exclude 'the')
        if (wordIndex >= lisnData.getLen() || count >= lisnData.getOptions())
        {
            return null;
        }

        // write the correct word at index 0
        retStr[0] = sent.getWordFromSentence(wordIndex);
        wordIxs[0] = sent.getWordIxFromSentence(wordIndex);

        retStr = lisnData.getWordsByGroup(wordIndex, count, wordIxs[0]);

        return retStr;
    }

    public Sprite[] getUserIconSelection(int wordIndex, string[] words)
    {
        Sprite[] sprites = new Sprite[words.Length];

        for(int i = 0; i < words.Length; i++)
        {
            sprites[i] = lisnData.getIcon(words[i]);
        }

        return sprites;
    }

    public string[] getCurrentSentence()
    {
        return sent.getSentenceString();
    }


    // scene setup
    public void setObjectPositions(int selector)
    {
        // set position of TalkerObj based on MainCameras position
        TalkerObj.transform.position = PlayerCamera.transform.position + talkerPos;
        // get rotation of camera
        Vector3 rot = Quaternion.identity.eulerAngles;
        // turn by 180 degree (object shall face camera, not look into the same direction)
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        // apply rotation to object
;       TalkerObj.transform.rotation = Quaternion.Euler(rot);

        switch (selector)
        {
            case 0:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos1;
                break;
            case 1:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos2;
                break;
            case 2:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos3;
                break;
        }
    }

    public void showObjects(bool show)
    {
        TalkerObj.SetActive(show);
        DistractorObj.SetActive(show);
        sceneEntered = show;
    }



    /// Create an audio player manager:
    /// Manages when to play and when to stop
    /// e.g. start target sentence n seconds after distracter
    /// stop distracter nn seconnds after target sentence
    /// generate "finishedplaying" event (used to show word selection UI)
    /// play hit/miss
    
    // Called when player selected the correct word option.
    // Decrese SNR by x dB
    // play 'success' sound




    // show "press E to play sentence" via UI elements 
    // hide UI when 'E' is pressed (checked from Update)
    
    // call audioManager and let talker/distractor start

    // callback when finished playing both sentences (?) audioManager call public function from here (?)

    // show wordSelection UI

    // WS1-4 or "not sure"
    // alter SNR based on hit, miss or unsure
    // play sound based on hit, miss or unsure
    // store result


    // generate next sentence
    // show "press E" again (UI)




}
