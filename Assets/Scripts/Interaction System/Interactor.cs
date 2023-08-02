using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private InteractableDetector _detector;
    
    //private PlayerInputActions _inputs;

    private void Awake()
    {
        //_inputs = new PlayerInputActions();
    }

    private void Start() {
        GameInput.Instance.OnInteractAction += TryInteract;
    }

    private void OnEnable()
    {
        //_inputs.Enable();

        //_inputs.Main.Interact.performed += TryInteract;
        //GameInput.Instance.OnInteractAction += TryInteract;
    }

    public void TryInteract(object sender, System.EventArgs e)
    {
        if (_detector.TryGetCurrentInteractable(out var interactable))
        {
            interactable.Interact();
        }
    }

    public void TryInteractX()
    {
        if (_detector.TryGetCurrentInteractable(out var interactable))
        {
            interactable.Interact();
        }
    }
}
