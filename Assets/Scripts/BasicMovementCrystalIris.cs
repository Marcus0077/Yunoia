using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BasicMovementCrystalIris : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction switchPlaces;

    // Movement variables
    public Rigidbody irisRB;
    private Vector2 moveDirection = Vector2.zero;
    public float moveSpeed;

    // Jump variables
    public LayerMask whatIsGround;
    public Transform groundPoint;
    public float jumpForce;
    private bool isGrounded;
    private float jumpTime;

    private bool crystalIsFrozen;
    private bool crystalCanMove;
    
    private bool canSwitch;
    
    private BasicMovementIris basicMovementIris;
    
    public TextMeshProUGUI timerText;

    void Awake()
    {
        basicMovementIris = FindObjectOfType<BasicMovementIris>();
        playerControls = new PlayerControls();
        moveSpeed = 4.0f;
        jumpForce = 3.0f;

        crystalIsFrozen = false;
        crystalCanMove = true;

        canSwitch = true;

        
        timerText = GameObject.FindGameObjectWithTag("Timer Text").GetComponent<TextMeshProUGUI>();
        timerText.text = "";
    }
    
    void FixedUpdate()
    {
        if (crystalCanMove == true)
        {
            if (crystalIsFrozen)
            {
                irisRB.constraints = RigidbodyConstraints.FreezeRotation;
                crystalIsFrozen = false;
            }
            
            MoveIris();
        }
        else
        {
            irisRB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            crystalIsFrozen = true;
        }

        if (switchPlaces.IsPressed() && canSwitch)
        {
            StartCoroutine(SwitchPlaces());
        }
    }

    void MoveIris()
    {
        moveDirection = move.ReadValue<Vector2>() * moveSpeed;
        irisRB.velocity = new Vector3(-moveDirection.y, irisRB.velocity.y, moveDirection.x);
        
        JumpIris();
        
        irisRB.AddForce(Physics.gravity * 1.5f * irisRB.mass); 
    }

    void JumpIris()
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
            irisRB.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }

    private IEnumerator SwitchPlaces()
    {
        canSwitch = false;
        
        if (basicMovementIris.canMove == false)
        {
            basicMovementIris.canMove = true;
            crystalCanMove = false;
        }
        else if (crystalCanMove == false)
        { 
            basicMovementIris.canMove = false; 
            crystalCanMove = true;
        }

        for (int i = 3; i > 0; i--)
        {
            timerText.text = "Timer: " + i;
            yield return new WaitForSeconds(1);
        }
        
        timerText.text = "";
        canSwitch = true;
    }

    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();

        switchPlaces = playerControls.SummonClone.SwitchPlaces;
        switchPlaces.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        switchPlaces.Disable();
    }
}
