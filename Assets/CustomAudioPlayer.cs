using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioPlayer : MonoBehaviour
{

    public AudioSource localSrc;
    // make this selectable via settings menu
    public int mode = 0;        // 0 -> auto, 1 -> manual

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
            if(mode == 0)
            {
                localSrc.PlayOneShot(clipArray[clip_ix]);
                if (clip_ix++ >= clipArray.Length) clip_ix = 0;
            }
            else if(mode == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    localSrc.PlayOneShot(clipArray[clip_ix]);
                    if (clip_ix++ >= clipArray.Length) clip_ix = 0;
                }
            }
        }
        
    }
}
