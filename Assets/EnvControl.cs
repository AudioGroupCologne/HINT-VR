using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvControl : MonoBehaviour
{

    public GameObject StaticCube;
    public GameObject MovingCube;
    public bool settingStaticCube;
    public bool settingMovingCube;
    AudioSource StaticSource;
    Component MovingSource;

    bool MovingCubeOn;
    bool StaticCubeOn;
    bool VarRadiusOn;

    // Start is called before the first frame update
    void Start()
    {
        //MovingSource = MovingCube.GetComponent("AudioSource1");
        //StaticSource = StaticCube.GetComponent("AudioSource1") as AudioSource;
        //StopEnvironment();

        // PlayerPrefs can store data between scenes and closing/re-opening
        // perform a simple int to bool cast here..
        MovingCubeOn = (PlayerPrefs.GetInt("MovingOn") == 1) ? true : false;
        StaticCubeOn = (PlayerPrefs.GetInt("StaticOn") == 1) ? true : false;
        VarRadiusOn = (PlayerPrefs.GetInt("VarRadOn") == 1) ? true : false;
        StartEnvironment();
    }

    // Update is called once per frame
    void Update()
    {
        // go back to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Go from Scene: " + SceneManager.GetActiveScene().buildIndex + " to " + (SceneManager.GetActiveScene().buildIndex - 1));
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            //StopEnvironment();
        }
    }

    // Start Cube Movement, enable Sound Sources etc.
    // Make sure that everything is 'inactive' by default
    void StartEnvironment()
    {
        MovingCube.SetActive(MovingCubeOn);
        StaticCube.SetActive(StaticCubeOn);
    }

    void StopEnvironment()
    {
        MovingCube.SetActive(false);
        StaticCube.SetActive(false);
    }

}
