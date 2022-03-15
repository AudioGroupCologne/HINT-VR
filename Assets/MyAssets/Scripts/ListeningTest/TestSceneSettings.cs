using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CustomTypes.TestSceneTypes;


public class TestSceneSettings : MonoBehaviour
{

    public delegate void OnSettingsDone(experiments exp);
    public OnSettingsDone settingsDoneCallback = delegate { Debug.Log("No settingsDone delegate set!"); };


    [SerializeField] GameObject settings;
    [SerializeField] GameObject topText;
    [SerializeField] GameObject expBtn1;
    [SerializeField] GameObject expBtn2;


    enum settingScreen { experiments };
    

    // state variable is required for managing mutliple setting screen with keyboard support
    private settingScreen state = settingScreen.experiments;


    public void Init()
    {
        ShowSettings(state);
        settings.gameObject.SetActive(true);
    }

    // Mainly for debugging purposes
    void Update()
    {
        switch (state)
        {
            case settingScreen.experiments:
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    experimentSelection(experiments.experiment1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    experimentSelection(experiments.experiment2);
                }
                break;
            default:
                Debug.LogError("Invalid settings option: " + state);
                break;
        }
    }

    void ShowSettings(settingScreen option)
    {
        switch (option)
        {
            case settingScreen.experiments:
                topText.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose Experiment";
                expBtn1.SetActive(true);
                expBtn2.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid settingScreen selection: " + option);
                break;

        }
    }

    public void experimentSelection(experiments exp)
    {
        settingsDoneCallback(exp);
    }

    // ToDo: Move this to sceneManager (make this the only component to be allowed to load or unload scenes!)
    public void QuitCallback()
    {
        SceneManager.LoadSceneAsync("VRMenuScene");
    }


}

