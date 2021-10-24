using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUserProgress : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshProUGUI TopText;
    [SerializeField] TMPro.TextMeshProUGUI gamesPlayed;
    [SerializeField] TMPro.TextMeshProUGUI averageSNR;
    [SerializeField] TMPro.TextMeshProUGUI rewards;

    // Start is called before the first frame update

    void Start()
    {

        int _roundsPlayed;
        float _average_snr;
        int _rewards;
        string _username;
        List<float> _snrValues = new List<float>();

        UserManagement.selfReference.getUserData(out _username, out _roundsPlayed, out _rewards, out _average_snr, out _snrValues);

        TopText.text = "Progress  (" + _username + ")";
        gamesPlayed.text = "Games played: " + _roundsPlayed.ToString();
        averageSNR.text = "Average SNR: " + _average_snr.ToString("#.00");
        rewards.text = "Rewards: " + _rewards.ToString();

        GetComponentInChildren<windowGraph>().SetProgressGraph(_snrValues);
    }
}
