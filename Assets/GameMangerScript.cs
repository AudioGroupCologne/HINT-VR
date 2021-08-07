using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMangerScript : MonoBehaviour
{
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
            SceneManager.LoadSceneAsync("MenuScene");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
