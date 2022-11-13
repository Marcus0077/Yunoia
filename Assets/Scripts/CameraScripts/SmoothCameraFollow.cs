using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothCameraFollow : MonoBehaviour
{
    // Transform references
    public Transform target;
    
    // Input variables
    PlayerControls playerControls;
    public InputAction exit;

    // Position offset variables
    public Vector3 positionOffset;
    
    [Range(-20f, 20f)]
    public float positionOffsetX;
    [Range(-20f, 20f)]
    public float positionOffsetY;
    [Range(-20f, 20f)]
    public float positionOffsetZ;
    
    // Rotation offset variables
    public Vector3 rotationOffset;
    
    [Range(-180f, 180f)]
    public float rotationOffsetX;
    [Range(-180f, 180f)]
    public float rotationOffsetY;
    [Range(-180f, 180f)]
    public float rotationOffsetZ;

    // Camera smoothing variables
    [Range(0.001f, 0.1f)]
    public float positionSmoothSpeed;
    
    [Range(0.001f, 0.1f)]
    public float rotationSmoothSpeed;

    // Get references and initialize variables when camera is activated.
    private void Awake()
    {
        playerControls = new PlayerControls();
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        RegularAngleCamera();
    }

    // Called each frame.
    void Update()
    {
        if (exit.IsPressed())
        {
            Application.Quit();
        }
    }

    // Called between frames.
    void FixedUpdate()
    {
        ChangeCameraPosition();
        ChangeCameraRotation();
    }

    private void ChangeCameraPosition()
    {
        positionOffset = new Vector3(positionOffsetX, positionOffsetY, positionOffsetZ);

        Vector3 desiredPosition = target.position + positionOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);
        transform.position = smoothedPosition;
    }

    private void ChangeCameraRotation()
    {
        rotationOffset = new Vector3(rotationOffsetX, rotationOffsetY, rotationOffsetZ);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset), rotationSmoothSpeed);
    }
    
    public void WideAngleCamera()
    {
        rotationSmoothSpeed = 0.01f;
        
        positionOffsetX = -18;
        positionOffsetY = 6f;
        positionOffsetZ = 0;

        rotationOffsetX = 0;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;

        rotationSmoothSpeed = 0.025f;
    }

    // Sets camera angle to a further and higher position and rotation.
    public void HigherAngleCamera()
    {
        positionOffsetX = -15;
        positionOffsetY = 4.5f;
        positionOffsetZ = 0;

        rotationOffsetX = 10;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;
    }
    // Sets camera angle to a closer and lower position and rotation.
    public void RegularAngleCamera()
    {
        positionOffsetX = -9;
        positionOffsetY = 2.5f;
        positionOffsetZ = 0;

        rotationOffsetX = 0;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;
    }

    // Enable input action map controls.
    public void OnEnable()
    {
        exit = playerControls.Camera.Exit;
        exit.Enable();
    }

    // Disable input action map controls.
    public void OnDisable()
    {
        exit.Disable();
    }
}
