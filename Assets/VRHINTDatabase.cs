using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHINTDatabase : MonoBehaviour
{
    // hold the target sentences audio clips as list of AudioCLips
    List<AudioClip[]> sentenceLists;
    // holds the number of lists in the database
    int numLists;
    // hold the number of sentences in each lists
    int listEntries;
    

    public VRHINTDatabase(string _targetAudioPath, int _numLists, int _listEntries)
    {
        sentenceLists = new List<AudioClip[]>();
        string subPath = _targetAudioPath;
        numLists = _numLists;
        listEntries = _listEntries;

        for(int i = 0; i < numLists; i++)
        {
                if (i < 10)
                {
                    subPath = _targetAudioPath + "0" + i;
                }
                else
                {
                    subPath = _targetAudioPath + i;
                }

                sentenceLists.Add(Resources.LoadAll<AudioClip>(subPath));

                if (listEntries != sentenceLists[i].Length)
                {
                    Debug.LogError("Invalid database format. options mismatch!");
                }
                Debug.Log("Audio-Entries " + i + ": " + sentenceLists[i].Length);
        }

        if(sentenceLists.Count != numLists)
        {
            Debug.LogError("Number of lists loaded does not match expected number of lists: " + numLists + " (expected) " + sentenceLists.Count + "(loaded)");
        }
    }


    public AudioClip[] getList(int index)
    {
        if(index >= sentenceLists.Count)
        {
            Debug.LogWarning("Index exceeds list entries!");
            return null;
        }

        return sentenceLists[index];
    }

    public AudioClip getSentence(int listIndex, int sentenceIndex)
    {
        if (listIndex >= sentenceLists.Count)
        {
            Debug.LogWarning("Index exceeds list entries!");
            return null;
        }

        if (sentenceIndex >= sentenceLists[listIndex].Length)
        {
            Debug.LogWarning("Index exceeds sentences entries!");
            return null;
        }

        return sentenceLists[listIndex][sentenceIndex];
    }

}
