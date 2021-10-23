using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] TrainingGameManager master;
    [SerializeField] LevelObjectManager levelObjects;
    [SerializeField] GameObject settings;

    private void Start()
    {
        settings.SetActive(true);
    }

    public void VoiceSelection(int index)
    {
        Debug.Log("Add option to load mutliple voices...");

        SetupSelection(UserManagement.selfReference.getUserGroup());

        settings.SetActive(false);

        master.OnStart(1, index);

    }

    void SetupSelection(int index)
    {

        switch(index)
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

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }


}
