using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{

    [SerializeField] Image[] rewardStickers;


    // Start is called before the first frame update
    void Start()
    {
        // make sure that all reward stickers are hidden
        for(int i = 0; i < rewardStickers.Length; i++)
        {
            rewardStickers[i].gameObject.SetActive(false);
        }
    }

    public void showReward(int index)
    {
        rewardStickers[index].gameObject.SetActive(true);
    }

}
