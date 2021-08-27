using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    bool isSelected = false;

    public void setSelectionStatus(bool selected)
    {
        isSelected = selected;
    }

    public bool getSelectionStatus()
    {
        return isSelected;
    }

}
