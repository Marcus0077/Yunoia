using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;

public class BasicMovement : MonoBehaviour, IAbility
{
    // Input variables
    public InputActionAsset playerControls;
    public InputAction move;
    public InputAction jump;
    public InputAction dash;
    
    // Script References
    public LimitedMovementCam limitedMovementCam;
    public Grapple grapple;
    
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

    private bool hasTransitioned;
    public Transform transitionPos;

    public bool canRotateCam;

    // Animation Variables
    [SerializeField] private Animator animator;
    private float animRunSpeed;
    private float animRunAccelerator;
    

    GameObject minionDeath;

    public GameObject winScreen;
    
    // Anger Room Variables.
    public bool inAngerRoom;
    public HideObstructions curAngerRoomPartialTrigger;
    public HideObstructions curAngerRoomFullTrigger;

    // Get references and initialize variables when player spawns.
    void Awake()
    {
        playerControls = GameManager.Instance.GetComponent<PlayerInput>().actions; // new PlayerControls();
        GameManager.Instance.abilities.Add(this);
        ResetRebind();
        move = playerControls["Move"];
        jump = playerControls["Jump"];
        dash = playerControls["Dash"];
        limitedMovementCam = FindObjectOfType<LimitedMovementCam>();
        stateDrivenCamAnimator = GameObject.FindGameObjectWithTag("StateDrivenCam").GetComponent<Animator>();

        if (this.GetComponentInChildren<Animator>() != null)
        {
            animator = this.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.Log("animator returned null value.");
        }

        if (GameObject.FindObjectOfType<LookAtCam>() != null)
        {
            if (GameManager.Instance.camTurn == null)
            {
                GameManager.Instance.camTurn = FindObjectsOfType<LookAtCam>()[0];
            }
        }

        if (this.GameObject().CompareTag("Player"))
        {
            if (winScreen != null)
            {
                winScreen.SetActive(false);
            }

            if (DataManager.gameData.checkpointed)
            {
                curRoom = DataManager.gameData.checkpointDatas[GameManager.Instance.currentLevel].room;
            }
            else
            {
                curRoom = 1;   
            }
        }
        else
        {
            curRoom = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().curRoom;

            if (limitedMovementCam != null)
            {
                limitedMovementCam.SetCurrentPlayer(this.gameObject);
            }

            if (GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().inAngerRoom)
            {
                inAngerRoom = true;

                if (GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().curAngerRoomFullTrigger 
                    != null)
                {
                    curAngerRoomFullTrigger = GameObject.FindGameObjectWithTag("Player").
                        GetComponent<BasicMovement>().curAngerRoomFullTrigger;
                    
                    curAngerRoomFullTrigger.isClone = true;
                }
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<BasicMovement>().curAngerRoomPartialTrigger 
                    != null)
                {
                    curAngerRoomPartialTrigger = GameObject.FindGameObjectWithTag("Player").
                        GetComponent<BasicMovement>().curAngerRoomPartialTrigger;
                    
                    curAngerRoomPartialTrigger.isClone = true;
                }
            }
        }

        if (GameObject.FindWithTag("MinionDeath") != null)
        {
            minionDeath = GameObject.FindWithTag("MinionDeath");
            minionDeath.SetActive(false);
        }

        moveSpeed = 5.5f;
        maxSpeed = 18f;
        jumpForce = 9f;
        accelerationValue = 1f;
        dashCooldown = 0f;
        dashAccelerate = 0;
        animRunSpeed = 0;
        animRunAccelerator = 0.075f;
        
        logFormulaCoefficient = .6f;
        logFormulaModifier = 2f;
        
