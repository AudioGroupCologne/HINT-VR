using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using CustomTypes;

/// <summary>
/// Simplified version of 'CustomAudioManager' script that only fulfills requirements of VRHINT.
/// </summary>

public class VRHINTAudioManager : MonoBehaviour
{
    // AudioMixer of current scene. Used for adaptive level management (based on hits/misses)
    [SerializeField] AudioMixer mixer;
    // To change parameters of an audio mixer 'exposed parameters' have to be created. Since there is no API to get them from the AudioMixer itself, hand them over through this way
    [SerializeField] string masterChannel;
    [SerializeField] string targetChannel;
    [SerializeField] string distractorChannel;
    [SerializeField] string playerChannel;

    // AudioSources used wihtin the project
    [SerializeField] AudioSource player;
    [SerializeField] AudioSource target;
    [SerializeField] AudioSource distractor;

    // delay before target (Talker) audio starts (see LiSN paper)
    [SerializeField] float startDelay;
    // time after which distractor (Story) audio stops, after target sentence has been played
    [SerializeField] float endDelay;

    // AudioMixer limitations
    [SerializeField] readonly float min_vol_dB = -70;
    [SerializeField] readonly float max_vol_dB = 10;

    public delegate void onPlayingDone();
    public onPlayingDone OnPlayingDoneCallback = delegate { Debug.Log("No OnPlayingDone delegate set!"); };

    private float noiseLen = 0;
    private float noiseIndex = 0;

    /**
     * Change level of audio channel by x dB
     */
    public void ChangeChannelLevel(audioChannels channel, float deltaVolume_db)
    {
        float volume;
        string chnStr = GetChannelString(channel);
        if(chnStr == null)
        {
            Debug.LogWarning("Channel not found!");
            return;
        }

        // get volume in dB from talker
        mixer.GetFloat(chnStr, out volume);

        // increase/decrease by dVol
        volume += deltaVolume_db;

        Debug.Log("Set " + channel + " to: " + volume + " dB" + " (change: " + deltaVolume_db + " dB)");
        SetLevel(chnStr, volume);
    }

    /**
     * Set level of  audio channel to x dB
     */
    public void SetChannelLevel(audioChannels channel, float level_db)
    {
        string chnStr = GetChannelString(channel);
        if (chnStr == null)
        {
            Debug.LogWarning("Channel not found!");
            return;
        }

        Debug.Log("Set " + channel + " to: " + level_db + " dB");
        SetLevel(chnStr, level_db);   
    }

    /**
     * Get level from audio channel
     */
    public float GetChannelLevel(audioChannels channel)
    {
        string chnStr = GetChannelString(channel);
        if (chnStr == null)
        {
            Debug.LogWarning("Channel not found!");
            return 0.0f;
        }

        return GetLevel(chnStr);
    }


    /**
     * Set AudioClip for target source
     */
    public void SetTargetAudio(AudioClip clip)
    {
        target.clip = clip;
    }


    // Create a type that only holds audio objects... (dist1, dist2, target)
    public void SetDistractorAudio(AudioClip clip, bool looped)
    {
        distractor.clip = clip;
        distractor.loop = looped;
    }

    /**
     * Start playback of target audio with optional start and end delays
     * Distractor is also triggered if the associated GameObject is active!
     */
    public void StartPlaying()
    {
        // whole duration of a single iteration
        float waitDuration = startDelay + target.clip.length + endDelay;

        noiseIndex += waitDuration;

        if (noiseLen == 0)
        {
            noiseLen = distractor.clip.length;
        }


        if (distractor.gameObject.activeInHierarchy)
        {
            // if we're close to the end of the noise signal, restart the playback
            if (noiseIndex >= noiseLen - 2.0f)
            {
                noiseIndex = 0;
                distractor.Stop();      
            }
        }

        distractor.Play();
        target.PlayDelayed(startDelay);

        StartCoroutine(AudioIsPlaying(waitDuration));
    }

    /**
     * Corountine to catch the end of the current target playback, pause the distractor and trigger the callback
     */
    private IEnumerator AudioIsPlaying(float audioCycle)
    {
        yield return new WaitForSeconds(audioCycle);

        distractor.Pause();

        OnPlayingDoneCallback();
    }

    /**
    * Unity only offers access to AudioMixer control options via exposed parameters (string types).
    * To avoid having to carry these strings through all components a simple enum is used for abstraction
    * The AudioManager script has to know the correct strings for all exposed parameters and map them to the enum.
    */
    private string GetChannelString(audioChannels channel)
    {
        switch (channel)
        {
            case audioChannels.master:
                return masterChannel;
            case audioChannels.target:
                return targetChannel;
            case audioChannels.distractor:
                return distractorChannel;
                break;
            case audioChannels.player:
                return playerChannel;
                break;
            default:
                Debug.LogWarning("Invalid channel selection!");
                return null;
        }
    }

    /**
     * Write new level to AudioMixer using exposed parameters.
     * Enforces min/max limits
     */
    private void SetLevel(string channel, float level_db)
    {
        // apply limits to volume
        if (level_db < min_vol_dB)
        {
            level_db = min_vol_dB;
            Debug.LogWarning("Min Volume is reached: " + level_db);
        }
        else if (level_db > max_vol_dB)
        {
            level_db = max_vol_dB;
            Debug.LogWarning("Max Volume is reached: " + level_db);
        }

        // write updated volume level to 'AudioMixer'
        mixer.SetFloat(channel, level_db);
    }

    /**
     * Get level from AudioMixer using exposed parameters
     */ 
    private float GetLevel(string channel)
    {
        float volume;
        mixer.GetFloat(channel, out volume);
        return volume;

    }


}
