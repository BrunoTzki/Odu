using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 350.0f;

    protected Rigidbody rigidbody;
    protected Vector3 playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        if (this.playerMovement != Vector3.zero)
        {
            this.rigidbody.velocity = playerMovement * this.speed * Time.deltaTime;
            this.transform.forward = this.rigidbody.velocity;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 playerInput = context.ReadValue<Vector2>();
        this.playerMovement = new Vector3(playerInput.x, 0f, playerInput.y);
    }
}
