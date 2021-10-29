using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScroll : MonoBehaviour
{
    Button[] btns;

    // Start is called before the first frame update
    void Start()
    {
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
