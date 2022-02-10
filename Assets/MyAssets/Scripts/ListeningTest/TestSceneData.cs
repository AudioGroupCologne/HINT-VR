using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTypes.TestSceneTypes;

public class TestSceneData
{

    AudioClip[] targetSentences;

    public TestSceneData(string _targetAudioPath)
    {
        //targetSentences = new List<AudioClip>();
        //targetSentences.Add(Resources.LoadAll<AudioClip>(_targetAudioPath));

        targetSentences = Resources.LoadAll<AudioClip>(_targetAudioPath);

        Debug.Log("Count: " + targetSentences.Length);

    }


    public AudioClip GetTargetSentences(int index)
    {
        if (index >= targetSentences.Length || index < 0)
            return null;

        return targetSentences[index];
    }

    public AudioClip GetDistractorStory(int talker, int storyIndex)
    {
        return null;
    }


}
