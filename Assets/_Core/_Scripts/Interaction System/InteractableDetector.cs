using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    private List<IInteractable> _interactablesList = new List<IInteractable>();

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IInteractable>(out var interactable);

        if (interactable is null) return;

        interactable.Highlight(true);
        _interactablesList.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent<IInteractable>(out var interactable);
        
        if (interactable is null) return;
        if (!_interactablesList.Contains(interactable)) return;

        interactable.Highlight(false);
        _interactablesList.Remove(interactable);
    }

    public bool TryGetCurrentInteractable(out IInteractable interactable)
    {
        if (_interactablesList.Count > 0)
        {
            interactable = _interactablesList[0];
            _interactablesList.Remove(interactable);
            return true;
        }

        interactable = null;
        return false;
    }
}
