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

        Debug.Log(other.name);
        
        interactable.Highlight(true);
        _interactablesList.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent<IInteractable>(out var interactable);
        
        if (interactable is null) return;
        if (!_interactablesList.Contains(interactable)) return;

        Debug.Log("Saiu: " + other.name);
        interactable.Highlight(false);
        _interactablesList.Remove(interactable);
    }
}
