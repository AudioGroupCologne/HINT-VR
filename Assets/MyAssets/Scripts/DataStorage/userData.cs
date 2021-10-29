using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class userData
{
    // login data
    [JsonProperty] string username;
    [JsonProperty] string password;

    // player settings
    // this can either be 1 (A) [no binaural difference] or 2 (B) [90 degree offset between target and distractor]
    [JsonProperty] int group = 0;
    [JsonProperty] float masterVolume = 0;

    // player progress
    [JsonProperty] int gamesPlayed = 0;
    [JsonProperty] int rewardsGained = 0;
    [JsonProperty] float currentSNR = 0;

    [JsonProperty] List<float> gamesSNR;
    [JsonProperty] List<int> gamesRewards;

    // user constructor
    public userData(string _name, string _pw, int _group)
    {
        username = _name;
        password = _pw;
        group = _group;

        gamesSNR = new List<float>();
        gamesRewards = new List<int>();
    }

    public bool checkPassword(string _pw)
    {
        if(password == _pw)
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
        Debug.Log("gamesPlayed: " + gamesPlayed);
        Debug.Log("Average SNR: " + currentSNR);
        Debug.Log("rewardsGained: " + rewardsGained);
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

    public void addResult(float snr, int reward)
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

    public void getData(out string _uname, out int _games, out int _rewards, out float _averageSNR, out List<float> _snrValues)
    {
        _uname = username;
        _games = gamesPlayed;
        _rewards = rewardsGained;
        _averageSNR = currentSNR;
        _snrValues = gamesSNR;
    }


}
