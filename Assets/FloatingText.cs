using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    TextMesh tMesh;
    //public Vector3 Offset = new Vector3(10, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        tMesh = GetComponent<TextMesh>();
        //transform.localPosition += Offset;
    }

    public void setText(string t)
    {
        tMesh.text = t;
    }

    public void selfDestruct()
    {
        Destroy(gameObject);
    }

}
