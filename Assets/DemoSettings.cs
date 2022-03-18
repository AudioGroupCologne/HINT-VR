using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using CustomTypes;

public class DemoSettings : MonoBehaviour
{

    [SerializeField] GameObject settings;
    [SerializeField] Toggle targetAudio;
    [SerializeField] Toggle distAudio;
    [SerializeField] Toggle distMove;

    public delegate void OnDistPosition(levelPositions pos);
    public OnDistPosition onDistPositionCallback = delegate { Debug.Log("No onDistPosition delegate set!"); };

    public delegate void OnToggleAudio(bool enabled);
    public OnToggleAudio onToggleTargetCallback = delegate { Debug.Log("No onToggleTarget delegate set!"); };
    public OnToggleAudio onToggleDistCallback = delegate { Debug.Log("No onToggleDist delegate set!"); };
    public OnToggleAudio onToggleDistMoveCallback = delegate { Debug.Log("No onToggleDistMove delegate set!"); };


    // Update is called once per frame
    void Update()
    {
        if (!settings.activeSelf)
            return;

        if(Input.GetKeyDown(KeyCode.T))
        {
            targetAudio.isOn = !targetAudio.isOn;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            distAudio.isOn = !distAudio.isOn;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            distMove.isOn = !distMove.isOn;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            DistPositions(0);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            DistPositions(1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DistPositions(2);
        }
    }

    public void DistPositions(int button_ix)
    {
        switch(button_ix)
        {
            case 0:
                onDistPositionCallback(levelPositions.front);
                break;
            case 1:
                onDistPositionCallback(levelPositions.left);
                break;
            case 2:
                onDistPositionCallback(levelPositions.right);
                break;
        }
    }

    public void ToggleTargetAudio()
    {
        onToggleTargetCallback(targetAudio.isOn);
        
    }

    public void ToggleDistAudio()
    {
        Debug.Log("ToggleDistAudio: " + distAudio.isOn);
        onToggleDistCallback(distAudio.isOn);
    }

    public void ToggleDistMove()
    {
        onToggleDistMoveCallback(distMove.isOn);
    }

    public void ShowDemoSettings(bool show)
    {
        settings.SetActive(show);
    }


}
