using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGameSettings : MonoBehaviour
{

    public GameObject Talker;
    public GameObject Distractor;

    public void DemoGameSettingBtnHanlder(int index)
    {
        switch (index)
        {
            case 0:
                loadSameVoice();
                setDistractorPosOne();
                break;
            case 1:
                loadSameVoice();
                setDistractorPosTwo();
                break;
            case 2:
                loadDifferentVoice();
                setDistractorPosOne();
                break;
            case 3:
                loadDifferentVoice();
                // use this to also test Pos3
                setDistractorPosThree();
                break;
        }

        // disable DemoGameSettings
        gameObject.SetActive(false);
    }

    public void DemoGameSettingsQuitBtn()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }

    // for now: same as different voice due to lack of assets...
    private void loadSameVoice()
    {

    }

    private void loadDifferentVoice()
    {

    }

    // 0 deg azimuth
    private void setDistractorPosOne()
    {
        // slightly belwo talker
        Distractor.transform.position = new Vector3(0, 2, 10);
    }

    // +90 deg azimuth
    private void setDistractorPosTwo()
    {
        Distractor.transform.position = new Vector3(10, 2, 0);
    }

    // -90 deg azimuth
    private void setDistractorPosThree()
    {
        Distractor.transform.position = new Vector3(-10, 2, 0);
    }
}
