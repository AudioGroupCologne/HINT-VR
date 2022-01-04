using UnityEngine;


class Sentence
{
    // holds the number of words within a sentence
    private int numWords;
    // holds the index of each word from the library (lisnDatabase)
    public int[] libIndex;
    // holds each word of a sentence as a string
    public string[] words;
    // holds each word of a sentrence as AudioClip
    public AudioClip[] audio;

    private int[] selectableWords;

    // create data structe of sentence
    public Sentence(int len, int[] selectables)
    {
        numWords = len;
        libIndex = new int[len];
        words = new string[len];
        audio = new AudioClip[len];
        //selectableWords = new int[selectables.Length];
        selectableWords = selectables;
        
    }

    // fill data fields with actual data, based on audioFiles
    public void createSentence( LiSN_database data )
    {

        libIndex = data.getSentence();
        audio = data.getSentenceAudio(libIndex);

        for (int i = 0; i < numWords; i++)
        {
            words[i] = (audio[i].ToString()).Split(' ')[0];
        }
    }

    // return full sentence as array of strings
    public string[] getSentenceStrings()
    {
        return words;
    }

    // retrun a single word from by index
    public string getWordString(int word_index)
    {
        if (word_index > numWords)
            return null;

        return words[word_index];
    }

    // return the index of a single word by index
    public int getWordIndex(int word_index)
    {
        if (word_index > numWords)
            return -1;

        return libIndex[word_index];
    }

    public string getSelectableWordString(int index)
    {
        if (index > selectableWords.Length)
            return null;

        return words[selectableWords[index]];
    }

    // get the actual libIndex from a selectable group (e.g. selectableWords: 1,3,5 and index = 1 then selectableWords[index] = 3)
    public int getSelectableWordIndex(int index)
    {
        if (index > selectableWords.Length)
            return -1;

        return libIndex[selectableWords[index]];
    }
}


