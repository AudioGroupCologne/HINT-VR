using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserManagement : MonoBehaviour
{

    public static UserManagement selfReference;
    private int activeUser = -1;

    private List<userData> userList;

    private void Start()
    {
        if(selfReference != null)
        {
            Debug.Log("UserManagement dublication!");
            return;
        }

        selfReference = this;

        userList = new List<userData>();

        if(jsonFiles.userDataAvalable())
        {
            Debug.Log("Load userList from JSON");
            jsonFiles.loadUserData(out userList);
            Debug.Log(userList.Count + " users loaded");
        }
        
    }

    public bool addUser(string name, string password, int group)
    {
        // check if username is already in use
        if (doesUserNameExist(name))
            return false;

        // construct new userData object
        userData newUser = new userData(name, password, group);
        // add newUser to userLstr
        userList.Add(newUser);
        // save updated userList to JSON
        jsonFiles.saveUserData(userList);

        // set new user as 'activeUser'
        activeUser = getUserID(name);

        return true;
    }

    public void addUserResults(float snr, int rewards)
    {
        if (activeUser < 0)
        {
            Debug.LogError("No user selected!");
            return;
        }

        userList[activeUser].addResult(snr, rewards);
        Debug.Log("Set SNR and rewards to user: " + userList[activeUser].getUserName());

        // save updated userList to JSON
        jsonFiles.saveUserData(userList);

    }

    public void changeUserVolume(float masterVol)
    {
        if (activeUser < 0)
        {
            Debug.LogError("No user selected!");
            return;
        }

        userList[activeUser].setMasterVolume(masterVol);
        Debug.Log("Set volume of user: " + userList[activeUser].getUserName());

        // save updated userList to JSON
        jsonFiles.saveUserData(userList);
    }

    public float getUserVolume()
    {
        if (activeUser < 0)
        {
            Debug.LogError("No user selected!");
            return 0.0f;
        }

        return userList[activeUser].getMasterVolume();
    }

    bool doesUserNameExist(string username)
    {
        for (int i = 0; i < userList.Count; i++)
        {
            if (username == userList[i].getUserName())
                return true;
        }
        return false;
    }

    int getUserID(string username)
    {
        for (int i = 0; i < userList.Count; i++)
        {
            if (username == userList[i].getUserName())
                return i;
        }

        return -1;
    }

    public bool userLogin(string uname, string pw)
    {
        if(checkPassword(uname, pw))
        {
            activeUser = getUserID(uname);
            return true;
        }

        return false;
    }

    public bool LoggedIn()
    {
        if (activeUser < 0)
            return false;

        return true;
    }

    public int getUserGroup()
    {
        if (!LoggedIn())
            return -1;

        return userList[activeUser].getGroup();
    }

    public void getUserData(out string username, out int gamesPlayed, out int rewards, out float averageSNR, out List<float> snrValues)
    {
        if (!LoggedIn())
        {
            username = null;
            gamesPlayed = 0;
            rewards = 0;
            averageSNR = 0.0f;
            snrValues = null;
            return;
        }

        userList[activeUser].getData(out username, out gamesPlayed, out rewards, out averageSNR, out snrValues);
    }
       

    public bool checkPassword(string username, string pw)
    {

        int userID = getUserID(username);

        if (userID == -1)
        {
            Debug.Log("User does not exist!");
            return false;
        }

        return userList[userID].checkPassword(pw);
    }
}
