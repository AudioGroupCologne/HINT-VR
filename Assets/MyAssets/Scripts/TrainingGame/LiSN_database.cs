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

    private int length = 5;
    private const int options = 9;
    private const int selections = 3;

    // create a list in which each entry is an array of words (the options for the selected word)
    private List<AudioClip[]> words;
    private List<Sprite[]> icons;
    private List<string[]> wordStrings;

    public int[] selectableGroup;
   
    // basic constructor, determining the list the shall be used
    public LiSN_database(int list, int voice)
    {
        words = new List<AudioClip[]>();
        icons = new List<Sprite[]>();
        wordStrings = new List<string[]>();
        selectableGroup = new int[selections];

        
        switch (list)
        {
            case 1:
                length = 5;
                load_resources_list1(voice);
                break;
            // other lists have yet to be included
            default:
                load_resources_list1(voice);
                break;

        }
    }

    public LiSN_database(string _targetAudioPath, string _iconsPath, int _wordGroups, int[] _selectables)
    {
        words = new List<AudioClip[]>();
        icons = new List<Sprite[]>();
        wordStrings = new List<string[]>();
        selectableGroup = new int[selections];
        string subPath;

        selectableGroup = _selectables;

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
            words.Add(Resources.LoadAll<AudioClip>(subPath));
            Debug.Log("Entries " + i + ": " + words[i].Length);
        }

        Debug.Log("Count: " + words.Count);


        for(int i = 0; i < _selectables.Length; i++)
        {
            if (_wordGroups < 10)
            {
                subPath = _iconsPath + "0" + i;
            }
            else
            {
                subPath = _iconsPath + i;
            }
            icons.Add(Resources.LoadAll<Sprite>(subPath));
            Debug.Log("Entries " + i + ": " + icons[i].Length);
        }
    }

    /*
    public LiSN_database(string _audioPath, string _iconPath, int _length, int _options, int[] _selectables)
    {
        words = new List<AudioClip[]>();
        icons = new List<Sprite[]>();
        wordStrings = new List<string[]>();

        selectableGroup = _selectables;

        var audioDir =  new DirectoryInfo(_audioPath);
        var fileInfo = audioDir.GetFiles();

        string[] files = Directory.GetFiles(_audioPath);
        string[] dirs = Directory.GetDirectories(_audioPath);
        foreach (string file in files)
        {
            //Do work on the files here
            Debug.Log(file);
        }
        foreach (string dir in dirs)
        {
            //Do work on the files here
            Debug.Log(dir);
        }

    }
    */

    private void asset_loader(string path)
    {

        words.Add(Resources.LoadAll<AudioClip>(path));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/verbs"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/numbers"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/adjectives"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/objects"));

        for (int i = 0; i < length; i++)
        {
            if (words[i].Length != options)
            {
                Debug.LogError("Loading audio clips failed: " + i + " len: " + words[i].Length);
            }
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

    
    public AudioClip[] getAudioArray(int index)
    {
        return words[index];
    }

    // get sentence by indicies within 'words' list
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
    public string[] getWordsByGroup(int groupIx, int n)
    {
        bool match = false;
        int[] tmp = new int[n];
        string[] groupWords = new string[n];

        // exclude invalid options
        if (n > options || groupIx > length - 1)
            return null;

        // make sure the selected word group has enough options
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
            groupWords[i] = getAudioArray(groupIx)[tmp[i]].ToString().Split(' ')[0];
        }

        return groupWords;
    }

    // Return 'n' different words of a word group (determined by 'groupIx') as an array of strings. 'exclude' can be used to make sure, that the word of the current sentence is not chosen.
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
        groupWords[0] = clips[tmp[0]].ToString().Split(' ')[0];

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
            groupWords[i] = clips[tmp[i]].ToString().Split(' ')[0];
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
        if (words[groupIndex].Length < count)
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

        select = createRandomSelection(selectableGroup[sel], count, correctWordIx);

        for(int i = 0; i < count; i++)
        {
            outStrings[i] = wordStrings[sel][select[i]];
            outSprites[i] = icons[sel][select[i]];
        }

        o_words = outStrings;
        o_icons = outSprites;
    }


    public void getSelectableGroups(out int[] o_selectableGroups)
    {
        o_selectableGroups = selectableGroup;
    }

    private void load_resources_list1(int voiceSelection)
    {

        selectableGroup[0] = 0; // subject
        selectableGroup[1] = 2; // count
        selectableGroup[2] = 4; // obeject
        load_audioclips(voiceSelection);
        load_icons();
        create_words();
    }

    private void create_words()
    {
        string[] tmp1 = new string[options];
        string[] tmp2 = new string[options];
        string[] tmp3 = new string[options];

        // ToDo: IMPROVE THIS!
        for (int j = 0; j < options; j++)
        {
            tmp1[j] = words[selectableGroup[0]][j].ToString().Split(' ')[0];
            tmp2[j] = words[selectableGroup[1]][j].ToString().Split(' ')[0];
            tmp3[j] = words[selectableGroup[2]][j].ToString().Split(' ')[0];
        }
        

        wordStrings.Add(tmp1);
        wordStrings.Add(tmp2);
        wordStrings.Add(tmp3);

    }

    private void load_audioclips(int voiceSelection)
    {
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/subjects"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/verbs"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/numbers"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/adjectives"));
        words.Add(Resources.LoadAll<AudioClip>("audio/rec_list_1/objects"));

        for (int i = 0; i < length; i++)
        {
            if (words[i].Length != options)
            {
                Debug.LogError("Loading audio clips failed: " + i + " len: " + words[i].Length);
            }
        }

    }

    private void load_icons()
    {
        icons.Add(Resources.LoadAll<Sprite>("icons/list_1/subjects"));
        icons.Add(Resources.LoadAll<Sprite>("icons/list_1/count"));
        icons.Add(Resources.LoadAll<Sprite>("icons/list_1/objects"));

        for (int i = 0; i < selections; i ++)
        {
            if (icons[i].Length != options)
            {
                Debug.LogError("Loading icons failed: " + i + " len: " + icons[i].Length);
            }
        }
    }




}
