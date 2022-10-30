using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovementPlayer : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction move;
    public InputAction jump;
    public InputAction crouch;

    // Movement variables
    public Rigidbody playerRB;
    public Vector2 moveDirection = Vector2.zero;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float maxSpeed;
    public bool canMove;
    public bool isFrozen;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    public bool isGrounded;
    public float jumpTime;

    public Vector2 lookDirection = Vector2.zero;
    
    public GameObject Blocker1;
    public GameObject Blocker2;

    public int attachedMinionCount;
    float logFormulaCoefficient;
    float logFormulaModifier;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        playerControls = new PlayerControls();
        
        moveSpeed = 4.0f;
        jumpForce = 1.2f;
        
        canMove = true;
        isFrozen = false;
        
        Blocker1 = GameObject.FindWithTag("Blocker1");
        Blocker2 = GameObject.FindWithTag("Blocker2");

        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;
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
        moveDirection = move.ReadValue<Vector2>() * moveSpeed / (1 + CalcMinionMoveChange());
        playerRB.velocity = new Vector3(moveDirection.y, playerRB.velocity.y, -moveDirection.x);
        
        JumpPlayer();

        playerRB.AddForce(Physics.gravity * (1.5f + CalcMinionMoveChange()) * playerRB.mass);
    }

    float CalcMinionMoveChange()
    {
        return Mathf.Max(0,Mathf.Pow(attachedMinionCount * logFormulaCoefficient, logFormulaModifier));
    }

    public void AddMinion(int value)
    {
        attachedMinionCount += value;
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockerTrigger1"))
        {
            Blocker1.GetComponent<MeshRenderer>().enabled = false;
            Blocker1.GetComponent<Collider>().enabled = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlockerTrigger1"))
        {
            Blocker1.GetComponent<MeshRenderer>().enabled = true;
            Blocker1.GetComponent<Collider>().enabled = true;
        }
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
