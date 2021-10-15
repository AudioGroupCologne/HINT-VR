using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] TrainingGameManager master;
    [SerializeField] LevelObjectManager levelObjects;
    [SerializeField] GameObject settings;

    public void TrainingGameSettingBtnHanlder(int index)
    {
        levelObjects.setLevelObjectPositions(index);
        levelObjects.showLevelObjects(true);

        // disable settingsUI
        settings.SetActive(false);

        // start TrainingGame once settings have been done
        master.OnStart();
    }

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }


}
