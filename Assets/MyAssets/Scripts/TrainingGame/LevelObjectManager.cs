using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    // the camare is used as an anchor, to allow for relative positioning
    [SerializeField] GameObject PlayerCamera;

    [SerializeField] GameObject TalkerObj;
    [SerializeField] GameObject DistractorObj;
    [SerializeField] GameObject generalUI;

    [SerializeField] Vector3 talkerPos;
    [SerializeField] Vector3 distractorPos1;
    [SerializeField] Vector3 distractorPos2;
    [SerializeField] Vector3 distractorPos3;

    [SerializeField] Vector3 relativeUIPosition;

    // Start is called before the first frame update
    void Start()
    {
        // hide LevelObjects by default
        showLevelObjects(false);
        generalUI.transform.position = PlayerCamera.transform.position + relativeUIPosition;
    }

    public void setLevelObjectPositions(int selector)
    {
        // set position of TalkerObj based on MainCameras position
        TalkerObj.transform.position = PlayerCamera.transform.position + talkerPos;
        // get rotation of camera
        Vector3 rot = Quaternion.identity.eulerAngles;
        // turn by 180 degree (object shall face camera, not look into the same direction)
        rot = new Vector3(rot.x, rot.y + 180, rot.z);
        // apply rotation to object
        TalkerObj.transform.rotation = Quaternion.Euler(rot);

        switch (selector)
        {
            case 0:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos1;
                break;
            case 1:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos2;
                break;
            case 2:
                DistractorObj.transform.position = PlayerCamera.transform.position + distractorPos3;
                break;
        }
    }

    public void showLevelObjects(bool show)
    {
        TalkerObj.SetActive(show);
        DistractorObj.SetActive(show);
    }
}
