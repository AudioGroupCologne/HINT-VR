using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Color highlightColor;
    [SerializeField] float rayDistance = 3f;

    // reference of current selection
    private Transform selection;
    // keep reference of selected object
    private Transform _selection;
    // keep default color of selected object
    private Color _selectionColor;
    

    // Update is called once per frame
    void Update()
    {
        RayCastSingle();
    }

    // add a function to be called if an object is hit via raycast
    void RayCastSingle()
    {
        Ray ray;
        RaycastHit hit;

        // center of screen will always equal mouse position (FPS logic)
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay();

        // RayCast returns true if a hit was made
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // get transform of RaycastHit
            selection = hit.transform;

            // check if 'selection' holds a new object or an already familiar one
            if(selection == _selection)
            {
                return;
            }

            // if a new object is selected, deselect the old one (this might happen if a RayCast goes directly from one object to another within the duration of one frame
            deselectObject();

            // check if the object is tagged as seletable
            if (selection.CompareTag(selectableTag))
            {  
                // Handle missing Renderer()
                if (selection.GetComponent<Renderer>() == null)
                {
                    return;
                }

                selectObject();
            }
        }
        // reset selection, if Raycast did not hit anything
        else
        {
            deselectObject();
        }
    }


    void selectObject()
    {
        Debug.Log("SELECT");
        // notify object about selection status
        // maybe use this instead of tag (?)
        selection.gameObject.GetComponent<SelectableObject>().setSelectionStatus(true);     

        // store refence to selected object
        _selection = selection;
        // store color of selected object
        _selectionColor = selection.GetComponent<Renderer>().material.color;
        // highlight selected object
        selection.GetComponent<Renderer>().material.color = highlightColor;
    }

    void deselectObject()
    {
        if (_selection != null)
        {
            Debug.Log("DESELECT");
            selection.gameObject.GetComponent<SelectableObject>().setSelectionStatus(false);
            _selection.GetComponent<Renderer>().material.color = _selectionColor;
            _selection = null;
        }
    }


}
