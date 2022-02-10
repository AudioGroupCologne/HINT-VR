using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;


    [Serializable]
    public class SoundEffect
    {
        public AudioClip soundEffect;
        public string tag;

        public SoundEffect(AudioClip clip, string ctag)
        {
            soundEffect = clip;
            tag = ctag;
        }
    }

    public AudioClip GetSoundEffect(string ctag)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].tag == ctag)
                return soundEffects[i].soundEffect;
        }

        return null;

    }

}
