using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomTypes.VRHINTTypes;

public class OverviewManager : MonoBehaviour
{

    [SerializeField] GameObject overviewUI;
    [SerializeField] TMPro.TextMeshProUGUI practice;
    [SerializeField] TMPro.TextMeshProUGUI rounds;
    [SerializeField] TMPro.TextMeshProUGUI lists;
    [SerializeField] TMPro.TextMeshProUGUI cond;
    [SerializeField] TMPro.TextMeshProUGUI listIndex;


    public void ShowPractice(bool show)
    {
        Debug.Log("Show overview " + show);
        practice.gameObject.SetActive(show);
    }


    public void ShowOverview(bool show)
    {
        overviewUI.gameObject.SetActive(show);
    }

    public void SetRounds(int _round, int _totalRounds)
    {
        rounds.text = "Round " + _round + " of " + _totalRounds;
    }

    public void SetLists(int _list, int _totalLists)
    {
        lists.text = "List " + _list + " of " + _totalLists;
     
    }

    public void SetCond(hintConditions _cond)
    {
        switch(_cond)
        {
            case hintConditions.quiet:
                cond.text = "quiet";
                break;
            case hintConditions.noiseFront:
                cond.text = "noiseFront";
                break;
            case hintConditions.noiseLeft:
                cond.text = "noiseLeft";
                break;
            case hintConditions.noiseRight:
                cond.text = "noiseRight";
                break;
            default:
                Debug.LogError("Invalid entry!");
                return;

        }
        
    }

    public void SetListIndex(int _listIndex)
    {
        listIndex.text = "ListIndex: " + _listIndex;
    }



}
