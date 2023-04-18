using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDummy : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer _visual;
    [SerializeField] private Color _highlightOffColor;
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _interactedColor;

    [SerializeField] private DialogueTrigger _dialogue;

    private bool _hasInteracted;
    
    public void Highlight(bool turnOn)
    {
        if (_hasInteracted) return;
        
        _visual.material.color = turnOn ? _highlightColor : _highlightOffColor;
    }

    public void Interact()
    {
        _visual.material.color = _interactedColor;

        _hasInteracted = true;

        _dialogue.TriggerDialogue();
    }
}
