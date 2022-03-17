using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using CustomTypes;
using CustomTypes.TestSceneTypes;

public class CustomAudioManager : MonoBehaviour
{
    // This class shall control both the distractor and talker 'AudioSource' used within 'TrainingGame'
    // (maybe make this more flexible in a later iteration, e.g. supporting more Sources events etc.)

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
    [SerializeField] AudioSource distractor1;
    [SerializeField] AudioSource distractor2;

    // delay before target (Talker) audio starts (see LiSN paper)
    [SerializeField] float startDelay;
    // time after which distractor (Story) audio stops, after target sentence has been played
    [SerializeField] float endDelay;


    /// <summary>
    /// This stuff is specific to the TrainingGame...
    /// SOLVE THIS THROUGH CUSTOM SOUNDEFFECTS TYPE WITH ASSOCIATED CALLBACKS
    /// playSoundEffect(enum trainingSounds); -> playSoundEffect(trainingSounds.onHit);
    /// playSoundEffect(enum testSounds); -> playSoundEffect(testSounds.onGood);
    /// 
    /// Files for SoundEffects have to be offered through sub-component or MasterScript
    /// </summary>

    // clip to be played on a correct answer

    SoundLibrary lib;

    [SerializeField] AudioClip attention;
    [SerializeField] bool playAttentionClip;

    // AudioMixer limitations
    [SerializeField] readonly float min_vol_dB = -40;
    [SerializeField] readonly float max_vol_dB = 20;

    public delegate void OnPlayingDone();
    public OnPlayingDone onPlayingDoneCallback = delegate { Debug.Log("No OnPlayingDone delegate set!"); };

    private bool distractorPaused = false;

    private void Start()
    {
        lib = GetComponent<SoundLibrary>();
    }
    void changeVolume(audioChannels channel, float deltaVolume_db)
    {
        switch(channel)
        {
            case audioChannels.master:
                changeVolume(masterChannel, deltaVolume_db);
                break;
            case audioChannels.target:
                changeVolume(targetChannel, deltaVolume_db);
                break;
            case audioChannels.distractor:
                changeVolume(distractorChannel, deltaVolume_db);
                break;
            case audioChannels.player:
                changeVolume(playerChannel, deltaVolume_db);
                break;
        }
    }

    // change level of selected AudioSource
    void changeVolume(string channel, float deltaVolume_db)
    {

        float volume;

        // get volume in dB from talker
        mixer.GetFloat(channel, out volume);
        // increase/decrease by dVol
        volume += deltaVolume_db;

        // apply limits to volume
        if (volume < min_vol_dB)
            volume = min_vol_dB;
        else if (volume > max_vol_dB)
            volume = max_vol_dB;


        Debug.Log("Volume: " + volume + " dB");

        // write updated volume level to 'AudioMixer'
        mixer.SetFloat(channel, volume);

    }

    public void setChannelVolume(audioChannels channel, float level_db)
    {
        switch (channel)
        {
            case audioChannels.master:
                setVolume(masterChannel, level_db);
                break;
            case audioChannels.target:
                setVolume(targetChannel, level_db);
                break;
            case audioChannels.distractor:
                setVolume(distractorChannel, level_db);
                break;
            case audioChannels.player:
                setVolume(playerChannel, level_db);
                break;
        }
    }

    void setVolume(string channel, float level_db)
    {
        // apply limits to volume
        if (level_db < min_vol_dB)
            level_db = min_vol_dB;
        else if (level_db > max_vol_dB)
            level_db = max_vol_dB;

        Debug.Log("Volume: " + level_db + " dB");

        // write updated volume level to 'AudioMixer'
        mixer.SetFloat(channel, level_db);
    }

    public void changeMasterVolume(float deltaVolume_db)
    {
        changeVolume(masterChannel, deltaVolume_db);
    }

    public void changeTalkerVolume(float deltaVolume_db)
    {
        Debug.Log("Change TalkerVolume by: " + deltaVolume_db + " dB");
        changeVolume(targetChannel, deltaVolume_db);
    }

