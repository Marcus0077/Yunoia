using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovementClone : MonoBehaviour
{
    // UI In Control
    public TextMeshProUGUI inControlText;

    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    private SmoothCameraFollow smoothCameraFollow;
    private SummonClone summonClone;
    private ExitClone exitClone;
    private CombatHandler combatHandler;

    
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

    public GameObject Wall_1;
    public GameObject Wall_2;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        exitClone = FindObjectOfType<ExitClone>();
        combatHandler = FindObjectOfType<CombatHandler>();

        Wall_1 = GameObject.FindWithTag("GoodWall1");
        Wall_2 = GameObject.FindWithTag("GoodWall2");

        summonClone.guidanceText.text = "Right-Click (mouse) or press 'Y' (controller) " +
                                        "to switch between the clone and the player! " +
                                        "\n\n Press 'G' (keyboard) or 'B' (controller) to exit.";
        
        summonClone.cloneVersionText.text = "The muted clone can only move and jump, " +
                                            "and cannot phase through the blue walls.";

        smoothCameraFollow.target = cloneRB.transform;
        
        playerControls = new PlayerControls();
        
        moveSpeed = 4.0f;
        jumpForce = 3.0f;
        combatHandler.cloneHP = 3;
        
        combatHandler.healthText.text = "Clone Health: " + combatHandler.cloneHP + "/3";

        cloneIsFrozen = false;
        cloneCanMove = true;
        
        inControlText = GameObject.FindGameObjectWithTag("In Control Text").GetComponent<TextMeshProUGUI>();
        inControlText.color = Color.magenta;
        inControlText.text = "In Control: Clone";
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
            inControlText.text = "In Control: Player";
            basicMovementPlayer.canMove = true;
            cloneCanMove = false;
            
            smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;
        }
        else if (cloneCanMove == false)
        { 
            inControlText.text = "In Control: Clone";
            basicMovementPlayer.canMove = false; 
            cloneCanMove = true;

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

    private void OnGUI()
    {
        if (GUI.Button(new Rect(25, 25, 125, 50), "Test Muted Clone"))
        {
            summonClone.cloneVersionText.text = "The muted clone can only move and jump, " +
                                                "and cannot phase through the blue walls.";
            
            Physics.IgnoreCollision(Wall_1.GetComponent<Collider>(), this.GetComponent<Collider>(), 
                false);
            Physics.IgnoreCollision(Wall_2.GetComponent<Collider>(), this.GetComponent<Collider>(), 
                false);
        }
        
        if (GUI.Button(new Rect(25, 100, 150, 50), "Test Restored Clone"))
        {
            summonClone.cloneVersionText.text = "The restored clone can move, jump " +
                                                "and phase through the blue walls!";
            
            Physics.IgnoreCollision(Wall_1.GetComponent<Collider>(), this.GetComponent<Collider>(), 
                true);
            Physics.IgnoreCollision(Wall_2.GetComponent<Collider>(), this.GetComponent<Collider>(), 
                true);
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
