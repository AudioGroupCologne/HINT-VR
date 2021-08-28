using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CustomAudioPlayer : MonoBehaviour, ICustomAudioPlayer
{

    [SerializeField] private AudioSource src;
    // holds the audio clips to be played (maybe add option to load folder as resource)
    [SerializeField] private AudioClip[] clipArray;
    // make this selectable via settings menu
    [SerializeField] private bool autoPlay = false;
    // only used when 'autoPlay' is true. Delay between clips in seconds
    [SerializeField] private float pauseSeconds = 5f;     
    
    int clip_ix = 0;
    // is set to true, when it is allowed to play a clip (last clip is done [and pause has passed if autoPlay is enabled])
    bool readyToPlayClip = true;

    private void Start()
    {
        if(autoPlay)
        {
            PlayClip();
        }
    }

    void ForcePlayClip()
    {
        readyToPlayClip = false;
        src.PlayOneShot(clipArray[clip_ix]);
        if (++clip_ix >= clipArray.Length) clip_ix = 0;
        StartCoroutine(waiter());
    }


    bool PlayClip()
    {
        // do nothing if Player is not ready
        if (!readyToPlayClip)
            return false;


        readyToPlayClip = false;
        src.PlayOneShot(clipArray[clip_ix]);
        if (++clip_ix >= clipArray.Length) clip_ix = 0;
        
        // start Coroutine to reset 'readyToPlayClip' once current clip is done being played
        StartCoroutine(waiter());
        
        return true;
    }

    IEnumerator waiter()
    {
        // wait until current clip is done being played
        yield return new WaitUntil(() => !src.isPlaying);

        // wait for 'pauseSeconds' in autoPlay mode
        if (autoPlay)
        {
            yield return new WaitForSeconds(pauseSeconds);
            PlayClip();
        }
        readyToPlayClip = true;       
    }

    // EXTERNAL PLAY EVENTS AUTOMATICALLY DISABLE AUTOPLAY
    // play clip if source is ready, if not: nothing happens
    public bool externalPlayRequest()
    {
        autoPlay = false;
        return PlayClip();
    }

    // don't wait until current clip is finished
    public void externalPlayNow()
    {
        autoPlay = false;
        ForcePlayClip();
    }

}
