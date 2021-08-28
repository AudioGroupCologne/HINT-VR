using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcomp : MonoBehaviour
{
    bool isSelected = false;
    [SerializeField] string highlightText;

    public void setSelectionStatus(bool selected)
    {
        isSelected = selected;
    }

    public bool getSelectionStatus()
    {
        return isSelected;
    }
}
