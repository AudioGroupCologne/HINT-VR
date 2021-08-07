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
