using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{

    [SerializeField] GameObject resultObj;


    // Start is called before the first frame update
    void Start()
    {
        resultObj.SetActive(false);
    }

    public void showResults()
    {
        resultObj.SetActive(true);
    }
}
