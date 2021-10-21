using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUserProgress : MonoBehaviour
{

    UserManagement user;
    [SerializeField] TMPro.TextMeshProUGUI gamesPlayed;
    [SerializeField] TMPro.TextMeshProUGUI averageSNR;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("load user progress screen");
    }
}
