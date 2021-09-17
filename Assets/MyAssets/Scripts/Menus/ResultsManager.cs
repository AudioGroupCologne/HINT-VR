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
        DataStorage.TrainingGame_Total = 0;
        DataStorage.TrainingGame_Hits = 0;
        DataStorage.TrainingGame_Misses = 0;
        writeResultsOnScreen();
    }

    private int getTotal()
    {
        return DataStorage.TrainingGame_Total;
    }

    private int getHits()
    {
        return DataStorage.TrainingGame_Hits;
    }

    private int getMisses()
    {
        return DataStorage.TrainingGame_Misses;
    }

    private void writeResultsOnScreen()
    {
        total.GetComponent<TMPro.TextMeshProUGUI>().text = "Total: " + getTotal().ToString();
        hits.GetComponent<TMPro.TextMeshProUGUI>().text = "Hits: " + getHits().ToString();
        misses.GetComponent<TMPro.TextMeshProUGUI>().text = "Misses: " + getMisses().ToString();
    }

}
