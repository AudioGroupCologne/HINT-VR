using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CustomTypes
{
    namespace TrainingGameTypes
    {
        public enum testtype { test1, test2};
    }


    namespace TestSceneTypes
    {

        public class testCondition
        {
            voices voiceDist1;
            voices voiceDist2;
            locationConditions loc;

            testCondition(voices dist1, voices dist2, locationConditions locCond)
            {
                voiceDist1 = dist1;
                voiceDist2 = dist2;
                loc = locCond;
            }
        }

        public class experiment
        {
            List<testCondition> conditions;

            experiment(List<testCondition> conds)
            {
                conditions = new List<testCondition>();
                conditions = conds;
            }
        }

        public enum voices { female1, female2, female3, male1, male2};
        public enum experiments { experiment1, experiment2 };
        public enum voiceConditions { sameVoice, differentFemaleVoices, differentMaleVoices };
        public enum locationConditions { sameLocation, differentLocations };
    }
}
