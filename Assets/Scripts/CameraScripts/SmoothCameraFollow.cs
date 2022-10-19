using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    // Script references
    private BasicMovementPlayer basicMovementPlayer;
    
    public Transform target;

    [Range(-20f, 20f)]
    public float offsetX;
    [Range(-20f, 20f)]
    public float offsetY;
    [Range(-20f, 20f)]
    public float offsetZ;

    public Vector3 offset;

    [Range(0.05f, 0.25f)]
    public float smoothSpeed;

    private void Awake()
    {
        basicMovementPlayer = basicMovementPlayer = FindObjectOfType<BasicMovementPlayer>();
        target = basicMovementPlayer.playerRB.transform;
    }

    void FixedUpdate()
    {
        offset = new Vector3(offsetX, offsetY, offsetZ);
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