    public void changeDistractorVolume(float deltaVolume_db)
    {
        changeVolume(distractorChannel, deltaVolume_db);
    }

    float getVolume(string channel)
    {
        float volume;
        mixer.GetFloat(channel, out volume);
        return volume;

    }

    public float getMasterVolume()
    {
        return getVolume(masterChannel);
    }

    public float getTalkerVolume()
    {
        return getVolume(targetChannel);
    }

    public float getDistractorVolume()
    {
        return getVolume(distractorChannel);
    }


    // Create a single AudioClip from an array of clips
    // REMARK: This only works if all clips have the same number of channels and the same sampling frequency
    static AudioClip Combine(params AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return null;

        int length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            length += clips[i].samples * clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
                continue;

            float[] buffer = new float[clips[i].samples * clips[i].channels];
            clips[i].GetData(buffer, 0);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        AudioClip result = AudioClip.Create("Combine", length, clips[0].channels, clips[0].frequency, false);
        result.SetData(data, 0);

        return result;
    }


    public void setTargetSentence(AudioClip[] sentence)
    {
        if (playAttentionClip)
        {
            AudioClip[] tmp = new AudioClip[sentence.Length + 1];
            tmp[0] = attention;
            sentence.CopyTo(tmp, 1);
            target.clip = Combine(tmp);
        }
        else
        {
            target.clip = Combine(sentence);
        }

    }

    public void setTargetSentence(AudioClip sentence)
    {
        if (playAttentionClip)
        {
            AudioClip[] tmp = new AudioClip[2];
            tmp[0] = attention;
            tmp[1] = sentence;
            target.clip = Combine(tmp);
        }
        else
        {
            target.clip = sentence;
        }

    }



    public void setDistracterSequences(AudioClip story_l, AudioClip story_r)
    {
        Debug.Log("Set seq");
        distractor1.clip = story_l;
        distractor2.clip = story_r;
        distractorPaused = false;

        distractor1.loop = true;
        distractor2.loop = true;
    }

    // Create a type that only holds audio objects... (dist1, dist2, target)
    public void setDistractorAudio(levelObjects dist, AudioClip clip, bool looped)
    {
        switch(dist)
        {
            case levelObjects.distractor1:
                distractor1.clip = clip;
                distractor1.loop = looped;
                break;
            case levelObjects.distractor2:
                distractor2.clip = clip;
                distractor2.loop = looped;
                break;
            default:
                Debug.LogWarning("Invalid distractor selection: " + dist);
                return;
        }
    }

    public void startPlaying()
    {
        Debug.Log("StartPlaying");

        // whole duration of a single iteration
        float waitDuration = startDelay + target.clip.length + endDelay;

        // immediately start playing distracter
        if (distractorPaused)
        {
            distractor1.UnPause();
            if (!distractor1.isPlaying)
            {
                Debug.Log("Dist1 could not be unpaused");
                distractor1.Play();
            }


            if (distractor2 != null)
            {
                distractor2.UnPause();
                if (!distractor2.isPlaying)
                {
                    Debug.Log("Dist2 could not be unpaused");
                    distractor2.Play();
                }

            }
        }
        else
        {
            distractor1.Play();
            if(distractor2 != null)
                distractor2.Play();
        }

        target.PlayDelayed(startDelay);

        StartCoroutine(AudioIsPlaying(waitDuration));
    }

    private IEnumerator AudioIsPlaying(float audioCycle)
    {
        yield return new WaitForSeconds(audioCycle);

        // stop disctracters
        distractor1.Pause();
        if (distractor2 != null)
            distractor2.Pause();

        distractorPaused = true;
        Debug.Log("Is playing lapsed");

        // trigger 'playingDoneCallback' (TrainingGameManager)
        onPlayingDoneCallback();
    }

    // play both of these through a separate audio source (non-directional and unaffected by adaptive AudioMixing)
    public void playSoundEffect(string identifier)
    {
        AudioClip tmp = lib.GetSoundEffect(identifier);

        if(tmp == null)
        {
            Debug.LogWarning("No clip found for identifier: " + identifier);
        }

        player.PlayOneShot(tmp);
    }
}
