using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;



public static class jsonFiles
{
    
    public static void saveUserData(List<userData> data)
    {
        Debug.Log("Save JSON");
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string targetPath = Application.persistentDataPath + "/userData.json";

        File.WriteAllText(targetPath, jsonData);
    }
    

    public static void loadUserData(out List<userData> data)
    {
        Debug.Log("Load JSON");
        string targetPath = Application.persistentDataPath + "/userData.json";

        if (!File.Exists(targetPath))
        {
            Debug.Log("File not found");
            data = null;
            return;
        }
        string readData = File.ReadAllText(targetPath);

        data = JsonConvert.DeserializeObject<List<userData>>(readData);
    }

    public static bool userDataAvalable()
    {
        Debug.Log("Check JSON");
        string targetPath = Application.persistentDataPath + "/userData.json";

        if (!File.Exists(targetPath))
        {
            Debug.Log("File not found");
            
            return false;
        }

        return true;
    }

    public static void saveVRHintResults(VRHintResults data, int userIndex)
    {
        Debug.Log("Save JSON");
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string targetPath = Application.persistentDataPath + "/testResults/" + userIndex + "-testUserData.json";
        File.WriteAllText(targetPath, jsonData);
    }

    public static void saveTestUserData(List<testUserData> data)
    {
        Debug.Log("Save JSON");
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string targetPath = Application.persistentDataPath + "/testUserData.json";
        File.WriteAllText(targetPath, jsonData);
    }

    public static void saveIndividualTestResult(userData.singeTestResult data, int userIndex)
    {
        Debug.Log("Save individual test results to JSON");
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        string targetPath = Application.persistentDataPath + "/testResults/" + userIndex +  "-testUserData.json";
        File.WriteAllText(targetPath, jsonData);
    }


    public static void loadTestUserData(out List<testUserData> data)
    {
        Debug.Log("Load JSON");
        string targetPath = Application.persistentDataPath + "/testUserData.json";

        if (!File.Exists(targetPath))
        {
            Debug.Log("File not found");
            data = null;
            return;
        }
        string readData = File.ReadAllText(targetPath);

        data = JsonConvert.DeserializeObject<List<testUserData>>(readData);
    }

    public static bool testUserDataAvalable()
    {
        Debug.Log("Check JSON");
        string targetPath = Application.persistentDataPath + "/testUserData.json";

        if (!File.Exists(targetPath))
        {
            Debug.Log("File not found");

            return false;
        }

        return true;
    }


}
