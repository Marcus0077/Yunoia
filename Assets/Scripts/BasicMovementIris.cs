using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovementIris : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;

    // Movement variables
    public Rigidbody irisRB;
    private Vector2 moveDirection = Vector2.zero;
    public float moveSpeed;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    private bool isGrounded;
    private float jumpTime;

    public bool canMove;
    public bool isFrozen;

    void Awake()
    {
        playerControls = new PlayerControls();
        moveSpeed = 4.0f;
        jumpForce = 3.0f;
        canMove = true;
        isFrozen = false;
    }
    
    void FixedUpdate()
    {
        if (canMove == true)
        {
            if (isFrozen)
            {
                irisRB.constraints = RigidbodyConstraints.FreezeRotation;
                isFrozen = false;
            }
            
            MoveIris();
        }
        else
        {
            irisRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            isFrozen = true;
        }
    }

    void MoveIris()
    {
        moveDirection = move.ReadValue<Vector2>() * moveSpeed;
        irisRB.velocity = new Vector3(-moveDirection.y, irisRB.velocity.y, moveDirection.x);
        
        JumpIris();
        
        irisRB.AddForce(Physics.gravity * 1.5f * irisRB.mass); 
    }

    void JumpIris()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, 0.3f, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (jump.IsPressed() && isGrounded)
        {
            irisRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }
    
    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
}
