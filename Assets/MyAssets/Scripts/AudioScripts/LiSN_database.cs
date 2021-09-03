using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiSN_database
{
    /*
     * There are 3 word lists given in the LiSN paper:
     * -> each lists constructs sentences with a fixed length of 6 words ('length')
     * -> each word (except 'the') has 9 different options ('options'
     * 
     * This class shall construct a database, which is able to load any of the 3 lists and offers an interface for the other components to access the audio files and associated strings.
    */

    private const int length = 6;
    private const int options = 9;
    
    // create a list in which each entry is an array of words (the options for the selected word)
    private List<AudioClip[]> words;


    // basic constructor, determining the list the shall be used
    public LiSN_database(int list)
    {
        words = new List<AudioClip[]>();
        switch (list)
        {
            case 1:
                loadList1();
                break;
            // other lists have yet to be included
            default:
                loadList1();
                break;

        }
    }

    public int getLen()
    {
        return length;
    }

    public int getOptions()
    {
        return options;
    }

    
    public AudioClip[] getAudioArray(int index)
    {
        return words[index];
    }

    public int[] getSentence()
    {
        int[] indices = new int[length];

        for (int i = 0; i < length; i++)
        {
            // generate random index to select an option of a group (short fields are taken into account by using 'words[i].Length' as upper limit)
            indices[i] = Random.Range(0, words[i].Length);
        }

        return indices;
    }

    // get sentence audio files by a pre-defined index array
    public AudioClip[] getSentenceAudio(int[] indices)
    {
        if (indices.Length != length)
            return null;


        AudioClip[] sentenceAudio = new AudioClip[length];

        for (int i = 0; i < length; i++)
        {
            // generate random index to select an option of a group (short fields are taken into account by using 'words[i].Length' as upper limit)
            indices[i] = Random.Range(0, words[i].Length);
            sentenceAudio[i] = words[i][indices[i]];
        }

        return sentenceAudio;
    }

    public AudioClip[] getSentenceAudio()
    {
        AudioClip[] sentenceAudio = new AudioClip[length];
        int[] indices = new int[length];

        for (int i = 0; i < length; i++)
        {
            // generate random index to select an option of a group (short fields are taken into account by using 'words[i].Length' as upper limit)
            indices[i] = Random.Range(0, words[i].Length);
            sentenceAudio[i] = words[i][indices[i]];
        }

        return sentenceAudio;
    }




    // Return 'n' different words of a word group (determined by 'groupIx') as an array of strings
    // This method does not know, which word is within a sentence!!! FIX THIS
    // Option A: Submit correct word as parameter (by index)
    // Option B: Hold current sentence within this class
    public string[] getWordsByGroup(int groupIx, int n)
    {
        bool match = false;
        int[] tmp = new int[n];
        string[] groupWords = new string[n];

        // exclude invalid options
        if (n > options || groupIx > length - 1)
            return null;

        // exclude 'the' entries (containing only one option) .Length would return '1'...
        if (words[groupIx].Length < n)
            return null;

        for (int i = 0; i < n; i++)
        {
            do
            {
                match = false;
                // generate a new random number to select the next word
                tmp[i] = Random.Range(0, options);
                // check if this index is already in use
                for (int j = 0; j < i; j++)
                {
                    // if the index has already been used, 
                    if (tmp[j] == tmp[i])
                    {
                        match = true;
                        break;
                    }
                }
            } while (match);

            // get word from array via index
            groupWords[i] = getAudioArray(groupIx)[tmp[i]].ToString();
        }

        return groupWords;
    }

    public string[] getWordsByGroup(int groupIx, int n, int exclude)
    {
        bool match = false;
        int[] tmp = new int[n];
        string[] groupWords = new string[n];
        AudioClip[] clips = getAudioArray(groupIx);

        // exclude invalid options
        if (n > options || groupIx > length - 1)
            return null;

        // exclude 'the' entries (containing only one option) .Length would return '1'...
        if (words[groupIx].Length < n)
            return null;

        tmp[0] = exclude;
        groupWords[0] = clips[tmp[0]].ToString();

        for (int i = 1; i < n; i++)
        {
            do
            {
                match = false;
                // generate a new random number to select the next word
                tmp[i] = Random.Range(0, options);
                // check if this index is already in use
                for (int j = 0; j < i; j++)
                {
                    // if the index has already been used, 
                    if (tmp[j] == tmp[i])
                    {
                        match = true;
                        break;
                    }
                }
            } while (match);

            // get word from array via index
            groupWords[i] = clips[tmp[i]].ToString();
        }

        return groupWords;
    }

    private void loadList1()
    {
        words.Add(Resources.LoadAll<AudioClip>("listTest/the"));
        words.Add(Resources.LoadAll<AudioClip>("listTest/subjects"));
        words.Add(Resources.LoadAll<AudioClip>("listTest/verbs"));
        words.Add(Resources.LoadAll<AudioClip>("listTest/count"));
        words.Add(Resources.LoadAll<AudioClip>("listTest/adjectives"));
        words.Add(Resources.LoadAll<AudioClip>("listTest/objects"));
    }




}
