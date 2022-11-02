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
    
    // Text UI References
    private TextMeshProUGUI dashCooldownUI;


    // Get references and initialize variables when player spawns.
    void Awake()
    {
        playerControls = new PlayerControls();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();

        dashCooldownUI = GameObject.FindGameObjectWithTag("Dash Cooldown").GetComponent<TextMeshProUGUI>();
        dashCooldownUI.text = "Dash Cooldown: Ready";
        
        moveSpeed = 4f;
        maxSpeed = 18f;
        jumpForce = 1.2f;
        accelerationValue = 1f;
        dashCooldown = 0f;

        canMove = true;
        isFrozen = false;
        dashAccelerate = 0;

        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {
        ActivateDash();
    }

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
    // Also gives option to jump.
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
            JumpPlayer();
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

        if (jump.IsPressed() && isGrounded)
        {
            playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }

    void ActivateDash()
    {
        if (dash.WasPressedThisFrame() && move.IsPressed() && dashCooldown <= 0 && !isFrozen)
        {
            dashAccelerate = 1;
            
            dashCooldown = 3f;
        }
    }
    
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
            dashCooldownUI.text = "Dash Cooldown: " + Math.Round(dashCooldown, 2);
        }
        else
        {
            dashCooldownUI.text = "Dash Cooldown: Ready";
        }
    }

    // Player looks in the direction they are moving.
    void LookPlayer()
    {
        if (move.IsPressed())
        {
            //transform.forward = new Vector3(moveDirection.x, 0f, moveDirection.y);

            float playerAngle = (float) (Math.Atan2(moveDirection.x, moveDirection.y) * 180) / (float) Math.PI;
            
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, playerAngle, 0), 
                Time.deltaTime / moveStartTimeDivider);
            
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
