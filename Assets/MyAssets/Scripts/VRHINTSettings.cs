using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomTypes.VRHINTTypes;

public class VRHINTSettings : MonoBehaviour
{

    [SerializeField] GameObject settings;
    [SerializeField] GameObject settingsGroup1;
    [SerializeField] GameObject settingsGroup2;


    public delegate void OnTestOrderSetting(testOrder order);
    public OnTestOrderSetting OnTestOrderCallback = delegate { Debug.Log("No testOrder delegate set!"); };

    public delegate void OnSettingsDone(feedbackSettings setting);
    public OnSettingsDone OnSettingsDoneCallback = delegate { Debug.Log("No settingsDone delegate set!"); };

    private int settingState = 0;

    // Start is called before the first frame update
    void Start()
    {
        settings.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!settings.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ButtonHandler(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ButtonHandler(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ButtonHandler(3);
        }
        
    }


    public void ButtonHandler(int setting)
    {

        if (settingState == 0)
        {
            switch (setting)
            {
                case 1:
                    OnTestOrderCallback(testOrder.first);
                    break;
                case 2:
                    OnTestOrderCallback(testOrder.second);
                    break;
                default:
                    Debug.Log("Invalid selection!");
                    return;
            }

            settingState = 1;
            settingsGroup1.SetActive(false);
            settingsGroup2.SetActive(true);

        }

        else if(settingState == 1)
        {
            switch (setting)
            {
                case 1:
                    OnSettingsDoneCallback(feedbackSettings.classic);
                    break;
                case 2:
                    OnSettingsDoneCallback(feedbackSettings.wordSelection);
                    break;
                case 3:
                    OnSettingsDoneCallback(feedbackSettings.comprehensionLevel);
                    break;
                default:
                    Debug.Log("Invalid selection!");
                    return;
            }

            settingState = 0;
            settingsGroup1.SetActive(true);
            settingsGroup2.SetActive(false);
            ShowSettings(false);

        }
        
    }

    public void ShowSettings(bool show)
    {
        settings.SetActive(show);
    }

    public void QuitButtonCallback()
    {
        SceneManager.LoadSceneAsync("VRMenuScene");
    }
}
