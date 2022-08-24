using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CustomTypes
{

    public enum audioChannels{ master, target, distractor, player };
    public enum levelObjects { target, distractor1, distractor2, userInterface, camera };
    public enum levelPositions { front, left, right };

    public enum distractorSettings { noDist, dist1, dist2, bothDist };


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
        // global! (used in Training and VRHINT)
        
    }

    namespace VRHINTTypes
    {
        public enum hintConditions { quiet, noiseFront, noiseLeft, noiseRight };

        public enum feedbackSettings { classic, wordSelection, comprehensionLevel, classicDark };

        public enum comprehension { good, bad };

        public enum testOrder {first, second};
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
            bool practiceMode = false;
            [SerializeField] List<testCondition> practice;
            [SerializeField] List<testCondition> conditions;
            

            public experiment(List<testCondition> conds)
            {
                conditions = new List<testCondition>();
                conditions = conds;
            }

            public experiment(List<testCondition> practiceConds , List<testCondition> conds)
            {

                practice = new List<testCondition>();
                practice = practiceConds;

                practiceMode = true;

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

            public bool PracticeModeAvailable()
            {
                return practiceMode;
            }

            public testCondition GetPracticeConditions(int index)
            {
                if (index >= practice.Count)
                {
                    return null;
                }

                return practice[index];
            }
        }

        public enum voices { female1, female2, female3, male1, male2};
        public enum experiments { experiment1, experiment2 };
        public enum voiceConditions { sameVoice, differentFemaleVoices, differentMaleVoices };
        public enum locationConditions { sameLocation, differentLocations };
    }
}
