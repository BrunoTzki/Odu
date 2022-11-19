using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    private IInteractable _currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<IInteractable>(out var interactable);

        if (interactable is null) return;

        Debug.Log(other.name);
        _currentInteractable = interactable;
        _currentInteractable.Highlight(true);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent<IInteractable>(out var interactable);
        
        if (interactable is null) return;
        if (!interactable.Equals(_currentInteractable)) return;

        Debug.Log("Saiu: " + other.name);
        _currentInteractable.Highlight(false);
        _currentInteractable = null;
    }
}
