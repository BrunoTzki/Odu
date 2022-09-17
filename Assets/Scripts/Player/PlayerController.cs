using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 400.0f;
    [SerializeField] private float turnSpeed = 1_000.0f;

    protected Rigidbody rigidbody;
    protected Vector3 playerInput;
    protected Matrix4x4 deformacaoPlano = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

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
        Vector3 skewedInput = this.deformacaoPlano.MultiplyPoint3x4(this.playerInput);
        Vector3 playerMovement = skewedInput * this.speed * Time.deltaTime;

        this.rigidbody.velocity = new Vector3(playerMovement.x, this.rigidbody.velocity.y, playerMovement.z);

        if (playerMovement != Vector3.zero)
        {
            Vector3 direction = (transform.position + playerMovement) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, this.turnSpeed * Time.deltaTime);
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        this.playerInput = new Vector3(rawInput.x, 0f, rawInput.y);
    }
}
