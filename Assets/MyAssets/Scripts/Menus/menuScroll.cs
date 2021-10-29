using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScroll : MonoBehaviour
{
    Button[] btns;

    private void Awake()
    {
        Debug.Log("Select first button");
        btns = gameObject.GetComponentsInChildren<Button>();
        StartCoroutine(initialSelection());
    }

    IEnumerator initialSelection()
    {
        yield return null;
        btns[0].Select();
        btns[0].OnSelect(null);
    }
}
