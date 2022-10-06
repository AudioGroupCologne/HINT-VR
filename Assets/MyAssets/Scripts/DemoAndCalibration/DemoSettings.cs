using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CustomTypes;
using CustomTypes.VRHINTTypes;

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

    public delegate void OnChangeVolume(float value);
    public OnChangeVolume onDistVolumeChange = delegate { Debug.Log("No onDistVolumeChange delegate set!"); };
    public OnChangeVolume onTargetVolumeChange = delegate { Debug.Log("No onTargetVolumeChange delegate set!"); };

    public delegate void OnStartTestMode(hintConditions cond);
    public OnStartTestMode onStartTestMode = delegate { Debug.Log("No onStartTestMode delegate set!"); };

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeDistVolume(2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeDistVolume(-2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeTargetVolume(2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeTargetVolume(-2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TestModeLeft();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            TestModeRight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TestModeFront();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            TestModeQuiet();
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

    public void ChangeDistVolume(float val)
    {
        onDistVolumeChange(val);
    }

    public void TestModeLeft()
    {
        onStartTestMode(hintConditions.noiseLeft);
    }

    public void TestModeRight()
    {
        onStartTestMode(hintConditions.noiseRight);
    }

    public void TestModeFront()
    {
        onStartTestMode(hintConditions.noiseFront);
    }

    public void TestModeQuiet()
    {
        onStartTestMode(hintConditions.quiet);
    }

    public void ChangeTargetVolume(float val)
    {
        onTargetVolumeChange(val);
    }

    public void ShowDemoSettings(bool show)
    {
        settings.SetActive(show);
    }

    public void Quit()
    {
        GameObject Listener = GameObject.Find("Listener");
        Listener.transform.parent = null;
        DontDestroyOnLoad(Listener);
        SceneManager.LoadSceneAsync("VRMenuScene");
    }

}
