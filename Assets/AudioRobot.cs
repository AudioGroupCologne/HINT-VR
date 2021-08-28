using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRobot : MonoBehaviour
{
    SelectableObject sObj;

    private ICustomAudioPlayer _audioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        sObj = GetComponent<SelectableObject>();
        _audioPlayer = GetComponent<ICustomAudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(sObj.getSelectionStatus())
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _audioPlayer.externalPlayRequest();
            }
        }
    }
}
