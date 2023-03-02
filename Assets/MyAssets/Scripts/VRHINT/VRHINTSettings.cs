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


    public delegate void onSettingsDone(testOrder order, feedbackSettings setting);
    public onSettingsDone OnSettingsDone = delegate { Debug.Log("No settingsDone delegate set!"); };

    private int settingState = 0;
    private testOrder _order;

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
            SettingsUIHandler(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SettingsUIHandler(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SettingsUIHandler(3);
        }
        
    }


    public void SettingsUIHandler(int selection)
    {

        if (settingState == 0)
        {
            TestOrder(selection);
            settingState = 1;
            settingsGroup1.SetActive(false);
            settingsGroup2.SetActive(true);
        }

        else if(settingState == 1)
        {
            FeedbackSystem(selection);
            settingState = 0;
            settingsGroup1.SetActive(true);
            settingsGroup2.SetActive(false);
            ShowSettings(false);
        }
    
    }

    private void TestOrder(int selection)
    {
        switch (selection)
        {
            case 1:
                _order = testOrder.first;
                break;
            case 2:
                _order = testOrder.second;
                break;
            default:
                Debug.Log("Invalid selection!");
                return;
        }
    }

    private void FeedbackSystem(int selection)
    {
        switch (selection)
        {
            case 1:
                OnSettingsDone(_order, feedbackSettings.classic);
                break;
            case 2:
                OnSettingsDone(_order, feedbackSettings.wordSelection);
                break;
            case 3:
                OnSettingsDone(_order, feedbackSettings.classicDark);
                break;
            default:
                Debug.Log("Invalid selection!");
                return;
        }
    }

    public void ShowSettings(bool show)
    {
        settings.SetActive(show);

        // make sure that the correct sub-UI component is shown
        if(settingState == 0)
        {
            settingsGroup1.SetActive(true);
            settingsGroup2.SetActive(false);
        }
        else if(settingState == 1)
        {
            settingsGroup1.SetActive(false);
            settingsGroup2.SetActive(true);
        }
        

    }

    public void QuitButtonCallback()
    {
        SceneManager.LoadSceneAsync("VRMenuScene");
    }
}
