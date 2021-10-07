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

        showObjects(false);
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

        // start playing again
        audioManager.startPlaying();

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
    }

}
