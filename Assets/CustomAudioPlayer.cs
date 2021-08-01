using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioPlayer : MonoBehaviour
{

    public AudioSource localSrc;

    public AudioClip[] clipArray;
    int clip_ix = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!localSrc.isPlaying)
        {
            localSrc.PlayOneShot(clipArray[clip_ix]);
            if (clip_ix++ >= clipArray.Length) clip_ix = 0;
        }
        
    }
}
