using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkerScript : MonoBehaviour
{
    public AudioClip the;
    public AudioClip[] subjects;
    public AudioClip[] verbs;
    public AudioClip[] count;
    public AudioClip[] adjectives;
    public AudioClip[] objects;
    public int clipCount;

    private AudioSource audioSrc;
    private int wordCount = 6;
    private int wordIx = 0;
    private bool sentenceReady = false;
    private AudioClip[] sentenceAudio;

    private void Start()
    {
        audioSrc = gameObject.AddComponent<AudioSource>();
        sentenceAudio = new AudioClip[wordCount];
    }


    // Update is called once per frame
    void Update()
    {
        if (!audioSrc.isPlaying)
        {
            if (wordIx < wordCount && sentenceReady)
            {
                audioSrc.PlayOneShot(sentenceAudio[wordIx++]);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                wordIx = 0;
                /*sentenceAudio = */createSentence();
                audioSrc.PlayOneShot(sentenceAudio[wordIx++]);
                sentenceReady = true;
            }
        }
    }



    private void createSentence()
    {
        AudioClip[] sentence = new AudioClip[6];
        int randIx = Random.Range(0, clipCount);
        Debug.Log("Ix: " + randIx);

        sentenceAudio[0] = the;
        sentenceAudio[1] = subjects[randIx];
        randIx = Random.Range(0, clipCount);
        sentenceAudio[2] = verbs[randIx];
        randIx = Random.Range(0, clipCount);
        sentenceAudio[3] = count[randIx];
        randIx = Random.Range(0, clipCount);
        sentenceAudio[4] = adjectives[randIx];
        randIx = Random.Range(0, clipCount);
        sentenceAudio[5] = objects[randIx];

    }

}
