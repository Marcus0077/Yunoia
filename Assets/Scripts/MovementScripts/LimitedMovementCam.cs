using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedMovementCam : MonoBehaviour
{
    // Input Variables
    PlayerControls playerControls;
    public InputAction camMove;
    public InputAction playerMove;
    
    // Camera Follow Sphere RigidBody
    public Rigidbody camFollowSphere;

    // Movement Variables
    public Vector2 moveDirection;
    public float accelerationValue;
    public float moveSpeed;
    public float returnSpeed;

    public Vector3 playerPos;

    public float returnToPlayerTimer;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);

        returnSpeed = 20f;
        returnToPlayerTimer = 0f;
        accelerationValue = 1f;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        if (camMove.IsPressed() && !playerMove.IsPressed())
        {
            returnToPlayerTimer = 1.5f;

            moveDirection = (Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y - 90, -Vector3.forward) *
                                 camMove.ReadValue<Vector2>().normalized * moveSpeed);

            camFollowSphere.velocity = new Vector3(moveDirection.y * accelerationValue,
                    -moveDirection.x * accelerationValue, camFollowSphere.velocity.z);
        }
        else if (!camMove.IsPressed() && !playerMove.IsPressed() && returnToPlayerTimer > 0)
        {
            camFollowSphere.velocity = Vector2.zero;
            returnToPlayerTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
        }
    }
    
    // Enable input action map controls.
    private void OnEnable()
    {
        playerMove = playerControls.Movement.Move;
        playerMove.Enable();
        
        camMove = playerControls.Movement.CamMove;
        camMove.Enable();
    }

    // Disable input action map controls.
    private void OnDisable()
    {
        playerMove.Disable();
        camMove.Disable();
    }
}
