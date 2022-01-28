using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    // the camare is used as an anchor, to allow for relative positioning
    [SerializeField] GameObject PlayerCamera;

    [SerializeField] GameObject TalkerObj;
    [SerializeField] GameObject DistractorLeftObj;
    [SerializeField] GameObject DistractorRightObj;
    [SerializeField] GameObject generalUI;

    [SerializeField] Vector3 talkerPos;
    [SerializeField] Vector3 distractorPos_left;
    [SerializeField] Vector3 distractorPos_right;

    [SerializeField] Vector3 relativeUIPosition;

    private bool objectsVisible = false;
    // 0: show all objects (default), 1: show only left dist, 2: show only right dist
    private int optionField = 0;

    // Start is called before the first frame update
    void Start()
    {
        generalUI.transform.position = PlayerCamera.transform.position + relativeUIPosition;
    }

    // ensure backwards compability through implementing 'option' as optional parameter
    public void setLevelObjectPositions(int option = 0)
    {
        optionField = option;

        // avoid collisions when shifting the transform
        if(objectsVisible)
        {
            showLevelObjects(false);
        }
        
        // set position of TalkerObj based on MainCameras position
        TalkerObj.transform.position = PlayerCamera.transform.position + talkerPos;
        // get rotation of camera
        Vector3 rot = Quaternion.identity.eulerAngles;
        // turn by 180 degree (object shall face camera, not look into the same direction)
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        // apply rotation to object
        TalkerObj.transform.rotation = Quaternion.Euler(rot);
        rot = new Vector3(rot.x, rot.y + 90, rot.z);
        DistractorRightObj.transform.rotation = Quaternion.Euler(rot);
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        DistractorLeftObj.transform.rotation = Quaternion.Euler(rot);

        DistractorLeftObj.transform.position = PlayerCamera.transform.position + distractorPos_left;
        DistractorRightObj.transform.position = PlayerCamera.transform.position + distractorPos_right;

        // go back to previous state
        if(objectsVisible)
        {
            showLevelObjects(true);
        }
        
    }

    /**
    * option: 
    * 0 -> show both distractors
    * 1 -> show only left disctractor
    * 2 -> show only right disctractor
    */
    public void showLevelObjects(bool show)
    {
        objectsVisible = show;
        TalkerObj.SetActive(show);

        switch(optionField)
        {
            case 0:
                DistractorLeftObj.SetActive(show);
                DistractorRightObj.SetActive(show);
                break;

            case 1:
                DistractorLeftObj.SetActive(show);
                DistractorRightObj.SetActive(false);
                break;

            case 2:
                DistractorLeftObj.SetActive(false);
                DistractorRightObj.SetActive(show);
                break;
        }
    }
}
