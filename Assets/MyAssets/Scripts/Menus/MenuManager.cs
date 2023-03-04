using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] MainMenu main;
    [SerializeField] ProgressScreen progress;

    // Start is called before the first frame update
    void Start()
    {

        // ### only added for screen recordings
        Cursor.visible = false;

        main.progressCallback = ShowProgess;
        progress.returnCallback = ShowMainMenu;
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!main.gameObject.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }

    public void ShowMainMenu()
    {
        main.gameObject.SetActive(true);
        progress.gameObject.SetActive(false);

    }

    void ShowProgess()
    {
        main.gameObject.SetActive(false);
        progress.gameObject.SetActive(true);
    }

}
