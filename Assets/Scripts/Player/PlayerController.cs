using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed = 350;

    private Rigidbody rigidbody;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.rigidbody.velocity = this.direction * this.speed * Time.deltaTime;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        this.direction = context.ReadValue<Vector3>();
    }
}
