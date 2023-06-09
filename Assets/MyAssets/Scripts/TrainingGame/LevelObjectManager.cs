using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomTypes;


public class LevelObjectManager : MonoBehaviour
{
    // the camare is used as an anchor, to allow for relative positioning
    [SerializeField] GameObject PlayerCamera;

    [SerializeField] GameObject TargetObj;
    [SerializeField] GameObject Distractor1Obj;
    [SerializeField] GameObject Distractor2Obj;
    [SerializeField] GameObject generalUI;
    

    [SerializeField] Vector3 positionLeft;
    [SerializeField] Vector3 positionRight;
    [SerializeField] Vector3 positionFront;
    [SerializeField] Vector3 relativeUIPosition;


    private bool objectsVisible = false;
    
    private distractorSettings setup = distractorSettings.bothDist;


    public void setDistractorSettings(distractorSettings setting)
    {
        setup = setting;
    }

    // ensure backwards compability through implementing 'option' as optional parameter
    public void setLevelObjectPositions()
    {

        // avoid collisions when shifting the transform
        if(objectsVisible)
        {
            showLevelObjects(false);
        }

        // set position of TalkerObj based on MainCameras position
        TargetObj.transform.position = PlayerCamera.transform.position + positionFront;

        // get rotation of camera
        Vector3 rot = Quaternion.identity.eulerAngles;

        // turn by 180 degree (object shall face camera, not look into the same direction)
        rot = new Vector3(rot.x, rot.y + 180, rot.z);

        // apply rotation to object
        TargetObj.transform.rotation = Quaternion.Euler(rot);

        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        Distractor1Obj.transform.rotation = Quaternion.Euler(rot);

        rot = new Vector3(rot.x, rot.y + 90, rot.z);
        Distractor2Obj.transform.rotation = Quaternion.Euler(rot);


        Distractor1Obj.transform.position = PlayerCamera.transform.position + positionLeft;
        Distractor2Obj.transform.position = PlayerCamera.transform.position + positionRight;

        // go back to previous state
        if(objectsVisible)
        {
            showLevelObjects(true);
        }
        
    }

    // Set position relative to camera
    public void AngularPosition(levelObjects obj, float _angle, float _distance, float _height = 0)
    {
        Vector3 tmp = Quaternion.AngleAxis(_angle, Vector3.up) * (PlayerCamera.transform.forward * _distance);
        tmp.y += _height;

        switch (obj)
        {
            case levelObjects.target:
                TargetObj.transform.position = PlayerCamera.transform.position + tmp;
                TargetObj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up);
                break;
            case levelObjects.distractor1:
                Distractor1Obj.transform.position = PlayerCamera.transform.position + tmp;
                Distractor1Obj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, _angle, 0);
                break;
            case levelObjects.distractor2:
                Distractor2Obj.transform.position = PlayerCamera.transform.position + tmp;
                Distractor2Obj.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up);
                break;
            case levelObjects.userInterface:
                generalUI.transform.position = PlayerCamera.transform.position + tmp;
                generalUI.transform.rotation = Quaternion.LookRotation(-PlayerCamera.transform.forward, PlayerCamera.transform.up) * Quaternion.Euler(0, 180, 0);
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }
        
    }

    public void setGameObjectToPosition(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;
    }

    public void setGameObjectToLevelObject(GameObject obj, levelObjects levelObj)
    {
        switch (levelObj)
        {
            case levelObjects.target:
                obj.transform.position = TargetObj.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(TargetObj.transform.forward, TargetObj.transform.up);
                break;
            case levelObjects.distractor1:
                obj.transform.position = Distractor1Obj.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(Distractor1Obj.transform.forward, Distractor1Obj.transform.up);
                break;
            case levelObjects.distractor2:
                obj.transform.position = Distractor2Obj.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(Distractor2Obj.transform.forward, Distractor2Obj.transform.up);
                break;
            case levelObjects.userInterface:
                obj.transform.position = generalUI.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(generalUI.transform.forward, generalUI.transform.up);
                break;
            case levelObjects.camera:
                obj.transform.position = PlayerCamera.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(PlayerCamera.transform.forward, PlayerCamera.transform.up);
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }
    }

    public void setLevelObjectPosition(levelObjects obj, levelPositions pos)
    {

        Vector3 tmp;

        switch (pos)
        {
            case levelPositions.front:
                tmp = positionFront;
                break;
            case levelPositions.left:
                tmp = positionLeft;
                break;
            case levelPositions.right:
                tmp = positionRight;
                break;
            default:
                Debug.LogError("Invalid levelPostions selector: " + pos);
                return;
        }
        
        // refactor this to angle and distance instead of coordinates
        switch(obj)
            {
            case levelObjects.target:
                TargetObj.transform.position = PlayerCamera.transform.position + tmp;
                break;
            case levelObjects.distractor1:
                Distractor1Obj.transform.position = PlayerCamera.transform.position + tmp;
                break;
            case levelObjects.distractor2:
                Distractor2Obj.transform.position = PlayerCamera.transform.position + tmp;
                break;
            default:
                Debug.LogError("Invalid levelObject selector: " + obj);
                return;
        }
    }


    public void showLevelObjects(bool show)
    {
        objectsVisible = show;
        TargetObj.SetActive(show);

        switch(setup)
        {
            case distractorSettings.dist1:
                Distractor1Obj.SetActive(show);
                if(Distractor2Obj != null)
                    Distractor2Obj.SetActive(false);
                break;

            case distractorSettings.dist2:
                Distractor1Obj.SetActive(false);
                if (Distractor2Obj != null)
                    Distractor2Obj.SetActive(show);
                break;

            case distractorSettings.bothDist:
                Distractor1Obj.SetActive(show);
                if (Distractor2Obj != null)
                    Distractor2Obj.SetActive(show);
                break;
        }
    }

    public void showLevelObject(levelObjects obj, bool show)
    {

        switch (obj)
        {
            case levelObjects.target:
                TargetObj.SetActive(show);
                break;

            case levelObjects.distractor1:
                Distractor1Obj.SetActive(show);
                break;

            case levelObjects.distractor2:
                Distractor2Obj.SetActive(show);
                break;
        }
    }
}
