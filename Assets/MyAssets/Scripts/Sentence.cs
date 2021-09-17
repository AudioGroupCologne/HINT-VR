using UnityEngine;


class Sentence
{
    // holds the number of words within a sentence
    private int numWords;
    // holds the index of each word from the library (here: audioFiles)
    public int[] libIndex;
    // holds each word of a sentence as a string
    public string[] words;
    // holds each word of a sentrence as AudioClip
    public AudioClip[] audio;

    // create data structe of sentence
    public Sentence(int len)
    {
        numWords = len;
        libIndex = new int[len];
        words = new string[len];
        audio = new AudioClip[len];
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
    public string[] getSentenceString()
    {
        return words;
    }

    // retrun a single word from by index
    public string getWordFromSentence(int index)
    {
        if (index > numWords)
            return null;

        return words[index];
    }

    // return the index of a single word by index
    public int getWordIxFromSentence(int index)
    {
        if (index > numWords)
            return -1;

        return libIndex[index];
    }
}


