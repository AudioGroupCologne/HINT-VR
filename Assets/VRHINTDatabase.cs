using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VRHINTDatabase : MonoBehaviour
{
    // hold the target sentences audio clips as list of AudioClips
    List<AudioClip[]> sentenceAudio;
    // hold the target sentences as list of strings
    List<string[]> sentenceStrings;
    // holds the number of lists in the database
    int numLists;
    // hold the number of sentences in each lists
    int listEntries;
    

 
    public VRHINTDatabase(string _targetAudioPath, int _numLists, int _listEntries)
    {
        sentenceAudio = new List<AudioClip[]>();
        sentenceStrings = new List<string[]>();
        string subPath = _targetAudioPath;
        string textPath = _targetAudioPath;
        numLists = _numLists;
        listEntries = _listEntries;

        for (int i = 1; i <= numLists; i++)
        {
            if (i < 10)
            {
                subPath = _targetAudioPath + "0" + i;
                textPath = _targetAudioPath + "0" + i + "/list" + i;
            }
            else
            {
                subPath = _targetAudioPath + i;
                textPath = _targetAudioPath + i + "/list" + i;
            }

            sentenceAudio.Add(Resources.LoadAll<AudioClip>(subPath));

            if (listEntries != sentenceAudio[i-1].Length)
            {
                Debug.LogError("Audio entries in list " + i + "don't match _listEntries: " + _listEntries + " loaded: " + sentenceAudio[i-1].Length);
            }

            TextAsset text = Resources.Load(textPath) as TextAsset; // "audio/german-hint/01/list1"
            string[] test = text.ToString().Replace('\r', ' ').Split('\n');
            sentenceStrings.Add(test);

            if (listEntries != sentenceStrings[i - 1].Length)
            {
                Debug.LogError("String entries in list "+ i + "don't match _listEntries: " + _listEntries +" loaded: " + sentenceStrings[i - 1].Length);
            }

            Debug.Log("List " + i + ": Audio: " + sentenceAudio[i - 1].Length + " Strings: " + sentenceStrings[i - 1].Length);

        }

        if(sentenceAudio.Count != numLists)
        {
            Debug.LogError("Number of audio lists loaded does not match expected number of lists: " + numLists + " (expected) " + sentenceAudio.Count + "(loaded)");
        }
        if (sentenceStrings.Count != numLists)
        {
            Debug.LogError("Number of string lists loaded does not match expected number of lists: " + numLists + " (expected) " + sentenceStrings.Count + "(loaded)");
        }
    }

    public AudioClip[] getList(int index)
    {
        if(index >= sentenceAudio.Count)
        {
            Debug.LogWarning("Index exceeds list entries!");
            return null;
        }

        return sentenceAudio[index];
    }

    public AudioClip getSentence(int listIndex, int sentenceIndex)
    {
        if (listIndex >= sentenceAudio.Count)
        {
            Debug.LogWarning("Index exceeds list entries!");
            return null;
        }

        if (sentenceIndex >= sentenceAudio[listIndex].Length)
        {
            Debug.LogWarning("Index exceeds sentences entries!");
            return null;
        }

        return sentenceAudio[listIndex][sentenceIndex];
    }

}
