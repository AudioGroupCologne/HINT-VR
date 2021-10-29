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

    private void Start()
    {
        volume.Select();
    }

    public void SilderChanged()
    {
        Debug.Log("Volume Slider: " + volume.value);
        mixer.SetFloat("MasterVol", volume.value);
    }


}
