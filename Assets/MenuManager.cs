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
        // get number of loadedScenes
        int countLoaded = SceneManager.sceneCount;
        Debug.Log("countLoaded: " + countLoaded);
        //Scene[] loadedScenes = new Scene[countLoaded];

        // unload all Scenes except MenuScene (0)
        for (int i = 1; i < countLoaded; i++)
        {
            //loadedScenes[i] = SceneManager.GetSceneAt(i);
            SceneManager.UnloadSceneAsync(i);
        }

        Debug.Log("Active Scene: " + SceneManager.GetActiveScene().buildIndex);

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
