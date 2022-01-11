using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Main;
    public GameObject Progress;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Main.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }

    public void ShowMainMenu()
    {
        Main.SetActive(true);
        Progress.SetActive(false);

    }

    public void ShowProgess()
    {
        Main.SetActive(false);
        Progress.SetActive(true);   
    }

}
