using UnityEngine;

internal class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
{
    // color used to indicate highlight
    [SerializeField] private Color highlightColor;
    // keep default color of selected object
    private Color _selectionColor;

    public void OnSelect(Transform selection)
    {

        /*
        var outline = selection.GetComponent<Outline>();
        if(outline != null)
        {
            outline.OutlineWidth = 10;
        }
        */


    }

    public void OnDeselect(Transform selection)
    {
        /*
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineWidth = 0;
        }
        */

    }
}