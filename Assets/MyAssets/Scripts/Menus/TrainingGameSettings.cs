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
        // highlight selection that was made...
        // make other buttons not selectable before voice was chosen?
    }

    public void SetupSelection(int index)
    {
        switch(index)
        {
            case 0:
                // Setup A: same direction (control group)
                levelObjects.setLevelObjectPositions(index);
                break;
            case 1:
                // Setup B: different direction
                levelObjects.setLevelObjectPositions(index);
                break;
        }

        levelObjects.showLevelObjects(true);
        settings.SetActive(false);
        master.OnStart();
    }

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }


}
