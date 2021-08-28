using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUISelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private string UIText;

    void Start()
    {
        selectionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = UIText;
    }

    public void OnSelect(Transform selection)
    {
        selectionUI.SetActive(true);
    }

    public void OnDeselect(Transform selection)
    {
        selectionUI.SetActive(false);
    }
}

