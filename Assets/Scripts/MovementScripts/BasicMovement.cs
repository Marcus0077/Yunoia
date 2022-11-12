using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovement : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction move;
    public InputAction jump;
    public InputAction dash;
    
    // Movement variables
    public Rigidbody playerRB;
    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
    public float moveSpeed;
    public float maxSpeed;
    public float accelerationValue;
    public bool canMove;
    public bool isFrozen;
    public float moveStartTimeDivider;
    
    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    public bool isGrounded;

    // Minion Variables
    public int attachedMinionCount;
    float logFormulaCoefficient;
    float logFormulaModifier;

    // Dash Variables
    private float dashCooldown;
    private int dashAccelerate;
    
    // Camera Reference
    private SmoothCameraFollow smoothCameraFollow;

    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource jumpSound;

    [SerializeField] private Animator animator;
    
    // Text UI References


    // Get references and initialize variables when player spawns.
    void Awake()
    {
        playerControls = new PlayerControls();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();

        animator = GetComponent<Animator>();

        moveSpeed = 4f;
        maxSpeed = 18f;
        jumpForce = 8f;
        accelerationValue = 1f;
        dashCooldown = 0f;
        
        canMove = true;
        isFrozen = false;
        dashAccelerate = 0;

        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        MovePlayer();
        
        if (!move.IsPressed())
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }
    }

    // Called each frame.
    private void Update()
    {
        JumpPlayer();
        ActivateDash();
    }

    // Decelerates player to normal speed after dashing.
    void DecelerateDash()
    {
        if (accelerationValue > 1f)
        {
            accelerationValue -= 0.02f * accelerationValue;
        }
        else
        {
            dashAccelerate = 0;
        }
    }
    
    // Accelerates player to a set speed while dashing.
    void AccelerateDash()
    {
        if (accelerationValue < 2.5f)
        {
            accelerationValue = 2.5f;
        }
        else if (accelerationValue < 5f)
        {
            accelerationValue += 0.05f * accelerationValue;
        }
    }

    // Applies gravity and movement velocity to player according to movement input.
    // Freezes player or clone depending on which one is currently active.
    // Applies look rotation depending on player's movement direction.
    // Gives options to jump and dash.
    void MovePlayer()
    {
        if (canMove == true)
        {
            if (isFrozen)
            {
                playerRB.constraints = RigidbodyConstraints.FreezeRotation;
                isFrozen = false;
            }
            
            moveDirection = move.ReadValue<Vector2>() * moveSpeed / (1 + CalcMinionMoveChange());
            playerRB.velocity = new Vector3(moveDirection.y * accelerationValue, playerRB.velocity.y, -moveDirection.x * accelerationValue);
        
            LookPlayer();
        }
        else
        {
            playerRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                                   RigidbodyConstraints.FreezeRotation;
            isFrozen = true;
        }
        
        DashPlayer();

        playerRB.AddForce(Physics.gravity * (1.5f + CalcMinionMoveChange()) * playerRB.mass);
        
        if (playerRB.velocity.magnitude > maxSpeed)
        {
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxSpeed);
        }
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

        if (jump.WasPressedThisFrame() && isGrounded && canMove)
        {
            playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            jumpSound.Play();
        }
    }

    // Initiates dash sequence.
    void ActivateDash()
    {
        if (dash.WasPressedThisFrame() && move.IsPressed() && dashCooldown <= 0 && !isFrozen)
        {
            dashSound.Play();

            dashAccelerate = 1;
            dashCooldown = 3f;
        }
    }
    
    // Goes through dash sequence, accelerating then decelerating the player, 
    // all while applying a 3 second cooldown, which allows dash to be activated
    // again once the cooldown reaches 0.
    void DashPlayer()
    {
        if (dashAccelerate == 1)
        {
            AccelerateDash();

            if (accelerationValue >= 5f)
            {
                dashAccelerate = 2;
            }
        }
        else if (dashAccelerate == 2)
        {
            DecelerateDash();
        }

        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    public float CooldownRemaining()
    {
        if (dashCooldown > 0)
        {
            return dashCooldown;
        }
        else
        {
            return -1;
        }
    }

    // Player looks in the direction they are moving, smoothly according to the angles
    // between last look rotation and current movement direction.
    void LookPlayer()
    {
        if (move.IsPressed())
        {
            float playerAngle = (float) (Math.Atan2(moveDirection.x, moveDirection.y) * 180) / (float) Math.PI;
            
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, playerAngle, 0), 
                Time.deltaTime / moveStartTimeDivider);

            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            
        }
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();

        dash = playerControls.Movement.Dash;
        dash.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
    }
    
    // Changes camera position and rotation depending on where the player is on the level.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HigherView"))
        {
            smoothCameraFollow.HigherAngleCamera();
        }

        if (other.CompareTag("RegularView"))
        {
            smoothCameraFollow.RegularAngleCamera();
        }
    }
    
    // Determines if player is on the ground or not.
    private void IsGrounded()
    {
        RaycastHit hit;

        Vector3 groundPointRight = new Vector3(groundPoint.position.x, groundPoint.position.y + 0.2f, 
            groundPoint.position.z - 0.4f);
        Vector3 groundPointLeft = new Vector3(groundPoint.position.x, groundPoint.position.y + 0.2f, 
            groundPoint.position.z + 0.4f);
        Vector3 groundPointTop = new Vector3(groundPoint.position.x + 0.4f, groundPoint.position.y + 0.2f, 
            groundPoint.position.z);
        Vector3 groundPointBottom = new Vector3(groundPoint.position.x - 0.4f, groundPoint.position.y + 0.2f, 
            groundPoint.position.z);
        
        
        Debug.DrawRay(groundPointRight, Vector3.down * 0.3f, Color.green, 0.5f);
        Debug.DrawRay(groundPointLeft, Vector3.down * 0.3f, Color.green, 0.5f);
        Debug.DrawRay(groundPointTop, Vector3.down * 0.3f, Color.green, 0.5f);
        Debug.DrawRay(groundPointBottom, Vector3.down * 0.3f, Color.green, 0.5f);
        
        
        if ((Physics.Raycast(groundPointRight, Vector3.down, out hit, 0.5f, whatIsGround) || 
            Physics.Raycast(groundPointLeft, Vector3.down, out hit, 0.5f, whatIsGround) || 
            Physics.Raycast(groundPointTop, Vector3.down, out hit, 0.5f, whatIsGround) || 
            Physics.Raycast(groundPointBottom, Vector3.down, out hit, 0.5f, whatIsGround)))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
