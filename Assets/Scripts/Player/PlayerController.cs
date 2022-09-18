using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float speed = 400.0f;
    [SerializeField] protected float turnSpeed = 1_000.0f;
    [SerializeField] protected float stepHeight = 0.5f;
    [SerializeField] protected float stepSmooth = 0.2f;
    [SerializeField] protected BoxCollider bodyCollider;

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
        this.HandleMove();
        this.HandleStairs();
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

    private void HandleStairs()
    {
        Vector3 playerBottom = new Vector3(this.transform.position.x, this.transform.position.y - this.bodyCollider.bounds.extents.y + 0.1f, this.transform.position.z);
        Vector3 maxStepHeight = playerBottom;
        maxStepHeight.y += this.stepHeight;
        
        RaycastHit bottomHit;
        if(Physics.Raycast(playerBottom, this.transform.forward, out bottomHit, this.bodyCollider.bounds.extents.x + 0.1f))
        {
            RaycastHit upperHit;
            if (Physics.Raycast(maxStepHeight, this.transform.forward, out upperHit, this.bodyCollider.bounds.extents.x + 0.2f) == false)
            {
                rigidbody.position += new Vector3(0f, stepSmooth, 0f);
            }
        }
    }
}
