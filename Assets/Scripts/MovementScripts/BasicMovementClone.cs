using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovementClone : MonoBehaviour
{
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    private SmoothCameraFollow smoothCameraFollow;
    private SummonClone summonClone;
    private ExitClone exitClone;
    private CombatHandler combatHandler;

    public GameObject Player;
    
    // Input variables
    PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction switchPlaces;

    // Movement variables
    public Rigidbody cloneRB;
    private Vector2 moveDirection = Vector2.zero;
    public float moveSpeed;
    private bool cloneIsFrozen;
    private bool cloneCanMove;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    private bool isGrounded;
    private float jumpTime;

    // Temp object references
    public GameObject Wall_1;
    public GameObject Wall_2;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        exitClone = FindObjectOfType<ExitClone>();
        combatHandler = FindObjectOfType<CombatHandler>();

        Wall_1 = GameObject.FindWithTag("GoodWall1");
        Wall_2 = GameObject.FindWithTag("GoodWall2");

        smoothCameraFollow.target = cloneRB.transform;
        
        playerControls = new PlayerControls();
        
        moveSpeed = 4.0f;
        jumpForce = 3.0f;
        combatHandler.cloneHP = 3;
        
        //combatHandler.healthText.text = "Clone Health: " + combatHandler.cloneHP + "/3";

        cloneIsFrozen = false;
        cloneCanMove = true;
    }
    
    void FixedUpdate()
    {
        // If clone can move, unfreeze (except rotation) clone and
        // give them control.
        if (cloneCanMove == true)
        {
            if (cloneIsFrozen)
            {
                cloneRB.constraints = RigidbodyConstraints.FreezeRotation;
                cloneIsFrozen = false;
            }
            
            LookClone();
            MoveClone();
        }
        // If clone cannot move, freeze clone.
        else
        {
            cloneRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                                   RigidbodyConstraints.FreezeRotation;
            cloneIsFrozen = true;
        }

        if (combatHandler.cloneHP <= 0)
        {
            exitClone.despawnClone = true;
        }
    }

    private void Update()
    {
        // Switch player and clone positions if switchPlaces button is pressed.
        if (switchPlaces.WasPressedThisFrame())
        {
            SwitchPlaces();
        }
    }

    // Applies gravity and movement velocity to player according to movement input.
    // Also gives option to jump.
    void MoveClone()
    {
        moveDirection = move.ReadValue<Vector2>() * moveSpeed;
        cloneRB.velocity = new Vector3(-moveDirection.y, cloneRB.velocity.y, moveDirection.x);
        
        JumpClone();
        
        cloneRB.AddForce(Physics.gravity * 1.5f * cloneRB.mass); 
    }

    // Applies jump force to player if they press the jump button and
    // if they are on the ground.
    void JumpClone()
    {
        IsGrounded();

        if (jump.IsPressed() && isGrounded)
        {
            cloneRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }
    
    void LookClone()
    {
        if (move.IsPressed())
            transform.forward = new Vector3(moveDirection.x, 0f, moveDirection.y);
    }

    // Switch clone and player control depending on which is
    // currently in control.
    // Applies 3 second cooldown timer to switch.
    private void SwitchPlaces()
    {
        if (basicMovementPlayer.canMove == false)
        {
            basicMovementPlayer.canMove = true;
            cloneCanMove = false;
            
            this.GetComponent<Grapple>().enabled = false;
            this.GetComponent<AbilityPush>().enabled = false;
            
            Player.GetComponent<Grapple>().enabled = true;
            Player.GetComponent<AbilityPush>().enabled = true;
            
            smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;
        }
        else if (cloneCanMove == false)
        {
            basicMovementPlayer.canMove = false; 
            cloneCanMove = true;
            
            this.GetComponent<Grapple>().enabled = true;
            this.GetComponent<AbilityPush>().enabled = true;
            
            Player.GetComponent<Grapple>().enabled = false;
            Player.GetComponent<AbilityPush>().enabled = false;

            smoothCameraFollow.target = cloneRB.transform;
        }
    }

    // Enable input action map controls.
    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();

        switchPlaces = playerControls.SummonClone.SwitchPlaces;
        switchPlaces.Enable();
    }
    
    // Disable input action map controls.
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        switchPlaces.Disable();
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
