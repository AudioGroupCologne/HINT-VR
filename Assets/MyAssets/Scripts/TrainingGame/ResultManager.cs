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
    [SerializeField] TMPro.TextMeshProUGUI rewards;


    // Start is called before the first frame update
    void Start()
    {
        resultObj.SetActive(false);
    }

    public void setTrainingGameResults(float _average_snr, int _rewards, int _hits, int _misses, int _roundsPlayed)
    {
        hits.text = "Hits: " + _hits.ToString();
        misses.text = "Misses: " + _misses.ToString();
        total.text = "Total: " + _roundsPlayed.ToString();
        rewards.text = "Rewards: " + _rewards.ToString();
        snr.text = "SNR: " + _average_snr.ToString("#.00");
    }

    public void showResults()
    {
        resultObj.SetActive(true);
    }
}
