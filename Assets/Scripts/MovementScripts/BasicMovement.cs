using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

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
    
    // Camera Variables
    private SmoothCameraFollow smoothCameraFollow;
    public string curCamState;
    public Animator stateDrivenCamAnimator;
    public int curRoom;

    // Sound Variables
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource runSoundOne;
    [SerializeField] private AudioSource runSoundTwo;

    private bool runSoundOneCanPlay = true;
    private bool runSoundTwoCanPlay = false;

    // Animation Variables
    [SerializeField] private Animator animator;

    GameObject minionDeath;
    
    // Get references and initialize variables when player spawns.
    void Awake()
    {
        playerControls = new PlayerControls();
        smoothCameraFollow = FindObjectOfType<SmoothCameraFollow>();
        stateDrivenCamAnimator = GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<Animator>();

        if (this.GameObject().CompareTag("Player"))
        {
            curRoom = 1;
        }
        else
        {
            curRoom = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().curRoom;
        }

        minionDeath = GameObject.FindWithTag("MinionDeath");
        minionDeath.SetActive(false);

        moveSpeed = 4f;
        maxSpeed = 18f;
        jumpForce = 8f;
        accelerationValue = 1f;
        dashCooldown = 0f;
        dashAccelerate = 0;
        
        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;
        
        canMove = true;
        isFrozen = false;
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        MovePlayer();

        SetWalkingAnimation();
        SetRunSound();
    }

    // Called each frame.
    private void Update()
    {
        JumpPlayer();
        ActivateDash();
    }

    // Plays the walking animation if this game object is moving, 
    // or plays the idle animation if this game object is not moving.
    void SetWalkingAnimation()
    {
        if (move.IsPressed() && canMove)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    // Plays the run sound(s) if this game object is moving
    // and is on the ground.
    void SetRunSound()
    {
        if (move.IsPressed() && runSoundOneCanPlay && isGrounded)
        {
            runSoundOne.Play();
            runSoundOneCanPlay = false;
            StartCoroutine(RunSoundOneDelay());
        }
        else if (move.IsPressed() && runSoundTwoCanPlay && isGrounded)
        {
            runSoundTwo.Play();
            runSoundTwoCanPlay = false;
            StartCoroutine(RunSoundTwoDelay());
        }
    }

    // Changes camera orientation depending on whether player or clone is in
    // control and what their current camera state it.
    public void CheckCameraState()
    {
        if (canMove)
        {
            if (curRoom == 1)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 1);
            }
            else if (curRoom == 2)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 2);
            }
            else if (curRoom == 3)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 3);
            }
            else if (curRoom == 4)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 4);
            }
            else if (curRoom == 5)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 5);
            }
            else if (curRoom == 6)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 6);
            }
            else if (curRoom == 7)
            {
                stateDrivenCamAnimator.SetInteger("roomNum", 7);
            }

            GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<CinemachineStateDrivenCamera>().Follow =
                this.transform;
        }
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
    // Applies dash if player uses dash input.
    void MovePlayer()
    {
        if (canMove == true)
        {
            if (isFrozen)
            {
                playerRB.constraints = RigidbodyConstraints.FreezeRotation;
                isFrozen = false;
            }
            moveDirection = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y-90, -Vector3.forward) * move.ReadValue<Vector2>().normalized * moveSpeed / (1 + CalcMinionMoveChange());
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
    
    // Calculates the movement change of this game object depending on how
    // many minions are attached to it.
    float CalcMinionMoveChange()
    {
        if(attachedMinionCount > 5)
        {
            minionDeath.SetActive(true);
        }
        return Mathf.Max(0,Mathf.Pow(attachedMinionCount * logFormulaCoefficient, logFormulaModifier));
    }

    // Adds a minion to the attached minion count of this game object.
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
            //jumpSound.Play();
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

    // If dash is on a cooldown, return the timer value.
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

    IEnumerator SetCameraBlend()
    {
        GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<CinemachineStateDrivenCamera>()
            .m_DefaultBlend.m_Time = 1f;

        yield return new WaitForSeconds(1f);
        
        GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<CinemachineStateDrivenCamera>()
            .m_DefaultBlend.m_Time = 0f;
    }
    
    // Changes camera position and rotation depending on where the player is on the level.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Camera1"))
        {
            curRoom = 1;
            stateDrivenCamAnimator.SetInteger("roomNum", 1);
        }
        if (other.CompareTag("Camera2"))
        {
            curRoom = 2;
            stateDrivenCamAnimator.SetInteger("roomNum", 2);
        }
        if (other.CompareTag("Camera3"))
        {
            curRoom = 3;
            stateDrivenCamAnimator.SetInteger("roomNum", 3);
        }
        if (other.CompareTag("Camera4"))
        {
            curRoom = 4;
            stateDrivenCamAnimator.SetInteger("roomNum", 4);
        }
        if (other.CompareTag("Camera5"))
        {
            curRoom = 5;
            stateDrivenCamAnimator.SetInteger("roomNum", 5);
        }
        if (other.CompareTag("Camera6"))
        {
            curRoom = 6;
            stateDrivenCamAnimator.SetInteger("roomNum", 6);
        }
        if (other.CompareTag("Camera7"))
        {
            curRoom = 7;
            stateDrivenCamAnimator.SetInteger("roomNum", 7);
        }
    }
    
    // Determines if player is on the ground or not using (4) raycasts.
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

        if ((Physics.Raycast(groundPointRight, Vector3.down, out hit, 0.3f, whatIsGround) || 
             Physics.Raycast(groundPointLeft, Vector3.down, out hit, 0.3f, whatIsGround) || 
             Physics.Raycast(groundPointTop, Vector3.down, out hit, 0.3f, whatIsGround) || 
             Physics.Raycast(groundPointBottom, Vector3.down, out hit, 0.3f, whatIsGround)))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Delays run sound one to account for footstep(s) speed.
    private IEnumerator RunSoundOneDelay()
    {
        yield return new WaitForSeconds(0.68f);
        
        runSoundTwoCanPlay = true;
    }

    // Delays run sound two to account for footstep(s) speed.
    private IEnumerator RunSoundTwoDelay()
    {
        yield return new WaitForSeconds(0.68f);

        runSoundOneCanPlay = true;
    }
}
