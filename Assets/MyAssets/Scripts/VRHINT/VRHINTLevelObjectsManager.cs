using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTypes.VRHINTTypes;

public class VRHINTLevelObjectsManager : MonoBehaviour
{
    // the camare is used as an anchor, to allow for relative positioning
    [SerializeField] GameObject PlayerCamera;
    [SerializeField] GameObject TargetObj;
    [SerializeField] GameObject DistractorObj;
    [SerializeField] GameObject uiObj;

    private bool targetVisible = true;
    private bool distVisible = true;
    private bool uiVisible = true;

    /**
     * Change distractor visibility.
     */
    public void ChangeObjectVisibility(hintObjects obj, bool isVisible)
    {
        switch(obj)
            {
            case hintObjects.target:
                targetVisible = isVisible;
                break;
            case hintObjects.distractor:
                distVisible = isVisible;
                break;
            case hintObjects.userInterface:
                uiVisible = isVisible;
                break;
            default:
                Debug.LogError("Invalid type: " + obj);
                break;
        }
    }


    /**
     * Set position of LevelObjects (target, distractor, userInferface) relative to camera using euler coordinates
     */
    public void SetRelativePosition(hintObjects obj, float angle, float distance, float height = 0)
    {
        Vector3 tmp = Quaternion.AngleAxis(angle, Vector3.up) * (PlayerCamera.transform.forward * distance);
        tmp.y += height;

        switch (obj)
        {
            case hintObjects.target:
                TargetObj.transform.position = PlayerCamera.transform.position + tmp;
                TargetObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up);
                break;
            case hintObjects.distractor:
                DistractorObj.transform.position = PlayerCamera.transform.position + tmp;
                DistractorObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, angle, 0);
                break;
            case hintObjects.userInterface:
                uiObj.transform.position = PlayerCamera.transform.position + tmp;
                uiObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, 180, 0);
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }

    }


    /**
     * Set LevelObjects active or inactive
     */
    public void ShowLevelObjects(bool showObjects)
    {
        TargetObj.SetActive(showObjects && targetVisible);
        DistractorObj.SetActive(showObjects && distVisible);
        uiObj.SetActive(showObjects && uiVisible);
    }
        
}
