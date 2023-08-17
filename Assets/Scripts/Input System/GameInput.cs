using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnInteractAction;

    [Header("Character Input Values")]
    [SerializeField] private Vector2 _move;
    [SerializeField] private bool _jump;
	[SerializeField] private bool _sprint;

    [Header("Mouse Cursor Settings")]
    [SerializeField] private bool _cursorLocked = true;

    private void Awake(){
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Main.Enable();

        _playerInputActions.Main.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj){
            OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
            _playerInputActions.Main.Interact.performed -= Interact_performed;

            _playerInputActions.Dispose();
    }

    public Vector2 GetMove(){
        _move = _playerInputActions.Main.Move.ReadValue<Vector2>();
        return _move;
    }

    public bool IsJumping(){
        _jump = _playerInputActions.Main.Jump.triggered;
        return _jump;
    }

    public bool IsDashing(){
        _sprint = _playerInputActions.Main.Dash.triggered;
        return _sprint;
    }

    private void OnApplicationFocus(bool hasFocus){
        SetCursorLockState(_cursorLocked);
    }

    public void SetCursorLockState(bool newState){
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
