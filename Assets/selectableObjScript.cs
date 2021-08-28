using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectableObjScript : MonoBehaviour
{
    bool isSelected = false;
    [SerializeField] string highlightText;

    private void Start()
    {
        if(highlightText.Length > 0)
        {
            GetComponent<TextMesh>().text = highlightText;
        }
    }

    public void setSelectionStatus(bool selected)
    {
        isSelected = selected;
    }

    public bool getSelectionStatus()
    {
        return isSelected;
    }
}
