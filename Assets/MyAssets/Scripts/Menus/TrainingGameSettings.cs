using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] GameObject settings;
    [SerializeField] GameObject topText;
    [SerializeField] GameObject voiceBtn1;
    [SerializeField] GameObject voiceBtn2;
    [SerializeField] GameObject distBtn1;
    [SerializeField] GameObject distBtn2;

    public delegate void OnSettingsDone(int targetVoice, int distVoice, int distSetting);
    public OnSettingsDone settingsDoneCallback = delegate { Debug.Log("No settingsDone delegate set!"); };

    private int targetVoiceSel = -1;
    private int distVoiceSel = -1;
    private int distSetting = -1;

    void Start()
    {
        topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Target Voice";
        distBtn1.SetActive(false);
        distBtn2.SetActive(false);
        voiceBtn1.SetActive(true);
        voiceBtn2.SetActive(true);
    }

    // Mainly for debugging purposes
    void Update()
    {
        if(targetVoiceSel == -1)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                TargetVoiceSelection(0);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                TargetVoiceSelection(1);
            }
        }
        else if (distVoiceSel == -1)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                DistractorVoiceSelection(0);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                DistractorVoiceSelection(1);
            }
        }
        else if (distSetting == -1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DistractorSelection(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DistractorSelection(1);
            }
        }

    }


    // Button callback function: Harold "male" (0), Katy "female" (1)
    public void TargetVoiceSelection(int voice)
    {

        targetVoiceSel = voice;

        topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Distractor Voice";
    }

    public void DistractorVoiceSelection(int voice)
    {

        distVoiceSel = voice;

        voiceBtn1.SetActive(false);
        voiceBtn2.SetActive(false);
        distBtn1.SetActive(true);
        distBtn2.SetActive(true);

        topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Distractor Setting";
    }

    public void DistractorSelection(int dist)
    {
        distSetting = dist;

        // hide settings UI
        settings.SetActive(false);

        // start training game
        settingsDoneCallback(targetVoiceSel, distVoiceSel, distSetting);
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
