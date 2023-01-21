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
        Vector3 maxStepHeight = playerBottom;
        maxStepHeight.y += this.stepHeight;

        RaycastHit hitLower;
        RaycastHit hitLower45;
        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(playerBottom, this.transform.forward, out hitLower, 0.2f))
        {
            RaycastHit upperHit;
            
            // A face do objeto não está 'apontando'/'contra' o jogador, ou seja, está apontando para cima, o que não caracteriza o degrau da escada
            if (Mathf.Approximately(0f, Mathf.Round(hitLower.normal.y)) == false) 
            {
                return;
            }

            if (Physics.Raycast(maxStepHeight, this.transform.forward, out upperHit, 0.2f) == false)
            {
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        } else if (Physics.Raycast(playerBottom, this.transform.TransformDirection(1.5f,0,1), out hitLower45, 0.2f))
        {
            // A face do objeto não está 'apontando'/'contra' o jogador, ou seja, está apontando para cima, o que não caracteriza o degrau da escada
            if (Mathf.Approximately(0f, Mathf.Round(hitLower45.normal.y)) == false) 
            {
                return;
            }

            RaycastHit hitUpper45;
            if (!Physics.Raycast(maxStepHeight, this.transform.TransformDirection(1.5f,0,1), out hitUpper45, 0.2f))
            {
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        } else if (Physics.Raycast(playerBottom, this.transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, 0.2f))
        {
            // A face do objeto não está 'apontando'/'contra' o jogador, ou seja, está apontando para cima, o que não caracteriza o degrau da escada
            if (Mathf.Approximately(0f, Mathf.Round(hitLowerMinus45.normal.y)) == false) 
            {
                return;
            }

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(maxStepHeight, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, 0.2f))
            {
                this.rigidbody.position += new Vector3(0f, stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
}
