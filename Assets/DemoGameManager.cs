using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DemoGameManager : MonoBehaviour
{

    private class Sentence
    {
        public int[] indices;
        public string[] words;
        public AudioClip[] audio;

        public Sentence(int len)
        {
            indices = new int[len];
            words = new string[len];
            audio = new AudioClip[len];
        }
    }

    private class audioFiles
    {
        public AudioClip[] the;
        public AudioClip[] subjects;
        public AudioClip[] verbs;
        public AudioClip[] count;
        public AudioClip[] adjectives;
        public AudioClip[] objects;

        public int wordCount = 6;
        public int wordOptions = 9;

        public audioFiles(int listIx)
        {
            loadAudioFiles();
        }

        public AudioClip[] getAudioArray(int index)
        {
            switch (index)
            {
                case 0:
                    return the;
                case 1:
                    return subjects;
                case 2:
                    return verbs;
                case 3:
                    return count;
                case 4:
                    return adjectives;
                case 5:
                    return objects;
                default:
                    return null;
            }
        }

        private void loadAudioFiles()
        {
            the = Resources.LoadAll<AudioClip>("listTest/the");
            subjects = Resources.LoadAll<AudioClip>("listTest/subjects");
            Debug.Log(subjects.Length + " subjects loaded");
            verbs = Resources.LoadAll<AudioClip>("listTest/verbs");
            Debug.Log(verbs.Length + " verbs loaded");
            count = Resources.LoadAll<AudioClip>("listTest/count");
            Debug.Log(count.Length + " count loaded");
            adjectives = Resources.LoadAll<AudioClip>("listTest/adjectives");
            Debug.Log(adjectives.Length + " suadjectives loaded");
            objects = Resources.LoadAll<AudioClip>("listTest/objects");
            Debug.Log(objects.Length + " objects loaded");
        }

    }

    //private wordClass testClass;
    private Sentence sent;
    private audioFiles aFiles;

    private AudioClip[] sentenceAudio;
    private string[] sentenceWords;
    private int[] sentenceIndices;
    private int clipCount = 9;
    private int wordCount = 6;
    private int wordIx = 0;
    private bool sentenceReady = false;

    public AudioSource targetSource;
    public AudioSource distracterSource;
    public GameObject sentenceUI;

    // Start is called before the first frame update
    void Start()
    {
        sent = new Sentence(wordCount);
        aFiles = new audioFiles(1);

        // make sure to disable UI at load.
        sentenceUI.SetActive(false);
        
        sentenceAudio = new AudioClip[wordCount];
        sentenceWords = new string[wordCount];
        sentenceIndices = new int[wordCount];

    }

    // Update is called once per frame
    void Update()
    {
        // wait for user input to play new sentence
        if (!targetSource.isPlaying)
        {
            // play full sentence
            if (wordIx < wordCount && sentenceReady)
            {
                //targetSource.PlayOneShot(sentenceAudio[wordIx++]);
                targetSource.PlayOneShot(sent.audio[wordIx++]);
            }
            // open UI element after playing last word
            else if(sentenceReady)
            {
                // a new sentence has to be created...
                sentenceReady = false;
                Debug.Log("Set UI active");
                // show UI element
                sentenceUI.SetActive(true);
            }
            // wait for user input before creating new sentence
            else if (Input.GetKeyDown(KeyCode.Space))
            {            
                createSentence(sent, aFiles);              
            }
        }
    }

    private void createSentence(Sentence st, audioFiles af)
    {
        // exception for 'the'
        st.indices[0] = 0;
        st.audio[0] = af.the[0];
        st.words[0] = af.the[0].ToString();

        for(int i = 1; i < af.wordCount; i++)
        {
            st.indices[i] = Random.Range(0, af.wordOptions);
            st.audio[i] = af.getAudioArray(i)[st.indices[i]];
            st.words[i] = st.audio[i].ToString();
        }

        wordIx = 0;
        sentenceReady = true;
    }

    public string[] getSentenceString()
    {
        return sent.words;
    }

    public string getWordFromSentence(int index)
    {
        if (index > wordCount)
            return null;

        return sent.words[index];
    }

    public int getWordIxFromSentence(int index)
    {
        if (index > wordCount)
            return -1;

        return sent.indices[index];
    }

    // make this prettier with references... (word class or word[][] or something...)
    // Return count different words from the selected group (via wordIndex [e.g. verbs], excluding the correct one<
    public string[] getFalseWordsFromGroup(int groupIndex, int count)
    {
        if (groupIndex > wordCount || count > wordCount - 1)
            return null;

        string[] retString = new string[count];
        string correctWord = getWordFromSentence(groupIndex);
        string tmp;
        int[] usedWords = new int[count];

        switch (groupIndex)
        {
            case 0:
                // get count DIFFERENT words, that are not the correct word
                for (int i = 0; i < count; i++)
                {
                    // generate new rand int to access word
                    usedWords[i] = Random.Range(0, clipCount);
                    
                    for(int j = 0; j < i; j++)
                    {
                        if(usedWords[j] == usedWords[i])
                        {
                            // reroll and set j back to 0
                            usedWords[i] = Random.Range(0, clipCount);
                            j = 0;
                        }
                    }
                    // get random word
                    tmp = aFiles.subjects[i].ToString();
                    if(tmp != correctWord)
                    {
                        retString[i] = tmp;
                    }
                    else
                    {
                        i--;
                    }
                }
                break;
        }


        for(int i = 0; i < count; i++)
        {
            retString[i] = sentenceAudio[groupIndex].ToString();
        }

        return retString;
    }

    /**
     * There are no doubling of words allowed.
     * 
     * @param wordIndex - determines number of the word within the sentences
     * @param count     - determines how many words shall be returned
     * return           - array of strings, with correct word a index 0
     *                    OR: null if wordIndex or count was invalid.
     * 
     */
    public string[] getUserWordSelection(int wordIndex, int count)
    {
        // invalid parameters (also exclude 'the')
        if (wordIndex >= aFiles.wordCount || count >= aFiles.wordOptions || wordIx == 0)
        {
            return null;
        }

        // hold selected strings to be returned
        string[] retStr = new string[count];
        // keep track of already selected words via their indices
        int[] wordIxs = new int[count];
        bool match = false;

        // write the correct word at index 0
        retStr[0] = getWordFromSentence(wordIndex);
        wordIxs[0] = getWordIxFromSentence(wordIndex);

        // select count - 1 random words from the same group
        for(int i = 1; i < count; i++)
        {
            do
            {
                match = false;
                // generate a new random number to select the next word
                wordIxs[i] = Random.Range(0, aFiles.wordOptions);
                // check if this index is already in use
                for (int j = 0; j < i; j++)
                {
                    // if the index has already been used, 
                    if (wordIxs[j] == wordIxs[i])
                    {
                        match = true;
                        break;
                    }
                }
            } while (match);

            // get word from array via index
            retStr[i] = aFiles.getAudioArray(wordIndex)[wordIxs[i]].ToString();
        }

        return retStr;
    }


    public void NextWordBtn()
    {
        Debug.Log("NextWord pressed");
        sentenceUI.SetActive(false);
    }


}
