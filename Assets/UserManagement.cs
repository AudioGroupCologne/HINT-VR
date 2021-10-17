using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManagement : MonoBehaviour
{
    class userEntry
    {
        string name;
        string password;

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
    }

    // UserSelection could have this component, bc. this is (for now) more or less the only instance where the userselection matters
    // But TrainingGame would also need user to know where/how to store data
    // Also the "Progress" screen needs to know the user.
    // Simply set currentUserID in __app (?)
    // This could be / together with some settings etc. the only data needed across multiple scenes/classes...

    List<userEntry> users;

    public bool AddUser(string name, string password)
    {
        // check if username is already in use
        if (DoesUserNameExist(name))
            return false;

        userEntry newUser = new userEntry(name, password);
        users.Add(newUser);

        return true;
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
