using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected float speed = 400.0f;
    [SerializeField] protected float turnSpeed = 1_000.0f;
    [SerializeField] protected float stepHeight = 0.5f;
    [SerializeField] protected float stepSmooth = 100f;
    [SerializeField] protected Transform bottomChecker;
    [SerializeField] protected BoxCollider bodyCollider;
    [SerializeField] protected GroundChecker groundChecker;
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

    protected void HandleMove()
    {
         Vector3 skewedInput = this.deformacaoPlano.MultiplyPoint3x4(this.playerInput);
        Vector3 playerMovement = skewedInput * this.speed * Time.deltaTime;

        if (this.groundChecker.IsOnFloor())
        {
            this.rigidbody.velocity = new Vector3(playerMovement.x, this.rigidbody.velocity.y, playerMovement.z);
        }

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

    protected void HandleStairs()
    {
        Vector3 playerBottom = this.bottomChecker.position;
        RaycastHit bottomHit;
        Vector3 maxStepHeight = playerBottom;
        maxStepHeight.y += this.stepHeight;

        if (Physics.Raycast(playerBottom, this.transform.forward, out bottomHit, 0.5f))
        {
            RaycastHit upperHit;

            if (Physics.Raycast(maxStepHeight, this.transform.forward, out upperHit, 0.5f) == false)
            {
                Debug.Log("Subir Degrau");
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(playerBottom, this.transform.TransformDirection(1.5f,0,1), out hitLower45, 0.2f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(maxStepHeight, this.transform.TransformDirection(1.5f,0,1), out hitUpper45, 0.2f))
            {
                Debug.Log("Subir Degrau");
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(playerBottom, this.transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, 0.2f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(maxStepHeight, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, 0.2f))
            {
                Debug.Log("Subir Degrau");
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
}
