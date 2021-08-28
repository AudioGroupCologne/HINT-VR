using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle toggleStatic;
    public Toggle toggleMoving;
    public Toggle toggleVarRadius;

    bool MovingCubeState;
    bool StaticCubeState;
    bool VarRadState;

    void Start()
    {
        MovingCubeState = (PlayerPrefs.GetInt("MovingOn") == 1) ? true : false;
        StaticCubeState = (PlayerPrefs.GetInt("StaticOn") == 1) ? true : false;
        VarRadState = (PlayerPrefs.GetInt("VarRadOn") == 1) ? true : false;

        toggleStatic.isOn = StaticCubeState;
        toggleMoving.isOn = MovingCubeState;

        if (MovingCubeState)
        {
            toggleVarRadius.isOn = VarRadState;
        }
    }

    public void ToggleStaticChanged()
    {
        // Get isOn from Toggle and store it in PlayerPrefs to access it in other scenes
        PlayerPrefs.SetInt("StaticOn", toggleStatic.isOn ? 1 : 0);
    }

    public void ToggleMovingChanged()
    {
        MovingCubeState = toggleMoving.isOn;
        PlayerPrefs.SetInt("MovingOn", toggleMoving.isOn ? 1 : 0);
        // VarRadius shall only be active, when MovingCube is enabled!
    }

    public void ToggleVarRadChanged()
    {
        PlayerPrefs.SetInt("VarRadOn", toggleVarRadius.isOn ? 1 : 0);
    }

}
