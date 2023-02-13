using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SmoothCameraFollow : MonoBehaviour
{
    // Input variables
    PlayerControls playerControls;
    public InputAction exit;

    private Transform Obstruction;

    private void Start()
    {
        playerControls = new PlayerControls();
        
        exit = playerControls.Camera.Exit;
        exit.Enable();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, this.GetComponent<CinemachineVirtualCamera>().Follow.transform.position - transform.position, out hit, 6f))
        {
            if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Clone")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        else if (Obstruction != null)
        {
            Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.On;

        }
    }

    // Disable input action map controls.
    public void OnDisable()
    {
        exit.Disable();
    }
}
