using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private InteractableDetector _detector;
    
    private PlayerInputActions _inputs;

    private void Awake()
    {
        _inputs = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputs.Enable();

        _inputs.Main.Interact.performed += TryInteract;
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        if (_detector.TryGetCurrentInteractable(out var interactable))
        {
            interactable.Interact();
        }
    }
}
