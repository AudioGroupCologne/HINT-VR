using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFeedback : MonoBehaviour
{
    public delegate void OnGood();
    public OnGood onGoodCallback = delegate { Debug.Log("No onGood delegate set!"); };
    public delegate void OnMedium();
    public OnMedium onMediumCallback = delegate { Debug.Log("No onMedium delegate set!"); };
    public delegate void OnBad();
    public OnBad onBadCallback = delegate { Debug.Log("No onBad delegate set!"); };


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ButtonHandler(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            ButtonHandler(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ButtonHandler(2);
        }
    }

    public void ButtonHandler(int button_id)
    {
        switch(button_id)
        {
            case 0:
                onGoodCallback();
                break;
            case 1:
                onMediumCallback();
                break;
            case 2:
                onBadCallback();
                break;
            default:
                Debug.LogError("Invalid button option");
                break;
        }
    }


}
