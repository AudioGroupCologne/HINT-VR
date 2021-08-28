using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private string type;
    bool isSelected = false;

    [System.Serializable] private enum PieceType
    {
        Hero,
        Moveable,
        Solid,
        Empty
    }

    [SerializeField] private enum tmp { a, fgd, sf};

    private PieceType teg;

    public void setSelectionStatus(bool selected)
    {
        isSelected = selected;
    }

    public bool getSelectionStatus()
    {
        return isSelected;
    }

    public string getSelectionType()
    {
        
        if (type.Length == 0)
            return null;
        
        return type;
    }

    public int getSelectionTypeEnum()
    {
        return (int)teg;
    }
}
