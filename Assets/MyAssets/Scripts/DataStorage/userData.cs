using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

using CustomTypes.VRHINTTypes;

public class userData
{
    // login data
    [JsonProperty] string username;
    [JsonProperty] string password;

    // player settings
    // this can either be 1 (A) [no binaural difference] or 2 (B) [90 degree offset between target and distractor]
    [JsonProperty] int group = 0;
    [JsonProperty] float masterVolume = 0;


    [JsonProperty] TrainingData trainingData;
    [JsonProperty] List<TestData> testData;



    // user constructor
    public userData(string _name, string _pw, int _group)
    {
        username = _name;
        password = _pw;
        group = _group;

        trainingData = new TrainingData();
        testData = new List<TestData>();
    }

    public bool checkPassword(string _pw)
    {
        if (password == _pw)
        {
            return true;
        }

        return false;
    }

    public string getUserName()
    {
        return username;
    }

    public void printUserData()
    {
        Debug.Log("Name: " + username);
        Debug.Log("Group: " + group);
        Debug.Log("gamesPlayed: " + trainingData.getGamesPlayed());
        Debug.Log("Average SNR: " + trainingData.getCurrentSNR());
        Debug.Log("rewardsGained: " + trainingData.getRewardsGained());
    }

    public void setGroup(char _group)
    {
        group = _group;
    }

    public int getGroup()
    {
        return group;
    }

    public void setMasterVolume(float _vol)
    {
        masterVolume = _vol;
    }

    public float getMasterVolume()
    {
        return masterVolume;
    }

    public void addTrainingProgress(float SNR, int rewards)
    {
        trainingData.addData(SNR, rewards);
    }

    public void getTrainingData(out string _uname, out int _games, out int _rewards, out float _averageSNR, out List<float> _snrValues)
    {
        _uname = username;
        trainingData.getData(out _games, out _rewards, out _averageSNR, out _snrValues);
    }

    public void addTestResults(List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRT, List<float> _hitQuote, feedbackSettings _system)
    {
        TestData tmp = new TestData();
        tmp.addTestResults(_listOrder, _condOrder, _listSRT, _hitQuote, _system);
        testData.Add(tmp);
    }


    public class TrainingData
        {
        // player progress
        [JsonProperty] int gamesPlayed = 0;
        [JsonProperty] int rewardsGained = 0;
        [JsonProperty] float currentSNR = 0;

        [JsonProperty] List<float> gamesSNR;
        [JsonProperty] List<int> gamesRewards;

        public TrainingData()
        {
            gamesSNR = new List<float>();
            gamesRewards = new List<int>();
        }

        public void addData(float snr, int reward)
        {

            gamesPlayed++;
            rewardsGained += reward;

            gamesSNR.Add(snr);
            gamesRewards.Add(reward);

            currentSNR = 0;
            for (int i = 0; i < gamesSNR.Count; i++)
            {
                currentSNR += gamesSNR[i];
            }
            currentSNR /= gamesSNR.Count;

        }

        public void getData(out int _games, out int _rewards, out float _averageSNR, out List<float> _snrValues)
        {
            _games = gamesPlayed;
            _rewards = rewardsGained;
            _averageSNR = currentSNR;
            _snrValues = gamesSNR;
        }

        public int getGamesPlayed()
        {
            return gamesPlayed;
        }

        public int getRewardsGained()
        {
            return rewardsGained;
        }

        public float getCurrentSNR()
        {
            return currentSNR;
        }
    }


    public class TestData
    {
        // order of sentence lists of test procedure
        [JsonProperty] List<int> listOrder;
        // order of conditions
        [JsonProperty] List<hintConditions> conditionsOrder;
        // SRT for each test list
        [JsonProperty] List<float> listSRT;
        // hit quote for each sentence
        [JsonProperty] List<float> hitQuote;

        [JsonProperty] System.DateTime testTime;
        [JsonProperty] System.DateTime testDate;
        [JsonProperty] feedbackSettings feedbackSystem;


        // user constructor
        public TestData()
        {
            listOrder = new List<int>();
            conditionsOrder = new List<hintConditions>();
            listSRT = new List<float>();
            hitQuote = new List<float>();
        }

        public void addTestResults(List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRT, List<float> _hitQuote, feedbackSettings _system)
        {

            listOrder = _listOrder;
            conditionsOrder = _condOrder;
            listSRT = _listSRT;
            hitQuote = _hitQuote;
            feedbackSystem = _system;
            testTime = System.DateTime.Now;
            testDate = System.DateTime.Today;

        }


        public void getData(out List<int> _listOrder, out List<hintConditions> _condOrder, out List<float> _listSRT, out List<float> _hitQuote)
        {
            _listOrder = listOrder;
            _condOrder = conditionsOrder;
            _listSRT = listSRT;
            _hitQuote = hitQuote;
        }

    }

}
