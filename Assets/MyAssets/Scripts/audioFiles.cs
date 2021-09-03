using UnityEngine;


public partial class DemoGameManager
{
    private class audioFiles
    {
        public AudioClip[] the;
        public AudioClip[] subjects;
        public AudioClip[] verbs;
        public AudioClip[] count;
        public AudioClip[] adjectives;
        public AudioClip[] objects;

        // all lists from LiSN have 6 word sentences
        public int wordCount = 6;
        // all lists from LiSN have 6 options per word (excluding 'the')
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

        /// THIS HAS TO BE OVERLOADED TO HANDLE DIFFERENT WORD LISTS etc.!!!!!
        // Return 'n' different words of a word group (determined by 'groupIx') as an array of strings
        public string[] getWordsByGroup(int groupIx, int n)
        {
            bool match = false;
            int[] tmp = new int[n];
            string[] groupWords = new string[n];

            if (n > wordOptions || groupIx > wordCount -1)
                return null;

            for (int i = 1; i < n; i++)
            {
                do
                {
                    match = false;
                    // generate a new random number to select the next word
                    tmp[i] = Random.Range(0, wordOptions);
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

    }

}
