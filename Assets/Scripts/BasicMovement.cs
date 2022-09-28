using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    
    PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;

    private Vector2 moveDirection = Vector2.zero;
    public Rigidbody irisRB;

    public LayerMask whatIsGround;
    public Transform groundPoint;
    private bool isGrounded;

    private float jumpTime;

    void Awake()
    {
        playerControls = new PlayerControls();
        moveSpeed = 4.0f;
        jumpForce = 3.0f;
    }

    private void OnEnable()
    {
        move = playerControls.Movement.Move;
        move.Enable();

        jump = playerControls.Movement.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    
    void FixedUpdate()
    {
        moveDirection = move.ReadValue<Vector2>() * moveSpeed;
        irisRB.velocity = new Vector3(-moveDirection.y, irisRB.velocity.y, moveDirection.x);

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

        irisRB.AddForce(Physics.gravity * 1.5f * irisRB.mass);
    }
}
