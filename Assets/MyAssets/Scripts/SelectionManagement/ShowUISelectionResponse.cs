using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUISelectionResponse : MonoBehaviour, ISelectionResponseType
{
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private string defaultUIText;
    

    void Start()
    {
        selectionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = defaultUIText;
    }

    public void OnSelect(Transform selection)
    {
        // always set default text if no type is given
        selectionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = defaultUIText;
        selectionUI.SetActive(true);
    }

    public void OnDeselect(Transform selection)
    {
        selectionUI.SetActive(false);
    }

    public void OnSelect(Transform selection, string type)
    {
        if(type == "audio")
        {
            selectionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Play Audio";
        }
        selectionUI.SetActive(true);
    }
}

