using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{

    [SerializeField] string pathBase = "audio";

    private void Start()
    {
        Debug.Log("Test new asset loader");
        load_wordGroup(pathBase, 5);
    }


    // each audio file has to be labeled with a prefix determining it's group, followed by the actual word of the clip (e.g. 05_cups or 05_trucks)
    public AudioClip[] load_wordGroup(string path, int groups)
    {
        List<AudioClip[]> tmp = new List<AudioClip[]>();
        string subPath;

        for (int i = 0; i < groups; i++)
        {
            if(groups < 10)
            {
                subPath = pathBase + "0" + i;
            }
            else
            {
                subPath = pathBase + i;
            }

            Debug.Log("subPath: " + subPath);

            tmp.Add(Resources.LoadAll<AudioClip>(subPath));
            Debug.Log("Entries " + i + ": " + tmp[i].Length);
        }

        Debug.Log("Count: " + tmp.Count);
        return tmp[0];

    }

}
