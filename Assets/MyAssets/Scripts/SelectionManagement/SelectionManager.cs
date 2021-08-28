using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] float rayDistance = 3f;

    private ISelectionResponseType _selectionResponseType;
    private ISelectionResponse _selectionResponse;

    // keep reference of selected object
    private Transform _selection;

    private void Awake()
    {
        // this allows to simply change the behaviour an selection by usign different SelectionResponse classes inhereting ISelectionResponse
        //_selectionResponse = GetComponent<ISelectionResponse>();
        _selectionResponseType = GetComponent<ISelectionResponseType>();
    }


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
        Transform selection;

        // center of screen will always equal mouse position (FPS logic)
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
            deselectObject(ref _selection);

            // check if the object is tagged as seletable
            if (selection.CompareTag(selectableTag))
            {  
                // Handle missing Renderer()
                if (selection.GetComponent<Renderer>() == null)
                {
                    return;
                }
                
                selectObject(selection);
                // keep reference of selection to determine whether a new object is selected in the next call
                _selection = selection;
            }
        }
        // reset selection, if Raycast did not hit anything
        else
        {
            deselectObject(ref _selection);     
        }
    }

    void selectObject(Transform selection)
    {
        if (selection == null)
            return;

        Debug.Log("Select");
        // notify object about selection status
        selection.gameObject.GetComponent<SelectableObject>().setSelectionStatus(true);
        // get type from 'SelectableObject'
        string type = selection.gameObject.GetComponent<SelectableObject>().getSelectionType();

        if(type != null)
        {
            // call selectionResponse
            _selectionResponseType.OnSelect(selection, type);
        }
        else
        {
            // call selectionResponse
            _selectionResponseType.OnSelect(selection);
        }
        
    }

    void deselectObject(ref Transform selection)
    {
        if (selection == null)
            return;

        Debug.Log("Deselect");
        // set SelectionStatus back to false
        selection.gameObject.GetComponent<SelectableObject>().setSelectionStatus(false);
        // call selectionResponse
        _selectionResponseType.OnDeselect(selection);
        // delete reference
        selection = null;

    }

}
