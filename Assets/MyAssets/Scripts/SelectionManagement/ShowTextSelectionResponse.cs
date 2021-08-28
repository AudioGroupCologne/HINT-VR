using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextSelectionResponse : MonoBehaviour, ISelectionResponse
{
    // color used to indicate highlight
    [SerializeField] private string highlightText;
    [SerializeField] private float distance;
    [SerializeField] private GameObject hText;

    //TextMesh hText;

    public void OnSelect(Transform selection)
    {
        Instantiate(hText, transform.position + transform.forward * distance, transform.rotation);
    }

    public void OnDeselect(Transform selection)
    {

    }
}
