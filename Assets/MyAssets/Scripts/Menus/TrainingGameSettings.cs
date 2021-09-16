using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    public TrainingGameManager masterScript;

    public void TrainingGameSettingBtnHanlder(int index)
    {
        masterScript.setObjectPositions(index);
        masterScript.showObjects(true);
        masterScript.OnContinue();

        // disable DemoGameSettings
        gameObject.SetActive(false);
    }

    public void TrainingGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

}
