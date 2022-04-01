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

    [SerializeField] AudioClip noise;

    [SerializeField] float objectDistance = 10.0f;
    [SerializeField] float interfaceDistance = 9.0f;
    [SerializeField] float interfaceHeight = 2.5f;

    [SerializeField] string targetAudioPath = "audio/german-hint/";
    [SerializeField] int numLists = 12;
    [SerializeField] int numSentences = 20;

    private VRHINTDatabase database;


    void Start()
    {
        // set delegates
        audioManager.onPlayingDoneCallback = OnPlayingDone;
        settingsManager.onDistPositionCallback = OnSetDistLocation;
        settingsManager.onToggleTargetCallback = OnToggleTargetAudio;
        settingsManager.onToggleDistCallback = OnToggleDistAudio;
        settingsManager.onToggleDistMoveCallback = OnToggleDistMove;
        settingsManager.onDistVolumeChange = OnDistVolumeChange;
        settingsManager.onTargetVolumeChange = OnTargetVolumeChange;

        // create database to hold target sentence lists
        database = new VRHINTDatabase(targetAudioPath, numLists, numSentences);

        GameObject Camera = GameObject.Find("CenterEyeAnchor");
        distMover.SetMovementParameters(Camera.transform.position, objectDistance);

        // place userInterface in correct position for setting selection
        levelManager.angularPosition(levelObjects.userInterface, 0, interfaceDistance, interfaceHeight);
        levelManager.angularPosition(levelObjects.target, 0, objectDistance);
        levelManager.angularPosition(levelObjects.distractor1, 0, objectDistance);

        levelManager.showLevelObject(levelObjects.target, true);
        levelManager.showLevelObject(levelObjects.distractor1, true);

        //audioManager.setTargetSentence(target);
        audioManager.setDistractorAudio(levelObjects.distractor1, noise, true);

        AudioClip comb = audioManager.Combine(database.getListAudio(1));
        audioManager.setTargetSentence(comb);

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

    public void OnDistVolumeChange(float value)
    {
        audioManager.changeDistractorVolume(value);
    }

    public void OnTargetVolumeChange(float value)
    {
        audioManager.changeTalkerVolume(value);
    }
}
