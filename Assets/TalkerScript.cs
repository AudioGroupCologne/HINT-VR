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
                createSentence();
                audioSrc.PlayOneShot(sentenceAudio[wordIx++]);
                sentenceReady = true;
            }
        }
    }

    private void createSentence()
    {
        sentenceAudio[0] = the;
        sentenceAudio[1] = subjects[Random.Range(0, clipCount)];
        sentenceAudio[2] = verbs[Random.Range(0, clipCount)];
        sentenceAudio[3] = count[Random.Range(0, clipCount)];
        sentenceAudio[4] = adjectives[Random.Range(0, clipCount)];
        sentenceAudio[5] = objects[Random.Range(0, clipCount)];
    }

    public string[] getSentenceString()
    {
        string[] sentenceString = new string[wordCount];
        for(int i = 0; i < wordCount; i++)
        {
            sentenceString[i] = sentenceAudio[i].ToString();
        }
        return sentenceString;
    } 

}
