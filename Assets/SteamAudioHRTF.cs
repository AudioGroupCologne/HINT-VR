using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAudioHRTF : MonoBehaviour
{

    // go to Steam Audio -> Settings to find the names of all available HRTF sets
    [SerializeField] string HRTF = "default";
    [SerializeField] bool enableHeadMovements = false;

    // Start is called before the first frame update
    void Start()
    {
        // HRTF setting
        GameObject SteamAudioManager = GameObject.Find("Steam Audio Manager");
        if (SteamAudioManager != null)
        {
            string[] hrtfs = SteamAudioManager.GetComponent<SteamAudio.SteamAudioManager>().hrtfNames;
            for (int i = 0; i < hrtfs.Length; i++)
            {
                if (HRTF == hrtfs[i])
                {
                    Debug.Log("Set currentHRTF to " + hrtfs[i]);
                    SteamAudioManager.GetComponent<SteamAudio.SteamAudioManager>().currentHRTF = i;
                }
            }
        }
        else
        {
            Debug.LogError("Didn't found SteamAudioManager object!");
        }
    }

    public bool getHeadMovementEnabled()
    {
        return enableHeadMovements;
    }

}
