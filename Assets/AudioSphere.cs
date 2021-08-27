using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSphere : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip clip;

    // AudioSphere shall be a 'SelectableObject' so:
    SelectableObject sObj;


    // Start is called before the first frame update
    void Start()
    {
        sObj = GetComponent<SelectableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // only allow to play audio is device is selected
        if (sObj.getSelectionStatus())
        {
            // press E to play audio
            if(Input.GetKeyDown(KeyCode.E))
            {
                playAudio();
            }
        }
    }

    void playAudio()
    {
        if(!src.isPlaying)
        {
            src.PlayOneShot(clip);
        }
    }

}

// so basically: either add 'SelectableObject' to every required device and then add behaviour based on getSelectionStatus()
// or: make script for objects (such as GraphicalAudioObject) available and call 'playAudio' directly from 'SelectionManager'