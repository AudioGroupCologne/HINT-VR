using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameSettings : MonoBehaviour
{
    [SerializeField] TrainingGameManager masterScript;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject results;

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



    // ### Refactor this to a sort of general UI/overlay for the scene...


    public void showResults()
    {
        settings.gameObject.SetActive(false);
        results.gameObject.SetActive(true);
    }


}
