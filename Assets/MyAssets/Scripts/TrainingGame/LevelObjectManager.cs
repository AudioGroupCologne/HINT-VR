using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTypes.TrainingGameTypes;
using CustomTypes.TestSceneTypes;

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

    // Start is called before the first frame update
    void Start()
    {
        generalUI.transform.position = PlayerCamera.transform.position + relativeUIPosition;
    }

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
                Distractor2Obj.SetActive(false);
                break;

            case distractorSettings.dist2:
                Distractor1Obj.SetActive(false);
                Distractor2Obj.SetActive(show);
                break;

            case distractorSettings.bothDist:
                Distractor1Obj.SetActive(show);
                Distractor2Obj.SetActive(show);
                break;
        }
    }
}
