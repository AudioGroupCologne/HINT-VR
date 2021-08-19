using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public GameObject total;
    public GameObject hits;
    public GameObject misses;

    void Start()
    {
        total.GetComponent<TMPro.TextMeshProUGUI>().text = "Total: " + getTotal().ToString();
        hits.GetComponent<TMPro.TextMeshProUGUI>().text = "Hits: " + getHits().ToString();
        misses.GetComponent<TMPro.TextMeshProUGUI>().text = "Misses: " + getMisses().ToString();
    }

    public void clearResults()
    {
        Debug.Log("Clear results");
        PlayerPrefs.SetInt("total", 0);
        PlayerPrefs.SetInt("hits", 0);
        PlayerPrefs.SetInt("misses", 0);
    }

    private int getTotal()
    {
        return PlayerPrefs.GetInt("total");
    }

    private int getHits()
    {
        return PlayerPrefs.GetInt("hits");
    }

    private int getMisses()
    {
        return PlayerPrefs.GetInt("misses");
    }


}
