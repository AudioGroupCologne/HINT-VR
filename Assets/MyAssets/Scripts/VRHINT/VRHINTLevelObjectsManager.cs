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

    private bool distActive = true;

    /**
     * Change distractor visibility.
     */
    public void ToggleDistractor(bool showDist)
    {
        distActive = showDist;
    }


    /**
     * Set position of LevelObjects (target, distractor, userInferface) relative to camera using euler coordinates
     */
    public void AngularPosition(hintObjects obj, float _angle, float _distance, float _height = 0)
    {
        Vector3 tmp = Quaternion.AngleAxis(_angle, Vector3.up) * (PlayerCamera.transform.forward * _distance);
        tmp.y += _height;

        switch (obj)
        {
            case hintObjects.target:
                TargetObj.transform.position = PlayerCamera.transform.position + tmp;
                TargetObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up);
                break;
            case hintObjects.distractor:
                DistractorObj.transform.position = PlayerCamera.transform.position + tmp;
                DistractorObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, _angle, 0);
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
        TargetObj.SetActive(showObjects);
        // only set distractor object to active if global and local setting is true
        DistractorObj.SetActive(showObjects && distActive);
    }
        
}
