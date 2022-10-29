using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothCameraFollow : MonoBehaviour
{
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    
    public Transform target;
    PlayerControls playerControls;
    public InputAction exit;

    [Range(-20f, 20f)]
    public float offsetX;
    [Range(-20f, 20f)]
    public float offsetY;
    [Range(-20f, 20f)]
    public float offsetZ;
    
    [Range(-180f, 180f)]
    public float rotationOffsetX;
    [Range(-180f, 180f)]
    public float rotationOffsetY;
    [Range(-180f, 180f)]
    public float rotationOffsetZ;

    public Vector3 offset;
    public Vector3 rotationOffset;

    [Range(0.05f, 0.25f)]
    public float smoothSpeed;

    private void Awake()
    {
        playerControls = new PlayerControls();
        basicMovementPlayer = basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        target = basicMovementPlayer.playerRB.transform;
    }

    void Update()
    {
        if (exit.IsPressed())
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        offset = new Vector3(offsetX, offsetY, offsetZ);
        rotationOffset = new Vector3(rotationOffsetX, rotationOffsetY, rotationOffsetZ);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset), smoothSpeed * 10);
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void HigherAngleCamera()
    {
        offsetX = -12;
        offsetY = 4;
        offsetZ = 0;

        rotationOffsetX = 10;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;
    }
    public void RegularAngleCamera()
    {
        offsetX = -9;
        offsetY = 2.5f;
        offsetZ = 0;

        rotationOffsetX = 0;
        rotationOffsetY = 90;
        rotationOffsetZ = 0;
    }

    public void OnEnable()
    {
        exit = playerControls.Camera.Exit;
        exit.Enable();
    }

    public void OnDisable()
    {
        exit.Disable();
    }
}
