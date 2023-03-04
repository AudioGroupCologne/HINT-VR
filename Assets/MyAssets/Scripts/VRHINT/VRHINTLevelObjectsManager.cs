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


    /**
     * Set position of LevelObjects (target, distractor, userInferface) relative to camera using euler coordinates
     */
    public void SetRelativePosition(hintObjects obj, float angle, float distance, float height = 0)
    {
        Vector3 tmp = Quaternion.AngleAxis(angle, Vector3.up) * (PlayerCamera.transform.forward * distance);

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
                tmp.y += height;
                uiObj.transform.position = PlayerCamera.transform.position + tmp;
                uiObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, 180, 0);
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }

    }


    /**
     * Hide/show all objects
     */
    public void ShowLevelObjects(bool showObjects)
    {
        TargetObj.SetActive(showObjects);
        DistractorObj.SetActive(showObjects);
        uiObj.SetActive(showObjects );
    }

    /**
     * Hide/show a specific object
     */ 
    public void ShowLevelObject(hintObjects obj, bool show)
    {
        switch (obj)
        {
            case hintObjects.target:
                TargetObj.SetActive(show);
                break;
            case hintObjects.distractor:
                DistractorObj.SetActive(show);
                break;
            case hintObjects.userInterface:
                uiObj.SetActive(show);
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }
    }
        
}
