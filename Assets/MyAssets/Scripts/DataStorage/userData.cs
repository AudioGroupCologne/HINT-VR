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

        int userIndex = UserManagement.selfReference.getNumTests();

        singeTestResult newTest = new singeTestResult(userIndex, tmp);
        jsonFiles.saveIndividualTestResult(newTest, userIndex);
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
        //[JsonProperty] List<string> Condition;
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

        public TestData(List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRT, List<float> _hitQuote, feedbackSettings _system)
        {
            listOrder = new List<int>();
            conditionsOrder = new List<hintConditions>();
            listSRT = new List<float>();
            hitQuote = new List<float>();
            testTime = System.DateTime.Now;
            testDate = System.DateTime.Today;
            addTestResults(_listOrder, _condOrder, _listSRT, _hitQuote, _system);
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

    public class singeTestResult
    {
        [JsonProperty] int userIndex;
        [JsonProperty] TestData data;

        public singeTestResult(int _userIndex, List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRT, List<float> _hitQuote, feedbackSettings _system)
        {
            userIndex = _userIndex;
            data = new TestData(_listOrder, _condOrder, _listSRT, _hitQuote, _system);
        }

        public singeTestResult(int _userIndex, TestData _data)
        {
            userIndex = _userIndex;
            data = _data;
        }
    }

}



public class VRHintResults
{
    [JsonProperty] string testSetup;
    [JsonProperty] int userIndex;
    [JsonProperty] List<VRHintListResult> subResults;

    public VRHintResults(feedbackSettings _setup, int userIndex, List<int> _listOrder, List<hintConditions> _condOrder, List<float> _listSRTs, List<float>[] _listSNRs, List<float>[] _listHitQuotes, List<string> _time)
    {
        subResults = new List<VRHintListResult>();

        switch (_setup)
        {
            case feedbackSettings.classic:
                testSetup = "classicVR";
                break;
            case feedbackSettings.comprehensionLevel:
                testSetup = "subjective";
                break;
            case feedbackSettings.wordSelection:
                testSetup = "wordSelection";
                break;
        }

        for (int i = 0; i < _listOrder.Count; i++)
        {
            subResults.Add(new VRHintListResult(_listOrder[i], _condOrder[i], _listSRTs[i], _listSNRs[i], _listHitQuotes[i], _time[i]));
        }

    }



    public class VRHintListResult
    {
        [JsonProperty] int ListIndex;
        //[JsonProperty] System.DateTime time;
        [JsonProperty] string time;
        [JsonProperty] string condition;
        
        [JsonProperty] List<float> listSNRs;
        [JsonProperty] List<float> listHitQuotes;
        [JsonProperty] float ListAverageSNR;
        


        public VRHintListResult(int _listIndex, hintConditions _condition, float _listSRT, List<float> _listSNRs, List<float> _hitQuotes, string _time)
        {
            ListIndex = _listIndex;
            listSNRs = _listSNRs;
            listHitQuotes = _hitQuotes;
            time = _time;

            ListAverageSNR = _listSRT;
            
            switch (_condition)
            {
                case hintConditions.quiet:
                    condition = "quiet";
                    break;
                case hintConditions.noiseFront:
                    condition = "noiseFront";
                    break;
                case hintConditions.noiseLeft:
                    condition = "noiseLeft";
                    break;
                case hintConditions.noiseRight:
                    condition = "noiseRight";
                    break;
            }
            
        }
    }


}
