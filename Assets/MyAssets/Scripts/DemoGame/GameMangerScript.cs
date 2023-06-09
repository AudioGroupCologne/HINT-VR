using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMangerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // unload all Scenes except preload and MenuScene
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
            {
                if(SceneManager.GetSceneByBuildIndex(i).name == "VRMenuScene")
                {
                    Debug.Log("Don't unload MenuScene");
                    continue;
                }
                Debug.Log("Unload Scene " + i);
                SceneManager.UnloadSceneAsync(i);
            }
        }

        if(!SceneManager.GetSceneByName("VRMenuScene").isLoaded)
        {
            SceneManager.LoadSceneAsync("VRMenuScene");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // go back to MenuScene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // don't reload MenuScene when already active
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("VRMenuScene")) {
                SceneManager.LoadSceneAsync("VRMenuScene");
            }
        }
    }

}
