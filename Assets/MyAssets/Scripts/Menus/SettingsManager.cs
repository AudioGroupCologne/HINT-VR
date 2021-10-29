using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider volume;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip onVolumeChanged;

    private void Start()
    {
        volume.Select();
    }

    public void SilderChanged()
    {
        // this is a bit unresponsive
        if (playerSource.isPlaying)
            return;

        Debug.Log("Volume Slider: " + volume.value);
        mixer.SetFloat("MasterVol", volume.value);
        playerSource.PlayOneShot(onVolumeChanged);

        // store changes in userData
        UserManagement.selfReference.changeUserVolume(volume.value);
    }


}
