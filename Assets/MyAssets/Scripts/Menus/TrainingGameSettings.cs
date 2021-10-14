using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] TrainingGameManager master;
    [SerializeField] LevelObjectManager levelObjects;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject results;

    public void TrainingGameSettingBtnHanlder(int index)
    {
        levelObjects.setLevelObjectPositions(index);
        levelObjects.showLevelObjects(true);

        // disable DemoGameSettings
        gameObject.SetActive(false);

        // start TrainingGame once settings have been done
        master.OnStart();
    }

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }



    // ### Refactor this to a sort of general UI/overlay for the scene...


    public void showResults()
    {
        settings.gameObject.SetActive(false);
        results.gameObject.SetActive(true);
    }


}
