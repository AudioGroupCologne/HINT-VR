using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserManagement : MonoBehaviour
{

    public static UserManagement usrMng;
    private int activeUser = -1;
    // move this into an individual class/file
    // add userResults to this entry:
    // totalGames
    // snr[totalGames]
    // totalRewards
    // rewards[totalGames]
    [System.Serializable]
    class userEntry
    {
        string name;
        string password;

        int gamesPlayed;
        int rewardsGained;
        // rewards gained at a particular game
        List<int> rewards;
        // average SNR of a particular game
        List<float> avg_snr;

        public userEntry(string uname, string upw)
        {
            name = uname;
            password = upw;
        }

        public string getUserName()
        {
            return name;
        }

        public bool checkPassword(string pw)
        {
            if (password == pw)
                return true;

            return false;
        }

        public void addResult(float snr, int reward)
        {
            avg_snr.Add(snr);
            rewards.Add(reward);
            gamesPlayed++;
            rewardsGained += reward;
        }

    }

    // UserSelection could have this component, bc. this is (for now) more or less the only instance where the userselection matters
    // But TrainingGame would also need user to know where/how to store data
    // Also the "Progress" screen needs to know the user.
    // Simply set currentUserID in __app (?)
    // This could be / together with some settings etc. the only data needed across multiple scenes/classes...

    List<userEntry> users;

    private void Start()
    {
        usrMng = this;
        users = new List<userEntry>();
        if(loadUserListFromFile())
        {
            Debug.Log(users.Count + " users loaded...");
        }
        
    }

    private void saveUserListToFile()
    {
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        string targetPath = Application.persistentDataPath + "/users.dat";

        if (File.Exists(targetPath))
        {
            file = File.OpenWrite(targetPath);
        }
        else 
        {
            file = File.Create(targetPath);
        }

        bf.Serialize(file, users);
        file.Close();
        
        
    }

    private bool loadUserListFromFile()
    {

        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        string targetPath = Application.persistentDataPath + "/users.dat";

        if (File.Exists(targetPath))
        {
            file = File.OpenRead(targetPath);
        }
        else
        {
            Debug.Log("File not found");
            return false;
        }

        users = (List<userEntry>)bf.Deserialize(file);
        file.Close();
        return true;
    }

    public bool AddUser(string name, string password)
    {
        // check if username is already in use
        if (DoesUserNameExist(name))
            return false;

        userEntry newUser = new userEntry(name, password);
        users.Add(newUser);
        saveUserListToFile();

        activeUser = GetUserID(name);

        return true;
    }

    public void AddUserResults(float snr, int rewards)
    {
        if (activeUser < 0)
        {
            Debug.LogError("No user selected!");
            return;
        }
            
        users[activeUser].addResult(snr, rewards);
        Debug.Log("Set SNR and rewards to user: " + users[activeUser].getUserName());

    }

    bool DoesUserNameExist(string username)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (name == users[i].getUserName())
                return true;
        }

        return false;
    }

    int GetUserID(string username)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (username == users[i].getUserName())
                return i;
        }

        return -1;
    }

    public bool UserLogin(string uname, string pw)
    {
        if(CheckPassword(uname, pw))
        {
            activeUser = GetUserID(uname);
            return true;
        }

        return false;
    }


    public bool CheckPassword(string username, string pw)
    {

        int userID = GetUserID(username);

        if (userID == -1)
        {
            Debug.Log("User does not exist!");
            return false;
        }

        return users[userID].checkPassword(pw);
    }
}
