using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSphere : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip clip;
    // maybe make this part of 'SelectableObject' with an option of no text
    [SerializeField] private GameObject HighlightText;

    private bool highlightTextVisible = false;

    // AudioSphere shall be a 'SelectableObject' so:
    SelectableObject sObj;

    // Start is called before the first frame update
    void Start()
    {
        sObj = GetComponent<SelectableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // only allow to play audio is device is selected
        if (sObj.getSelectionStatus())
        {
            //ShowHightlightText();

            // press E to play audio
            if(Input.GetKeyDown(KeyCode.E))
            {
                playAudio();
            }
        }
        else
        {
            //HideHightlightText();
        }
    }

    void playAudio()
    {
        if(!src.isPlaying)
        {
            src.PlayOneShot(clip);
        }
    }

    void ShowHightlightText()
    {
        if(HighlightText != null && !highlightTextVisible)
        {
            var hText = Instantiate(HighlightText, transform.position, Quaternion.identity, transform);
            //hText.GetComponent<HighlightText>().setText("Testing");
            //hText.GetComponent<TMPro.TextMeshProUGUI>().text = "Trying";
            highlightTextVisible = true;
        }
    }

    void HideHightlightText()
    {
        if(HighlightText != null && highlightTextVisible)
        {
            //HighlightText.GetComponent<HighlightText>().selfDestruct();
            //floatingTextVisible = false;
        }
    }

}

// so basically: either add 'SelectableObject' to every required device and then add behaviour based on getSelectionStatus()
// or: make script for objects (such as GraphicalAudioObject) available and call 'playAudio' directly from 'SelectionManager'