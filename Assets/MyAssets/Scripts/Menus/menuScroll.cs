using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScroll : MonoBehaviour
{
    Button[] btns;
    int selectedBtn = 0;

    // ToDo: make this work with Oculus Controls
    // ToDo: write somethings similar for word selection

    // Start is called before the first frame update
    void Start()
    {
        btns = gameObject.GetComponentsInChildren<Button>();
        Debug.Log("Button count: " + btns.Length);
        btns[selectedBtn].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (++selectedBtn > (btns.Length - 1))
            {
                selectedBtn = 0;
            }
            btns[selectedBtn].Select();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (--selectedBtn < 0)
            {
                selectedBtn = (btns.Length - 1);
            }
            btns[selectedBtn].Select();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Invoke button " + selectedBtn);
            btns[selectedBtn].onClick.Invoke();
        }
    }

}
