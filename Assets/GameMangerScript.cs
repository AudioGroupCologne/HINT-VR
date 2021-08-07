using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMangerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
