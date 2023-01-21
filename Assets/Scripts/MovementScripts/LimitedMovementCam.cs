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
    
    // Camera Follow Sphere(s) RigidBody
    public Rigidbody camFollowSphere;
    public Rigidbody seekerSphere;

    // Movement Variables
    public Vector2 moveDirection;
    public float baseAccelerationValue;
    public float accelerationValueX;
    public float accelerationValueY;
    public float moveSpeed;
    public float returnSpeed;

    public Vector3 playerPos;

    public float returnToPlayerTimer;

    public CinemachineVirtualCamera curCamera;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
        
        seekerSphere.position = camFollowSphere.position;

        returnSpeed = 20f;
        returnToPlayerTimer = 0f;
        baseAccelerationValue = 1f;
        accelerationValueX = 1f;
        accelerationValueY = 1f;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        if (camMove.IsPressed() && !playerMove.IsPressed())
        {
            Debug.Log(camMove.ReadValue<Vector2>().x);
            //Debug.Log(camMove.ReadValue<Vector2>().y);
            
            
            returnToPlayerTimer = 1.5f;

            moveDirection = (Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y - 90, -Vector3.forward) *
                                 camMove.ReadValue<Vector2>().normalized * moveSpeed);

            camFollowSphere.velocity = new Vector3(moveDirection.y * accelerationValueY,
                     -moveDirection.x * accelerationValueX, camFollowSphere.velocity.z);
            
            seekerSphere.velocity = new Vector3(moveDirection.y * accelerationValueY,
                -moveDirection.x * accelerationValueX, camFollowSphere.velocity.z);
            
            // IF sphere moves to where camera can no longer go, stop sphere on that axis.
            
            // if ()
            // {
            //     accelerationValueY = 0f;
            // }
            // else if ()
            // {
            //     accelerationValueY = 1f;
            // }
        }
        else if (!camMove.IsPressed() && !playerMove.IsPressed() && returnToPlayerTimer > 0)
        {
            camFollowSphere.velocity = Vector2.zero;
            seekerSphere.velocity = Vector3.zero;
            
            returnToPlayerTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, returnSpeed * Time.deltaTime);
            seekerSphere.transform.position = Vector3.MoveTowards(seekerSphere.transform.position, playerPos, returnSpeed * Time.deltaTime);
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
