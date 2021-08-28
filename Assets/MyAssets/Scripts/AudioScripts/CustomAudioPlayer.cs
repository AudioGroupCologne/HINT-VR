using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudioPlayer : MonoBehaviour
{

    public AudioSource localSrc;
    // make this selectable via settings menu
    public bool autoPlay = false;
    // only used when 'autoPlay' is true. Delay between clips in seconds
    public float pauseBetweenClips = 5;     
    // holds the audio clips to be played (maybe add option to load folder as resource)
    public AudioClip[] clipArray;
    int clip_ix = 0;
    bool playNextClip = true;


    // Update is called once per frame
    void Update()
    {    
        if (playNextClip)
        {
            // manual play mode
             if (!autoPlay)
            {
                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    return;          
                }
            }

            playNextClip = false;
            localSrc.PlayOneShot(clipArray[clip_ix]);
            if (++clip_ix >= clipArray.Length) clip_ix = 0;
            Debug.Log("Start Coroutine");
            StartCoroutine(waiter());

        }   
    }

    IEnumerator waiter()
    {
        // wait until current clip is done being played
        yield return new WaitUntil(() => !localSrc.isPlaying);

        // wait for 'pauseBetweenClips' seconds in autoPlay mode
        if (autoPlay)
        {
            yield return new WaitForSeconds(pauseBetweenClips);
        }
        playNextClip = true;       
    }

}
