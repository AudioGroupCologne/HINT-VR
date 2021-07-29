using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool EnableStaticSource;
    public bool EnableMovingSource;
    public Toggle toggleStatic;
    public Toggle toggleMoving;

    bool MovingCubeState;
    bool StaticCubeState;

    void Start()
    {
        MovingCubeState = (PlayerPrefs.GetInt("MovingOn") == 1) ? true : false;
        StaticCubeState = (PlayerPrefs.GetInt("StaticOn") == 1) ? true : false;

        toggleStatic.isOn = StaticCubeState;
        toggleMoving.isOn = MovingCubeState;
    }

    public void ToggleStaticChanged()
    {
        // get value from ToggleComponent
        EnableStaticSource = toggleStatic.isOn;
        // Store ToggleValue in PlayerPrefs to access it in other scenes
        PlayerPrefs.SetInt("StaticOn", EnableStaticSource? 1 : 0);
    }

    public void ToggleMovingChanged()
    {
        EnableMovingSource = toggleMoving.isOn;
        PlayerPrefs.SetInt("MovingOn", EnableMovingSource? 1 : 0);
    }
}
