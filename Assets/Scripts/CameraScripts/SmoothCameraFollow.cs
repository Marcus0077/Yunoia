using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothCameraFollow : MonoBehaviour
{
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

    // Get references and initialize variables when camera is spawned.
    private void Awake()
    {
        playerControls = new PlayerControls();

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

    // Change the main camera position depending on the position offset values.
    private void ChangeCameraPosition()
    {
        positionOffset = new Vector3(positionOffsetX, positionOffsetY, positionOffsetZ);

        Vector3 desiredPosition = positionOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);
        transform.position = smoothedPosition;
    }

    // Change the main camera rotation depending on the rotation offset values.
    private void ChangeCameraRotation()
    {
        rotationOffset = new Vector3(rotationOffsetX, rotationOffsetY, rotationOffsetZ);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset), rotationSmoothSpeed);
    }
    
    // Sets camera angle to a further and wider position and rotation. (experimental)
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
    
    // back angle camera follow
    public void BackAngleCamera()
    {
        positionOffsetX = -10f;
        positionOffsetY = 7f;
        positionOffsetZ = 6;

        rotationOffsetX = 25f;
        rotationOffsetY = 120f;
        rotationOffsetZ = 0f;
    }
    
    // setup camera bounds

    // Sets camera angle to a further and higher position and rotation. (experimental)
    public void HigherAngleCamera()
    {
        positionOffsetX = -15;
        positionOffsetY = 8f;
        positionOffsetZ = 0;

        rotationOffsetX = 15;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;
    }
    // Sets camera angle to a closer and lower position and rotation. (experimental)
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
