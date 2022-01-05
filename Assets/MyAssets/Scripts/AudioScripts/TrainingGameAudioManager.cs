using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TrainingGameAudioManager : MonoBehaviour
{
    // This class shall control both the distracter and talker 'AudioSource' used within 'TrainingGame'
    // (maybe make this more flexible in a later iteration, e.g. supporting more Sources events etc.)

    // AudioMixer of current scene. Used for adaptive level management (based on hits/misses)
    [SerializeField] AudioMixer mixer;
    // To change parameters of an audio mixer 'exposed parameters' have to be created. Since there is no API to get them from the AudioMixer itself, hand them over through this way
    [SerializeField] string masterChannel;
    [SerializeField] string talkerChannel;
    [SerializeField] string distracterChannel;
    // AudioSources used wihtin the project
    [SerializeField] AudioSource player;
    [SerializeField] AudioSource talker;
    [SerializeField] AudioSource distracter_left;
    [SerializeField] AudioSource distracter_right;
    // delay before target (Talker) audio starts (see LiSN paper)
    [SerializeField] float startDelay;
    // time after which distracter (Story) audio stops, after target sentence has been played
    [SerializeField] float endDelay;

    // clip to be played on a correct answer
    [SerializeField] AudioClip hit;
    // clip to be played on an incorrect answer
    [SerializeField] AudioClip miss;
    // clip to be played on 'unsure' selection
    [SerializeField] AudioClip unsure;
    // clip to be played on receiving a reward sticker
    [SerializeField] AudioClip reward;

    [SerializeField] AudioClip attention;
    [SerializeField] bool playAttentionClip;
    
    // AudioMixer limitations
    [SerializeField] float min_vol_dB = -40;
    [SerializeField] float max_vol_dB = 20;

    public delegate void OnPlayingDone();
    public OnPlayingDone onPlayingDoneCallback = delegate { Debug.Log("No OnPlayingDone delegate set!"); };

    private bool distracterPaused = false;

    // DEPRECATED: Allowed to apply master audio setting based on userVolume value
    /*
    private void Start()
    {
        //changeLevel(2, UserManagement.selfReference.getUserVolume());
    }
    */

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


    public void changeMasterVolume(float deltaVolume_db)
    {
        changeVolume(masterChannel, deltaVolume_db);
    }

    public void changeTalkerVolume(float deltaVolume_db)
    {
        changeVolume(talkerChannel, deltaVolume_db);
    }

    public void changeDistracterVolume(float deltaVolume_db)
    {
        changeVolume(distracterChannel, deltaVolume_db);
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
        return getVolume(talkerChannel);
    }

    public float getDistracterVolume()
    {
        return getVolume(distracterChannel);
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
        if(playAttentionClip)
        {
            AudioClip[] tmp = new AudioClip[sentence.Length + 1];
            tmp[0] = attention;
            sentence.CopyTo(tmp, 1);
            talker.clip = Combine(tmp);
        }
        else
        {
            talker.clip = Combine(sentence);
        }
        
    }

    public void setDistracterSequences(AudioClip story_l, AudioClip story_r)
    {
        Debug.Log("Set seq");
        distracter_left.clip = story_l;
        distracter_right.clip = story_r;
        distracterPaused = false;
    }

    public void startPlaying()
    {
        Debug.Log("StartPlaying");


        // whole duration of a single iteration
        float waitDuration = startDelay + talker.clip.length + endDelay;    

        // immediately start playing distracter
        if (distracterPaused)
        {
            distracter_left.UnPause();
            distracter_right.UnPause();
        }
        else
        {
            distracter_left.Play();
            distracter_right.Play();
        }

        talker.PlayDelayed(startDelay);

        StartCoroutine(AudioIsPlaying(waitDuration));
    }

    private IEnumerator AudioIsPlaying(float audioCycle)
    {
        yield return new WaitForSeconds(audioCycle);

        // stop disctracters
        distracter_left.Pause();
        distracter_right.Pause();
        distracterPaused = true;

        // trigger 'playingDoneCallback' (TrainingGameManager)
        onPlayingDoneCallback();
    }

    // play both of these through a separate audio source (non-directional and unaffected by adaptive AudioMixing)
    public void playOnHit()
    {
        player.PlayOneShot(hit);
    }

    public void playOnMiss()
    {
        player.PlayOneShot(miss);
    }

    public void playOnUnsure()
    {
        player.PlayOneShot(unsure);
    }

    public void playOnReward()
    {
        // wait until OnHit is done!
        player.PlayOneShot(reward);
    }

}
