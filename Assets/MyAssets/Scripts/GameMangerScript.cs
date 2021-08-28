using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMangerScript : MonoBehaviour
{
    public GameObject eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ScCnt: " + SceneManager.sceneCountInBuildSettings);

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (SceneManager.GetSceneByBuildIndex(i).isLoaded)
            {
                Debug.Log("Unload Scene " + i);
                SceneManager.UnloadSceneAsync(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // go back to MenuScene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC in GameManager");
            Cursor.lockState = CursorLockMode.None;

            // don't reload MenuScene when already active
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MenuScene")) {
                SceneManager.LoadSceneAsync("MenuScene");
            }
        }

        if(Input.GetKeyDown(KeyCode.AltGr))
        {
            Debug.Log("Toggle Cursor lockState");
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // maybe add an "press any key to start" (?)
    }


    private static GameObject instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Debug.Log("Destroy dublication");
            Destroy(gameObject);
        }
            
    }

}
