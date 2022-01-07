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

    // Start is called before the first frame update
    void Start()
    {
        generalUI.transform.position = PlayerCamera.transform.position + relativeUIPosition;
    }

    public void setLevelObjectPositions()
    {
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

    public void showLevelObjects(bool show)
    {
        Debug.Log("LevelObjects: " + show);
        objectsVisible = show;
        TalkerObj.SetActive(show);
        DistractorLeftObj.SetActive(show);
        DistractorRightObj.SetActive(show);
    }
}
