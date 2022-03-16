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
            ButtonHandler(feedbackSettings.classic);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ButtonHandler(feedbackSettings.wordSelection);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ButtonHandler(feedbackSettings.comprehensionLevel);
        }
    }


    public void ButtonHandler(feedbackSettings setting)
    {

        OnSettingsDoneCallback(setting);
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
