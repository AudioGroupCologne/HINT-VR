using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGameSettings : MonoBehaviour
{
    public DemoGameManager dmScript;

    public void DemoGameSettingBtnHanlder(int index)
    {
        switch (index)
        {
            case 0:
                dmScript.loadSameVoice();
                dmScript.setDistractorPosOne();
                break;
            case 1:
                dmScript.loadSameVoice();
                dmScript.setDistractorPosTwo();
                break;
            case 2:
                dmScript.loadDifferentVoice();
                dmScript.setDistractorPosOne();
                break;
            case 3:
                dmScript.loadDifferentVoice();
                // use this to also test Pos3
                dmScript.setDistractorPosThree();
                break;
        }

        dmScript.showObjects(true);

        // disable DemoGameSettings
        gameObject.SetActive(false);
    }

    public void DemoGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

}
