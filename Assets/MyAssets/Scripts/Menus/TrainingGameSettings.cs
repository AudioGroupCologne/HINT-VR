using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingGameSettings : MonoBehaviour
{

    //[SerializeField] settingsLevel setLvl = settingsLevel.targetVoiceSelection;
    [SerializeField] voiceSelection defaultVoice = voiceSelection.femaleVoice;

    [SerializeField] bool targetVoiceSelection = true;
    [SerializeField] bool distractorVoiceSelection = false;
    [SerializeField] bool distractorCountSelection = false;

    [SerializeField] GameObject settings;
    [SerializeField] GameObject topText;
    [SerializeField] GameObject voiceBtn1;
    [SerializeField] GameObject voiceBtn2;
    [SerializeField] GameObject distBtn1;
    [SerializeField] GameObject distBtn2;

    enum voiceSelection { maleVoice, femaleVoice};
    enum distracterSetting { oneDistractor, twoDistractors};

    
    public delegate void OnSettingsDone(int targetVoice, int distVoice, int distSetting);
    public OnSettingsDone settingsDoneCallback = delegate { Debug.Log("No settingsDone delegate set!"); };

    // simple state variable, required for keyboard controls
    private int settingsState = 0;

    private int targetVoiceSel;
    private int distVoiceSel;
    private int distSetting;

    public void Init()
    {
        // set default values
        targetVoiceSel = (int)defaultVoice;
        distVoiceSel = (int)defaultVoice;
        distSetting = (int)distracterSetting.twoDistractors;

        if (targetVoiceSelection)
        {
            ShowSelection(0);
        }
        else if (distractorVoiceSelection)
        {
            ShowSelection(1);
        }
        else if (distractorCountSelection)
        {
            ShowSelection(3);
        }
        else
        {
            settingsDoneCallback(targetVoiceSel, distVoiceSel, distSetting);
            settings.SetActive(false);
        }
    }

    // Mainly for debugging purposes
    void Update()
    {
        switch(settingsState)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.H))
                {
                    TargetVoiceSelection(0);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    TargetVoiceSelection(1);
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.H))
                {
                    DistractorVoiceSelection(0);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    DistractorVoiceSelection(1);
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    DistractorSelection(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    DistractorSelection(1);
                }
                break;
        }
    }

    void ShowSelection(int index)
    {
        switch(index)
        {
            case 0:
                // targetVoice
                topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Target Voice";
                distBtn1.SetActive(false);
                distBtn2.SetActive(false);
                voiceBtn1.SetActive(true);
                voiceBtn2.SetActive(true);
                break;
            case 1:
                // distractorVoice
                topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Distractor Voice";
                settingsState = 1;
                voiceBtn1.SetActive(false);
                voiceBtn2.SetActive(false);
                distBtn1.SetActive(false);
                distBtn2.SetActive(false);
                voiceBtn1.SetActive(true);
                voiceBtn2.SetActive(true);
                break;
            case 2:
                // distractorSettings
                topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Distractor Setting";
                settingsState = 2;
                distBtn1.SetActive(true);
                distBtn2.SetActive(true);
                voiceBtn1.SetActive(false);
                voiceBtn2.SetActive(false);
                break;
        }
    }

    // Button callback function: Harold "male" (0), Katy "female" (1)

    public void VoiceSelectionCallback(int voice)
    {
        switch(settingsState)
        {
            case 0:
                TargetVoiceSelection(voice);
                break;
            case 1:
                DistractorVoiceSelection(voice);
                break;
            default:
                Debug.LogError("Invalid VoiceSelection Event: " + settingsState);
                break;
        }
    }
    void TargetVoiceSelection(int voice)
    {

        targetVoiceSel = voice;

        if(!distractorVoiceSelection && !distractorCountSelection)
        {
            settingsDoneCallback(targetVoiceSel, targetVoiceSel, (int)distracterSetting.twoDistractors);
            settings.SetActive(false);
            return;
        }
       
        if(distractorVoiceSelection)
        {
            ShowSelection(1);
        }
        else if(distractorCountSelection)
        {
            ShowSelection(2);
        }
        
    }

    void DistractorVoiceSelection(int voice)
    {

        distVoiceSel = voice;

        if (!distractorCountSelection)
        {
            settingsDoneCallback(targetVoiceSel, targetVoiceSel, (int)distracterSetting.twoDistractors);
            settings.SetActive(false);
            return;
        }

        ShowSelection(2);
    }

    public void DistractorSelection(int dist)
    {
        distSetting = dist;

        // start training game
        settingsDoneCallback(targetVoiceSel, distVoiceSel, distSetting);
        settings.SetActive(false);

        // hide settings UI
        settings.SetActive(false);
    }

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("VRMenuScene");
    }


    // DEPRECATED: Old option to allow for non-spatialized control group
    /*
    void SetupSelection(int index)
    {

        Debug.Log("Setup index is deprecated!!!");

        switch (index)
        {
            case 1:
                // Setup A: same direction (control group)
                Debug.Log("Same direction (control) [1]");
                levelObjects.setLevelObjectPositions(index);
                break;
            case 2:
                // Setup B: different direction
                Debug.Log("Differenct directions [2]");
                levelObjects.setLevelObjectPositions(index);
                break;
            default:
                Debug.LogError("Invalid setupOption: " + index);
                return;
        }

        levelObjects.showLevelObjects(true);
        
    }
    */




}
