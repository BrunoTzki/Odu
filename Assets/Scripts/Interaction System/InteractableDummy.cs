using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDummy : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer _visual;
    [SerializeField] private Color _highlightOffColor;
    [SerializeField] private Color _highlightColor;
    
    public void Highlight(bool turnOn)
    {
        _visual.material.color = turnOn ? _highlightColor : _highlightOffColor;
    }

    public void Interact()
    {
        
    }
}
