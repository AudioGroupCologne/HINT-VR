using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomTypes;

public class DemoSceneManager : MonoBehaviour
{

    [SerializeField] CustomAudioManager audioManager;
    [SerializeField] LevelObjectManager levelManager;
    [SerializeField] DemoSettings settingsManager;
    [SerializeField] CircMovement distMover;

    [SerializeField] AudioClip target;
    [SerializeField] AudioClip noise;

    [SerializeField] float objectDistance = 10.0f;
    [SerializeField] float interfaceDistance = 9.0f;
    [SerializeField] float interfaceHeight = 2.0f;

    void Start()
    {
        // set delegates
        audioManager.onPlayingDoneCallback = OnPlayingDone;
        settingsManager.onDistPositionCallback = OnSetDistLocation;
        settingsManager.onToggleTargetCallback = OnToggleTargetAudio;
        settingsManager.onToggleDistCallback = OnToggleDistAudio;
        settingsManager.onToggleDistMoveCallback = OnToggleDistMove;

        GameObject Listener = GameObject.Find("Listener");

        // set listener to same position as camera
        levelManager.setGameObjectToLevelObject(Listener, levelObjects.camera);

        if (Listener.GetComponent<SteamAudioHRTF>().getHeadMovementEnabled())
        {
            GameObject Camera = GameObject.Find("CenterEyeAnchor");
            // set OVR as parent
            Listener.transform.parent = Camera.transform;
        }
        else
        {
            GameObject Player = GameObject.Find("Player");
            // set Player as parent
            Listener.transform.parent = Player.transform;
        }

        distMover.SetMovementParameters(Listener.transform.position, objectDistance);

        // place userInterface in correct position for setting selection
        levelManager.angularPosition(levelObjects.userInterface, 0, interfaceDistance, interfaceHeight);
        levelManager.angularPosition(levelObjects.target, 0, objectDistance);
        levelManager.angularPosition(levelObjects.distractor1, 0, objectDistance);

        levelManager.showLevelObject(levelObjects.target, true);
        levelManager.showLevelObject(levelObjects.distractor1, true);

        audioManager.setTargetSentence(target);
        audioManager.setDistractorAudio(levelObjects.distractor1, noise, true);
        

        // show settings screen
        //settingsManager.ShowSettings(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnPlayingDone()
    {

        Debug.Log("OnPlayingDone");

    }

    void OnSetDistLocation(levelPositions pos)
    {
        switch(pos)
        {
            case levelPositions.front:
                levelManager.angularPosition(levelObjects.distractor1, 0, objectDistance);
                break;
            case levelPositions.left:
                levelManager.angularPosition(levelObjects.distractor1, 270, objectDistance);
                break;
            case levelPositions.right:
                levelManager.angularPosition(levelObjects.distractor1, 90, objectDistance);
                break;
        }
        
    }

    void OnToggleTargetAudio(bool enabled)
    {
        audioManager.toggleAudioSource(levelObjects.target, enabled);
    }

    void OnToggleDistAudio(bool enabled)
    {
        audioManager.toggleAudioSource(levelObjects.distractor1, enabled);
    }

    void OnToggleDistMove(bool enabled)
    {
        distMover.ToggleMovement(enabled);
    }
}
