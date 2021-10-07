using UnityEngine;

internal class HighlightSelectionResponse : MonoBehaviour, ISelectionResponse
{
    // color used to indicate highlight
    [SerializeField] private Color highlightColor;
    // keep default color of selected object
    private Color _selectionColor;

    public void OnSelect(Transform selection)
    {
        // store color of selected object
        _selectionColor = selection.GetComponent<Renderer>().material.color;
        // highlight selected object
        selection.GetComponent<Renderer>().material.color = highlightColor;
    }

    public void OnDeselect(Transform selection)
    {
        // set color back to default
        selection.GetComponent<Renderer>().material.color = _selectionColor;
    }
}