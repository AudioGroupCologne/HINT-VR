using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGameSettings : MonoBehaviour
{
    public DemoGameManager dmScript;

    public void DemoGameSettingBtnHanlder(int index)
    {
        dmScript.setObjectPositions(index);
        dmScript.showObjects(true);

        // disable DemoGameSettings
        gameObject.SetActive(false);
    }

    public void DemoGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

}