        canMove = true;
        isFrozen = false;
        hasTransitioned = false;
        canRotateCam = true;
    }
    
    // Called between frames.
    void FixedUpdate()
    {
        Time.timeScale = 1.0f;
        
        MovePlayer();

        SetWalkingAnimation();
        SetRunSound();
    }

    // Called each frame.
    private void Update()
    {
        JumpPlayer();
        ActivateDash();
        SoundActivation();
    }

    void SoundActivation()
    {
        GetComponent<AudioListener>().enabled = canMove;
    }

    // Plays the walking animation if this game object is moving, 
    // or plays the idle animation if this game object is not moving.
    void SetWalkingAnimation()
    {
        if (move.IsPressed() && canMove)
        {
            if (animRunSpeed < Mathf.Abs(move.ReadValue<Vector2>().x) + Mathf.Abs(move.ReadValue<Vector2>().y))
            {
                animRunSpeed += animRunAccelerator;
            }
            else
            {
                animRunSpeed = Mathf.Abs(move.ReadValue<Vector2>().x) + Mathf.Abs(move.ReadValue<Vector2>().y);
            }

            animator.SetFloat("Speed", animRunSpeed);
        }
        else
        {
            if (animRunSpeed > 0)
            {
                animRunSpeed -= animRunAccelerator;
                
            }
            else
            {
                animRunSpeed = 0;
            }

            animator.SetFloat("Speed", animRunSpeed);
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
            stateDrivenCamAnimator.SetInteger("roomNum", curRoom);
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
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        
        if (canMove == true)
        {
            if (isFrozen)
            {
                playerRB.constraints = RigidbodyConstraints.FreezeRotation;
                isFrozen = false;
            }
            
            if (SceneManager.GetActiveScene().name == "VSDenial")
            {
                moveDirection = Quaternion.AngleAxis(0, Vector3.right) * move.ReadValue<Vector2>().normalized * moveSpeed / (1 + CalcMinionMoveChange());
                playerRB.velocity = new Vector3(moveDirection.y * accelerationValue, playerRB.velocity.y, -moveDirection.x * accelerationValue);
            }
            else if (grapple != null)
            {
                if(GameManager.Instance.camTurn != null && canRotateCam)
                {
                    moveDirection = Quaternion.AngleAxis(FindObjectsOfType<LookAtCam>()[0].lookRotation.eulerAngles.y - 90, -Vector3.forward) * move.ReadValue<Vector2>().normalized * moveSpeed / (1 + CalcMinionMoveChange());
                }
                else
                {
                    moveDirection = Quaternion.AngleAxis(-90, -Vector3.forward) * move.ReadValue<Vector2>().normalized * moveSpeed / (1 + CalcMinionMoveChange());
                }
                
                

                if (grapple.grappleActive && !isGrounded)
                {
                    playerRB.velocity = new Vector3(0.0f, playerRB.velocity.y, -moveDirection.x * accelerationValue);
                }
                else
                {
                    playerRB.velocity = new Vector3(moveDirection.y * accelerationValue, playerRB.velocity.y, -moveDirection.x * accelerationValue);
                }
            }
        
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
        if(attachedMinionCount > 5 && !hasTransitioned && this.GameObject().CompareTag("Player"))
        {
            StartCoroutine(TransitionToWinScreen());
        }
        
        return Mathf.Max(0,Mathf.Pow(attachedMinionCount * logFormulaCoefficient, logFormulaModifier));
    }

    IEnumerator TransitionToWinScreen()
    {
        hasTransitioned = true;
        
        GameObject.FindObjectOfType<PauseMenu>().DisableInput();
        
        if (GameObject.FindObjectOfType<FadeBlack>() != null)
        {
            GameObject.FindObjectOfType<FadeBlack>().FadeToBlack(1.5f);
            yield return new WaitForSeconds(1.5f);

            winScreen.SetActive(true);

            foreach (GameObject Minion in GameObject.FindGameObjectsWithTag("Minion"))
            {
                Destroy(Minion);
            }
            
            attachedMinionCount = 0;
        }
    }

    public void ClickedContinue()
    {
        StartCoroutine(TransitionToEres());
    }
    
    IEnumerator TransitionToEres()
    {
        winScreen.SetActive(false);
        
        curRoom = 9;
        stateDrivenCamAnimator.SetInteger("roomNum", 9);
        this.transform.position = transitionPos.position;
        
        yield return new WaitForSeconds(1.5f);
        
        GameObject.FindObjectOfType<PauseMenu>().EnableInput();
        GameObject.FindObjectOfType<FadeBlack>().FadeToTransparent(3f);
        
    }
    
    // Adds a minion to the attached minion count of this game object.
    public void AddMinion(int value)
    {
        attachedMinionCount += value;
        GameObject.FindWithTag("Player").GetComponent<Rumbler>().RumbleConstant(1, 2, 0.25f);
    }

    // Applies jump force to player if they press the jump button and
    // if they are on the ground.
    // Added condition for currently using the grapple (Marcus)
    void JumpPlayer()
    {
        IsGrounded();

        if (jump.WasPressedThisFrame())
        {
            if (isGrounded && canMove)
            {
                playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                animator.SetBool("Jump", true);
                //jumpSound.Play();
            }
            else if (grapple != null && grapple.grappleActive)
            {
                playerRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                //jumpSound.Play();

                grapple.DestroyHook();
            }
            
        }
    }

    // Initiates dash sequence.
    void ActivateDash()
    {
        if (dash.WasPressedThisFrame() && move.IsPressed() && dashCooldown <= 0 && !isFrozen)
        {
            dashSound.Play();
            animator.SetBool("Dash", true);

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
        move.Enable();
        jump.Enable();
        dash.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        //move.Disable();
        //jump.Disable();
        //dash.Disable();
    }

    // Changes active camera depending on where the player is on the level.
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Teleport>() || other.GetComponent<TeleToBeaver>())
        {
            StartCoroutine(delayedSwitchCamera(other));
        }
        else if (this.CompareTag("Player") || (other.GetComponent<YeetTheClone>() == null && this.CompareTag("Clone")))
        {
            switchCamera(other);
        }
    }

    private void switchCamera(Collider other)
    {
        if (other.tag.ToString().Trim(other.tag.ToString()[other.tag.ToString().Length - 1]).CompareTo("Camera") ==
            0)
        {
            curRoom = other.tag.ToCharArray()[other.tag.ToCharArray().Length - 1] - 48;

            limitedMovementCam.GetCurrentCameraData(curRoom);
            limitedMovementCam.SetCurrentPlayer(this.gameObject);
        }
        else if (other.CompareTag("Camera10"))
        {
            curRoom = 10;

            limitedMovementCam.GetCurrentCameraData(curRoom);
            limitedMovementCam.SetCurrentPlayer(this.gameObject);
        }
    }
    
    private IEnumerator delayedSwitchCamera(Collider other)
    {
        yield return new WaitForSeconds(1f);
        
        switchCamera(other);
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

    public void ResetRebind()
    {
        playerControls = GameManager.Instance.GetInputs(playerControls);
    }

    private void OnDestroy()
    {
        GameManager.Instance.abilities.Remove(this);
    }
}
