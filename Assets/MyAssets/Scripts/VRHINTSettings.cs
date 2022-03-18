using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomTypes.VRHINTTypes;

public class VRHINTSettings : MonoBehaviour
{

    [SerializeField] GameObject settings;

    public delegate void OnSettingsDone(feedbackSettings setting);
    public OnSettingsDone OnSettingsDoneCallback = delegate { Debug.Log("No settingsDone delegate set!"); };

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

        if(Input.GetKeyDown(KeyCode.Alpha1))
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
        switch(setting)
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
        }
        ShowSettings(false);
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
