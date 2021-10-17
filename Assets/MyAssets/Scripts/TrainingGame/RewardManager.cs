using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    [SerializeField] GameObject rewardObj;
    [SerializeField] Image[] rewardStickers;


    // Start is called before the first frame update
    void Start()
    {
        // make sure that all reward stickers are hidden
        for (int i = 0; i < rewardStickers.Length; i++)
        {
            rewardStickers[i].gameObject.SetActive(false);
        }

        rewardObj.SetActive(false);
    }

    public void showReward(int index)
    {
        if(index == 0)
        {
            rewardObj.SetActive(true);
        }

        rewardStickers[index].gameObject.SetActive(true);
    }
}
