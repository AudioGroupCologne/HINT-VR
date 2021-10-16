using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{

    [SerializeField] GameObject resultObj;
    [SerializeField] TMPro.TextMeshProUGUI hits;
    [SerializeField] TMPro.TextMeshProUGUI misses;
    [SerializeField] TMPro.TextMeshProUGUI total;
    [SerializeField] TMPro.TextMeshProUGUI snr;


    // Start is called before the first frame update
    void Start()
    {
        resultObj.SetActive(false);
    }

    public void showResults()
    {
        setResults();
        resultObj.SetActive(true);
    }

    public void clearResults()
    {
        Debug.Log("Clear results");
        DataStorage.TrainingGame_Total = 0;
        DataStorage.TrainingGame_Hits = 0;
        DataStorage.TrainingGame_Misses = 0;
        showResults();
    }

    private void setResults()
    {

        float snr_avg = 0;

        hits.text = "Hits: " + DataStorage.TrainingGame_Hits.ToString();
        misses.text = "Misses: " + DataStorage.TrainingGame_Misses.ToString();
        total.text = "Total: " + DataStorage.TrainingGame_Total.ToString();

        for(int i = 0; i < DataStorage.TrainingGame_SNR.Length; i++)
        {
            snr_avg += DataStorage.TrainingGame_SNR[i];
        }

        snr_avg /= DataStorage.TrainingGame_Total;
        Debug.Log("SNR avg: " + snr_avg);

        snr.text = "SNR: " + snr_avg.ToString("#.00");
        Debug.Log(snr.text);
    }
}
