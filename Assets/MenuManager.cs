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
        Settings.SetActive(false);
        Main.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
