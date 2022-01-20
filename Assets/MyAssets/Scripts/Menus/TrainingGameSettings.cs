using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] GameObject settings;

    public delegate void OnVoiceSelection(int voice);
    public OnVoiceSelection voiceCallback = delegate { Debug.Log("No voiceSelection delegate set!"); };


    // Mainly for debugging purposes
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Selected Harold");
            VoiceSelection(0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Selected Katy");
            VoiceSelection(1);
        }

    }


    // Button callback function: Harold "male" (0), Katy "female" (1)
    public void VoiceSelection(int voice)
    {
        // hide settings UI
        settings.SetActive(false);

        // start training game
        voiceCallback(voice);
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
