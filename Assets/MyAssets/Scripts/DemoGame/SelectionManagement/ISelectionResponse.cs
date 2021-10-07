using UnityEngine;

internal interface ISelectionResponse
{
    void OnSelect(Transform selection);

    void OnDeselect(Transform selection);

}

// extend option to use Types through a separate interface
internal interface ISelectionResponseType : ISelectionResponse
{
    void OnSelect(Transform selection, string type);
}
