using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CustomTypes
{

    public enum audioChannels{ master, target, distractor, player };

    public class soundEffectEntry
    {
        AudioClip sound;
        string tag;

        public soundEffectEntry(AudioClip clip, string ctag)
        {
            sound = clip;
            tag = ctag;
        }
    }


    namespace TrainingGameTypes
    {
        public enum testtype { test1, test2};
    }


    namespace TestSceneTypes
    {
        [Serializable]
        public class testCondition
        {
            public voices voiceDist1;
            public voices voiceDist2;
            public locationConditions loc;

            public testCondition(voices dist1, voices dist2, locationConditions locCond)
            {
                voiceDist1 = dist1;
                voiceDist2 = dist2;
                loc = locCond;
            }
        }


        [Serializable]
        public class experiment
        {
            [SerializeField] List<testCondition> conditions;

            public experiment(List<testCondition> conds)
            {
                conditions = new List<testCondition>();
                conditions = conds;
            }

            public experiment(voices[] dist1, voices[] dist2, locationConditions[] locs)
            {
                conditions = new List<testCondition>();
                for(int i = 0; i < dist1.Length; i++)
                {
                    testCondition tmp = new testCondition(dist1[i], dist2[i], locs[i]);
                    conditions.Add(tmp);
                }
            }

            public testCondition GetTestConditions(int index)
            {
                if(index >= conditions.Count)
                {
                    return null;
                }

                return conditions[index];
            }
        }

        public enum voices { female1, female2, female3, male1, male2};
        public enum experiments { experiment1, experiment2 };
        public enum voiceConditions { sameVoice, differentFemaleVoices, differentMaleVoices };
        public enum locationConditions { sameLocation, differentLocations };

        public enum levelObjects { target, distractor1, distractor2, userInterface};

        public enum levelPositions { front, left, right};
    }
}
