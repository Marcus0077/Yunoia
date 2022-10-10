using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovementClone : MonoBehaviour
{
    // UI Cooldown Timer
    public TextMeshProUGUI cooldownTimerText;
    
    // UI In Control
    public TextMeshProUGUI inControlText;
    
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    
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

    // Determines if clone can switch positions with player
    private bool canSwitch;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        
        playerControls = new PlayerControls();
        
        moveSpeed = 4.0f;
        jumpForce = 3.0f;

        cloneIsFrozen = false;
        cloneCanMove = true;
        canSwitch = true;
        
        cooldownTimerText = GameObject.FindGameObjectWithTag("Cooldown Timer").GetComponent<TextMeshProUGUI>();
        cooldownTimerText.text = "Cooldown Timer: ";
        
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
            
            MoveClone();
        }
        // If clone cannot move, freeze clone.
        else
        {
            cloneRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            cloneIsFrozen = true;
        }

        // Switch player and clone positions if switchPlaces button is pressed.
        if (switchPlaces.IsPressed() && canSwitch)
        {
            StartCoroutine(SwitchPlaces());
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
            cloneRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }

    // Switch clone and player control depending on which is
    // currently in control.
    // Applies 3 second cooldown timer to switch.
    private IEnumerator SwitchPlaces()
    {
        canSwitch = false;
        
        if (basicMovementPlayer.canMove == false)
        {
            inControlText.text = "In Control: Player";
            basicMovementPlayer.canMove = true;
            cloneCanMove = false;
        }
        else if (cloneCanMove == false)
        { 
            inControlText.text = "In Control: Clone";
            basicMovementPlayer.canMove = false; 
            cloneCanMove = true;
        }

        for (int i = 3; i > 0; i--)
        {
            cooldownTimerText.text = "Cooldown Timer: " + i;
            yield return new WaitForSeconds(1);
        }
        
        cooldownTimerText.text = "Cooldown Timer: ";
        canSwitch = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Allow clone to pass through special walls.
        if (collision.gameObject.tag == "CloneWall")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
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
}
