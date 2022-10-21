using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovementPlayer : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction crouch;

    // Movement variables
    public Rigidbody playerRB;
    private Vector2 moveDirection = Vector2.zero;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxSpeed;
    public bool canMove;
    public bool isFrozen;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    private bool isGrounded;
    private float jumpTime;
    
    private Vector2 lookDirection = Vector2.zero;

    // Get references and initialize variables when player spawns.
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
        // If player can move, unfreeze (except rotation) player and
        // give them control.
        if (canMove == true)
        {
            if (isFrozen)
            {
                playerRB.constraints = RigidbodyConstraints.FreezeRotation;
                isFrozen = false;
            }
            
            LookPlayer();
            MovePlayer();
        }
        // If player cannot move, freeze player.
        else
        {
            playerRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                                   RigidbodyConstraints.FreezeRotation;
            isFrozen = true;
        }

        if (playerRB.velocity.magnitude > maxSpeed)
        {
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxSpeed);
        }
    }

    // Applies gravity and movement velocity to player according to movement input.
    // Also gives option to jump.
    void MovePlayer()
    {
        moveDirection = move.ReadValue<Vector2>() * moveSpeed;
        playerRB.velocity = new Vector3(moveDirection.y, playerRB.velocity.y, -moveDirection.x);
        
        JumpPlayer();
        
        playerRB.AddForce(Physics.gravity * 1.5f * playerRB.mass); 
    }

    // Applies jump force to player if they press the jump button and
    // if they are on the ground.
    void JumpPlayer()
    {
        IsGrounded();

        if (jump.IsPressed() && isGrounded)
        {
            playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }

    void LookPlayer()
    {
        if (move.IsPressed())
        {
            transform.forward = new Vector3(moveDirection.x, 0f, moveDirection.y);
        }
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    private void IsGrounded()
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
    }
}
