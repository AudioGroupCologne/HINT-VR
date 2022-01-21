using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightText : MonoBehaviour
{

    public void setText(string t)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = t;
    }
}
