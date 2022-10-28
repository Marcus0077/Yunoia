using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovementClone : MonoBehaviour
{
    private TextMeshProUGUI dashCooldownUI;
    
    // Script references
    public BasicMovementPlayer basicMovementPlayer;
    public SmoothCameraFollow smoothCameraFollow;
    public SummonClone summonClone;
    public ExitClone exitClone;
    public CombatHandler combatHandler;
    public LevelSwitchManager levelSwitchManager;
    public AbilityPush cloneAbilityPush;

    // Player game object reference.
    public GameObject Player;
    
    // Input variables
    PlayerControls playerControls;
    public InputAction move;
    public InputAction jump;
    public InputAction switchPlaces;
    public InputAction dash;

    // Movement variables
    public Rigidbody cloneRB;
    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
    public float moveSpeed;
    public float maxSpeed;
    public bool cloneIsFrozen;
    public bool cloneCanMove;
    public float accelerationValue;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    public bool isGrounded;
    public float jumpTime;

    // Temp Object References
    public bool isOnTrigger2;
    public GameObject Wall_1;
    public GameObject Blocker2;
    public GameObject Blocker3;
    
    public Vector3 blocker3InPos;
    public Vector3 blocker3OutPos;

    // Clone version bool.
    public bool cloneRestored;

    // Minion Variables
    public int attachedMinionCount;
    float logFormulaCoefficient;
    float logFormulaModifier;
    
    // Dash Variables
    private float dashCooldown;
    private bool dashAccelerate;

    // Get references and initialize variables when clone is instantiated.
    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        
        basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        summonClone = FindObjectOfType<SummonClone>();
        exitClone = FindObjectOfType<ExitClone>();
        combatHandler = FindObjectOfType<CombatHandler>();
        
        levelSwitchManager = FindObjectOfType<LevelSwitchManager>();
        
        Physics.IgnoreCollision(this.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);

        Wall_1 = GameObject.FindWithTag("GoodWall1");
        Blocker2 = GameObject.FindWithTag("Blocker2");
        Blocker3 = GameObject.FindWithTag("Blocker3");
        
        blocker3InPos = Blocker3.transform.position;
        blocker3OutPos = new Vector3(blocker3InPos.x - 3f, blocker3InPos.y,
            blocker3InPos.z);

        smoothCameraFollow.target = cloneRB.transform;
        
        playerControls = new PlayerControls();
        
        moveSpeed = 4f;
        maxSpeed = 18f;
        jumpForce = 1.2f;
        accelerationValue = 1f;
        combatHandler.cloneHP = 3;

        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;

        dashCooldownUI = GameObject.FindGameObjectWithTag("Dash Cooldown").GetComponent<TextMeshProUGUI>();
        dashCooldownUI.text = "Dash Cooldown: Ready";
        combatHandler.healthText.text = "Clone Health: " + combatHandler.cloneHP + "/3";

        cloneRestored = true;
        cloneIsFrozen = false;
        cloneCanMove = true;
        dashAccelerate = false;
        isOnTrigger2 = false;
        
        if (levelSwitchManager.pushRestored == true)
        {
            cloneAbilityPush.restored = true;
        }
        else
        {
            cloneAbilityPush.restored = false;
        }
    }
    
    void FixedUpdate()
    {
        MoveClone();
    }

    private void Update()
    {
        SwitchPlaces();
        ActivateDash();
        CheckRestoredWalls();
    }
    
    void DecelerateDash()
    {
        if (accelerationValue > 1f)
        {
            accelerationValue -= 0.02f * accelerationValue;
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
    
    void ActivateDash()
    {
        if (dash.WasPressedThisFrame() && move.IsPressed() && dashCooldown <= 0 && !cloneIsFrozen)
        {
            dashAccelerate = true;
            
            dashCooldown = 3f;
        }
    }
    
    void DashClone()
    {
        if (dashAccelerate)
        {
            AccelerateDash();

            if (accelerationValue >= 5f)
            {
                dashAccelerate = false;
            }
        }
        else if (!dashAccelerate)
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

    // Applies gravity and movement velocity to player according to movement input.
    // Also gives option to jump.
    void MoveClone()
    {
        if (cloneCanMove == true)
        {
            if (cloneIsFrozen)
            {
                cloneRB.constraints = RigidbodyConstraints.FreezeRotation;
                cloneIsFrozen = false;
            }
            
            moveDirection = move.ReadValue<Vector2>() * moveSpeed / (1 + CalcMinionMoveChange());
        
            cloneRB.velocity = new Vector3(moveDirection.y * accelerationValue, cloneRB.velocity.y, -moveDirection.x * accelerationValue);
        
            LookClone();
            JumpClone();
        }
        else
        {
            cloneRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | 
                                   RigidbodyConstraints.FreezeRotation;
            cloneIsFrozen = true;
        }
        
        DashClone();

        cloneRB.AddForce(Physics.gravity * (1.5f + CalcMinionMoveChange()) * cloneRB.mass);
        
        if (cloneRB.velocity.magnitude > maxSpeed)
        {
            cloneRB.velocity = Vector3.ClampMagnitude(cloneRB.velocity, maxSpeed);
        }
    }

    float CalcMinionMoveChange()
    {
        return Mathf.Max(0, Mathf.Pow(attachedMinionCount * logFormulaCoefficient, logFormulaModifier));
    }

    public void AddMinion(int value)
    {
        attachedMinionCount += value;
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
        if (switchPlaces.WasPressedThisFrame())
        {
            if (basicMovementPlayer.playerCanMove == false)
            {
                basicMovementPlayer.playerCanMove = true;
                cloneCanMove = false;

                this.GetComponent<Grapple>().enabled = false;
                this.GetComponent<AbilityPush>().enabled = false;

                Player.GetComponent<Grapple>().enabled = true;
                Player.GetComponent<AbilityPush>().enabled = true;

                smoothCameraFollow.target = basicMovementPlayer.playerRB.transform;
            }
            else if (cloneCanMove == false)
            {
                basicMovementPlayer.playerCanMove = false;
                cloneCanMove = true;

                this.GetComponent<Grapple>().enabled = true;
                this.GetComponent<AbilityPush>().enabled = true;

                Player.GetComponent<Grapple>().enabled = false;
                Player.GetComponent<AbilityPush>().enabled = false;

                smoothCameraFollow.target = cloneRB.transform;
            }
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

        dash = playerControls.Movement.Dash;
        dash.Enable();
    }
    
    // Disable input action map controls.
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        switchPlaces.Disable();
        dash.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockerTrigger3"))
        {
            Blocker3.transform.position = blocker3OutPos;
        }

        if (other.CompareTag("BlockerTrigger2"))
        {
            isOnTrigger2 = true;
            
            Blocker2.GetComponent<MeshRenderer>().enabled = false;
            Blocker2.GetComponent<Collider>().enabled = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BlockerTrigger3"))
        {
            Blocker3.transform.position = blocker3InPos;
        }

        if (other.CompareTag("BlockerTrigger2"))
        {
            isOnTrigger2 = false;
            
            Blocker2.GetComponent<MeshRenderer>().enabled = true;
            Blocker2.GetComponent<Collider>().enabled = true;
        }
    }

    void CheckRestoredWalls()
    {
        Physics.IgnoreCollision(Wall_1.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
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
