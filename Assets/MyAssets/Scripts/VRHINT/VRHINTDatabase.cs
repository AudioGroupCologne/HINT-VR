using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHINTDatabase
{
    // hodls the distracter noise signal
    AudioClip noise;
    // hold the target sentences audio clips as list of AudioClips
    List<AudioClip[]> sentenceAudio;
    // hold the target sentences as list of strings
    List<string[]> sentenceStrings;
    // holds the number of lists in the database
    int numLists;
    // hold the number of sentences in each lists
    int listEntries;
    
    
    /**
     * Create a database object that imports and holds all audio files and strings required to run the HINT procedure.
     */
    public VRHINTDatabase(string _targetAudioPath, string _noisePath, int _numLists, int _listEntries)
    {
        sentenceAudio = new List<AudioClip[]>();
        sentenceStrings = new List<string[]>();
        string subPath;
        string textPath;
        numLists = _numLists;
        listEntries = _listEntries;

        noise  = Resources.Load<AudioClip>(_noisePath);

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

            TextAsset text = Resources.Load(textPath) as TextAsset;
            string[] test = text.ToString().Replace("\r", string.Empty).Split('\n');
            sentenceStrings.Add(test);

            if (listEntries != sentenceStrings[i - 1].Length)
            {
                Debug.LogError("String entries in list "+ i + "don't match _listEntries: " + _listEntries +" loaded: " + sentenceStrings[i - 1].Length);
            }
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

    /**
     * Get noise file
     */
    public AudioClip getNoise()
    {
        return noise;
    }

    /**
     * Get all audio files of a given list
     */
    public AudioClip[] getListAudio(int index)
    {
        if(index - 1 >= sentenceAudio.Count)
        {
            Debug.LogWarning("Index exceeds list entries: " + index);
            return null;
        }

        return sentenceAudio[index - 1];
    }

    /**
     * Get all strings of a given list
     */
    public string[] getListStrings(int index)
    {
        if (index - 1 >= sentenceStrings.Count)
        {
            Debug.LogWarning("Index exceeds list entries: " + index);
            return null;
        }

        return sentenceStrings[index - 1];
    }

    /**
     * Get the audio file of a sentence of a list by index
     */
    public AudioClip getSentenceAudio(int listIndex, int sentenceIndex)
    {
        if (listIndex - 1 >= sentenceAudio.Count)
        {
            Debug.LogWarning("Index exceeds list entries: " + listIndex);
            return null;
        }

        if (sentenceIndex >= sentenceAudio[listIndex - 1].Length)
        {
            Debug.LogWarning("Index exceeds sentences entries: " + sentenceIndex);
            return null;
        }

        return sentenceAudio[listIndex - 1][sentenceIndex];
    }

    /**
     * Get sentence of a list as string ("Die Farbe tropft auf den Boden")
     */
    public string getSentenceString(int listIndex, int sentenceIndex)
    {
        if (listIndex - 1 >= sentenceStrings.Count)
        {
            Debug.LogWarning("Index exceeds list entries: " + listIndex);
            return null;
        }

        if (sentenceIndex >= sentenceStrings[listIndex - 1].Length)
        {
            Debug.LogWarning("Index exceeds sentences entries: " + sentenceIndex);
            return null;
        }

        return sentenceStrings[listIndex - 1][sentenceIndex];
    }

    /**
     * Get the words of a sentence of a list by as string array ("Die", "Farbe", "tropft", "auf", "den", "Boden")
     */
    public string[] getSentenceWords(int listIndex, int sentenceIndex)
    {
        if (listIndex - 1 >= sentenceStrings.Count)
        {
            Debug.LogWarning("Index exceeds list entries: " + listIndex);
            return null;
        }

        if (sentenceIndex >= sentenceStrings[listIndex - 1].Length)
        {
            Debug.LogWarning("Index exceeds sentences entries: " + sentenceIndex);
            return null;
        }

        string tmp = sentenceStrings[listIndex - 1][sentenceIndex];
        string[] ret = tmp.Split(' ');

        return ret;
        
    }

    /**
     * Get 'count' random words from the database, matched by length, capitalization and start/end of sentence
     */
    public string[] getRandomWords(int count, string exclude, bool sentenceStart)
    {

        int cnt = 0;
        string[] tmp = new string[count];
        bool match = false;
        bool capital = isCapital(exclude);

        while(cnt < count)
        {
            // randomly select a sentence from any list
            string randomSentence = sentenceStrings[Random.Range(0, numLists)][Random.Range(0, listEntries)];
            // split sentence into separate words
            string[] ret = randomSentence.Split(' ');
            string randomWord;

            if(sentenceStart)
            {
                randomWord = ret[0];
            }
            else
            {
                randomWord = ret[Random.Range(1, ret.Length)];
            }

            // call continue if anything does not match
            if(capital && !isCapital(randomWord))
            {
                continue;
            }
            if (!capital && isCapital(randomWord))
            {
                continue;
            }

            // exclude correct word
            if (randomWord == exclude)
                continue;

            // make sure there are no repetitions
            foreach(string word in tmp)
            {
                if (word == randomWord)
                {
                    match = true;
                    break;
                }
            }

            if (match)
            {
                match = false;
                continue;
            }
               
            // don't allow 'Die', 'Er' etc as options for 'Krankenwagen'... (maybe amp this up to strlen +-3 or something...)
            if(exclude.Length > 3 && randomWord.Length <= 3)
                continue;

            // only use sentence beginnings if 'exclude' is a sentence beginning & vice-versa (improve capital separation)

            // word is accepted
            tmp[cnt++] = randomWord;

        }

        return tmp;

    }

    /**
     * Simple utility to check capitalization of a given word.
     */
    private bool isCapital(string word)
    {
        if (word[0] >= 'A' && word[0] <= 'Z')
            return true;

        return false;
    }

}
