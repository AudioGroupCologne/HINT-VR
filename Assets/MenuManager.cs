using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Main;
    public GameObject Settings;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScCnt: " + SceneManager.sceneCountInBuildSettings);
        
        for(int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
            {
                Debug.Log("Unload Scene " + i);
                SceneManager.UnloadSceneAsync(i);
            }
            
        }

        // Set MainMenu active
        Main.SetActive(true);
        // Set SettingsMenu inactive
        Settings.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
