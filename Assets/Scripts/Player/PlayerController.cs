using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 20.0f;

    protected Rigidbody rigidbody;
    protected Vector2 playerInput;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 playerMovement = this.playerInput * this.speed * Time.deltaTime;

        this.rigidbody.velocity = new Vector3(playerMovement.x, this.rigidbody.velocity.y, playerMovement.y);

        if (playerMovement != Vector2.zero)
        {
            this.transform.LookAt(transform.position + new Vector3(this.rigidbody.velocity.x, 0f, this.rigidbody.velocity.z));
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        this.playerInput = context.ReadValue<Vector2>();
    }
}
