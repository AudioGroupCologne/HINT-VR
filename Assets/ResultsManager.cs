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
        writeResultsOnScreen();
    }

    public void clearResults()
    {
        Debug.Log("Clear results");
        DataStorage.DemoGameTotal = 0;
        DataStorage.DemoGameHits = 0;
        DataStorage.DemoGameMisses = 0;
        writeResultsOnScreen();
    }

    private int getTotal()
    {
        return DataStorage.DemoGameTotal;
    }

    private int getHits()
    {
        return DataStorage.DemoGameHits;
    }

    private int getMisses()
    {
        return DataStorage.DemoGameMisses;
    }

    private void writeResultsOnScreen()
    {
        total.GetComponent<TMPro.TextMeshProUGUI>().text = "Total: " + getTotal().ToString();
        hits.GetComponent<TMPro.TextMeshProUGUI>().text = "Hits: " + getHits().ToString();
        misses.GetComponent<TMPro.TextMeshProUGUI>().text = "Misses: " + getMisses().ToString();
    }

}
