using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiSN_database
{
    /*
     * This class loads the audio and image assets for the training game from Unity's Resource system.
     * 
     * The strings for the entries are taken from the filenames of the audio assets!
     * 
     * 
     * The word structure of a sentences has to be consistent: e.g. subject verb adjective object.
     * The words of a sentence (e.g. subjects) have to be located in an individual folder, named after their position in the sentence: subject: 00, verb: 01, adjectvie: 02, object: 03
     * 
     * The database differentiates between 'selectable' and non-'selectable' groups.
     * Only 'selectable' groups will be part of the questions in the game (for example: only subjects or objects shall be asked).
     * The group index is used to determine, which entries are 'selectable': e.g. subject & object: _selectables = {0,3}
     * 
     * Icons only have to be provided for 'selectable' groups.
     * The folder names of the icons has to match those of the audio clips: e.g. subject: icons/00, object: icons/03
     * 
     * Apart from just storing data, this class also holds several utility functions, like creating random selections, which are needed for the training game.
     * 
    */

    private int length;
    private int options;
    private int[] selectableGroups;

    // create a list in which each entry is an array of words (the options for the selected word)
    private List<AudioClip[]> clips;
    private List<Sprite[]> icons;
    private List<string[]> words;


    public LiSN_database(string _targetAudioPath, string _iconsPath, int _wordGroups, int[] _selectables)
    {
        string subPath;
        clips = new List<AudioClip[]>();
        icons = new List<Sprite[]>();
        words = new List<string[]>();
        selectableGroups = new int[_selectables.Length];

        length = _wordGroups;
        selectableGroups = _selectables;
        
        // load audio clips from Resources
        for (int i = 0; i < _wordGroups; i++)
        {
            if (_wordGroups < 10)
            {
                subPath = _targetAudioPath + "0" + i;
            }
            else
            {
                subPath = _targetAudioPath + i;
            }
            clips.Add(Resources.LoadAll<AudioClip>(subPath));

            if(options == 0)
            {
                options = clips[i].Length;
            }
            else if(options != clips[i].Length)
            {
                Debug.LogError("Invalid database format. options mismatch!");
            }
            Debug.Log("Audio-Entries " + i + ": " + clips[i].Length);
        }

        Debug.Log("Count: " + clips.Count + " Options: " + options);
        if(options == 0)
        {
            Debug.LogError("Target audio assets could not be found!");
        }
       
        // load icons from Resources and parse 'words'
        for(int i = 0; i < selectableGroups.Length; i++)
        {
            if (_wordGroups < 10)
            {
                subPath = _iconsPath + "0" + selectableGroups[i];
            }
            else
            {
                subPath = _iconsPath + selectableGroups[i];
            }
            icons.Add(Resources.LoadAll<Sprite>(subPath));

            string[] tmp = new string[options];
            for (int j = 0; j < clips[0].Length; j++)
            {
                tmp[j] = clips[selectableGroups[i]][j].ToString().Split(' ')[0];
            }
            words.Add(tmp);

            Debug.Log("Icon-Entries " + i + ": " + icons[i].Length);
        }        

    }

    public int getSentenceLen()
    {
        return length;
    }

    public int getWordOptions()
    {
        return options;
    }

    public int[] getSelectableGroups()
    {
        return selectableGroups;
    }


    public AudioClip[] getAudioArray(int index)
    {
        return clips[index];
    }

    // get sentence by indicies within 'words' list
    public int[] getSentence()
    {
        int[] indices = new int[length];

        for (int i = 0; i < length; i++)
        {
            // generate random index to select an option of a group (short fields are taken into account by using 'clips[i].Length' as upper limit)
            indices[i] = Random.Range(0, clips[i].Length);
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
            // generate random index to select an option of a group (short fields are taken into account by using 'clips[i].Length' as upper limit)
            indices[i] = Random.Range(0, clips[i].Length);
            sentenceAudio[i] = clips[i][indices[i]];
        }

        return sentenceAudio;
    }

    public AudioClip[] getSentenceAudio()
    {
        AudioClip[] sentenceAudio = new AudioClip[length];
        int[] indices = new int[length];

        for (int i = 0; i < length; i++)
        {
            // generate random index to select an option of a group (short fields are taken into account by using 'clips[i].Length' as upper limit)
            indices[i] = Random.Range(0, clips[i].Length);
            sentenceAudio[i] = clips[i][indices[i]];
        }

        return sentenceAudio;
    }

    // Return 'n' different words of a group (determined by 'groupIx') as an array of strings
    public string[] getWordsByGroup(int groupIx, int n)
    {
        bool match = false;
        int[] tmp = new int[n];
        string[] groupWords = new string[n];

        // exclude invalid options
        if (n > options || groupIx > length - 1)
            return null;

        // make sure the selected word group has enough options
        if (clips[groupIx].Length < n)
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
            groupWords[i] = getAudioArray(groupIx)[tmp[i]].ToString().Split(' ')[0];
        }

        return groupWords;
    }

    // Return 'n' different words of a group (determined by 'groupIx') as an array of strings. 'exclude' can be used to make sure, that the word of the current sentence is not chosen.
    public string[] getWordsByGroup(int groupIx, int n, int exclude)
    {
        bool match = false;
        int[] tmp = new int[n];
        string[] groupWords = new string[n];
        AudioClip[] groupClips = getAudioArray(groupIx);

        // exclude invalid options
        if (n > options || groupIx > length - 1)
            return null;

        // exclude 'the' entries (containing only one option) .Length would return '1'...
        if (clips[groupIx].Length < n)
            return null;

        tmp[0] = exclude;
        groupWords[0] = groupClips[tmp[0]].ToString().Split(' ')[0];

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
            groupWords[i] = groupClips[tmp[i]].ToString().Split(' ')[0];
        }

        return groupWords;
    }


    // only added icons to 'highlighted' words...
    // create data structure, which holds 'selectable' words within a list (List1: subjects [0], numbers[3], objects[5]
    public Sprite getIcon(int group, string word)
    {
        for(int i = 0; i < icons[group].Length; i++)
        {
            if (word == icons[group][i].ToString().Split(' ')[0])
                return icons[group][i];
        }
        Debug.Log("No icon found!");
        return icons[0][0];
    }

    public Sprite getIcon(int group, int index)
    {
        return icons[group][index];
    }

    private int[] createRandomSelection(int groupIndex, int count, int exclude)
    {
        bool match = false;
        int[] selection = new int[count];

        // exclude invalid options
        if (count > options || groupIndex > length - 1)
            return null;

        // exclude 'the' entries (containing only one option) .Length would return '1'...
        if (clips[groupIndex].Length < count)
            return null;

        selection[0] = exclude;
        for (int i = 1; i < count; i++)
        {
            do
            {
                match = false;
                // generate a new random number to select the next word
                selection[i] = Random.Range(0, options);
                // check if this index is already in use
                for (int j = 0; j < i; j++)
                {
                    // if the index has already been used, 
                    if (selection[j] == selection[i])
                    {
                        match = true;
                        break;
                    }
                }
            } while (match);

        }
        return selection;
    }

    public void getSelectableWords(int sel, int count, int correctWordIx, out string[] o_words, out Sprite[] o_icons)
    {

        int[] select = new int[count];
        string[] outStrings = new string[count];
        Sprite[] outSprites = new Sprite[count];

        select = createRandomSelection(selectableGroups[sel], count, correctWordIx);

        for(int i = 0; i < count; i++)
        {
            outStrings[i] = words[sel][select[i]];
            outSprites[i] = icons[sel][select[i]];
        }

        o_words = outStrings;
        o_icons = outSprites;
    }

}
